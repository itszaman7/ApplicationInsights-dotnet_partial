namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using Microsoft.ApplicationInsights.Channel;    
    internal class TraceMetricIdDimensionExtractor : IDimensionExtractor
    {

        public string DefaultValue { get; set; } = MetricTerms.Autocollection.Metric.TraceCount.Id;

        public string Name { get; set; } = MetricDimensionNames.TelemetryContext.Property(MetricTerms.Autocollection.MetricId.Moniker.Key);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
