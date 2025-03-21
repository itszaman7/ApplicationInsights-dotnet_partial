namespace Microsoft.ApplicationInsights.Common
{
    using System;
    using System.Threading;

    /// <summary>
    internal class InterlockedThrottle
    {

        /// <summary>
        /// <summary>
        /// Will execute the action only if the time period has elapsed.
        /// </summary>
        /// <param name="action">Action to be executed.</param>
        public void PerformThrottledAction(Action action)
        {
            if (now.Ticks > Interlocked.Read(ref this.timeStamp))
            {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
