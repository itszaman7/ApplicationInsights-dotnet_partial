namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http.Headers;
    using Microsoft.ApplicationInsights.Common;

    internal static class HttpHeadersUtilities
    {
        }

        internal static string GetHeaderKeyValue(HttpHeaders headers, string headerName, string keyName)
        {
            IEnumerable<string> headerValues = GetHeaderValues(headers, headerName);
            return HeadersUtilities.GetHeaderKeyValue(headerValues, keyName);
        }
        }

        internal static bool ContainsRequestContextKeyValue(HttpHeaders headers, string keyName)
        {
            return ContainsHeaderKeyValue(headers, RequestResponseHeaders.RequestContextHeader, keyName);
        }

        internal static void SetRequestContextKeyValue(HttpHeaders headers, string keyName, string keyValue)
        {
            SetHeaderKeyValue(headers, RequestResponseHeaders.RequestContextHeader, keyName, keyValue);
        }

        internal static void SetHeaderKeyValue(HttpHeaders headers, string headerName, string keyName, string keyValue)
        {
            if (headers == null)
            {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
