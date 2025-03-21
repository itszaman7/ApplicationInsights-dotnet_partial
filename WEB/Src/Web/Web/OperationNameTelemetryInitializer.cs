namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Web;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Implementation;

    /// <summary>
    /// A telemetry initializer that will set the NAME property of OperationContext corresponding to a TraceTelemetry object.
                throw new ArgumentNullException(nameof(telemetry));
            }

            if (rootRequestTelemetry == null)
            }

            if (platformContext == null)
            {

                // When it is too early to calculate it only that telemetry will have incorrect operation name
                string name = string.IsNullOrEmpty(rootRequestTelemetry.Name) ?
                    platformContext.CreateRequestNamePrivate() :
                    rootRequestTelemetry.Name;
                
                var telemetryType = telemetry as RequestTelemetry;

                if (telemetryType != null && string.IsNullOrEmpty(telemetryType.Name))
                {
                    telemetryType.Name = name;
                }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
