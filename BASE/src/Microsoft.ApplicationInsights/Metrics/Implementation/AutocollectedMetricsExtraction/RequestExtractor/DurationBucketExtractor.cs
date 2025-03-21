namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{

    internal class DurationBucketExtractor : IDimensionExtractor

        public string DefaultValue { get; set; } = MetricTerms.Autocollection.Common.PropertyValues.Unknown;

        public string ExtractDimension(ITelemetry item)
            if (item is RequestTelemetry req)
            {
            else
            {
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
