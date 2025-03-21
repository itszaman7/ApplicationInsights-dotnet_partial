namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers
{
    using System;

    internal class QuickPulseTimings
    {
        public QuickPulseTimings(
            TimeSpan servicePollingInterval,
            TimeSpan servicePollingBackedOffInterval,
            TimeSpan timeToServicePollingBackOff,
            TimeSpan collectionInterval,
            this.ServicePollingInterval = servicePollingInterval;
            this.ServicePollingBackedOffInterval = servicePollingBackedOffInterval;
            this.TimeToServicePollingBackOff = timeToServicePollingBackOff;
            this.CollectionInterval = collectionInterval;
            this.TimeToCollectionBackOff = timeToCollectionBackOff;
            this.CatastrophicFailureTimeout = catastrophicFailuretimeout;
        }

        public QuickPulseTimings(TimeSpan servicePollingInterval, TimeSpan collectionInterval)
        {
            get
            {
                return new QuickPulseTimings(
                    servicePollingInterval: TimeSpan.FromSeconds(5),
                    servicePollingBackedOffInterval: TimeSpan.FromMinutes(1),
                    timeToServicePollingBackOff: TimeSpan.FromMinutes(1),
                    collectionInterval: TimeSpan.FromSeconds(1),
                    timeToCollectionBackOff: TimeSpan.FromSeconds(20),

        public TimeSpan CatastrophicFailureTimeout { get; private set; }
    }
}

# This file contains partial code from the original project
# Some functionality may be missing or incomplete
