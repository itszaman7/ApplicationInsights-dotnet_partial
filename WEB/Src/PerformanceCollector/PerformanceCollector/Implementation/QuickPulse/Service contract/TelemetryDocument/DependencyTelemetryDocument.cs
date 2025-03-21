namespace Microsoft.ManagementServices.RealTimeDataProcessing.QuickPulseService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [DataContract]
    internal struct DependencyTelemetryDocument : ITelemetryDocument
    {
        [DataMember(EmitDefaultValue = false)]
        public Guid Id { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool? Success { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TimeSpan Duration { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string OperationId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ResultCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string CommandName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string DependencyTypeName { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public KeyValuePair<string, string>[] Properties { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string CloudRoleName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string CloudRoleInstance { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string[] DocumentStreamIds { get; set; }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
