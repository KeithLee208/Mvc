// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;

namespace Microsoft.AspNet.Mvc.ModelBinding
{
    public class FormValueProviderFactory : IValueProviderFactory
    {
        private const string FormEncodedContentType = "application/x-www-form-urlencoded";

        public async Task<IValueProvider> GetValueProviderAsync(RequestContext requestContext)
        {
            var request = requestContext.HttpContext.Request;
            
            if (IsSupportedContentType(request))
            {
                var queryCollection = await request.GetFormAsync();
                var culture = GetCultureInfo(request);
                return new ReadableStringCollectionValueProvider(queryCollection, culture);
            }

            return null;
        }

        private bool IsSupportedContentType(HttpRequest request)
        {
            var contentType = request.GetContentType();
            return contentType != null &&
                   string.Equals(contentType.ContentType, FormEncodedContentType, StringComparison.OrdinalIgnoreCase);
        }

        private static CultureInfo GetCultureInfo(HttpRequest request)
        {
            // TODO: Tracked via https://github.com/aspnet/HttpAbstractions/issues/10. Determine what's the right way to 
            // map Accept-Language to culture.
            return CultureInfo.CurrentCulture;
        }
    }
}
