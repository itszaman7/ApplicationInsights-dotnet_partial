namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Web;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Web.Implementation;

    /// <summary>
    /// Listens to ASP.NET DiagnosticSource and enables instrumentation with Activity: let ASP.NET create root Activity for the request.
    /// </summary>
    public class AspNetDiagnosticTelemetryModule : IObserver<DiagnosticListener>, IDisposable, ITelemetryModule
    {
        private const string AspNetListenerName = "Microsoft.AspNet.TelemetryCorrelation";
        private const string IncomingRequestEventName = "Microsoft.AspNet.HttpReqIn";
        private const string IncomingRequestStartEventName = "Microsoft.AspNet.HttpReqIn.Start";
        private const string IncomingRequestStopEventName = "Microsoft.AspNet.HttpReqIn.Stop";

        private IDisposable allListenerSubscription;
        private RequestTrackingTelemetryModule requestModule;
        private ExceptionTrackingTelemetryModule exceptionModule;

        private IDisposable aspNetSubscription;

        /// <summary>
        /// Indicates if module initialized successfully.
        /// </summary>
        private bool isEnabled = true;

        /// <summary>
        /// Initializes the telemetry module.
        /// </summary>
        /// <param name="configuration">Telemetry configuration to use for initialization.</param>
        public void Initialize(TelemetryConfiguration configuration)
        {
            try
            {
                foreach (var module in TelemetryModules.Instance.Modules)
                {
                    if (module is RequestTrackingTelemetryModule requestTrackingModule)
                    {
                        this.requestModule = requestTrackingModule;
                    }
                    else if (module is ExceptionTrackingTelemetryModule exceptionTracingModule)
                    {
                        this.exceptionModule = exceptionTracingModule;
        /// <summary>
        /// Implements IObserver OnNext callback, subscribes to AspNet DiagnosticSource.
        /// </summary>
        /// <param name="value">DiagnosticListener value.</param>
        public void OnNext(DiagnosticListener value)
            {
                var eventListener = new AspNetEventObserver(this.requestModule, this.exceptionModule);
                this.aspNetSubscription = value.Subscribe(eventListener, AspNetEventObserver.IsEnabled, AspNetEventObserver.OnActivityImport);
            }
        }

        /// <summary>
        /// Disposes all subscriptions to DiagnosticSources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        #region IObserver

        /// <summary>
        /// IObserver OnError callback.
        /// </summary>

                Activity currentActivity = Activity.Current;

                if (name == IncomingRequestEventName && 
                    activityObj is Activity && 
                    currentActivity != null && 
                    currentActivity.OperationName == IncomingRequestEventName)
                {
                    // this is a first IsEnabled call without context that ensures that Activity instrumentation is on
                    // and Activity was created by TelemetryCorrelation module
                            WebEventSource.Log.HttpRequestNotAvailable(ex.Message, ex.StackTrace);
                            return;
                        }

                        // parse custom headers if enabled
                        if (ActivityHelpers.RootOperationIdHeaderName != null)
                        {
                            var rootId = StringUtilities.EnforceMaxLength(
                                request.UnvalidatedGetHeader(ActivityHelpers.RootOperationIdHeaderName),
                                InjectionGuardConstants.RequestHeaderMaxLength);
                            {
                                activity.SetParentId(rootId);
                            }
                        }

                        // even if there was no parent, parse Correlation-Context
                        // length requirements are in https://osgwiki.com/index.php?title=CorrelationContext&oldid=459234
                        request.Headers.ReadActivityBaggage(activity);
                    }
                }

                    if (value.Key == IncomingRequestStartEventName)
                    {
                        this.requestModule?.OnBeginRequest(context);
                    }
                    else if (value.Key == IncomingRequestStopEventName)
                    {
                        // AppInsights overrides Activity to allow Request-Id and legacy headers backward compatible mode
                        // it it was overriden, we need to restore it here.
                        var overrideActivity = (Activity)context.Items[ActivityHelpers.RequestActivityItemName];
                        if (overrideActivity != null && Activity.Current != overrideActivity)
                        {
                            Activity.Current = overrideActivity;
                        }

                        if (IsFirstRequest(context))
                        {
                            this.exceptionModule?.OnError(context);
                            this.requestModule?.OnEndRequest(context);
                        }
                catch (Exception e)
                {
                    WebEventSource.Log.UnknownError(e.ToString());
                }
            }

            #region IObserver

            public void OnError(Exception error)
            {
            }

            public void OnCompleted()
            {
            }

            #endregion

            private static bool IsFirstRequest(HttpContext context)
            {
                var firstRequest = true;
                try
                {
                    if (context != null)
                    {
                        firstRequest = context.Items[FirstRequestFlag] == null;
                        if (firstRequest)
                        {
                            context.Items.Add(FirstRequestFlag, true);
                        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
