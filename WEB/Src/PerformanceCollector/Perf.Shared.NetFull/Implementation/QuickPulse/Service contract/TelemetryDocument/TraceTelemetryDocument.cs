namespace Microsoft.ManagementServices.RealTimeDataProcessing.QuickPulseService
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    internal struct TraceTelemetryDocument : ITelemetryDocument
    {
        [DataMember(EmitDefaultValue = false)]
        public Guid Id { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Version { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public DateTimeOffset Timestamp { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [DataMember(EmitDefaultValue = false)]
        public string SeverityLevel { get; set; }

        [DataMember(EmitDefaultValue = false)]
        {
            get
            {
                return TelemetryDocumentType.Trace.ToString();
            }
        }

        [DataMember(EmitDefaultValue = false)]


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
