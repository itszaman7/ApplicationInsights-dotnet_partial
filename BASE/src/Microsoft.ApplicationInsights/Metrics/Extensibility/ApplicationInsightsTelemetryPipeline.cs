namespace Microsoft.ApplicationInsights.Metrics.Extensibility
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using static System.FormattableString;

    /// <summary>An adapter that represents the Application Insights SDK pipelie towards the Metrics Aggregation SDK subsystem.</summary>
            Util.ValidateNotNull(telemetryPipeline, nameof(telemetryPipeline));

            this.trackingClient = new ApplicationInsights.TelemetryClient(telemetryPipeline);
        }

        /// <summary>Creaes a new Application Insights telemetry pipeline adapter.</summary>
        /// <param name="telemetryClient">The Application Insights telemetry pipeline to be adapted.</param>
        {
            Util.ValidateNotNull(metricAggregate, nameof(metricAggregate));
            Util.ValidateNotNull(metricAggregate.AggregationKindMoniker, nameof(metricAggregate.AggregationKindMoniker));

            cancelToken.ThrowIfCancellationRequested();

            IMetricAggregateToTelemetryPipelineConverter converter;
            bool hasConverter = MetricAggregateToTelemetryPipelineConverters.Registry.TryGet(
                                                                                            typeof(ApplicationInsightsTelemetryPipeline),
                                                                                            metricAggregate.AggregationKindMoniker,
                                                                                            out converter);
            if (false == hasConverter)
            {
                throw new ArgumentException(Invariant($"Cannot track the specified {metricAggregate}, because there is no {nameof(IMetricAggregateToTelemetryPipelineConverter)}")
                                          + Invariant($" registered for it. A converter must be added to {nameof(MetricAggregateToTelemetryPipelineConverters)}")
                                          + Invariant($".{nameof(MetricAggregateToTelemetryPipelineConverters.Registry)} for the pipeline type")
                                          + Invariant($" '{metricAggregate.AggregationKindMoniker}'."));
            }

            object telemetryItem = converter.Convert(metricAggregate);
            var metricTelemetryItem = (ApplicationInsights.DataContracts.MetricTelemetry)telemetryItem;
            this.trackingClient.Track(metricTelemetryItem);

            return this.completedTask;
        }

        /// <summary>Flushes the Application Insights pipeline used by this adaptor.</summary>
        /// <param name="cancelToken">Cancellation is not supported by the underlying pipeline, but it is respected be this method.</param>
        /// <returns>The task representing the Flush operation.</returns>
        public Task FlushAsync(CancellationToken cancelToken)


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
