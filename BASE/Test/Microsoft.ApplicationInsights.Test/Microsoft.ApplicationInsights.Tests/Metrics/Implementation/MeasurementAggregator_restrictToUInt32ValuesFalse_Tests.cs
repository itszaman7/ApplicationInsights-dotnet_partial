using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.ApplicationInsights.Metrics.Extensibility;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

using Microsoft.ApplicationInsights.Metrics.TestUtility;
using System.Globalization;
using System.Threading;

using CycleKind = Microsoft.ApplicationInsights.Metrics.Extensibility.MetricAggregationCycleKind;
using System.Linq;

namespace Microsoft.ApplicationInsights.Metrics
{
    /// <summary />
    [TestClass]
    public class MeasurementAggregator_restrictToUInt32ValuesFalse_Tests
    {
        /// <summary />
        [TestMethod]
        public void Ctor()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new MeasurementAggregator(configuration: null, dataSeries: null, aggregationCycleKind: CycleKind.Custom));

            {
                var aggregator = new MeasurementAggregator(
                                                new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
                                                dataSeries: null,
                                                aggregationCycleKind: CycleKind.Custom);
                Assert.IsNotNull(aggregator);
            }
        }

        /// <summary />
        [TestMethod]
        public void TrackValueDouble()
        {
            var endTS = new DateTimeOffset(2017, 9, 25, 17, 1, 0, TimeSpan.FromHours(-8));
            long periodMillis = (long) (endTS - default(DateTimeOffset)).TotalMilliseconds;

            {
                // Empty aggregator:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 0, sum: 0.0, max: 0.0, min: 0.0, stdDev: 0.0, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
            {
                // Zero value:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
                                                    dataSeries: null,
                // 3 values:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);
                aggregator.TrackValue(1800000);
                aggregator.TrackValue(0);
                aggregator.TrackValue(-4200000);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 0, sum: 0, max: 0, min: 0, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(1);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 1, sum: 1, max: 1, min: 1, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(0.5);
                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 0, sum: 0, max: 0, min: 0, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(Math.Exp(300));

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 1, sum: Math.Exp(300), max: Math.Exp(300), min: Math.Exp(300), stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(-2 * Math.Exp(300));
                double minus2exp200 = -2 * Math.Exp(300);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 2, sum: -Math.Exp(300), max: Math.Exp(300), min: minus2exp200, stdDev: 2.91363959286188000E+130, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(Math.Exp(300));

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 3, sum: 0, max: Math.Exp(300), min: minus2exp200, stdDev: 2.74700575206167000E+130, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(Math.Exp(700));
                ValidateNumericAggregateValues(aggregate, count: 8, sum: Double.MaxValue, max: Double.MaxValue, min: -Double.MaxValue, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(-Double.PositiveInfinity);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 9, sum: 0, max: Double.MaxValue, min: -Double.MaxValue, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
            {
                // Large number of small values:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                for (int i = 0; i < 100000; i++)
                {
                    for (double v = 0; v <= 1.0 || Math.Abs(1.0 - v) < TestUtil.MaxAllowedPrecisionError; v += 0.01)
                    {
                        aggregator.TrackValue(v);
                    }
                }

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 10100000, sum: 5050000, max: 1, min: 0, stdDev: 0.29154759474226500, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
            {
                // Large number of large values:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
                                                    dataSeries: null,

                for (int i = 0; i < 100000; i++)
                {
                    for (double v = 0; v <= 300000.0 || Math.Abs(300000.0 - v) < TestUtil.MaxAllowedPrecisionError; v += 3000)
                    {
                        aggregator.TrackValue(v);
                    }
                }

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
        {
            var endTS = new DateTimeOffset(2017, 9, 25, 17, 1, 0, TimeSpan.FromHours(-8));
            long periodMillis = (long) (endTS - default(DateTimeOffset)).TotalMilliseconds;

            var aggregator = new MeasurementAggregator(
                                                new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
                                                dataSeries: null,
                                                aggregationCycleKind: CycleKind.Custom);

            aggregator.TrackValue(null);
            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 1, sum: -1, max: -1, min: -1, stdDev: 0.0, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            ValidateNumericAggregateValues(aggregate, count: 2, sum: 1, max: 2, min: -1, stdDev: 1.5, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            aggregator.TrackValue((object) (Int16) (0-3));

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 3, sum: -2, max: 2, min: -3, stdDev: 2.05480466765633, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            aggregator.TrackValue((object) (UInt16) 4);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 5, sum: -3, max: 4, min: -5, stdDev: 3.26190128606002, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            aggregator.TrackValue((object) (UInt32) 6);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 6, sum: 3, max: 6, min: -5, stdDev: 3.86221007541882, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            aggregator.TrackValue((object) (Int64) (0-7));

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 7, sum: -4, max: 6, min: -7, stdDev: 4.43547848464572, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            aggregator.TrackValue((object) (UInt64) 8);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 8, sum: 4, max: 8, min: -7, stdDev: 5.02493781056044, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue((object) (IntPtr) 0xFF) );

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 12, sum: 6, max: 12, min: -11, stdDev: 7.34279692397023, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            aggregator.TrackValue("-1.300E+01");

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 13, sum: -7, max: 12, min: -13, stdDev: 7.91896831484996, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            aggregator.TrackValue("  +14. ");
        }

        /// <summary />
        [TestMethod]
        public void TrackValueConcurrently()
        {
            var endTS = new DateTimeOffset(2017, 9, 25, 17, 1, 0, TimeSpan.FromHours(-8));
            long periodMillis = (long) (endTS - default(DateTimeOffset)).TotalMilliseconds;

            var aggregator = new MeasurementAggregator(
            Trace.WriteLine($"s_countBufferWaitSpinEvents: {MetricSeriesAggregatorBase<double>.countBufferWaitSpinEvents}");
            Trace.WriteLine($"s_countBufferWaitSpinCycles: {MetricSeriesAggregatorBase<double>.countBufferWaitSpinCycles}");
            Trace.WriteLine($"s_timeBufferWaitSpinMillis: {TimeSpan.FromMilliseconds(MetricSeriesAggregatorBase<double>.timeBufferWaitSpinMillis)}");

            Trace.WriteLine($"s_countBufferFlushes: {MetricSeriesAggregatorBase<double>.countBufferFlushes}");
            Trace.WriteLine($"s_countNewBufferObjectsCreated: {MetricSeriesAggregatorBase<double>.countNewBufferObjectsCreated}");
#endif
            ValidateNumericAggregateValues(aggregate, count: 5050000, sum: 757500000000, max: 300000, min: 0, stdDev: 87464.2784226795, timestamp: default(DateTimeOffset), periodMs: periodMillis);
        }

                        maxWorkers = Volatile.Read(ref s_trackValueConcurrentWorker_Max);
                    }
                }

                for (int i = 0; i < 500; i++)
                {
                    for (int v = 0; v <= 300000; v += 3000)
                    {
                        aggregator.TrackValue(v);
                        //Thread.Yield();
                        await Task.Delay(0);
                    }

                    await Task.Delay(1);
                }
            }
            finally
            {
                Interlocked.Decrement(ref s_trackValueConcurrentWorker_Current);
            }
        }

        private static void ValidateNumericAggregateValues(MetricAggregate aggregate, int count, double sum, double max, double min, double stdDev, DateTimeOffset timestamp, long periodMs)
        {
            TestUtil.ValidateNumericAggregateValues(aggregate, "", "null", count, sum, max, min, stdDev, timestamp, periodMs, "Microsoft.Azure.Measurement");
        }

        /// <summary />
        [TestMethod]
        public void CreateAggregateUnsafe()
        {
            var aggregationManager = new MetricAggregationManager();
            var seriesConfig = new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false);

            IEnumerable<KeyValuePair<string, string>> setDimensionNamesValues = new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("Dim 1", "DV1"),
                                                                                                                     new KeyValuePair<string, string>("Dim 2", "DV2"),
                                                                                                                     new KeyValuePair<string, string>("Dim 3", "DV3"),
                                                                                                                     new KeyValuePair<string, string>("Dim 2", "DV2a") };

            IEnumerable<KeyValuePair<string, string>> expectedDimensionNamesValues = new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("Dim 1", "DV1"),
                                                                                                                          new KeyValuePair<string, string>("Dim 2", "DV2a"),
                                                                                                                          new KeyValuePair<string, string>("Dim 3", "DV3") };

            var metric = new MetricSeries(
                                    aggregationManager,
                                    new MetricIdentifier(String.Empty, "Cows Sold", expectedDimensionNamesValues.Select( nv => nv.Key ).ToArray()),
                                    setDimensionNamesValues,
                                    seriesConfig);

            var aggregator = new MeasurementAggregator(
            CommonSimpleDataSeriesAggregatorTests.TryRecycle_PersistentAggregator(accumulatorAggregator);
        }

        /// <summary />
        [TestMethod]
        public void GetDataSeries()
        {
            var aggregationManager = new MetricAggregationManager();
            var seriesConfig = new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false);
            var metric = new MetricSeries(aggregationManager, new MetricIdentifier("Cows Sold"), null, seriesConfig);

            var aggregatorForConcreteSeries = new MeasurementAggregator(
                                                    (MetricSeriesConfigurationForMeasurement) metric.GetConfiguration(),
                                                    dataSeries: metric,
                                                    aggregationCycleKind: CycleKind.Custom);

            var aggregatorForNullSeries = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

            CommonSimpleDataSeriesAggregatorTests.GetDataSeries(aggregatorForConcreteSeries, aggregatorForNullSeries, metric);
        }

        /// <summary />
        [TestMethod]
        public void Reset()
        {
            {
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                CommonSimpleDataSeriesAggregatorTests.Reset_NonpersistentAggregator(aggregator, aggregateKindMoniker: "Microsoft.Azure.Measurement");
            }
            {
                var aggregator = new MetricSeriesConfigurationForTestingAccumulatorBehavior.Aggregator(
                                                    new MetricSeriesConfigurationForTestingAccumulatorBehavior(),
                                                    dataSeries: null,
        }

        /// <summary />
        [TestMethod]
        public void CompleteAggregation()
        {
            var aggregationManager = new MetricAggregationManager();

            var mesurementConfig = new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false);
            var measurementMetric = new MetricSeries(aggregationManager, new MetricIdentifier("Animal Metrics", "Cows Sold"), null, mesurementConfig);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
