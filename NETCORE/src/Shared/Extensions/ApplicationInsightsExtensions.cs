namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    using Microsoft.ApplicationInsights;
#if AI_ASPNETCORE_WEB
    using Microsoft.ApplicationInsights.AspNetCore;
    using Microsoft.ApplicationInsights.AspNetCore.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
    using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
#endif

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.Extensibility;

    using Microsoft.ApplicationInsights.Extensibility.EventCounterCollector;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
    using Microsoft.ApplicationInsights.WindowsServer;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;

#if AI_ASPNETCORE_WORKER
    using Microsoft.ApplicationInsights.WorkerService;
    using Microsoft.ApplicationInsights.WorkerService.Implementation.Tracing;
    using Microsoft.ApplicationInsights.WorkerService.TelemetryInitializers;
#endif

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.Memory;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> that allow adding Application Insights services to application.
    /// </summary>
    public static partial class ApplicationInsightsExtensions
    {
        private const string VersionKeyFromConfig = "version";
        private const string InstrumentationKeyFromConfig = "ApplicationInsights:InstrumentationKey";
        private const string ConnectionStringFromConfig = "ApplicationInsights:ConnectionString";
        private const string DeveloperModeFromConfig = "ApplicationInsights:TelemetryChannel:DeveloperMode";
        private const string EndpointAddressFromConfig = "ApplicationInsights:TelemetryChannel:EndpointAddress";

        private const string InstrumentationKeyForWebSites = "APPINSIGHTS_INSTRUMENTATIONKEY";
        private const string ConnectionStringEnvironmentVariable = "APPLICATIONINSIGHTS_CONNECTION_STRING";
        private const string DeveloperModeForWebSites = "APPINSIGHTS_DEVELOPER_MODE";
        private const string EndpointAddressForWebSites = "APPINSIGHTS_ENDPOINTADDRESS";

        private const string ApplicationInsightsSectionFromConfig = "ApplicationInsights";
        private const string TelemetryChannelSectionFromConfig = "ApplicationInsights:TelemetryChannel";

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Used in NetStandard2.0 build.")]
        private const string EventSourceNameForSystemRuntime = "System.Runtime";

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Used in NetStandard2.0 build.")]
        private const string EventSourceNameForAspNetCoreHosting = "Microsoft.AspNetCore.Hosting";
            where T : ITelemetryProcessor
        {
            return services.AddSingleton<ITelemetryProcessorFactory>(serviceProvider =>
                new TelemetryProcessorFactory(serviceProvider, typeof(T)));
        }

        /// <summary>
        /// Adds an Application Insights Telemetry Processor into a service collection via a <see cref="ITelemetryProcessorFactory"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
            return services.AddSingleton<ITelemetryProcessorFactory>(serviceProvider =>
                new TelemetryProcessorFactory(serviceProvider, telemetryProcessorType));
        }

        /// <summary>
        /// Extension method to provide configuration logic for application insights telemetry module.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
        /// <param name="configModule">Action used to configure the module.</param>
        /// <returns>
        /// The <see cref="IServiceCollection"/>.
        /// </returns>
        [Obsolete("Use ConfigureTelemetryModule overload that accepts ApplicationInsightsServiceOptions.")]
        public static IServiceCollection ConfigureTelemetryModule<T>(this IServiceCollection services, Action<T> configModule)
            where T : ITelemetryModule
        {
            if (configModule == null)
            {
                throw new ArgumentNullException(nameof(configModule));
            }
            return services.AddSingleton(
                typeof(ITelemetryModuleConfigurator),
                new TelemetryModuleConfigurator((config, options) => configModule((T)config), typeof(T)));
        }

        /// <summary>
        /// Extension method to provide configuration logic for application insights telemetry module.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
        /// <param name="configModule">Action used to configure the module.</param>
            Action<T, ApplicationInsightsServiceOptions> configModule)
            where T : ITelemetryModule
        {
            if (configModule == null)
            {
                throw new ArgumentNullException(nameof(configModule));
            }

            return services.AddSingleton(
                typeof(ITelemetryModuleConfigurator),
            }

            var telemetryConfigValues = new List<KeyValuePair<string, string>>();

            bool wasAnythingSet = false;

            if (developerMode != null)
            {
                telemetryConfigValues.Add(new KeyValuePair<string, string>(
                    DeveloperModeForWebSites,
            {
                telemetryConfigValues.Add(new KeyValuePair<string, string>(InstrumentationKeyForWebSites, instrumentationKey));
                wasAnythingSet = true;
            }

            if (endpointAddress != null)
            {
                telemetryConfigValues.Add(new KeyValuePair<string, string>(EndpointAddressForWebSites, endpointAddress));
                wasAnythingSet = true;
            }

            if (wasAnythingSet)
            {
                configurationSourceRoot.Add(new MemoryConfigurationSource() { InitialData = telemetryConfigValues });
            }

            return configurationSourceRoot;
        }

        /// <summary>
        /// Read configuration from appSettings.json, appsettings.{env.EnvironmentName}.json,
        /// IConfiguation used in an application and EnvironmentVariables.
        /// Bind configuration to ApplicationInsightsServiceOptions.
        /// Values can also be read from environment variables to support azure web sites configuration.
        /// </summary>
        /// <param name="config">Configuration to read variables from.</param>
        /// <param name="serviceOptions">Telemetry configuration to populate.</param>
        internal static void AddTelemetryConfiguration(
            IConfiguration config,
            ApplicationInsightsServiceOptions serviceOptions)
                config.GetSection(ApplicationInsightsSectionFromConfig).Bind(serviceOptions);
                config.GetSection(TelemetryChannelSectionFromConfig).Bind(serviceOptions);

                if (config.TryGetValue(primaryKey: ConnectionStringEnvironmentVariable, backupKey: ConnectionStringFromConfig, value: out string connectionStringValue))
                {
                    serviceOptions.ConnectionString = connectionStringValue;
                }

                if (config.TryGetValue(primaryKey: InstrumentationKeyForWebSites, backupKey: InstrumentationKeyFromConfig, value: out string instrumentationKey))
                {
                {
            if (backupKey != null && string.IsNullOrWhiteSpace(value))
            {
                value = config[backupKey];
            }

            return !string.IsNullOrWhiteSpace(value);
        }

        private static bool IsApplicationInsightsAdded(IServiceCollection services)
        {
            // We treat TelemetryClient as a marker that AI services were added to service collection
            return services.Any(service => service.ServiceType == typeof(TelemetryClient));
        }

        private static void AddCommonInitializers(IServiceCollection services)
        {
#if AI_ASPNETCORE_WEB
            services.AddSingleton<ITelemetryInitializer, Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers.DomainNameRoleInstanceTelemetryInitializer>();
#else
            services.AddSingleton<ITelemetryInitializer, Microsoft.ApplicationInsights.WorkerService.TelemetryInitializers.DomainNameRoleInstanceTelemetryInitializer>();
#endif
            services.AddSingleton<ITelemetryInitializer, HttpDependenciesParsingTelemetryInitializer>();
            services.AddSingleton<ITelemetryInitializer, ComponentVersionTelemetryInitializer>();
        }

        private static void AddCommonTelemetryModules(IServiceCollection services)
        {
            // Previously users were encouraged to manually add the DiagnosticsTelemetryModule.
            services.TryAddEnumerable(ServiceDescriptor.Singleton<ITelemetryModule, DiagnosticsTelemetryModule>());

                provider.GetService<IOptions<TelemetryConfiguration>>().Value);
            services.AddSingleton<TelemetryClient>();
        }

        private static void AddAndConfigureDependencyTracking(IServiceCollection services)
        {
            services.AddSingleton<ITelemetryModule, DependencyTrackingTelemetryModule>();

            services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) =>
            {
                    excludedDomains.Add("core.cloudapi.de");
                    excludedDomains.Add("core.usgovcloudapi.net");

                    if (module.EnableLegacyCorrelationHeadersInjection)
                    {
                        excludedDomains.Add("localhost");
                        excludedDomains.Add("127.0.0.1");
                    }

                    var includedActivities = module.IncludeDiagnosticSourceActivities;
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddApplicationInsights();

                // The default behavior is to capture only logs above Warning level from all categories.
                // This can achieved with this code level filter -> loggingBuilder.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider>("",LogLevel.Warning);
                // However, this will make it impossible to override this behavior from Configuration like below using appsettings.json:
                // {
                //   "Logging": {
                //     "ApplicationInsights": {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
