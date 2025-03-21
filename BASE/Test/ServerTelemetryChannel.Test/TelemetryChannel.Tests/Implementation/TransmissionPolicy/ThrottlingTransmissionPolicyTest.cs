namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TransmissionPolicy
{
    using System;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class ThrottlingTransmissionPolicyTest
    {
        [TestClass]
        [TestCategory("TransmissionPolicy")]
        public class HandleTransmissionSentEvent : ErrorHandlingTransmissionPolicyTest
            {
                this.EvaluateIfStatusCodeTriggersThrottling(ResponseStatusCodes.ResponseCodeTooManyRequestsOverExtendedTime, 0, 0, 0, false);
            }

            [TestMethod]
            public void AssertPaymentRequiredDoesntChangeCapacity()
            {
                this.EvaluateIfStatusCodeIgnored(ResponseCodePaymentRequired);
            }

                const string unparsableDate = "no one can parse me! :)";

                var transmitter = new StubTransmitter();
                var policy = new ThrottlingTransmissionPolicy();
                policy.Initialize(transmitter);

                using (var listener = new TestEventListener())
                {
                    const long AllKeyword = -1;
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways, (EventKeywords)AllKeyword);
                            {
                    Assert.AreEqual(unparsableDate, (string)trace.Payload[0]);
                }
            }

            private void EvaluateIfStatusCodeTriggersThrottling(int responseCode, int? expectedSenderCapacity, int? expectedBufferCapacity, int? expectedStorageCapacity, bool hasFlushTask)
            {
                const int RetryAfterSeconds = 2;
                var waitForTheFirstApplyAsync = TimeSpan.FromMilliseconds(100);
                var waitForTheSecondApplyAsync = TimeSpan.FromMilliseconds(RetryAfterSeconds * 1000 + 500);

                Assert.AreEqual(expectedBufferCapacity, policy.MaxBufferCapacity);
                Assert.AreEqual(expectedStorageCapacity, policy.MaxStorageCapacity);

                // ASSERT: Throttle expires and policy will be reset.
                Assert.IsTrue(transmitter.IsApplyInvoked(waitForTheSecondApplyAsync));

                Assert.IsNull(policy.MaxSenderCapacity);
                Assert.IsNull(policy.MaxBufferCapacity);
                Assert.IsNull(policy.MaxStorageCapacity);
            }
                var transmitter = new StubTransmitterEvalOnApply();

                var policy = new AuthenticationTransmissionPolicy()
                {
                    Enabled = true,
                };
                policy.Initialize(transmitter);

                // ACT
                transmitter.InvokeTransmissionSentEvent(statusCode, default, false);
                public StubTransmitterEvalOnApply()
                {
                    this.autoResetEvent = new AutoResetEvent(false);
                    this.OnApplyPolicies = () => this.autoResetEvent.Set();
                }

                public void InvokeTransmissionSentEvent(int responseStatusCode, TimeSpan retryAfter, bool isFlushAsyncInProgress)
                {
                    this.OnTransmissionSent(new TransmissionProcessedEventArgs(
                        transmission: new StubTransmission() { IsFlushAsyncInProgress = isFlushAsyncInProgress },
                        exception: null,
                        response: new HttpWebResponseWrapper()
                        {
                            StatusCode = responseStatusCode,
                            StatusDescription = null,
                            RetryAfterHeader = DateTime.Now.ToUniversalTime().Add(retryAfter).ToString("R", CultureInfo.InvariantCulture),
                        }
                    ));
                }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
