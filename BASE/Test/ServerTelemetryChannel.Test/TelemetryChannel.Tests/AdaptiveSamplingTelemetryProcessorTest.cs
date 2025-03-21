namespace Microsoft.ApplicationInsights.WindowsServer.Channel
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    [TestCategory("WindowsOnly")] // these tests are flaky on linux builds.
    public class AdaptiveSamplingTelemetryProcessorTest
    {
        [TestMethod]
        public void AllTelemetryCapturedWhenProductionRateIsLow()
        {
            var sentTelemetry = new List<ITelemetry>();
            int itemsProduced = 0;

            using (var tc = new TelemetryConfiguration() { TelemetryChannel = new StubTelemetryChannel() })
            {
                var chainBuilder = new TelemetryProcessorChainBuilder(tc);

                // set up adaptive sampling that evaluates and changes sampling % frequently
                chainBuilder
                    .UseAdaptiveSampling(
                        new Channel.Implementation.SamplingPercentageEstimatorSettings()
                        {
                            EvaluationInterval = TimeSpan.FromSeconds(1),
                            SamplingPercentageDecreaseTimeout = TimeSpan.FromSeconds(2),
                            SamplingPercentageIncreaseTimeout = TimeSpan.FromSeconds(2),
                        },
                        this.TraceSamplingPercentageEvaluation)
                    .Use((next) => new StubTelemetryProcessor(next) { OnProcess = (t) => sentTelemetry.Add(t) });

                chainBuilder.Build();

                const int productionFrequencyMs = 1000;

                var productionTimer = new Timer(
                    (state) =>
                    {
                        tc.TelemetryProcessorChain.Process(new RequestTelemetry());
                        itemsProduced++;
                    },
                    null,
                    productionFrequencyMs,
                    productionFrequencyMs);

                Thread.Sleep(25000);
                
                // dispose timer and wait for callbacks to complete
                DisposeTimer(productionTimer);
            }

            Assert.AreEqual(itemsProduced, sentTelemetry.Count);
        }
        {
            var testDurationSec = 30;
            var proactivelySampledInRatePerSec = 25;
            var targetProactiveCount = proactivelySampledInRatePerSec * testDurationSec;
            var precision = 0.2;
            var (proactivelySampledInAndSentCount, sentCount) = ProactiveSamplingTest(
                proactivelySampledInRatePerSec: proactivelySampledInRatePerSec,
                beforeSamplingRatePerSec: proactivelySampledInRatePerSec * 3,
                targetAfterSamplingRatePerSec: proactivelySampledInRatePerSec * 2,
                precision: precision,

            Trace.WriteLine($"'Ideal' proactively sampled in telemetry item count: {targetProactiveCount}");
            Trace.WriteLine($"Expected range: from {targetProactiveCount - precision * targetProactiveCount} to {targetProactiveCount + precision * targetProactiveCount}");
            Trace.WriteLine(
                $"Actual proactively sampled in  telemetry item count: {proactivelySampledInAndSentCount} ({100.0 * proactivelySampledInAndSentCount / targetProactiveCount:##.##}% of ideal)");

            // all proactively sampled in should be sent assuming we have perfect algo
            // as they happen with rate 5 items per sec and we want 10 rate of sent telemetry
            Assert.IsTrue(proactivelySampledInAndSentCount / (double)targetProactiveCount > 1 - precision,
                $"Expected {proactivelySampledInAndSentCount} to be between {targetProactiveCount} +/- {targetProactiveCount * precision}");
            var beforeSamplingRate = 42;
            var proactiveRate = beforeSamplingRate - 2;

            // for some reason, sampling on Linux test machines is not very stable
            // perhaps agents are not powerful or really virtual
            // there is nothing special in id generation or sampling on Linux
            // so we are blaming test infra
            double precision = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? 0.4 : 0.3;

            var (proactivelySampledInAndSentCount, sentCount) = ProactiveSamplingTest(
                beforeSamplingRatePerSec: beforeSamplingRate,
                targetAfterSamplingRatePerSec: proactiveRate / 2,
                precision: precision,
                testDurationInSec: testDuration); //plus warm up

            // most of of sent should be proactively sampled in 
            // as proactive happen with rate >> than target
            Trace.WriteLine($"'Ideal' proactively sampled in telemetry item count: {sentCount}");
            Trace.WriteLine(
                $"Expected range: from {sentCount - sentCount * precision} to {sentCount + sentCount * precision}");
            // we'll produce proactively  sampled in items and also 'normal' items with the same rate
            // but allow only proactively sampled in + a bit more

            // number of items produced should be close to target
            int targetItemCount = (int)(testDurationInSec * targetAfterSamplingRatePerSec);

            var sentTelemetry = new List<ITelemetry>();

            using (var tc = new TelemetryConfiguration() { TelemetryChannel = new StubTelemetryChannel() })
            {
                            SamplingPercentageDecreaseTimeout = TimeSpan.FromSeconds(4),
                            SamplingPercentageIncreaseTimeout = TimeSpan.FromSeconds(4),
                        },
                        this.TraceSamplingPercentageEvaluation)
                    .Use((next) => new StubTelemetryProcessor(next) { OnProcess = (t) => sentTelemetry.Add(t) });

                chainBuilder.Build();

                var sw = Stopwatch.StartNew();
                var productionTimer = new Timer(
                        var requests = new RequestTelemetry[beforeSamplingRatePerSec];

                        for (int i = 0; i < beforeSamplingRatePerSec; i++)
                        {
                            requests[i] = new RequestTelemetry()
                            {
                                ProactiveSamplingDecision = i < proactivelySampledInRatePerSec ? SamplingDecision.SampledIn : SamplingDecision.None
                            };

                            requests[i].Context.Operation.Id = ActivityTraceId.CreateRandom().ToHexString();
                        }

                        foreach (var request in requests)
                        {
                            if (((Stopwatch) state).Elapsed.TotalSeconds < warmUpInSec)
                            {
                                // let's ignore telemetry from first few rate evaluations - it does not make sense
                                request.Properties["ignore"] = "true";
                            }

                            tc.TelemetryProcessorChain.Process(request);
                        }
                    },
                    sw,
                    0,
                    1000);

                Thread.Sleep(TimeSpan.FromSeconds(testDurationInSec + warmUpInSec));

                // dispose timer and wait for callbacks to complete
            }

            var notIgnoredSent = sentTelemetry.Where(i => i is ISupportProperties propItem && !propItem.Properties.ContainsKey("ignore")).ToArray();

            var proactivelySampledInAndSentCount = notIgnoredSent.Count(i =>
                i is ISupportAdvancedSampling advSamplingItem &&
                advSamplingItem.ProactiveSamplingDecision == SamplingDecision.SampledIn);

            // check that normal sampling requirements still apply (we generated as much items as expected)
            Trace.WriteLine($"'Ideal' telemetry item count: {targetItemCount}");
                    (state) =>
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            tc.TelemetryProcessorChain.Process(new RequestTelemetry());
                            itemsProduced++;
                        }
                    },
                    null,
                    0,
                DisposeTimer(productionTimer);
            }

            // number of items produced should be close to target of 5/second
            int targetItemCount = 25 * 5;

            // tolrance +-
            int tolerance = targetItemCount / 2;

            Trace.WriteLine(string.Format("'Ideal' telemetry item count: {0}", targetItemCount));
                "Actual telemetry item count: {0} ({1:##.##}% of ideal)",
                sentTelemetry.Count,
                100.0 * sentTelemetry.Count / targetItemCount));

            Assert.IsTrue(sentTelemetry.Count > targetItemCount - tolerance);
            Assert.IsTrue(sentTelemetry.Count < targetItemCount + tolerance);
        }

        [TestMethod]
        public void SamplingPercentageAdjustsForSpikyProductionRate()
        {
            var sentTelemetry = new List<ITelemetry>();
            int itemsProduced = 0;

            using (var tc = new TelemetryConfiguration() { TelemetryChannel = new StubTelemetryChannel() })
            {
                var chainBuilder = new TelemetryProcessorChainBuilder(tc);

                // set up adaptive sampling that evaluates and changes sampling % frequently
                chainBuilder
                    },
                    null,
                    0,
                    regularProductionFrequencyMs);

                var spikeProductionTimer = new Timer(
                    (state) =>
                    {
                        for (int i = 0; i < 200; i++)
                        {
                Thread.Sleep(30000);

                // dispose timers and wait for callbacks to complete
                DisposeTimer(regularProductionTimer);
                DisposeTimer(spikeProductionTimer);
            }

            // number of items produced should be close to target of 5/second
            int targetItemCount = 30 * 5;
            int tolerance = targetItemCount / 2;
            public void Process(ITelemetry item)
            {
                if (item is RequestTelemetry req)
                {
                    requests.Enqueue(req);
                }
                else if (item is EventTelemetry evt)
                {
                    events.Enqueue(evt);
            var sentSample = sent as ISupportSampling;
            Assert.IsNotNull(sentSample);
            Assert.IsTrue(sentSample.SamplingPercentage.HasValue);
        }

        [TestMethod]
        public void SamplingSkipsSampledTelemetryItemProperty()
        {
            var unsampled = new AdaptiveTesterMessageSink();
            var sampled = new AdaptiveTesterMessageSink();

            Assert.IsTrue(unsampled.requests.Count == 1);
            Assert.IsTrue(sampled.requests.Count == 0);
        }

        [TestMethod]
        public void AdaptiveSamplingSetsExcludedTypesOnInternalSamplingProcessor()
        {
            var tc = new TelemetryConfiguration { TelemetryChannel = new StubTelemetryChannel() };
            var channelBuilder = new TelemetryProcessorChainBuilder(tc);

            var fieldInfo = typeof(AdaptiveSamplingTelemetryProcessor).GetField("samplingProcessor", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic);
            SamplingTelemetryProcessor internalProcessor = (SamplingTelemetryProcessor) fieldInfo.GetValue(tc.TelemetryProcessorChain.FirstTelemetryProcessor);

            Assert.AreEqual("request;", internalProcessor.ExcludedTypes);
        }

        [TestMethod]
        public void CurrentSamplingRateResetsOnInitialSamplingRateChange()
        {
        public void SettingsFromPassedInTelemetryProcessorsAreAppliedToSamplingTelemetryProcessor()
        {
            var nextMock = new Mock<ITelemetryProcessor>();
            var next = nextMock.Object;
            var adaptiveSamplingProcessor = new AdaptiveSamplingTelemetryProcessor(
                new Channel.Implementation.SamplingPercentageEstimatorSettings
                {
                    InitialSamplingPercentage = 25,
                },
                null,
        {
            // Regular Dispose() does not wait for all callbacks to complete
            // so TelemetryConfiguration could be disposed while callback still runs
            AutoResetEvent allDone = new AutoResetEvent(false);
            timer.Dispose(allDone);
            // this will wait for all callbacks to complete
            allDone.WaitOne();
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
