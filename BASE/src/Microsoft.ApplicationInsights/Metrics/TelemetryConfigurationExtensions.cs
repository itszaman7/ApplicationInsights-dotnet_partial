namespace Microsoft.ApplicationInsights.Metrics
{
    using System;
        /// <returns>The <c>MetricManager</c> instscne assiciated with the specified telemetry pipeline.</returns>
        public static MetricManager GetMetricManager(this TelemetryConfiguration telemetryPipeline)
        {
            return telemetryPipeline?.GetMetricManager(createIfNotExists: true);
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
