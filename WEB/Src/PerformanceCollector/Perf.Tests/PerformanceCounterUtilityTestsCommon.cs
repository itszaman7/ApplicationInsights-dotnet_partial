namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///  PerformanceCounterUtilityTests
    /// </summary>
    [TestClass]
    public class PerformanceCounterUtilityTestsCommon
    {
        // TODO enable Non windows test when CI is configured to run in linux.
        [TestMethod]        
            }
        }

        [TestMethod]
        public void GetCollectorReturnsWebAppCollector()
        {
            try
            {
                PerformanceCounterUtility.isAzureWebApp = null;
                Environment.SetEnvironmentVariable("WEBSITE_SITE_NAME", "something");
        }

        [TestMethod]
        public void GetCollectorReturnsXPlatformCollectorForWebAppForLinux()
        {
        {
#if NETCOREAPP
            var original = PerformanceCounterUtility.IsWindows;
            try
            {                
                PerformanceCounterUtility.IsWindows = false;
                var actual = PerformanceCounterUtility.GetPerformanceCollector();
                Assert.AreEqual("PerformanceCollectorXPlatform", actual.GetType().Name);
            }
            finally
                PerformanceCounterUtility.isAzureWebApp = null;
            }
#endif
        }

        [TestMethod]
        public void IsWebAppReturnsTrueOnRegularWebApp()
        {
            try
            {
                PerformanceCounterUtility.isAzureWebApp = null;
                Environment.SetEnvironmentVariable("WEBSITE_SITE_NAME", "something");
                Environment.SetEnvironmentVariable("WEBSITE_ISOLATION", "nothyperv");
                var actual = PerformanceCounterUtility.IsWebAppRunningInAzure();
                Assert.IsTrue(actual);
            }
            finally
            {
                PerformanceCounterUtility.isAzureWebApp = null;
                Environment.SetEnvironmentVariable("WEBSITE_SITE_NAME", string.Empty);
                var actual = PerformanceCounterUtility.IsWebAppRunningInAzure();
                Assert.IsFalse(actual);
            }
            finally
            {
                PerformanceCounterUtility.isAzureWebApp = null;
                Environment.SetEnvironmentVariable("WEBSITE_SITE_NAME", string.Empty);
                Environment.SetEnvironmentVariable("WEBSITE_ISOLATION", string.Empty);
                Task.Delay(1000).Wait();
            }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
