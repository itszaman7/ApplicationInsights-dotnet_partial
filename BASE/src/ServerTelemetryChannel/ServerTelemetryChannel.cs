namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Authentication;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TransmissionPolicy;

    using TelemetryBuffer = Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TelemetryBuffer;

    /// <summary>
    /// Represents a communication channel for sending telemetry to Application Insights via HTTP/S.
    /// </summary>
    public sealed class ServerTelemetryChannel : ITelemetryChannel, IAsyncFlushable, ITelemetryModule, ISupportCredentialEnvelope
    {
        internal TelemetrySerializer TelemetrySerializer;
        internal TelemetryBuffer TelemetryBuffer;
        internal Transmitter Transmitter;

        private readonly InterlockedThrottle throttleEmptyIkeyLog = new InterlockedThrottle(interval: TimeSpan.FromSeconds(30));
        private readonly TransmissionPolicyCollection policies;

        private bool? developerMode;
        private int telemetryBufferCapacity;
        private ITelemetryProcessor telemetryProcessor;
        private bool isInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerTelemetryChannel"/> class.
        /// </summary>
#if NETFRAMEWORK
        [SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "WebApplicationLifecycle is needed for the life of the application.")]
        public ServerTelemetryChannel() : this(new Network(), new WebApplicationLifecycle())
#else
        // TODO: IApplicationLifecycle implementation for netcore need to be written instead of null here.
        public ServerTelemetryChannel() : this(new Network(), null)
#endif
        {
        }

        internal ServerTelemetryChannel(INetwork network, IApplicationLifecycle applicationLifecycle)
        {
            this.policies = new TransmissionPolicyCollection(network, applicationLifecycle);
            this.Transmitter = new Transmitter(policies: this.policies);

            this.TelemetrySerializer = new TelemetrySerializer(this.Transmitter);
            this.TelemetryBuffer = new TelemetryBuffer(this.TelemetrySerializer, applicationLifecycle);
            this.telemetryBufferCapacity = this.TelemetryBuffer.Capacity;

            this.TelemetryProcessor = this.TelemetryBuffer;
            this.isInitialized = false;
        }

        /// <summary>
        /// Gets or sets default interval after which diagnostics event will be logged if telemetry sending was disabled.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TimeSpan DefaultBackoffEnabledReportingInterval
        {
            get
            {
        /// <summary>
        /// Gets or sets a value indicating whether developer mode of telemetry transmission is enabled.
        /// When developer mode is True, <see cref="TelemetryChannel"/> sends telemetry to Application Insights immediately 
        /// during the entire lifetime of the application. When developer mode is False, <see cref="TelemetryChannel"/>
        /// respects production sending policies defined by other properties.
        /// </summary>
        public bool? DeveloperMode
        {
            get
            {
                return this.developerMode;
            }

            set
            {
                if (value != this.developerMode)
                {
                    if (value.HasValue && value.Value)
                    {
                        this.telemetryBufferCapacity = this.TelemetryBuffer.Capacity;
        }

        /// <summary>
        /// Gets or sets the HTTP address where the telemetry is sent.
        /// </summary>
        public string EndpointAddress
        {
            get { return this.TelemetrySerializer.EndpointAddress?.ToString(); }
            set { this.TelemetrySerializer.EndpointAddress = new Uri(value); }
        }

        /// <summary>
        /// Gets or Sets the subscriber to an event with Transmission and HttpWebResponseWrapper.
        /// </summary>
        public EventHandler<TransmissionStatusEventArgs> TransmissionStatusEvent
        {
            get
            {
                return this.TelemetrySerializer.TransmissionStatusEvent;
            }

            set
            {
                this.TelemetrySerializer.TransmissionStatusEvent = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum telemetry batching interval. Once the interval expires, <see cref="TelemetryChannel"/> 
        /// serializes the accumulated telemetry items for transmission.

        /// <summary>
        /// Gets or sets the maximum number of telemetry items will accumulate in a memory before 
        /// the <see cref="TelemetryChannel"/> serializing them for transmission to Application Insights.
        {
            get { return this.TelemetryBuffer.Capacity; }
            set { this.TelemetryBuffer.Capacity = value; }
        }

        /// <summary>
        /// Gets or sets the maximum number of telemetry items that can be in the backlog to send. This is a hard limit
        /// and Items will be dropped by the <see cref="ServerTelemetryChannel"/> once this limit is hit until items
        /// are drained from the buffer.
        /// </summary>
        public int MaxBacklogSize
        {
            get { return this.TelemetryBuffer.BacklogSize; }
            set { this.TelemetryBuffer.BacklogSize = value; }
        }

        /// <summary>
        /// Gets or sets the maximum amount of memory, in bytes, that <see cref="TelemetryChannel"/> will use 
        /// to buffer transmissions before sending them to Application Insights.
        /// </summary>

        /// <summary>
        /// Gets or sets the maximum number of telemetry transmissions that <see cref="TelemetryChannel"/> will 
        /// send to Application Insights at the same time.
        /// </summary>
        public int MaxTransmissionSenderCapacity
        {
            get { return this.Transmitter.MaxSenderCapacity; }
            set { this.Transmitter.MaxSenderCapacity = value; }
        }
        /// use to store unsent telemetry transmissions.
        /// </summary>
        public long MaxTransmissionStorageCapacity
        {
            get { return this.Transmitter.MaxStorageCapacity; }
            set { this.Transmitter.MaxStorageCapacity = value; }
        }

        /// <summary>
        /// Gets or sets the folder to be used as a temporary storage for events that were not sent because of temporary connectivity issues. 
        }

        /// <summary>
        /// Gets or sets a value indicating whether a limiter on the maximum number of <see cref="ITelemetry"/> objects 
        /// that can be sent in a given throttle window is enabled. Items attempted to be sent exceeding of the local 
        /// throttle amount will be treated the same as a backend throttle.
        /// </summary>
        public bool EnableLocalThrottling
        {
            get { return this.Transmitter.ApplyThrottle; }
            get { return this.Transmitter.ThrottleWindow; }
            set { this.Transmitter.ThrottleWindow = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="CredentialEnvelope"/> which is used for AAD.
        /// </summary>
        /// <remarks>
        /// <see cref="ISupportCredentialEnvelope.CredentialEnvelope"/> on <see cref="ServerTelemetryChannel"/> sets <see cref="Transmitter.CredentialEnvelope"/> and then sets <see cref="TransmissionSender.CredentialEnvelope"/> 
        /// which is used to set <see cref="Transmission.CredentialEnvelope"/> just before calling <see cref="Transmission.SendAsync"/>.
        /// <summary>
        /// Gets or sets first TelemetryProcessor in processor call chain.
        /// </summary>
        internal ITelemetryProcessor TelemetryProcessor
        {
            get
            {
                return this.telemetryProcessor;
            }

                    else
                    {
                        if (!Debugger.IsAttached)
                        {
                            this.throttleEmptyIkeyLog.PerformThrottledAction(() => TelemetryChannelEventSource.Log.TelemetryChannelNoInstrumentationKey());
                        }
                    }

                    return;
                }
                {
                    TelemetryChannelEventSource.Log.TelemetryChannelSend(
                        item.ToString(),
                        item.Context.InstrumentationKey.Substring(0, Math.Min(item.Context.InstrumentationKey.Length, 8)));
                }

                this.TelemetryProcessor.Process(item);
            }
        }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
