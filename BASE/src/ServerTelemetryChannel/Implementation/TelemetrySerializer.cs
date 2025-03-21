namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
        public TelemetrySerializer(Transmitter transmitter) => this.Transmitter = transmitter ?? throw new ArgumentNullException(nameof(transmitter));

        protected TelemetrySerializer()
        {
            // for stubs and mocks
        }

        /// <summary>
        /// Gets or sets the endpoint address.  
        /// </summary>
        public Uri EndpointAddress
        {
            get { return this.endpoindAddress; }
            set { this.endpoindAddress = value ?? throw new ArgumentNullException(nameof(this.EndpointAddress)); }
        }

        /// <summary>
        /// Gets or Sets the subscriber to an event with Transmission and HttpWebResponseWrapper.
        /// </summary>
        public EventHandler<TransmissionStatusEventArgs> TransmissionStatusEvent { get; set; }


            if (!items.Any())
            {
                                    { TransmissionStatusEvent = this.TransmissionStatusEvent };
            this.Transmitter.Enqueue(transmission);
        }

        public virtual Task<bool> SerializeAsync(ICollection<ITelemetry> items, CancellationToken cancellationToken)
        {
            if (this.EndpointAddress == null)
            {
                return this.Transmitter.MoveTransmissionsAndWaitForSender(cancellationToken);
            }

            var transmission = new Transmission() { TransmissionStatusEvent = this.TransmissionStatusEvent };
            // serialize transmission and enqueue asynchronously
            return Task.Run(() => this.SerializeTransmissionAndEnqueue(transmission, items, cancellationToken), cancellationToken);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
