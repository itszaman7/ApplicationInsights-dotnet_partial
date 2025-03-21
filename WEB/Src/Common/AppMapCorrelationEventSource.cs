namespace Microsoft.ApplicationInsights.Common
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// ETW EventSource tracing class.
    /// </summary>
    //// [EventSource(Name = "Microsoft-ApplicationInsights-Extensibility-AppMapCorrelation")] - EVERY COMPONENT SHOULD DEFINE IT"S OWN NAME
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "appDomainName is required")]
    internal sealed partial class AppMapCorrelationEventSource : EventSource
    {
        public static readonly AppMapCorrelationEventSource Log = new AppMapCorrelationEventSource();
        private readonly ApplicationNameProvider applicationNameProvider = new ApplicationNameProvider();

        private AppMapCorrelationEventSource()
        {
        }
        }

        [Event(
            2,
            Keywords = Keywords.Diagnostics,
            Message = "Failed to add cross component correlation header. Error: {0}",
            Level = EventLevel.Warning)]
        public void SetCrossComponentCorrelationHeaderFailed(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(2, exception, this.applicationNameProvider.Name);
        }

        [Event(
            3,
            Keywords = Keywords.Diagnostics,
            Message = "Failed to determine cross component correlation header. Error: {0}",
            Level = EventLevel.Warning)]
        public void GetCrossComponentCorrelationHeaderFailed(string exception, string appDomainName = "Incorrect")
        }

        [Event(
            4,
            Keywords = Keywords.Diagnostics,
            Message = "Failed to determine role name header. Error: {0}",
            Level = EventLevel.Warning)]
        public void GetComponentRoleNameHeaderFailed(string exception, string appDomainName = "Incorrect")
            5,
            Keywords = Keywords.Diagnostics,
            Message = "Unknown error occurred.",
            Level = EventLevel.Warning)]
        public void UnknownError(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(5, exception, this.applicationNameProvider.Name);
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
