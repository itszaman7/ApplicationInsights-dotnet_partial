namespace Microsoft.ApplicationInsights.Metrics
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Metrics.Extensibility;

    internal class DefaultAggregationPeriodCycle
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming Rules", "SA1310: C# Field must not contain an underscore", Justification = "By design: Structured name.")]
        private const int RunningState_NotStarted = 0;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming Rules", "SA1310: C# Field must not contain an underscore", Justification = "By design: Structured name.")]
        private const int RunningState_Running = 1;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming Rules", "SA1310: C# Field must not contain an underscore", Justification = "By design: Structured name.")]
        private const int RunningState_Stopped = 2;

        private readonly MetricAggregationManager aggregationManager;
        private readonly MetricManager metricManager;

        private readonly TaskCompletionSource<bool> workerTaskCompletionControl;

        private int runningState;
        private Thread aggregationThread;

        public DefaultAggregationPeriodCycle(MetricAggregationManager aggregationManager, MetricManager metricManager)
        {
            Util.ValidateNotNull(aggregationManager, nameof(aggregationManager));
            Util.ValidateNotNull(metricManager, nameof(metricManager));

            this.aggregationThread = null;
        }

        ~DefaultAggregationPeriodCycle()
        {
            Task fireAndForget = this.StopAsync();
        }

        public bool Start()
        {
            if (prev != RunningState_NotStarted)
            {
                return false; // Was already running or stopped.
            }

            // We create a new thread rather than using the thread pool.
            // This is because inside of the main loop in the Run() method we use a synchronous wait.
            // The reason for that is to prevent aggregation from being affected by potential thread pool starvation.
            // As a result, Run() is a very long running task that occupies a thread forever.
            // If we were to schedule Run() on the thread pool it would be possible that the thread chosen by the
            // pool had run user code before. Such user code may be doing an asynchronous wait scheduled to
            // continue on the same thread(e.g. this can occur when using a custom synchronization context or a 
            // custom task scheduler). If such case the waiting user code will never continue.
            // By creating our own thread, we guarantee no interactions with potentially incorrectly written async user code.

            this.aggregationThread = new Thread(this.Run);
            this.aggregationThread.Name = nameof(DefaultAggregationPeriodCycle);
            this.aggregationThread.IsBackground = true;

            this.aggregationThread.Start();
            return true;
        }

        public Task StopAsync()
        {
            Interlocked.Exchange(ref this.runningState, RunningState_Stopped);

            return this.workerTaskCompletionControl.Task;
        }

            DateTimeOffset now = DateTimeOffset.Now;

            if (new DateTimeOffset(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, now.Offset) <= now
                    && new DateTimeOffset(now.Year, now.Month, now.Day, now.Hour, now.Minute, 4, now.Offset) >= now)
            {
                now = new DateTimeOffset(now.Year, now.Month, now.Day, now.Hour, now.Minute, 1, now.Offset);
            }
                now = Util.RoundDownToSecond(now);
            }

            AggregationPeriodSummary aggregates = this.aggregationManager.StartOrCycleAggregators(
                                                                            MetricAggregationCycleKind.Default,
                                                                            futureFilter: null,
                                                                            tactTimestamp: now);
            if (aggregates != null)
            {
                Task fireAndForget = Task.Run(() => this.metricManager.TrackMetricAggregates(aggregates, flush: false));
                        this.workerTaskCompletionControl.TrySetResult(true);
                        return;
                    }

                    this.FetchAndTrackMetrics();
                }
            }
            catch (Exception ex)
            {
                // This is a Thread, and we don't want any exception thrown ever from this part as this would cause application crash.


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
