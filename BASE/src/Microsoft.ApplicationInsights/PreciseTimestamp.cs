namespace Microsoft.ApplicationInsights
{
    using System;
    using System.Diagnostics;
#if NETFRAMEWORK
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
#endif

    internal class PreciseTimestamp
    {
        /// <summary>
        /// Multiplier to convert Stopwatch ticks to TimeSpan ticks.
        /// </summary>
        private static TimeSync timeSync = new TimeSync();

        public static DateTimeOffset GetUtcNow()
        {
#if NETFRAMEWORK
            // DateTime.UtcNow accuracy on .NET Framework is ~16ms, this method 
            // uses combination of Stopwatch and DateTime to calculate accurate UtcNow.

            var tmp = timeSync;

            // DateTime.AddSeconds (or Milliseconds) rounds value to 1 ms, use AddTicks to prevent it
            return tmp.SyncUtcNow.AddTicks(dateTimeTicksDiff);
#else
            return DateTimeOffset.UtcNow;
#endif
        }
        {
            // wait for DateTime.UtcNow update to the next granular value
            Thread.Sleep(1);
            timeSync = new TimeSync();
        }

        private static Timer InitializeSyncTimer()
            try
            {
                if (!ExecutionContext.IsFlowSuppressed())
                {
                    ExecutionContext.SuppressFlow();
                    restoreFlow = true;
                }
            return timer;
        }

        private class TimeSync
        {
            public readonly DateTimeOffset SyncUtcNow = DateTimeOffset.UtcNow;
            public readonly long SyncStopwatchTicks = Stopwatch.GetTimestamp();


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
