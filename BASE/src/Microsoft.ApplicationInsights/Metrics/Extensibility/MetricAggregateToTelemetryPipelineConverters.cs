namespace Microsoft.ApplicationInsights.Metrics.Extensibility
{
    using System;
    using System.Collections.Concurrent;

    /// <summary>A registry for injecting converters from <c>MetricAggregate</c> items to data exchange
    /// types employed by the respective data ingestion/processing/sink mechanism. </summary>
    /// @PublicExposureCandidate
    internal sealed class MetricAggregateToTelemetryPipelineConverters 
    {
        /// <summary>Default singelton.</summary>
        public static readonly MetricAggregateToTelemetryPipelineConverters Registry = new MetricAggregateToTelemetryPipelineConverters();

        /// <param name="aggregationKindMoniker">Aggregation kind moniker.</param>
        /// <param name="converter">The converter being registered.</param>
        public void Add(Type pipelineType, string aggregationKindMoniker, IMetricAggregateToTelemetryPipelineConverter converter)
        {
            Util.ValidateNotNull(converter, nameof(converter));

            ConcurrentDictionary<string, IMetricAggregateToTelemetryPipelineConverter> converters = this.pipelineTable.GetOrAdd(
                                                                                pipelineType,
        }

        /// <summary>Attempts to get a metric aggregate converter from the registry.</summary>

            ConcurrentDictionary<string, IMetricAggregateToTelemetryPipelineConverter> converters;
            if (false == this.pipelineTable.TryGetValue(pipelineType, out converters))
            {

            bool hasConverter = converters.TryGetValue(aggregationKindMoniker, out converter);
            return hasConverter;
        }
            ////                              + $", but it specifies the type '{pipelineType.Name}' that does not implement that interface.");
            ////}

            Util.ValidateNotNullOrWhitespace(aggregationKindMoniker, nameof(aggregationKindMoniker));


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
