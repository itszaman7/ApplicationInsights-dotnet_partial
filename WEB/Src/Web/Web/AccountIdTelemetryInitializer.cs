namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Web;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Implementation;

    /// <summary>
    /// A telemetry initializer that will set the User properties of Context corresponding to a RequestTelemetry object.
    /// User.AccountId is updated with properties derived from the RequestTelemetry.RequestTelemetry.Context.User.
    /// </summary>
    public class AccountIdTelemetryInitializer : WebTelemetryInitializerBase
    {
                return;
            }

            if (string.IsNullOrEmpty(authUserCookie.Value))
            {
            var cookieParts = authUserCookieString.Split('|');

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
            {
                throw new ArgumentNullException(nameof(platformContext));
            }

            if (string.IsNullOrEmpty(telemetry.Context.User.AccountId))
                telemetry.Context.User.AccountId = requestTelemetry.Context.User.AccountId;
            }
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
