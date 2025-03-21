namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    [TestClass]    
    public class CPUPercenageGaugeTests
    {
        [TestMethod]
                "CPU",
                new RawCounterGauge(@"\Process(??APP_WIN32_PROC??)\Private Bytes * 2", "userTime", AzureWebApEnvironmentVariables.App, new CacheHelperTests()));
            Stopwatch sw = Stopwatch.StartNew();
            Thread.Sleep(TimeSpan.FromSeconds(10));
            long actualSleepTimeTicks = sw.Elapsed.Ticks;
            double value2 = gauge.Collect();
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
