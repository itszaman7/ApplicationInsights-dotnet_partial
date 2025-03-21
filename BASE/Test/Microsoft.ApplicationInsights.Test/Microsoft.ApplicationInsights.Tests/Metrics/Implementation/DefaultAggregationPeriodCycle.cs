using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Microsoft.ApplicationInsights.Metrics
{
    /// <summary />
    [TestClass]
    public class DefaultAggregationPeriodCycleTests
            Assert.AreEqual(
                    new DateTimeOffset(2017, 9, 28, 10, 41, 1, 0, TimeSpan.FromHours(-8)),
                    DefaultAggregationPeriodCycle.GetNextCycleTargetTime_UnitTestAccessor(new DateTimeOffset(2017, 9, 28, 10, 40, 31, 25, TimeSpan.FromHours(-8))));

            Assert.AreEqual(
                    new DateTimeOffset(2017, 9, 28, 10, 42, 1, 0, TimeSpan.FromHours(-8)),
                    DefaultAggregationPeriodCycle.GetNextCycleTargetTime_UnitTestAccessor(new DateTimeOffset(2017, 9, 28, 10, 40, 40, 1, TimeSpan.FromHours(-8))));

            Assert.AreEqual(
                    new DateTimeOffset(2017, 9, 28, 10, 42, 1, 0, TimeSpan.FromHours(-8)),

            Assert.AreEqual(
                    new DateTimeOffset(2017, 9, 28, 10, 42, 1, 0, TimeSpan.FromHours(-8)),
                    DefaultAggregationPeriodCycle.GetNextCycleTargetTime_UnitTestAccessor(new DateTimeOffset(2017, 9, 28, 10, 40, 59, 0, TimeSpan.FromHours(-8))));

                    new DateTimeOffset(2017, 9, 28, 10, 42, 1, 0, TimeSpan.FromHours(-8)),
                    DefaultAggregationPeriodCycle.GetNextCycleTargetTime_UnitTestAccessor(new DateTimeOffset(2017, 9, 28, 10, 41, 0, 0, TimeSpan.FromHours(-8))));

            Assert.AreEqual(
                   new DateTimeOffset(2017, 9, 28, 10, 42, 1, 0, TimeSpan.FromHours(12)),
                   DefaultAggregationPeriodCycle.GetNextCycleTargetTime_UnitTestAccessor(new DateTimeOffset(2017, 9, 28, 10, 41, 0, 0, TimeSpan.FromHours(12))));

            Assert.AreEqual(
                   new DateTimeOffset(2017, 9, 28, 10, 42, 1, 0, TimeSpan.FromHours(0)),


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
