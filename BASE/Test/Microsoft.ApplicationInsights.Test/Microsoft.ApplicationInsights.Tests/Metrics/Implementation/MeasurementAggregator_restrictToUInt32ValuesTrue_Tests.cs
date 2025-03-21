using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.ApplicationInsights.Metrics.Extensibility;
using Microsoft.ApplicationInsights.Metrics.TestUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using CycleKind = Microsoft.ApplicationInsights.Metrics.Extensibility.MetricAggregationCycleKind;

namespace Microsoft.ApplicationInsights.Metrics
{
    /// <summary />
    [TestClass]
    public class MeasurementAggregator_restrictToUInt32ValuesTrue_Tests
    {
        /// <summary />
        [TestMethod]
        public void Ctor()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new MeasurementAggregator(configuration: null, dataSeries: null, aggregationCycleKind: CycleKind.Custom));

            {
                var aggregator = new MeasurementAggregator(
                                                new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true),
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
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                // Zero value:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                aggregator.TrackValue(0);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 1, sum: 0.0, max: 0.0, min: 0.0, stdDev: 0.0, timestamp: default(DateTimeOffset), periodMs: periodMillis);
                // Values out of range:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue(-1) );
                Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue(Int32.MinValue) );
                Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue(Int64.MinValue) );

                Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue(((long) UInt32.MaxValue) + (long) 1) );

                aggregator.TrackValue(Double.NaN);
                Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue(Double.PositiveInfinity) );
                Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue(Double.NegativeInfinity) );
                Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue(Double.MaxValue) );

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 0, sum: 0, max: 0, min: 0, stdDev: 0.0, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
                aggregator.TrackValue(42);
                aggregator.TrackValue(19);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 2, sum: 61.0, max: 42.0, min: 19.0, stdDev: 11.5, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
            {
                // 3 values:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);
                aggregator.TrackValue(1800000);
                aggregator.TrackValue(0);
                aggregator.TrackValue(4200000);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 3, sum: 6000000, max: 4200000.0, min: 0, stdDev: 1720465.05340853, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
            {
                // Rounded values:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);
                aggregator.TrackValue(1);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 1, sum: 1, max: 1, min: 1, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(-0.0000001);
                aggregator.TrackValue(0.00000001);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 3, sum: 1, max: 1, min: 0, stdDev: 0.471404520791032, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(100.0000001);
                aggregator.TrackValue( 99.9999999);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 9, sum: 12884902085, max: 4294967295, min: 0, stdDev: 1753413037.5015, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            }
            {
                // Very large numbers:
                var aggregator = new MeasurementAggregator(
                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true),
                                                    dataSeries: null,
                                                    aggregationCycleKind: CycleKind.Custom);

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 0, sum: 0, max: 0, min: 0, stdDev: 0, timestamp: default(DateTimeOffset), periodMs: periodMillis);

                aggregator.TrackValue(UInt32.MaxValue - 10000);
                aggregator.TrackValue(UInt32.MaxValue - 1000);
                aggregator.TrackValue(UInt32.MaxValue - 100);
                aggregator.TrackValue(UInt32.MaxValue);

                aggregate = aggregator.CreateAggregateUnsafe(endTS);
                    {
                        aggregator.TrackValue(v);
                    }
                }

                MetricAggregate aggregate = aggregator.CreateAggregateUnsafe(endTS);
                ValidateNumericAggregateValues(aggregate, count: 10100000, sum: 1515000000000, max: 300000, min: 0, stdDev: 87464.2784226795, timestamp: default(DateTimeOffset), periodMs: periodMillis);
            }
        }

            ValidateNumericAggregateValues(aggregate, count: 2, sum: 6, max: 4, min: 2, stdDev: 1, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue((object) (Int32) (0-5)) );

            aggregator.TrackValue((object) (UInt32) 6);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 3, sum: 12, max: 6, min: 2, stdDev: 1.63299316185545, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue((object) (Int64) (0-7)) );
            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue((object) (IntPtr) 0xFF) );
            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue((object) (UIntPtr) 0xFF) );
            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue((object) (Char) 'x') );

            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue((object) (Single) (0f-9.0f)) );

            aggregator.TrackValue((object) (Double) 10.0);

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 5, sum: 30, max: 10, min: 2, stdDev: 2.82842712474619, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            aggregator.TrackValue("  +14 ");

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
            ValidateNumericAggregateValues(aggregate, count: 7, sum: 56, max: 14, min: 2, stdDev: 4, timestamp: default(DateTimeOffset), periodMs: periodMillis);

            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue("fifteen") );
            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue("") );
            Assert.ThrowsException<ArgumentException>( () => aggregator.TrackValue("foo-bar") );

            aggregate = aggregator.CreateAggregateUnsafe(endTS);
                                                aggregationCycleKind: CycleKind.Custom);

            List<Task> parallelTasks = new List<Task>();
            s_trackValueConcurrentWorker_Current = 0;
            s_trackValueConcurrentWorker_Max = 0;

            for (int i = 0; i < 100; i++)
            {
                Task t = Task.Run( () => TrackValueConcurrentWorker(aggregator) );
                parallelTasks.Add(t);
            }

            Task.WaitAll(parallelTasks.ToArray());

            Assert.AreEqual(0, s_trackValueConcurrentWorker_Current);
            Assert.IsTrue(
                        90 <= s_trackValueConcurrentWorker_Max,
                        "The local machine has timing characteristics resuling in not enough concurrency. Try re-running or tweaking delays."
                      + $" (s_trackValueConcurrentWorker_Max = {s_trackValueConcurrentWorker_Max})");

        {
            await Task.Delay(3);

            int currentWorkersAtStart = Interlocked.Increment(ref s_trackValueConcurrentWorker_Current);
            try
            {
                int maxWorkers = Volatile.Read(ref s_trackValueConcurrentWorker_Max);
                while (currentWorkersAtStart > maxWorkers)
                {
                    int prevMaxWorkers = Interlocked.CompareExchange(ref s_trackValueConcurrentWorker_Max, currentWorkersAtStart, maxWorkers);

                    if (prevMaxWorkers == maxWorkers)
                    {
                        break;
                    }
                    else
                    {
                        maxWorkers = Volatile.Read(ref s_trackValueConcurrentWorker_Max);
                    }
                }
            {
                Interlocked.Decrement(ref s_trackValueConcurrentWorker_Current);
            }
        }

        /// <summary />
        [TestMethod]
        public void CreateAggregateUnsafe()
        {
            var aggregationManager = new MetricAggregationManager();
            var seriesConfig = new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true);

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
                                                    (MetricSeriesConfigurationForMeasurement) metric.GetConfiguration(),
                                                    metric,
                                                    CycleKind.Custom);

            CommonSimpleDataSeriesAggregatorTests.CreateAggregateUnsafe(aggregator, metric, expectedDimensionNamesValues);
        }

        /// <summary />
        [TestMethod]
        public void TryRecycle()
        {
            var measurementAggregator = new MeasurementAggregator(
        {
            var aggregationManager = new MetricAggregationManager();
            var seriesConfig = new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true);
            var metric = new MetricSeries(aggregationManager, new MetricIdentifier("Cows Sold"), null, seriesConfig);

            var aggregatorForConcreteSeries = new MeasurementAggregator(
                                                    (MetricSeriesConfigurationForMeasurement) metric.GetConfiguration(),
                                                    dataSeries: metric,
                                                    aggregationCycleKind: CycleKind.Custom);



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
