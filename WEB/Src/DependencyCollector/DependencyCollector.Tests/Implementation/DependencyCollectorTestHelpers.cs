namespace Microsoft.ApplicationInsights.Tests
{
    using System.Net;
        internal static string GetCookieValueFromWebRequest(HttpWebRequest webRequest, string cookieKey)
        {
            {
                CookieCollection collection = webRequest.CookieContainer.GetCookies(webRequest.RequestUri);
                string cookie = collection[cookieKey].ToString();
                return cookie;
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
