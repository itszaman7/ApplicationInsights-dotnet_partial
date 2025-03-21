//-----------------------------------------------------------------------
// <copyright file="DiagnosticSourceTelemetryModuleTests.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.DiagnosticSourceListener.Tests
{
    using System;
    using System.Linq;
    using Microsoft.ApplicationInsights.Tests;
    using Microsoft.ApplicationInsights.Tracing.Tests;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.CommonTestShared;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using static System.Globalization.CultureInfo;

    [TestClass]
    [TestCategory("DiagnosticSourceListener")]
    public sealed class DiagnosticSourceTelemetryModuleTests : IDisposable
    {
                    module.Initialize(null);
                });
            }
        }
                Assert.AreEqual(testDiagnosticSource.Name, telemetry.Properties["DiagnosticSource"]);
                Assert.AreEqual(SeverityLevel.Information, telemetry.SeverityLevel);
                Assert.AreEqual(1234.ToString(InvariantCulture), telemetry.Properties["Prop1"]);
                string expectedVersion = SdkVersionHelper.GetExpectedSdkVersion(prefix: "dsl:", loggerType: typeof(DiagnosticSourceTelemetryModule));
                Assert.AreEqual(expectedVersion, telemetry.Context.GetInternalContext().SdkVersion);
            }
        }

        [TestMethod]
        public void ReportsSingleEventEvenIfInitializedMoreThanOnce()
            using (var module = new DiagnosticSourceTelemetryModule())
            {
                var testDiagnosticSource = new TestDiagnosticSource();
                var listeningRequest = new DiagnosticSourceListeningRequest(testDiagnosticSource.Name);
                module.Sources.Add(listeningRequest);

                module.Initialize(GetTestTelemetryConfiguration());
                module.Initialize(GetTestTelemetryConfiguration());

                testDiagnosticSource.Write("JustOnce", new { Index = 8888 });

                Assert.AreEqual(1, this.adapterHelper.Channel.SentItems.Length);
            }
        }

        [TestMethod]
        public void HandlesPropertiesWithNullValues()
        {
            using (var module = new DiagnosticSourceTelemetryModule())
            {
                var testDiagnosticSource = new TestDiagnosticSource();
                var listeningRequest = new DiagnosticSourceListeningRequest(testDiagnosticSource.Name);
                module.Sources.Add(listeningRequest);

                module.Initialize(GetTestTelemetryConfiguration());

                testDiagnosticSource.Write("Hey!", new { Prop1 = (object)null });

                TraceTelemetry telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems.First();
                Assert.AreEqual("Hey!", telemetry.Message);
        }

        private TelemetryConfiguration GetTestTelemetryConfiguration(bool resetChannel = true)
        {
            var configuration = new TelemetryConfiguration();
            configuration.InstrumentationKey = this.adapterHelper.InstrumentationKey;
            if (resetChannel)
            {
                configuration.TelemetryChannel = this.adapterHelper.Channel.Reset();
            }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
