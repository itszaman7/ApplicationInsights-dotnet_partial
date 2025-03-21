namespace Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId
{
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    internal static class ApplicationIdHelper
    {
        /// <summary>
        /// special string which describes that ID was taken from Breeze.
        /// </summary>
        private const string Format = "cid-v1:{0}";

        /// </summary>
        private const int ApplicationIdMaxLength = 50;

        /// <summary>
        /// Format an Application Id string (ex: 00000000-0000-0000-0000-000000000000) 
        /// as (ex: cid-v1:00000000-0000-0000-0000-000000000000).
        /// </summary>
        /// Remove all characters which are not header safe.
        /// </summary>
        /// <remarks>
        /// Input is expected to be a GUID. For performance, only use the Regex after an unsupported character is discovered.
        /// </remarks>
        internal static string SanitizeString(string input)
        {
            if (input == null)
            {
                return null;
            }

            foreach (var ch in input)
            {
                if (!IsCharHeaderSafe(ch))
                {
                    return Regex.Replace(input, @"[^\u0020-\u007F]", string.Empty);
                }
            }

            return input;
        /// Check a strings length and trim to a max length if needed.
                input = input.Substring(0, maxLength);
            }

            return input;
        }

        /// <summary>


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
