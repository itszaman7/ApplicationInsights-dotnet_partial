namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Microsoft.ApplicationInsights.Extensibility.Filtering;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers;
    using Microsoft.ManagementServices.RealTimeDataProcessing.QuickPulseService;

    /// <summary>
    /// DTO containing data that we send to QPS.
    /// </summary>
    internal class QuickPulseDataSample
    {
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Argument exceptions are valid.")]
        public QuickPulseDataSample(QuickPulseDataAccumulator accumulator, IDictionary<string, Tuple<PerformanceCounterData, double>> perfData, IEnumerable<Tuple<string, int>> topCpuData, bool topCpuDataAccessDenied)
        {
            // NOTE: it is crucial not to keep any heap references on input parameters, new objects with separate roots must be created!
            // CollectionConfiguration is an exception to this rule as it may be still processed, and the sender will check the reference count before sending it out
            if (accumulator == null)
            {
                throw new ArgumentNullException(nameof(accumulator));
            }

            if (perfData == null)
            {
                throw new ArgumentNullException(nameof(perfData));
            }

            if (accumulator.StartTimestamp == null)
            {
                throw new ArgumentNullException(nameof(accumulator.StartTimestamp));
            }
            long requestDurationInTicks = requestCountAndDuration.Item2;

            this.AIRequests = (int)requestCount;
            this.AIRequestsPerSecond = sampleDuration.TotalSeconds > 0 ? requestCount / sampleDuration.TotalSeconds : 0;
            this.AIRequestDurationAveInMs = requestCount > 0 ? (double)requestDurationInTicks / TimeSpan.TicksPerMillisecond / requestCount : 0;
            this.AIRequestsFailedPerSecond = sampleDuration.TotalSeconds > 0 ? accumulator.AIRequestFailureCount / sampleDuration.TotalSeconds : 0;
            this.AIRequestsSucceededPerSecond = sampleDuration.TotalSeconds > 0 ? accumulator.AIRequestSuccessCount / sampleDuration.TotalSeconds : 0;

            Tuple<long, long> dependencyCountAndDuration = QuickPulseDataAccumulator.DecodeCountAndDuration(accumulator.AIDependencyCallCountAndDurationInTicks);
            long dependencyCount = dependencyCountAndDuration.Item1;
            long dependencyDurationInTicks = dependencyCountAndDuration.Item2;

            this.AIDependencyCalls = (int)dependencyCount;
            this.AIDependencyCallsPerSecond = sampleDuration.TotalSeconds > 0 ? dependencyCount / sampleDuration.TotalSeconds : 0;
            this.AIDependencyCallDurationAveInMs = dependencyCount > 0 ? (double)dependencyDurationInTicks / TimeSpan.TicksPerMillisecond / dependencyCount : 0;
            this.AIDependencyCallsFailedPerSecond = sampleDuration.TotalSeconds > 0 ? accumulator.AIDependencyCallFailureCount / sampleDuration.TotalSeconds : 0;
            this.AIDependencyCallsSucceededPerSecond = sampleDuration.TotalSeconds > 0 ? accumulator.AIDependencyCallSuccessCount / sampleDuration.TotalSeconds : 0;

            this.AIExceptionsPerSecond = sampleDuration.TotalSeconds > 0 ? accumulator.AIExceptionCount / sampleDuration.TotalSeconds : 0;

            this.GlobalDocumentQuotaReached = accumulator.GlobalDocumentQuotaReached;

            try
            {
                this.PerfCountersLookup =
                    perfData.ToDictionary(
                        p =>
                        (QuickPulseDefaults.DefaultCounterOriginalStringMapping.ContainsKey(p.Value.Item1.ReportAs)
                             ? QuickPulseDefaults.DefaultCounterOriginalStringMapping[p.Value.Item1.ReportAs]
                             : p.Value.Item1.ReportAs),
                        p => p.Value.Item2);
            }
            catch (Exception e)

        public double AIDependencyCallsPerSecond { get; private set; }

        public double AIDependencyCallDurationAveInMs { get; private set; }

        public double AIDependencyCallsFailedPerSecond { get; private set; }

        public double AIDependencyCallsSucceededPerSecond { get; private set; }

        public double AIExceptionsPerSecond { get; private set; }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
