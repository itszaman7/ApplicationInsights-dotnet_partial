using EventCounterCollector.Tests;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.EventCounterCollector;
using Microsoft.ApplicationInsights.Extensibility.EventCounterCollector.Implementation;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EventCounterCollector.Tests
{
    [TestClass]
    public class EventCounterCollectionModuleTests
    {
        private string TestEventCounterSourceName = "Microsoft-ApplicationInsights-Extensibility-EventCounterCollector.Tests.TestEventCounter";
        private string TestEventCounterName1 = "mycountername1";

        [TestMethod]
        [TestCategory("EventCounter")]
        public void WarnsIfNoCountersConfigured()
        {
            using (var eventListener = new EventCounterCollectorDiagnosticListener())
            using (var module = new EventCounterCollectionModule())
            {
                ConcurrentQueue<ITelemetry> itemsReceived = new ConcurrentQueue<ITelemetry>();
                module.Initialize(GetTestTelemetryConfiguration(itemsReceived));
                Assert.IsTrue(CheckEventReceived(eventListener.EventsReceived, nameof(EventCounterCollectorEventSource.ModuleIsBeingInitializedEvent))); 
                Assert.IsTrue(CheckEventReceived(eventListener.EventsReceived, nameof(EventCounterCollectorEventSource.EventCounterCollectorNoCounterConfigured)));
            }
        }

            ConcurrentQueue<ITelemetry> itemsReceived = new ConcurrentQueue<ITelemetry>();

            using (var eventListener = new EventCounterCollectorDiagnosticListener())
            using (var module = new EventCounterCollectionModule(refreshTimeInSecs))
            {
                module.Counters.Add(new EventCounterCollectionRequest() { EventSourceName = this.TestEventCounterSourceName, EventCounterName = this.TestEventCounterName1 });
                module.Initialize(GetTestTelemetryConfiguration(itemsReceived));

                // ACT                
                // These will fire counters 'mycountername2' which is not in the configured list.
        public void ValidateSingleEventCounterCollection()
        {
            // ARRANGE
            const int refreshTimeInSecs = 1;
            ConcurrentQueue<ITelemetry> itemsReceived = new ConcurrentQueue<ITelemetry>();
            string expectedName = this.TestEventCounterSourceName + "|" + this.TestEventCounterName1;
            string expectedMetricNamespace = String.Empty;
            double expectedMetricValue = (1000 + 1500 + 1500 + 400) / 4;
            int expectedMetricCount = 4;

            using (var module = new EventCounterCollectionModule(refreshTimeInSecs))
            {
                module.Counters.Add(new EventCounterCollectionRequest() {EventSourceName = this.TestEventCounterSourceName, EventCounterName = this.TestEventCounterName1 });
                module.Initialize(GetTestTelemetryConfiguration(itemsReceived));

                // ACT
                // Making 4 calls with 1000, 1500, 1500, 400 value, leading to an average of 1100.

                PrintTelemetryItems(itemsReceived);

                // VALIDATE
                ValidateTelemetry(itemsReceived, expectedName, expectedMetricNamespace, expectedMetricValue, expectedMetricCount);

                // Wait another refresh interval to receive more events, but with zero as counter values.
                // as nobody is publishing events.
                Task.Delay(((int)refreshTimeInSecs * 1000)).Wait();                
                Assert.IsTrue(itemsReceived.Count >= 1);
            // ARRANGE
            const int refreshTimeInSecs = 1;
            ConcurrentQueue<ITelemetry> itemsReceived = new ConcurrentQueue<ITelemetry>();
            string expectedName = this.TestEventCounterName1;
            string expectedMetricNamespace = this.TestEventCounterSourceName;
            double expectedMetricValue = 1000;
            int expectedMetricCount = 1;

            using (var module = new EventCounterCollectionModule(refreshTimeInSecs))
            {
                    Trace.WriteLine("Metric.Timestamp:" + metric.Timestamp);
                    Trace.WriteLine("Metric.Sdk:" + metric.Context.GetInternalContext().SdkVersion);
                    foreach (var prop in metric.Properties)
                    {
                        Trace.WriteLine("Metric. Prop:" + "Key:" + prop.Key + "Value:" + prop.Value);
                    }
                }
                Trace.WriteLine("======================================");
            }
        }
            foreach(var evt in allEvents)
            {
                if(evt.Equals(expectedEvent))
                {
                    found = true;
                    break;
                }
            }

            return found;
        }

        private TelemetryConfiguration GetTestTelemetryConfiguration(ConcurrentQueue<ITelemetry> itemsReceived)
        {
            var configuration = new TelemetryConfiguration();
            configuration.InstrumentationKey = "testkey";
            configuration.TelemetryChannel = new TestChannel(itemsReceived);
            return configuration;
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
