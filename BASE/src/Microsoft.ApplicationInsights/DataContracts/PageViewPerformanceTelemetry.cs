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
    /// Telemetry type used to track page load performance.
    /// </summary>
    public sealed class PageViewPerformanceTelemetry : ITelemetry, ISupportProperties, ISupportAdvancedSampling, IAiSerializableTelemetry
    {
        internal const string EtwEnvelopeName = "PageViewPerformance";
        internal readonly PageViewPerfData Data;
        internal string EnvelopeName = "AppBrowserTimings";
        private IExtension extension;
        private double? samplingPercentage;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageViewPerformanceTelemetry"/> class.
        /// </summary>
        public PageViewPerformanceTelemetry()
        {
            this.Data = new PageViewPerfData();
            this.Context = new TelemetryContext(this.Data.properties);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageViewPerformanceTelemetry"/> class with the
        /// specified <paramref name="pageName"/>.
        /// </summary>
        /// <exception cref="ArgumentException">The <paramref name="pageName"/> is null or empty string.</exception>
        public PageViewPerformanceTelemetry(string pageName) : this()
        {
            this.Name = pageName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageViewPerformanceTelemetry"/> class by cloning an existing instance.
        /// </summary>
        /// <param name="source">Source instance of <see cref="PageViewPerformanceTelemetry"/> to clone from.</param>
        private PageViewPerformanceTelemetry(PageViewPerformanceTelemetry source)
        {
            this.Data = source.Data.DeepClone();
        /// </summary>
        public TelemetryContext Context { get; private set; }

        /// <summary>
        /// Gets or sets gets the extension used to extend this telemetry instance using new strong typed object.
        /// </summary>
        public IExtension Extension
        {
            get { return this.extension; }
            set { this.extension = value; }
        }

        /// <summary>
        /// Gets or sets page view ID.
        /// </summary>
        public string Id
        {
            get { return this.Data.id; }
            set { this.Data.id = value; }
        }

        /// <summary>
        /// Gets or sets the name of the page.
        /// </summary>
        public string Name
        {
            get { return this.Data.name; }
            set { this.Data.name = value; }
        }

        /// <summary>
        /// Gets or sets the page view Uri.
        /// </summary>
        public Uri Url
        {
            get
            {
                if (this.Data.url.IsNullOrWhiteSpace())
                {
                    return null;
                {
                    this.Data.url = null;
                }
                else
                {
                    this.Data.url = value.ToString();
                }
            }
        }

            set { this.Data.perfTotal = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the page load network time.
        /// </summary>
        public TimeSpan NetworkConnect
        {
            get { return Utils.ValidateDuration(this.Data.networkConnect); }
            set { this.Data.networkConnect = value.ToString(); }

        /// <summary>
        /// Gets or sets the page load send request time.
        /// </summary>
        public TimeSpan SentRequest
        {
            get { return Utils.ValidateDuration(this.Data.sentRequest); }
            set { this.Data.sentRequest = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the page load recieve response duration.
        /// </summary>
        public TimeSpan ReceivedResponse
        {
            get { return Utils.ValidateDuration(this.Data.receivedResponse); }
            set { this.Data.receivedResponse = value.ToString(); }
        }
        /// <summary>
        /// Gets item type for sampling evaluation.
        /// </summary>
        public SamplingTelemetryItemTypes ItemTypeFlag => SamplingTelemetryItemTypes.PageViewPerformance;

        /// <inheritdoc/>
        public SamplingDecision ProactiveSamplingDecision { get; set; }

        /// <summary>
        /// Deeply clones a <see cref="PageViewTelemetry"/> object.
            this.Name = Utils.PopulateRequiredStringValue(this.Name, "name", typeof(PageViewTelemetry).FullName);
            this.Properties.SanitizeProperties();
            this.Metrics.SanitizeMeasurements();
            this.Url = this.Url.SanitizeUri();
            this.Id.SanitizeName();
        }

        /// <inheritdoc/>
        public void SerializeData(ISerializationWriter serializationWriter)
        {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
