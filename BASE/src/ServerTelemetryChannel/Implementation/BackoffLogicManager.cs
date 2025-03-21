namespace Microsoft.ApplicationInsights.Channel.Implementation
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;

    internal class BackoffLogicManager
    {
        internal const int SlotDelayInSeconds = 10;

        private const int MaxDelayInSeconds = 3600;
        private const int DefaultBackoffEnabledReportingIntervalInMin = 30;

        private static readonly Random Random = new Random();
        private static readonly DataContractJsonSerializer Serializer = new DataContractJsonSerializer(typeof(BackendResponse));

        private readonly object lockConsecutiveErrors = new object();
        private readonly TimeSpan minIntervalToUpdateConsecutiveErrors;
        
        private bool exponentialBackoffReported = false;
        private int consecutiveErrors;
        private DateTimeOffset nextMinTimeToUpdateConsecutiveErrors = DateTimeOffset.MinValue;

        public BackoffLogicManager()
        {
            this.DefaultBackoffEnabledReportingInterval = TimeSpan.FromMinutes(DefaultBackoffEnabledReportingIntervalInMin);
            this.minIntervalToUpdateConsecutiveErrors = TimeSpan.FromSeconds(SlotDelayInSeconds);
            this.CurrentDelay = TimeSpan.FromSeconds(SlotDelayInSeconds);
        }

        public BackoffLogicManager(TimeSpan defaultBackoffEnabledReportingInterval)
        {
            this.DefaultBackoffEnabledReportingInterval = defaultBackoffEnabledReportingInterval;
            this.minIntervalToUpdateConsecutiveErrors = TimeSpan.FromSeconds(SlotDelayInSeconds);
            this.CurrentDelay = TimeSpan.FromSeconds(SlotDelayInSeconds);
        }

        internal BackoffLogicManager(TimeSpan defaultBackoffEnabledReportingInterval, TimeSpan minIntervalToUpdateConsecutiveErrors) 
        }

        /// <summary>
        /// Gets the last status code SDK received from the backend.
        /// </summary>
        public int LastStatusCode { get; private set; }

        public TimeSpan DefaultBackoffEnabledReportingInterval { get; set; }

        internal TimeSpan CurrentDelay { get; private set; }
                backendResponse = null;
        {
            lock (this.lockConsecutiveErrors)
            {
                this.consecutiveErrors = 0;
            }
        }

        public void ReportBackoffEnabled(int statusCode)
        {
            this.LastStatusCode = statusCode;
            lock (this.lockConsecutiveErrors)
            {
                // Do not increase number of errors more often than minimum interval (SlotDelayInSeconds) 
                // since we have 3 senders and all of them most likely would fail if we have intermittent error  
                if (DateTimeOffset.UtcNow > this.nextMinTimeToUpdateConsecutiveErrors)
                {
                    this.consecutiveErrors++;
                    this.nextMinTimeToUpdateConsecutiveErrors = DateTimeOffset.UtcNow + this.minIntervalToUpdateConsecutiveErrors;
                }
            }

        public void ReportBackoffDisabled()
        {
            this.LastStatusCode = 200;

            if (this.exponentialBackoffReported)
            {
                TelemetryChannelEventSource.Log.BackoffDisabled();
                this.exponentialBackoffReported = false;
            }
        protected virtual TimeSpan GetBackOffTime(string headerValue)
        {
            if (!TryParseRetryAfter(headerValue, out TimeSpan retryAfterTimeSpan))
            {
                double delayInSeconds;

                if (this.ConsecutiveErrors <= 1)
                {
                    delayInSeconds = SlotDelayInSeconds;
                }
        {
            retryAfterTimeSpan = TimeSpan.FromSeconds(0);

            if (string.IsNullOrEmpty(retryAfter))
            {
                return false;
            }

            TelemetryChannelEventSource.Log.RetryAfterHeaderIsPresent(retryAfter);


                return false;
            }

            TelemetryChannelEventSource.Log.TransmissionPolicyRetryAfterParseFailedWarning(retryAfter);

            return false;
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
