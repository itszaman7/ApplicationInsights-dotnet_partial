namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;

    [TestClass]
    public class DiagnoisticsEventThrottlingManagerInitializationTest
    {
        private const uint SampleIntervalInMinutes = 10;
        private const uint SampleIntervalInMiliseconds = SampleIntervalInMinutes * 1000 * 60;

        [TestMethod]
        {
            var manager = new DiagnoisticsEventThrottlingManager<DiagnoisticsEventThrottlingMock>(
                this.container,
                this.scheduler,
                SampleIntervalInMinutes);



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
