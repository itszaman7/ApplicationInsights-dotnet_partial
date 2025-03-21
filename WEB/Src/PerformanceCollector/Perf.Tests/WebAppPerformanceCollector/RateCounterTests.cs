namespace Microsoft.ApplicationInsights.Tests
{
    using System.Globalization;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    {
        [TestMethod]
        public void RateCounterGaugeGetValueAndResetGetsTheValueFromJson()

            double value = privateBytesRate.Collect();

            System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(7));
            // Rate should be (200000-10000)/ 7 secs  = ~14000
            value = privateBytesRate.Collect();


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
