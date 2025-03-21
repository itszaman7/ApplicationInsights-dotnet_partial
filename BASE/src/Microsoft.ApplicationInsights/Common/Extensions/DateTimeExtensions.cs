namespace Microsoft.ApplicationInsights.Common.Extensions
{
    using System;
    /// Provides extension methods for <see cref="DateTime"/>.
    /// </summary>
    {
        /// <summary>
        /// Converts the value of the current System.DateTime object to its equivalent string representation using the specified format and CultureInfo.InvariantCulture.
        /// </summary>
        /// <returns>A string representation of value of the current System.DateTime object as specified by format and provider.</returns>
        public static string ToInvariantString(this DateTime input, string format) => input.ToString(format, CultureInfo.InvariantCulture);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
