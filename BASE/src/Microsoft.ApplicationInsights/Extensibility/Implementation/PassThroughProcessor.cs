namespace Microsoft.ApplicationInsights.Shared.Extensibility.Implementation
{
    using System;
    using Microsoft.ApplicationInsights.Channel;

    /// <summary>
            {
                throw new ArgumentNullException(nameof(sink));
        }

        internal TelemetrySink Sink { get; } 

        public void Process(ITelemetry item)
        {
            this.Sink.Process(item);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
