namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.WindowsServer;
    using Microsoft.ApplicationInsights.WorkerService;
    using Microsoft.ApplicationInsights.WorkerService.Implementation.Tracing;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> that allow adding Application Insights services to application.
    {
        /// <summary>
        /// Adds Application Insights services into service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
        /// <param name="instrumentationKey">Instrumentation key to use for telemetry.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        [Obsolete("InstrumentationKey based global ingestion is being deprecated. Use the AddApplicationInsightsTelemetryWorkerService() overload which accepts Action<ApplicationInsightsServiceOptions> and set ApplicationInsightsServiceOptions.ConnectionString. See https://github.com/microsoft/ApplicationInsights-dotnet/issues/2560 for more details.")]
        public static IServiceCollection AddApplicationInsightsTelemetryWorkerService(
            this IServiceCollection services,
        /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
        /// <param name="configuration">Configuration to use for sending telemetry.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddApplicationInsightsTelemetryWorkerService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetryWorkerService(options => AddTelemetryConfiguration(configuration, options));
            return services;
        }
        /// The <see cref="IServiceCollection"/>.
        /// </returns>
        public static IServiceCollection AddApplicationInsightsTelemetryWorkerService(
            this IServiceCollection services,
            Action<ApplicationInsightsServiceOptions> options)
        {
            services.AddApplicationInsightsTelemetryWorkerService();
            services.Configure(options);
            return services;
        }
        {
            services.AddApplicationInsightsTelemetryWorkerService();
            services.Configure((ApplicationInsightsServiceOptions o) => options.CopyPropertiesTo(o));
            return services;
        }

        /// <summary>
        /// Adds Application Insights services into service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>

                    AddDefaultApplicationIdProvider(services);
                    AddTelemetryConfigAndClient(services);
                    AddApplicationInsightsLoggerProvider(services);
                }

                return services;
            }
            catch (Exception e)
            {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
