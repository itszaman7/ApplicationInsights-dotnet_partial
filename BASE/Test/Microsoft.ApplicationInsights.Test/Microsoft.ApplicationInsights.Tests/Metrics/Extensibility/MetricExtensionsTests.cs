using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Microsoft.ApplicationInsights.Metrics.Extensibility;
using Microsoft.ApplicationInsights.Metrics.TestUtility;
using Microsoft.ApplicationInsights.Extensibility;

namespace Microsoft.ApplicationInsights.Metrics
{
    /// <summary />
    [TestClass]
        [TestMethod]
        public void GetConfiguration()
        {
            TelemetryConfiguration pipeline = TestUtil.CreateAITelemetryConfig();
            TelemetryClient client = new TelemetryClient(pipeline);

            {
                Metric metric = client.GetMetric("CowsSold");
                Assert.AreEqual(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());
                Assert.AreSame(MetricConfigurations.Common.Measurement(), metric.GetConfiguration());
            }
            {
                Metric metric = client.GetMetric("ChickensSold", config);
                Assert.AreEqual(config, metric.GetConfiguration());
                Assert.AreSame(config, metric.GetConfiguration());
            }
        //        TelemetryClient client = new TelemetryClient(telemetryPipeline);
        //        Metric metric = client.GetMetric("CowsSold");
        //        Assert.AreSame(telemetryPipeline.GetMetricManager(), metric.GetMetricManager());
        //        TestUtil.CompleteDefaultAggregationCycle(telemetryPipeline.GetMetricManager());
        //    }
        //}



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
