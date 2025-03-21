namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule
{
    internal class DiagnoisticsEventCounters
    {
        private readonly object syncRoot = new object();
        private volatile int execCount;

        internal DiagnoisticsEventCounters(
            int execCountInitial = 0)
        {
            this.execCount = execCountInitial;
        }
                if (int.MaxValue > this.execCount)
            return this.execCount;
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
