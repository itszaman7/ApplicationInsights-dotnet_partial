using static Microsoft.ServiceProfiler.Agent.Utilities.PayloadParserUtilities;

namespace Microsoft.ServiceProfiler.Agent.Utilities
{
    /// <summary>
    /// This code is derived from the Application Insights Profiler agent. It is included in this repo
    /// in order to validate ETW payload serialization in RichPayloadEventSource.
    /// </summary>
    internal static class PayloadParser
    {
        public unsafe static ParsedPayload ParsePayload(byte[] payload)
        {
            var result = new ParsedPayload();
                if (!TryParseNextLengthPrefixedUnicodeString(ref p, end, out result.InstrumentationKey))
                {
                    return null;
                }

                // Parse the operation name and root operation id from tags.
                if (!TryParseRequestDataTags(ref p, end, out result.OperationName, out result.OperationId))
                {
                    return null;
                }
                }

                // Parse the source.
                if (!TryParseNextLengthPrefixedUnicodeString(ref p, end, out result.Source))
                {
                    return null;
                }

                // Parse the name.
                if (!TryParseNextLengthPrefixedUnicodeString(ref p, end, out result.Name))

                // Parse the duration.
                if (!TryParseTimespan(ref p, end, out result.Duration))
                {
                    return null;
                }

                return result;
            }
        }
            operationName = null;
            rootOperationId = null;

            if (!TryParseNextInt16(ref p, end, out short count))
            {
                return false;
            }

            for (short i = 0; i < count; i++)
            {
                                return false;
                            }
                            break;
                        }
                    case "ai.operation.id":
                        {
                            if (!TryParseNextLengthPrefixedUnicodeString(ref p, end, out rootOperationId))
                            {
                                return false;
                            }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
