namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// ETW EventSource tracing class.
    /// </summary>
#if REDFIELD
    [EventSource(Name = "Redfield-Microsoft-ApplicationInsights-Extensibility-DependencyCollector")]
#else
    [EventSource(Name = "Microsoft-ApplicationInsights-Extensibility-DependencyCollector")]
#endif
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "appDomainName is required")]
    internal sealed class DependencyCollectorEventSource : EventSource
    {
        public static readonly DependencyCollectorEventSource Log = new DependencyCollectorEventSource();
        private readonly ApplicationNameProvider applicationNameProvider = new ApplicationNameProvider();

        private DependencyCollectorEventSource()
        {
        }

        [Event(
            1,
            Keywords = Keywords.RddEventKeywords,
            Message = "[msg=RemoteDependencyModule failed];[msg={0}];[fwv={1}];",
            Level = EventLevel.Error)]
        public void RemoteDependencyModuleError(string msg, string frameworkVersion, string appDomainName = "Incorrect")
        {
            this.WriteEvent(1, msg ?? string.Empty, frameworkVersion ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(
            2,
            Keywords = Keywords.RddEventKeywords,
            Message = "[msg=DependencyOperationTelemetryInitializerFailed];[msg={0}]",
            Level = EventLevel.Error)]
        public void DependencyOperationTelemetryInitializerError(string msg, string appDomainName = "Incorrect")
        {
            this.WriteEvent(2, msg, this.applicationNameProvider.Name);
        }

        [Event(
            3,
            Keywords = Keywords.RddEventKeywords,
            Message = "[msg=DependencyOperationNameNull];",
            Level = EventLevel.Warning)]
        public void DependencyOperationNameNullWarning(string appDomainName = "Incorrect")
        {
            this.WriteEvent(3, this.applicationNameProvider.Name);
            Message = "[msg=RemoteDependencyModule verbose.];[msg={0}]",
            Level = EventLevel.Verbose)]
        public void RemoteDependencyModuleVerbose(string msg, string appDomainName = "Incorrect")
        {
            this.WriteEvent(6, msg, this.applicationNameProvider.Name);
        }

        [Event(
            7,
            Keywords = Keywords.RddEventKeywords,
        [Event(
            8,
            Keywords = Keywords.RddEventKeywords,
            Message = "[msg=RemoteDependencyModule information.];[msg={0}]",
            Level = EventLevel.Informational)]
        public void RemoteDependencyModuleInformation(string msg, string appDomainName = "Incorrect")
        {
            this.WriteEvent(8, msg ?? string.Empty, this.applicationNameProvider.Name);
        }

            Keywords = Keywords.RddEventKeywords,
            Message = "Begin callback called for id = '{0}', name= '{1}'",
            Level = EventLevel.Verbose)]
        public void BeginCallbackCalled(long id, string name, string appDomainName = "Incorrect")
        {
            this.WriteEvent(10, id, name ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(
            11,
            Level = EventLevel.Warning)]
        public void EndCallbackWithNoBegin(string id, string appDomainName = "Incorrect")
        {
            this.WriteEvent(12, id, this.applicationNameProvider.Name);
        }

        [NonEvent]
        public void CallbackError(long id, string callbackName, Exception exception)
        {
            string idString = null;
                idString = id.ToString(CultureInfo.InvariantCulture);
            }
            else if (this.IsEnabled(EventLevel.Error, Keywords.RddEventKeywords))
            {
                idString = "_";
            }
            else
            {
                return;
            }

            this.CallbackError(idString, callbackName ?? string.Empty, exception == null ? "null" : exception.ToInvariantString());
        }

        [Event(
            13,
            Keywords = Keywords.RddEventKeywords,
            Message = "Callback '{1}' failed for id = '{0}'. Exception: {2}",
            Level = EventLevel.Error)]
        public void CallbackError(string id, string callbackName, string exceptionString, string appDomainName = "Incorrect")
        /// </summary>
        [Event(16, Message = "WebRequest is null.", Level = EventLevel.Warning)]
        public void WebRequestIsNullWarning(string appDomainName = "Incorrect")
        {
            this.WriteEvent(16, this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Logs the information when a telemetry item that is already existing in the tables (that is currently being tracked) is tracked again.
        /// </summary>
        }

        /// <summary>
        /// Logs the information when the telemetry item to track is null.
        /// </summary>
        [Event(18, Message = "Telemetry to track is null.", Level = EventLevel.Warning)]
        public void TelemetryToTrackIsNullWarning(string appDomainName = "Incorrect")
        {
            this.WriteEvent(18, this.applicationNameProvider.Name);
        }

        [Event(19, 
            Message = "RemoteDependency profiler failed to attach. Collection will default to EventSource implementation. Error details: {0}",
            Level = EventLevel.Error)]
        public void ProfilerFailedToAttachError(string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(19, error ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(
            20,
            Keywords = Keywords.RddEventKeywords,
            Message = "UnexpectedCallbackParameter. Expected type: {0}.",
            Level = EventLevel.Warning)]
        public void UnexpectedCallbackParameter(string expectedType, string appDomainName = "Incorrect")
        {
            this.WriteEvent(20, expectedType ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(
            Keywords = Keywords.RddEventKeywords,
            Message = "End async callback called for id = '{0}'",
            Level = EventLevel.Verbose)]
        public void EndAsyncCallbackCalled(string id, string appDomainName = "Incorrect")
        {
            this.WriteEvent(21, id, this.applicationNameProvider.Name);
        }

        [Event(
           22,
           Message = "End async exception callback called for id = '{0}'",
           Level = EventLevel.Verbose)]
        public void EndAsyncExceptionCallbackCalled(string id, string appDomainName = "Incorrect")
        {
            this.WriteEvent(22, id, this.applicationNameProvider.Name);
        }

        [Event(
            23,
            Message = "Current Activity is null for event = '{0}'",
            27,
            Keywords = Keywords.RddEventKeywords,
            Message = "HttpDesktopDiagnosticSourceListener: Begin callback called for id = '{0}', name= '{1}'",
            Level = EventLevel.Verbose)]
        public void HttpDesktopBeginCallbackCalled(long id, string name, string appDomainName = "Incorrect")
        {
            this.WriteEvent(27, id, name ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(
            Message = "HttpDesktopDiagnosticSourceListener: End callback called for id = '{0}'",
            Level = EventLevel.Verbose)]
        public void HttpDesktopEndCallbackCalled(long id, string appDomainName = "Incorrect")
        {
            this.WriteEvent(28, id, this.applicationNameProvider.Name);
        }

        [Event(
            29,
            Keywords = Keywords.RddEventKeywords,
            Message = "System.Net.Http.HttpRequestOut.Start id = '{0}'",
            Level = EventLevel.Verbose)]
        public void HttpCoreDiagnosticSourceListenerStart(string id, string appDomainName = "Incorrect")
        {
            this.WriteEvent(29, id, this.applicationNameProvider.Name);
        }

        [Event(
            30,
            Keywords = Keywords.RddEventKeywords,

        [Event(
            31,
            Keywords = Keywords.RddEventKeywords,
            Message = "System.Net.Http.Request id = '{0}'",
            Level = EventLevel.Verbose)]
        public void HttpCoreDiagnosticSourceListenerRequest(Guid id, string appDomainName = "Incorrect")
        {
            this.WriteEvent(31, id, this.applicationNameProvider.Name);
        }
            Message = "HttpHandlerDiagnosticListener failed to initialize. Error details '{0}'",
            Level = EventLevel.Error)]
        public void HttpHandlerDiagnosticListenerFailedToInitialize(string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(36, error ?? string.Empty, this.applicationNameProvider.Name);
        }
        
        [Event(
        }

        [Event(
            40,
            Keywords = Keywords.RddEventKeywords,
            Message = "SqlClientDiagnosticSourceListener OnNext failed to call event handler. Error details '{0}'",
            Level = EventLevel.Error)]
        public void SqlClientDiagnosticSourceListenerOnNextFailed(string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(40, error, this.applicationNameProvider.Name);
            41,
            Keywords = Keywords.RddEventKeywords,
            Message = "{0} failed to subscribe. Error details '{1}'",
            Level = EventLevel.Error)]
        public void DiagnosticSourceListenerFailedToSubscribe(string listenerName, string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(41, listenerName, error, this.applicationNameProvider.Name);
        }

        [Event(
        [Event(
            45,
            Keywords = Keywords.RddEventKeywords,
            Message = "Ending operation for dependency name {0}, not tracking this item.",
            Level = EventLevel.Verbose)]
        public void EndOperationNoTracking(string depName, string appDomainName = "Incorrect")
        {
            this.WriteEvent(45, depName, this.applicationNameProvider.Name);
        }

            46,
            Keywords = Keywords.RddEventKeywords,
            Message = "Not tracking operation for event = '{0}', id = '{1}', listener is not active.",
            Level = EventLevel.Verbose)]
        public void NotActiveListenerNoTracking(string evntName, string activityId, string appDomainName = "Incorrect")
        {
            this.WriteEvent(46, evntName, activityId, this.applicationNameProvider.Name);
        }

        [Event(
            47,
            Keywords = Keywords.RddEventKeywords,
            Message = "Detected Http Client instrumentation version {0} on for HttpClient version {1}.{2} with informational version {3}.",
            Level = EventLevel.Verbose)]
        public void HttpCoreDiagnosticListenerInstrumentationVersion(int httpInstrumentationVersion, int httpClientMajorVersion, int httpClientMinorVersion, string infoVersion, string appDomainName = "Incorrect")
        {
            this.WriteEvent(47, httpInstrumentationVersion, httpClientMajorVersion, httpClientMinorVersion, infoVersion, this.applicationNameProvider.Name);
        }

        [Event(
            48,
            Keywords = Keywords.RddEventKeywords,
            Message = "Http request is already instrumented.",
            Level = EventLevel.Verbose)]
        public void HttpRequestAlreadyInstrumented(string appDomainName = "Incorrect")
        {
            this.WriteEvent(48, this.applicationNameProvider.Name);
        }

        [Event(
            49,
            Keywords = Keywords.RddEventKeywords,
            Message = "Failed to parse Url '{0}'",
            Level = EventLevel.Warning)]
        public void FailedToParseUrl(string url, string appDomainName = "Incorrect")
        {
            this.WriteEvent(49, url, this.applicationNameProvider.Name);
        }

        /// <summary>
        public sealed class Keywords
        {
            /// <summary>
            /// Key word for user actionable events.
            /// </summary>
            public const EventKeywords UserActionable = (EventKeywords)0x1;

            /*  Reserve first 3 for other service keywords
             *  public const EventKeywords Service1 = (EventKeywords)0x2;
             *  public const EventKeywords Service2 = (EventKeywords)0x4;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
