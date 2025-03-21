﻿namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Tracing;

#if REDFIELD
    [EventSource(Name = "Redfield-Microsoft-ApplicationInsights-Core")]
#else
    [EventSource(Name = "Microsoft-ApplicationInsights-Core")]
#endif
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "appDomainName is required")]
    internal sealed class CoreEventSource : EventSource
    {
        public static readonly CoreEventSource Log = new CoreEventSource();

#if NETSTANDARD2_0
        public EventCounter IngestionResponseTimeCounter;
#endif

        private readonly ApplicationNameProvider nameProvider = new ApplicationNameProvider();

        internal CoreEventSource()
        {
#if NETSTANDARD2_0
            this.IngestionResponseTimeCounter = new EventCounter("IngestionEndpoint-ResponseTimeMsec", this);
#endif
        }

        public static bool IsVerboseEnabled
        {
            [NonEvent]
            get
            {
                return Log.IsEnabled(EventLevel.Verbose, (EventKeywords)(-1));
            }
        }

        /// <summary>
        /// Logs the information when there operation to track is null.
        /// </summary>
        [Event(1, Message = "Operation object is null.", Level = EventLevel.Warning)]
        public void OperationIsNullWarning(string appDomainName = "Incorrect")
        {
            this.WriteEvent(1, this.nameProvider.Name);
        }

        /// <summary>
        /// Logs the information when there operation to stop does not match the current operation.
        /// </summary>
        [Event(2, Message = "Operation to stop does not match the current operation. Telemetry is not tracked.", Level = EventLevel.Error)]
        public void InvalidOperationToStopError(string appDomainName = "Incorrect")
        {
            this.WriteEvent(2, this.nameProvider.Name);
        }

        [Event(
            3,
            Keywords = Keywords.VerboseFailure,
            Message = "[msg=Log verbose];[msg={0}]",
            Level = EventLevel.Verbose)]
        public void LogVerbose(string msg, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                3,
                msg ?? string.Empty,
                this.nameProvider.Name);
        }
        
        [Event(
            4,
            Keywords = Keywords.Diagnostics | Keywords.UserActionable,
            Message = "Diagnostics event throttling has been started for the event {0}",
            Level = EventLevel.Informational)]
        public void DiagnosticsEventThrottlingHasBeenStartedForTheEvent(
            string eventId,
            string appDomainName = "Incorrect")
        {
            this.WriteEvent(4, eventId ?? "NULL", this.nameProvider.Name);
        }

        [Event(
            5,
            Keywords = Keywords.Diagnostics | Keywords.UserActionable,
            Message = "Diagnostics event throttling has been reset for the event {0}, event was fired {1} times during last interval",

        [Event(
            8,
            Keywords = Keywords.Diagnostics,
            Message = "A scheduler timer was removed",
            Level = EventLevel.Verbose)]
        public void DiagnoisticsEventThrottlingSchedulerTimerWasRemoved(string appDomainName = "Incorrect")
        {
            this.WriteEvent(8, this.nameProvider.Name);
        }
            this.WriteEvent(12, this.nameProvider.Name);
        }

        [Event(
           13,
           Message = "Telemetry tracking was enabled. Messages are being logged.",
           Level = EventLevel.Verbose)]
        public void TrackingWasEnabled(string appDomainName = "Incorrect")
        {
            this.WriteEvent(13, this.nameProvider.Name);
            Keywords = Keywords.ErrorFailure,
            Message = "[msg=Log Error];[msg={0}]",
            Level = EventLevel.Error)]
        public void LogError(string msg, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                14, 
                msg ?? string.Empty,
                this.nameProvider.Name);
        }
                this.nameProvider.Name);
        }

        [Event(
            17,
            Keywords = Keywords.UserActionable,
            Message = "ApplicationInsights configuration file loading failed. Type '{0}' does not have property '{1}'. Property initialization was skipped. Monitoring will continue.",
            Level = EventLevel.Error)]
        public void IncorrectPropertyConfigurationError(string type, string property, string appDomainName = "Incorrect")
        {
                property ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            18,
            Keywords = Keywords.UserActionable,
            Message = "ApplicationInsights configuration file loading failed. Element '{0}' element does not have a Type attribute, does not specify a value and is not a valid collection type. Type initialization was skipped. Monitoring will continue.",
            Level = EventLevel.Error)]
        public void IncorrectInstanceAtributesConfigurationError(string definition, string appDomainName = "Incorrect")
                this.nameProvider.Name);
        }

        [Event(
            20,
            Keywords = Keywords.UserActionable,
            Message = "ApplicationInsights configuration file loading failed. Exception: '{0}'. Monitoring will continue if you set InstrumentationKey programmatically.",
            Level = EventLevel.Error)]
        public void ConfigurationFileCouldNotBeParsedError(string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                20,
                error ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            21,
            Keywords = Keywords.UserActionable,
            Message = "ApplicationInsights configuration file loading failed. Type '{0}' will not be create. Error: '{1}'. Monitoring will continue if you set InstrumentationKey programmatically.",
        [Event(
            22,
            Keywords = Keywords.UserActionable,
            Message = "ApplicationInsights configuration file loading failed. Type '{0}' will not be initialized. Error: '{1}'. Monitoring will continue if you set InstrumentationKey programmatically.",
            Level = EventLevel.Error)]
        public void ComponentInitializationConfigurationError(string type, string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                22,
                type ?? string.Empty,
            Level = EventLevel.Warning)]
        public void ApplicationInsightsConfigNotFoundWarning(string file, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                23,
                file ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
           Level = EventLevel.Error)]
        public void FailedToGetMachineName(string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                25,
                error ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            Level = EventLevel.Error)]
        public void FailedToFlushMetricAggregators(string ex, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                26,
                ex ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            27,
            Message = "Failed to snapshot aggregated metrics. Exception: {0}.",
            Level = EventLevel.Error)]
        public void FailedToSnapshotMetricAggregators(string ex, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                27,
                ex ?? string.Empty,
                this.nameProvider.Name);
        }
            Level = EventLevel.Error)]
        public void FailedToRunMetricProcessor(string processorName, string ex, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                28,
                processorName ?? string.Empty,
                ex ?? string.Empty,
                this.nameProvider.Name);
        }

        {
            this.WriteEvent(
                29,
                maxBacklogSize,               
                this.nameProvider.Name);
        }

        [Event(
            30,
            Message = "Flush was called on the telemetry channel (InMemoryChannel) after it was disposed.",

        [Event(
            32,
            Message = "Failed to get environment variables due to security exception; code is likely running in partial trust. Exception: {0}.",
            Level = EventLevel.Warning)]
        public void FailedToLoadEnvironmentVariables(string ex, string appDomainName = "Incorrect")
        {
            this.WriteEvent(32, ex, this.nameProvider.Name);
        }

        // Verbosity is Error - so it is always sent to portal; Keyword is Diagnostics so throttling is not applied.
        [Event(33,
            Message = "A Metric Extractor detected a telemetry item with SamplingPercentage < 100. Metrics Extractors should be used before Sampling Processors or any other Telemetry Processors that might filter out Telemetry Items. Otherwise, extracted metrics may be incorrect.",
            Level = EventLevel.Error,
            Keywords = Keywords.Diagnostics | Keywords.UserActionable)]
        public void MetricExtractorAfterSamplingError(string appDomainName = "Incorrect")
        {
            this.WriteEvent(33, this.nameProvider.Name);
        }

            this.WriteEvent(34, this.nameProvider.Name);
        }

        [Event(35, Message = "Item was rejected because it has no instrumentation key set. Item: {0}", Level = EventLevel.Verbose)]
        public void ItemRejectedNoInstrumentationKey(string item, string appDomainName = "Incorrect")
        {
            this.WriteEvent(35, item ?? string.Empty, this.nameProvider.Name);
        }

        [Event(
        public void FailedToAddHeartbeatProperty(string heartbeatProperty, string heartbeatPropertyValue, string ex = null, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                37,
                heartbeatProperty ?? string.Empty,
                heartbeatPropertyValue ?? string.Empty,
                ex ?? string.Empty,
                this.nameProvider.Name);
        }

                38,
                heartbeatPropertyValue ?? string.Empty,
                isHealthy,
                this.nameProvider.Name);
        }

        [Event(
            39,
            Message = "Could not set heartbeat payload property '{0}' = {1}, isHealthy was set = {2}, isHealthy value = {3}. Exception: {4}.",
            Level = EventLevel.Warning)]
        public void FailedToSetHeartbeatProperty(string heartbeatProperty, string heartbeatPropertyValue, bool isHealthyHasValue, bool isHealthy, string ex = null, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                39,
                heartbeatProperty ?? string.Empty,
                heartbeatPropertyValue ?? string.Empty,
                isHealthyHasValue,
                isHealthy,
                ex ?? string.Empty,
                this.nameProvider.Name);
                heartbeatPropertyValue ?? string.Empty,
                isHealthyHasValue,
                isHealthy,
                this.nameProvider.Name);
        }

        [Event(
            41,
            Keywords = Keywords.UserActionable,
            Message = "Failed to retrieve Application Id for the current application insights resource. Make sure the configured instrumentation key is valid. Error: {0}",
            this.WriteEvent(41, exception, this.nameProvider.Name);
        }

        [Event(
            42,
            Keywords = Keywords.UserActionable,
            Message = "Failed to retrieve Application Id for the current application insights resource. Endpoint returned HttpStatusCode: {0}",
            Level = EventLevel.Warning)]
        public void ApplicationIdProviderFetchApplicationIdFailedWithResponseCode(string httpStatusCode, string appDomainName = "Incorrect")
        {
            this.WriteEvent(42, httpStatusCode, this.nameProvider.Name);
        }

        [Event(
            43,
            Keywords = Keywords.UserActionable,
            Message = "Process was called on the TelemetrySink after it was disposed, the telemetry data was dropped.",
            Level = EventLevel.Error)]
        public void TelemetrySinkCalledAfterBeingDisposed(string appDomainName = "Incorrect")
        {
        }

        /// <summary>
        /// Logs the details when there operation to stop does not match the current operation.
        /// </summary>
        [Event(44, Message = "Operation to stop does not match the current operation. Details {0}.", Level = EventLevel.Warning)]
        public void InvalidOperationToStopDetails(string details, string appDomainName = "Incorrect")
        {
            this.WriteEvent(44, details, this.nameProvider.Name);
        }
        [Event(47, Message = "Connection String exceeds max length of {0} characters.", Level = EventLevel.Error, Keywords = Keywords.UserActionable)]
        public void ConnectionStringExceedsMaxLength(int maxLength, string appDomainName = "Incorrect") => this.WriteEvent(47, maxLength, this.nameProvider.Name);

        [Event(48, Message = "Connection String cannot be empty.", Level = EventLevel.Error, Keywords = Keywords.UserActionable)]
        public void ConnectionStringEmpty(string appDomainName = "Incorrect") => this.WriteEvent(48, this.nameProvider.Name);

        [Event(49, Message = "Connection String cannot contain duplicate keys.", Level = EventLevel.Error, Keywords = Keywords.UserActionable)]
        [Event(65, Keywords = Keywords.UserActionable, Message = "Error loading http module type from assembly {0}, type name {1}, exception: {2}.", Level = EventLevel.Error)]
        public void HttpModuleLoadingError(string assemblyName, string moduleName, string exception, string appDomainName = "Incorrect") => this.WriteEvent(65, assemblyName ?? string.Empty, moduleName ?? string.Empty, exception ?? string.Empty, this.nameProvider.Name);
        */

        [Event(66, Message = "Call to WindowsIdentity.Current failed with the exception: {0}.", Level = EventLevel.Warning)]
        public void LogWindowsIdentityAccessSecurityException(string error, string appDomainName = "Incorrect") => this.WriteEvent(66, error ?? string.Empty, this.nameProvider.Name);

        #endregion

        [Event(67, Message = "Backend has responded with {0} status code in {1}ms.", Level = EventLevel.Informational)]

        [Event(69, Message = "{0}", Level = EventLevel.Error, Keywords = Keywords.UserActionable)]
        public void ConnectionStringParseError(string message, string appDomainName = "Incorrect") => this.WriteEvent(69, message, this.nameProvider.Name);

        [NonEvent]
        [SuppressMessage("Microsoft.Performance", "CA1822: MarkMembersAsStatic", Justification = "This method does access instance data in NetStandard 2.0 scenarios.")]
        public void IngestionResponseTimeEventCounter(float responseDurationInMs)
        {
#if NETSTANDARD2_0
            this.IngestionResponseTimeCounter.WriteMetric(responseDurationInMs);

        [Event(72, Keywords = Keywords.UserActionable, Message = "Failed to create file for self diagnostics at {0}. Error message: {1}.", Level = EventLevel.Error)]
        public void SelfDiagnosticsFileCreateException(string logDirectory, string exception, string appDomainName = "Incorrect") => this.WriteEvent(72, logDirectory, exception, this.nameProvider.Name);

        [Event(73, Message = "Failed to get AAD Token. Error message: {0}.", Level = EventLevel.Error)]
        public void FailedToGetToken(string exception, string appDomainName = "Incorrect") => this.WriteEvent(73, exception, this.nameProvider.Name);

        [Event(74, Message = "Ingestion Service responded with redirect. {0}", Level = EventLevel.Informational)]
        public void IngestionRedirectInformation(string message, string appDomainName = "Incorrect") => this.WriteEvent(74, message, this.nameProvider.Name);

        public void TransmissionStatusEventFailed(Exception ex)
        {
            if (this.IsEnabled(EventLevel.Error, (EventKeywords)(-1)))
            {
                this.TransmissionStatusEventError(ex.ToInvariantString());
            }
        }

        /// <summary>
        /// Keywords for the PlatformEventSource.
        /// </summary>
        public sealed class Keywords
        {
            /// <summary>
            /// Key word for user actionable events.
            /// </summary>
            public const EventKeywords UserActionable = (EventKeywords)EventSourceKeywords.UserActionable;

            /// <summary>
            /// Keyword for errors that trace at Verbose level.
            /// <summary>
            /// Keyword for errors that trace at Verbose level.
            /// </summary>
            public const EventKeywords VerboseFailure = (EventKeywords)EventSourceKeywords.VerboseFailure;

            /// <summary>
            /// Keyword for errors that trace at Error level.
            /// </summary>
            public const EventKeywords ErrorFailure = (EventKeywords)EventSourceKeywords.ErrorFailure;
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
