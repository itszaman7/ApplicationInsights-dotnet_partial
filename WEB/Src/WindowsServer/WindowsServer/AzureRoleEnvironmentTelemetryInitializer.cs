#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;    
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation;    

    /// <summary>
    /// A telemetry initializer that will gather Azure Role Environment context information.
    /// </summary>
    public class AzureRoleEnvironmentTelemetryInitializer : ITelemetryInitializer
    {
        private const string WebSiteEnvironmentVariable = "WEBSITE_SITE_NAME";
        private bool? isAzureWebApp = null;
        private string roleInstanceName;
        /// </summary>
        public AzureRoleEnvironmentTelemetryInitializer()
        {
            WindowsServerEventSource.Log.TelemetryInitializerLoaded(this.GetType().FullName);

            if (this.IsAppRunningInAzureWebApp())
            {
            {
                try
                {
                    this.roleName = AzureRoleEnvironmentContextReader.Instance.GetRoleName();
                    this.roleInstanceName = AzureRoleEnvironmentContextReader.Instance.GetRoleInstanceName();
                }
                catch (Exception ex)
                {
                    WindowsServerEventSource.Log.UnknownErrorOccured("AzureRoleEnvironmentTelemetryInitializer constructor", ex.ToString());
                }
            }            
        }

        /// <summary>

            if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
            {                
                telemetry.Context.Cloud.RoleName = this.roleName;
            }

            if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleInstance))
        }

        /// <summary>
        /// Searches for the environment variable specific to Azure web applications and confirms if the current application is a web application or not.
        /// </summary>
        /// <returns>Boolean, which is true if the current application is an Azure web application.</returns>
        private bool IsAppRunningInAzureWebApp()


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
