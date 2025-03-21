namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TransmissionPolicy
{
    using System;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using Microsoft.ApplicationInsights.Channel.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [TestCategory("TransmissionPolicy")]
        [TestMethod]
        public void Verify400DoesNotTriggerThrottling() => this.EvaluateIfStatusCodeIgnored(ResponseStatusCodes.BadRequest);

        [TestMethod]
        public void Verify200DoesNotTriggerThrottling() => this.EvaluateIfStatusCodeIgnored(ResponseStatusCodes.Success);

        [TestMethod]
        public void VerifyOtherDoesNotTriggerThrottling() => this.EvaluateIfStatusCodeIgnored(000);

        private void EvaluateIfStatusCodeTriggersThrottling(int statusCode)

            // SETUP
            var transmitter = new StubTransmitterEvalOnApply();

            var policy = new AuthenticationTransmissionPolicy()
            {
                Enabled = true,
            };

            // we override the default timer here to speed up unit tests.
            policy.PauseTimer = new TaskTimerInternal { Delay = retryDelay };
            policy.Initialize(transmitter);

            // ACT
            transmitter.InvokeTransmissionSentEvent(statusCode);

            // ASSERT: First Handle will trigger Throttle and delay.
            Assert.IsTrue(transmitter.IsApplyInvoked(waitForTheFirstApplyAsync));

            Assert.AreEqual(0, policy.MaxSenderCapacity);

            Assert.IsNull(policy.MaxSenderCapacity);
            Assert.IsNull(policy.MaxBufferCapacity);
            Assert.IsNull(policy.MaxStorageCapacity);
        }

        private void EvaluateIfStatusCodeIgnored(int statusCode)
        {
            var waitForTheFirstApplyAsync = TimeSpan.FromMilliseconds(100);

            transmitter.InvokeTransmissionSentEvent(statusCode);

            // ASSERT: The Apply event handler should not be called.
            Assert.IsFalse(transmitter.IsApplyInvoked(waitForTheFirstApplyAsync));

            // ASSERT: Capacities should have default values.
            Assert.IsNull(policy.MaxSenderCapacity);
            Assert.IsNull(policy.MaxBufferCapacity);
            Assert.IsNull(policy.MaxStorageCapacity);
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
