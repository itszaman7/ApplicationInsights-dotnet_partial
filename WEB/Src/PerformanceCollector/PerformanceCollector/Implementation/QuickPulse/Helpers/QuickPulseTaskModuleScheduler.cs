namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    {
        public static readonly QuickPulseTaskModuleScheduler Instance = new QuickPulseTaskModuleScheduler();

        private QuickPulseTaskModuleScheduler()
        {
        }

        public IQuickPulseModuleSchedulerHandle Execute(Action<CancellationToken> action)
        {
            State state = new State(action);

            return state;
        {
            private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            private readonly Task task;
            }

            public void Stop(bool wait)
            {
                    // wait and ignore all exceptions
                    this.task.ContinueWith(_ => { }).Wait();
                }
            }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
