namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Gauge that computes normalized CPU percentage utilized by a process by utilizing the last computed time (divided by the processors count).
    /// </summary>
    internal class NormalizedCPUPercentageGauge : CPUPercenageGauge
    {
        public NormalizedCPUPercentageGauge(string name, ICounterValue value) : base(name, value)
        {
            int? count = PerformanceCounterUtility.GetProcessorCount();

            if (count.HasValue)
            {
                this.isInitialized = true;
            }
        }
        protected override double CollectPercentage()
        {
            if (!this.isInitialized)
                return 0;
            }


            return result;
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
