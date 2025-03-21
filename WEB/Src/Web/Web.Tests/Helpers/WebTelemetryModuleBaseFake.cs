namespace Microsoft.ApplicationInsights.Web.Helpers
{
    using System.Web;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Implementation;

    internal class WebTelemetryModuleBaseFake : WebTelemetryModuleBase
    {
        public bool OnBeginRequestCalled { get; set; }

        public bool OnEndRequestCalled { get; set; }

        public bool OnErrorCalled { get; set; }
        }

        public override void OnEndRequest(
            RequestTelemetry requestTelemetry,
            HttpContext platformContext)
        {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
