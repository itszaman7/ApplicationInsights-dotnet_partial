namespace Microsoft.ApplicationInsights
{
    using System.Diagnostics;
    using System.IO;
    
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.DataContracts;

    [TestClass]
    public class AppInsightsStandaloneTests
    {
        private string tempPath;

        [TestInitialize]
        public void Initialize()
        {
            this.tempPath = Path.Combine(Path.GetTempPath(), "ApplicationInsightsTests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(this.tempPath);
            File.Copy("Microsoft.ApplicationInsights.dll", $"{tempPath}\\Microsoft.ApplicationInsights.dll", true);
            File.Delete($"{tempPath}\\System.Diagnostics.DiagnosticSource.dll");
        }

        [TestCleanup]
        public void Cleanup()
        {
            Directory.Delete(this.tempPath, true);
        }
        [TestMethod]
        public void AppInsightsUsesActivityWhenDiagnosticSourceIsAvailableNonW3C()
        {
            try
            {
                // Regular use case - System.DiagnosticSource is available. Regular unit test can cover this scenario.
                var config = new TelemetryConfiguration();
                DisableW3CFormatInActivity();
                config.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
                var tc = new TelemetryClient(config);
                using (var requestOperation = tc.StartOperation<RequestTelemetry>("request", "guid"))
                {
                    using (var dependencyOperation = tc.StartOperation<DependencyTelemetry>("dependency", "guid"))
                    {
                        Assert.IsTrue(dependencyOperation.Telemetry.Id.StartsWith("|guid."));
                        tc.TrackTrace("Hello World!");
                    }
                }
            }
            finally
                    // "guid" is not w3c compatible. Ignored
                    Assert.IsFalse(dependencyOperation.Telemetry.Id.StartsWith("|guid."));
                    // but "guid" will be stored in custom properties
                    Assert.AreEqual("guid",dependencyOperation.Telemetry.Properties["ai_legacyRootId"]);
                    tc.TrackTrace("Hello World!");
                }
            }
        }

        private string RunTestApplication(string operationId)
        {
            var fileName = $"{this.tempPath}\\ActivityTest.exe";

            Assert.IsTrue(CreateTestApplication(fileName), "Failed to create a test application. See console output for details.");

            Process p = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
           

            return p.StandardOutput.ReadToEnd();
        }

        private static void DisableW3CFormatInActivity()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;
        }
        {
                class ActivityTest
                {
                    static void Main(string[] args)
                    {
                        var config = new TelemetryConfiguration();
                        config.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
                        var tc = new TelemetryClient(config);
                        using (var requestOperation = tc.StartOperation<RequestTelemetry>(""request"", args[0]))
                        {
                            using (var dependencyOperation = tc.StartOperation<DependencyTelemetry>(""dependency"", args[0]))
                options: new CSharpCompilationOptions(OutputKind.ConsoleApplication));

            var emitResult = compilation.Emit(fileName);
            if (emitResult.Success)
            {
                return true;
            }
            else
            {
                foreach(var d in emitResult.Diagnostics)


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
