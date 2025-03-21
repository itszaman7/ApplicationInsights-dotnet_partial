namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    using ITelemetry = Microsoft.ApplicationInsights.Channel.ITelemetry;
    using Channel.Helpers;

    public class TelemetryBufferTest
    {
        [TestClass]
        public class Class : TelemetryBufferTest
        {
            [TestMethod]
            public void ImplementsIEnumerableToAllowInspectingBufferContentsInTests()
            {
                TelemetryBuffer instance = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());
                Assert.IsTrue(instance is IEnumerable<ITelemetry>);
            }

            [TestMethod]
            public void ConstructorThrowsArgumentNullExceptionWhenSerializerIsNull()
            {
                AssertEx.Throws<ArgumentNullException>(() => new TelemetryBuffer(null, new StubApplicationLifecycle()));
            }

#if NETFRAMEWORK
            [TestMethod]
            public void ConstructorThrowsArgumentNullExceptionWhenApplicationLifecycleIsNull()
            {
                AssertEx.Throws<ArgumentNullException>(() => new TelemetryBuffer(new StubTelemetrySerializer(), null));
            }
#endif
        }

        [TestClass]
        public class MaxTransmissionDelay
        {
            [TestMethod]
            public void DefaultValueIsAppropriateForProductionEnvironmentAndUnitTests()
            {
                var buffer = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());
                Assert.AreEqual(TimeSpan.FromSeconds(30), buffer.MaxTransmissionDelay);
            }

            [TestMethod]
            public void CanBeChangedByChannelToTunePerformance()
            {
                var buffer = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());

                var expectedValue = TimeSpan.FromSeconds(42);
                buffer.MaxTransmissionDelay = expectedValue;

                Assert.AreEqual(expectedValue, buffer.MaxTransmissionDelay);
            }
        }
                var buffer = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());
                buffer.Capacity = 42;
                buffer.BacklogSize = 3900;
                Assert.AreEqual(42, buffer.Capacity);
                Assert.AreEqual(3900, buffer.BacklogSize);
            }

            [TestMethod]
            public void ThrowsArgumentExceptionWhenBacklogSizeIsLowerThanCapacity()
            {
                var buffer = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());
                buffer.Capacity = 1111;
                AssertEx.Throws<ArgumentException>(() => buffer.BacklogSize = 1110);

                buffer.BacklogSize = 8000;
                AssertEx.Throws<ArgumentException>(() => buffer.Capacity = 8001);

            }

            [TestMethod]
            public void ThrowsArgumentOutOfRangeExceptionWhenNewValueIsLessThanMinimum()
            {
                var buffer = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());
                AssertEx.Throws<ArgumentOutOfRangeException>(() => buffer.Capacity = 0);
                AssertEx.Throws<ArgumentOutOfRangeException>(() => buffer.BacklogSize = 0);
                AssertEx.Throws<ArgumentOutOfRangeException>(() => buffer.BacklogSize = 1000); // 1001 is minimum anything low would throw.

                bool exceptionThrown = false;
                try
                {
                Assert.IsTrue(exceptionThrown == false, "No exception should be thrown when trying to set backlog size to 1001");                
            }
        }

        // TODO: Test that TelemetryBuffer.Send synchronously clears the buffer to prevent item # 501 from flushing again
        [TestClass]
        [TestCategory("WindowsOnly")] // these tests are flaky on linux builds.
        public class Send : TelemetryBufferTest
        {
            [TestMethod]
            public void ThrowsArgumentNullExceptionWhenTelemetryIsNull()
            {
                var buffer = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());
                AssertEx.Throws<ArgumentNullException>(() => buffer.Process((ITelemetry)null));
            }

            [TestMethod]
            public void AddsTelemetryToBufferUntilItReachesMax()
            {
                var buffer = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());
                buffer.Capacity = 42;

                buffer.Process(new StubTelemetry());

                Assert.AreEqual(1, buffer.Count());
            }

            [TestMethod]
            public void TelemetryBufferDoesNotGrowBeyondMaxBacklogSize()
            {
                // validate that items are not added after maxunsentbacklogsize is reached.
                // this also validate that items can still be added after Capacity is reached as it is only a soft limit.
                int bufferItemCount = buffer.Count();
                Assert.AreEqual(1002, bufferItemCount);
            }

            [TestMethod]
            
            public void FlushesBufferWhenNumberOfTelemetryItemsReachesMax()
            {
                var serializer = new StubTelemetrySerializer
                {
                    OnSerialize = telemetry =>
                    {
                        flushedTelemetry = telemetry.ToList();
                        bufferFlushed.Set();
                    },
                };

                var telemetryBuffer = new TelemetryBuffer(serializer, new StubApplicationLifecycle());

                Assert.IsTrue(anotherThread.Wait(50));
            }

            [TestMethod]
            public void StartsTimerThatFlushesBufferAfterMaxTransmissionDelay()
            {
                var telemetrySerialized = new ManualResetEventSlim();
                var serializer = new StubTelemetrySerializer();
                serializer.OnSerialize = telemetry => 
                { 
                    telemetrySerialized.Set();
                };

                var buffer = new TelemetryBuffer(serializer, new StubApplicationLifecycle());

                buffer.MaxTransmissionDelay = TimeSpan.FromMilliseconds(1);
                buffer.Process(new StubTelemetry());

                Assert.IsTrue(telemetrySerialized.Wait(1000));
            public void DoesNotCancelPreviousFlush()
            {
                var telemetrySerialized = new ManualResetEventSlim();
                var serializer = new StubTelemetrySerializer();
                serializer.OnSerialize = telemetry =>
                {
                    telemetrySerialized.Set();
                };
                var buffer = new TelemetryBuffer(serializer, new StubApplicationLifecycle());

                serializer.OnSerialize = telemetry => 
                {
                    serializedTelemetry = new List<ITelemetry>(telemetry);
                };

                var telemetryBuffer = new TelemetryBuffer(serializer, new StubApplicationLifecycle());
        
                var expectedTelemetry = new StubTelemetry();
                telemetryBuffer.Process(expectedTelemetry);
        
            }

            [TestMethod]
            public void SerializesBufferOnThreadPoolToFreeUpCustomersThread()
            {
                int serializerThreadId = -1;
                var serializerInvoked = new ManualResetEventSlim();
                var serializer = new StubTelemetrySerializer();
                serializer.OnSerialize = telemetry =>
                {
            }

            [TestMethod]
            [Timeout(10000)]
            public void EmptiesBufferAfterSerialization()
            {
                var buffer = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());
                buffer.Capacity = 10;
                buffer.Process(new StubTelemetry());


                Task anotherThread;
                lock (telemetryBuffer)
                {
                    anotherThread = Task.Run(() => telemetryBuffer.FlushAsync());
                    Assert.IsFalse(anotherThread.Wait(10));
                }

                Assert.IsTrue(anotherThread.Wait(50));
            }
            }

            [TestMethod]
            [Timeout(10000)]
            public void DoesNotContinueOnCapturedSynchronizationContextToImprovePerformance()
            {
                var buffer = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());
                buffer.Process(new StubTelemetry());
        
                bool postedBack = false;
                using (var context = new StubSynchronizationContext())
                {
                    context.OnPost = (callback, state) =>
                    {
                        postedBack = true;
                        callback(state);
                    };

                    buffer.FlushAsync().GetAwaiter().GetResult();
        
        }

        [TestClass]
        public class HandleApplicationStoppingEvent : TelemetryBufferTest
        {
            [TestMethod]
            public void FlushesBufferToPreventLossOfTelemetry()
            {
                var applicationLifecycle = new StubApplicationLifecycle();
                var telemetrySerialized = new ManualResetEventSlim();
                var serializer = new StubTelemetrySerializer();
                serializer.OnSerialize = telemetry => 
                {
                    telemetrySerialized.Set();
                };
                var buffer = new TelemetryBuffer(serializer, applicationLifecycle);
                buffer.Process(new StubTelemetry());

                applicationLifecycle.OnStopping(ApplicationStoppingEventArgs.Empty);


            [TestMethod]
            public void PreventsOperatingSystemFromSuspendingAsynchronousOperations()
            {
                var applicationLifecycle = new StubApplicationLifecycle();
                var buffer = new TelemetryBuffer(new StubTelemetrySerializer(), applicationLifecycle);
                buffer.Process(new StubTelemetry());

                bool deferralAcquired = false;
                Func<Func<Task>, Task> asyncTaskRunner = asyncMethod =>
            {
                List<ITelemetry> serializedTelemetry = null;

                var serializer = new StubTelemetrySerializer
                {
                    OnSerializeAsync = (telemetry, cancellationToken) =>
                    {
                        serializedTelemetry = new List<ITelemetry>(telemetry);
                        return Task.FromResult(true);
                    }
                };

                var telemetryBuffer = new TelemetryBuffer(serializer, new StubApplicationLifecycle());

                var expectedTelemetry = new StubTelemetry();
                telemetryBuffer.Process(expectedTelemetry);

                var taskResult = await telemetryBuffer.FlushAsync(default);

                Assert.AreSame(expectedTelemetry, serializedTelemetry.Single());
            public async Task EmptiesBufferAfterSerialization()
            {
                var serializer = new StubTelemetrySerializer
                {
                    OnSerializeAsync = (telemetry, cancellationToken) =>
                    {
                        return Task.FromResult(true);
                    }
                };


                bool postedBack = false;
                using (var context = new StubSynchronizationContext())
                {
                    context.OnPost = (callback, state) =>
                    {
                        postedBack = true;
                        callback(state);
                    };

                    Assert.IsFalse(postedBack);
                    Assert.IsTrue(taskResult);
                }
            }

            [TestMethod]
            public async Task BufferFlushAsyncTaskRespectCancellationToken()
            {
                var telemetryBuffer = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());
                await Assert.ThrowsExceptionAsync<TaskCanceledException>(() => telemetryBuffer.FlushAsync(new CancellationToken(true)));


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
