#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;

    [TestClass]
    public class DeviceTelemetryInitializerTest

            Assert.Null(requestTelemetry.Context.Device.Id);

            source.Initialize(requestTelemetry);

            string id = requestTelemetry.Context.Device.Id;

            string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            string hostName = Dns.GetHostName();

            if (hostName.EndsWith(domainName, StringComparison.OrdinalIgnoreCase) == false)
            {
                hostName = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", hostName, domainName);
            }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
