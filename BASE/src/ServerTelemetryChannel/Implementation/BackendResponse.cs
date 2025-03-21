namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System.Runtime.Serialization;
        [DataMember(Name = "itemsReceived", IsRequired=true)]
        public int ItemsReceived { get; set; }

        [DataMember(Name = "itemsAccepted", IsRequired = true)]


        [DataContract]
            public int Index { get; set; }

            [DataMember(Name = "statusCode")]
            public int StatusCode { get; set; }
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
