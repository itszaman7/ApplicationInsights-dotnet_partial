namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.Channel;

    using TaskEx = System.Threading.Tasks.Task;

    internal class TransmissionBuffer
    {
        // 5MB default capacity
        private const int DefaultCapacityInKiloBytes = 5120;
        private readonly Queue<Transmission> transmissions = new Queue<Transmission>(DefaultCapacityInKiloBytes);
        private int capacity = DefaultCapacityInKiloBytes * 1024;
        private long size = 0;
        public event EventHandler<TransmissionProcessedEventArgs> TransmissionDequeued;

        /// <summary>
        /// Gets or sets the maximum amount of memory in bytes for buffering <see cref="Transmission"/> objects.
        /// </summary>
        /// <remarks>
        /// Use this property to limit the amount of memory used to store telemetry in memory of the 
        /// application before transmission. Once the maximum amount of memory is
        /// reached, <see cref="Enqueue"/> will reject new transmissions.
        /// </remarks>
        public virtual int Capacity
        {
            get
            {
                return this.capacity;
            }

            set
                        this.transmissions.Enqueue(transmission);
                        enqueueSucceded = true;
                        TelemetryChannelEventSource.Log.BufferEnqueued(transmission.Id, this.transmissions.Count);
                    }
                }
                else
                {
                    // We tried to dequeue from storage and storage is empty
                    // return false to stop moving from storage to buffer when policy is applied otherwise we get infinite loop
        {
            Transmission dequeudTransmission = null;

            lock (this.transmissions)
            {
                if (this.transmissions.Count > 0)
                {
                    dequeudTransmission = this.transmissions.Dequeue();
                    this.size -= dequeudTransmission.Content.Length;
                }
            }
        {
            EventHandler<TransmissionProcessedEventArgs> handler = this.TransmissionDequeued;
            if (handler != null)
            {
                handler(this, args);
            }
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
