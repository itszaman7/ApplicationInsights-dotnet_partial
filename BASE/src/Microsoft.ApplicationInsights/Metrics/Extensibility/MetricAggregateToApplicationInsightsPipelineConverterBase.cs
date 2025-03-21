namespace Microsoft.ApplicationInsights.Metrics.Extensibility
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.ApplicationInsights.DataContracts;

    using static System.FormattableString;

    /// <summary>Converts the Metrics-Aggregation-SDK exchange type for aggregates (<c>MetricAggregate</c>) to
    /// the Application Insights exchange type for the same (<c>MetricTelemetry</c>). This abstract base class provides
    /// common functionality between aggregates for different aggregation kinds.
    /// </summary>
    /// @PublicExposureCandidate
    internal abstract class MetricAggregateToApplicationInsightsPipelineConverterBase : IMetricAggregateToTelemetryPipelineConverter
    {
        /// <summary>Property name for storing the aggregation interval length.</summary>
        public const string AggregationIntervalMonikerPropertyKey = "_MS.AggregationIntervalMs";

        /// <summary>Gets the name for the aggregation kind sopported by this converter (e.g. <c>Microsoft.Azure.Measurement</c>).</summary>
        public abstract string AggregationKindMoniker { get; }

        /// <summary>Converts a <c>Microsoft.ApplicationInsights.Metrics.MetricAggregate</c> to
        /// a <c>Microsoft.ApplicationInsights.DataContracts.MetricTelemetry</c>. </summary>
        /// <param name="aggregate">A metric aggregate.</param>
        /// <returns>A metric telemetry item representing the aggregate.</returns>
        public object Convert(MetricAggregate aggregate)
        {
            this.ValidateAggregate(aggregate);

            MetricTelemetry telemetryItem = this.ConvertAggregateToTelemetry(aggregate);
            return telemetryItem;
        }

        /// <summary>Subclasses need to override this method to actually send the metric telemetry item's properties
        /// based on the cntents of the aggregate and the aggregation kind.</summary>
        /// <param name="telemetryItem">A metric telemetry item representing the aggregate.</param>
        /// <param name="aggregate">A metric aggregate.</param>
        protected abstract void PopulateDataValues(MetricTelemetry telemetryItem, MetricAggregate aggregate);

        private static void PopulateTelemetryContext(
                                                IDictionary<string, string> dimensions,
                                                Microsoft.ApplicationInsights.DataContracts.TelemetryContext telemetryContext,
                                                out IEnumerable<KeyValuePair<string, string>> nonContextDimensions)
        {
            if (dimensions == null)
                }

                switch (dimension.Key)
                {
                    case MetricDimensionNames.TelemetryContext.InstrumentationKey:
                        telemetryContext.InstrumentationKey = dimension.Value;
                        break;

                    case MetricDimensionNames.TelemetryContext.Cloud.RoleInstance:
                        telemetryContext.Cloud.RoleInstance = dimension.Value;
                        break;

                    case MetricDimensionNames.TelemetryContext.Cloud.RoleName:

                    case MetricDimensionNames.TelemetryContext.Device.Id:
                        telemetryContext.Device.Id = dimension.Value;
                        break;

                    #pragma warning disable CS0618  // Type or member is obsolete
                    case MetricDimensionNames.TelemetryContext.Device.Language:
                        telemetryContext.Device.Language = dimension.Value;
                        break;
                    #pragma warning restore CS0618  // Type or member is obsolete

                    case MetricDimensionNames.TelemetryContext.Device.Model:
                        telemetryContext.Device.Model = dimension.Value;
                        break;

                    #pragma warning disable CS0618  // Type or member is obsolete
                    case MetricDimensionNames.TelemetryContext.Device.NetworkType:
                        telemetryContext.Device.NetworkType = dimension.Value;
                        break;
                    #pragma warning restore CS0618  // Type or member is obsolete

                    case MetricDimensionNames.TelemetryContext.Device.OemName:
                        telemetryContext.Device.OemName = dimension.Value;
                        break;

                    case MetricDimensionNames.TelemetryContext.Device.OperatingSystem:
                        telemetryContext.Device.OperatingSystem = dimension.Value;
                        break;

                    #pragma warning disable CS0618  // Type or member is obsolete

                    case MetricDimensionNames.TelemetryContext.Operation.CorrelationVector:
                        telemetryContext.Operation.CorrelationVector = dimension.Value;
                        break;

                    case MetricDimensionNames.TelemetryContext.Operation.Id:
                        telemetryContext.Operation.Id = dimension.Value;
                        break;

                    case MetricDimensionNames.TelemetryContext.Operation.Name:

                    case MetricDimensionNames.TelemetryContext.Session.IsFirst:
                        try
                        {
                            telemetryContext.Session.IsFirst = System.Convert.ToBoolean(dimension.Value, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            try
                            {
                        telemetryContext.User.UserAgent = dimension.Value;
                        break;

                    default:
                        string dimensionName;
                        if (MetricDimensionNames.TelemetryContext.IsProperty(dimension.Key, out dimensionName))
                        {
                            telemetryContext.GlobalProperties[dimensionName] = dimension.Value;
                        }
                        else
            IDictionary<string, string> props = telemetryItem.Properties;
            if (props != null)
            {
                long periodMillis = (long)aggregate.AggregationPeriodDuration.TotalMilliseconds;
                props.Add(AggregationIntervalMonikerPropertyKey, periodMillis.ToString(CultureInfo.InvariantCulture));
            }

            telemetryItem.Timestamp = aggregate.AggregationPeriodStart;

            // Populate TelemetryContext:
            IEnumerable<KeyValuePair<string, string>> nonContextDimensions;
            PopulateTelemetryContext(aggregate.Dimensions, telemetryItem.Context, out nonContextDimensions);

            // Set dimensions. We do this after the context, becasue dimensions take precedence (i.e. we potentially overwrite):
            if (nonContextDimensions != null)
            {
                foreach (KeyValuePair<string, string> nonContextDimension in nonContextDimensions)
                {
                    telemetryItem.Properties[nonContextDimension.Key] = nonContextDimension.Value;
                }
            }

            // Set SDK version moniker:

            Util.StampSdkVersionToContext(telemetryItem);

            return telemetryItem;
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
