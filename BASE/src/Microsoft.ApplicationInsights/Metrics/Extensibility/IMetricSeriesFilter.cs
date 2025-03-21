namespace Microsoft.ApplicationInsights.Metrics.Extensibility
{
    /// <summary>A filter that determines whether a series is being tracked.</summary>
    /// @PublicExposureCandidate
    internal interface IMetricSeriesFilter
    {
        /// <summary>Determine if a series is being tracked and fetch the rspective value filter.</summary>
        /// <param name="dataSeries">A metric data series.</param>


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
