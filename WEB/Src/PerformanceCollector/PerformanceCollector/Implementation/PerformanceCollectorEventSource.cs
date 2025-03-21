namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation
{
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Tracing;
    using Microsoft.ApplicationInsights.Common;

#if REDFIELD
    [EventSource(Name = "Redfield-Microsoft-ApplicationInsights-Extensibility-PerformanceCollector")]
#else
    [EventSource(Name = "Microsoft-ApplicationInsights-Extensibility-PerformanceCollector")]
#endif
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "appDomainName is required")]
    internal sealed class PerformanceCollectorEventSource : EventSource
    {
        private readonly ApplicationNameProvider applicationNameProvider = new ApplicationNameProvider();

        private PerformanceCollectorEventSource()
        {
        }

        public static PerformanceCollectorEventSource Log { get; } = new PerformanceCollectorEventSource();

        #region Infra init - success

        [Event(1, Level = EventLevel.Informational, Message = @"Performance counter infrastructure is being initialized. {0}")]
        public void ModuleIsBeingInitializedEvent(
            string message,
            string applicationName = "dummy")
        {
            this.WriteEvent(1, message, this.applicationNameProvider.Name);
        }

        [Event(3, Level = EventLevel.Informational, Message = @"Performance counter {0} has been successfully registered with performance collector.")]
        public void CounterRegisteredEvent(string counter, string applicationName = "dummy")
        {
            this.WriteEvent(3, counter, this.applicationNameProvider.Name);
        }

        [Event(4, Level = EventLevel.Informational, Message = @"Performance counters have been refreshed. Refreshed counters count is {0}.")]
        public void CountersRefreshedEvent(
            string countersRefreshedCount,
            string applicationName = "dummy")
        }

#endregion

#region Infra init - failure

        [Event(5, Keywords = Keywords.UserActionable, Level = EventLevel.Warning, Message = @"Performance counter {1} has failed to register with performance collector. Please make sure it exists. Technical details: {0}")]
        public void CounterRegistrationFailedEvent(string e, string counter, string applicationName = "dummy")
        {
            this.WriteEvent(5, e, counter, this.applicationNameProvider.Name);
        }

        [Event(6, Keywords = Keywords.UserActionable, Level = EventLevel.Warning, Message = @"Performance counter specified as {1} in ApplicationInsights.config was not parsed correctly. Please make sure that a proper name format is used. Expected formats are \category(instance)\counter or \category\counter. Technical details: {0}")]
        public void CounterParsingFailedEvent(string e, string counter, string applicationName = "dummy")
        {
            this.WriteEvent(6, e, counter, this.applicationNameProvider.Name);
        }

        [Event(8, Keywords = Keywords.UserActionable, Level = EventLevel.Error,
            Message = @"Error collecting {0} of the configured performance counters. Please check the configuration.
{1}")]
        public void CounterCheckConfigurationEvent(
            string misconfiguredCountersCount,
            string e,
            string applicationName = "dummy")
        {
            this.WriteEvent(8, misconfiguredCountersCount, e, this.applicationNameProvider.Name);
        }

        // Verbosity is Error - so it is always sent to portal; Keyword is Diagnostics so throttling is not applied.
            this.WriteEvent(10, counterCount, operationDurationInMs, this.applicationNameProvider.Name);
        }

#endregion

#region Data reading - failure

        [Event(11, Level = EventLevel.Warning, Message = @"Performance counter {1} has failed the reading operation. Error message: {0}")]
        public void CounterReadingFailedEvent(string e, string counter, string applicationName = "dummy")
        {
        }

#endregion

#endregion

#region Data sending - failure

        [Event(12, Level = EventLevel.Warning, Message = @"Failed to send a telemetry item for performance collector. Error text: {0}")]
        public void TelemetrySendFailedEvent(string e, string applicationName = "dummy")
        {
            this.WriteEvent(12, e, this.applicationNameProvider.Name);
        }

        {
            this.WriteEvent(13, e, this.applicationNameProvider.Name);
        }

#endregion

#region Troubleshooting

        [Event(14, Message = "{0}", Level = EventLevel.Verbose)]
        public void TroubleshootingMessageEvent(string message, string applicationName = "dummy")
        public void PerfCounterNetCoreOnlyOnAzureWebApp(string applicationName = "dummy")
        {
            this.WriteEvent(21, this.applicationNameProvider.Name);
        }

        [Event(22, Keywords = Keywords.UserActionable, Level = EventLevel.Error, Message = @"Performance counter is not available in the supported list of XPlatform counters. Counter is {0}.")]
        public void CounterNotXPlatformSupported(
    string counterName,
    string applicationName = "dummy")
        {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
