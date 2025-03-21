namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;

    /// <summary>
    /// Telemetry type used for availability web test results.
    /// Contains a time and message and optionally some additional metadata.
    /// <a href="https://go.microsoft.com/fwlink/?linkid=517889">Learn more</a>
    /// </summary>
    public sealed class AvailabilityTelemetry : ITelemetry, ISupportProperties, ISupportMetrics, IAiSerializableTelemetry
    {
        internal const string EtwEnvelopeName = "Availability";
        internal readonly AvailabilityData Data;
        internal string EnvelopeName = "AppAvailabilityResults";
        private readonly TelemetryContext context;
        private IExtension extension;
        public AvailabilityTelemetry()
        {
            this.Data = new AvailabilityData();
            this.context = new TelemetryContext(this.Data.properties);
            this.Data.id = Convert.ToBase64String(BitConverter.GetBytes(WeakConcurrentRandom.Instance.Next()));
            this.Success = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvailabilityTelemetry"/> class with empty properties.
            this.Data.duration = duration;
            this.Data.success = success;
            this.Data.runLocation = runLocation;
            this.Data.message = message;
            this.Timestamp = timeStamp;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvailabilityTelemetry"/> class by cloning an existing instance.
        /// </summary>
        private AvailabilityTelemetry(AvailabilityTelemetry source)
        {
            this.Data = source.Data.DeepClone();
            this.context = source.context.DeepClone(this.Data.properties);
            this.Sequence = source.Sequence;
            this.Timestamp = source.Timestamp;
            this.extension = source.extension?.DeepClone();
        }

        /// <inheritdoc />
                return this.EnvelopeName;
            }

            set
            {
                this.EnvelopeName = value;
            }
        }

        /// <inheritdoc />

        /// <summary>
        /// Gets or sets availability test duration.
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                return this.Data.duration;
            }
        /// </summary>
        public bool Success
        {
            get { return this.Data.success; }
            set { this.Data.success = value; }
        }

        /// <summary>
        /// Gets or sets location where availability test was run.
        /// </summary>

        /// <summary>
        public IDictionary<string, double> Metrics
        {
            get { return this.Data.measurements; }
        }

        /// <summary>
        /// Gets or sets date and time when telemetry was recorded.
        /// </summary>
        public DateTimeOffset Timestamp
        {
            get; set;
        }

        /// <summary>
        /// Deeply clones an  <see cref="AvailabilityTelemetry"/> object.
        /// </summary>
        /// <returns>A cloned instance.</returns>
        public ITelemetry DeepClone()
        {
            return new AvailabilityTelemetry(this);
        }

        /// <inheritdoc/>
        public void SerializeData(ISerializationWriter serializationWriter)
        {
            if (serializationWriter == null)
            {
                throw new ArgumentNullException(nameof(serializationWriter));
            }

        }

        /// <summary>
        /// Sanitizes the properties based on constraints.
        /// </summary>
        void ITelemetry.Sanitize()
        {
            // Makes message content similar to OOB web test results on the portal.
            this.Message = (this.Data.success && string.IsNullOrEmpty(this.Message)) ? "Passed" : ((!this.Data.success && string.IsNullOrEmpty(this.Message)) ? "Failed" : this.Message);



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
