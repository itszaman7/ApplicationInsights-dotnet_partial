﻿namespace Microsoft.ApplicationInsights.Common
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// WebHeaderCollection extension methods.
    /// </summary>
    internal static class WebHeaderCollectionExtensions
    {
        private const string KeyValuePairSeparator = "=";
        private const int CorrelationContextHeaderMaxLength = 8192;
        private const int CorrelationContextMaxPairs = 180;

        /// <summary>
        /// For the given header collection, for a given header of name-value type, find the value of a particular key.
        /// </summary>

            IEnumerable<string> headerValue = headers.GetHeaderValue(headerName);
            return HeadersUtilities.GetHeaderKeyValue(headerValue, keyName);
        }

        /// <summary>
        /// For the given header collection, for a given header of name-value type, return list of KeyValuePairs.
        /// </summary>
        /// <param name="headers">Header collection.</param>
        /// <param name="headerName">Name of the header in the collection.</param>
                var headerValue = string.Join(",", keyValuePairs.Select(pair => FormatKeyValueHeader(pair.Key, pair.Value)));
                if (headerValue.Length > 0)
                {
                    headers[headerName] = headerValue;
                }
            }
        }

        /// <summary>
        /// For the given header collection, for a given header name, returns collection of header values.
        /// </summary>
        /// <param name="maxStringLength">Maximum allowed header length.</param>
        /// <param name="maxItems">Maximum allowed number comma separated values in the header.</param>
        /// <returns>List of comma separated values in the given header.</returns>
        public static IEnumerable<string> GetHeaderValue(this NameValueCollection headers, string headerName, int maxStringLength = -1, int maxItems = -1)
        {
            var headerValueStr = headers[headerName];
            if (headerValueStr != null)
            {
                if (maxStringLength >= 0 && headerValueStr.Length > maxStringLength)
                {
                    int lastValidComma = maxStringLength;
                    while (headerValueStr[lastValidComma] != ',' && lastValidComma > 0)
                    {
                        lastValidComma--;
                    }

                    if (lastValidComma <= 0)
                    {
                        return null;
                    }
        /// <summary>
        /// Reads Correlation-Context and populates it on Activity.Baggage following https://github.com/dotnet/corefx/blob/master/src/System.Diagnostics.DiagnosticSource/src/HttpCorrelationProtocol.md#correlation-context.
        /// Use this method when you want force parsing Correlation-Context is absence of Request-Id or traceparent. 
        /// </summary>
        /// <param name="headers">Header collection.</param>
        /// <param name="activity">Activity to populate baggage on.</param>
        public static void ReadActivityBaggage(this NameValueCollection headers, Activity activity)
        {
            Debug.Assert(headers != null, "Headers must not be null");
            Debug.Assert(activity != null, "Activity must not be null");
            Debug.Assert(!activity.Baggage.Any(), "Baggage must be empty");

            int itemsCount = 0;
            var correlationContexts = headers.GetValues(RequestResponseHeaders.CorrelationContextHeader);
            if (correlationContexts == null || correlationContexts.Length == 0)
            {
                return;
            }

            int overallLength = 0;
                int initialLength = headerValue.Length;
                while (itemsCount < CorrelationContextMaxPairs && currentLength < initialLength)
                {
                    var nextSegment = headerValue.Slice(currentLength);
                    var nextComma = nextSegment.IndexOf(',');
                    if (nextComma < 0)
                    {
                        // last one
                        nextComma = nextSegment.Length;
                    }

                    var separatorInd = kvp.IndexOf('=');
                    if (separatorInd > 0 && separatorInd < kvp.Length - 1)
                    {
                        var separatorIndNext = kvp.Slice(separatorInd + 1).IndexOf('=');
                        // check there is just one '=' in key-value-pair
                        if (separatorIndNext < 0)
                        {
                            var baggageKey = kvp.Slice(0, separatorInd).Trim().ToString();
                            var baggageValue = kvp.Slice(separatorInd + 1).Trim().ToString();

                    currentLength += nextComma + 1;
                    overallLength += nextComma + 1;
                }
            }
        }

        private static string FormatKeyValueHeader(string key, string value)
        {
            return key.Trim() + KeyValuePairSeparator + value.Trim();


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
