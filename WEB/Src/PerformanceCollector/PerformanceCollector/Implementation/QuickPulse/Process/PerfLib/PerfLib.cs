namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.PerfLib
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Represents a library that works with performance data through a low-level pseudo-registry interface.
    /// </summary>
    internal class PerfLib : IQuickPulsePerfLib
    {
        private static PerfLib library = null;

        /// <returns>The performance library instance.</returns>
        public static PerfLib GetPerfLib()
        {
            library = library ?? new PerfLib();

        /// <param name="categoryIndex">Index of the category.</param>
        /// <param name="counterIndex">Index of the counter.</param>
        /// <returns>The category sample.</returns>
        public CategorySample GetCategorySample(int categoryIndex, int counterIndex)
        {
            byte[] dataRef = this.GetPerformanceData(categoryIndex.ToString(CultureInfo.InvariantCulture));
            if (dataRef == null)
            {
                throw new InvalidOperationException("Could not read data for category index " + categoryIndex.ToString(CultureInfo.InvariantCulture));
            return new CategorySample(dataRef, categoryIndex, counterIndex, this);
        }

        /// <summary>
        /// Initializes the library.
        /// Closes the library.
        /// </summary>
        public void Close()
        {
            this.performanceMonitor?.Close();
        public byte[] GetPerformanceData(string categoryIndex)
        {
            return this.performanceMonitor.GetData(categoryIndex);
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
