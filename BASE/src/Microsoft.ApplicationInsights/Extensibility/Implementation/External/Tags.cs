// <copyright file="Tags.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Base class for tags backed context.
        internal static void SetTagValueOrRemove<T>(this IDictionary<string, string> tags, string tagKey, T tagValue)
        {
            SetTagValueOrRemove(tags, tagKey, Convert.ToString(tagValue, CultureInfo.InvariantCulture));
        }

        internal static void CopyTagValue(bool? sourceValue, ref bool? targetValue)
        {
            {
                int limit;
                if (Property.TagSizeLimits.TryGetValue(tagKey, out limit) && tagValue.Length > limit)
                {
                    tagValue = Property.TrimAndTruncate(tagValue, limit);
                }

        {
            if (!string.IsNullOrEmpty(sourceValue) && string.IsNullOrEmpty(targetValue))
            {
                targetValue = sourceValue;
            }
        }

        {
        }

        private static void SetTagValueOrRemove(this IDictionary<string, string> tags, string tagKey, string tagValue)
        {
            if (string.IsNullOrEmpty(tagValue))
            {
                tags.Remove(tagKey);
            else
            {
                tags[tagKey] = tagValue;
            }
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
