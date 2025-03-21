using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.ApplicationInsights.Extensibility;

using System.Linq;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Channel;

namespace Microsoft.ApplicationInsights.Metrics.Extensibility
        /// <summary />
        [TestMethod]
        public void Ctor()
        {
            {
                var period = new AggregationPeriodSummary(null, null);
                Assert.IsNotNull(period);
            }
        }

        /// <summary />
        [TestMethod]
        public void PersistentAggregates()
        {
            {
                MetricAggregate[] p = new MetricAggregate[0];
                var period = new AggregationPeriodSummary(p, null);

                Assert.IsNull(period.NonpersistentAggregates);

                Assert.IsNotNull(period.PersistentAggregates);
                Assert.AreSame(p, period.PersistentAggregates);
                Assert.AreEqual(0, period.PersistentAggregates.Count);
            }
                Assert.IsNotNull(period.NonpersistentAggregates);
                Assert.AreSame(np, period.NonpersistentAggregates);
                Assert.AreEqual(3, period.NonpersistentAggregates.Count);

                Assert.AreEqual("MNS1", period.NonpersistentAggregates[0].MetricNamespace);
                Assert.AreEqual("mid1", period.NonpersistentAggregates[0].MetricId);
                Assert.AreEqual("KindA", period.NonpersistentAggregates[0].AggregationKindMoniker);

                Assert.AreEqual("MNS3", period.NonpersistentAggregates[2].MetricNamespace);
                Assert.AreEqual("mid3", period.NonpersistentAggregates[2].MetricId);
                Assert.AreEqual("KindC", period.NonpersistentAggregates[2].AggregationKindMoniker);
            }

        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
