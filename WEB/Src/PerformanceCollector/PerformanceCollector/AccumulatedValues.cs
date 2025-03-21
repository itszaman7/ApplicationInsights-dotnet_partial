namespace Microsoft.ApplicationInsights.Extensibility.Filtering
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Threading;

    /// <summary>
    /// Accumulator for calculated metrics.
    /// </summary>
    internal class AccumulatedValues
    {
        private readonly AggregationType aggregationType;

        public AccumulatedValues(string metricId, AggregationType aggregationType)
        {
            this.MetricId = metricId;
            this.aggregationType = aggregationType;
        }

        public string MetricId { get; }

        public void AddValue(double value)
            bool lockTaken = false;
            try
            {
                this.spinLock.Enter(ref lockTaken);

                switch (this.aggregationType)
                {
                    case AggregationType.Avg:
                    case AggregationType.Sum:
                        this.sum += value;
                        break;
                    case AggregationType.Min:
                        if (value < this.min)
                        {
                            this.min = value;
                        }

                        break;
                }
        {
            bool lockTaken = false;
            try
            {
                this.spinLock.Enter(ref lockTaken);

                count = this.count;
                switch (this.aggregationType)
                {
                    default:
                        throw new ArgumentOutOfRangeException(
                            nameof(this.aggregationType),
                            this.aggregationType,
                            string.Format(CultureInfo.InvariantCulture, "AggregationType is not supported: {0}", this.aggregationType));
                }
            }
            finally
            {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
