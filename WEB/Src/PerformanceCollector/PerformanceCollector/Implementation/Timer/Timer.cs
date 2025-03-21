namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.Timer
{
    using System;
    using System.Threading;

    /// <summary>The timer implementation.</summary>
    internal class Timer : ITimer, IDisposable
    {
        #region Constants and Fields

        /// <summary>The timer.</summary>
        private System.Threading.Timer timer;

        #endregion

        public Timer(TimerCallback callback)
        {
            this.timer = new System.Threading.Timer(callback, null, Timeout.Infinite, Timeout.Infinite);
        }


        #region Public Methods and Operators

        /// <summary>Changes the timer's parameters.</summary>
        /// <param name="dueTime">The due time.</param>
        public void ScheduleNextTick(TimeSpan dueTime)
        {
            this.timer.Change(dueTime, Timeout.InfiniteTimeSpan);
            this.timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        #endregion

        {
            if (disposing)
            {
                if (this.timer != null)
                {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
