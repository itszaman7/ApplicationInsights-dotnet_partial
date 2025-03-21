namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Web;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Implementation;
            {
                return this.filters;
            }

            set
            {
                if (value != null)
                {
                    this.filters = value;

                    // We expect customers to configure telemetry initializer before they add it to active configuration
                    // So we will not protect it with locks (to improve perf)
                    this.filterPatterns = value.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                }
            }
        }
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

            if (string.IsNullOrEmpty(telemetry.Context.Operation.SyntheticSource))
            {
                if (platformContext != null)
                {
                                {
                                    telemetry.Context.Operation.SyntheticSource = SyntheticSourceName;
                                    platformContext.Items.Add(SyntheticSourceNameKey, SyntheticSourceName);
                                    return;
                                }
                            }
                        }
                    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
