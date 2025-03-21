namespace Microsoft.ManagementServices.RealTimeDataProcessing.QuickPulseService
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    internal struct RequestTelemetryDocument : ITelemetryDocument
    {
        [DataMember(EmitDefaultValue = false)]
        public Guid Id { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Version { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTimeOffset Timestamp { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string OperationId { get; set; }
        public bool? Success { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TimeSpan Duration { get; set; }

        [DataMember(EmitDefaultValue = false)]
        {
            get
            {
                return TelemetryDocumentType.Request.ToString();
            }

            private set
            {
            }
        }

        [DataMember(EmitDefaultValue = false)]


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
