namespace Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers
{
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.AspNetCore.Hosting;
    /// </summary>
    public class AspNetCoreEnvironmentTelemetryInitializer : ITelemetryInitializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AspNetCoreEnvironmentTelemetryInitializer"/> class.
        /// </summary>
        public void Initialize(ITelemetry telemetry)
        {
            if (this.environment != null)
            {
                if (telemetry is ISupportProperties telProperties && !telProperties.Properties.ContainsKey(AspNetCoreEnvironmentPropertyName))
                {
                    telProperties.Properties.Add(
                        AspNetCoreEnvironmentPropertyName,
                        this.environment.EnvironmentName);
                }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
