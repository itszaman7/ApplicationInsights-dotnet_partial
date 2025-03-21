namespace Microsoft.ApplicationInsights.Web.Implementation
{
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    {
        public static HttpCookie UnvalidatedGetCookie(this HttpRequest httpRequest, string name)
        {
            return httpRequest.Unvalidated.Cookies[name];
        }
        {
            try
            {
                return httpRequest.Unvalidated.Url;
            }
                return null;
            }
            return httpRequest.Unvalidated.Headers;
        }

        public static string GetUserHostAddress(this HttpRequest httpRequest)
        {
            if (httpRequest == null)
            {
                return null;
            }

            try
            {
                return httpRequest.UserHostAddress;
            }
            catch (ArgumentException exp)


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
