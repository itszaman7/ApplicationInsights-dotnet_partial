// <copyright file="InMemoryTransmitter.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Microsoft.ApplicationInsights.Channel
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Common.Extensions;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Authentication;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// A transmitter that will immediately send telemetry over HTTP. 
    /// Telemetry items are being sent when Flush is called, or when the buffer is full (An OnFull "event" is raised) or every 30 seconds. 
    /// </summary>
    internal class InMemoryTransmitter : IDisposable
    {
        private readonly TelemetryBuffer buffer;

        /// <summary>
        /// A lock object to serialize the sending calls from Flush, OnFull event and the Runner.  
        /// </summary>
        private object sendingLockObj = new object();

        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", Justification = "Object is disposed within the using statement of the " + nameof(Runner) + " method.")]
        private AutoResetEvent startRunnerEvent;
        private bool enabled = true;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// </summary>
        private void Runner()
        {
            SdkInternalOperationsMonitor.Enter();
            try
            {
                using (this.startRunnerEvent = new AutoResetEvent(false))
                {
                    while (this.enabled)
                    {
                        // Pulling all items from the buffer and sending as one transmission.
                        this.DequeueAndSend(timeout: default(TimeSpan)); // when default(TimeSpan) is provided, value is ignored and default timeout of 100 sec is used

                        // Waiting for the flush delay to elapse
                        this.startRunnerEvent.WaitOne(this.sendingInterval);
                    }
            }
            finally
            {
                SdkInternalOperationsMonitor.Exit();
            }
        }

        /// <summary>
        /// Happens when the in-memory buffer is full. Flushes the in-memory buffer and sends the telemetry items.
        /// </summary>
        private void OnBufferFull()
        {
            this.startRunnerEvent.Set();
            CoreEventSource.Log.LogVerbose("StartRunnerEvent set as Buffer is full.");
        }

        /// <summary>
        /// Flushes the in-memory buffer and send it.
        /// </summary>
        private void DequeueAndSend(TimeSpan timeout)
        {
            lock (this.sendingLockObj)
            {
                IEnumerable<ITelemetry> telemetryItems = this.buffer.Dequeue();
                try
                {
                    // send request
                    this.Send(telemetryItems, timeout).GetAwaiter().GetResult();
                }
                catch (Exception ex)
        /// <summary>
        /// Serializes a list of telemetry items and sends them.
        /// </summary>
        private Task Send(IEnumerable<ITelemetry> telemetryItems, TimeSpan timeout)
        {
            byte[] data = null;

            if (telemetryItems != null)
            {
                data = JsonSerializer.Serialize(telemetryItems);
                        // We need to try catch the Set call in case the auto-reset event wait interval occurs between setting enabled
                        // to false and the call to Set then the auto-reset event will have already been disposed by the runner thread.
                    }
                }

                this.Flush(default(TimeSpan));
            }
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
