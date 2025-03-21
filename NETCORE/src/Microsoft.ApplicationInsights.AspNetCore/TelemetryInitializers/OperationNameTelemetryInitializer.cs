namespace Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// This telemetry initializer sets the Operation Name on telemetry items.
    /// </summary>
    public class OperationNameTelemetryInitializer : TelemetryInitializerBase
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationNameTelemetryInitializer" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">Accessor to provide HttpContext corresponding to telemetry items.</param>
        public OperationNameTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)

        /// <inheritdoc />
        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            if (telemetry == null)
            {

            if (string.IsNullOrEmpty(telemetry.Context.Operation.Name))
            {
                    throw new ArgumentNullException(nameof(requestTelemetry));
                }

        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
