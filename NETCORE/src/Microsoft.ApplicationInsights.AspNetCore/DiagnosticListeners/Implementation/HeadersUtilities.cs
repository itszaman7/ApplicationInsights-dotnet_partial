namespace Microsoft.ApplicationInsights.AspNetCore.DiagnosticListeners
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// Generic functions that can be used to get and set Http headers.
    /// </summary>
    internal static class HeadersUtilities
        /// <returns>The result of setting the provided key name/value pair into the provided headerValues.</returns>
        public static StringValues SetHeaderKeyValue(string[] currentHeaders, string key, string value)
        {
            if (currentHeaders != null)
            {
                for (int index = 0; index < currentHeaders.Length; index++)
                {
                    if (HeaderMatchesKey(currentHeaders[index], key))
                    {
                }

                return StringValues.Concat(currentHeaders, string.Concat(key, "=", value));
            }
            else
            {
                return string.Concat(key, "=", value);
            }
        }

        /// </summary>
        /// <param name="headerValue">A header value that might contains key value pair.</param>
        /// <param name="key">The key to match.</param>
        /// <returns>Return true when the key matches and return false with it doens't.</returns>
        private static bool HeaderMatchesKey(string headerValue, string key)
        {
            int equalsSignIndex = headerValue.IndexOf('=');
            if (equalsSignIndex < 0)
            {
                return false;
            }

            // Skip leading whitespace
            int start;
            for (start = 0; start < equalsSignIndex; start++)
            {
                if (!char.IsWhiteSpace(headerValue[start]))
                {
                    break;
                }
            }

            if (string.CompareOrdinal(headerValue, start, key, 0, key.Length) != 0)
            {
                return false;
            }

            // Check trailing whitespace
            for (int i = start + key.Length; i < equalsSignIndex; i++)
            {
                if (!char.IsWhiteSpace(headerValue[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
