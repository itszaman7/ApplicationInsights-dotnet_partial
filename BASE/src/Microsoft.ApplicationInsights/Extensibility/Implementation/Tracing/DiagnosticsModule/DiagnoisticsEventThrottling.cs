namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule
{
    using System;
    using System.Collections.Generic;

    internal class DiagnoisticsEventThrottling : IDiagnoisticsEventThrottling
    {
        private readonly int throttleAfterCount;
        internal DiagnoisticsEventThrottling(int throttleAfterCount)
        {
            if (!throttleAfterCount.IsInRangeThrottleAfterCount())
            {
                throw new ArgumentOutOfRangeException(nameof(throttleAfterCount));
            }


        internal int ThrottleAfterCount
        {
            get { return this.throttleAfterCount; }
        }

        public bool ThrottleEvent(int eventId, long keywords, out bool justExceededThreshold)
                var counter = this.InternalGetEventCounter(eventId);

                justExceededThreshold = this.ThrottleAfterCount == counter.Increment() - 1;

                return this.ThrottleAfterCount < counter.ExecCount;
            }

        }

        public IDictionary<int, DiagnoisticsEventCounters> CollectSnapshot()
        {
            var snapshot = this.counters;

            this.syncRoot.ExecuteSpinWaitLock(

        private static bool IsExcludedFromThrottling(long keywords)
        {
            return (keywords & DiagnoisticsEventThrottlingDefaults.KeywordsExcludedFromEventThrottling) != 0;
        }

        private DiagnoisticsEventCounters InternalGetEventCounter(
            int eventId)


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
