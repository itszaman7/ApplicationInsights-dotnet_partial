namespace Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId
{
    using System;
    using System.Collections.Concurrent;
    using System.Net;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    internal class FailedRequestsManager
    {
        private const int DefaultRetryWaitTimeSeconds = 30;

        // Delay between trying to get Application Id once we get a failure while trying to get it. 
        // This is to throttle tries between failures to safeguard against performance hits. The impact would be that telemetry generated during this interval would not have x-component Application Id.
        private readonly TimeSpan retryWaitTime;

        private ConcurrentDictionary<string, FailedResult> failingInstrumentationKeys = new ConcurrentDictionary<string, FailedResult>();

        internal FailedRequestsManager()
        {
            this.retryWaitTime = TimeSpan.FromSeconds(DefaultRetryWaitTimeSeconds);
        }

            this.failingInstrumentationKeys.TryAdd(instrumentationKey, new FailedResult(this.retryWaitTime, httpStatusCode));

            CoreEventSource.Log.ApplicationIdProviderFetchApplicationIdFailedWithResponseCode(httpStatusCode.ToString());
        }

        /// <summary>
        /// Registers failure for further action in future.
        /// </summary>
        /// <param name="instrumentationKey">Instrumentation Key for which the failure occurred.</param>
        {
            if (ex is AggregateException ae)
            {
                var innerException = ae.Flatten().InnerException;
                if (innerException != null)
                {
                    this.RegisterFetchFailure(instrumentationKey, innerException);
                    return;
                }
            }
            else if (ex is WebException webException && webException.Response != null && webException.Response is HttpWebResponse httpWebResponse)
            {
                this.failingInstrumentationKeys.TryAdd(instrumentationKey, new FailedResult(this.retryWaitTime, httpWebResponse.StatusCode));
            }
            else
            {
                this.failingInstrumentationKeys.TryAdd(instrumentationKey, new FailedResult(this.retryWaitTime));
            }
        }

        public bool CanRetry(string instrumentationKey)
        {
            if (this.failingInstrumentationKeys.TryGetValue(instrumentationKey, out FailedResult lastFailedResult))
            {
                if (lastFailedResult.CanRetry())
                {
                    this.failingInstrumentationKeys.TryRemove(instrumentationKey, out FailedResult value);
        {

            /// <summary>
            /// Initializes a new instance of the <see cref="FailedResult" /> class.
            /// </summary>
            /// <param name="retryAfter">Time to wait before a retry.</param>
            /// <param name="httpStatusCode">Failure response code. Used to determine if we should retry requests.</param>
            public FailedResult(TimeSpan retryAfter, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
            {
                this.retryAfterTime = DateTime.UtcNow + retryAfter;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
