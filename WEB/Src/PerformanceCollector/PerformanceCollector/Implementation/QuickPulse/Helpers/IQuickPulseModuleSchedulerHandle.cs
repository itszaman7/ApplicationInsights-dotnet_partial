namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers
{
    using System;

    internal interface IQuickPulseModuleSchedulerHandle : IDisposable
    {
        void Stop(bool wait);
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
