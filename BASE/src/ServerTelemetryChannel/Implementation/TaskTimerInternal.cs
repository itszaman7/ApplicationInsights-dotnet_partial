// <copyright file="TaskTimerInternal.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// Runs a task after a certain delay and log any error.
        /// Gets or sets the delay before the task starts. 
        /// </summary>
        public TimeSpan Delay
        {
            get
            {
                return this.delay;
            }

            set
                    {
                        CancelAndDispose(Interlocked.CompareExchange(ref this.tokenSource, null, newTokenSource));
                        try
                        {
                            Task task = elapsed();

                            // Task may be executed synchronously
                            // It should return Task.FromResult but just in case we check for null if someone returned null
                            if (task != null)
                            {
                        catch (Exception exception)
                        {
                            LogException(exception);
                        }
                    },
                CancellationToken.None,
                TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);

            CancelAndDispose(Interlocked.Exchange(ref this.tokenSource, newTokenSource));
        }

        /// <summary>
        /// Cancels the current task.
        private static void LogException(Exception exception)
        {
            if (exception is AggregateException aggregateException)
            {
                aggregateException = aggregateException.Flatten();
                foreach (Exception e in aggregateException.InnerExceptions)
                {
                    TelemetryChannelEventSource.Log.LogError(e.ToInvariantString());
                }
            }

            TelemetryChannelEventSource.Log.LogError(exception.ToInvariantString());
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
        {
            if (disposing)
            {
                this.Cancel();
            }
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
