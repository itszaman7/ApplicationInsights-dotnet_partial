using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.ApplicationInsights.Metrics.Extensibility;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Metrics.TestUtility;

namespace Microsoft.ApplicationInsights.Metrics
{
    /// <summary />
    [TestClass]
    public class MetricAggregationManagerTests
    {
        /// <summary />
        [TestMethod]
        public void Ctor()
        {
            var aggregationManager = new MetricAggregationManager();
            Assert.IsNotNull(aggregationManager);
        }

        /// <summary />
        [TestMethod]
        public void DefaultState()
        {
            DateTimeOffset dto = new DateTimeOffset(2017, 10, 2, 17, 5, 0, TimeSpan.FromHours(-7));

            var aggregationManager = new MetricAggregationManager();

            var measurementMetric = new MetricSeries(
                                            aggregationManager,
                                            new MetricIdentifier("Measurement Metric"),
                                            null,
                                            new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            var accumulatorMetric = new MetricSeries(
                                            aggregationManager,
                                            new MetricIdentifier("Accumulator Metric"),
            accumulatorMetric.TrackValue(2);
            
            AggregationPeriodSummary defaultPeriod = aggregationManager.StartOrCycleAggregators(MetricAggregationCycleKind.Default, dto, futureFilter: null);
            Assert.IsNotNull(defaultPeriod);
            Assert.IsNotNull(defaultPeriod.NonpersistentAggregates);
            Assert.IsNotNull(defaultPeriod.PersistentAggregates);

            Assert.AreEqual(1, defaultPeriod.NonpersistentAggregates.Count);
            Assert.AreEqual("Measurement Metric", (defaultPeriod.NonpersistentAggregates[0]).MetricId);
            Assert.AreEqual(1, defaultPeriod.NonpersistentAggregates[0].Data["Count"]);

        /// <summary />
        [TestMethod]
        public void StartOrCycleAggregators()
        {
            StartOrCycleAggregatorsTest(MetricAggregationCycleKind.Default, supportsSettingFilters: false);
            StartOrCycleAggregatorsTest(MetricAggregationCycleKind.QuickPulse, supportsSettingFilters: true);
            StartOrCycleAggregatorsTest(MetricAggregationCycleKind.Custom, supportsSettingFilters: true);

            {
                DateTimeOffset dto = new DateTimeOffset(2017, 10, 2, 17, 5, 0, TimeSpan.FromHours(-7));

                var aggregationManager = new MetricAggregationManager();
                Assert.ThrowsException<ArgumentException>( () => aggregationManager.StartOrCycleAggregators((MetricAggregationCycleKind) 42, dto, futureFilter: null) );
            }
        }

        private static void StartOrCycleAggregatorsTest(MetricAggregationCycleKind cycleKind, bool supportsSettingFilters)
        {
            DateTimeOffset dto = new DateTimeOffset(2017, 10, 2, 17, 5, 0, TimeSpan.FromHours(-7));
                                            null,
                                            new MetricSeriesConfigurationForTestingAccumulatorBehavior());

            // Cycle once, get nothing:
            AggregationPeriodSummary period = aggregationManager.StartOrCycleAggregators(cycleKind, dto, futureFilter: null);
            Assert.IsNotNull(period);
            Assert.IsNotNull(period.NonpersistentAggregates);
            Assert.IsNotNull(period.PersistentAggregates);

            Assert.AreEqual(0, period.NonpersistentAggregates.Count);
            Assert.IsNotNull(period.PersistentAggregates[0]);

            Assert.AreEqual("Measurement Metric", period.NonpersistentAggregates[0].MetricId);
            Assert.AreEqual(1, period.NonpersistentAggregates[0].Data["Count"]);
            Assert.AreEqual(1.0, period.NonpersistentAggregates[0].Data["Sum"]);

            Assert.IsNotNull(period);
            Assert.IsNotNull(period.NonpersistentAggregates);
            Assert.IsNotNull(period.PersistentAggregates);

            Assert.AreEqual(0, period.NonpersistentAggregates.Count);
            Assert.AreEqual(1, period.PersistentAggregates.Count);

            Assert.IsNotNull(period.PersistentAggregates[0]);

            Assert.AreEqual("Accumulator Metric", period.PersistentAggregates[0].MetricId);
            // Note: for persistent, values tracked under Deny filter should persist for the future, for non-persistent they are just discarded.

            if (false == supportsSettingFilters)
            {
                Assert.ThrowsException<ArgumentException>(() => aggregationManager.StartOrCycleAggregators(
                                                                                       cycleKind,
                                                                                       dto,
                                                                                       futureFilter: new AcceptAllFilter()));
            }
            else
                measurementMetric.TrackValue(3);
                accumulatorMetric.TrackValue(4);

                period = aggregationManager.StartOrCycleAggregators(cycleKind, dto, futureFilter: null);
                Assert.IsNotNull(period);
                Assert.IsNotNull(period.NonpersistentAggregates);
                Assert.IsNotNull(period.PersistentAggregates);

                Assert.AreEqual(0, period.NonpersistentAggregates.Count);
                Assert.AreEqual(0, period.PersistentAggregates.Count);

                period = aggregationManager.StartOrCycleAggregators(cycleKind, dto, futureFilter: null);
                Assert.IsNotNull(period);
                Assert.IsNotNull(period.NonpersistentAggregates);
                Assert.IsNotNull(period.PersistentAggregates);

                Assert.AreEqual(0, period.NonpersistentAggregates.Count);
                Assert.AreEqual(1, period.PersistentAggregates.Count);

                Assert.IsNotNull(period.PersistentAggregates[0]);
        {
            DateTimeOffset dto = new DateTimeOffset(2017, 10, 2, 17, 5, 0, TimeSpan.FromHours(-7));

            var aggregationManager = new MetricAggregationManager();

            var measurementMetric = new MetricSeries(
                                            aggregationManager,
                                            new MetricIdentifier("Measurement Metric"),
                                            null,
                                            new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));
                                            new MetricSeriesConfigurationForTestingAccumulatorBehavior());

            // Cannot stop default:

            Assert.ThrowsException<ArgumentException>( () => aggregationManager.StopAggregators(MetricAggregationCycleKind.Default, dto) );

            // Stop cycles that never started:

            AggregationPeriodSummary customPeriod = aggregationManager.StopAggregators(MetricAggregationCycleKind.Custom, dto);
            Assert.IsNotNull(customPeriod);
            Assert.IsNotNull(quickpulsePeriod);
            Assert.IsNotNull(quickpulsePeriod.NonpersistentAggregates);
            Assert.IsNotNull(quickpulsePeriod.PersistentAggregates);

            Assert.AreEqual(0, quickpulsePeriod.NonpersistentAggregates.Count);
            Assert.AreEqual(0, quickpulsePeriod.PersistentAggregates.Count);

            // Track a value. Stop cycles that never started again. Observe that persistent cycle was active by default:

            measurementMetric.TrackValue(1);

            Assert.AreEqual(0, customPeriod.NonpersistentAggregates.Count);
            Assert.AreEqual(1, customPeriod.PersistentAggregates.Count);
            Assert.AreEqual("Accumulator Metric", customPeriod.PersistentAggregates[0].MetricId);
            Assert.AreEqual(2.0, customPeriod.PersistentAggregates[0].Data["Sum"]);

            quickpulsePeriod = aggregationManager.StopAggregators(MetricAggregationCycleKind.QuickPulse, dto);
            Assert.IsNotNull(quickpulsePeriod);
            Assert.IsNotNull(quickpulsePeriod.NonpersistentAggregates);
            Assert.IsNotNull(quickpulsePeriod.PersistentAggregates);

            customPeriod = aggregationManager.StopAggregators(MetricAggregationCycleKind.Custom, dto);
            Assert.IsNotNull(customPeriod);
            Assert.IsNotNull(customPeriod.NonpersistentAggregates);
            Assert.IsNotNull(customPeriod.PersistentAggregates);

            Assert.AreEqual(1, customPeriod.NonpersistentAggregates.Count);
            Assert.AreEqual("Measurement Metric", customPeriod.NonpersistentAggregates[0].MetricId);
            Assert.AreEqual(1, customPeriod.NonpersistentAggregates[0].Data["Count"]);
            Assert.AreEqual(3.0, customPeriod.NonpersistentAggregates[0].Data["Sum"]);

            Assert.AreEqual(1, customPeriod.PersistentAggregates.Count);
            Assert.AreEqual("Accumulator Metric", customPeriod.PersistentAggregates[0].MetricId);
            Assert.AreEqual(6.0, customPeriod.PersistentAggregates[0].Data["Sum"]);

            quickpulsePeriod = aggregationManager.StopAggregators(MetricAggregationCycleKind.QuickPulse, dto);
            Assert.IsNotNull(quickpulsePeriod);
            Assert.IsNotNull(quickpulsePeriod.NonpersistentAggregates);
            Assert.IsNotNull(quickpulsePeriod.PersistentAggregates);

            Assert.IsNotNull(quickpulsePeriod);
            Assert.IsNotNull(quickpulsePeriod.NonpersistentAggregates);
            Assert.IsNotNull(quickpulsePeriod.PersistentAggregates);

            Assert.AreEqual(0, quickpulsePeriod.NonpersistentAggregates.Count);
            Assert.AreEqual(1, customPeriod.PersistentAggregates.Count);
            Assert.AreEqual("Accumulator Metric", customPeriod.PersistentAggregates[0].MetricId);
            Assert.AreEqual(6.0, customPeriod.PersistentAggregates[0].Data["Sum"]);

            quickpulsePeriod = aggregationManager.StopAggregators(MetricAggregationCycleKind.QuickPulse, dto);
                return true;
            }
        }

        private class DenyAllFilter : IMetricSeriesFilter
        {
            public bool WillConsume(MetricSeries dataSeries, out IMetricValueFilter valueFilter)
            {
                valueFilter = null;
                return false;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
