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
    /// It extracts auto-collected, pre-aggregated metrics from TraceTelemetry objects which represent
    /// count of Trace telemetry tracked in this service.
    /// </summary>
    internal class TraceMetricsExtractor : ISpecificAutocollectedMetricsExtractor
    {
        /// <summary>
        /// The default value for the <see cref="MaxCloudRoleInstanceValuesToDiscover"/> property.
        /// </summary>
        public const int MaxCloudRoleInstanceValuesToDiscoverDefault = 2;

        /// <summary>
        /// The default value for the <see cref="MaxCloudRoleNameValuesToDiscover"/> property.
        /// </summary>
        public const int MaxCloudRoleNameValuesToDiscoverDefault = 2;

        private bool isInitialized = false;
        private List<IDimensionExtractor> dimensionExtractors = new List<IDimensionExtractor>();

        public TraceMetricsExtractor()
        {
        }

        public string ExtractorName { get; } = "Traces";

        public string ExtractorVersion { get; } = "1.1";
        /// Gets or sets the maximum number of auto-discovered Cloud RoleInstance values.
        /// </summary>
        public int MaxCloudRoleInstanceValuesToDiscover { get; set; } = MaxCloudRoleInstanceValuesToDiscoverDefault;

        /// <summary>
        /// Gets or sets the maximum number of auto-discovered Cloud RoleName values.
        /// </summary>
        public int MaxCloudRoleNameValuesToDiscover { get; set; } = MaxCloudRoleNameValuesToDiscoverDefault;

        public void InitializeExtractor(TelemetryClient metricTelemetryClient)
                            dimLimit = dim.MaxValues == 0 ? 1 : dim.MaxValues;

                            seriesCountLimit = seriesCountLimit * (1 + dimLimit);
                            valuesPerDimensionLimit[i++] = dimLimit;
                        }

                        MetricConfiguration config = new MetricConfigurationForMeasurement(
                                                                        seriesCountLimit,
                                                                        valuesPerDimensionLimit,
                                                                        new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));
                }
            }            
        }

        public void ExtractMetrics(ITelemetry fromItem, out bool isItemProcessed)
        {
            TraceTelemetry trace = fromItem as TraceTelemetry;
            if (trace == null)
            {
                isItemProcessed = false;
            {
                var dim = this.dimensionExtractors[i];
                if (dim.MaxValues == 0)
                {
                    dimValues[i] = MetricTerms.Autocollection.Common.PropertyValues.Other;
                }
                else
                {
                    dimValues[i] = dim.ExtractDimension(trace);
                    if (string.IsNullOrEmpty(dimValues[i]))
                        dimValues[i] = dim.DefaultValue;
                    }
                }
            }

            CommonHelper.TrackValueHelper(this.traceCountMetric, 1, dimValues);
            isItemProcessed = true;
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
