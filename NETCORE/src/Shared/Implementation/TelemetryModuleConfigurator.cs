#if AI_ASPNETCORE_WEB
    namespace Microsoft.ApplicationInsights.AspNetCore
#else
    namespace Microsoft.ApplicationInsights.WorkerService
#endif
{
    using System;
    using System.Diagnostics.CodeAnalysis;

#if AI_ASPNETCORE_WEB
#else
    using Microsoft.ApplicationInsights.WorkerService;
#endif
    using Microsoft.ApplicationInsights.Extensibility;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryModuleConfigurator"/> class.
        /// </summary>
        {
            this.configure = configure;
            this.TelemetryModuleType = telemetryModuleType;
        }
        /// </summary>
        public Type TelemetryModuleType { get; }

        [Obsolete("Use Configure(ITelemetryModule telemetryModule, ApplicationInsightsServiceOptions options) instead.", true)]
        [SuppressMessage("Documentation Rules", "SA1600:ElementsMustBeDocumented", Justification = "This method is obsolete.")]

        /// <summary>
        /// Configures telemetry module.
        /// </summary>


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
