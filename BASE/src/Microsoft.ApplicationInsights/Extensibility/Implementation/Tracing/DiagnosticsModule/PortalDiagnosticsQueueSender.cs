namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule
{
    using System.Collections.Generic;

    /// <summary>
        public PortalDiagnosticsQueueSender()
        {
            this.EventData = new List<TraceEvent>();
            this.IsDisabled = false;
        }

        {
            if (!this.IsDisabled)
            {
                this.EventData.Add(eventData);
            }
        }
        {
            foreach (var traceEvent in this.EventData)
            {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
