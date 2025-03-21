namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers
{
    using System;
    using System.Threading;

    internal interface IQuickPulseModuleScheduler
    {
        IQuickPulseModuleSchedulerHandle Execute(Action<CancellationToken> action);
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
