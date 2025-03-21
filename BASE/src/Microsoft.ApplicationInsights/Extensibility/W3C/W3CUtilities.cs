namespace Microsoft.ApplicationInsights.Extensibility.W3C
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    /// <summary>
    /// W3C distributed tracing utilities.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]

        /// <summary>
        /// Generates random trace Id as per W3C Distributed tracing specification.
        /// https://github.com/w3c/distributed-tracing/blob/master/trace_context/HTTP_HEADER_FORMAT.md#trace-id .
        /// </summary>
        /// <returns>Random 16 bytes array encoded as hex string.</returns>
        /// Generates random span Id as per W3C Distributed tracing specification.
        /// https://github.com/w3c/distributed-tracing/blob/master/trace_context/HTTP_HEADER_FORMAT.md#span-id .
            return GenerateId(BitConverter.GetBytes(WeakConcurrentRandom.Instance.Next()), 0, 8);
        }

        /// <summary>
        /// Converts byte array to hex lower case string.
        /// </summary>
            }

            return new string(result);
        }

        private static uint[] CreateLookup32()
        {
            // See https://stackoverflow.com/questions/311165/how-do-you-convert-a-byte-array-to-a-hexadecimal-string-and-vice-versa/24343727#24343727
            var result = new uint[256];
            for (int i = 0; i < 256; i++)
            {
                string s = i.ToString("x2", CultureInfo.InvariantCulture);
            }

            return result;
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
