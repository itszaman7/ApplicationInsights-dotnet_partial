namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TransmissionPolicy
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Channel.Implementation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    /// <summary>
    /// This class defines how the ServerTelemetryChannel will behave when it receives Response Codes 
    /// from the Ingestion Service related to Authentication (AAD) scenarios.
    /// </summary>
    /// <remarks>
    /// This class is disabled by default and expected to be enabled only when AAD has been configured in <see cref="TelemetryConfiguration.CredentialEnvelope"/>.
    /// </remarks>
    internal class AuthenticationTransmissionPolicy : TransmissionPolicy, IDisposable
    {
        internal TaskTimerInternal PauseTimer = new TaskTimerInternal { Delay = TimeSpan.FromMinutes(1) };
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// This method subscribes to the <see cref="Transmitter.TransmissionSent"/> event.
        /// <remarks>
        /// AN EXPLANATION OF THE STATUS CODES:
        /// - <see cref="ResponseStatusCodes.Unauthorized"/>
        /// "HTTP/1.1 401 Unauthorized - please provide the valid authorization token".
        /// This indicates that the authorization token was either absent, invalid, or expired.
        /// The root cause is not known and we should throttle retries.
        /// - <see cref="ResponseStatusCodes.Forbidden"/>
        /// "HTTP/1.1 403 Forbidden - provided credentials do not grant the access to ingest the telemetry into the component".
        /// This indicates the configured identity does not have permissions to publish to this resource.
            this.LogCapacityChanged();
            this.Apply();

            this.backoffLogicManager.ReportBackoffEnabled(e.Response.StatusCode);
            this.Transmitter.Enqueue(e.Transmission);

            // Check this.pauseTimer above for the configured wait time.
            this.PauseTimer.Start(() =>
                {

        private void ResetPolicy()
        {
            this.MaxSenderCapacity = null;
            this.MaxBufferCapacity = null;
            this.MaxStorageCapacity = null;
            this.LogCapacityChanged();
            this.Apply();
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.PauseTimer != null)
                {
                    this.PauseTimer.Dispose();
                    this.PauseTimer = null;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
