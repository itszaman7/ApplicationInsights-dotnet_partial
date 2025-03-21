namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel
{
    using System;
    using System.Diagnostics;
    using System.Linq;
        {
            string emptyConfig = ConfigurationHelpers.GetEmptyConfig();

            var node = ConfigurationHelpers.GetTelemetryChannelFromDefaultSink(configAfterTransform)
                .FirstOrDefault(element => element.Attribute("Type").Value == ConfigurationHelpers.GetPartialTypeName(typeToFind));

        }

        {
            string emptyConfig = ConfigurationHelpers.GetEmptyConfig();
            XDocument configAfterInstall = ConfigurationHelpers.InstallTransform(emptyConfig);            
            XDocument configAfterUninstall = ConfigurationHelpers.UninstallTransform(configAfterInstall.ToString());
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
