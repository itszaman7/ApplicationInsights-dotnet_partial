namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    

    public class TransmissionSenderTest
    {
        [TestClass]
        public class MaxNumberOfTransmissions : TransmissionSenderTest
        {
            [TestMethod]
            public void DefaultValueIsAppropriateForMostApps()
            {
                var sender = new TransmissionSender();
                Assert.AreEqual(10, sender.Capacity);
            }

            [TestMethod]
            public void CanBeSetToZeroToDisableSendingOfTransmissions()
            {
                var sender = new TransmissionSender();
                sender.Capacity = 0;
                Assert.AreEqual(0, sender.Capacity);
            }

            [TestMethod]
            public void ThrowsArgumentOutOfRangeExceptionWhenNewValueLessThanZero()
            {
                var sender = new TransmissionSender();
                AssertEx.Throws<ArgumentOutOfRangeException>(() => sender.Capacity = -1);
            }
        }

        [TestClass]
        public class EnqueueAsync : TransmissionSenderTest
        {
            [TestMethod]
            public void StartsSendingTransmissionAndReturnsImmediatelyToUnblockCallingThread()
            {
                var transmissionCanFinishSending = new ManualResetEventSlim();
                var transmission = new StubTransmission { OnSend = () => { transmissionCanFinishSending.Wait(); return null; } };
                var sender = new TransmissionSender();

                sender.Enqueue(() => transmission);

                transmissionCanFinishSending.Set();
            }
                var sender = new TransmissionSender { Capacity = 1 };
                bool result = sender.Enqueue(() => new StubTransmission());
                Assert.IsTrue(result);
            }

            [TestMethod]
            public void ReturnsFalseWhenNewTransmissionExceedsMaxNumber()
            {
                var sender = new TransmissionSender { Capacity = 0 };
                bool transmissionSent = sender.Enqueue(() => new StubTransmission());
            public void DoesNotInvokeTransmissionGetterWhenMaxNumberOfTransmissionsIsExceededToKeepItBuffered()
            {
                bool transmissionGetterInvoked = false;
                Func<Transmission> transmissionGetter = () =>
                {
                    transmissionGetterInvoked = true;
                    return new StubTransmission();
                };
                var sender = new TransmissionSender { Capacity = 0 };

            public void DoesNotCountNullTransmissionsReturnedFromEmptyBufferAgainstMaxNumber()
            {
                var sender = new TransmissionSender { Capacity = 1 };
                sender.Enqueue(() => null);

                Transmission transmission2 = new StubTransmission();
                Assert.IsTrue(sender.Enqueue(() => transmission2));
            }

            [TestMethod]
                    };

                    var buffer = new TransmissionSender();
                    buffer.Enqueue(() => new StubTransmission());
                }

                Assert.IsFalse(postedBack);
            }
        }

        [TestClass]
        public class TransmissionSent : TransmissionSenderTest
        {
            [TestMethod]
            public void IsRaisedWhenTransmissionIsSentSuccessfully()
            {
                var sender = new TransmissionSender();

                var eventIsRaised = new ManualResetEventSlim();
                object eventSender = null;
            [TestMethod]
            public void IsRaisedWhenTransmissionThrownExceptionWhileSending()
            {
                var sender = new TransmissionSender();

                var eventIsRaised = new ManualResetEventSlim();
                object eventSender = null;
                TransmissionProcessedEventArgs eventArgs = null;
                sender.TransmissionSent += (s, a) =>
                {
                Assert.AreSame(transmission, eventArgs.Transmission);
                Assert.AreSame(exception, eventArgs.Exception);
            }

            [TestMethod]
            public void IsRaisedWhenTransmissionReturnsPartialSuccessResult()
            {
                var sender = new TransmissionSender();

                var eventIsRaised = new ManualResetEventSlim();
                object eventSender = null;
                TransmissionProcessedEventArgs eventArgs = null;
                sender.TransmissionSent += (s, a) =>
                {
                    eventSender = s;
                    eventArgs = a;
                    eventIsRaised.Set();
                };

                var wrapper = new HttpWebResponseWrapper();
                sender.Enqueue(() => transmission);

                Assert.IsTrue(eventIsRaised.Wait(50));
                Assert.AreSame(sender, eventSender);
                Assert.AreSame(transmission, eventArgs.Transmission);
                Assert.AreSame(wrapper, eventArgs.Response);
            }

            [TestMethod]
            public void IsRaisedWhenTransmissionIsThrottledLocallyWithItems()
                sender.ApplyThrottle = true;

                var eventIsRaised = new ManualResetEventSlim();
                var firedCount = 0;
                var eventArgs = new List<Implementation.TransmissionProcessedEventArgs>();
                sender.TransmissionSent += (s, a) =>
                {
                    firedCount++;
                    eventArgs.Add(a);
                    if (firedCount == 2)
            [TestMethod]
            public void IsRaisedWhenTransmissionIsThrottledLocallyWithByteArray()
            {
                var sender = new TransmissionSender();
                sender.ApplyThrottle = true;

                var eventIsRaised = new ManualResetEventSlim();
                var firedCount = 0;
                var eventArgs = new List<Implementation.TransmissionProcessedEventArgs>();
                {
                    telemetryItems.Add(new DataContracts.EventTelemetry());
                }

                var wrapper = new HttpWebResponseWrapper();
                Transmission transmission = new StubTransmission(JsonSerializer.Serialize(telemetryItems, false)) { OnSend = () => wrapper };
                sender.Enqueue(() => transmission);

                Assert.IsTrue(eventIsRaised.Wait(50));
                Assert.AreEqual(2, firedCount);
                var telemetryItems = new List<ITelemetry>();
                for (var i = 0; i < sender.ThrottleLimit + 10; i++)
                {
                    telemetryItems.Add(new DataContracts.EventTelemetry());
                }

                var wrapper = new HttpWebResponseWrapper();
                Transmission transmission = new StubTransmission(telemetryItems) { OnSend = () => wrapper };
                transmission.IsFlushAsyncInProgress = true;
                sender.Enqueue(() => transmission);

                Assert.IsTrue(eventIsRaised.Wait(50));
                // Both accepted and rejected transmission has flush task
                Assert.IsTrue(eventArgs[0].Transmission.IsFlushAsyncInProgress);
                Assert.IsTrue(eventArgs[1].Transmission.IsFlushAsyncInProgress);
            }

            [TestMethod]
            public void WaitForPreviousTransmissionsToCompleteCancelationToken()
            {
                var sender = new TransmissionSender();
                Assert.AreEqual(TaskStatus.Canceled,
                    sender.WaitForPreviousTransmissionsToComplete(new CancellationToken(true)).Result);
                Assert.AreEqual(TaskStatus.Canceled,
                    sender.WaitForPreviousTransmissionsToComplete(1, new CancellationToken(true)).Result);
            }

            [TestMethod]
            public void WaitForPreviousTransmissionsToCompleteReturnsSuccessWithNoInFlightTransmission()
            {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
