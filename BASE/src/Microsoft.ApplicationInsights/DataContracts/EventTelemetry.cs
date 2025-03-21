namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;

    /// <summary>
    /// Telemetry type used to track custom events.
    /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#trackevent">Learn more</a>
    /// </summary>
    public sealed class EventTelemetry : ITelemetry, ISupportProperties, ISupportAdvancedSampling, ISupportMetrics, IAiSerializableTelemetry
    {
        internal const string EtwEnvelopeName = "Event";
        internal const string DefaultEnvelopeName = "AppEvents";
        internal readonly EventData Data;
        internal string EnvelopeName = DefaultEnvelopeName;
        private readonly TelemetryContext context;
        private IExtension extension;

        private double? samplingPercentage;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTelemetry"/> class.
        /// </summary>
        public EventTelemetry()
        {
        {
            this.Name = name;
        }

        private EventTelemetry(EventTelemetry source)
        {
            this.Data = source.Data.DeepClone();
            this.context = source.context.DeepClone(this.Data.properties);
            this.Sequence = source.Sequence;
            this.Timestamp = source.Timestamp;
            this.samplingPercentage = source.samplingPercentage;
            this.ProactiveSamplingDecision = source.ProactiveSamplingDecision;
            this.extension = source.extension?.DeepClone();
        }

        /// <inheritdoc />
        string IAiSerializableTelemetry.TelemetryName
        {
            get
            {
        /// </summary>
        public IExtension Extension
        {
            get { return this.extension; }
            set { this.extension = value; }
            set { this.Data.name = value; }
        }

        /// <summary>
        /// Gets a dictionary of application-defined event metrics.
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#properties">Learn more</a>
        /// </summary>
        public IDictionary<string, double> Metrics
        {
            get { return this.Data.measurements; }
        }

        /// <summary>
        /// Gets a dictionary of application-defined property names and values providing additional information about this event.
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#properties">Learn more</a>
        /// </summary>
        public IDictionary<string, string> Properties
        {
            get { return this.Data.properties; }
        }
        /// Gets item type for sampling evaluation.
        /// </summary>
        public SamplingTelemetryItemTypes ItemTypeFlag => SamplingTelemetryItemTypes.Event;

        /// <inheritdoc/>
        public SamplingDecision ProactiveSamplingDecision { get; set; }

        /// <summary>
        /// Deeply clones a <see cref="EventTelemetry"/> object.
        /// </summary>
        /// <returns>A cloned instance.</returns>
        public ITelemetry DeepClone()
        {
            return new EventTelemetry(this);
        }

        /// <summary>
        /// Sanitizes the properties based on constraints.
        /// </summary>
        void ITelemetry.Sanitize()


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
