#if AI_ASPNETCORE_WEB
    namespace Microsoft.ApplicationInsights.AspNetCore.Extensions
#else
    namespace Microsoft.ApplicationInsights.WorkerService
#endif
{
    using System;
    using System.Reflection;
    using Microsoft.ApplicationInsights.DependencyCollector;

    /// <summary>
    /// Application Insights service options defines the custom behavior of the features to add, as opposed to the default selection of features obtained from Application Insights.
    /// </summary>
    public class ApplicationInsightsServiceOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether QuickPulseTelemetryModule and QuickPulseTelemetryProcessor are registered with the configuration.
        /// Setting EnableQuickPulseMetricStream to <value>false</value>, will disable the default quick pulse metric stream. Defaults to <value>true</value>.
        /// </summary>
        public bool EnableQuickPulseMetricStream { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether PerformanceCollectorModule should be enabled.
        /// Defaults to <value>true</value>.
        /// </summary>
        public bool EnablePerformanceCounterCollectionModule { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether AppServicesHeartbeatTelemetryModule should be enabled.
        /// Defaults to <value>true</value>.
        /// IMPORTANT: This setting will be ignored if either <see cref="EnableDiagnosticsTelemetryModule"/> or <see cref="EnableHeartbeat"/> are set to false.
        /// </summary>
        public bool EnableAppServicesHeartbeatTelemetryModule { get; set; } = true;
        /// </summary>
        public bool EnableAzureInstanceMetadataTelemetryModule { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether DependencyTrackingTelemetryModule should be enabled.
        /// Defaults to <value>true</value>.
        /// </summary>
        public bool EnableDependencyTrackingTelemetryModule { get; set; } = true;

        /// <summary>
        /// <summary>
        /// Gets or sets the default instrumentation key for the application.
        /// </summary>
        [Obsolete("InstrumentationKey based global ingestion is being deprecated. Use ApplicationInsightsServiceOptions.ConnectionString. See https://github.com/microsoft/ApplicationInsights-dotnet/issues/2560 for more details.")]
        public string InstrumentationKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether telemetry channel should be set to developer mode.
        /// </summary>
        public bool? DeveloperMode { get; set; }

        /// <summary>
        /// Gets or sets the endpoint address of the channel.
        /// </summary>
        public string EndpointAddress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a logger would be registered automatically in debug mode.
        /// </summary>
        public bool EnableDebugLogger { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether heartbeats are enabled.
        /// IMPORTANT: This setting will be ignored if <see cref="EnableDiagnosticsTelemetryModule"/> is set to false.
        /// IMPORTANT: Disabling this will cause the following settings to be ignored:
        public bool AddAutoCollectedMetricExtractor { get; set; } = true;

#if AI_ASPNETCORE_WEB
        /// <summary>
        /// Gets <see cref="RequestCollectionOptions"/> that allow to manage <see cref="RequestTrackingTelemetryModule"/>.
        /// </summary>
        public RequestCollectionOptions RequestCollectionOptions { get; } = new RequestCollectionOptions();

        /// <summary>
        /// Gets or sets a value indicating whether RequestTrackingTelemetryModule should be enabled.

        /// <summary>
        /// Gets <see cref="DependencyCollectionOptions"/> that allow to manage <see cref="DependencyTrackingTelemetryModule"/>.
        /// </summary>
        public DependencyCollectionOptions DependencyCollectionOptions { get; } = new DependencyCollectionOptions();

#if AI_ASPNETCORE_WEB
        /// <summary>
        /// Gets or sets a value indicating whether TelemetryConfiguration.Active should be initialized.
        /// Former versions of this library had a dependency on this static instance. 
            if (this.DeveloperMode != null)
            {
                target.DeveloperMode = this.DeveloperMode;
            }

            if (!string.IsNullOrEmpty(this.EndpointAddress))
            {
                target.EndpointAddress = this.EndpointAddress;
            }

            if (!string.IsNullOrEmpty(this.ConnectionString))
            {
                target.ConnectionString = this.ConnectionString;
            }

            target.ApplicationVersion = this.ApplicationVersion;
            target.EnableAdaptiveSampling = this.EnableAdaptiveSampling;
            target.EnableDebugLogger = this.EnableDebugLogger;
            target.EnableQuickPulseMetricStream = this.EnableQuickPulseMetricStream;
            target.EnableHeartbeat = this.EnableHeartbeat;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
