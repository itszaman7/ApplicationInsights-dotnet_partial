namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse
{
    using System;

    /// <summary>
    /// </summary>
    internal sealed class QuickPulseProcess
        /// <summary>
        /// Initializes a new instance of the <see cref="QuickPulseProcess"/> class. 
        /// </summary>
        /// <param name="processName">Process name.</param>
        /// <param name="totalProcessorTime">Total processor time.</param>
        public QuickPulseProcess(string processName, TimeSpan totalProcessorTime)
        {
            this.ProcessName = processName;
        public TimeSpan TotalProcessorTime { get; }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
