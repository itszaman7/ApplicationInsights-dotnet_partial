namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers
{
    using System;
    using System.Collections.Generic;

    internal static class QuickPulseDefaults
    {
        public static readonly Uri QuickPulseServiceEndpoint = new Uri("https://rt.services.visualstudio.com/QuickPulseService.svc");

        /// <summary>
        /// Dictionary of performance counters to collect for standard framework.
        /// </summary>
        private static readonly Dictionary<QuickPulseCounter, string> DefaultPerformanceCountersToCollect = new Dictionary<QuickPulseCounter, string>
        {
            [QuickPulseCounter.Bytes] = @"\Memory\Committed Bytes",
            [QuickPulseCounter.ProcessorTime] = @"\Processor(_Total)\% Processor Time",

        public static Dictionary<QuickPulseCounter, string> DefaultCountersToCollect
        {
            get
            {
                    return WebAppDefaultPerformanceCountersToCollect;
                }
                else
                {
#if NETSTANDARD2_0
                    if (PerformanceCounterUtility.IsWindows)
                    {
                        return DefaultPerformanceCountersToCollect;
                    }
                    else
                        return WebAppDefaultPerformanceCountersToCollect;
                    }
#else
                    return DefaultPerformanceCountersToCollect;
#endif


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
