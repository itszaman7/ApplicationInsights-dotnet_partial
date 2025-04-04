﻿//-----------------------------------------------------------------------
// <copyright file="EventSourceTelemetryModuleTests.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.EventSourceListener.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.CommonTestShared;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.EventSourceListener;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Tests;
    using Microsoft.ApplicationInsights.TraceEvent.Shared.Implementation;
    using Microsoft.ApplicationInsights.TraceEvent.Shared.Utilities;
    using Microsoft.ApplicationInsights.Tracing.Tests;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using static System.Globalization.CultureInfo;

    [TestClass]
    public sealed class EventSourceTelemetryModuleTests : IDisposable
    {
        private readonly AdapterHelper adapterHelper = new AdapterHelper();

        public void Dispose()
        {
            this.adapterHelper.Dispose();
        }

        [TestMethod]
        [TestCategory("EventSourceListener")]
        public void ThrowsWhenNullConfigurationPassedToInitialize()
        {
            using (var module = new EventSourceTelemetryModule())
            {
                ExceptionAssert.Throws<ArgumentNullException>(() =>
                {
                    module.Initialize(null);
                });
            }
        }

        [TestMethod]
        [TestCategory("EventSourceListener")]
        public void WarnsIfNoSourcesConfigured()
        {
            using (var eventListener = new EventSourceModuleDiagnosticListener())
            using (var module = new EventSourceTelemetryModule())
            {
                module.Initialize(GetTestTelemetryConfiguration());
                Assert.AreEqual(1, eventListener.EventsReceived.Count);
                Assert.AreEqual(nameof(EventSourceListenerEventSource.NoSourcesConfigured), eventListener.EventsReceived[0]);
            }
        }

        [TestMethod]
        [TestCategory("EventSourceListener")]
        public void ReportsSingleEvent()
        {
            using (var module = new EventSourceTelemetryModule())
            {
                var listeningRequest = new EventSourceListeningRequest();
                listeningRequest.Name = TestEventSource.ProviderName;
                module.Sources.Add(listeningRequest);

                string expectedVersion = SdkVersionHelper.GetExpectedSdkVersion(prefix: "evl:", loggerType: typeof(EventSourceTelemetryModule));
                Assert.AreEqual(expectedVersion, telemetry.Context.GetInternalContext().SdkVersion);
            }
        }

        [TestMethod]
        [TestCategory("EventSourceListener")]
        public void PrefixMatchEnablingEventSource()
        {
            using (var module = new EventSourceTelemetryModule())
                {
                    Name = TestEventSource.ProviderName.Substring(0, TestEventSource.ProviderName.Length - 2),
                    PrefixMatch = true
                };
                module.Sources.Add(listeningRequest);

                module.Initialize(GetTestTelemetryConfiguration());

                TestEventSource.Default.InfoEvent("Hey!");


        [TestMethod]
        [TestCategory("EventSourceListener")]
        public void ReportsSingleEventFromSourceCreatedAfterModuleCreated()
        {
            using (var module = new EventSourceTelemetryModule())
            {
                var listeningRequest = new EventSourceListeningRequest();
                listeningRequest.Name = OtherTestEventSource.ProviderName;
                module.Sources.Add(listeningRequest);
                TestEventSource.Default.WarningEvent(1, 2);

                // Now request reporting events only with certain keywords
                listeningRequest.Keywords = TestEventSource.Keywords.NonRoutine;
                module.Initialize(GetTestTelemetryConfiguration(resetChannel: false));

                TestEventSource.Default.InfoEvent("Hey again!");
                TestEventSource.Default.WarningEvent(3, 4);

                List<TraceTelemetry> expectedTelemetry = new List<TraceTelemetry>();
                traceTelemetry.Properties["information"] = "Hey!";
                expectedTelemetry.Add(traceTelemetry);
                traceTelemetry = new TraceTelemetry("Warning!", SeverityLevel.Warning);
                traceTelemetry.Properties["i1"] = 1.ToString(InvariantCulture);
                traceTelemetry.Properties["i2"] = 2.ToString(InvariantCulture);
                expectedTelemetry.Add(traceTelemetry);
                // Note that second informational event is not expected
                traceTelemetry = new TraceTelemetry("Warning!", SeverityLevel.Warning);
                traceTelemetry.Properties["i1"] = 3.ToString(InvariantCulture);
                traceTelemetry.Properties["i2"] = 4.ToString(InvariantCulture);

                CollectionAssert.AreEqual(expectedTelemetry, this.adapterHelper.Channel.SentItems, new TraceTelemetryComparer(), "Reported events are not what was expected");
            }
        }

        [TestMethod]
        [TestCategory("EventSourceListener")]
        public void ReactsToConfigurationChangesWithDisabledEventSources()
        {
            using (var module = new EventSourceTelemetryModule())
                    Name = TestEventSource.ProviderName
                };

                // Disabled
                module.DisabledSources.Add(disableListeningRequest);
                module.Initialize(GetTestTelemetryConfiguration());
                TestEventSource.Default.InfoEvent("Hey!");
                int sentCount = this.adapterHelper.Channel.SentItems.Count();
                Assert.AreEqual(0, sentCount);

                module.DisabledSources.Add(disableListeningRequest);
                module.Initialize(GetTestTelemetryConfiguration());
                TestEventSource.Default.InfoEvent("Hey!");
                sentCount = this.adapterHelper.Channel.SentItems.Count();
                Assert.AreEqual(0, sentCount);
            }
        }

        [TestMethod]
        [TestCategory("EventSourceListener")]
        public void ReportsSeverityLevel()
        {
            using (var module = new EventSourceTelemetryModule())
            {
                var listeningRequest = new EventSourceListeningRequest();
                listeningRequest.Name = TestEventSource.ProviderName;
                module.Sources.Add(listeningRequest);

                module.Initialize(GetTestTelemetryConfiguration());

                };

                CollectionAssert.AreEqual(expectedTelemetry, this.adapterHelper.Channel.SentItems, new TraceTelemetryComparer(), "Reported events are not what was expected");
            }
        }

        [TestMethod]
        [TestCategory("EventSourceListener")]
        public void ReportsAllProperties()
        {
            OnEventWrittenHandler onWrittenHandler = (EventWrittenEventArgs args, TelemetryClient client) =>
            {
                listeningRequest.Name = TestEventSource.ProviderName;
                module.Sources.Add(listeningRequest);

                module.Initialize(GetTestTelemetryConfiguration());

                TestEventSource.Default.Tricky(7, "TrickyEvent", "Actual message");

                Assert.AreEqual(1, this.adapterHelper.Channel.SentItems.Length);
                TraceTelemetry telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems[0];
                Assert.AreEqual("Manifest message", telemetry.Message);
                Assert.AreEqual(SeverityLevel.Information, telemetry.SeverityLevel);
                Assert.AreEqual("Actual message", telemetry.Properties["Message"]);
                Assert.AreEqual("7", telemetry.Properties["EventId"]);
                Assert.AreEqual("TrickyEvent", telemetry.Properties["EventName"]);
                Assert.IsTrue(telemetry.Properties[telemetry.Properties.Keys.First(key => key.StartsWith("EventId", StringComparison.Ordinal) && !string.Equals(key, "EventId", StringComparison.Ordinal))].Equals("7", StringComparison.Ordinal));
                Assert.IsTrue(telemetry.Properties[telemetry.Properties.Keys.First(key => key.StartsWith("EventName", StringComparison.Ordinal) && !string.Equals(key, "EventName", StringComparison.Ordinal))].Equals("Tricky", StringComparison.Ordinal));
            }
        }

        [TestMethod]
            Assert.AreEqual("//1/1/", activityPath);
        }

        [TestMethod]
        [TestCategory("EventSourceListener")]
        public void ActivityPathDecoderHandlesNonhierarchicalActivityIds()
        {
            string guidString = "bf0209f9-bf5e-415e-86ed-0e20b615b406";
            Guid activityId = new Guid(guidString);
            string activityPath = ActivityPathDecoder.GetActivityPathString(activityId);

        [TestMethod]
        [TestCategory("EventSourceListener")]
        public void ActivityPathDecoderHandlesEmptyActivityId()
        {
            string activityPath = ActivityPathDecoder.GetActivityPathString(Guid.Empty);
            Assert.AreEqual(Guid.Empty.ToString(), activityPath);
        }

        [TestMethod]
        [TestCategory("EventSourceListener")]
        public void DoNotReportTplEvents()
        {
            using (var module = new EventSourceTelemetryModule())
            {
                module.Initialize(GetTestTelemetryConfiguration());

                for (int i = 0; i < 10; i += 2)
                {
                    Parallel.For(0, 2, (idx) =>
                        PerformActivityAsync(i + idx).GetAwaiter().GetResult();
                    });

                }

                Assert.AreEqual(0, this.adapterHelper.Channel.SentItems.Length);
            }
        }

        [TestMethod]
        [TestCategory("EventSourceListener")]
        public void DisablesAppInsightsDataByDefault()
        {
            using (var module = new EventSourceTelemetryModule())
            {
                module.Initialize(GetTestTelemetryConfiguration());

                Assert.AreEqual(1, module.DisabledSources.Count);
                Assert.AreEqual(new DisableEventSourceRequest { Name = "Microsoft-ApplicationInsights-Data" }, module.DisabledSources[0]);
            }
        public void DoesNotDisableAppInsightsDataIfExplicitlyEnabled()
        {
            using (var module = new EventSourceTelemetryModule())
            {
                module.Sources.Add(new EventSourceListeningRequest { Name = "Microsoft-ApplicationInsights-Data" });
                module.Initialize(GetTestTelemetryConfiguration());

                Assert.AreEqual(0, module.DisabledSources.Count);
            }
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
