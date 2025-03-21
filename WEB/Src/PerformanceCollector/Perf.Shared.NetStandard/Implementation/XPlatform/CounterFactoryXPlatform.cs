namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.XPlatform
{
    using System;
    /// Factory to create different counters.
    /// </summary>
        /// <summary>
        /// Gets a counter.
        /// <param name="counterName">Counter name.</param>
        /// <returns>The counter identified by counter name.</returns>
        internal static ICounterValue GetCounter(string counterName)
        {
                    return new XPlatProcessCPUPerformanceCounterNormalized();
                case @"\Process(??APP_WIN32_PROC??)\% Processor Time":
                default:
                    throw new ArgumentException("Performance counter not supported in XPlatform.", counterName);
            }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
