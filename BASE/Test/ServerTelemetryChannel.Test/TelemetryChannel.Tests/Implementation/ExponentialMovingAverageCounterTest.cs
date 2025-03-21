namespace Microsoft.ApplicationInsights.WindowsServer.Channel.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    

    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;

    /// <summary>
    /// Note: exponential moving average information may be found at https://en.wikipedia.org/wiki/Moving_average
    /// </summary>
    public class ExponentialMovingAverageCounterTest
    {
        [TestMethod]
        public void AverageValueIsZeroPriorToStart()
        {
            var counter = new ExponentialMovingAverageCounter(.1);

            Assert.AreEqual(0, counter.Average);
        }

        [TestMethod]
        public void AverageValueIsFirstIntervalValuePriorToClosingOfFirstInterval()
            for (int i = 0; i < IncrementCount; i++)
            Assert.AreEqual(IncrementCount, counter.Average);
        }

        [TestMethod]
        public void AverageValueIsFirstIntervalValueAfterClosingOfFirstInterval()
        {
            const int IncrementCount = 3;

            for (int i = 0; i < IncrementCount; i++)
            {
                counter.Increment();
            }
        }

        [TestMethod]
        public void AverageValueIsMovingAverageAfterClosingOfAtLeastTwoIntervals()
        {
            var counter = new ExponentialMovingAverageCounter(.1);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
