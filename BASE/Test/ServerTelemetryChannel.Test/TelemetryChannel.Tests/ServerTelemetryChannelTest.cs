namespace Microsoft.ApplicationInsights.WindowsServer.Channel
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    
    using Helpers;
    using System.Collections.Generic;
    using Extensibility.Implementation;

    using System.Diagnostics.Tracing;
    using TaskEx = System.Threading.Tasks.Task;

    public class ServerTelemetryChannelTest : IDisposable
    {
        private readonly TelemetryConfiguration configuration;

        public ServerTelemetryChannelTest()
        {
            this.configuration = new TelemetryConfiguration();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.configuration.Dispose();            
        }

        [TestClass]
        public class Constructor : ServerTelemetryChannelTest
        {
            [TestMethod]
            public void InitializesTransmitterWithNetworkAvailabilityPolicy()
            {
                var network = new StubNetwork { OnIsAvailable = () => false };

                var channel = new ServerTelemetryChannel(network, new StubApplicationLifecycle());
                channel.Initialize(new TelemetryConfiguration());
                Thread.Sleep(50);

                Assert.AreEqual(0, channel.Transmitter.Sender.Capacity);
            }
        }

        [TestClass]
        public class DefaultBackoffEnabledReportingInterval
        {
            [TestMethod]
            public void DefaultBackoffEnabledReportingIntervalUpdatesBackoffLogicManager()
            {
                ServerTelemetryChannel channel = new ServerTelemetryChannel
                {
                    DefaultBackoffEnabledReportingInterval = TimeSpan.FromHours(42)
                };

                Assert.AreEqual(channel.Transmitter.BackoffLogicManager.DefaultBackoffEnabledReportingInterval, TimeSpan.FromHours(42));
            }
        }

        [TestClass]
            [TestMethod]
            public void WhenSetToTrueChangesTelemetryBufferCapacityToOneForImmediateTransmission()
            {
                var channel = new ServerTelemetryChannel();
                channel.DeveloperMode = true;
                Assert.AreEqual(1, channel.TelemetryBuffer.Capacity);
            }

            [TestMethod]
            public void WhenSetToFalseChangesTelemetryBufferCapacityToOriginalValueForBufferedTransmission()
            public void EndpointAddressIsStoredBySerializer()
            {
                var channel = new ServerTelemetryChannel();
                
                Uri expectedEndpoint = new Uri("http://abc.com");
                channel.EndpointAddress = expectedEndpoint.AbsoluteUri;
                
                Assert.AreEqual(expectedEndpoint, channel.TelemetrySerializer.EndpointAddress);
            }
        }
            public void GetterReturnsTelemetryBufferCapacity()
            {
                var channel = new ServerTelemetryChannel();
                channel.TelemetryBuffer.Capacity = 42;
                Assert.AreEqual(42, channel.MaxTelemetryBufferCapacity);
            }

            [TestMethod]
            public void SetterChangesTelemetryBufferCapacity()
            {
                var channel = new ServerTelemetryChannel();
                channel.MaxTelemetryBufferCapacity = 42;
                Assert.AreEqual(42, channel.TelemetryBuffer.Capacity);
            }
        }

        [TestClass]
        public class MaxTransmissionBufferCapacity : ServerTelemetryChannelTest
        {
            [TestMethod]
            }

            [TestMethod]
            public void ChangesMaxBufferCapacityOfTransmitter()
            {
                var channel = new ServerTelemetryChannel { Transmitter = new StubTransmitter() };
                channel.MaxTransmissionBufferCapacity = 42;
                Assert.AreEqual(42, channel.Transmitter.MaxBufferCapacity);
            }
        }

        [TestClass]
        public class MaxTransmissionSenderCapacity : ServerTelemetryChannelTest
        {
            [TestMethod]
            public void ReturnsMaxSenderCapacityOfTransmitter()
            {
                var channel = new ServerTelemetryChannel { Transmitter = new StubTransmitter() };
                channel.Transmitter.MaxSenderCapacity = 42;
                Assert.AreEqual(42, channel.MaxTransmissionSenderCapacity);
        }

        [TestClass]
        public class MaxTransmissionStorageCapacity : ServerTelemetryChannelTest
        {
            [TestMethod]
            public void GetterReturnsMaxStorageCapacityOfTransmitter()
            {
                var channel = new ServerTelemetryChannel { Transmitter = new StubTransmitter() };
                channel.Transmitter.MaxStorageCapacity = 42000;
            }

            [TestMethod]
            public void SetterChangesStorageCapacityOfTransmitter()
            {
                var channel = new ServerTelemetryChannel { Transmitter = new StubTransmitter() };
                channel.MaxTransmissionStorageCapacity = 42000;
                Assert.AreEqual(42000, channel.Transmitter.MaxStorageCapacity);
            }
        }

        [TestClass]
        public class StorageFolder : ServerTelemetryChannelTest
        {
            [TestMethod]
            public void GetterReturnsStorageFolderOfTransmitter()
            {
                var channel = new ServerTelemetryChannel { Transmitter = new StubTransmitter() };
                channel.Transmitter.StorageFolder = "test";
                Assert.AreEqual("test", channel.StorageFolder);

        [TestClass]
        public class Flush : ServerTelemetryChannelTest
        {
            [TestMethod]
            public void FlushesTelemetryBuffer()
            {
                var mockTelemetryBuffer = new Mock<TelemetryChannel.Implementation.TelemetryBuffer>();
                var channel = new ServerTelemetryChannel { TelemetryBuffer = mockTelemetryBuffer.Object };
                channel.Initialize(TelemetryConfiguration.CreateDefault());
                var expectedException = new Exception();
                var tcs = new TaskCompletionSource<object>();
                tcs.SetException(expectedException);
                var mockTelemetryBuffer = new Mock<TelemetryChannel.Implementation.TelemetryBuffer>();
                mockTelemetryBuffer.Setup(x => x.FlushAsync()).Returns(tcs.Task);
                var channel = new ServerTelemetryChannel { TelemetryBuffer = mockTelemetryBuffer.Object };
                channel.Initialize(TelemetryConfiguration.CreateDefault());

                var actualException = AssertEx.Throws<Exception>(() => channel.Flush());

                var initializedConfiguration = new TelemetryConfiguration();
                channel.Initialize(initializedConfiguration);

                Assert.IsTrue(transmissionPoliciesApplied.WaitOne(1000));
            }

            [TestMethod]
            public void InitializeCallsTransmitterInitialize()
            {
                var transmitterInitialized = new ManualResetEvent(false);
        }

        [TestClass]
        public class Send : ServerTelemetryChannelTest
        {
            [TestMethod]
            public void PassesTelemetryToTelemetryProcessor()
            {
                ITelemetry sentTelemetry = null;
                var channel = new ServerTelemetryChannel();
            [TestMethod]
            public void DropsTelemetryWithNoInstrumentationKey()
            {
                ITelemetry sentTelemetry = null;
                var channel = new ServerTelemetryChannel();
                channel.Initialize(TelemetryConfiguration.CreateDefault());
                channel.TelemetryProcessor = new StubTelemetryProcessor(null) { OnProcess = (t) => sentTelemetry = t };

                var telemetry = new StubTelemetry();
                // No instrumentation key
                    var expectedMessage = listener.Messages.First();
                    Assert.AreEqual(67, expectedMessage.EventId);
                }
            }
        }

        [TestClass]
        public class InternalOperation : ServerTelemetryChannelTest
        {
            class TransmissionStubChecksInternalOperation : Transmission
            {
                public Action<bool> WasCalled;

                public override Task<HttpWebResponseWrapper> SendAsync()
                {
                    Assert.IsTrue(SdkInternalOperationsMonitor.IsEntered());
                    this.WasCalled(true);
                    return base.SendAsync();
                }
            }

            class TelemetrySerializerStub : TelemetrySerializer
            {
                public Action<bool> WasCalled;

                public TelemetrySerializerStub(Transmitter t) : base(t)
                {
                }

                public override void Serialize(ICollection<ITelemetry> items)
                {
                    var transmission = new TransmissionStubChecksInternalOperation();
                    transmission.WasCalled = this.WasCalled;
                    base.Transmitter.Enqueue(transmission);
                }
            }

            [TestMethod]
            public void SendWillBeMarkedAsInternalOperation()
            {
                bool wasCalled = false;
                var channel = new ServerTelemetryChannel();
                channel.TelemetrySerializer = new TelemetrySerializerStub(channel.Transmitter) { WasCalled = (called) => { wasCalled = called; } };
#if NETCOREAPP
                channel.TelemetryBuffer = new TelemetryChannel.Implementation.TelemetryBuffer(channel.TelemetrySerializer, null);
#else
                channel.TelemetryBuffer = new TelemetryChannel.Implementation.TelemetryBuffer(channel.TelemetrySerializer, new WebApplicationLifecycle());
#endif
                channel.TelemetryProcessor = channel.TelemetryBuffer;
                channel.MaxTelemetryBufferCapacity = 1;
                channel.Initialize(TelemetryConfiguration.CreateDefault());

                var telemetry = new StubTelemetry();
                telemetry.Context.InstrumentationKey = Guid.NewGuid().ToString();
                channel.Send(telemetry);
                Thread.Sleep(TimeSpan.FromSeconds(1));

                var channel = new ServerTelemetryChannel();
                Assert.AreEqual(null, channel.EndpointAddress);

                var configuration = new TelemetryConfiguration
                {
                    TelemetryChannel = channel,
                };

                Assert.AreEqual("https://dc.services.visualstudio.com/", configuration.EndpointContainer.Ingestion.AbsoluteUri);
                Assert.AreEqual("https://dc.services.visualstudio.com/v2/track", channel.EndpointAddress);
            {
                var connectionstring = $"instrumentationkey=00000000-0000-0000-0000-000000000000";

                var channel = new ServerTelemetryChannel();

                var configuration = new TelemetryConfiguration
                {
                    TelemetryChannel = channel,
                    ConnectionString = connectionstring,
                };
            }

            [TestMethod]
            [TestCategory("ConnectionString")]
            public void VerifyEndpointConnectionString_SetFromConfiguration_ExplicitEndpoint_WithTrailingSlash()
            {
                var explicitEndpoint = "https://127.0.0.1/";
                var connectionString = $"InstrumentationKey=00000000-0000-0000-0000-000000000000;IngestionEndpoint={explicitEndpoint}";

                var channel = new ServerTelemetryChannel();
                var channel = new ServerTelemetryChannel();

                var configuration = new TelemetryConfiguration
                {
                    TelemetryChannel = channel,
                    ConnectionString = connectionString,
                };

                Assert.AreEqual("https://127.0.0.1/", configuration.EndpointContainer.Ingestion.AbsoluteUri);
                Assert.AreEqual("https://127.0.0.1/v2/track", channel.EndpointAddress);
                var configuration = new TelemetryConfiguration
                {
                    ConnectionString = connectionString,
                };

                Assert.AreEqual("https://127.0.0.1/", configuration.EndpointContainer.Ingestion.AbsoluteUri);

                var channel = new ServerTelemetryChannel();
                channel.Initialize(configuration);
                Assert.AreEqual("https://127.0.0.1/v2/track", channel.EndpointAddress);
            }


            [TestMethod]
            [TestCategory("ConnectionString")]
            public void VerifyEndpointConnectionString_SetFromInitialize_ExplicitEndpoint_WithoutTrailingSlash()
            {
                var explicitEndpoint = "https://127.0.0.1";
                var connectionString = $"InstrumentationKey=00000000-0000-0000-0000-000000000000;IngestionEndpoint={explicitEndpoint}";

                var configuration = new TelemetryConfiguration
                {
                    ConnectionString = connectionString,
                };

                Assert.AreEqual("https://127.0.0.1/", configuration.EndpointContainer.Ingestion.AbsoluteUri);

                var channel = new ServerTelemetryChannel();
                channel.Initialize(configuration);
                Assert.AreEqual("https://127.0.0.1/v2/track", channel.EndpointAddress);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
