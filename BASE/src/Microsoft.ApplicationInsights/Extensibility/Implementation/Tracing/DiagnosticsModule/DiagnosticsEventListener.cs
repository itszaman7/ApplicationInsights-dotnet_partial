namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Linq;

    internal class DiagnosticsEventListener : EventListener
    {
        private const long AllKeyword = -1;
        private readonly EventLevel logLevel;
        private readonly DiagnosticsListener listener;
        private readonly List<EventSource> eventSourcesDuringConstruction = new List<EventSource>();

        public DiagnosticsEventListener(DiagnosticsListener listener, EventLevel logLevel)
            if (ShouldSubscribe(eventSource))
            {
                // If our constructor hasn't run yet (we're in a callback from the base class
                // constructor), just make a note of the event source. Otherwise logLevel is
                // set to the default, which is "LogAlways".
                var tmp = this.eventSourcesDuringConstruction;
                if (tmp != null)
                {
                    lock (tmp)
                    {
                        if (this.eventSourcesDuringConstruction != null)
                        {
                            this.eventSourcesDuringConstruction.Add(eventSource);
                            return;
                        }
                    }
                }

                this.EnableEvents(eventSource, this.logLevel, (EventKeywords)AllKeyword);
            }

            base.OnEventSourceCreated(eventSource);
        }

        /// <summary>
        /// This method checks if the given EventSource Name matches known EventSources that we want to subscribe to.
        /// </summary>
        private static bool ShouldSubscribe(EventSource eventSource)
        {
                    case "Redfield-Microsoft-ApplicationInsights-Extensibility-DependencyCollector":
                    case "Redfield-Microsoft-ApplicationInsights-Extensibility-EventCounterCollector":
                    case "Redfield-Microsoft-ApplicationInsights-Extensibility-PerformanceCollector":
                    case "Redfield-Microsoft-ApplicationInsights-Extensibility-PerformanceCollector-QuickPulse":
                    case "Redfield-Microsoft-ApplicationInsights-Extensibility-Web":
                    case "Redfield-Microsoft-ApplicationInsights-Extensibility-WindowsServer":
                    case "Redfield-Microsoft-ApplicationInsights-WindowsServer-Core":
                    case "Redfield-Microsoft-ApplicationInsights-Extensibility-EventSourceListener":
                    case "Redfield-Microsoft-ApplicationInsights-AspNetCore":
                    case "Redfield-Microsoft-ApplicationInsights-LoggerProvider":
                        return true;
                    default:
                        return false;
                }
            }

            if (eventSource.Name == "Microsoft-AspNet-Telemetry-Correlation")
            {
                return true;
            }
#else
            if (eventSource.Name.StartsWith("Microsoft-A", StringComparison.Ordinal))
            {
                switch (eventSource.Name)
                {
                    case "Microsoft-ApplicationInsights-Core": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/BASE/src/Microsoft.ApplicationInsights/Extensibility/Implementation/Tracing/CoreEventSource.cs
                    case "Microsoft-ApplicationInsights-WindowsServer-TelemetryChannel": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/BASE/src/ServerTelemetryChannel/Implementation/TelemetryChannelEventSource.cs

                    // AppMapCorrelation has a shared partial class: https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/WEB/Src/Common/AppMapCorrelationEventSource.cs
                    case "Microsoft-ApplicationInsights-Extensibility-AppMapCorrelation-Dependency": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/WEB/Src/DependencyCollector/DependencyCollector/Implementation/AppMapCorrelationEventSource.cs

                    case "Microsoft-ApplicationInsights-Extensibility-DependencyCollector": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/WEB/Src/DependencyCollector/DependencyCollector/Implementation/DependencyCollectorEventSource.cs
                    case "Microsoft-ApplicationInsights-Extensibility-EventCounterCollector": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/WEB/Src/EventCounterCollector/EventCounterCollector/EventCounterCollectorEventSource.cs
                    case "Microsoft-ApplicationInsights-Extensibility-PerformanceCollector": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/WEB/Src/PerformanceCollector/PerformanceCollector/Implementation/PerformanceCollectorEventSource.cs
                    case "Microsoft-ApplicationInsights-Extensibility-PerformanceCollector-QuickPulse": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/WEB/Src/PerformanceCollector/PerformanceCollector/Implementation/QuickPulse/QuickPulseEventSource.cs
                    case "Microsoft-ApplicationInsights-Extensibility-Web": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/WEB/Src/Web/Web/Implementation/WebEventSource.cs
                    case "Microsoft-ApplicationInsights-Extensibility-WindowsServer": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/WEB/Src/WindowsServer/WindowsServer/Implementation/WindowsServerEventSource.cs
                    case "Microsoft-ApplicationInsights-WindowsServer-Core": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/WEB/Src/WindowsServer/WindowsServer/Implementation/MetricManager.cs
                    case "Microsoft-ApplicationInsights-Extensibility-EventSourceListener": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/LOGGING/src/EventSource.Shared/EventSource.Shared/Implementation/EventSourceListenerEventSource.cs
                    case "Microsoft-ApplicationInsights-AspNetCore": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/NETCORE/src/Microsoft.ApplicationInsights.AspNetCore/Extensibility/Implementation/Tracing/AspNetCoreEventSource.cs


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
