namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;

    /// This processor is always appended as the last processor in the chain.
    /// </summary>
    internal class TransmissionProcessor : ITelemetryProcessor
        /// </summary>        
        /// <param name="sink">The <see cref="TelemetrySink"/> holding to the telemetry channel to use for sending telemetry.</param>
        internal TransmissionProcessor(TelemetrySink sink)
        {
            this.sink = sink ?? throw new ArgumentNullException(nameof(sink));
        }

        /// <summary>
        /// Process the given <see cref="ITelemetry"/> item. Here processing is sending the item through the channel/>.
        /// </summary>
        public void Process(ITelemetry item)
        {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
