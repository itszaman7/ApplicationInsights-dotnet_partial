namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.Mocks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TestFramework;

    [TestClass]
    public class PortalDiagnosticsSenderTest
    {
        private readonly IList<ITelemetry> sendItems = new List<ITelemetry>();
        private readonly PortalDiagnosticsSender nonThrottlingPortalSender;

        private readonly PortalDiagnosticsSender throttlingPortalSender;

        private readonly IDiagnoisticsEventThrottlingManager throttleAllManager
            = new DiagnoisticsEventThrottlingManagerMock(true);

        private readonly IDiagnoisticsEventThrottlingManager dontThrottleManager
            = new DiagnoisticsEventThrottlingManagerMock(false);

        public PortalDiagnosticsSenderTest()
        {
                configuration, 
                this.dontThrottleManager);

            this.throttlingPortalSender = new PortalDiagnosticsSender(
                configuration,
                this.throttleAllManager); 
        }

        [TestMethod]
        public void TestSendingOfEvent()
            Assert.AreEqual("SDKTelemetry", trace.Context.Operation.SyntheticSource);
        }

        [TestMethod]
        public void TestSendingEmptyPayload()
        {
            var evt = new TraceEvent
            {
                MetaData = new EventMetaData
                {
            Assert.IsNotNull(trace);
            Assert.AreEqual(
                "AI (Internal): [] Something failed",
                trace.Message);
            Assert.AreEqual(0, trace.Properties.Count);
        }

        [TestMethod]
        public void SendNotFailIfChannelNotInitialized()
        {
            var configuration = new TelemetryConfiguration();
            var portalSenderWithDefaultCOnfiguration = new PortalDiagnosticsSender(
                configuration,
                this.dontThrottleManager);

            var evt = new TraceEvent
            {
                MetaData = new EventMetaData
                {
                    EventId = 10,
                    Keywords = 0x20,
                    Level = EventLevel.Warning,
                    MessageFormat = "Something failed"
                },
                Payload = null
            };

            portalSenderWithDefaultCOnfiguration.Send(evt);
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
