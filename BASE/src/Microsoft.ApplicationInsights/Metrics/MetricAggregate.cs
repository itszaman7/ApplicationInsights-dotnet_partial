namespace Microsoft.ApplicationInsights.Metrics
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>Holds the metric aggregation results of a particular metric data series over an aggregation time period.
    /// The specific data fields on instanced of this class are not strongly typed (property bag) which allows using this
    /// aggregate type for aggregates of any aggregation kind.</summary>
    public class MetricAggregate
    {
        // We want to make the aggregate thread safe, but we expect no significant contention, so a simple lock will suffice.
        private readonly object updateLock = new object();

        private DateTimeOffset aggregationPeriodStart;
        /// <summary>Ceates a new metric aggregate.</summary>
        /// <param name="metricNamespace">The namespace of the metric that produces this aggregate.</param>
            this.aggregationPeriodDuration = default(TimeSpan);

            this.Dimensions = new ConcurrentDictionary<string, string>();
            this.Data = new ConcurrentDictionary<string, object>();
        }

        /// <summary>Gets the namespace of the metric that produces this aggregate.</summary>
        public string MetricNamespace { get; }

        /// <summary>Gets the id (name) of the metric that produced this aggregate.</summary>
        public string MetricId { get; }

        /// <summary>Gets the moniker defining the kind of the aggregation used for the respective metric.</summary>
        public string AggregationKindMoniker { get; }

        /// <summary>Gets or sets the start of the aggregation period summarized by this aggregate.</summary>
        public DateTimeOffset AggregationPeriodStart
        {
            get
            {
            get
            {
                lock (this.updateLock)
                {
                    return this.aggregationPeriodDuration;
                }
            }

            set
            {
                lock (this.updateLock)
                {
                    this.aggregationPeriodDuration = value;
                }
            }
        }

        /// <summary>Gets get table of dimension name-values that specify the data series that produced this agregate within the overall metric.</summary>
        public IDictionary<string, string> Dimensions { get; }

            if (this.Data.TryGetValue(dataKey, out dataValue))
            {
                try
                {
                    T value = (T)Convert.ChangeType(dataValue, typeof(T), CultureInfo.InvariantCulture);
                    return value;
                }
                catch
                {
                    return defaultValue;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
