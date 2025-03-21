namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Endpoints
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.ConfigString;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// This class encapsulates parsing a connection string and returning an Endpoint's URI.
    /// </summary>
    internal class EndpointProvider : IEndpointProvider
    {
        /// <summary>
        /// Maximum allowed length connection string.
        /// </summary>
        /// <remarks>
        /// Currently 8 accepted keywords (~200 characters).
        /// Assuming 200 characters per value (~1600 characters). 
        /// Total theoretical max length = (1600 + 200) = 1800.
        /// Setting an over-exaggerated max length to protect against malicious injections (2^12 = 4096).
        /// </remarks>
        internal const int ConnectionStringMaxLength = 4096;

        private static readonly char[] TrimPeriod = new char[] { '.' };

        private string connectionString;
        private IDictionary<string, string> connectionStringParsed = new Dictionary<string, string>(0);

        /// <summary>
        /// Gets or sets the connection string. 
                this.connectionStringParsed = ParseConnectionString(value);
            }
        }

        /// <summary>
        /// Will evaluate connection string and return the requested endpoint.
        /// </summary>
        /// <param name="endpointName">Specify which endpoint you want.</param>
        /// <returns>Returns a <see cref="Uri" /> for the requested endpoint.</returns>
        public Uri GetEndpoint(EndpointName endpointName)
                    else
                    {
                        throw new ArgumentException("Connection String Invalid: The value for EndpointSuffix is invalid.");
                    }
                }
                else
                {
                    return new Uri(endpointMeta.Default);
                }
            }
        /// <summary>
        /// Will evaluate connection string and return the requested instrumentation key.
        /// </summary>
        /// <returns>Returns the instrumentation key from the connection string.</returns>
        public string GetInstrumentationKey()
        {
            if (this.connectionStringParsed.TryGetValue("InstrumentationKey", out string value))
            {
                return value;
            }
            else
            {
                throw new ArgumentException("Connection String Invalid: InstrumentationKey is required.");
            }
        }

        /// <summary>
        /// Parse a connection string and return a Dictionary.
        /// </summary>
        /// <remarks>Example: "key1=value1;key2=value2;key3=value3".</remarks>
        /// <returns>A dictionary parsed from the input connection string.</returns>
        internal static IDictionary<string, string> ParseConnectionString(string connectionString)
        {
            try
            {
                return ConfigStringParser.Parse(connectionString);
            }
        internal static bool TryBuildUri(string prefix, string suffix, out Uri uri, string location = null)
        {
            // Location and Host are user input fields and need to be checked for extra periods.

            if (location != null)
            {
                location = location.Trim().TrimEnd(TrimPeriod);

                // Location names are expected to match Azure region names. No special characters allowed.
                if (!location.All(x => char.IsLetterOrDigit(x)))
                string.IsNullOrEmpty(location) ? string.Empty : (location + "."),
                prefix,
                ".",
                suffix.Trim().TrimStart(TrimPeriod));

            return Uri.TryCreate(uriString, UriKind.Absolute, out uri);
        }

        private string GetLocation() => this.connectionStringParsed.TryGetValue("Location", out string location) ? location : null;
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
