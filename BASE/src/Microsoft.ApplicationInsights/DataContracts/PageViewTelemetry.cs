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
    /// Telemetry type used to track page views.
    /// </summary>
    /// <remarks>
    /// You can send information about pages viewed by your application to Application Insights by
    /// passing an instance of the <see cref="PageViewTelemetry"/> class to the <see cref="TelemetryClient.TrackPageView(PageViewTelemetry)"/>
    /// method.
    /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#page-views">Learn more</a>
    /// </remarks>
    public sealed class PageViewTelemetry : ITelemetry, ISupportProperties, ISupportAdvancedSampling, ISupportMetrics, IAiSerializableTelemetry
    {
        internal const string EtwEnvelopeName = "PageView";
        internal readonly PageViewData Data;
        internal string EnvelopeName = "AppPageViews";
        private readonly TelemetryContext context;
        private IExtension extension;

        private double? samplingPercentage;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageViewTelemetry"/> class.
        /// </summary>
        public PageViewTelemetry()
        {
            this.Data = new PageViewData();
            this.context = new TelemetryContext(this.Data.properties);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageViewTelemetry"/> class with the
        /// specified <paramref name="pageName"/>.
        /// </summary>
        /// <exception cref="ArgumentException">The <paramref name="pageName"/> is null or empty string.</exception>
        public PageViewTelemetry(string pageName) : this()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageViewTelemetry"/> class by cloning an existing instance.
        /// </summary>
        /// <param name="source">Source instance of <see cref="PageViewTelemetry"/> to clone from.</param>
        private PageViewTelemetry(PageViewTelemetry source)
        {
            this.Data = source.Data.DeepClone();
            this.context = source.context.DeepClone(this.Data.properties);
            {
        public TelemetryContext Context
        {
            get { return this.context; }
        }

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
        /// Gets or sets the page view duration.
        /// </summary>
        public TimeSpan Duration
        {
            get { return Utils.ValidateDuration(this.Data.duration); }
            set { this.Data.duration = value.ToString(); }
        }

        /// <summary>
        /// Gets a dictionary of custom defined metrics.
        /// </summary>
        public IDictionary<string, string> Properties
        {
            get { return this.Data.properties; }
        }

        /// <summary>
        /// Gets or sets data sampling percentage (between 0 and 100).
        /// Should be 100/n where n is an integer. <a href="https://go.microsoft.com/fwlink/?linkid=832969">Learn more</a>
        /// </summary>
            get { return this.samplingPercentage; }
            set { this.samplingPercentage = value; }
        }

        /// <summary>
        /// Gets item type for sampling evaluation.
        /// </summary>
        public SamplingTelemetryItemTypes ItemTypeFlag => SamplingTelemetryItemTypes.PageView;

        /// <inheritdoc/>
        /// Sanitizes the properties based on constraints.
        /// </summary>
        void ITelemetry.Sanitize()
        {
            this.Name = this.Name.SanitizeName();
            this.Name = Utils.PopulateRequiredStringValue(this.Name, "name", typeof(PageViewTelemetry).FullName);
            this.Properties.SanitizeProperties();
            this.Metrics.SanitizeMeasurements();
            this.Url = this.Url.SanitizeUri();
            this.Id.SanitizeName();


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
