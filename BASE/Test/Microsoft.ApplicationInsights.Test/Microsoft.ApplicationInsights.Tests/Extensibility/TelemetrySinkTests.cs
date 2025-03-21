namespace Microsoft.ApplicationInsights.Extensibility
{
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class TelemetrySinkTests
    {
        [TestMethod]
        public void CommonTelemetryProcessorsAreInvoked()
        {
            var configuration = new TelemetryConfiguration();

            var sentTelemetry = new List<ITelemetry>(1);
            var channel = new StubTelemetryChannel();
            channel.OnSend = (telemetry) => sentTelemetry.Add(telemetry);
            configuration.TelemetryChannel = channel;

            var chainBuilder = new TelemetryProcessorChainBuilder(configuration);
            configuration.TelemetryProcessorChainBuilder = chainBuilder;
            chainBuilder.Use((next) =>
            {
                var first = new StubTelemetryProcessor(next);
                first.OnProcess = (telemetry) => telemetry.Context.GlobalProperties.Add("SeenByFirst", "true");
                return first;
            });
            chainBuilder.Use((next) =>
            {
                var second = new StubTelemetryProcessor(next);
                second.OnProcess = (telemetry) => telemetry.Context.GlobalProperties.Add("SeenBySecond", "true");
                return second;
            });

            var client = new TelemetryClient(configuration);
            client.TrackTrace("t1");

            Assert.AreEqual(1, sentTelemetry.Count);
            Assert.IsTrue(sentTelemetry[0].Context.GlobalProperties.ContainsKey("SeenByFirst"));
            Assert.IsTrue(sentTelemetry[0].Context.GlobalProperties.ContainsKey("SeenBySecond"));
        }

        [TestMethod]
        public void SinkProcessorsAreInvoked()
        {
            var configuration = new TelemetryConfiguration();

            var sentTelemetry = new List<ITelemetry>(1);
            var channel = new StubTelemetryChannel();
            channel.OnSend = (telemetry) => sentTelemetry.Add(telemetry);
            configuration.TelemetryChannel = channel;

            var chainBuilder = new TelemetryProcessorChainBuilder(configuration, configuration.DefaultTelemetrySink);
            configuration.DefaultTelemetrySink.TelemetryProcessorChainBuilder = chainBuilder;
            chainBuilder.Use((next) =>
            {
                var first = new StubTelemetryProcessor(next);
                first.OnProcess = (telemetry) => telemetry.Context.GlobalProperties.Add("SeenByFirst", "true");
                return first;
            });
            chainBuilder.Use((next) =>
            {
                var second = new StubTelemetryProcessor(next);
                second.OnProcess = (telemetry) => telemetry.Context.GlobalProperties.Add("SeenBySecond", "true");
                return second;
            });

            var client = new TelemetryClient(configuration);
            client.TrackTrace("t1");

            Assert.IsFalse(configuration.TelemetryProcessors.OfType<StubTelemetryProcessor>().Any()); // Both processors belong to the sink, not to the common chain.
            Assert.AreEqual(1, sentTelemetry.Count);
            Assert.IsTrue(sentTelemetry[0].Context.GlobalProperties.ContainsKey("SeenByFirst"));
            Assert.IsTrue(sentTelemetry[0].Context.GlobalProperties.ContainsKey("SeenBySecond"));
        }

        [TestMethod]
        public void CommonAndSinkProcessorsAreInvoked()
        {
            var configuration = new TelemetryConfiguration();
            client.TrackTrace("t1");

            Assert.AreEqual(1, sentTelemetry.Count);
            Assert.IsTrue(sentTelemetry[0].Context.GlobalProperties.ContainsKey("SeenByCommonProcessor"));
            Assert.IsTrue(sentTelemetry[0].Context.GlobalProperties.ContainsKey("SeenBySinkProcessor"));
        }

        [TestMethod]
        public void ReplacingTelemetryChannelOnConfiguraitonReplacesItForDefaultSink()
        {
            var configuration = new TelemetryConfiguration();

            var firstSentTelemetry = new List<ITelemetry>(1);
            var firstChannel = new StubTelemetryChannel();
            firstChannel.OnSend = (telemetry) => firstSentTelemetry.Add(telemetry);
            configuration.TelemetryChannel = firstChannel;

            var client = new TelemetryClient(configuration);
            client.TrackTrace("t1");


            client.TrackTrace("t1");

            Assert.AreEqual(1, firstSentTelemetry.Count);
            Assert.AreEqual(1, secondSentTelemetry.Count);
        }

        [TestMethod]
        public void TelemetryIsDeliveredToMultipleSinks()
        {
            var chainBuilder = new TelemetryProcessorChainBuilder(configuration);
            configuration.TelemetryProcessorChainBuilder = chainBuilder;

            var secondChannelTelemetry = new List<ITelemetry>();
            var secondChannel = new StubTelemetryChannel();
            secondChannel.OnSend = (telemetry) => secondChannelTelemetry.Add(telemetry);
            var secondSink = new TelemetrySink(configuration, secondChannel);
            configuration.TelemetrySinks.Add(secondSink);

            var thirdChannelTelemetry = new List<ITelemetry>();
        [TestMethod]
        public void MultipleSinkTelemetryProcessorsAreInvoked()
        {
            var configuration = new TelemetryConfiguration();

            var commonChainBuilder = new TelemetryProcessorChainBuilder(configuration);
            configuration.TelemetryProcessorChainBuilder = commonChainBuilder;
            commonChainBuilder.Use((next) =>
            {
                var commonProcessor = new StubTelemetryProcessor(next);
                commonProcessor.OnProcess = (telemetry) => telemetry.Context.GlobalProperties.Add("SeenByCommonProcessor", "true");
                return commonProcessor;
            });

            var firstChannelTelemetry = new List<ITelemetry>();
            var firstChannel = new StubTelemetryChannel();
            firstChannel.OnSend = (telemetry) => firstChannelTelemetry.Add(telemetry);
            configuration.DefaultTelemetrySink.TelemetryChannel = firstChannel;

            var firstSinkChainBuilder = new TelemetryProcessorChainBuilder(configuration, configuration.DefaultTelemetrySink);
            bool firstSinkTelemetryProcessorDisposed = false;
            firstSinkChainBuilder.Use((next) =>
            {
                var firstSinkTelemetryProcessor = new StubTelemetryProcessor(next);
                firstSinkTelemetryProcessor.OnDispose = () => firstSinkTelemetryProcessorDisposed = true;
                return firstSinkTelemetryProcessor;
            });
            configuration.DefaultTelemetrySink.TelemetryProcessorChainBuilder = firstSinkChainBuilder;

            var secondChannel = new StubTelemetryChannel();
            var secondSink = new TelemetrySink(configuration, secondChannel);
            var secondSinkChainBuilder = new TelemetryProcessorChainBuilder(configuration, secondSink);
            bool secondSinkTelemetryProcessorDisposed = false;
            secondSinkChainBuilder.Use((next) =>
            {
                var secondSinkTelemetryProcessor = new StubTelemetryProcessor(next);
                secondSinkTelemetryProcessor.OnDispose = () => secondSinkTelemetryProcessorDisposed = true;
                return secondSinkTelemetryProcessor;
            });
            secondSink.TelemetryProcessorChainBuilder = secondSinkChainBuilder;
        }

        /// <summary>
        /// Ensures that all the sinks get the full copy of the telemetry context.
        /// This is a test to ensure DeepClone is copying over all the properties.
        /// </summary>
        [TestMethod]
        public void EnsureAllSinksGetFullTelemetryContext()
        {
            var configuration = new TelemetryConfiguration();
            var commonChainBuilder = new TelemetryProcessorChainBuilder(configuration);
            configuration.TelemetryProcessorChainBuilder = commonChainBuilder;

            ITelemetryChannel secondTelemetryChannel = new StubTelemetryChannel
            {
                OnSend = telemetry =>
                {
                    Assert.AreEqual("UnitTest", telemetry.Context.Cloud.RoleName);
                    Assert.AreEqual("TestVersion", telemetry.Context.Component.Version);
                    Assert.AreEqual("TestDeviceId", telemetry.Context.Device.Id);
                    Assert.AreEqual("userId", telemetry.Context.User.Id);
                    Assert.AreEqual("OpId", telemetry.Context.Operation.Id);
                }
            };

            configuration.TelemetrySinks.Add(new TelemetrySink(configuration, secondTelemetryChannel));
            configuration.TelemetryProcessorChainBuilder.Build();

            TelemetryClient telemetryClient = new TelemetryClient(configuration);

        }

        /// <summary>
        /// Ensures all telemetry sinks get the similar (objects with same values filled in them) telemetry items.
        /// </summary>
        [TestMethod]
        public void EnsureAllTelemetrySinkItemsAreSimilarAcrossSinks()
        {
            var configuration = new TelemetryConfiguration();
            var commonChainBuilder = new TelemetryProcessorChainBuilder(configuration);
            configuration.TelemetryProcessorChainBuilder = commonChainBuilder;

            string jsonFromFirstChannel = null;
            string jsonFromSecondChannel = null;

            ITelemetryChannel firstTelemetryChannel = new StubTelemetryChannel
            {
                OnSend = telemetry =>
                {
                    jsonFromFirstChannel = JsonConvert.SerializeObject(telemetry);
                }
            };

            ITelemetryChannel secondTelemetryChannel = new StubTelemetryChannel
            {
                OnSend = telemetry =>
                {
                    jsonFromSecondChannel = JsonConvert.SerializeObject(telemetry);
                }
            };
            configuration.DefaultTelemetrySink.TelemetryChannel = firstTelemetryChannel;
            configuration.TelemetrySinks.Add(new TelemetrySink(configuration, secondTelemetryChannel));

            configuration.TelemetryProcessorChainBuilder.Build();

            TelemetryClient telemetryClient = new TelemetryClient(configuration);

            // Setup TelemetryContext in a way that it is filledup.
            telemetryClient.Context.Operation.Id = "OpId";
            telemetryClient.Context.Cloud.RoleName = "UnitTest";
                });
            Assert.AreEqual(jsonFromFirstChannel, jsonFromSecondChannel);

            telemetryClient.TrackTrace(
                "Message",
                SeverityLevel.Critical,
                new Dictionary<string, string>() { { "Key", "Value" } });
            Assert.AreEqual(jsonFromFirstChannel, jsonFromSecondChannel);
        }

        /// <summary>
        /// Ensure broadcast processor does not drop telemetry items.
        /// </summary>
        [TestMethod]
        public void EnsureEventsAreNotDroppedByBroadcastProcessor()
        {
            var configuration = new TelemetryConfiguration();
            var commonChainBuilder = new TelemetryProcessorChainBuilder(configuration);
            configuration.TelemetryProcessorChainBuilder = commonChainBuilder;

                OnSend = telemetry =>
                {
                    itemsReceivedBySink1.Add(telemetry);
                }
            };

            ITelemetryChannel secondTelemetryChannel = new StubTelemetryChannel
            {
                OnSend = telemetry =>
                {
                    itemsReceivedBySink2.Add(telemetry);
                }
            };

            configuration.DefaultTelemetrySink.TelemetryChannel = firstTelemetryChannel;
            configuration.TelemetrySinks.Add(new TelemetrySink(configuration, secondTelemetryChannel));

            configuration.TelemetryProcessorChainBuilder.Build();

            TelemetryClient telemetryClient = new TelemetryClient(configuration);
            telemetryClient.Context.Device.Id = "TestDeviceId";
            telemetryClient.Context.Flags = 1234;
            telemetryClient.Context.InstrumentationKey = Guid.Empty.ToString();
            telemetryClient.Context.Location.Ip = "127.0.0.1";
            telemetryClient.Context.Session.Id = "SessionId";
            telemetryClient.Context.User.Id = "userId";

            Parallel.ForEach(
                new int[100],
                new ParallelOptions
                        "Local",
                        true,
                        "Message",
                        new Dictionary<string, string>() { { "Key", "Value" } },
                        new Dictionary<string, double>() { { "Dimension1", 0.9865 } });

                        "HTTP",
                        "Target",
                        "Test",
                        "https://azure",
                        DateTimeOffset.Now,
                        TimeSpan.FromMilliseconds(100),
                        "200",
                        true);

                    telemetryClient.TrackEvent(


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
