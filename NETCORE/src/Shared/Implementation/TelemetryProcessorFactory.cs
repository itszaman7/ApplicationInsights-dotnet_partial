#if AI_ASPNETCORE_WEB
    namespace Microsoft.ApplicationInsights.AspNetCore
#else
    namespace Microsoft.ApplicationInsights.WorkerService
#endif
{
    using System;
    /// </summary>
    internal class TelemetryProcessorFactory : ITelemetryProcessorFactory
        private readonly IServiceProvider serviceProvider;
        private readonly Type telemetryProcessorType;
        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryProcessorFactory"/> class.
            this.serviceProvider = serviceProvider;
            this.telemetryProcessorType = telemetryProcessorType;
        }

        {
            return (ITelemetryProcessor)ActivatorUtilities.CreateInstance(this.serviceProvider, this.telemetryProcessorType, next);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
