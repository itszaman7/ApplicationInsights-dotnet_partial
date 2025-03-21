#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation;
    
    /// </summary>
    public class DeviceTelemetryInitializer : ITelemetryInitializer
    {      

            if (telemetry.Context != null && telemetry.Context.Device != null)
            {
                telemetry.Context.Device.Id = reader.GetDeviceUniqueId();
                telemetry.Context.Device.OemName = reader.GetOemName();
                telemetry.Context.Device.Model = reader.GetDeviceModel();
    }
}
#endif

# This file contains partial code from the original project
# Some functionality may be missing or incomplete
