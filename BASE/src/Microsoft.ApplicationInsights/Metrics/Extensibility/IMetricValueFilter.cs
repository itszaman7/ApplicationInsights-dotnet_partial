namespace Microsoft.ApplicationInsights.Metrics.Extensibility
{
        /// <summary>Determine whether a value will be tracked or ignored while aggregating a metric data time series.</summary>
        /// <param name="dataSeries">A metric data time series.</param>
        /// <returns>Whether or not a value will be tracked or ignored while aggregating a metric data time series.</returns>
        bool WillConsume(MetricSeries dataSeries, double metricValue);

        /// <param name="dataSeries">A metric data time series.</param>
        /// <param name="metricValue">A metric value.</param>


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
