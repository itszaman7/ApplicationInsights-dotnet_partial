namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Web;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Implementation;

    /// <summary>
                return;
            }

            var authUserCookieString = HttpUtility.UrlDecode(authUserCookie.Value);

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
                throw new ArgumentNullException(nameof(telemetry));
            }

            if (requestTelemetry == null)
            {
                throw new ArgumentNullException(nameof(platformContext));
            }

            if (string.IsNullOrEmpty(telemetry.Context.User.AuthenticatedUserId))
            {
                if (string.IsNullOrEmpty(requestTelemetry.Context.User.AuthenticatedUserId))
                {
                    GetAuthUserContextFromUserCookie(platformContext.Request.UnvalidatedGetCookie(RequestTrackingConstants.WebAuthenticatedUserCookieName), requestTelemetry);
                }

                telemetry.Context.User.AuthenticatedUserId = requestTelemetry.Context.User.AuthenticatedUserId;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
