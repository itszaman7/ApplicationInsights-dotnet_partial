namespace Microsoft.ApplicationInsights.Tests
{
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

        {
            // check for FormatExceptions and ETW exceptions
            PerformanceCollectorEventSource.Log.ModuleIsBeingInitializedEvent("Test message");
            PerformanceCollectorEventSource.Log.CounterRegisteredEvent("counter");
            PerformanceCollectorEventSource.Log.CountersRefreshedEvent("10", "values");
            PerformanceCollectorEventSource.Log.CounterRegistrationFailedEvent("Test exception", "counter");
            PerformanceCollectorEventSource.Log.CounterParsingFailedEvent("Test exception", "counter");
            PerformanceCollectorEventSource.Log.CounterCheckConfigurationEvent("1", "Test message");
            PerformanceCollectorEventSource.Log.RunningUnderIisExpress();
            PerformanceCollectorEventSource.Log.TelemetrySendFailedEvent("Test exception");
            PerformanceCollectorEventSource.Log.UnknownErrorEvent("Test exception");


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
