#if NETCOREAPP
namespace Microsoft.ApplicationInsights.Tests
{

    /// </summary>
    [TestClass]
        {
           this.PerformanceCollectorSanityTest(new PerformanceCollectorXPlatform(), @"\Process(??APP_WIN32_PROC??)\% Processor Time Normalized", "Process", @"% Processor Time Normalized", null);
        }

        public void PerformanceCollectorAddRemoveCountersForXPlatformTest()
        {
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
