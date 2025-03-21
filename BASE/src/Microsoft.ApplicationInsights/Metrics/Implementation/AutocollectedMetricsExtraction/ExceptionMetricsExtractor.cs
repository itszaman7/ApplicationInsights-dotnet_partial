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
    /// It extracts auto-collected, pre-aggregated metrics from ExceptionTelemetry objects which represent
    /// count of Exceptions tracked in this service.
    /// </summary>
    internal class ExceptionMetricsExtractor : ISpecificAutocollectedMetricsExtractor
    {
        /// <summary>
        /// The default value for the <see cref="MaxCloudRoleInstanceValuesToDiscover"/> property.
        /// </summary>
        public const int MaxCloudRoleInstanceValuesToDiscoverDefault = 2;

        /// <summary>
        /// The default value for the <see cref="MaxCloudRoleNameValuesToDiscover"/> property.
        /// </summary>
        public const int MaxCloudRoleNameValuesToDiscoverDefault = 2;

        private readonly object lockObject = new object();

        /// <summary>
        /// Extracted metric.
        /// </summary>
        private Metric exceptionServerMetric = null;

        public string ExtractorName { get; } = "Exceptions";

        public string ExtractorVersion { get; } = "1.1";

        /// <summary>
        /// Gets or sets the maximum number of auto-discovered Cloud RoleInstance values.
        /// </summary>
        public int MaxCloudRoleInstanceValuesToDiscover { get; set; } = MaxCloudRoleInstanceValuesToDiscoverDefault;

            {
                lock (this.lockObject)
                            valuesPerDimensionLimit[i++] = dimLimit;
                        }

                        MetricConfiguration config = new MetricConfigurationForMeasurement(
                                                                        seriesCountLimit,
                                                                        valuesPerDimensionLimit,
                                                                        new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));
                        config.ApplyDimensionCapping = true;
                        config.DimensionCappedString = MetricTerms.Autocollection.Common.PropertyValues.DimensionCapFallbackValue;

                            dimensionNames.Add(this.dimensionExtractors[i].Name);
                        }

                        MetricIdentifier metricIdentifier = new MetricIdentifier(MetricIdentifier.DefaultMetricNamespace,
                                    MetricTerms.Autocollection.Metric.ExceptionCount.Name,
                                    dimensionNames);

                        this.exceptionServerMetric = metricTelemetryClient.GetMetric(
                                                                    metricIdentifier: metricIdentifier,
                                                                    metricConfiguration: config,
            if (this.exceptionServerMetric == null)
            {
                //// This should be caught and properly logged by the base class:
                throw new InvalidOperationException(Invariant($"Cannot execute {nameof(this.ExtractMetrics)}.")
                                                  + Invariant($" There is no {nameof(this.exceptionServerMetric)}.")
                                                  + Invariant($" Either this metrics extractor has not been initialized, or it has been disposed."));
            }

            string[] dimValues = new string[this.dimensionExtractors.Count];
            
                    dimValues[i] = MetricTerms.Autocollection.Common.PropertyValues.Other;
                }
                else
                {
                    dimValues[i] = dim.ExtractDimension(exception);
                    if (string.IsNullOrEmpty(dimValues[i]))
                    {
                        dimValues[i] = dim.DefaultValue;
                    }
                }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
