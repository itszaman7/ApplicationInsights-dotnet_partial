//-----------------------------------------------------------------------
// <copyright file="DiagnosticSourceTelemetryModule.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.DiagnosticSourceListener
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticSourceTelemetryModule"/> class.
        /// </summary>
        public DiagnosticSourceTelemetryModule() : this(Track)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticSourceTelemetryModule"/> class.
        /// </summary>
        /// <param name="onEventWrittenHandler">Action to be executed each time an event is written to format and send via the configured <see cref="TelemetryClient"/>.</param>
        public DiagnosticSourceTelemetryModule(OnEventWrittenHandler onEventWrittenHandler)
        {
            if (onEventWrittenHandler == null)
            {
                throw new ArgumentNullException(nameof(onEventWrittenHandler));
            }

            this.onEventWrittenHandler = onEventWrittenHandler;
            var telemetryClient = new TelemetryClient(configuration);
            telemetryClient.Context.GetInternalContext().SdkVersion = SdkVersionUtils.GetSdkVersion("dsl:");

            this.client = telemetryClient;

            // Protect against multiple subscriptions if Initialize is called twice
            if (this.allDiagnosticListenersSubscription == null)
            {
                var subscription = DiagnosticListener.AllListeners.Subscribe(this);
                if (Interlocked.CompareExchange(ref this.allDiagnosticListenersSubscription, subscription, null) != null)
            }
        }

        /// <summary>
        /// Dispose of this instance.
        /// </summary>
        public void Dispose()
        {
            if (this.diagnosticListenerSubscriptions != null)
            {
                foreach (var subscription in this.diagnosticListenerSubscriptions)
                {
                    subscription.Dispose();
                }
            }

            this.allDiagnosticListenersSubscription?.Dispose();
        }

        void IObserver<DiagnosticListener>.OnCompleted()
        {
        }

        void IObserver<DiagnosticListener>.OnError(Exception error)
        {
        }

        void IObserver<DiagnosticListener>.OnNext(DiagnosticListener listener)
        {
            foreach (var source in this.Sources)
            // Transfer properties from payload to telemetry
            if (payload != null)
            {
                foreach (var property in DeclaredPropertiesCache.GetDeclaredProperties(payload))
                {
                    if (!property.IsSpecialName)
                    {
                        telemetry.Properties.Add(property.Name, Convert.ToString(property.GetValue(payload), CultureInfo.InvariantCulture));
                    }
                }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
