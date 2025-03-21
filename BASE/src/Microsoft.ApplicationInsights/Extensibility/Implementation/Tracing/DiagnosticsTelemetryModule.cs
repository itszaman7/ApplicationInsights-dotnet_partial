namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Linq;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule;

    /// <summary>
    /// Use diagnostics telemetry module to report SDK internal problems to the portal and VS debug output window.
    /// </summary>
    public sealed class DiagnosticsTelemetryModule : ITelemetryModule, IHeartbeatPropertyManager, IDisposable
    {
        internal readonly IList<IDiagnosticsSender> Senders = new List<IDiagnosticsSender>();
        internal readonly DiagnosticsListener EventListener;
        internal readonly IHeartbeatProvider HeartbeatProvider = null;
        
        private readonly object lockObject = new object();
        private readonly IDiagnoisticsEventThrottlingScheduler throttlingScheduler = new DiagnoisticsEventThrottlingScheduler();
        private volatile bool disposed = false;
        private string instrumentationKey;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsTelemetryModule"/> class. 
        /// </summary>
        public DiagnosticsTelemetryModule()
        {
            // Adding a dummy queue sender to keep the data to be sent to the portal before the initialize method is called
            this.Senders.Add(new PortalDiagnosticsQueueSender());

            this.EventListener = new DiagnosticsListener(this.Senders);

            this.HeartbeatProvider = new HeartbeatProvider();
        }
            set => this.HeartbeatProvider.HeartbeatInterval = value;
        }

        /// <summary>
        /// Gets a list of default heartbeat property providers that are disabled and will not contribute to the
        /// default heartbeat properties. The only default heartbeat property provide currently defined is named
        /// 'Base'.
        /// </summary>
        public IList<string> ExcludedHeartbeatPropertyProviders => this.HeartbeatProvider.ExcludedHeartbeatPropertyProviders;

        /// <summary>
        /// Gets or sets diagnostics Telemetry Module LogLevel configuration setting. 
        /// Possible values LogAlways, Critical, Error, Warning, Informational and Verbose.
        /// </summary>
        public string Severity
        {
            get => this.EventListener.LogLevel.ToString();
            set => this.EventListener.SetLogLevel(value);
        }

        /// Gets or sets instrumentation key for diagnostics. Use to redirect SDK 
        /// internal problems reporting to the separate instrumentation key.
        /// </summary>
        public string DiagnosticsInstrumentationKey
        {
            get
            {
                return this.instrumentationKey;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.instrumentationKey = value;

                    // Set instrumentation key in Portal sender
                    foreach (var portalSender in this.Senders.OfType<PortalDiagnosticsSender>())
                    {
                        portalSender.DiagnosticsInstrumentationKey = this.instrumentationKey;
                    }

                    this.HeartbeatProvider.InstrumentationKey = this.instrumentationKey;
                }
            }
        }

        /// <summary>Gets a value indicating whether this module has been initialized.</summary>
        internal bool IsInitialized { get; private set; } = false;

        /// <summary>
        /// Initializes this telemetry module.
        /// </summary>
        /// <param name="configuration">Telemetry configuration to use for this telemetry module.</param>
        public void Initialize(TelemetryConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
                        };

        /// <returns>True if the payload provider was added, false if it hasn't been added yet 
        /// (<see cref="DiagnosticsTelemetryModule.AddHeartbeatProperty"/>).</returns>
        public bool SetHeartbeatProperty(string propertyName, string propertyValue = null, bool? isHealthy = null)
        {
            if (!string.IsNullOrEmpty(propertyName) && (propertyValue != null || isHealthy != null))
            {
                return this.HeartbeatProvider.SetHeartbeatProperty(propertyName, false, propertyValue, isHealthy);
            }
            else
            {

            return false;
        }

        /// <summary>
        /// Disposes this object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of resources.
        /// </summary>
        /// <param name="managed">Indicates if managed code is being disposed.</param>
        private void Dispose(bool managed)
        {
            if (managed && !this.disposed)
            {
                this.EventListener.Dispose();
                (this.throttlingScheduler as IDisposable).Dispose();
                foreach (var disposableSender in this.Senders.OfType<IDisposable>())
                {
                    disposableSender.Dispose();
                }

                this.HeartbeatProvider.Dispose();
            }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
