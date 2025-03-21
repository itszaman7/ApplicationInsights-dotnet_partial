namespace Microsoft.ApplicationInsights.Extensibility.Filtering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    /// Unlike the main accumulator, this one might not have finished being processed at swap time,
    /// so the consumer should keep the reference to it post-swap and make the best effort not to send
    /// prematurely. <see cref="referenceCount"/> indicates that the accumulator is still being processed
    /// when non-zero.
    /// </summary>
    internal class CollectionConfigurationAccumulator
    {
        /// <summary>
        /// Used by writers to indicate that a processing operation is still in progress.
        /// </summary>

            // prepare the accumulators based on the collection configuration
            IEnumerable<Tuple<string, AggregationType>> allMetrics = collectionConfiguration?.TelemetryMetadata;
            foreach (Tuple<string, AggregationType> metricId in allMetrics ?? Enumerable.Empty<Tuple<string, AggregationType>>())
            {
        /// Gets a dictionary of metricId => AccumulatedValues.
        /// </summary>
        public Dictionary<string, AccumulatedValues> MetricAccumulators { get; } = new Dictionary<string, AccumulatedValues>();

        public CollectionConfiguration CollectionConfiguration { get; }

        public void Release()
        {
            Interlocked.Decrement(ref this.referenceCount);
        }
        public long GetRef()


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
