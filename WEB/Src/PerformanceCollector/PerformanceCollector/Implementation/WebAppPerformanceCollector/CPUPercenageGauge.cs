namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector
{
    using System;

    /// <summary>
    /// Gauge that computes the CPU percentage utilized by a process by utilizing the last computed time.
    /// </summary>
    internal class CPUPercenageGauge : ICounterValue
    {
        /// Name of the counter.
        /// </summary>
        private string name;
        /// Initializes a new instance of the <see cref="CPUPercenageGauge"/> class.
        /// </summary>
        /// <param name="name"> Name of the SumUpCountersGauge.</param>
        /// <param name="value"> Gauges to sum.</param>
        public CPUPercenageGauge(string name, ICounterValue value)
        {
            this.name = name;
            this.valueProvider = value;
        }

        /// <summary>
        /// Returns the percentage of the CPU process utilization time with respect to the total duration.
        /// <summary>
        /// Returns the percentage of the CPU process utilization time with respect to the total duration.
        /// </summary>
        /// <returns>The value of the target metric.</returns>
        protected virtual double CollectPercentage()
        {
                var baseValue = this.lastCollectedTime.Ticks - previouslyCollectedTime.Ticks;
                baseValue = baseValue != 0 ? baseValue : 1;

                var diff = this.lastCollectedValue - previouslyCollectedValue;

                if (diff < 0)
                    PerformanceCollectorEventSource.Log.WebAppCounterNegativeValue(
                    this.lastCollectedValue,
                    previouslyCollectedValue,
                    this.name);
                }
                else


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
