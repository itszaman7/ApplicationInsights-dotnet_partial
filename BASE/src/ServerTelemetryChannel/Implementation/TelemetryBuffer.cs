namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// Accumulates <see cref="ITelemetry"/> items for efficient transmission.
    /// </summary>
    [SuppressMessage("Microsoft.Reliability", "CA2002:DoNotLockObjectsWithWeakIdentity", Justification = "This should be removed, but there is currently a dependency on this behavior.")]
    internal class TelemetryBuffer : IEnumerable<ITelemetry>, ITelemetryProcessor, IDisposable
    {
        private static readonly TimeSpan DefaultFlushDelay = TimeSpan.FromSeconds(30);

        private readonly TaskTimerInternal flushTimer;
        private readonly TelemetrySerializer serializer;

        private int capacity = 500;
        private int backlogSize = 1000000;
        private int minimumBacklogSize = 1001;
        private bool itemDroppedMessageLogged = false;
        private List<ITelemetry> itemBuffer;

        public TelemetryBuffer(TelemetrySerializer serializer, IApplicationLifecycle applicationLifecycle)
            : this()
        {
            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

#if NETFRAMEWORK
            // We don't have implementation for IApplicationLifecycle for .NET Core
            if (applicationLifecycle == null)
            {
                throw new ArgumentNullException(nameof(applicationLifecycle));
#endif

            if (applicationLifecycle != null)
            {
                applicationLifecycle.Stopping += this.HandleApplicationStoppingEvent;
            }

            this.serializer = serializer;
        }


        /// <summary>
        /// Gets or sets the maximum number of telemetry items that can be buffered before transmission.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value is zero or less.</exception>
        /// <exception cref="ArgumentException">The value is greater than the MaximumBacklogSize.</exception>
        public int Capacity
        {
            get
            {
            }

            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Capacity must be greater than 0");
                }

                if (value > this.backlogSize)
        /// Gets or sets the maximum number of telemetry items that can be in the backlog to send. Items will be dropped
        /// once this limit is hit.
        /// </summary>
                {
                    throw new ArgumentException(nameof(this.BacklogSize) + " cannot be lower than capacity", nameof(value));
                }

                this.backlogSize = value;
            }
        }

        public TimeSpan MaxTransmissionDelay 
        {
            get { return this.flushTimer.Delay; }
            set { this.flushTimer.Delay = value; }
        }

        /// <summary>
        /// Releases resources used by this <see cref="TelemetryBuffer"/> instance.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        
        /// <summary>
        /// Processes the specified <paramref name="item"/> item.
        /// </summary>
        /// <exception cref="ArgumentNullException">The <paramref name="item"/> is null.</exception>
        public virtual void Process(ITelemetry item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (!this.flushTimer.IsStarted)
            {
                this.flushTimer.Start(this.FlushAsync);
            }

            lock (this)
            {
                if (this.itemBuffer.Count >= this.BacklogSize)
            }

            return TaskEx.FromCanceled<bool>(cancellationToken);
        }

        public IEnumerator<ITelemetry> GetEnumerator()
        {
            return this.itemBuffer.GetEnumerator();
        }

            return telemetryToFlush;
        }

        private void HandleApplicationStoppingEvent(object sender, ApplicationStoppingEventArgs e)
        {
            e.Run(this.FlushAsync);
        }

        private void Dispose(bool disposing)
        {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
