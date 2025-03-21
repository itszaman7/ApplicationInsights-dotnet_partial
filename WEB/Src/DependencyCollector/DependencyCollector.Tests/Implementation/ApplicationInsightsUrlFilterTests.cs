namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ApplicationInsightsUrlFilterTests
    {
                Assert.IsTrue(urlFilter.IsApplicationInsightsUrl(url));
            }
        }

        [TestMethod]
        public void IsApplicationInsightsUrlReturnsTrueForQuickPulseServiceEndpoint()
        {
        [TestMethod]
        public void IsApplicationInsightsUrlReturnsTrueForTelemetryChannelEndpointAddress()
        {
                string url = null;
                ApplicationInsightsUrlFilter urlFilter = new ApplicationInsightsUrlFilter(configuration);
                Assert.IsFalse(urlFilter.IsApplicationInsightsUrl(url));
                url = string.Empty;
                Assert.IsFalse(urlFilter.IsApplicationInsightsUrl(url));
            }
        }
                Assert.IsFalse(urlFilter.IsApplicationInsightsUrl(url));
            }
        }

        [TestMethod]
        public void IsApplicationInsightsUrlReturnsTrueForTelemetryServiceEndpointIfTelemetryChannelIsNull()
        {
            using (TelemetryConfiguration configuration = this.CreateStubTelemetryConfiguration())
            {
                configuration.TelemetryChannel = null;
                string url = "https://dc.services.visualstudio.com/v2/track";
                ApplicationInsightsUrlFilter urlFilter = new ApplicationInsightsUrlFilter(configuration);
                Assert.IsTrue(urlFilter.IsApplicationInsightsUrl(url));
            }
        }

        private TelemetryConfiguration CreateStubTelemetryConfiguration()
        {
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            configuration.TelemetryChannel = new StubTelemetryChannel { EndpointAddress = "https://endpointaddress" };
            configuration.InstrumentationKey = Guid.NewGuid().ToString();


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
