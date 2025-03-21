namespace Microsoft.ApplicationInsights.WorkerService.Implementation.Tracing
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Tracing;
    using Microsoft.ApplicationInsights.Shared.Internals;

    /// <summary>
    /// Event source for Application Insights Worker Service SDK.
    /// </summary>
#if REDFIELD
    [EventSource(Name = "Redfield-Microsoft-ApplicationInsights-WorkerService")]
#else
    [EventSource(Name = "Microsoft-ApplicationInsights-WorkerService")]
#endif
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "appDomainName is required")]
    [SuppressMessage("", "SA1611:ElementParametersMustBeDocumented", Justification = "Internal only class.")]
    internal sealed class WorkerServiceEventSource : EventSource
    {
        /// <summary>
        /// The singleton instance of this event source.
        /// Due to how EventSource initialization works this has to be a public field and not
        /// a property otherwise the internal state of the event source will not be enabled.
        /// </summary>
        public static readonly WorkerServiceEventSource Instance = new WorkerServiceEventSource();
        private readonly ApplicationNameProvider applicationNameProvider = new ApplicationNameProvider();

        /// <summary>
        /// Prevents a default instance of the <see cref="WorkerServiceEventSource"/> class from being created.

        /// <summary>
        /// Logs informational message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="appDomainName">An ignored placeholder to make EventSource happy.</param>
        [Event(1, Message = "Message : {0}", Level = EventLevel.Warning, Keywords = Keywords.Diagnostics)]
        public void LogInformational(string message, string appDomainName = "Incorrect")
        /// <summary>
        /// Logs error message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="appDomainName">An ignored placeholder to make EventSource happy.</param>
        [Event(3, Message = "An error has occurred which may prevent application insights from functioning. Error message: '{0}'", Level = EventLevel.Error)]
        public void LogError(string message, string appDomainName = "Incorrect")
        {
        /// </summary>
        [Event(5, Keywords = Keywords.Diagnostics, Message = "An error has occurred while setting up TelemetryConfiguration. Error message: '{0}' ", Level = EventLevel.Error)]
        public void TelemetryConfigurationSetupFailure(string errorMessage, string appDomainName = "Incorrect")
        {
            this.WriteEvent(5, errorMessage, this.applicationNameProvider.Name);
        }

        /// <summary>
            Keywords = Keywords.Diagnostics,
            Message = "An error has occurred while initializing the TelemetryModule: '{0}'. Error message: '{1}' ",
            Level = EventLevel.Error)]
        public void TelemetryModuleInitialziationSetupFailure(string moduleName, string errorMessage, string appDomainName = "Incorrect")
        {
            this.WriteEvent(6, moduleName, errorMessage, this.applicationNameProvider.Name);
        }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
