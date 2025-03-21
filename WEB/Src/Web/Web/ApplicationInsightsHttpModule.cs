namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Web;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Web.Implementation;

    /// <summary>
    /// Platform agnostic module for web application instrumentation.
    /// </summary>
    public sealed class ApplicationInsightsHttpModule : IHttpModule
    {
        private static readonly MethodInfo OnStepMethodInfo;
        private static readonly bool AddOnSendingHeadersMethodExists;

        // Delegate preferred over Invoke to gain performance, only in NET452 or above as ISubscriptionToken is not available in Net40
        private static readonly Func<HttpResponse, Action<HttpContext>, ISubscriptionToken> OpenDelegateForInvokingAddOnSendingHeadersMethod;

        private readonly RequestTrackingTelemetryModule requestModule;
        private readonly Action<HttpContext> addOnSendingHeadersMethodParam;
        private readonly object[] onExecuteActionParam = { (Action<HttpContextBase, Action>)OnExecuteRequestStep };

        private object[] addOnSendingHeadersMethodParams;

        /// <summary>
        /// Indicates if module initialized successfully.
        /// </summary>
        private bool isEnabled = true;

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Enforcing static fields initialization.")]
        static ApplicationInsightsHttpModule()
        {
            // We use reflection here because 'AddOnSendingHeaders' is only available post .net framework 4.5.2. Hence we call it if we can find it.
            // Not using reflection would result in MissingMethodException when 4.5 or 4.5.1 is present. 
            var addOnSendingHeadersMethod = typeof(HttpResponse).GetMethod("AddOnSendingHeaders");

                AddOnSendingHeadersMethodExists = true;
                OpenDelegateForInvokingAddOnSendingHeadersMethod = (Func<HttpResponse, Action<HttpContext>, ISubscriptionToken>)Delegate.CreateDelegate(
                    typeof(Func<HttpResponse, Action<HttpContext>, ISubscriptionToken>),
                    null,
                    addOnSendingHeadersMethod,
                    true);
            }

            // OnExecuteRequestStep is available starting with 4.7.1
            // If this is executed in 4.7.1 runtime (regardless of targeted .NET version),
        {
            try
            {
                // The call initializes TelemetryConfiguration that will create and Initialize modules
                TelemetryConfiguration configuration = TelemetryConfiguration.Active;
                foreach (var module in TelemetryModules.Instance.Modules)
                {
                    if (module is RequestTrackingTelemetryModule telemetryModule)
                    {
                        this.requestModule = telemetryModule;
                    context.BeginRequest += this.OnBeginRequest;

                    // OnExecuteRequestStep is available starting with 4.7.1
                    // If this is executed in 4.7.1 runtime (regardless of targeted .NET version),
                    // we will use it to restore lost activity, otherwise keep PreRequestHandlerExecute
                    if (OnStepMethodInfo != null && HttpRuntime.UsingIntegratedPipeline)
                    {
                        try
                        {
                            OnStepMethodInfo.Invoke(context, this.onExecuteActionParam);
                }
                catch (Exception exc)
                {
                    this.isEnabled = false;
                    WebEventSource.Log.WebModuleInitializationExceptionEvent(exc.ToInvariantString());
                }
            }
        }

        /// <summary>
        {
        }

        /// <summary>
        /// Restores Activity before each pipeline step if it was lost.
        /// </summary>
        /// <param name="context">HttpContext instance.</param>
        /// <param name="step">Step to be executed.</param>
        private static void OnExecuteRequestStep(HttpContextBase context, Action step)
        {
            if (context == null)
            {
                WebEventSource.Log.NoHttpContextWarning();
                return;
            }

            TraceCallback(nameof(OnExecuteRequestStep), context.ApplicationInstance);
            if (context.CurrentNotification == RequestNotification.ExecuteRequestHandler && !context.IsPostNotification)
            {
                ActivityHelpers.RestoreActivityIfNeeded(context.Items);
                try
                {
                }
            }
        }

        private void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            var httpApplication = (HttpApplication)sender;

            if (httpApplication == null)
            {
                WebEventSource.Log.NoHttpApplicationWarning();
                return;
            }

            TraceCallback(nameof(this.Application_PreRequestHandlerExecute), httpApplication);
            ActivityHelpers.RestoreActivityIfNeeded(httpApplication.Context?.Items);
        }

        private void OnBeginRequest(object sender, EventArgs eventArgs)
        {
            try
            {
                if (httpApplication?.Response != null && AddOnSendingHeadersMethodExists)
                {                                     
                    // Faster delegate based invocation.
                    OpenDelegateForInvokingAddOnSendingHeadersMethod.Invoke(httpApplication.Response, this.addOnSendingHeadersMethodParam);
                }
            }
            catch (Exception ex)
            {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
