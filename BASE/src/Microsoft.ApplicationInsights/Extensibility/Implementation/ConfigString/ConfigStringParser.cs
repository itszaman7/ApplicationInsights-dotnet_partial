namespace Microsoft.ApplicationInsights.Extensibility.Implementation.ConfigString
{
    using System;
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    internal static class ConfigStringParser
    {
        private static readonly char[] SplitSemicolon = new char[] { ';' };

        /// <summary>
        /// Parse a given string and return a dictionary of the key/value pairs.
        {
            if (configString == null)
            {
                string message = "Input cannot be null.";
                CoreEventSource.Log.ConfigurationStringParseWarning(message);
                throw new ArgumentNullException(message);
            }
                throw new ArgumentException(message);
            }

            var keyValuePairs = configString.Split(SplitSemicolon, StringSplitOptions.RemoveEmptyEntries);
            var dictionary = new Dictionary<string, string>(keyValuePairs.Length, StringComparer.OrdinalIgnoreCase);

                if (keyAndValue.Length != 2)
                {
                    string message = Invariant($"Input contains invalid delimiters and cannot be parsed. Expected example: 'key1=value1;key2=value2;key3=value3'.");
                    CoreEventSource.Log.ConfigurationStringParseWarning(message);
                    throw new ArgumentException(message);
                }

                var key = keyAndValue[0].Trim();
                var value = keyAndValue[1].Trim();

                if (dictionary.ContainsKey(key))
                {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
