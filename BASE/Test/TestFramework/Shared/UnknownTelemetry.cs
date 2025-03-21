namespace Microsoft.ApplicationInsights.TestFramework
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    // Request Telemetry in disguise
    public sealed class UnknownTelemetry : OperationTelemetry, ITelemetry, ISupportProperties, ISupportMetrics, ISupportSampling
    {
        internal new const string TelemetryName = "Unknown";
        private readonly TelemetryContext context;
        private IExtension extension;
        private double? samplingPercentage;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownTelemetry"/> class.
        /// </summary>
        public UnknownTelemetry()
        {
            this.Properties = new Dictionary<string, string>();
            this.Metrics = new Dictionary<string, double>();
            this.context = new TelemetryContext(this.Properties);
            this.GenerateId();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownTelemetry"/> class with the given <paramref name="name"/>,
        /// <paramref name="startTime"/>, <paramref name="duration"/>, <paramref name="responseCode"/> and <paramref name="success"/> property values.
        /// </summary>
        public UnknownTelemetry(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success)
            : this()
        {
            this.Name = name; // Name is optional but without it UX does not make much sense
            this.Timestamp = startTime;
            this.Duration = duration;
            this.ResponseCode = responseCode;
            this.Success = success;
            this.Properties = new Dictionary<string, string>();
            this.Metrics = new Dictionary<string, double>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownTelemetry"/> class by cloning an existing instance.
        /// Gets or sets date and time when telemetry was recorded.
        /// </summary>
        public override DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the value that defines absolute order of the telemetry item.
        /// </summary>
        public override string Sequence { get; set; }
        /// </summary>
        public override string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets human-readable name of the requested page.
        /// </summary>
        }

        /// <summary>
        /// Gets or sets response code returned by the application after handling the request.
        /// </summary>
        public string ResponseCode
        {
            get;
            set;
        }
            set;
        }

        /// <summary>
        /// Gets a dictionary of application-defined property names and values providing additional information about this request.        
        /// </summary>
        public override IDictionary<string, string> Properties
        {
            get;
        }

        /// <summary>
        /// Gets or sets request url (optional).
        /// </summary>
        public Uri Url
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the source for the request telemetry object. This often is a hashed instrumentation key identifying the caller.
        /// </summary>
        public string Source
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the MetricExtractorInfo.
        /// </summary>
        internal string MetricExtractorInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Deeply clones a <see cref="UnknownTelemetry"/> object.
        /// </summary>
        /// <returns>A cloned instance.</returns>
        public override ITelemetry DeepClone()
        {
            return new UnknownTelemetry(this);
        }

        /// <inheritdoc/>
        public override void SerializeData(ISerializationWriter serializationWriter)
            if (!this.Success.HasValue)
            {
                this.Success = true;
            }

            if (string.IsNullOrEmpty(this.ResponseCode))
            {
                this.ResponseCode = this.Success.Value ? "200" : string.Empty;
            }
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
