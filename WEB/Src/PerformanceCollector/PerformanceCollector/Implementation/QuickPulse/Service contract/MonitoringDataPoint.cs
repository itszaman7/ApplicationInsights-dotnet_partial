namespace Microsoft.ManagementServices.RealTimeDataProcessing.QuickPulseService
{
    using System;
    using System.Runtime.Serialization;

    using Microsoft.ApplicationInsights.Extensibility.Filtering;

    [KnownType(typeof(ExceptionTelemetryDocument))]
    [KnownType(typeof(EventTelemetryDocument))]
    [KnownType(typeof(TraceTelemetryDocument))]
    internal struct MonitoringDataPoint
         *      adding ProcessorCount
         * 3 - adding TopCpuProcesses
        */
        public const int CurrentInvariantVersion = 5;

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public int InvariantVersion { get; set; }

        [DataMember]
        public string StreamId { get; set; }

        [DataMember]
        public string MachineName { get; set; }

        [DataMember]

        [DataMember]
        public MetricPoint[] Metrics { get; set; }

        [DataMember]
        public ITelemetryDocument[] Documents { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool GlobalDocumentQuotaReached { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ProcessCpuData[] TopCpuProcesses { get; set; }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
