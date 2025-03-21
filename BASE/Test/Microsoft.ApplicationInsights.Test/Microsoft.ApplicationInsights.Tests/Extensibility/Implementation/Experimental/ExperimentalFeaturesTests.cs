using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Experimental
    public class ExperimentalFeaturesTests
    {
        {
            var telemetryConfiguration = new TelemetryConfiguration();

            bool value = telemetryConfiguration.EvaluateExperimentalFeature("abc");
            Assert.IsFalse(value);
        }

            bool fakeValue = telemetryConfiguration.EvaluateExperimentalFeature("fake");
            Assert.IsFalse(fakeValue);
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
