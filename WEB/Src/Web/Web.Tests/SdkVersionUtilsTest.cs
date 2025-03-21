namespace Microsoft.ApplicationInsights.Web.Implementation
{
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.TestFramework;
            string expected = SdkVersionHelper.GetExpectedSdkVersion(typeof(RequestTrackingTelemetryModule), prefix: string.Empty);

            Assert.AreEqual(expected, SdkVersionUtils.GetSdkVersion(null));
        }

        [TestMethod]
        public void GetSdkVersionReturnsVersionWithoutPrefixForStringEmpty()
        {
            Assert.AreEqual(expected, SdkVersionUtils.GetSdkVersion(string.Empty));
        }
            string expected = SdkVersionHelper.GetExpectedSdkVersion(typeof(RequestTrackingTelemetryModule), prefix: "lala");

        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
