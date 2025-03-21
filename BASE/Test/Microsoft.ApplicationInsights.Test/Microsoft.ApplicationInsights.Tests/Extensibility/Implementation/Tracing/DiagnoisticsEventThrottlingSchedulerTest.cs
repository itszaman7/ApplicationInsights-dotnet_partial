namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class DiagnoisticsEventThrottlingSchedulerTest : IDisposable
    {
        private const int SchedulingRoutineRunInterval = 10;
        private const int ExecuteTimes = 3;

        private readonly DiagnoisticsEventThrottlingScheduler scheduler = new DiagnoisticsEventThrottlingScheduler();

        public void Dispose()
        {
            this.scheduler.Dispose();
        }
                failedWithExpectedException = true;
            }

            Assert.IsTrue(failedWithExpectedException);
        }

        [TestMethod]
        [TestMethod]
        public void TestRemovingScheduledActionsIsNullException()
        {
            bool failedWithExpectedException = false;
            try
            {
                this.scheduler.RemoveScheduledRoutine(null);
            }

            Assert.IsTrue(failedWithExpectedException);
        }

        [TestMethod]
        public void TestRemovingScheduledActionsIsNotOfExpectedType()
        {
            bool failedWithExpectedException = false;
            try
            {
                this.scheduler.RemoveScheduledRoutine(new object());
            }
            catch (ArgumentException)
                failedWithExpectedException = true;
            }

            Assert.IsTrue(failedWithExpectedException);
        }
        
        private class RoutineCounter
        {
            public int ExecutedTimes { get; private set; }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
