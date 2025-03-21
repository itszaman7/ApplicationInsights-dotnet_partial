namespace Microsoft.ApplicationInsights.Channel
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Extensibility;
    using System.Net.Http;
    using System.Threading;

    public class InMemoryTransmitterTests
    {
        [TestClass]
        public class SendingInterval
        {
            [TestMethod]
            public void DefaultValueIsAppropriateForProductionEnvironmentAndUnitTests()
            {
                Assert.AreEqual(expectedValue, transmitter.SendingInterval);
            }

            {
                public bool WasCalled = false;

                public override IEnumerable<ITelemetry> Dequeue()
                {
                    Assert.IsTrue(SdkInternalOperationsMonitor.IsEntered());
                    task.Wait();

                    WasCalled = true;
                    return base.Dequeue();
                }
            }
                {
                    buffer.OnFull();

                    for (int i = 0; i < 10; i++)
                    {
                        if (buffer.WasCalled)
                var buffer = new TelemetryBufferWithInternalOperationValidation();
                var transmitter = new InMemoryTransmitter(buffer);
                transmitter.Flush(TimeSpan.FromSeconds(1));

                for (int i = 0; i < 10; i++)
                {
                        break;
                    }

                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
