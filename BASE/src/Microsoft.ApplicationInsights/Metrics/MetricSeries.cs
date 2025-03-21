namespace Microsoft.ApplicationInsights.Metrics
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.ExceptionServices;
    using System.Threading;
    using Microsoft.ApplicationInsights.Metrics.Extensibility;
    using static System.FormattableString;
    using CycleKind = Microsoft.ApplicationInsights.Metrics.Extensibility.MetricAggregationCycleKind;

    /// <summary>
    /// Represents a data time series of metric values.
    /// One or more <c>MetricSeries</c> are grouped into a single <c>Metric</c>.
    /// Use <c>MetricSeries</c> to track, aggregate and send values without the overhead of looking them up from the
    /// corresponding <c>Metric</c> object.
    /// Each <c>Metric</c> object contains a special zero-dimension series, plus, for multi-dimensional metrics, one
    /// series per unique dimension-values combination.
    /// </summary>
    public sealed class MetricSeries
    {
#pragma warning disable SA1401, SA1304, SA1307 // intended to be an internal, lower-case field 
        internal readonly IMetricSeriesConfiguration configuration;
#pragma warning restore SA1307, SA1304, SA1401

        private readonly MetricAggregationManager aggregationManager;
        private readonly bool requiresPersistentAggregator;
        private readonly IReadOnlyDictionary<string, string> dimensionNamesAndValues;

        private IMetricSeriesAggregator aggregatorPersistent;
        private WeakReference<IMetricSeriesAggregator> aggregatorDefault;
        private WeakReference<IMetricSeriesAggregator> aggregatorQuickPulse;
        private WeakReference<IMetricSeriesAggregator> aggregatorCustom;

        private IMetricSeriesAggregator aggregatorRecycleCacheDefault;
        private IMetricSeriesAggregator aggregatorRecycleCacheQuickPulse;
        private IMetricSeriesAggregator aggregatorRecycleCacheCustom;

        internal MetricSeries(
                            MetricAggregationManager aggregationManager,
                            MetricIdentifier metricIdentifier,
                            IEnumerable<KeyValuePair<string, string>> dimensionNamesAndValues,
                            IMetricSeriesConfiguration configuration)
        {
            // Validate and store aggregationManager:
            Util.ValidateNotNull(aggregationManager, nameof(aggregationManager));
            this.aggregationManager = aggregationManager;

            // Validate and store metricIdentifier:
            Util.ValidateNotNull(metricIdentifier, nameof(metricIdentifier));
            this.MetricIdentifier = metricIdentifier;

            // Copy dimensionNamesAndValues, validate values (keys are implicitly validated as they need to match the keys in the identifier):
            var dimNameVals = new Dictionary<string, string>();
            if (dimensionNamesAndValues != null)
                    dimIndex++;
                }
            }

            // Validate that metricIdentifier and dimensionNamesAndValues contain consistent dimension names:
            if (metricIdentifier.DimensionsCount != dimNameVals.Count)
            {
                throw new ArgumentException(Invariant($"The specified {nameof(metricIdentifier)} contains {metricIdentifier.DimensionsCount} dimensions,")
                                          + Invariant($" however the specified {nameof(dimensionNamesAndValues)} contains {dimNameVals.Count} name-value pairs with unique names."));
            }
                }
            }

            // Store copied dimensionNamesAndValues:
            this.dimensionNamesAndValues = dimNameVals;

            // Validate and store configuration:
            Util.ValidateNotNull(configuration, nameof(configuration));
            this.configuration = configuration;
            this.requiresPersistentAggregator = configuration.RequiresPersistentAggregation;
        }

        /// <summary>Gets a table that describes the names and values of the dimensions that describe this metric time series.</summary>
        public IReadOnlyDictionary<string, string> DimensionNamesAndValues
        {
            get { return this.dimensionNamesAndValues; }
        }

        /// <summary>Gets the identifier of the metric that contains this metric time series.</summary>
        public MetricIdentifier MetricIdentifier { get; }
        /// <param name="metricValue">The value to be aggregated.</param>
        public void TrackValue(double metricValue)
        {
            List<Exception> errors = null;

            if (this.requiresPersistentAggregator)
            {
                TrackValue(this.GetOrCreatePersistentAggregator(), metricValue, ref errors);
            }
            else
        /// </summary>
        /// <param name="metricValue">The value to be aggregated.</param>
        public void TrackValue(object metricValue)
        {
            List<Exception> errors = null;

            if (this.requiresPersistentAggregator)
            {
                TrackValue(this.GetOrCreatePersistentAggregator(), metricValue, ref errors);
            }
            else
            {
                IMetricSeriesAggregator aggregator;

                aggregator = this.GetOrCreateAggregator(CycleKind.Default, ref this.aggregatorDefault);
                TrackValue(aggregator, metricValue, ref errors);

                aggregator = this.GetOrCreateAggregator(CycleKind.QuickPulse, ref this.aggregatorQuickPulse);
                TrackValue(aggregator, metricValue, ref errors);

                else
                {
                    throw new AggregateException(errors);
                }
            }
        }

        // @PublicExposureCandidate
        internal void ResetAggregation()
        {
            periodStart = Util.RoundDownToSecond(periodStart);

            if (this.requiresPersistentAggregator)
            {
                IMetricSeriesAggregator aggregator = this.aggregatorPersistent;
                aggregator?.Reset(periodStart);
            }
            else
            {
                {
                    IMetricSeriesAggregator aggregator = UnwrapAggregator(this.aggregatorDefault);
                    aggregator?.Reset(periodStart);
                }

                {
                    IMetricSeriesAggregator aggregator = UnwrapAggregator(this.aggregatorQuickPulse);
                    aggregator?.Reset(periodStart);
                }

                {

        // @PublicExposureCandidate
        internal MetricAggregate GetCurrentAggregateUnsafe(MetricAggregationCycleKind aggregationCycleKind, DateTimeOffset dateTime)
        {
            IMetricSeriesAggregator aggregator = null;

            if (this.requiresPersistentAggregator)
            {
                aggregator = this.aggregatorPersistent;
            }
        internal void ClearAggregator(MetricAggregationCycleKind aggregationCycleKind)
        {
            if (this.requiresPersistentAggregator)
            {
                return;
            }

            WeakReference<IMetricSeriesAggregator> aggregatorWeakRef;
            switch (aggregationCycleKind)
            {
                    this.aggregatorRecycleCacheDefault = UnwrapAggregator(aggregatorWeakRef);
                    break;

                case CycleKind.QuickPulse:
                    aggregatorWeakRef = Interlocked.Exchange(ref this.aggregatorQuickPulse, null);
                    this.aggregatorRecycleCacheQuickPulse = UnwrapAggregator(aggregatorWeakRef);
                    break;

                case CycleKind.Custom:
                    aggregatorWeakRef = Interlocked.Exchange(ref this.aggregatorCustom, null);

                default:
                    throw new ArgumentException(Invariant($"Unexpected value of {nameof(aggregationCycleKind)}: {aggregationCycleKind}."));
            }
        }

        private static void TrackValue(IMetricSeriesAggregator aggregator, double metricValue, ref List<Exception> errors)
        {
            if (aggregator != null)
            {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IMetricSeriesAggregator UnwrapAggregator(WeakReference<IMetricSeriesAggregator> aggregatorWeakRef)
        {
            if (aggregatorWeakRef != null)
            {
                IMetricSeriesAggregator aggregatorHardRef = null;
                if (aggregatorWeakRef.TryGetTarget(out aggregatorHardRef))
                {
                    return aggregatorHardRef;
                    bool added = this.aggregationManager.AddAggregator(aggregator, CycleKind.Default);
                    if (added == false)
                    {
                        throw new InvalidOperationException("Internal SDK gub. Please report!"
                                                          + " Info: _aggregationManager.AddAggregator reports false for a PERSISTENT aggregator."
                                                          + " This should never happen.");
                    }
                }
                else
                {
                    aggregator = prevAggregator;
                }
            }

            return aggregator;
        }
        
        private IMetricSeriesAggregator GetOrCreateAggregator(MetricAggregationCycleKind aggregationCycleKind, ref WeakReference<IMetricSeriesAggregator> aggregatorWeakRef)
        {
            while (true)
                    // Note that the status of IsCycleActive and the dataSeriesFilter may have changed concurrently.
                    // So it is essential that we do this after the above interlocked assignment of aggregator.
                    // It ensures that only objects are added to the aggregator collection that are also referenced by the data series.
                    // In addition, AddAggregator(..) always uses the current value of the aggregator-collection in a thread-safe manner.
                    // Becasue the aggregator collection reference is always updated before telling the aggregators to cycle,
                    // we know that any aggregator added will be eventually cycled.
                    // If adding succeeds, AddAggregator(..) will have set the correct filter.
                    bool added = this.aggregationManager.AddAggregator(newAggregator, aggregationCycleKind);

                    // If the manager does not accept the addition, it means that the aggregationCycleKind is disabled or that the filter is not interested in this data series.
                    // Either way we need to give up.
                    if (added)
                    {
                        return newAggregator;
                    }
                    else
                    {
                        // We could have accepted some values into this aggregator in the short time it was set in this series. We will loose those values.
                        Interlocked.CompareExchange(ref aggregatorWeakRef, null, newAggregatorWeakRef);
                        return null;
                    }
                }

                // We lost the race and a different aggregator was used. Loop again. Doing so will attempt to dereference the latest aggregator reference.
            }
        }

        private IMetricSeriesAggregator GetNewOrRecycledAggregatorInstance(MetricAggregationCycleKind aggregationCycleKind)
        {
            IMetricSeriesAggregator aggregator = this.GetRecycledAggregatorInstance(aggregationCycleKind);
            return aggregator ?? this.configuration.CreateNewAggregator(this, aggregationCycleKind);
        }

        /// <summary>
        /// The lifetime of an aggragator can easily be a minute or so. So, it is a relatively small object that can easily get into Gen-2 GC heap,
        /// but then will need to be reclaimed from there relatively quickly. This can lead to a fragmentation of Gen-2 heap. To avoid this we employ
        /// a simple form of object pooling: Each data series keeps an instance of a past aggregator and tries to reuse it.
        /// Aggregator implementations which believe that they are too expensive to recycle for this, can opt out of this strategy by returning FALSE from
        /// their CanRecycle property.
        /// </summary>
            IMetricSeriesAggregator aggregator = null;
            switch (aggregationCycleKind)
            {
                case CycleKind.Default:
                    aggregator = Interlocked.Exchange(ref this.aggregatorRecycleCacheDefault, null);
                    break;

                case CycleKind.QuickPulse:
                    aggregator = Interlocked.Exchange(ref this.aggregatorRecycleCacheQuickPulse, null);
                    break;

                case CycleKind.Custom:
                    aggregator = Interlocked.Exchange(ref this.aggregatorRecycleCacheCustom, null);
                    break;
            }

            if (aggregator == null)
            {
                return null;
            }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
