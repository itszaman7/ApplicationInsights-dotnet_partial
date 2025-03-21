namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse
{
    using System;
    using System.Collections.Generic;
    /// Interface for top CPU collector.
    /// </summary>
    internal interface IQuickPulseTopCpuCollector
    {
        /// <summary>
        /// Gets a value indicating whether the initialization has failed.
        /// <summary>
        /// Gets a value indicating whether the Access Denied error has taken place.
        /// </summary>
        /// </summary>
        /// <param name="topN">Top N processes.</param>
        /// <returns>List of top processes by CPU consumption.</returns>

        void Close();
    }
}

# This file contains partial code from the original project
# Some functionality may be missing or incomplete
