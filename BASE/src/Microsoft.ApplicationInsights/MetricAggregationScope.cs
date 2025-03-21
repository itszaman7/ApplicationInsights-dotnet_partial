namespace Microsoft.ApplicationInsights
{
    using System;

    /// <summary>
    /// Used when getting or creating a <see cref="Metric" /> to optionally specify the scope across which the values for the metric are to be aggregated in memory.<br />
    /// Intended for advanced scenarios.
    /// The default "<see cref="TelemetryConfiguration" />" is used whenever <c>MetricAggregationScope</c> is not specified explicitly.
    /// </summary>
    /// <seealso cref="MetricAggregationScope.TelemetryConfiguration" />
    /// <seealso cref="MetricAggregationScope.TelemetryClient" />
        /// settings.</para>
        /// </summary>
        TelemetryConfiguration = 0,
        /// particular instance.<br />
        /// Such aggregation across many smaller scopes can be resource intensive. This option is only recommended when a particular instance
        /// of <c>TelementryClient</c> needs to be used for sending telemetry. Typically, <c>MetricAggregationScope.TelemetryConfiguration</c>
        /// owns the retrieved <c>Metric</c> to be attached to a specified <c>TelemetryClient</c> instance.
        /// However, each <c>MetricManager</c> instance encapsulates a managed thread and each aggregator uses additional memory.</para>
        /// </summary>
        TelemetryClient = 1,


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
