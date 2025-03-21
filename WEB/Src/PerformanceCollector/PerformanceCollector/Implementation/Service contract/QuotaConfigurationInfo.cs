namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.ServiceContract
{
    using System.Collections.Generic;
    using System.Linq;

    [DataContract]
    internal class QuotaConfigurationInfo
    {
        [DataMember(IsRequired = false)]
        public float? InitialQuota { get; set; }

        [DataMember(IsRequired = true)]


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
