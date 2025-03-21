namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.XPlatform
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// Represents value of CPU Utilization by Process counter value.
    /// </summary>
    internal class XPlatProcessCPUPerformanceCounter : ICounterValue
    {
        private double lastCollectedValue = 0;
        private DateTimeOffset lastCollectedTime = DateTimeOffset.MinValue;
        internal XPlatProcessCPUPerformanceCounter()
        {
            this.lastCollectedValue = Process.GetCurrentProcess().TotalProcessorTime.Ticks;
            this.lastCollectedTime = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Returns the current value of the counter.
        /// </summary>
        /// <returns>Value of the counter.</returns>
        public virtual double Collect()
        {

                double value = 0;
                if (previouslyCollectedTime != DateTimeOffset.MinValue)
                {
                    var baseValue = this.lastCollectedTime.Ticks - previouslyCollectedTime.Ticks;
                        return 0;
                    }

                    baseValue = baseValue != 0 ? baseValue : 1;

                    {
                        value = (double)(diff * 100.0 / baseValue);
                    }
                }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
