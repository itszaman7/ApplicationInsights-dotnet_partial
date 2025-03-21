using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.ApplicationInsights.Metrics.Extensibility;
using Microsoft.ApplicationInsights.Metrics.TestUtility;


namespace Microsoft.ApplicationInsights.Metrics
{ 
    /// <summary />
    [TestClass]
    public class MetricManagerTests
    {

            Assert.ThrowsException<ArgumentNullException>( () => manager.CreateNewSeries("ns", null, config) );
            Assert.ThrowsException<ArgumentNullException>( () => manager.CreateNewSeries("ns", "Foo Bar", null) );

            MetricSeries series = manager.CreateNewSeries("NS", "Foo Bar", config);
            Assert.IsNotNull(series);

            Assert.AreEqual(config, series.GetConfiguration());
            Assert.AreSame(config, series.GetConfiguration());

            Assert.AreEqual("NS", series.MetricIdentifier.MetricNamespace);
            Assert.AreEqual("Foo Bar", series.MetricIdentifier.MetricId);

            TestUtil.CompleteDefaultAggregationCycle(manager);
        }

        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
                var manager = new MetricManager(metricsCollector);
                manager.Flush();

                Assert.AreEqual(0, metricsCollector.Count);
                TestUtil.CompleteDefaultAggregationCycle(manager);
            }
            {
                var metricsCollector = new MemoryMetricTelemetryPipeline();
                var manager = new MetricManager(metricsCollector);

                series2.TrackValue(-1);
                series2.TrackValue(-1);
                series2.TrackValue(-1);

                manager.Flush();

                Assert.AreEqual(2, metricsCollector.Count);

                metricsCollector.Clear();
                Assert.AreEqual(0, metricsCollector.Count);

                manager.Flush();

                Assert.AreEqual(0, metricsCollector.Count);

                TestUtil.CompleteDefaultAggregationCycle(manager);
            }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
