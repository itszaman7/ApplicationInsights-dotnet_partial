namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.StandardPerfCollector
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// Represents normalized value of CPU Utilization by Process counter value (divided by the processors count).
    /// </summary>
    internal class NormalizedProcessCPUPerformanceCounter : ICounterValue, IDisposable
    {
        private readonly int processorsCount;
        private readonly bool isInitialized = false;
        internal NormalizedProcessCPUPerformanceCounter(string instanceName)
        {
            int? count = PerformanceCounterUtility.GetProcessorCount();

            if (count.HasValue)
            {
            }
        }

        /// <summary>
        /// Returns the current value of the counter as a <c ref="MetricTelemetry"/>.
        /// </summary>
        /// <returns>Value of the counter.</returns>
        public double Collect()
        {
                        CultureInfo.CurrentCulture,
                        "Failed to perform a read for performance counter {0}",
                        PerformanceCounterUtility.FormatPerformanceCounter(this.performanceCounter)),
                    e);
            }
        }
        }

        /// <summary>
        /// Dispose implementation.
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.performanceCounter != null)
                {
                    this.performanceCounter.Dispose();


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
