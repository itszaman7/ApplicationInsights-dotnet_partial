using System;
using System.Collections.Generic;

using Microsoft.ApplicationInsights.Channel;


namespace Microsoft.ApplicationInsights.Metrics.TestUtility
{
    /// <summary>
    /// A stub of <see cref="ITelemetryChannel"/>.
    /// </summary>
    public sealed class StubApplicationInsightsTelemetryChannel : ITelemetryChannel
        /// <summary>
        /// Initializes a new instance of the <see cref="StubTelemetryChannel"/> class.
        /// </summary>
        public StubApplicationInsightsTelemetryChannel()
        {
            OnSend = telemetry => { };
            OnFlush = () => { };
            OnDispose = () => { };
            TelemetryItems = new List<ITelemetry>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this channel is in developer mode.
        /// </summary>
        public bool? DeveloperMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the channel's URI. To this URI the telemetry is expected to be sent.
        public IList<ITelemetry> TelemetryItems { get; }

        /// <summary>
        /// Implements the <see cref="ITelemetryChannel.Send"/> method by invoking the <see cref="OnSend"/> callback.
        /// </summary>
        public void Send(ITelemetry item)
        /// <summary>
        /// Implements the <see cref="IDisposable.Dispose"/> method.
        /// </summary>
        public void Dispose()
        {
            OnDispose();


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
