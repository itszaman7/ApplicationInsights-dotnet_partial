namespace Microsoft.ApplicationInsights.AspNetCore.DiagnosticListeners
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.AspNetCore.DiagnosticListeners.Implementation;
    using Microsoft.ApplicationInsights.AspNetCore.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
    using Microsoft.ApplicationInsights.AspNetCore.Implementation;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Experimental;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.W3C;
    using Microsoft.ApplicationInsights.W3C;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// <see cref="IApplicationInsightDiagnosticListener"/> implementation that listens for events specific to AspNetCore hosting layer.
    /// </summary>
    internal class HostingDiagnosticListener : IApplicationInsightDiagnosticListener
    {
        /// <summary>
        /// Name of custom property to store the legacy RootId when operating in W3C mode. Backend/UI understands this property.
        /// </summary>
        internal const string LegacyRootIdProperty = "ai_legacyRootId";

        private const string ActivityCreatedByHostingDiagnosticListener = "ActivityCreatedByHostingDiagnosticListener";
        private const string ProactiveSamplingFeatureFlagName = "proactiveSampling";
        private const string ConditionalAppIdFeatureFlagName = "conditionalAppId";

        private static readonly ActiveSubsciptionManager SubscriptionManager = new ActiveSubsciptionManager();

        /// <summary>
        /// This class need to be aware of the AspNetCore major version.
        /// This will affect what DiagnosticSource events we receive.
        /// To support AspNetCore 1.0,2.0,3.0 we listen to both old and new events.
        /// If the running AspNetCore version is 2.0 or 3.0, both old and new events will be sent. In this case, we will ignore the old events.
        /// Also 3.0 is W3C Tracing Aware (i.e it populates Activity from traceparent headers) and hence SDK need to be aware.
        /// </summary>
        private readonly AspNetCoreMajorVersion aspNetCoreMajorVersion;

        private readonly bool proactiveSamplingEnabled = false;
        private readonly bool conditionalAppIdEnabled = false;

        private readonly TelemetryConfiguration configuration;
        private readonly TelemetryClient client;
        private readonly IApplicationIdProvider applicationIdProvider;
        private readonly string sdkVersion = SdkVersionUtils.GetVersion();
        private readonly bool injectResponseHeaders;
        private readonly bool trackExceptions;
        private readonly bool enableW3CHeaders;

        // fetch is unique per event and per property
        private readonly PropertyFetcher httpContextFetcherOnBeforeAction = new PropertyFetcher("httpContext");
        private readonly PropertyFetcher httpContextFetcherOnBeforeAction30 = new PropertyFetcher("HttpContext");
        private readonly PropertyFetcher routeDataFetcher = new PropertyFetcher("routeData");
        private readonly PropertyFetcher routeDataFetcher30 = new PropertyFetcher("RouteData");
        private readonly PropertyFetcher routeValuesFetcher = new PropertyFetcher("Values");
        private readonly PropertyFetcher httpContextFetcherStart = new PropertyFetcher("HttpContext");
        private readonly PropertyFetcher httpContextFetcherStop = new PropertyFetcher("HttpContext");
        private readonly PropertyFetcher httpContextFetcherDiagExceptionUnhandled = new PropertyFetcher("httpContext");
        private readonly PropertyFetcher httpContextFetcherDiagExceptionHandled = new PropertyFetcher("httpContext");
        private readonly PropertyFetcher exceptionFetcherDiagExceptionUnhandled = new PropertyFetcher("exception");
        private readonly PropertyFetcher exceptionFetcherDiagExceptionHandled = new PropertyFetcher("exception");

        private string lastIKeyLookedUp;
        private string lastAppIdUsed;

        /// <summary>
        /// Initializes a new instance of the <see cref="HostingDiagnosticListener"/> class.
        /// </summary>
        /// <param name="client"><see cref="TelemetryClient"/> to post traces to.</param>
        /// <param name="applicationIdProvider">Provider for resolving application Id to be used in multiple instruemntation keys scenarios.</param>
        /// <param name="injectResponseHeaders">Flag that indicates that response headers should be injected.</param>
        /// <param name="trackExceptions">Flag that indicates that exceptions should be tracked.</param>
        /// <param name="enableW3CHeaders">Flag that indicates that W3C header parsing should be enabled.</param>
        /// <param name="aspNetCoreMajorVersion">Major version of AspNetCore.</param>
        public HostingDiagnosticListener(
            TelemetryClient client,
            IApplicationIdProvider applicationIdProvider,
            bool injectResponseHeaders,
            bool trackExceptions,
            bool enableW3CHeaders,
            AspNetCoreMajorVersion aspNetCoreMajorVersion)
        {
            this.aspNetCoreMajorVersion = aspNetCoreMajorVersion;
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.applicationIdProvider = applicationIdProvider;
            this.injectResponseHeaders = injectResponseHeaders;
            this.trackExceptions = trackExceptions;
            this.enableW3CHeaders = enableW3CHeaders;
            AspNetCoreEventSource.Instance.HostingListenerInformational(this.aspNetCoreMajorVersion, "HostingDiagnosticListener constructed.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HostingDiagnosticListener"/> class.
        /// </summary>
        /// <param name="configuration"><see cref="TelemetryConfiguration"/> as a settings source.</param>
        /// <param name="client"><see cref="TelemetryClient"/> to post traces to.</param>
        /// <param name="applicationIdProvider">Provider for resolving application Id to be used in multiple instruemntation keys scenarios.</param>
        /// <param name="injectResponseHeaders">Flag that indicates that response headers should be injected.</param>
        /// <param name="trackExceptions">Flag that indicates that exceptions should be tracked.</param>
        /// <param name="enableW3CHeaders">Flag that indicates that W3C header parsing should be enabled.</param>
        /// <param name="aspNetCoreMajorVersion">Major version of AspNetCore.</param>
        public HostingDiagnosticListener(
            TelemetryConfiguration configuration,
            TelemetryClient client,
            IApplicationIdProvider applicationIdProvider,
            bool injectResponseHeaders,
            bool trackExceptions,
            bool enableW3CHeaders,
            AspNetCoreMajorVersion aspNetCoreMajorVersion)
            : this(client, applicationIdProvider, injectResponseHeaders, trackExceptions, enableW3CHeaders, aspNetCoreMajorVersion)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.proactiveSamplingEnabled = this.configuration.EvaluateExperimentalFeature(ProactiveSamplingFeatureFlagName);
            this.conditionalAppIdEnabled = this.configuration.EvaluateExperimentalFeature(ConditionalAppIdFeatureFlagName);
        }

        /// <inheritdoc/>
        public string ListenerName { get; } = "Microsoft.AspNetCore";

        /// <summary>
        /// Diagnostic event handler method for 'Microsoft.AspNetCore.Mvc.BeforeAction' event.
        /// </summary>
        /// <param name="httpContext">HttpContext is used to retrieve information about the Request and Response.</param>
        /// <param name="routeValues">Used to get the name of the request.</param>
        public static void OnBeforeAction(HttpContext httpContext, IDictionary<string, object> routeValues)
        {
            var telemetry = httpContext.Features.Get<RequestTelemetry>();

            if (telemetry != null && string.IsNullOrEmpty(telemetry.Name))
            {
                string name = GetNameFromRouteContext(routeValues);
                if (!string.IsNullOrEmpty(name))
                {
                    name = httpContext.Request.Method + " " + name;
                    telemetry.Name = name;
                }
            }
        }

        /// <inheritdoc />
        public void OnSubscribe()
        {
            SubscriptionManager.Attach(this);
        /// </summary>
        /// <param name="httpContext">HttpContext is used to retrieve information about the Request and Response.</param>
        public void OnHttpRequestInStart(HttpContext httpContext)
        {
            if (this.client.IsEnabled())
            {
                // It's possible to host multiple apps (ASP.NET Core or generic hosts) in the same process
                // Each of this apps has it's own HostingDiagnosticListener and corresponding Http listener.
                // We should ignore events for all of them except one
                if (!SubscriptionManager.IsActive(this))
                    AspNetCoreEventSource.Instance.LogHostingDiagnosticListenerOnHttpRequestInStartActivityNull();
                    return;
                }

                var currentActivity = Activity.Current;
                Activity newActivity = null;
                string originalParentId = currentActivity.ParentId;
                string legacyRootId = null;
                bool traceParentPresent = false;
                var headers = httpContext.Request.Headers;
                // 1. No incoming headers. originalParentId will be null. Simply use the Activity as such.
                // 2. Incoming Request-ID Headers. originalParentId will be request-id, but Activity ignores this for ID calculations.
                //    If incoming ID is W3C compatible, ignore current Activity. Create new one with parent set to incoming W3C compatible rootid.
                //    If incoming ID is not W3C compatible, we can use Activity as such, but need to store originalParentID in custom property 'legacyRootId'
                // 3. Incoming TraceParent header.
                //    3a - 2.XX Need to ignore current Activity, and create new from incoming W3C TraceParent header.
                //    3b - 3.XX Use Activity as such because 3.XX is W3C Aware.

                // Another 3 possibilities when TelemetryConfiguration.EnableW3CCorrelation = false
                // 1. No incoming headers. originalParentId will be null. Simply use the Activity as such.
                // 2. Incoming Request-ID Headers. originalParentId will be request-id, Activity uses this for ID calculations.
                // 3. Incoming TraceParent header. Will simply Ignore W3C headers, and Current Activity used as such.

                // Attempt to find parent from incoming W3C Headers which 2.XX Hosting is unaware of.
                if (this.aspNetCoreMajorVersion != AspNetCoreMajorVersion.Three
                     && currentActivity.IdFormat == ActivityIdFormat.W3C
                     && headers.TryGetValue(W3CConstants.TraceParentHeader, out StringValues traceParentValues)
                     && traceParentValues != StringValues.Empty)
                {
                    var parentTraceParent = StringUtilities.EnforceMaxLength(
                        traceParentValues.First(),
                        InjectionGuardConstants.TraceParentHeaderMaxLength);
                    originalParentId = parentTraceParent;
                    traceParentPresent = true;
                    AspNetCoreEventSource.Instance.HostingListenerInformational(this.aspNetCoreMajorVersion, "Retrieved trace parent from headers.");
                }

                // Scenario #1. No incoming correlation headers.
                if (originalParentId == null)
                {
                    // Nothing to do here.
                    AspNetCoreEventSource.Instance.HostingListenerInformational(this.aspNetCoreMajorVersion, "OriginalParentId is null.");
                }
                else if (traceParentPresent)
                {
                    // Scenario #3. W3C-TraceParent
                    // We need to ignore the Activity created by Hosting, as it did not take W3CTraceParent into consideration.
#pragma warning disable CA2000 // Dispose objects before losing scope
                    // Even though we lose activity scope here, its retrieved using Activity.Current in end call back, and disposed/ended there
                    newActivity = new Activity(ActivityCreatedByHostingDiagnosticListener);
                    CopyActivityPropertiesFromAspNetCore(currentActivity, newActivity);

                    newActivity.SetParentId(originalParentId);
                    AspNetCoreEventSource.Instance.HostingListenerInformational(this.aspNetCoreMajorVersion, "Ignoring original Activity from Hosting to create new one using traceparent header retrieved by sdk.");

                    // read and populate tracestate
                    ReadTraceState(httpContext.Request.Headers, newActivity);
                }
                else if (this.aspNetCoreMajorVersion == AspNetCoreMajorVersion.Three && headers.ContainsKey(W3CConstants.TraceParentHeader))
                {
#pragma warning restore CA2000 // Dispose objects before losing scope
                            CopyActivityPropertiesFromAspNetCore(currentActivity, newActivity);
                            newActivity.SetParentId(ActivityTraceId.CreateFromString(traceId), default(ActivitySpanId), ActivityTraceFlags.None);
                            AspNetCoreEventSource.Instance.HostingListenerInformational(this.aspNetCoreMajorVersion, "Ignoring original Activity from Hosting to create new one using w3c compatible request-id.");
                        }
                        else
                        {
                            // store rootIdFromOriginalParentId in custom Property
                            legacyRootId = ExtractOperationIdFromRequestId(originalParentId);
                            AspNetCoreEventSource.Instance.HostingListenerInformational(this.aspNetCoreMajorVersion, "Incoming Request-ID is not W3C Compatible, and hence will be ignored for ID generation, but stored in custom property legacy_rootID.");
                        }
                    }
                }

                if (newActivity != null)
                {
                    newActivity.Start();
                    currentActivity = newActivity;
                }

                // Read Correlation-Context is all scenarios irrespective of presence of either request-id or traceparent headers.
                ReadCorrelationContext(httpContext.Request.Headers, currentActivity);

                var requestTelemetry = this.InitializeRequestTelemetry(httpContext, currentActivity, Stopwatch.GetTimestamp(), legacyRootId);

                requestTelemetry.Context.Operation.ParentId = GetParentId(currentActivity, originalParentId);

                this.AddAppIdToResponseIfRequired(httpContext, requestTelemetry);
            }
        }
            this.EndRequest(httpContext, Stopwatch.GetTimestamp());
        }

        /// <summary>
        /// Diagnostic event handler method for 'Microsoft.AspNetCore.Diagnostics.HandledException' event.
        /// </summary>
        /// <param name="httpContext">HttpContext is used to retrieve information about the Request and Response.</param>
        /// <param name="exception">Used to create exception telemetry.</param>
        public void OnDiagnosticsHandledException(HttpContext httpContext, Exception exception)
        {
                }
                else if (value.Key == "Microsoft.AspNetCore.Hosting.HttpRequestIn.Stop")
                {
                    httpContext = this.httpContextFetcherStop.Fetch(value.Value) as HttpContext;
                    if (httpContext != null)
                    {
                        this.OnHttpRequestInStop(httpContext);
                    }
                }
                else if (value.Key == "Microsoft.AspNetCore.Mvc.BeforeAction")
                        }

                        routeData = this.routeDataFetcher30.Fetch(value.Value);
                        if (routeData == null)
                        {
                            routeData = this.routeDataFetcher.Fetch(value.Value);
                        }
                    }
                    else
                    {
                        context = this.httpContextFetcherOnBeforeAction.Fetch(value.Value) as HttpContext;
                        if (context == null)
                        {
                            context = this.httpContextFetcherOnBeforeAction30.Fetch(value.Value) as HttpContext;
                        }

                        routeData = this.routeDataFetcher.Fetch(value.Value);
                        if (routeData == null)
                        {
                            routeData = this.routeDataFetcher30.Fetch(value.Value);
                    if (context != null && routeValues != null)
                    {
                        OnBeforeAction(context, routeValues);
                    }
                }
                else if (this.trackExceptions && value.Key == "Microsoft.AspNetCore.Diagnostics.UnhandledException")
                {
                    httpContext = this.httpContextFetcherDiagExceptionUnhandled.Fetch(value.Value) as HttpContext;
                    exception = this.exceptionFetcherDiagExceptionUnhandled.Fetch(value.Value) as Exception;
                    if (httpContext != null && exception != null)
            }
        }

        private static string ExtractOperationIdFromRequestId(string originalParentId)
        {
            if (originalParentId[0] == '|')
            {
                int indexDot = originalParentId.IndexOf('.');
                if (indexDot > 1)
                {
                    return originalParentId;
                }
            }
            else
            {
                return originalParentId;
            }
        }

        private static bool TryGetW3CCompatibleTraceId(string requestId, out ReadOnlySpan<char> result)
                }
                else
                {
                    result = null;
                    return false;
                }
            }
            else
            {
                result = null;
                                var itemValue = StringUtilities.EnforceMaxLength(parts[1], InjectionGuardConstants.ContextHeaderValueMaxLength);
                                activity.AddBaggage(itemName.Trim(), itemValue.Trim());
                            }
                        }

                        AspNetCoreEventSource.Instance.HostingListenerVerbose("Correlation-Context retrived from header and stored into activity baggage.");
                    }
                }
            }
            catch (Exception ex)
        private static string GetNameFromRouteContext(IDictionary<string, object> routeValues)
        {
            string name = null;

            if (routeValues.Count > 0)
            {
                object controller;
                routeValues.TryGetValue("controller", out controller);
                string controllerString = (controller == null) ? string.Empty : controller.ToString();

                    if (routeValues.Keys.Count > 2)
                    {
                        // Add parameters
                        var sortedKeys = routeValues.Keys
                            .Where(key =>
                                !string.Equals(key, "controller", StringComparison.OrdinalIgnoreCase) &&
                                !string.Equals(key, "action", StringComparison.OrdinalIgnoreCase) &&
                                !string.Equals(key, "!__route_group", StringComparison.OrdinalIgnoreCase))
                            .OrderBy(key => key, StringComparer.OrdinalIgnoreCase)
                            .ToArray();
        }

        private RequestTelemetry InitializeRequestTelemetry(HttpContext httpContext, Activity activity, long timestamp, string legacyRootId = null)
        {
            var requestTelemetry = new RequestTelemetry();

            if (activity.IdFormat == ActivityIdFormat.W3C)
            {
                var traceId = activity.TraceId.ToHexString();
                requestTelemetry.Id = activity.SpanId.ToHexString();
                requestTelemetry.Context.Operation.Id = traceId;
                AspNetCoreEventSource.Instance.RequestTelemetryCreated("W3C", requestTelemetry.Id, traceId);
            }
            else
            {
                requestTelemetry.Context.Operation.Id = activity.RootId;
                requestTelemetry.Id = activity.Id;
                AspNetCoreEventSource.Instance.RequestTelemetryCreated("Hierarchical", requestTelemetry.Id, requestTelemetry.Context.Operation.Id);
                foreach (var prop in activity.Baggage)
                {
                    if (!requestTelemetry.Properties.ContainsKey(prop.Key))
                    {
                        requestTelemetry.Properties[prop.Key] = prop.Value;
                    }
                }

                if (!string.IsNullOrEmpty(legacyRootId))
                {
            if (!string.IsNullOrEmpty(headerCorrelationId))
            {
                headerCorrelationId = StringUtilities.EnforceMaxLength(headerCorrelationId, InjectionGuardConstants.AppIdMaxLength);
                if (string.IsNullOrEmpty(instrumentationKey))
                {
                    return headerCorrelationId;
                }

                string applicationId = null;
                if ((this.applicationIdProvider?.TryGetApplicationId(instrumentationKey, out applicationId) ?? false)
        {
            if (this.injectResponseHeaders)
            {
                IHeaderDictionary responseHeaders = httpContext.Response?.Headers;
                if (responseHeaders != null &&
                    !string.IsNullOrEmpty(requestTelemetry.Context.InstrumentationKey) &&
                    (!responseHeaders.ContainsKey(RequestResponseHeaders.RequestContextHeader) ||
                     HttpHeadersUtilities.ContainsRequestContextKeyValue(
                         responseHeaders,
                         RequestResponseHeaders.RequestContextTargetKey)))
                {
                    if (this.lastIKeyLookedUp != requestTelemetry.Context.InstrumentationKey)
                    {
                        var appIdResolved = this.applicationIdProvider?.TryGetApplicationId(requestTelemetry.Context.InstrumentationKey, out this.lastAppIdUsed);
                        if (appIdResolved.HasValue && appIdResolved.Value)
                        {
                            this.lastIKeyLookedUp = requestTelemetry.Context.InstrumentationKey;
                        }
                    }


                var telemetry = httpContext?.Features.Get<RequestTelemetry>();

                if (telemetry == null)
                {
                    // Log we are not tracking this request as it cannot be found in context.
                    return;
                }

                var activity = Activity.Current;
                    telemetry.Context.GetInternalContext().SdkVersion = this.sdkVersion;
                }

                this.client.TrackRequest(telemetry);

                // Stop what we started.
                if (activity != null && activity.OperationName == ActivityCreatedByHostingDiagnosticListener)
                {
                    activity.Stop();
                }
                        "Exception", Activity.Current?.Id);
                    return;
                }

                var telemetry = httpContext?.Features.Get<RequestTelemetry>();
                if (telemetry != null)
                {
                    telemetry.Success = false;
                }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
