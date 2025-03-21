namespace Microsoft.ApplicationInsights.DependencyCollector
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.EventHandlers;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.SqlClientDiagnostics;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
#if NETSTANDARD
    using System.Reflection;
    using System.Runtime.Versioning;
#else
    using Microsoft.Diagnostics.Instrumentation.Extensions.Intercept;
#endif

    /// <summary>
    /// Remote dependency monitoring.
    /// </summary>
    public class DependencyTrackingTelemetryModule : ITelemetryModule, IDisposable
    {
        private readonly object lockObject = new object();

#if NET452
        private HttpDesktopDiagnosticSourceListener httpDesktopDiagnosticSourceListener;
        private FrameworkHttpEventListener httpEventListener;
        private FrameworkSqlEventListener sqlEventListener;
#endif

        private HttpCoreDiagnosticSourceListener httpCoreDiagnosticSourceListener;
        private TelemetryDiagnosticSourceListener telemetryDiagnosticSourceListener;
        private SqlClientDiagnosticSourceListener sqlClientDiagnosticSourceListener;
        private AzureSdkDiagnosticListenerSubscriber azureSdkDiagnosticListener;

#if !NETSTANDARD
        private ProfilerSqlCommandProcessing sqlCommandProcessing;
        private ProfilerSqlConnectionProcessing sqlConnectionProcessing;
        private ProfilerHttpProcessing httpProcessing;
#endif
        private TelemetryConfiguration telemetryConfiguration;

        private bool disposed = false;

        /// <summary>
        /// Gets or sets a value indicating whether to disable runtime instrumentation.
        /// </summary>
        public bool DisableRuntimeInstrumentation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable Http Desktop DiagnosticSource instrumentation.
        /// </summary>
        public bool DisableDiagnosticSourceInstrumentation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable legacy (x-ms*) correlation headers injection.
        /// </summary>
        public bool EnableLegacyCorrelationHeadersInjection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable W3C distributed tracing headers injection.
        /// </summary>
        [Obsolete("This field has been deprecated. Please set Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical; Activity.ForceDefaultIdFormat = true;")]
        public bool EnableW3CHeadersInjection { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether to enable Request-Id correlation headers injection.
        /// </summary>
        public bool EnableRequestIdHeaderInjectionInW3CMode { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to track the SQL command text in SQL dependencies.
        public bool EnableAzureSdkTelemetryListener { get; set; } = true;

        /// <summary>
        /// Gets or sets the endpoint that is to be used to get the application insights resource's profile (appId etc.).
        /// </summary>
        [Obsolete("This field has been deprecated. Please set TelemetryConfiguration.Active.ApplicationIdProvider = new ApplicationInsightsApplicationIdProvider() and customize ApplicationInsightsApplicationIdProvider.ProfileQueryEndpoint.")]
        public string ProfileQueryEndpoint { get; set; }

        /// <summary>Gets a value indicating whether this module has been initialized.</summary>
        internal bool IsInitialized { get; private set; } = false;
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Initialize method is called after all configuration properties have been loaded from the configuration.
        /// </summary>
        public void Initialize(TelemetryConfiguration configuration)
                            // Net40 only supports runtime instrumentation
                            // net452 supports either but not both to avoid duplication
                            this.InitializeForRuntimeInstrumentationOrFramework();
#endif

                            // net452 referencing .net core System.Net.Http supports diagnostic listener
                            this.httpCoreDiagnosticSourceListener = new HttpCoreDiagnosticSourceListener(
                                configuration,
                                this.SetComponentCorrelationHttpHeaders,
                                this.ExcludeComponentCorrelationHttpHeadersOnDomains,
                }
            }
        }

#if !NETSTANDARD
        internal virtual void InitializeForRuntimeProfiler()
        {
            // initialize instrumentation extension
            var extensionBaseDirectory = string.IsNullOrWhiteSpace(AppDomain.CurrentDomain.RelativeSearchPath)
                ? AppDomain.CurrentDomain.BaseDirectory
            // obtain agent version
            var agentVersion = Decorator.GetAgentVersion();
            DependencyCollectorEventSource.Log.RemoteDependencyModuleInformation("AgentVersion is " + agentVersion);

            this.httpProcessing = new ProfilerHttpProcessing(
                this.telemetryConfiguration,
                agentVersion,
                DependencyTableStore.Instance.WebRequestConditionalHolder,
                this.SetComponentCorrelationHttpHeaders,
                this.ExcludeComponentCorrelationHttpHeadersOnDomains,
        {
            if (!this.disposed)
            {
                if (disposing)
                {
#if NET452
                    // Net40 does not support framework event source and diagnostic source
                    if (this.httpDesktopDiagnosticSourceListener != null)
                    {
                        this.httpDesktopDiagnosticSourceListener.Dispose();
#endif
                    }

                    if (this.telemetryDiagnosticSourceListener != null)
                    {
                        this.telemetryDiagnosticSourceListener.Dispose();
                    }

                    if (this.sqlClientDiagnosticSourceListener != null)
                    {
                        this.sqlClientDiagnosticSourceListener.Dispose();
                    if (this.azureSdkDiagnosticListener != null)
                    {
                        this.azureSdkDiagnosticListener.Dispose();
                    }
                }

                this.disposed = true;
            }
        }

        /// <summary>
        /// When the first Activity is created in the process (on .NET Framework), it synchronizes DateTime.UtcNow 
        /// in order to make it's StartTime and duration precise, it may take up to 16ms. 
        /// Let's create the first Activity ever here, so we will not miss those 16ms on the first dependency tracking.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        private static void PrepareFirstActivity()
        {
#if REDFIELD
            // Redfield uses an older version of Diagnostic Source in which Activity is not Disposable.
            activity.Start();
            activity.Stop();
        }

#if !NETSTANDARD
        /// <summary>
        /// Initialize for framework event source (not supported for Net40).
        /// </summary>
        private void InitializeForDiagnosticAndFrameworkEventSource()
        {
        /// Initialize for runtime instrumentation or framework event source.
        /// </summary>
        private void InitializeForRuntimeInstrumentationOrFramework()
        {
            if (this.IsProfilerAvailable())
            {
                DependencyCollectorEventSource.Log.RemoteDependencyModuleInformation("Profiler is attached.");
                if (!this.DisableRuntimeInstrumentation)
                {
                    try
                        this.InitializeForDiagnosticAndFrameworkEventSource();
                        DependencyCollectorEventSource.Log.ProfilerFailedToAttachError(exp.ToInvariantString());
                    }
                }
                else
                {
                    // if config is set to disable runtime instrumentation then default to framework event source
                    this.InitializeForDiagnosticAndFrameworkEventSource();
                    DependencyCollectorEventSource.Log.RemoteDependencyModuleVerbose("Runtime instrumentation is set to disabled. Initialize with framework event source instead.");
                }
            }
            else
            {
                // if profiler is not attached then default to diagnostics and framework event source
                this.InitializeForDiagnosticAndFrameworkEventSource();

                // Log a message to indicate the profiler is not attached
                DependencyCollectorEventSource.Log.RemoteDependencyModuleProfilerNotAttached();
            }
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
