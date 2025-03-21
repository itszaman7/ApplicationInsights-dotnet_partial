namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;

    /// <summary>
    /// Subscriber to ETW Event source events, which sends data to other Senders (F5 and Portal).
    /// </summary>
    internal class DiagnosticsListener : IDisposable
        private readonly IList<IDiagnosticsSender> diagnosticsSenders = new List<IDiagnosticsSender>();
        private EventLevel logLevel = EventLevel.Error;
        private DiagnosticsEventListener eventListener;

        public DiagnosticsListener(IList<IDiagnosticsSender> senders)
        {
            if (senders == null || senders.Count < 1)
        {
            get => this.logLevel;

            set
            {
                if (this.LogLevel != value)
                {
        /// <summary>
        /// Sets LogLevel. Possible values LogAlways, Critical, Error, Warning, Informational and Verbose.
        /// </summary>
        public void SetLogLevel(string value)
        {
            // Once logLevel is set from configuration, restart listener with new value
            if (!string.IsNullOrEmpty(value))
            {
                EventLevel parsedValue;
                if (Enum.IsDefined(typeof(EventLevel), value) == true)
                {
                    parsedValue = (EventLevel)Enum.Parse(typeof(EventLevel), value, true);
                    this.LogLevel = parsedValue;
                }
                    }
                }
            }
        }
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.eventListener.Dispose();
            }
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
