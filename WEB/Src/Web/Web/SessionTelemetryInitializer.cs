namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Web;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Implementation;

            if (string.IsNullOrWhiteSpace(sessionCookie.Value))
            {
                WebEventSource.Log.WebSessionTrackingSessionCookieIsEmptyWarning();
                return;
            }

            var parts = sessionCookie.Value.Split('|');
            }

            requestTelemetry.Context.Session.Id = parts[SessionCookieSessionIdIndex];
        }

        /// <summary>
        /// Implements initialization logic.
        /// </summary>
        /// <param name="platformContext">Http context.</param>
        /// <param name="requestTelemetry">Request telemetry object associated with the current request.</param>
        /// <param name="telemetry">Telemetry item to initialize.</param>
        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            if (requestTelemetry == null)
            {
                throw new ArgumentNullException(nameof(requestTelemetry));
                if (string.IsNullOrEmpty(requestTelemetry.Context.Session.Id))

                telemetry.Context.Session.Id = requestTelemetry.Context.Session.Id;

                if (requestTelemetry.Context.Session.IsFirst.HasValue)
                {
                    telemetry.Context.Session.IsFirst = requestTelemetry.Context.Session.IsFirst;
                }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
