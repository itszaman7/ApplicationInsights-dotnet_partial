using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Microsoft.ApplicationInsights.Metrics.Extensibility;
using Microsoft.ApplicationInsights.Metrics.TestUtility;
using System.Threading;

namespace Microsoft.ApplicationInsights.Metrics
{
    /// <summary />
    [TestClass]
    public class MetricSeriesTests
    {
        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void Properties()
        {
            var manager = new MetricManager(new MemoryMetricTelemetryPipeline());
            IMetricSeriesConfiguration config = new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false);
            MetricSeries series = manager.CreateNewSeries("namespace", "Foo Bar", config);

            Assert.AreEqual("namespace", series.MetricIdentifier.MetricNamespace);
            Assert.AreEqual("Foo Bar", series.MetricIdentifier.MetricId);

            Assert.AreEqual(config, series.GetConfiguration());
            Assert.AreSame(config, series.GetConfiguration());

            TestUtil.CompleteDefaultAggregationCycle(manager);
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void TrackValueDouble()
        {
            DateTimeOffset startTS = DateTimeOffset.Now;

            var aggregateCollector = new MemoryMetricTelemetryPipeline();
            var manager = new MetricManager(aggregateCollector);
            IMetricSeriesConfiguration config = new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false);
            MetricSeries series = manager.CreateNewSeries("ns", "Foo Bar", config);

            Thread.Sleep(1500);

            series.TrackValue(0.4);
            series.TrackValue(0.8);
            series.TrackValue(-0.04);

            Assert.AreEqual(0, aggregateCollector.Count);
            manager.Flush();
            Assert.AreEqual(1, aggregateCollector.Count);

            DateTimeOffset endTSRounded = DateTimeOffset.Now;
            endTSRounded = new DateTimeOffset(endTSRounded.Year, endTSRounded.Month, endTSRounded.Day, endTSRounded.Hour, endTSRounded.Minute, endTSRounded.Second, 0, endTSRounded.Offset);

            TestUtil.ValidateNumericAggregateValues(aggregateCollector[0], ns: "ns", name: "Foo Bar", count: 3, sum: 1.16, max: 0.8, min: -0.04, stdDev: 0.343058142140496, aggKindMoniker: "Microsoft.Azure.Measurement");

            // Timestamp checks have to be approximate, since we have no possibilityt to get exact timetamps snapped internally.

            // The following might break sometimes!
            // There is a little chance that second boundary is crossed between startTS and the aggregation timestamps are snapped.
            // rerun the test if it happens.

            Assert.AreEqual(
                        new DateTimeOffset(startTS.Year, startTS.Month, startTS.Day, startTS.Hour, startTS.Minute, startTS.Second, 0, startTS.Offset),
                        aggregateCollector[0].AggregationPeriodStart);

            const int millisecsTollerance = 50;
            long durationMs = (long) aggregateCollector[0].AggregationPeriodDuration.TotalMilliseconds;

            Assert.IsTrue(Math.Abs(durationMs - (endTSRounded - aggregateCollector[0].AggregationPeriodStart).TotalMilliseconds) < millisecsTollerance);

            Assert.AreEqual(1, aggregateCollector.Count);
            aggregateCollector.Clear();
            Assert.AreEqual(0, aggregateCollector.Count);

            manager.Flush();
            Assert.AreEqual(0, aggregateCollector.Count);

            TestUtil.CompleteDefaultAggregationCycle(manager);
        }

        [TestMethod]
        public void TrackValueObject()
        {
            DateTimeOffset startTS = DateTimeOffset.Now;

            var aggregateCollector = new MemoryMetricTelemetryPipeline();
            var manager = new MetricManager(aggregateCollector);
            IMetricSeriesConfiguration config = new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false);
            MetricSeries series = manager.CreateNewSeries("ns-ping", "Foo Bar", config);

            Assert.AreEqual(
                        new DateTimeOffset(startTS.Year, startTS.Month, startTS.Day, startTS.Hour, startTS.Minute, startTS.Second, 0, startTS.Offset),
                        aggregateCollector[0].AggregationPeriodStart);

            const int millisecsTollerance = 50;
            long durationMs = (long) aggregateCollector[0].AggregationPeriodDuration.TotalMilliseconds;
            Assert.IsTrue(Math.Abs(durationMs - (endTSRounded - aggregateCollector[0].AggregationPeriodStart).TotalMilliseconds) < millisecsTollerance);

            TestUtil.CompleteDefaultAggregationCycle(manager);
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void ResetAggregation()
        {
            // Do not start this test in the last 10 secs or first 2 secs of a minute, to make sure the timings below are likely to work out.

            while (DateTimeOffset.Now.Second >= 50 || DateTimeOffset.Now.Second < 2)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(500));
            }

            DateTimeOffset startTS = DateTimeOffset.Now;

            var aggregateCollector = new MemoryMetricTelemetryPipeline();
            var manager = new MetricManager(aggregateCollector);
            IMetricSeriesConfiguration config = new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false);
            MetricSeries series = manager.CreateNewSeries("ns", "Foo Bar", config);

            series.TrackValue(0.4);
            series.TrackValue(2);
            series.TrackValue(-2);

            Thread.Sleep(TimeSpan.FromMilliseconds(1500));

            DateTimeOffset resetTS = DateTimeOffset.Now;
            series.ResetAggregation();

            series.TrackValue(0.17);

            Thread.Sleep(TimeSpan.FromMilliseconds(1500));

            DateTimeOffset resetTS = DateTimeOffset.Now;
            series.ResetAggregation(resetTS);

            series.TrackValue(0.17);
            series.TrackValue(0.32);
            series.TrackValue(-0.15);
            series.TrackValue(1.07);

            DateTimeOffset endTS = DateTimeOffset.Now;

            TestUtil.ValidateNumericAggregateValues(aggregateCollector[0], ns: "ns", name: "Foo Bar", count: 4, sum: 1.41, max: 1.07, min: -0.15, stdDev: 0.447681527427702, aggKindMoniker: "Microsoft.Azure.Measurement");

            // The following might break sometimes!
            // There is a little chance that second boundary is crossed between startTS and the aggregation timestamps are snapped.
            // rerun the test if it happens.

            Assert.AreNotEqual(
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void GetCurrentAggregateUnsafe_Measurement()
        {
            IMetricSeriesConfiguration seriesConfig = new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false);
            const string aggregationKindMoniker = "Microsoft.Azure.Measurement";

            // Do not start this test in the last 10 secs or first 2 secs of a minute, to make sure the timings below are likely to work out.

            while (DateTimeOffset.Now.Second >= 49 || DateTimeOffset.Now.Second < 3)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(500));
            }

            {
                MetricAggregate aggregate = series.GetCurrentAggregateUnsafe();
                TestUtil.ValidateNumericAggregateValues(aggregate, ns: "NS", name: "Foo Bar", count: 3, sum: 0.4, max: 2, min: -2, stdDev: 1.64384373412506, aggKindMoniker: aggregationKindMoniker);

                // The following might break sometimes!
                // There is a little chance that second boundary is crossed between test TS and the aggregation timestamps are snapped.
                // rerun the test if it happens.

                Assert.AreEqual(
            series.TrackValue(0.17);
            series.TrackValue(0.32);
            series.TrackValue(-0.15);
            series.TrackValue(1.07);

            {
                MetricAggregate aggregate = series.GetCurrentAggregateUnsafe();

                TestUtil.ValidateNumericAggregateValues(aggregate, ns: "NS", name: "Foo Bar", count: 7, sum: 1.81, max: 2, min: -2, stdDev: 1.13330652229191, aggKindMoniker: aggregationKindMoniker);

                // The following might break sometimes!
                // There is a little chance that second boundary is crossed between test TS and the aggregation timestamps are snapped.
                // rerun the test if it happens.

                Assert.AreEqual(
                            new DateTimeOffset(startTS.Year, startTS.Month, startTS.Day, startTS.Hour, startTS.Minute, startTS.Second, 0, startTS.Offset),
                            aggregate.AggregationPeriodStart);
            }

            Thread.Sleep(1500);
            DateTimeOffset flushTS = DateTimeOffset.Now;
            manager.Flush();

            {
                Assert.IsFalse(seriesConfig.RequiresPersistentAggregation);
                MetricAggregate aggregate = series.GetCurrentAggregateUnsafe();
                Assert.IsNull(aggregate);
            }

            series.TrackValue(0);
                Assert.IsFalse(seriesConfig.RequiresPersistentAggregation);
                TestUtil.ValidateNumericAggregateValues(aggregate, ns: "NS", name: "Foo Bar", count: 1, sum: 0, max: 0, min: 0, stdDev: 0, aggKindMoniker: aggregationKindMoniker);

                // The following might break sometimes!
                // There is a little chance that second boundary is crossed between test TS and the aggregation timestamps are snapped.
                // rerun the test if it happens.

                Assert.AreEqual(
                            new DateTimeOffset(flushTS.Year, flushTS.Month, flushTS.Day, flushTS.Hour, flushTS.Minute, flushTS.Second, 0, flushTS.Offset),
                            aggregate.AggregationPeriodStart);
            // Do not start this test in the last 10 secs or first 2 secs of a minute, to make sure the timings below are likely to work out.

            while (DateTimeOffset.Now.Second >= 49 || DateTimeOffset.Now.Second < 3)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(500));
            }

            DateTimeOffset startTS = DateTimeOffset.Now;

            var aggregateCollector = new MemoryMetricTelemetryPipeline();
            var manager = new MetricManager(aggregateCollector);
            MetricSeries series = manager.CreateNewSeries("ns", "Foo Bar", seriesConfig);

            DateTimeOffset stepTS = startTS.AddMinutes(2);
            DateTimeOffset stepTSRounded = new DateTimeOffset(stepTS.Year, stepTS.Month, stepTS.Day, stepTS.Hour, stepTS.Minute, stepTS.Second, 0, stepTS.Offset);

            {
                MetricAggregate aggregate = series.GetCurrentAggregateUnsafe(MetricAggregationCycleKind.Default, stepTS);
                Assert.IsNull(aggregate);

                            new DateTimeOffset(startTS.Year, startTS.Month, startTS.Day, startTS.Hour, startTS.Minute, startTS.Second, 0, startTS.Offset),
                            aggregate.AggregationPeriodStart);

                Assert.AreEqual(
                            (stepTSRounded - aggregate.AggregationPeriodStart).TotalMilliseconds,
                            aggregate.AggregationPeriodDuration.TotalMilliseconds);

                {
                    Assert.IsFalse(seriesConfig.RequiresPersistentAggregation);

            DateTimeOffset customCycleStartTS = stepTS.AddMinutes(1);
            manager.StartOrCycleAggregators(MetricAggregationCycleKind.Custom, customCycleStartTS, futureFilter: null);

            series.TrackValue(0.17);
            series.TrackValue(0.32);
            series.TrackValue(-0.15);
            series.TrackValue(1.07);

            stepTS = stepTS.AddMinutes(2);
            stepTSRounded = new DateTimeOffset(stepTS.Year, stepTS.Month, stepTS.Day, stepTS.Hour, stepTS.Minute, stepTS.Second, 0, stepTS.Offset);

                // This might break: Second boundary might be crossed between snapping test and the aggregation timestamps. Try re-running.

                Assert.AreEqual(
                            new DateTimeOffset(startTS.Year, startTS.Month, startTS.Day, startTS.Hour, startTS.Minute, startTS.Second, 0, startTS.Offset),
                            aggregate.AggregationPeriodStart);

                Assert.AreEqual(
                            (stepTSRounded - aggregate.AggregationPeriodStart).TotalMilliseconds,
                            aggregate.AggregationPeriodDuration.TotalMilliseconds);

                {
                    Assert.IsFalse(seriesConfig.RequiresPersistentAggregation);

                    TestUtil.ValidateNumericAggregateValues(aggregate, ns: "ns", name: "Foo Bar", count: 4, sum: 1.41, max: 1.07, min: -0.15, stdDev: 0.447681527427702, aggKindMoniker: aggregationKindMoniker);

                    // This might break: Second boundary might be crossed between snapping test and the aggregation timestamps. Try re-running.

                    Assert.AreEqual(
                                new DateTimeOffset(customCycleStartTS.Year, customCycleStartTS.Month, customCycleStartTS.Day, customCycleStartTS.Hour, customCycleStartTS.Minute, customCycleStartTS.Second, 0, customCycleStartTS.Offset),
                    Assert.AreEqual(
                                (stepTSRounded - aggregate.AggregationPeriodStart).TotalMilliseconds,
                                aggregate.AggregationPeriodDuration.TotalMilliseconds);

                    manager.StartOrCycleAggregators(MetricAggregationCycleKind.Custom, flushTS, null);

                    aggregate = series.GetCurrentAggregateUnsafe(MetricAggregationCycleKind.Custom, stepTS);
                    Assert.IsNull(aggregate);
                }


            series.TrackValue(0);

            stepTS = stepTS.AddMinutes(2);
            stepTSRounded = new DateTimeOffset(stepTS.Year, stepTS.Month, stepTS.Day, stepTS.Hour, stepTS.Minute, stepTS.Second, 0, stepTS.Offset);

            manager.StopAggregators(MetricAggregationCycleKind.Custom, stepTS);

            {
                MetricAggregate aggregate = series.GetCurrentAggregateUnsafe(MetricAggregationCycleKind.Default, stepTS);
                                aggregate.AggregationPeriodDuration.TotalMilliseconds);
                }


                aggregate = series.GetCurrentAggregateUnsafe(MetricAggregationCycleKind.Custom, stepTS);

                {
                    Assert.IsFalse(seriesConfig.RequiresPersistentAggregation);

                    Assert.IsNull(aggregate);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
