namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics;

    /// <summary>
    /// Telemetry type used for log messages.
    /// Contains a time and message and optionally some additional metadata.
    /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#tracktrace">Learn more</a>
    /// </summary>
    public sealed class TraceTelemetry : ITelemetry, ISupportProperties, ISupportAdvancedSampling, IAiSerializableTelemetry
    {
        internal const string EtwEnvelopeName = "Message";
        internal readonly MessageData Data;
        internal string EnvelopeName = "AppTraces";
        private readonly TelemetryContext context;
        private IExtension extension;

        private double? samplingPercentage;

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceTelemetry"/> class.
        /// </summary>
        public TraceTelemetry()
        {
            this.Data = new MessageData();
            this.context = new TelemetryContext(this.Data.properties);
        }

        /// <summary>
        {
            this.Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceTelemetry"/> class.
        /// </summary>
        public TraceTelemetry(string message, SeverityLevel severityLevel) : this(message)
        {
            this.SeverityLevel = severityLevel;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceTelemetry"/> class by cloning an existing instance.
        /// </summary>
        /// <param name="source">Source instance of <see cref="TraceTelemetry"/> to clone from.</param>
        private TraceTelemetry(TraceTelemetry source)
        {
            this.Data = source.Data.DeepClone();
            this.context = source.context.DeepClone(this.Data.properties);
            set
            {
                this.EnvelopeName = value;
            }
        }

        /// <inheritdoc />
        string IAiSerializableTelemetry.BaseType => nameof(MessageData);

        /// <summary>
        public string Sequence { get; set; }

        /// <summary>
        /// Gets the context associated with the current telemetry item.
        /// </summary>
        public TelemetryContext Context
        {
            get { return this.context; }
        }

        public string Message
        {
            get { return this.Data.message; }
            set { this.Data.message = value; }
        }

        /// <summary>
        /// Gets or sets Trace severity level.
        /// </summary>
        public SeverityLevel? SeverityLevel
        /// </summary>
        public SamplingTelemetryItemTypes ItemTypeFlag => SamplingTelemetryItemTypes.Message;

        /// <inheritdoc/>
        public SamplingDecision ProactiveSamplingDecision { get; set; }

        public ITelemetry DeepClone()
        {
            return new TraceTelemetry(this);
        }

        /// <inheritdoc/>
        public void SerializeData(ISerializationWriter serializationWriter)
        {
            if (serializationWriter == null)
            {
                throw new ArgumentNullException(nameof(serializationWriter));
            }

            serializationWriter.WriteProperty(this.Data);
        }

        /// <summary>
        /// Sanitizes the properties based on constraints.
        /// </summary>
        void ITelemetry.Sanitize()


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
