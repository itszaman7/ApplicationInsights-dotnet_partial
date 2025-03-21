namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Channel.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Authentication;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TransmissionPolicy;

    /// <summary>
    /// Implements throttled and persisted transmission of telemetry to Application Insights. 
    /// </summary>
    internal class Transmitter : IDisposable
    {
        internal readonly TransmissionSender Sender;
        internal readonly TransmissionBuffer Buffer;
        internal readonly TransmissionStorage Storage;
        private readonly TransmissionPolicyCollection policies;
        private readonly BackoffLogicManager backoffLogicManager;
        private readonly Task<bool> successTask = Task.FromResult(true);
        private readonly Task<bool> failedTask = Task.FromResult(false);

        private bool arePoliciesApplied;
        private int maxSenderCapacity;
        private int maxBufferCapacity;
        private long maxStorageCapacity;

        /// <summary>
        /// Initializes a new instance of the <see cref="Transmitter" /> class. Used only for UTs.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "TODO: change this in future submits.")]
        internal Transmitter(
            TransmissionSender sender = null,
            TransmissionBuffer transmissionBuffer = null,
            TransmissionStorage storage = null,
            TransmissionPolicyCollection policies = null,
            BackoffLogicManager backoffLogicManager = null)
        {
            this.backoffLogicManager = backoffLogicManager ?? new BackoffLogicManager();
            this.Sender = sender ?? new TransmissionSender();
            this.Sender.TransmissionSent += this.HandleSenderTransmissionSentEvent;
            this.maxSenderCapacity = this.Sender.Capacity;

            this.Buffer.TransmissionDequeued += this.HandleBufferTransmissionDequeuedEvent;
            this.maxBufferCapacity = this.Buffer.Capacity;

            this.Storage = storage ?? new TransmissionStorage();
            this.maxStorageCapacity = this.Storage.Capacity;

            this.policies = policies ?? TransmissionPolicyCollection.Default;
            this.policies.Initialize(this);
        }

        public int MaxBufferCapacity
        {
            get
            {
                return this.maxBufferCapacity;
            }

            set
            {
                this.maxBufferCapacity = value;
        public int MaxSenderCapacity
        {
            get
            {
                return this.maxSenderCapacity;
            }

            set
            {
                this.maxSenderCapacity = value;
                this.ApplyPoliciesIfAlreadyApplied();
            }
        }

        public long MaxStorageCapacity
        {
            get
            {
                return this.maxStorageCapacity;
            }
        public int ThrottleWindow
        {
            get { return this.Sender.ThrottleWindow; }
            set { this.Sender.ThrottleWindow = value; }
        }

        public BackoffLogicManager BackoffLogicManager
        {
            get { return this.backoffLogicManager; }
        }
        }

        /// <summary>
        /// Releases resources used by this <see cref="Transmitter"/> instance.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

            TelemetryChannelEventSource.Log.TransmitterEnqueue(transmission.Id);

            if (!this.arePoliciesApplied)
            {
                this.ApplyPolicies();
            }

            Func<Transmission> transmissionGetter = () => transmission;

                transmission.IsFlushAsyncInProgress = false;
                TelemetryChannelEventSource.Log.TransmitterStorageSkipped(transmission.Id);
        }

        internal Task<bool> FlushAsync(Transmission transmission, CancellationToken cancellationToken)
        {
            TaskStatus taskStatus = TaskStatus.Canceled;
            if (!cancellationToken.IsCancellationRequested)
            {
                transmission.IsFlushAsyncInProgress = true;
                this.Enqueue(transmission);

                {
                    this.Storage.DecrementFlushAsyncCounter();
                }
            }

            Task<bool> flushTaskStatus = null;
            if (taskStatus == TaskStatus.Canceled)
            {
                flushTaskStatus = TaskEx.FromCanceled<bool>(cancellationToken);
            }
            {
                flushTaskStatus = this.failedTask;
            }

            return flushTaskStatus;
        }

        internal Task<bool> MoveTransmissionsAndWaitForSender(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return TaskEx.FromCanceled<bool>(cancellationToken);
            }

            TaskStatus senderStatus = TaskStatus.Canceled;
            bool isStorageEnqueueSuccess = false;

            try
            {
                this.Storage.IncrementFlushAsyncCounter();
                isStorageEnqueueSuccess = MoveTransmissions(this.Buffer.Dequeue, this.Storage.Enqueue, this.Buffer.Size, cancellationToken);
                TelemetryChannelEventSource.Log.MovedFromBufferToStorage();
                senderStatus = this.Sender.WaitForPreviousTransmissionsToComplete(cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception exp)
            {
                senderStatus = TaskStatus.Faulted;
                TelemetryChannelEventSource.Log.TransmissionFlushAsyncWarning(exp.ToString());
            }
            finally
            TelemetryChannelEventSource.Log.MovedFromBufferToStorage();
            var senderStatus = this.Sender.WaitForPreviousTransmissionsToComplete(transmissionFlushAsyncId, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();

            if (!isStorageEnqueueSuccess && senderStatus != TaskStatus.Canceled)
            {
                return TaskStatus.Faulted;
            }

            return senderStatus;
        }
                MoveTransmissions(this.Storage.Dequeue, this.Buffer.Enqueue);
                TelemetryChannelEventSource.Log.MovedFromStorageToBuffer();
            }
            else
            {
                MoveTransmissions(this.Buffer.Dequeue, this.Storage.Enqueue);
                TelemetryChannelEventSource.Log.MovedFromBufferToStorage();
                this.EmptyBuffer();
            }

        }

        internal void EmptyBuffer()
        {
            TelemetryChannelEventSource.Log.TransmitterEmptyBuffer();
            while (this.Buffer.Dequeue() != null)
            {
            }
        }

        internal void EmptyStorage()
        {
            TelemetryChannelEventSource.Log.TransmitterEmptyStorage();
            while (this.Storage.Dequeue() != null)
            {
            }
        }

        protected void OnTransmissionSent(TransmissionProcessedEventArgs e)
        {
            }
            while (transmissionMoved && size > 0);

            return transmissionMoved;
        }

        private void ApplyPoliciesIfAlreadyApplied()
        {
            if (this.arePoliciesApplied)
            {
        {
            this.OnTransmissionSent(e);

            try
            {
                MoveTransmissions(this.Buffer.Dequeue, this.Sender.Enqueue);
            }
            catch (Exception exp)
            {
                TelemetryChannelEventSource.Log.ExceptionHandlerStartExceptionWarning(exp.ToString());

        private void HandleBufferTransmissionDequeuedEvent(object sender, TransmissionProcessedEventArgs e)
        {
            try
            {
                MoveTransmissions(this.Storage.Dequeue, this.Buffer.Enqueue);
            }
            catch (Exception exp)
            {
                TelemetryChannelEventSource.Log.ExceptionHandlerStartExceptionWarning(exp.ToString());


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
