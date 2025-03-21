namespace Microsoft.ApplicationInsights.Channel
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Linq;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    
    using DataContracts;
    using System.Threading.Tasks;
    using System.Threading;

    [TestClass]
    public class InMemoryChannelTest
    {
        [TestMethod]
            sentTelemetry.Context.InstrumentationKey = Guid.NewGuid().ToString();

            channel.Send(sentTelemetry);
            IEnumerable<ITelemetry> telemetries = telemetryBuffer.Dequeue();

            Assert.AreEqual(1, telemetries.Count());
            Assert.AreSame(sentTelemetry, telemetries.First());
            var telemetryBuffer = new TelemetryBuffer();
            var channel = new InMemoryChannel(telemetryBuffer, new InMemoryTransmitter(telemetryBuffer));
            var sentTelemetry = new StubTelemetry();
            // No instrumentation key

            using (TestEventListener listener = new TestEventListener())
            {
                listener.EnableEvents(CoreEventSource.Log, EventLevel.Verbose);
            {
                SendingInterval = TimeSpan.FromDays(1),
                EndpointAddress = "http://localhost/bad"
            };
            
            var telemetry = new TraceTelemetry("test");
            telemetry.Context.InstrumentationKey = Guid.NewGuid().ToString();
            telemetry.Context.InstrumentationKey = Guid.NewGuid().ToString();
            channel.Send(telemetry);

            using (TestEventListener listener = new TestEventListener())
            {
                listener.EnableEvents(CoreEventSource.Log, EventLevel.Warning);
                channel.Flush(TimeSpan.FromSeconds(1));

                var expectedMessage = listener.Messages.First();
                Assert.AreEqual(24, expectedMessage.EventId);
            }
        }
#endif



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
