namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Metrics;
    using static System.FormattableString;

    /// <summary>
    /// An instance of this class is contained within the <see cref="AutocollectedMetricsExtractor"/> telemetry processor.
    /// It extracts auto-collected, pre-aggregated (aka. "standard") metrics from RequestTelemetry objects which represent
    /// invocations of the monitored service.
    /// </summary>
    internal class RequestMetricsExtractor : ISpecificAutocollectedMetricsExtractor
    {
        /// <summary>
        /// The default value for the <see cref="MaxResponseCodeToDiscover"/> property.        
        /// </summary>
        public const int MaxResponseCodeToDiscoverDefault = 30;

        /// <summary>
        /// The default value for the <see cref="MaxCloudRoleInstanceValuesToDiscover"/> property.
        /// </summary>
        public const int MaxCloudRoleInstanceValuesToDiscoverDefault = 2;

        /// <summary>
        /// The default value for the <see cref="MaxCloudRoleNameValuesToDiscover"/> property.
        /// </summary>
        public const int MaxCloudRoleNameValuesToDiscoverDefault = 2;


        public string ExtractorVersion { get; } = "1.1";

        /// <summary>
        /// Gets or sets the maximum number of auto-discovered Request response code.
        /// </summary>
        public int MaxResponseCodeToDiscover { get; set; } = MaxResponseCodeToDiscoverDefault;

        /// <summary>
        /// Gets or sets the maximum number of auto-discovered Cloud RoleInstance values.
        /// </summary>
        public int MaxCloudRoleInstanceValuesToDiscover { get; set; } = MaxCloudRoleInstanceValuesToDiscoverDefault;

        /// <summary>
        /// Gets or sets the maximum number of auto-discovered Cloud RoleName values.
        /// </summary>
        public int MaxCloudRoleNameValuesToDiscover { get; set; } = MaxCloudRoleNameValuesToDiscoverDefault;

        public void InitializeExtractor(TelemetryClient metricTelemetryClient)
        {
            if (metricTelemetryClient == null)
            {
                this.requestDurationMetric = null;
                return;
            }

            if (!this.isInitialized)
            {
                lock (this.lockObject)
                {
                        config.ApplyDimensionCapping = true;
                        config.DimensionCappedString = MetricTerms.Autocollection.Common.PropertyValues.DimensionCapFallbackValue;

                        IList<string> dimensionNames = new List<string>(this.dimensionExtractors.Count);
                        for (i = 0; i < this.dimensionExtractors.Count; i++)
                        {
                            dimensionNames.Add(this.dimensionExtractors[i].Name);
                        }

                        MetricIdentifier metricIdentifier = new MetricIdentifier(MetricIdentifier.DefaultMetricNamespace,
                                    MetricTerms.Autocollection.Metric.RequestDuration.Name,
                                    dimensionNames);

                        this.requestDurationMetric = metricTelemetryClient.GetMetric(
                                                                    metricIdentifier: metricIdentifier,
                                                                    metricConfiguration: config,
                                                                    aggregationScope: MetricAggregationScope.TelemetryClient);
                        this.isInitialized = true;
                    }
                }
                if (dim.MaxValues == 0)
                {
                    dimValues[i] = MetricTerms.Autocollection.Common.PropertyValues.Other;
                }
                else
                {
                    dimValues[i] = dim.ExtractDimension(request);
                    if (string.IsNullOrEmpty(dimValues[i]))
                    {
                        dimValues[i] = dim.DefaultValue;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
