using System;

namespace Microsoft.ApplicationInsights.TestFramework
{
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// A stub of <see cref="ITelemetryProcessor"/>.
    /// </summary>
        /// Initializes a new instance of the <see cref="StubTelemetryProcessor"/> class.
        /// </summary>
        public StubTelemetryProcessor(ITelemetryProcessor next)
        {
            this.next = next;
            this.OnDispose = () => { };
        /// <summary>
        /// Implements the <see cref="ITelemetryProcessor.Initialize"/> method by invoking the process method
        /// </summary>
        public void Process(ITelemetry telemetry)
        {
            this.OnProcess(telemetry);
            }
        }

        public void Dispose()
        {
            this.OnDispose();


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
