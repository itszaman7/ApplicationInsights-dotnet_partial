namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.Linq;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;

    internal class AzureSdkEventListener : EventListener
    {
#if NET452
        private static readonly object[] EmptyArray = new object[0];
#else
        private static readonly object[] EmptyArray = Array.Empty<object>();
#endif

            {

            if (this.telemetryClient == null)
            {
                this.eventSources.Add(eventSource);
                return;
            }

            // EventSource names are deduplicated for environments like
            // Functions where the same library can be loaded twice.
            }

            var payloadArray = eventData.Payload?.ToArray() ?? EmptyArray;
            string message = string.Empty;
            if (eventData.Message != null)
            {
                try
                {
                    message = string.Format(CultureInfo.InvariantCulture, eventData.Message, payloadArray);
                }
                catch (FormatException)
                {
                }
            }
            else
            {
                message = String.Join(", ", payloadArray); 
            }
                trace.Properties["EventName"] = eventData.EventName;
            }
#endif
            this.telemetryClient?.TrackTrace(trace);
        }

        private static SeverityLevel FromEventLevel(EventLevel level)
        {
            switch (level)
                case EventLevel.Informational:
                    return SeverityLevel.Information;
                case EventLevel.Verbose:
                default:
                    return SeverityLevel.Verbose;
            }
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
