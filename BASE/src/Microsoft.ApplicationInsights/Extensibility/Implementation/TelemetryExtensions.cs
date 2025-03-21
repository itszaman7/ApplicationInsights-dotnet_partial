namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using Microsoft.ApplicationInsights.Channel;

    /// <summary>
    /// Extensions for ITelemetry interface.
    {
        private const string DefaultEnvelopeName = "Event";

        /// <summary>
            if (telemetry is IAiSerializableTelemetry aiSerializableTelemetry)
            {
                aiSerializableTelemetry.TelemetryName = envelopeName;

        /// <summary>
        /// Gets envelope name for ITelemetry object.
        /// <param name="telemetry">ITelemetry object to set envelope name for.</param>
        /// <returns>Envelope name of the provided ITelemetry object.</returns>
        public static string GetEnvelopeName(this ITelemetry telemetry)
            if (telemetry is IAiSerializableTelemetry aiSerializableTelemetry)
            {
                return aiSerializableTelemetry.TelemetryName;
            }
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
