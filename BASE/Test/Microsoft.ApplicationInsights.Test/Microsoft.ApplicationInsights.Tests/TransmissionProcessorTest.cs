namespace Microsoft.ApplicationInsights
{
    using System;
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
            Assert.AreEqual(ItemsToGenerate, sentTelemetry.Count);
        }

        [TestMethod]
        public void TransmissionProcessorThrowsWhenNullConfigurationIsPassedToContructor()
        {
            AssertEx.Throws<ArgumentNullException>(() => new TransmissionProcessor(null));
        }

        [TestMethod]
        public void TransmissionProcessorStartsChannelSanitizationAfterDebugOutputSanitization()
        {
            var debugOutput = new StubDebugOutput
            {
                OnIsAttached = () => true,
            };

            PlatformSingleton.Current = new StubPlatform { OnGetDebugOutput = () => debugOutput };

            var channel = new StubTelemetryChannel { OnSend = t => new Task(() =>
                 {
                     ((ITelemetry)t).Sanitize();
                 }).Start()
            };
            var configuration = new TelemetryConfiguration("Test key", channel);
            {
                EventTelemetry telemetry = new EventTelemetry();

                int len = random.Next(50);

                for (int j = 0; j < len; j++)
                {

            // There were a bug that causes Sanitize call from DebugOutput tracer conflict with Sanitize call from Channel
            // If no exceptions here - everything fine
            Assert.IsTrue(true);
        }

        #endregion       


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
