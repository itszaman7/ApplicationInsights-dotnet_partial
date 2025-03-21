//-----------------------------------------------------------------------
// <copyright file="EtwTelemetryModuleTests.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.EtwTelemetryCollector.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.EtwCollector;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Tracing.Tests;
    using Microsoft.Diagnostics.Tracing.Session;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using static System.Globalization.CultureInfo;

    [TestClass]
    public sealed class EtwTelemetryModuleTests : IDisposable
    {
        private const int NoEventSourcesConfiguredEventId = 1;
        private const int ModuleInitializationFailedEventId = 3;
        private const int AccessDeniedEventId = 4;

        private readonly AdapterHelper adapterHelper = new AdapterHelper();
        private static bool isTestEnvGood;

        public void Dispose()
        {
            this.adapterHelper.Dispose();
        }

        private TelemetryConfiguration GetTestTelemetryConfiguration(bool resetChannel = true)
        {
            var configuration = new TelemetryConfiguration();
            configuration.InstrumentationKey = this.adapterHelper.InstrumentationKey;
            if (resetChannel)
            {
                configuration.TelemetryChannel = this.adapterHelper.Channel.Reset();
            }
            else
            {
                configuration.TelemetryChannel = this.adapterHelper.Channel;
            }

            return configuration;
        }

        [ClassInitialize]
#pragma warning disable CA1801 // Review unused parameters
        public static void InitializeClass(TestContext testContext)
#pragma warning restore CA1801 // Review unused parameters
        {
            // Only users with administrative privileges, users in the Performance Log Users group,
            // and services running as LocalSystem, LocalService, or NetworkService can enable trace providers
            bool? isElevated = TraceEventSession.IsElevated();
            if (isElevated.HasValue && isElevated.Value)
            {
                EtwTelemetryModuleTests.isTestEnvGood = true;
                return;
            }

            foreach (IdentityReference group in WindowsIdentity.GetCurrent().Groups)
            {

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public void DefaultConstructorExists()
        {
            using (EtwTelemetryModule module = new EtwTelemetryModule())
            {
                Assert.IsNotNull(module, "There has to be a default constructor, which has no parameter.");
            }
        }
                Assert.AreEqual(ModuleInitializationFailedEventId, listener.EventsReceived[0].EventId);
                Assert.AreEqual("Argument configuration is required. The initialization is terminated.", listener.EventsReceived[0].Payload[1].ToString());
            }
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public void InitializeFailedWhenDisposed()
        {
            using (EventSourceModuleDiagnosticListener listener = new EventSourceModuleDiagnosticListener())
            {
                EtwTelemetryModule module = new EtwTelemetryModule();
                module.Dispose();
                module.Initialize(GetTestTelemetryConfiguration());

                Assert.AreEqual(1, listener.EventsReceived.Count);
                Assert.AreEqual(ModuleInitializationFailedEventId, listener.EventsReceived[0].EventId);
                Assert.AreEqual("Can't initialize a module that is disposed. The initialization is terminated.", listener.EventsReceived[0].Payload[1].ToString());
            }
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public void InitializeFailedWhenSourceIsNotSpecified()
        {
            using (EventSourceModuleDiagnosticListener listener = new EventSourceModuleDiagnosticListener())
            using (TraceEventSessionMock traceEventSession = new TraceEventSessionMock(false))
            using (EtwTelemetryModule module = new EtwTelemetryModule(() => traceEventSession))
            {
                module.Initialize(GetTestTelemetryConfiguration());
                Assert.AreEqual(1, listener.EventsReceived.Count);
                Assert.AreEqual(NoEventSourcesConfiguredEventId, listener.EventsReceived[0].EventId);
                Assert.AreEqual("EtwTelemetryModule", listener.EventsReceived[0].Payload[1].ToString());
            }
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public void InitializeFailedWhenAccessDenied()
        {
                Assert.AreEqual(1, traceEventSession.EnabledProviderNames.Count);
                Assert.AreEqual("Test Provider", traceEventSession.EnabledProviderNames[0]);
            }
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public void ProviderEnabledByGuid()
        {
            using (EventSourceModuleDiagnosticListener listener = new EventSourceModuleDiagnosticListener())
        public void ProviderNotEnabledByEmptyGuid()
        {
            using (EventSourceModuleDiagnosticListener listener = new EventSourceModuleDiagnosticListener())
            using (TraceEventSessionMock traceEventSession = new TraceEventSessionMock(false))
            using (EtwTelemetryModule module = new EtwTelemetryModule(() => traceEventSession))
            {
                Guid guid = Guid.Empty;
                module.Sources.Add(new EtwListeningRequest()
                {
                    ProviderGuid = guid,
                module.Initialize(GetTestTelemetryConfiguration());
                Assert.IsFalse(traceEventSession.EnabledProviderGuids.Any(g => Guid.Empty.Equals(g)));
            }
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public async Task ReportSingleEvent()
        {
            using (EtwTelemetryModule module = new EtwTelemetryModule())
                    ProviderName = TestProvider.ProviderName
                });
                module.Initialize(GetTestTelemetryConfiguration());

                TestProvider.Log.Info("Hello!");
                int expectedEventCount = 2;
                await WaitForItems(adapterHelper.Channel, expectedEventCount).ConfigureAwait(false);

                // The very 1st event is for the manifest.
                Assert.AreEqual(expectedEventCount, this.adapterHelper.Channel.SentItems.Length);
            using (EtwTelemetryModule module = new EtwTelemetryModule())
            {
                module.Sources.Add(new EtwListeningRequest()
                {
                    ProviderName = TestProvider.ProviderName
                });
                module.Initialize(GetTestTelemetryConfiguration());
                TestProvider.Log.Info("Hello!");
                TestProvider.Log.Info("World!");

                // The very 1st event is for the manifest.
                Assert.AreEqual(expectedEventCount, this.adapterHelper.Channel.SentItems.Length);
                TraceTelemetry hello = (TraceTelemetry)this.adapterHelper.Channel.SentItems[1];
                TraceTelemetry world = (TraceTelemetry)this.adapterHelper.Channel.SentItems[2];
                Assert.AreEqual("Hello!", hello.Message);
                Assert.AreEqual("World!", world.Message);
            }
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public async Task ReportsAllProperties()
        {
            using (var module = new EtwTelemetryModule())
            {
                module.Sources.Add(new EtwListeningRequest()
                {
                    ProviderName = TestProvider.ProviderName
                });
                module.Initialize(GetTestTelemetryConfiguration());
            {
                module.Sources.Add(new EtwListeningRequest()
                {
                    ProviderName = TestProvider.ProviderName
                });
                module.Initialize(GetTestTelemetryConfiguration());

                TestProvider.Log.Tricky(7, "TrickyEvent", "Actual message");

                int expectedEventCount = 2;
                await this.WaitForItems(this.adapterHelper.Channel, expectedEventCount).ConfigureAwait(false);

                Assert.AreEqual(expectedEventCount, this.adapterHelper.Channel.SentItems.Length);
                TraceTelemetry telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems[1];

                Assert.AreEqual("Manifest message", telemetry.Message);
                Assert.AreEqual(SeverityLevel.Information, telemetry.SeverityLevel);
                Assert.AreEqual("Actual message", telemetry.Properties["Message"]);
                Assert.AreEqual("7", telemetry.Properties["EventId"]);
                Assert.AreEqual("Tricky", telemetry.Properties["EventName"]);
                Assert.AreEqual("7", telemetry.Properties[telemetry.Properties.Keys.First(key => key.StartsWith("EventId", StringComparison.Ordinal) && !string.Equals(key, "EventId", StringComparison.Ordinal))]);
                Assert.AreEqual("TrickyEvent", telemetry.Properties[telemetry.Properties.Keys.First(key => key.StartsWith("EventName", StringComparison.Ordinal) && !string.Equals(key, "EventName", StringComparison.Ordinal))]);
            }
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public async Task ReactsToConfigurationChanges()
        {
            using (var module = new EtwTelemetryModule())
                TestProvider.Log.Warning(3, 4);
                await this.WaitForItems(this.adapterHelper.Channel, 5).ConfigureAwait(false);

                List<TraceTelemetry> expectedTelemetry = new List<TraceTelemetry>();
                TraceTelemetry traceTelemetry = new TraceTelemetry("Hey!", SeverityLevel.Information);
                traceTelemetry.Properties["information"] = "Hey!";
                expectedTelemetry.Add(traceTelemetry);
                traceTelemetry = new TraceTelemetry("Warning!", SeverityLevel.Warning);
                traceTelemetry.Properties["i1"] = 1.ToString(InvariantCulture);
                traceTelemetry.Properties["i2"] = 2.ToString(InvariantCulture);
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public async Task DoNotReportTplEvents()
        {
            using (var module = new EtwTelemetryModule())
            {
                module.Initialize(GetTestTelemetryConfiguration());


                }

                // Wait 2 seconds to see if any events arrive asynchronously through the ETW module.
                // This is a long time but unfortunately there is no good way to make ETW infrastructure "go faster"
                // and we want to make sure that no TPL events sneak through.
                await this.WaitForItems(this.adapterHelper.Channel, 1, TimeSpan.FromSeconds(2)).ConfigureAwait(false);

                Assert.AreEqual(0, this.adapterHelper.Channel.SentItems.Length);
            }
        }

        [TestMethod]
                listeningRequest.ProviderName = TestProvider.ProviderName;
                module.Sources.Add(listeningRequest);

                module.Initialize(GetTestTelemetryConfiguration());

                for (int i = 0; i < 6; i += 2)
                {
                    Parallel.For(0, 2, (idx) =>
                    {
                        PerformActivityAsync(i + idx).GetAwaiter().GetResult();
            return Task.Run(async () =>
            {
                TestProvider.Log.RequestStart(requestId);
                await Task.Delay(50).ConfigureAwait(false);
                TestProvider.Log.RequestStop(requestId);
            });
        }

        /// <summary>
        /// Wait until we caputred <paramref name="count"/> telemetry items or timeout in <see cref="CustomTelemetryChannel" />.
            do
            {
                if (DateTime.Now - start > timeout)
                {
                    // Exceeded allocated time.
                    return;
                }

                itemsCaptured = await channel.WaitForItemsCaptured(timeout.Value).ConfigureAwait(false);
                if (itemsCaptured == null)
                    // Timed out waiting for new events to arrive.
                    return;
                }
            }
            while (itemsCaptured.Value < count);
        }

        [TestCleanup]
        public void Cleanup()
        {
                // This clean up is there to clean up possible left over trace event sessions during the Debug of the unit tests.
                foreach (var name in TraceEventSession.GetActiveSessionNames().Where(n => n.StartsWith("ApplicationInsights-", StringComparison.Ordinal)))
                {
                    TraceEventSession.GetActiveSession(name).Stop();
                }
            }
            catch
            {
                // This should normally not happen. But if this happens, there's, unfortunately, nothing too much we can do here.
            }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
