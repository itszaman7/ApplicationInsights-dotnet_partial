namespace Microsoft.ApplicationInsights.TestFramework
{
    using System;
    /// </summary>
    internal class MockProcessorModule : ITelemetryProcessor, ITelemetryModule
        public bool ModuleInitialized { get; private set; } = false;

        public void Initialize(TelemetryConfiguration configuration) => this.ModuleInitialized = true;



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
