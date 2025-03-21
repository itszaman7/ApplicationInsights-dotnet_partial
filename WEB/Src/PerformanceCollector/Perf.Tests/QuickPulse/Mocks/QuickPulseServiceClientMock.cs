namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.ApplicationInsights.Extensibility.Filtering;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse;

    internal class QuickPulseServiceClientMock : IQuickPulseServiceClient
    {
        public readonly object ResponseLock = new object();

        public volatile bool CountersEnabled = true;

        private readonly object countersLock = new object();

        private List<QuickPulseDataSample> samples = new List<QuickPulseDataSample>();

        private List<int> batches = new List<int>();

        public int PingCount { get; private set; }

        public bool? ReturnValueFromPing { private get; set; }

        public string LastAuthApiKey { get; private set; }

        public string LastPingInstance { get; private set; }

        public bool AlwaysThrow { get; set; } = false;

        public List<QuickPulseDataSample> SnappedSamples
        {
            get
                this.PingCount = 0;
                this.LastSampleBatchSize = null;
                this.LastPingTimestamp = null;
                this.LastPingInstance = string.Empty;

                this.samples.Clear();
            }
        }

        public bool? Ping(
                if (this.CountersEnabled)
                {
                    lock (this.countersLock)
                    {
                        this.PingCount++;
                        this.LastPingTimestamp = timestamp;
                        this.LastAuthApiKey = authApiKey;
                    }
                }

                if (this.AlwaysThrow)
                {
                    throw new InvalidOperationException("Mock is set to always throw");
                }

                configurationInfo = this.CollectionConfigurationInfo?.ETag == configurationETag ? null : this.CollectionConfigurationInfo;
                servicePollingIntervalHint = this.ServicePollingIntervalHint;
                this.CurrentServiceUri = this.CurrentServiceUriMockValue;

                return this.ReturnValueFromPing;
            string configurationETag,
            string authApiKey,
            string authToken,
            out CollectionConfigurationInfo configurationInfo,
            CollectionConfigurationError[] collectionConfigurationErrors)

                if (this.AlwaysThrow)
                {
                    throw new InvalidOperationException("Mock is set to always throw");
                }

                configurationInfo = this.CollectionConfigurationInfo?.ETag == configurationETag ? null : this.CollectionConfigurationInfo;
                this.CollectionConfigurationErrors = collectionConfigurationErrors;

                return this.ReturnValueFromSubmitSample;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
