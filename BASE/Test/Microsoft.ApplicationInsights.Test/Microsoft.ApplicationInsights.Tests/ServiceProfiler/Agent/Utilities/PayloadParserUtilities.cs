using System;

namespace Microsoft.ServiceProfiler.Agent.Utilities
{
    /// <summary>
    /// This code is derived from the Application Insights Profiler agent. It is included in this repo
    /// in order to validate ETW payload serialization in RichPayloadEventSource.
    /// </summary>
    internal static class PayloadParserUtilities
    {
        /// <summary>
        /// First try to get the size of the next string value payload. Then try to retrieve the corresponding string payload according to the size.
        /// </summary>
        /// <param name="value">Set to the string value payload when success.</param>
        public static unsafe bool TryParseNextLengthPrefixedUnicodeString(ref byte* ptr, byte* end, out string value)
        {
            short size;
            if (TryParseNextInt16(ref ptr, end, out size))
            {
                return TryParseNextUnicodeString(ref ptr, end, size, out value);
            }
            else
            {
                value = string.Empty;
                return false;
            }
        }

        /// </summary>
        public static unsafe bool TryParseNextInt16(ref byte* ptr, byte* end, out short value)
        {
            var afterPtr = ptr + sizeof(short);
            if (afterPtr <= end)
            {
                value = *((short*)ptr);
                ptr = afterPtr;
                return true;
            }
        /// <summary>
        /// Try to get the size of the next string value payload.
        /// </summary>
        public static unsafe bool TryParseNextInt32(ref byte* ptr, byte* end, out int value)
        {
            var afterPtr = ptr + sizeof(int);
            if (afterPtr <= end)
            {
                value = *((int*)ptr);
                ptr = afterPtr;
            }
            else
            {
                value = 0;
            else
            {
                value = 0;
                return false;
            }
        }

        /// <summary>
        /// Try to get the next string value from the payload.
        /// </summary>
            }

            value = null;
            return false;
        }

        /// <summary>
        /// Try to parse a <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="ptr">Pointer to data.</param>
            }

            if (IsLengthPrefixedTimeSpanString(tickCount))
            {
                ptr = originalPtr;
                if (TryParseNextLengthPrefixedUnicodeString(ref ptr, end, out string timeSpanString) && TimeSpan.TryParse(timeSpanString, out timeSpan))
                {
                    return true;
                }
            }
        /// </remarks>
        private static bool IsLengthPrefixedTimeSpanString(long val)
        {
            // Quick check to reject values that could never be valid strings.
            // The mask value here tests for ASCII range 0000-003F in the top three 'chars'
            // and odd numbers in the range 0000-003F (0 to 63) for the length.
            if (unchecked((ulong)val & 0xFFC0FFC0FFC0FFC1UL) != 0)
            {
                return false;
            }
            // the mask above.
            var length = (ushort)val;
            if (!(length >= 16 && length <= 52))
            {
                return false;
            }

            // The first character must be a digit or minus sign.
            var ch = (char)(val >> 16);
            if (!(IsDigit(ch) || ch == '-'))

        /// <summary>
        /// Is the given character a decimal digit (0 - 9)
        /// </summary>
        /// <param name="ch">The character.</param>
        /// <returns>True if it's a decimal digit.</returns>
        /// <remarks>This is not the same as <see cref="char.IsDigit(char)"/> because it does not include digits from the extended Unicode range.</remarks>
        private static bool IsDigit(char ch) => unchecked((uint)(ch - '0')) < 10u;
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
