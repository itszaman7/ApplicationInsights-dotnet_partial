#if AI_ASPNETCORE_WEB
    namespace Microsoft.ApplicationInsights.AspNetCore
#else
namespace Microsoft.ApplicationInsights.WorkerService
#endif
#else
    using Microsoft.ApplicationInsights.WorkerService;
#endif
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    {
        /// <summary>
        /// Gets the type of <see cref="ITelemetryModule"/> to be configured.
        /// </summary>
        Type TelemetryModuleType { get; }

        [Obsolete("Use Configure(ITelemetryModule telemetryModule, ApplicationInsightsServiceOptions options) instead.")]
        [SuppressMessage("Documentation Rules", "SA1600:ElementsMustBeDocumented", Justification = "This method is obsolete.")]
        [SuppressMessage("Documentation Rules", "SA1611:ElementParametersMustBeDocumented", Justification = "This method is obsolete.")]
        /// Configures the given <see cref="ITelemetryModule"/>.
        /// </summary>
        /// <param name="telemetryModule">Module to be configured.</param>


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
