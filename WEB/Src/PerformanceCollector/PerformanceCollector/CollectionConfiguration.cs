namespace Microsoft.ApplicationInsights.Extensibility.Filtering
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers;

    /// <summary>
    /// Represents the collection configuration - a set of calculated metrics, performance counters, and full telemetry documents to be collected by the SDK.
    /// </summary>
    /// <remarks>
    /// This class is a hub for all pieces of configurable collection configuration.
    /// Upon initialization
    ///   - it creates collection-time instances of <see cref="CalculatedMetric&lt;T&gt;"/> and maintains them in separate collections by telemetry type.
    ///     These are used to filter and calculated calculated metrics configured by the service.
    ///   - it creates collection-time instances of <see cref="DocumentStream"/> which are used to filter and send out full telemetry documents.
    ///   - it creates certain metadata collections which are used by other collection-time components to learn more about what is being collected at any given time.
    /// </remarks>
    internal class CollectionConfiguration
    {
        private readonly CollectionConfigurationInfo info;

        #region Collection-time instances used to filter and calculate data on telemetry passing through the pipeline
        private readonly List<CalculatedMetric<RequestTelemetry>> requestTelemetryMetrics = new List<CalculatedMetric<RequestTelemetry>>();

        private readonly List<CalculatedMetric<DependencyTelemetry>> dependencyTelemetryMetrics =
            new List<CalculatedMetric<DependencyTelemetry>>();

        private readonly List<CalculatedMetric<ExceptionTelemetry>> exceptionTelemetryMetrics =
            new List<CalculatedMetric<ExceptionTelemetry>>();

        private readonly List<CalculatedMetric<EventTelemetry>> eventTelemetryMetrics = new List<CalculatedMetric<EventTelemetry>>();

        private readonly List<CalculatedMetric<TraceTelemetry>> traceTelemetryMetrics = new List<CalculatedMetric<TraceTelemetry>>();

        private readonly List<DocumentStream> documentStreams = new List<DocumentStream>();
        #endregion

        #region Metadata used by other components
        private readonly List<Tuple<string, AggregationType>> telemetryMetadata = new List<Tuple<string, AggregationType>>();

        private readonly List<Tuple<string, string>> performanceCounters = new List<Tuple<string, string>>();
           Clock timeProvider,
           IEnumerable<DocumentStream> previousDocumentStreams = null)
        {
            this.info = info ?? throw new ArgumentNullException(nameof(info));

            // create metrics based on descriptions in info
            this.CreateTelemetryMetrics(out CollectionConfigurationError[] metricErrors);

            // maintain a separate collection of all (Id, AggregationType) pairs with some additional data - to allow for uniform access to all types of metrics
            // this includes both telemetry metrics and Metric metrics

            // create performance counters
            this.CreatePerformanceCounters(out CollectionConfigurationError[] performanceCounterErrors);
            
            errors = metricErrors.Concat(documentStreamErrors).Concat(performanceCounterErrors).ToArray();

            foreach (var error in errors)
            {
                error.Data["ETag"] = this.info.ETag;
            }
        }

        public IEnumerable<CalculatedMetric<RequestTelemetry>> RequestMetrics => this.requestTelemetryMetrics;

        public IEnumerable<CalculatedMetric<DependencyTelemetry>> DependencyMetrics => this.dependencyTelemetryMetrics;

        public IEnumerable<CalculatedMetric<ExceptionTelemetry>> ExceptionMetrics => this.exceptionTelemetryMetrics;

        public IEnumerable<CalculatedMetric<EventTelemetry>> EventMetrics => this.eventTelemetryMetrics;

        /// </summary>
        public IEnumerable<Tuple<string, AggregationType>> TelemetryMetadata => this.telemetryMetadata;
        
        /// <summary>
        /// Gets document streams. Telemetry items are provided by QuickPulseTelemetryProcessor.
        /// </summary>
        public IEnumerable<DocumentStream> DocumentStreams => this.documentStreams; 
        
        /// <summary>
        /// Gets a list of performance counters.
        /// </summary>
        /// <remarks>
        /// Performance counter name is stored in CalculatedMetricInfo.Projection.
        /// </remarks>
        public IEnumerable<Tuple<string, string>> PerformanceCounters => this.performanceCounters;
        
        public string ETag => this.info.ETag;

        private static void AddMetric<TTelemetry>(
          CalculatedMetricInfo metricInfo,
                                string.Format(CultureInfo.InvariantCulture, "Failed to create metric {0}.", metricInfo),
                                e,
                                Tuple.Create("MetricId", metricInfo.Id)),
                        }).ToArray();
            }
        }

        private void CreatePerformanceCounters(out CollectionConfigurationError[] errors)
        {
            var errorList = new List<CollectionConfigurationError>();

            IEnumerable<string> duplicateMetricIds =
                performanceCounterMetrics.GroupBy(pc => pc.Id, StringComparer.Ordinal).Where(group => group.Count() > 1).Select(group => group.Key);

            foreach (var duplicateMetricId in duplicateMetricIds)
            {
                errorList.Add(
                    CollectionConfigurationError.CreateError(
                        CollectionConfigurationErrorType.PerformanceCounterDuplicateIds,
                        string.Format(CultureInfo.InvariantCulture, "Duplicate performance counter id '{0}'", duplicateMetricId),
                        Tuple.Create("MetricId", duplicateMetricId)));
            }

            errors = errorList.ToArray();
        }
        
            out CollectionConfigurationError[] errors,
            Clock timeProvider,
            IEnumerable<DocumentStream> previousDocumentStreams)
        {
            var errorList = new List<CollectionConfigurationError>();
            var documentStreamIds = new HashSet<string>();

            // quota might be changing concurrently on the collection thread, but we don't need the exact value at any given time
            // we will try to carry over the last known values to this new configuration
            Dictionary<string, Tuple<float, float, float, float, float>> previousQuotasByStreamId =
                        errorList.Add(
                            CollectionConfigurationError.CreateError(
                                CollectionConfigurationErrorType.DocumentStreamDuplicateIds,
                                string.Format(CultureInfo.InvariantCulture, "Document stream with a duplicate id ignored: {0}", documentStreamInfo.Id),
                                null,
                                Tuple.Create("DocumentStreamId", documentStreamInfo.Id)));

                        continue;
                    }

                        float? initialQuota = this.info.QuotaInfo?.InitialQuota;

                        var documentStream = new DocumentStream(
                            documentStreamInfo,
                            out localErrors,
                            timeProvider,
                            initialRequestQuota: initialQuota ?? previousQuotas?.Item1,
                            initialDependencyQuota: initialQuota ?? previousQuotas?.Item2,
                            initialExceptionQuota: initialQuota ?? previousQuotas?.Item3,
                            initialEventQuota: initialQuota ?? previousQuotas?.Item4,
                            initialTraceQuota: initialQuota ?? previousQuotas?.Item5,
                            maxRequestQuota: maxQuota,
                            maxDependencyQuota: maxQuota,
                            maxExceptionQuota: maxQuota,
                            maxEventQuota: maxQuota,
                            maxTraceQuota: maxQuota,
                            quotaAccrualRatePerSec: quotaAccrualRatePerSec);

                        documentStreamIds.Add(documentStreamInfo.Id);
                        this.documentStreams.Add(documentStream);
                    // there must not be metrics with duplicate ids
                    errorList.Add(
                        CollectionConfigurationError.CreateError(
                            CollectionConfigurationErrorType.MetricDuplicateIds,
                            string.Format(CultureInfo.InvariantCulture, "Metric with a duplicate id ignored: {0}", metricInfo.Id),
                            null,
                            Tuple.Create("MetricId", metricInfo.Id)));

                    continue;
                }
                metricIds.Add(metricInfo.Id);
            }

            errors = errorList.ToArray();
        }

        private void CreateMetadata()
        {
            foreach (var metricIds in
                this.requestTelemetryMetrics.Select(metric => Tuple.Create(metric.Id, metric.AggregationType))


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
