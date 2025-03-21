// <copyright file="TaskTimerInternal.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// Runs a task after a certain delay and log any error.
    /// </summary>
    internal class TaskTimerInternal : IDisposable
    {
        /// <summary>
        /// Represents an infinite time span.
        /// </summary>
        public static readonly TimeSpan InfiniteTimeSpan = new TimeSpan(0, 0, 0, 0, Timeout.Infinite);

        private TimeSpan delay = TimeSpan.FromMinutes(1);
        private CancellationTokenSource tokenSource;

        /// <summary>
        /// Gets or sets the delay before the task starts. 
        /// </summary>
        public TimeSpan Delay
            }

            set
            {
                if ((value <= TimeSpan.Zero || value.TotalMilliseconds > int.MaxValue) && value != InfiniteTimeSpan)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.delay = value;
                            // Task may be executed synchronously
                            // It should return Task.FromResult but just in case we check for null if someone returned null
                            if (task != null)
                            {
                                await task.ConfigureAwait(false);
                            }
                        }
                        catch (Exception exception)
                        {
                            LogException(exception);
                TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);

            CancelAndDispose(Interlocked.Exchange(ref this.tokenSource, newTokenSource));
        }

        /// <summary>
        /// Cancels the current task.
        /// </summary>
        public void Cancel()
                aggregateException = aggregateException.Flatten();
                foreach (Exception e in aggregateException.InnerExceptions)
                {
                    CoreEventSource.Log.LogError(e.ToInvariantString());
                }
            }

            CoreEventSource.Log.LogError(exception.ToInvariantString());
        }

        private static void CancelAndDispose(CancellationTokenSource tokenSource)
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
                tokenSource.Dispose();
            }
        }

        private void Dispose(bool disposing)


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
