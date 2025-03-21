namespace Microsoft.ApplicationInsights.Metrics
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Microsoft.ApplicationInsights.Metrics.ConcurrentDatastructures;
    using Microsoft.ApplicationInsights.Metrics.Extensibility;
    using static System.FormattableString;
    using CycleKind = Microsoft.ApplicationInsights.Metrics.Extensibility.MetricAggregationCycleKind;

    internal class MetricAggregationManager
    {
        // We support 4 aggregation cycles. 2 of them can be accessed from the outside:

        private AggregatorCollection aggregatorsForPersistent = null;
        private AggregatorCollection aggregatorsForDefault = null;
        private AggregatorCollection aggregatorsForQuickPulse = null;
        private AggregatorCollection aggregatorsForCustom = null;

        internal MetricAggregationManager()
        {
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset timestamp = Util.RoundDownToSecond(now);

            this.aggregatorsForDefault = new AggregatorCollection(timestamp, filter: null);
            this.aggregatorsForPersistent = new AggregatorCollection(timestamp, filter: null);
        }

        public AggregationPeriodSummary StartOrCycleAggregators(MetricAggregationCycleKind aggregationCycleKind, DateTimeOffset tactTimestamp, IMetricSeriesFilter futureFilter)
        {
            switch (aggregationCycleKind)
            {
                case CycleKind.Default:
                    if (futureFilter != null)
                    {
                        throw new ArgumentException(Invariant($"Cannot specify non-null {nameof(futureFilter)} when {nameof(aggregationCycleKind)} is {aggregationCycleKind}."));
                    }

                    return this.CycleAggregators(ref this.aggregatorsForDefault, tactTimestamp, futureFilter, stopAggregators: false);

                case CycleKind.QuickPulse:
                    return this.CycleAggregators(ref this.aggregatorsForQuickPulse, tactTimestamp, futureFilter, stopAggregators: false);

                case CycleKind.Custom:
                    return this.CycleAggregators(ref this.aggregatorsForCustom, tactTimestamp, futureFilter, stopAggregators: false);

                default:
                    throw new ArgumentException(Invariant($"Unexpected value of {nameof(aggregationCycleKind)}: {aggregationCycleKind}."));
            }
        }

        public AggregationPeriodSummary StopAggregators(MetricAggregationCycleKind aggregationCycleKind, DateTimeOffset tactTimestamp)
        {

                default:
                    throw new ArgumentException(Invariant($"Unexpected value of {nameof(aggregationCycleKind)}: {aggregationCycleKind}."));
            }
        }

        internal bool AddAggregator(IMetricSeriesAggregator aggregator, MetricAggregationCycleKind aggregationCycleKind)
        {
            Util.ValidateNotNull(aggregator, nameof(aggregator));


            switch (aggregationCycleKind)
            {
                case CycleKind.Default:
                    return AddAggregator(aggregator, this.aggregatorsForDefault);

                case CycleKind.QuickPulse:
                    return AddAggregator(aggregator, this.aggregatorsForQuickPulse);

                case CycleKind.Custom:
        }

        private static bool AddAggregator(IMetricSeriesAggregator aggregator, AggregatorCollection aggregatorCollection)
        {
            if (aggregatorCollection == null)
            {
                return false;
            }

            IMetricSeriesFilter seriesFilter = aggregatorCollection.Filter;
        private static List<MetricAggregate> GetNonpersistentAggregations(DateTimeOffset tactTimestamp, AggregatorCollection aggregators)
        {
            // Complete each non-persistent aggregator:
            // (we snapshotted the entire collection, so Count is stable)

            GrowingCollection<IMetricSeriesAggregator> actualAggregators = aggregators?.Aggregators;

            if (null == actualAggregators || 0 == actualAggregators.Count)
            {
                return new List<MetricAggregate>(capacity: 0);
                    if (aggregate != null)
                    {
                        nonpersistentAggregations.Add(aggregate);
            {
                throw new InvalidOperationException("Internal SDK bug. Please report. Cannot cycle persistent aggregators.");
            }

            tactTimestamp = Util.RoundDownToSecond(tactTimestamp);

            // For non-persistent aggregators: create empty holder for the next aggregation period and swap for the previous holder:
            AggregatorCollection prevAggregators;
            if (stopAggregators)
            {
                    IMetricSeriesAggregator aggregator = persistentValsAggregators.Current;
                    if (aggregator != null)
                    {
                        // Persistent aggregators are always active, regardless of filters for a particular cycle.
                        // But we can apply the cycle's filters to determine whether or not to pull the aggregator
                        // for a aggregate at this time. Of course, only series filters, not value filters, can be considered.
                        IMetricValueFilter unusedValueFilter;
                        bool satisfiesFilter = Util.FilterWillConsume(previousFilter, aggregator.DataSeries, out unusedValueFilter);
                        if (satisfiesFilter)
                        {
                            MetricAggregate aggregate = aggregator.CompleteAggregation(tactTimestamp);

                            if (aggregate != null)
                            {
                                persistentValsAggregations.Add(aggregate);
                            }
                        }
                    }
                }
            }
            finally
            {
                persistentValsAggregators.Dispose();
            }

            return persistentValsAggregations;
        }

        #region class AggregatorCollection

            }

            public DateTimeOffset PeriodStart { get; }

            public GrowingCollection<IMetricSeriesAggregator> Aggregators { get; }

            public IMetricSeriesFilter Filter { get; }
        }

        #endregion class AggregatorCollection


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
