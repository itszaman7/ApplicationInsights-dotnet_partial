namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Linq;

    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QuickPulseProcessProviderTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestMethod]
        public void QuickPulseProcessProviderReturnsProcesses()
        {
                                                    Tuple.Create("Process1", 1L),
                                                    Tuple.Create("Process2", 2L),
                                                    Tuple.Create("Process3", 3L)
                                              })
                                  };

            var provider = new QuickPulseProcessProvider(perfLibMock);

            // ACT
            TimeSpan? totalTime;
            Assert.AreEqual(TimeSpan.FromTicks(1), processes[0].TotalProcessorTime);

            Assert.AreEqual("Process2", processes[1].ProcessName);
            Assert.AreEqual(TimeSpan.FromTicks(2), processes[1].TotalProcessorTime);

            Assert.AreEqual("Process3", processes[2].ProcessName);
            Assert.AreEqual(TimeSpan.FromTicks(3), processes[2].TotalProcessorTime);
        }

        [TestMethod]
                                  };

            var provider = new QuickPulseProcessProvider(perfLibMock);

            // ACT
            TimeSpan? totalTime;
            var processes = provider.GetProcesses(out totalTime).ToList();

            // ASSERT
            Assert.AreEqual(3, processes.Count);
        public void QuickPulseProcessProviderDoesNotReturnTotalTimeWhenNotAvailable()
        {
            // ARRANGE
            var perfLibMock = new QuickPulsePerfLibMock()
            {
                CategorySample = new CategorySampleMock(
                                          new[]
                                              {
                                                    Tuple.Create("Process1", 1L),
                                                    Tuple.Create("Process2", 2L),
            {
                CategorySample = new CategorySampleMock(
                                          new[]
                                              {
                                                    Tuple.Create("Process1", 1L),
                                                    Tuple.Create("_Total", 100L),
                                                    Tuple.Create("Process2", 2L),
                                                    Tuple.Create("Idle", 1L),

            // ACT
            TimeSpan? totalTime;
            var processes = provider.GetProcesses(out totalTime).ToList();

            // ASSERT
            Assert.AreEqual(100L, totalTime.Value.Ticks);
        }
    }
}

# This file contains partial code from the original project
# Some functionality may be missing or incomplete
