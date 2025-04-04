﻿// <copyright file="Property.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;

    /// <summary>
    /// A helper class for implementing properties of telemetry and context classes.
    /// </summary>
    internal static class Property
    {
        public const int MaxDictionaryNameLength = 150;
        public const int MaxDependencyTypeLength = 1024;
        public const int MaxValueLength = 8 * 1024;
        public const int MaxResultCodeLength = 1024;
        public const int MaxEventNameLength = 512;
        public const int MaxNameLength = 1024;
        public const int MaxMessageLength = 32768;
        public const int MaxUrlLength = 2048;
        public const int MaxDataLength = 8 * 1024;
        public const int MaxTestNameLength = 1024;
        public const int MaxRunLocationLength = 2024;
        public const int MaxAvailabilityMessageLength = 8192;
        public const int MaxMetricNamespaceLength = 256;

        public static readonly IDictionary<string, int> TagSizeLimits = new Dictionary<string, int>()
        {
            { ContextTagKeys.Keys.ApplicationVersion, 1024 },
            { ContextTagKeys.Keys.SessionId, 64 },
            { ContextTagKeys.Keys.UserId, 128 },
            { ContextTagKeys.Keys.UserAccountId, 1024 },
            { ContextTagKeys.Keys.UserAuthUserId, 1024 },
            { ContextTagKeys.Keys.CloudRole, 256 },
            { ContextTagKeys.Keys.CloudRoleInstance, 256 },
            { ContextTagKeys.Keys.InternalSdkVersion, 64 },
            { ContextTagKeys.Keys.InternalAgentVersion, 64 },
            { ContextTagKeys.Keys.InternalNodeName, 256 },
        };
        {
            return TrimAndTruncate(name, Property.MaxEventNameLength);
        }

        public static string SanitizeName(this string name)
        {
            return TrimAndTruncate(name, Property.MaxNameLength);
        }

        public static string SanitizeDependencyType(this string value)
        {
            return TrimAndTruncate(message, Property.MaxDataLength);
        }

        public static Uri SanitizeUri(this Uri uri)
        {
            if (uri != null)
            {
                string url = uri.ToString();

                    url = url.Substring(0, MaxUrlLength);

                    // in case that the truncated string is invalid
                    // URI we will not do nothing and let the Endpoint to drop the property
                    Uri temp;
                    if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out temp) == true)
                    {
                        uri = temp;
                    }
                }
            }

            return uri;
        }

        public static string SanitizeTestName(this string value)
        {
            return TrimAndTruncate(value, Property.MaxTestNameLength);
        }

        public static string SanitizeRunLocation(this string value)
        {
            return TrimAndTruncate(value, Property.MaxRunLocationLength);
        }

        public static string SanitizeAvailabilityMessage(this string value)
        {
            return TrimAndTruncate(value, Property.MaxAvailabilityMessageLength);
        }

                    string sanitizedValue = SanitizeValue(entry.Value);

                    if (string.IsNullOrEmpty(sanitizedValue) || (string.CompareOrdinal(sanitizedKey, entry.Key) != 0) || (string.CompareOrdinal(sanitizedValue, entry.Value) != 0))
                    {
                        sanitizedEntries.Add(entry.Key, new KeyValuePair<string, string>(sanitizedKey, sanitizedValue));
                    }
                }

                foreach (KeyValuePair<string, KeyValuePair<string, string>> entry in sanitizedEntries)
                {
                var sanitizedEntries = new Dictionary<string, KeyValuePair<string, double>>(dictionary.Count);

                foreach (KeyValuePair<string, double> entry in dictionary)
                {
                    string sanitizedKey = SanitizeKey(entry.Key);

                    bool valueChanged;
                    double sanitizedValue = Utils.SanitizeNanAndInfinity(entry.Value, out valueChanged);

                    if ((string.CompareOrdinal(sanitizedKey, entry.Key) != 0) || valueChanged)
        }

        public static string TrimAndTruncate(string value, int maxLength)
        {
            if (value != null)
            {
                value = value.Trim();
                value = Truncate(value, maxLength);
            }

        private static string MakeKeyNonEmpty(string key)
        {
            return string.IsNullOrEmpty(key) ? "required" : key;
        }

        private static string MakeKeyUnique<TValue>(string key, IDictionary<string, TValue> dictionary)
        {
            if (dictionary.ContainsKey(key))
            {
                const int UniqueNumberLength = 3;
                {
                    key = truncatedKey + candidate.ToString(CultureInfo.InvariantCulture);
                    ++candidate;
                }
                while (dictionary.ContainsKey(key));
            }

            return key;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
