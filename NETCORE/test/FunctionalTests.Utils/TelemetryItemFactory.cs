namespace FunctionalTests.Utils
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using AI;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal static class TelemetryItemFactory
    {
        public static IList<Envelope> GetTelemetryItems(string content)
        {
            var items = new List<Envelope>();

            if (string.IsNullOrWhiteSpace(content))
            {
                return items;
            }
            var newLines = new [] { "\r\n", "\n" };

            string[] lines = content.Split(newLines, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                JsonReader reader = new JsonTextReader(new StringReader(line));
                reader.DateParseHandling = DateParseHandling.None;
                    case TelemetryItemType.Exception:
                        {
                            result = JsonConvert.DeserializeObject<TelemetryItem<ExceptionData>>(content);
                            break;
                        }

                    case TelemetryItemType.Request:
                    case TelemetryItemType.RemoteDependency:
                        {
                            result = JsonConvert.DeserializeObject<TelemetryItem<RemoteDependencyData>>(content);
                            break;
                        }

                    case TelemetryItemType.Message:
                            result = JsonConvert.DeserializeObject<TelemetryItem<MessageData>>(content);
                            break;
                        }

                    case TelemetryItemType.Event:
                        {
                            result = JsonConvert.DeserializeObject<TelemetryItem<EventData>>(content);
                            break;
                        }

                    default:
                        {
                            throw new InvalidDataException("Unsupported telemetry type");
                        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
