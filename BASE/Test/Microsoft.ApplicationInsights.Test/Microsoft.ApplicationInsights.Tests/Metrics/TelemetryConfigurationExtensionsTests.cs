using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Metrics;
using Microsoft.ApplicationInsights.Metrics.TestUtility;
using Microsoft.ApplicationInsights.DataContracts;

namespace SomeCustomerNamespace
{
    /// <summary />
    [TestClass]
    public class TelemetryConfigurationExtensionsTests
    {
        /// <summary />
        [TestCategory(TestCategoryNames.NeedsAggregationCycleCompletion)]
        [TestMethod]
        public void Metrics_DefaultPipeline()
        {
            TelemetryConfiguration defaultTelemetryPipeline = TelemetryConfiguration.CreateDefault();
            using (defaultTelemetryPipeline)
            {
                Metrics_SpecifiedPipeline(defaultTelemetryPipeline);
                TestUtil.CompleteDefaultAggregationCycle(defaultTelemetryPipeline.GetMetricManager());
            }
        }
                MetricManager managerCust1 = customTelemetryPipeline1.GetMetricManager();
                MetricManager managerCust2 = customTelemetryPipeline2.GetMetricManager();
                Assert.IsFalse(Object.ReferenceEquals(managerCust1, managerCust2));

                Metrics_SpecifiedPipeline(customTelemetryPipeline1);
                Metrics_SpecifiedPipeline(customTelemetryPipeline2);

                TestUtil.CompleteDefaultAggregationCycle(managerDef);
                TestUtil.CompleteDefaultAggregationCycle(managerCust1);
                TestUtil.CompleteDefaultAggregationCycle(managerCust2);
            }
            telemetryPipeline.InstrumentationKey = Guid.NewGuid().ToString("D");

            MetricManager manager1 = telemetryPipeline.GetMetricManager();
            Assert.IsNotNull(manager1);

            MetricManager manager2 = telemetryPipeline.GetMetricManager();
            Assert.IsNotNull(manager2);

            Assert.AreEqual(manager1, manager2);
            IMetricSeriesConfiguration seriesConfig = new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false);

            manager1.CreateNewSeries("ns", "Metric A", seriesConfig).TrackValue(42);
            manager1.CreateNewSeries("ns", "Metric A", seriesConfig).TrackValue("18");
            manager2.CreateNewSeries("ns", "Metric A", seriesConfig).TrackValue(10000);
            manager2.CreateNewSeries("ns", "Metric B", seriesConfig).TrackValue(-0.001);
            manager1.Flush();

            Assert.AreEqual(4, telemetryCollector.TelemetryItems.Count);
            Assert.AreEqual(1, ((MetricTelemetry) telemetryCollector.TelemetryItems[2]).Count);
            Assert.AreEqual(18, ((MetricTelemetry) telemetryCollector.TelemetryItems[2]).Sum);

            Assert.IsInstanceOfType(telemetryCollector.TelemetryItems[3], typeof(MetricTelemetry));
            Assert.AreEqual("Metric A", ((MetricTelemetry) telemetryCollector.TelemetryItems[3]).Name);
            Assert.AreEqual(1, ((MetricTelemetry) telemetryCollector.TelemetryItems[3]).Count);
            Assert.AreEqual(42, ((MetricTelemetry) telemetryCollector.TelemetryItems[3]).Sum);
        }

        //private class CollectingTelemetryInitializer : ITelemetryInitializer
        //{
        //    private List<ITelemetry> _telemetryItems = new List<ITelemetry>();

        //    public IList<ITelemetry> TelemetryItems { get { return _telemetryItems; } }

        //    public void Initialize(ITelemetry telemetry)
        //    {
        //        _telemetryItems.Add(telemetry);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
