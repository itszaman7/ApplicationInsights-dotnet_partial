//-----------------------------------------------------------------------
// <copyright file="EtwTelemetryModule.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.EtwCollector
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.EtwCollector.Implemenetation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Implementation;
    using Microsoft.ApplicationInsights.TraceEvent.Shared.Implementation;
    using Microsoft.ApplicationInsights.TraceEvent.Shared.Utilities;
    using Microsoft.Diagnostics.Tracing;
    using Microsoft.Diagnostics.Tracing.Session;

    /// <summary>
    /// A module to trace data submitted via .NET framework <seealso cref="Microsoft.Diagnostics.Tracing.Session" /> class.
    /// </summary>
    public class EtwTelemetryModule : ITelemetryModule, IDisposable
    {
        private readonly object lockObject;
        private TelemetryClient client;
        private bool isDisposed = false;
        private bool isInitialized = false;
        private List<Guid> enabledProviderIds;
                 "ApplicationInsights-{0}-{1}",
                 nameof(EtwTelemetryModule),
                 Guid.NewGuid()))))
        {
        }

        internal EtwTelemetryModule(Func<ITraceEventSession> traceEventSessionFactory)
        {
            this.lockObject = new object();
            this.Sources = new List<EtwListeningRequest>();
            this.enabledProviderIds = new List<Guid>();
            this.enabledProviderNames = new List<string>();

            if (traceEventSessionFactory == null)
            {
                throw new ArgumentNullException(nameof(traceEventSessionFactory));
            }

            this.traceEventSessionFactory = traceEventSessionFactory;
        }

        /// <summary>
        /// Gets the list of ETW Provider listening requests (information about which providers should be traced).
        /// </summary>
        public IList<EtwListeningRequest> Sources { get; private set; }

        /// <summary>
        /// Initializes the telemetry module and starts tracing ETW events specified via <see cref="Sources"/> property.
        /// </summary>
        /// <param name="configuration">Module configuration.</param>
            }

            if (this.isDisposed)
            {
                errorMessage = "Can't initialize a module that is disposed. The initialization is terminated.";
                EventSourceListenerEventSource.Log.ModuleInitializationFailed(
                    nameof(EtwTelemetryModule),
                    errorMessage);
                return;
            }

            lock (this.lockObject)
            {

                // sdkVersionIdentifier will be used in telemtry entry as a identifier for the sender.
                // The value will look like: etw:x.x.x-x
                const string SdkVersionIdentifier = "etw:";
                this.client.Context.GetInternalContext().SdkVersion = SdkVersionUtils.GetSdkVersion(SdkVersionIdentifier);

                if (this.isInitialized)
                {
                    this.DisableProviders();
                    this.enabledProviderIds.Clear();
                                    this.traceEventSession.Source.Dynamic.All -= this.OnEvent;
                                    this.isInitialized = false;
                                },
                                TaskCreationOptions.LongRunning);
                        }
                    }
                    finally
                    {
                        this.isInitialized = true;
                    }
        /// <summary>
        /// Disposes the module.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the module.
        /// </summary>
        /// <param name="isDisposing">Indicate if it is called by Dispose().</param>
        protected virtual void Dispose(bool isDisposing)
        {
            // Mark this object as disposed even when disposing run into exception, which is not expected.
            this.isDisposed = true;
            if (isDisposing)
            {
                if (this.traceEventSession != null)
        private void EnableProviders()
        {
            // Enable TPL provider to get hierarchical activity IDs
            var tplProviderRequest = new EtwListeningRequest()
            {
                ProviderGuid = TplActivities.TplEventSourceGuid,
                Level = TraceEventLevel.Always,
                Keywords = TplActivities.TaskFlowActivityIdsKeyword,
            };
            this.EnableProvider(tplProviderRequest);

            foreach (EtwListeningRequest request in this.Sources)
            {
                this.EnableProvider(request);
            }
        }

        private void EnableProvider(EtwListeningRequest request)
        {
            string errorMessage;
            {
                this.traceEventSession.DisableProvider(id);
            }

            foreach (string providerName in this.enabledProviderNames)
            {
                this.traceEventSession.DisableProvider(providerName);
            }
        }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
