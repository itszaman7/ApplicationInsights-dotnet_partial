namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.IO;
    using System.Linq;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.WindowsServer.Channel.Helpers;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    using TaskEx = System.Threading.Tasks.Task;
    using System.Threading.Tasks;
    using System.Threading;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TransmissionPolicy;

    public class TransmitterTest
    {
        private static StubTransmissionSender CreateSender(ICollection<Transmission> enqueuedTransmissions)
        {
            var sender = new StubTransmissionSender();
            sender.OnEnqueue = getTransmission =>
            {
                enqueuedTransmissions.Add(getTransmission());
                return false;
            };

            return sender;
        }

        private static Transmitter CreateTransmitter(
            TransmissionSender sender = null, 
            TransmissionBuffer buffer = null, 
            TransmissionStorage storage = null, 
            IEnumerable<TransmissionPolicy.TransmissionPolicy> policies = null)
        {
            return new Transmitter(
                sender ?? new StubTransmissionSender(),
                buffer ?? new StubTransmissionBuffer(),
                storage ?? new StubTransmissionStorage(),
                new TransmissionPolicyCollection(policies));
        }

        [TestClass]
        public class Constructor : TransmitterTest
        {
            [TestMethod]
            public void InitializesTransmissionPolicies()
            {
                Transmitter policyTransmitter = null;
                var policy = new StubTransmissionPolicy();
                policy.OnInitialize = t => policyTransmitter = t;

                Transmitter transmitter = CreateTransmitter(policies: new[] { policy });

                Assert.AreSame(transmitter, policyTransmitter);
            }
        }

        [TestClass]
        public class StorageFolder : TransmitterTest
        {
            [TestMethod]
            public void TransmittionIsSavedToStorageFolder()
            {
                var testDirectory = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
                testDirectory.Create();

                try
                {
                    Transmitter transmitter = new Transmitter();
                    transmitter.StorageFolder = testDirectory.FullName;

                    transmitter.MaxSenderCapacity = 0;
                    transmitter.MaxBufferCapacity = 0;

                    transmitter.Initialize();

                    transmitter.Enqueue(new StubTransmission());

                    Assert.AreEqual(1, testDirectory.EnumerateFiles().Count());
                }
                finally
                {
                    testDirectory.Delete(true);
                }
            }
        }

        [TestClass]
        public class MaxBufferCapacity : TransmitterTest
        {
            [TestMethod]
            public void ReturnsCurrentTransmissionBufferCapacityByDefault()
            {
                var buffer = new StubTransmissionBuffer { Capacity = 42 };
                Transmitter transmitter = CreateTransmitter(buffer: buffer);
                Assert.AreEqual(42, transmitter.MaxBufferCapacity);
            }

            [TestMethod]
            public void ReturnsNewValueImmediatelyAfterPropertyIsSet()
            {
                Transmitter transmitter = CreateTransmitter();
                transmitter.MaxBufferCapacity = 42;
                Assert.AreEqual(42, transmitter.MaxBufferCapacity);
            }

            [TestMethod]
            public void ReturnsMaximumTransmissionBufferCapacityRegardlessOfPolicyInEffect()
            {
                var buffer = new StubTransmissionBuffer { Capacity = 42 };
                var policy = new StubTransmissionPolicy { MaxBufferCapacity = 0 };
                Transmitter transmitter = CreateTransmitter(buffer: buffer, policies: new[] { policy });

                policy.Apply();

                Assert.AreEqual(42, transmitter.MaxBufferCapacity);
            }

            [TestMethod]
            public void ChangesCurrentBufferCapacityImmediatelyWhenNoOverridingPoliciesAreInEffect()
            {
                var buffer = new StubTransmissionBuffer();
                Transmitter transmitter = CreateTransmitter(buffer: buffer);
                transmitter.ApplyPolicies();

                transmitter.MaxBufferCapacity = 42;

                Assert.AreEqual(42, buffer.Capacity);
            }

            [TestMethod]
            public void DoesNotChangeCurrentBufferCapacityWhenOverridingPolicyIsInEffect()
            {
                var buffer = new StubTransmissionBuffer();
                var policy = new StubTransmissionPolicy { MaxBufferCapacity = 0 };
                Transmitter transmitter = CreateTransmitter(buffer: buffer, policies: new[] { policy });
                policy.Apply();

                transmitter.MaxBufferCapacity = 42;

                Assert.AreEqual(0, buffer.Capacity);
            }
        }

        [TestClass]
        public class MaxSenderCapacity : TransmitterTest
        {
            [TestMethod]
            public void ReturnsCurrentTransmissionSenderCapacityByDefault()
            {
                var sender = new StubTransmissionSender { Capacity = 42 };
                Transmitter transmitter = CreateTransmitter(sender: sender);
                Assert.AreEqual(42, transmitter.MaxSenderCapacity);
            }

            [TestMethod]
            public void ReturnsNewValueImmediatelyAfterPropertyIsSet()
            {
                Transmitter transmitter = CreateTransmitter();
                transmitter.MaxSenderCapacity = 42;
                Assert.AreEqual(42, transmitter.MaxSenderCapacity);
            }

            [TestMethod]
            public void ReturnsMaximumTransmissionSenderCapacityRegardlessOfPolicyInEffect()
            {
                var sender = new StubTransmissionSender { Capacity = 42 };
                var policy = new StubTransmissionPolicy { MaxSenderCapacity = 0 };
                Transmitter transmitter = CreateTransmitter(sender: sender, policies: new[] { policy });

                policy.Apply();

                Assert.AreEqual(42, transmitter.MaxSenderCapacity);
            }

            [TestMethod]
            public void ChangesCurrentTransmissionSenderCapacityImmediatelyWhenNoOverridingPoliciesAreInEffect()
            {
                var sender = new StubTransmissionSender();
                Transmitter transmitter = CreateTransmitter(sender: sender);
                transmitter.ApplyPolicies();

                transmitter.MaxSenderCapacity = 42;

                Assert.AreEqual(42, sender.Capacity);
                policy.Apply();

                transmitter.MaxSenderCapacity = 42;

                Assert.AreEqual(0, sender.Capacity);
            }
        }

        [TestClass]
        public class MaxStorageCapacity : TransmitterTest
        {
            [TestMethod]
            public void ReturnsCurrentTransmissionStorageCapacityByDefault()
            {
                var storage = new StubTransmissionStorage { Capacity = 42 };
                Transmitter transmitter = CreateTransmitter(storage: storage);
                Assert.AreEqual(42, transmitter.MaxStorageCapacity);
            }

            [TestMethod]
            public void ReturnsMaximumTransmissionStorageCapacityRegardlessOfPolicyInEffect()
            {
                var storage = new StubTransmissionStorage { Capacity = 42 };
                var policy = new StubTransmissionPolicy { MaxStorageCapacity = 0 };
                Transmitter transmitter = CreateTransmitter(storage: storage, policies: new[] { policy });

                policy.Apply();

                Assert.AreEqual(42, transmitter.MaxStorageCapacity);
            }
                policy.Apply();

                transmitter.MaxStorageCapacity = 42;

                Assert.AreEqual(0, storage.Capacity);
            }
        }

        [TestClass]
        public class Initialize : TransmitterTest
            [TestMethod]
            public void InitializeCallsStorageInitialize()
            {
                IApplicationFolderProvider provider = null;
                var storage = new StubTransmissionStorage();
                storage.OnInitialize = _ => provider = _;

                Transmitter transmitter = CreateTransmitter(null, null, storage);
                transmitter.Initialize();

                Assert.IsNotNull(provider);
            }
        }

        [TestClass]
        public class ApplyPoliciesAsync : TransmitterTest
        {
            

            [TestMethod]
            public void SetsSenderCapacityToMinValueReturnedByTransmissionPolicies()
            {
                var sender = new StubTransmissionSender();
                var policies = new[]
                {
                    new StubTransmissionPolicy { MaxSenderCapacity = 4 },
                    new StubTransmissionPolicy { MaxSenderCapacity = 2 },
                };

                Transmitter transmitter = CreateTransmitter(sender: sender, policies: policies);
            public void SetsBufferCapacityToMinValueReturnedByTransmissionPolicies()
            {
                var buffer = new StubTransmissionBuffer();
                var policies = new[]
                {
                    new StubTransmissionPolicy { MaxBufferCapacity = 4 },
                    new StubTransmissionPolicy { MaxBufferCapacity = 2 },
                };

                Transmitter transmitter = CreateTransmitter(buffer: buffer, policies: policies);
            public void SetsStorageCapacityToMinValueReturnedByTransmissionPolicies()
            {
                var storage = new StubTransmissionStorage();
                var policies = new[]
                {
                    new StubTransmissionPolicy { MaxStorageCapacity = 4 },
                    new StubTransmissionPolicy { MaxStorageCapacity = 2 },
                };

                Transmitter transmitter = CreateTransmitter(storage: storage, policies: policies);
                transmitter.ApplyPolicies();

                Assert.AreEqual(2, storage.Capacity);
            }

            [TestMethod]
            public void DoesNotChangeComponentCapacityIfNoneOfPoliciesAreApplicable()
            {
                var sender = new StubTransmissionSender { Capacity = 1 };
                var buffer = new StubTransmissionBuffer { Capacity = 10 };
                var storage = new StubTransmissionStorage { Capacity = 100 };
                var policy = new StubTransmissionPolicy()
                {
                    MaxSenderCapacity = 0,
                    MaxBufferCapacity = 0,
                    MaxStorageCapacity = 0,
                };

                Transmitter transmitter = CreateTransmitter(sender, buffer, storage, new[] { policy });

                IList<Transmission> enqueuedTransmissions = new List<Transmission>();

                var sender = new StubTransmissionSender { Capacity = 2 };
                var buffer = new StubTransmissionBuffer { Capacity = 1 };
                var storage = new StubTransmissionStorage { Capacity = 100 };

                var policies = new[] { new StubTransmissionScheduledPolicy(), new StubTransmissionScheduledPolicy() };
                Transmitter transmitter = CreateTransmitter(sender, buffer, storage, policies: policies);

                var items = new List<ITelemetry> { new EventTelemetry(), new EventTelemetry() };

            [TestMethod]
            public void RestoresOriginalComponentCapacityInCaseOfOnePolicyRunningByTimerTwoTimes()
            {
                IList<Transmission> enqueuedTransmissions = new List<Transmission>();

                var sender = new StubTransmissionSender { Capacity = 2 };
                var buffer = new StubTransmissionBuffer { Capacity = 1 };
                var storage = new StubTransmissionStorage { Capacity = 100 };

                    itemsAccepted: 1,
                    errorCodes: new[] { "500" });

                var wrapper = new HttpWebResponseWrapper
                {
                    StatusCode = 500,
                    Content = response
                };

                sender.OnTransmissionSent(new TransmissionProcessedEventArgs(transmission, null, wrapper));
            [TestMethod]
            public void EmptiesStorageIfCapacityIsZero()
            {              
                //// We set capacity to 0 and clear the cache when DC responds with 439.

                var buffer = new StubTransmissionBuffer();
                buffer.Enqueue(() => new StubTransmission());
                var storage = new StubTransmissionStorage();
                storage.Enqueue(() => new StubTransmission());
                var sender = new StubTransmissionSender();

                Assert.AreEqual(0, storage.Queue.Count);
            }
            public void MovesTransmissionsFromStorageToBufferWhenBufferCapacityIsGreaterThanZero()
            {
                var storedTransmission = new StubTransmission();

                var storage = new StubTransmissionStorage();
                storage.Enqueue(() => storedTransmission);

                var buffer = new TransmissionBuffer();

                var policy = new StubTransmissionPolicy();
                policy.MaxBufferCapacity = 0;

                Transmitter transmitter = CreateTransmitter(buffer: buffer, storage: storage, policies: new[] { policy });

                policy.MaxBufferCapacity = 1;
                policy.Apply();

                Transmission bufferedTransmission = buffer.Dequeue();
                Assert.AreSame(storedTransmission, bufferedTransmission);
            }

            [TestMethod]
            public void MovesTransmissionsFromBufferToStorageWhenBufferCapacityIsZero()
            {
                var storage = new StubTransmissionStorage { Capacity = 1 };

                var bufferedTransmission = new StubTransmission();
                var buffer = new TransmissionBuffer();
                storage.Enqueue(() => bufferedTransmission);
                
                var bufferedTransmission = new StubTransmission();
                var buffer = new TransmissionBuffer();
                buffer.Enqueue(() => bufferedTransmission);

                Transmission sentTransmission = null;
                var sender = new StubTransmissionSender();
                sender.OnEnqueue = getTransmission =>
                {
                    sentTransmission = getTransmission();
                    return false;
                    {
                        sentTransmission = getTransmission();
                        return sentTransmission != null;
                    },
                };

                Transmitter transmitter = CreateTransmitter(sender: sender);

                var transmission = new StubTransmission();
                transmitter.Enqueue(transmission);
                Transmitter transmitter = CreateTransmitter(sender: sender, buffer: buffer);

                var transmission = new StubTransmission();
                transmitter.Enqueue(transmission);

                Assert.AreSame(transmission, bufferedTransmission);
            }

            [TestMethod]
            public void StoresTransmissionWhenBufferIsFull()
                    {
                        storedTransmission = transmission;
                        return false;
                    }
                };

                var sender = new StubTransmissionSender { OnEnqueue = t => false };
                var buffer = new StubTransmissionBuffer { OnEnqueue = t => false };
                Transmitter transmitter = CreateTransmitter(sender: sender, buffer: buffer, storage: storage);

                transmitter.Enqueue(enqueuedTransmission);

                Assert.AreSame(enqueuedTransmission, storedTransmission);
            }

            [TestMethod]
            public void AppliesTransmistionPoliciesIfTheyNeverBeenAppliedBefore()
            {
                var senderPolicy = new StubTransmissionPolicy { MaxSenderCapacity = 0 };
                var sender = new StubTransmissionSender { Capacity = 1 };
                {
                    OnEnqueue = getTransmission =>
                    {
                        sentTransmission = getTransmission();
                        return sentTransmission != null;
                    },
                };

                Transmitter transmitter = CreateTransmitter(sender: sender);

                var transmission = new StubTransmission();
                var result = await transmitter.FlushAsync(transmission, default);

                Assert.AreSame(transmission, sentTransmission);
                Assert.IsTrue(result);
            }

            [TestMethod]
            public async Task StoresTransmissionWhenSenderIsFull()
            {
                    OnEnqueue = getTransmission =>
                    {
                        isInStorage = true;
                        return true;
                    }
                };

                Transmitter transmitter = CreateTransmitter(sender: sender, buffer: buffer, storage: storage);

                var transmission = new StubTransmission();
                        if (transmission != null)
                        {
                            storedTransmission = transmission;
                            transmission.IsFlushAsyncInProgress = true;
                            return true;
                        }

                        return false;
                    }
                };
                var enqueuedTransmission = new StubTransmission();
                var result = await transmitter.FlushAsync(enqueuedTransmission, default);

                Assert.AreSame(enqueuedTransmission, storedTransmission);
                Assert.IsTrue(result);
            }

            [TestMethod]
            public async Task AppliesTransmistionPoliciesIfTheyNeverBeenAppliedBefore()
            {
                Transmitter transmitter = CreateTransmitter(sender: sender, policies: new[] { senderPolicy });

                var result = await transmitter.FlushAsync(new StubTransmission(), default);

                Assert.AreEqual(senderPolicy.MaxSenderCapacity, sender.Capacity);
                Assert.IsTrue(result);
            }

            [TestMethod]
            public async Task DoesNotApplyPolicesIfTheyAlreadyApplied()
            public async Task TracesDiagnosticsEvent()
            {
                Transmitter transmitter = CreateTransmitter();
                using (var listener = new TestEventListener())
                {
                    const long AllKeywords = -1;
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways, (EventKeywords)AllKeywords);

                    var result = await transmitter.FlushAsync(new StubTransmission(), default);

            [TestMethod]
            public async Task FlushAsyncReturnsFalseWhenTransmissionIsNotSentOrStored()
            {
                var sender = new StubTransmissionSender { OnEnqueue = t => false };
                var buffer = new StubTransmissionBuffer { OnEnqueue = t => false };
                var storage = new StubTransmissionStorage { OnEnqueue = t => false };
                Transmitter transmitter = CreateTransmitter(sender: sender, buffer: buffer, storage: storage);

                var enqueuedTransmission = new StubTransmission();
                var result = await transmitter.FlushAsync(enqueuedTransmission, default);
            }
        }

        [TestClass]
        public class MoveTransmissionsAndWaitForSender : TransmitterTest
        {
            [TestMethod]
            public async Task LogsEventAfterMovingTransmissionsToStorage()
            {
                var sender = new StubTransmissionSender { OnEnqueue = t => false };

                    transmitter.MoveTransmissionsAndWaitForSender(1, default);
                    trace = listener.Messages.First();
                    Assert.AreEqual(52, trace.EventId);
                }
            }

            [TestMethod]
            public async Task RespectsCancellationToken()
            {
                await Assert.ThrowsExceptionAsync<TaskCanceledException>(() => transmitter.MoveTransmissionsAndWaitForSender(new CancellationToken(true)));
                Assert.AreEqual(TaskStatus.Canceled, transmitter.MoveTransmissionsAndWaitForSender(1, new CancellationToken(true)));
            }
        }

        [TestClass]
        public class HandleSenderTransmissionSentEvent : TransmitterTest
        {
            [TestMethod]
            public void MovesOldestTransmissionFromBufferToSender()
            {
                Transmission sentTransmission = null;
                var sender = new StubTransmissionSender();
                sender.OnEnqueue = getTransmission =>
                {
                    sentTransmission = getTransmission();
                    return false;
                };

                Transmission bufferedTransmission = new StubTransmission();

                object eventSender = null;
                TransmissionProcessedEventArgs eventArgs = null;
                queue.TransmissionSent += (s, e) =>
                {
                    eventSender = s;
                    eventArgs = e;
                };

                var transmission = new StubTransmission();
                {
                    const long AllKeywords = -1;
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.Warning, (EventKeywords)AllKeywords);

                    sender.OnTransmissionSent(new TransmissionProcessedEventArgs(new StubTransmission()));

                    EventWrittenEventArgs message = listener.Messages.First();
                    AssertEx.Contains(exception.Message, (string)message.Payload[0], StringComparison.Ordinal);
                }
            }
                var previouslyStoredTransmissions = new List<Transmission> { new StubTransmission(), new StubTransmission() };
                int storageIndex = 0;
                var storage = new StubTransmissionStorage
                {
                    OnDequeue = () =>
                    {
                        if (storageIndex < previouslyStoredTransmissions.Count)
                        {
                            return previouslyStoredTransmissions[storageIndex++];
                        }
                { 
                    OnEnqueue = getTransmissionAsync => 
                    { 
                        throw exception; 
                    } 
                };

                Transmitter transmitter = CreateTransmitter(buffer: buffer);

                using (var listener = new TestEventListener())


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
