namespace Microsoft.ApplicationInsights.Metrics.Extensibility
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>A <c>IMetricTelemetryPipeline</c> that holds aggregates in memory instead of sending them anywhere for processing.
    /// This is useful for local testing and debugging scenarios.
    /// An instance of this class holds up to <see cref="CountLimit"/> aggregates in memory. WHen additional aggregates are written,
    /// the oldest ones get discarded.</summary>
    /// @PublicExposureCandidate
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001: Types that own disposable fields should be disposable", Justification = "OK not to explicitly dispose a released SemaphoreSlim.")]
    internal class MemoryMetricTelemetryPipeline : IMetricTelemetryPipeline, IReadOnlyList<MetricAggregate>
    {
        /// <summary>Default setting for how many items to hold in memory.</summary>
        /// <summary>Creates a new <c>MemoryMetricTelemetryPipeline</c> that holds up to <c>CountLimitDefault</c> aggregates in memory.</summary>
        public MemoryMetricTelemetryPipeline()
            : this(CountLimitDefault)
        {
        }

        /// <summary>Creates a new <c>MemoryMetricTelemetryPipeline</c> that holds up to the specified number of aggregates in memory.</summary>
        /// <param name="countLimit">Max number of most recent aggregates to hold in memory.</param>
        public MemoryMetricTelemetryPipeline(int countLimit)
        {
            if (countLimit <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(countLimit));
            }

            this.CountLimit = countLimit;
        }

        /// <summary>Gets the max buffer size.</summary>
                }

                return count;
            }
        }

        /// <summary>Provides access to the metric aggregates that have been written to this pipeline.</summary>
        /// <param name="index">Index of the aggregate in the buffer.</param>
        /// <returns>Metric aggregate at the specified index.</returns>
        public MetricAggregate this[int index]
        {
            get
            {
                MetricAggregate metricAggregate;
                this.updateLock.WaitAsync().GetAwaiter().GetResult();
                try
                {
                    metricAggregate = this.metricAgregates[index];
                }
                finally
                {
                    this.updateLock.Release();
                }

                return metricAggregate;
            }
        }

        /// <summary>Clears the buffer.</summary>
        public void Clear()
        /// the oldest item (the one at index 0) gets discarded before adding the new item at the end of the buffer.</summary>
        /// <param name="metricAggregate">Aggregate to keep.</param>
        /// <param name="cancelToken">To signal cancellation of the track-operation.</param>
        /// <returns>A task representing the completion of this operation.</returns>
        public async Task TrackAsync(MetricAggregate metricAggregate, CancellationToken cancelToken)
        {
            Util.ValidateNotNull(metricAggregate, nameof(metricAggregate));

            await this.updateLock.WaitAsync(cancelToken).ConfigureAwait(true);
            try
        IEnumerator<MetricAggregate> IEnumerable<MetricAggregate>.GetEnumerator()
        {
            IEnumerator<MetricAggregate> enumerator;
            this.updateLock.WaitAsync().GetAwaiter().GetResult();
            try
            {
                enumerator = this.metricAgregates.GetEnumerator();
            }
            finally
            {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
