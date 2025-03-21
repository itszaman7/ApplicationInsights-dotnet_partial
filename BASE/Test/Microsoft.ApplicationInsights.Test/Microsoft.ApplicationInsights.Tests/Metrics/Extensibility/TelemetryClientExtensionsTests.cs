using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Microsoft.ApplicationInsights.Metrics.TestUtility;
using Microsoft.ApplicationInsights.Extensibility;

namespace Microsoft.ApplicationInsights.Metrics.Extensibility
            TelemetryClient client1 = new TelemetryClient(telemetryPipeline1);
            TelemetryClient client2 = new TelemetryClient(telemetryPipeline1);

            MetricManager managerP11 = telemetryPipeline1.GetMetricManager();
            MetricManager managerP12 = telemetryPipeline1.GetMetricManager();
            MetricManager managerP21 = telemetryPipeline2.GetMetricManager();
            MetricManager managerP22 = telemetryPipeline2.GetMetricManager();

            MetricManager managerCp11 = client1.GetMetricManager(MetricAggregationScope.TelemetryConfiguration);
            MetricManager managerCp12 = client1.GetMetricManager(MetricAggregationScope.TelemetryConfiguration);
            MetricManager managerCp21 = client2.GetMetricManager(MetricAggregationScope.TelemetryConfiguration);
            MetricManager managerCp22 = client2.GetMetricManager(MetricAggregationScope.TelemetryConfiguration);

            MetricManager managerCc11 = client1.GetMetricManager(MetricAggregationScope.TelemetryClient);
            Assert.IsNotNull(managerCp22);
            Assert.IsNotNull(managerCc22);

            Assert.AreSame(managerP11, managerP12);
            Assert.AreSame(managerP21, managerP22);
            Assert.AreNotSame(managerP11, managerP21);

            Assert.AreSame(managerP11, managerCp11);

            Assert.AreNotSame(managerP21, managerCc11);
            Assert.AreNotSame(managerP21, managerCc21);

            TestUtil.CompleteDefaultAggregationCycle(
                        managerP11,
                        managerP21,
                        managerCc21);

            telemetryPipeline1.Dispose();
            telemetryPipeline2.Dispose();
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
