namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector;
    using Microsoft.VisualStudio.TestTools.UnitTesting;    
            NormalizedCPUPercentageGauge normalizedGauge = new NormalizedCPUPercentageGauge(
                "CPU",
                new RawCounterGauge(@"\Process(??APP_WIN32_PROC??)\Normalized Private Bytes", "userTime", AzureWebApEnvironmentVariables.App, new CacheHelperTests()));
            double value1 = gauge.Collect();
            double normalizedValue1 = normalizedGauge.Collect();


            Stopwatch sw = Stopwatch.StartNew();
            Thread.Sleep(TimeSpan.FromSeconds(10));

            double value2 = gauge.Collect();
            double normalizedValue2 = normalizedGauge.Collect();

            Assert.IsTrue(
                Math.Abs(value2 - (normalizedValue2 * initialProcessorsCount)) < 0.005,


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
