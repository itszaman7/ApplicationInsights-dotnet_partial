namespace Microsoft.ManagementServices.RealTimeDataProcessing.QuickPulseService
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    internal struct ExceptionTelemetryDocument : ITelemetryDocument
    {
        [DataMember(EmitDefaultValue = false)]
        public Guid Id { get; set; }

        public string Version { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string SeverityLevel { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string Exception { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ExceptionType { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string CloudRoleName { get; set; }
            get
            {
                return TelemetryDocumentType.Exception.ToString();
            }


        [DataMember(EmitDefaultValue = false)]
        public string[] DocumentStreamIds { get; set; }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
