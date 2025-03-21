namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using System.Diagnostics.Tracing;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    

    [TestClass]
    public class DiagnosticsTelemetryModuleTest
                
                Assert.IsTrue(string.IsNullOrEmpty(initializedModule.DiagnosticsInstrumentationKey));
                Assert.AreEqual("Error", initializedModule.Severity);

                Assert.AreEqual(1, initializedModule.Senders.Count);
                Assert.AreEqual(1, initializedModule.Senders.OfType<PortalDiagnosticsSender>().Count());
            }
        }

        [TestMethod]
        public void TestDiagnosticsModuleSetInstrumentationKey()
        {
            var diagnosticsInstrumentationKey = Guid.NewGuid().ToString();
            using (var initializedModule = new DiagnosticsTelemetryModule())
            {
                initializedModule.Initialize(new TelemetryConfiguration());
                initializedModule.DiagnosticsInstrumentationKey = diagnosticsInstrumentationKey;

                Assert.AreEqual(diagnosticsInstrumentationKey, initializedModule.DiagnosticsInstrumentationKey);

                Assert.AreEqual(
                    diagnosticsInstrumentationKey,
                    initializedModule.Senders.OfType<PortalDiagnosticsSender>().First().DiagnosticsInstrumentationKey);
            }
        [TestMethod]
        public void TestDiagnosticModuleDoesNotThrowIfInitailizedTwice()
        {
            using (DiagnosticsTelemetryModule module = new DiagnosticsTelemetryModule())
            {
                module.Initialize(new TelemetryConfiguration());
                module.Initialize(new TelemetryConfiguration());
            }
                    Task.Run(() =>
                    {
                        taskStarted.Set();
                        while (!cancellationTokenSource.IsCancellationRequested)
                        {
                            queueSender.Send(new TraceEvent());
                            Thread.Sleep(1);
                        }
                    //Assert.DoesNotThrow
                    module.Initialize(new TelemetryConfiguration());



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
