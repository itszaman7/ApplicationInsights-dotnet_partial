namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// From <a href="http://code.msdn.microsoft.com/Samples-for-Parallel-b4b76364/view/SourceCode"/>.
    /// </summary>
    {
        public static readonly TaskScheduler Instance = new CurrentThreadTaskScheduler();

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
            return this.TryExecuteTask(task);
        }
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return Enumerable.Empty<Task>();
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
