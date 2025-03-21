namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    {
        [TestMethod]
        public void RateCounterGaugeGetValueAndResetGetsTheValueFromJson()
        {
            RawCounterGauge readIoBytes = new RawCounterGauge("READ IO BYTES", "readIoBytes", AzureWebApEnvironmentVariables.App, new CacheHelperTests());
            RawCounterGauge writeIoBytes = new RawCounterGauge("WRITE IO BYTES", "writeIoBytes", AzureWebApEnvironmentVariables.App, new CacheHelperTests());
            RawCounterGauge otherIoBytes = new RawCounterGauge("OTHER IO BYTES", "otherIoBytes", AzureWebApEnvironmentVariables.App, new CacheHelperTests());

            RatioCounterGauge readIoBytesRate = new RatioCounterGauge(@"\Process(??APP_WIN32_PROC??)\Private Bytes", readIoBytes, writeIoBytes);

            Assert.IsTrue(value1 != 0);

           SumUpCountersGauge writeAndOtherBytes = new SumUpCountersGauge("Sum Up Bytes", writeIoBytes, otherIoBytes);

        [TestMethod]
        public void RatioCounterGaugeDoesNotThrowWithNullGauges()


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
