namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DiagnoisticsEventThrottlingInitializationTest
    {

            Assert.AreEqual(
                ThrottleAfterCountDefault,
                throttling.ThrottleAfterCount,
                "Unexpected ThrottleAfterCount state");

        {
            Assert.AreEqual(
                DiagnoisticsEventThrottlingDefaults.MinimalThrottleAfterCount,
                new DiagnoisticsEventThrottling(DiagnoisticsEventThrottlingDefaults.MinimalThrottleAfterCount).ThrottleAfterCount);

            Assert.AreEqual(
                DiagnoisticsEventThrottlingDefaults.MaxThrottleAfterCount,
                new DiagnoisticsEventThrottling(DiagnoisticsEventThrottlingDefaults.MaxThrottleAfterCount).ThrottleAfterCount);

            catch (ArgumentOutOfRangeException)
            {
                failedWithExpectedException = true;
            }

            Assert.IsTrue(failedWithExpectedException);
            try
            {
                new DiagnoisticsEventThrottling(DiagnoisticsEventThrottlingDefaults.MaxThrottleAfterCount + 1);
            }
            catch (ArgumentOutOfRangeException)
            {
                failedWithExpectedException = true;
            }

            Assert.IsTrue(failedWithExpectedException);
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
