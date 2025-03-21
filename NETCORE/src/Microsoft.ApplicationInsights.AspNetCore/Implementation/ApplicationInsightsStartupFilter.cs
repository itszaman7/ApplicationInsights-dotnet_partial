namespace Microsoft.ApplicationInsights.AspNetCore
{
    using System;
    using Microsoft.ApplicationInsights.AspNetCore.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.AspNetCore.Builder;
    internal class ApplicationInsightsStartupFilter : IStartupFilter
    {
        private readonly ILogger<ApplicationInsightsStartupFilter> logger;

        /// Initializes a new instance of the <see cref="ApplicationInsightsStartupFilter"/> class.
        /// </summary>
        /// <param name="logger">Instance of ILogger.</param>
        public ApplicationInsightsStartupFilter(ILogger<ApplicationInsightsStartupFilter> logger)
        {
            this.logger = logger;
                {
                    // Attempting to resolve TelemetryConfiguration triggers configuration of the same
                    // via <see cref="TelemetryConfigurationOptionsSetup"/> class which triggers
                    // initialization of TelemetryModules and construction of TelemetryProcessor pipeline.
                    var tc = app.ApplicationServices.GetService<TelemetryConfiguration>();
                }
                catch (Exception ex)
                {
                // Invoking next builder is not wrapped in try catch to ensure any exceptions gets propogated up.
                next(app);
            };
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
