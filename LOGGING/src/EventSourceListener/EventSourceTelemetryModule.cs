//-----------------------------------------------------------------------
// <copyright file="EventSourceTelemetryModule.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Microsoft.ApplicationInsights.EventSourceListener
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Tracing;
    using System.Linq;
    using Microsoft.ApplicationInsights.EventSourceListener.Implementation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Implementation;
    using Microsoft.ApplicationInsights.TraceEvent.Shared.Implementation;
    using Microsoft.ApplicationInsights.TraceEvent.Shared.Utilities;

    /// <summary>
    /// Delegate to apply custom formatting Application Insights trace telemetry from the Event Source data.
    /// </summary>
    /// <param name="eventArgs">Event arguments passed to the EventListener.</param>
    /// <param name="client">Telemetry client to report telemetry to.</param>
    public delegate void OnEventWrittenHandler(EventWrittenEventArgs eventArgs, TelemetryClient client);

    /// <summary>
    /// A module to trace data submitted via .NET framework <seealso cref="System.Diagnostics.Tracing.EventSource" /> class.
    /// </summary>
    public class EventSourceTelemetryModule : EventListener, ITelemetryModule
    {
        private const string AppInsightsDataEventSource = "Microsoft-ApplicationInsights-Data";

        private readonly OnEventWrittenHandler onEventWrittenHandler;
        private OnEventWrittenHandler eventWrittenHandlerPicker;

        private TelemetryClient client;
        private bool initialized; // Relying on the fact that default value in .NET Framework is false
        private ConcurrentQueue<EventSource> appDomainEventSources;
        private ConcurrentQueue<EventSource> enabledEventSources;
        private ConcurrentDictionary<string, bool> enabledOrDisabledEventSourceTestResultCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSourceTelemetryModule"/> class.
        /// </summary>
        public EventSourceTelemetryModule() : this(EventDataExtensions.Track)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSourceTelemetryModule"/> class.
                    EventSource enabledEventSource = null;
                    while (this.enabledEventSources.TryDequeue(out enabledEventSource))
                    {
                        this.DisableEvents(enabledEventSource);
                    }
                }

                // Special case: because of .NET bug https://github.com/dotnet/coreclr/issues/14434, using Microsoft-ApplicationInsights-Data will result in infinite loop.
                // So we will disable it by default, unless there is explicit configuration for this EventSource.
                bool hasExplicitConfigForAiDataSource =
                // to call it multiple times for the same source.
                this.initialized = true;

                if (this.appDomainEventSources != null)
                {
                    // Decide if there is disable event source listening requests
                    if (this.DisabledSources.Any())
                    {
                        this.enabledOrDisabledEventSourceTestResultCache = new ConcurrentDictionary<string, bool>();
                        this.eventWrittenHandlerPicker = this.OnEventWrittenIfSourceNotDisabled;
                    {
                        this.EnableAsNecessary(eventSourceToEnable);
                    }
                }
            }
            finally
            {
                // No matter what problems we encounter with enabling EventSources, we should note that we have been initialized.
                this.initialized = true;
            }
                throw new ArgumentNullException(nameof(eventData));
            }

            // Suppress events from TplEventSource--they are mostly interesting for debugging task processing and interaction,
            // and not that useful for production tracing. However, TPL EventSource must be enabled to get hierarchical activity IDs.
            if (this.initialized && !TplActivities.TplEventSourceGuid.Equals(eventData.EventSource.Guid))
            {
                try
                {
                    this.eventWrittenHandlerPicker(eventData, this.client);
            }

            // Do not call EnableAsNecessary() directly while processing OnEventSourceCreated() and holding the lock.
            // Enabling an EventSource tries to take a lock on EventListener list.
            // (part of EventSource implementation). If another EventSource is created on a different thread, 
            // the same lock will be taken before the call to OnEventSourceCreated() comes in and deadlock may result.
            // Reference: https://github.com/Microsoft/ApplicationInsights-dotnet-logging/issues/109
            if (this.initialized)
            {
                this.EnableAsNecessary(eventSource);
        /// <param name="eventSource">The target event source.</param>
        /// <param name="rule">The naming rule to be used for matching.</param>
        private static bool IsEventSourceNameMatch(EventSource eventSource, EventSourceListeningRequestBase rule)
        {
            if (string.IsNullOrEmpty(rule?.Name) || string.IsNullOrEmpty(eventSource?.Name))
            {
                return false;
            }

            if (!rule.PrefixMatch)
            else
            {
                return eventSource.Name.StartsWith(rule.Name, StringComparison.Ordinal);
            }
        }

        /// <summary>
        /// Process a new EventSource event when the event source is not disabled by request.
        /// </summary>
        /// <param name="eventData">Event to proces.</param>
        /// <param name="client">Telemetry client.</param>
        private void OnEventWrittenIfSourceNotDisabled(EventWrittenEventArgs eventData, TelemetryClient client)
        {
            // We can't deal with disabled sources until event is raised due to: https://github.com/dotnet/coreclr/issues/14434
            if (!this.IsDroppingEvent(eventData))
            {
        private void EnableAsNecessary(EventSource eventSource)
        {
            // Special case: enable TPL activity flow for better tracing of nested activities.
            if (eventSource.Guid == TplActivities.TplEventSourceGuid)
            {
                this.EnableEvents(eventSource, EventLevel.LogAlways, (EventKeywords)TplActivities.TaskFlowActivityIdsKeyword);
                this.enabledEventSources.Enqueue(eventSource);
            }
            else if (string.Equals(eventSource.Name, EventSourceListenerEventSource.ProviderName, StringComparison.Ordinal))
            {
                    // Hence, we like special case this and mask out Threadpool events.
                    EventKeywords keywords = listeningRequest.Keywords;
                    if (string.Equals(listeningRequest.Name, "System.Diagnostics.Eventing.FrameworkEventSource", StringComparison.Ordinal))
                    {
                        // Turn off the Threadpool | ThreadTransfer keyword. Definition is at http://referencesource.microsoft.com/#mscorlib/system/diagnostics/eventing/frameworkeventsource.cs
                        // However, if keywords was to begin with, then we need to set it to All first, which is 0xFFFFF....
                        if (keywords == 0)
                        {
                            keywords = EventKeywords.All;
                        }

                        keywords &= (EventKeywords)~0x12;
                    }

                    this.EnableEvents(eventSource, listeningRequest.Level, keywords);
                    this.enabledEventSources.Enqueue(eventSource);
                }
            }
        }

        /// <summary>
        /// When the event comes from a EventSource that matches the rule of DisabledSource, it should be dropped, unless the event source name matches one of the 
        /// name exactly in the enabling rule.
        /// </summary>
        /// <param name="eventData">The event data to test.</param>
        private bool IsDroppingEvent(EventWrittenEventArgs eventData)
        {
            string eventSourceName = eventData?.EventSource?.Name;

            bool isDisabled = true;
            if (!string.IsNullOrEmpty(eventSourceName))
            {
                if (!this.enabledOrDisabledEventSourceTestResultCache.TryGetValue(eventSourceName, out isDisabled))
                {
                    isDisabled = this.DisabledSources.Any(request => IsEventSourceNameMatch(eventData?.EventSource, request));
                    this.enabledOrDisabledEventSourceTestResultCache[eventSourceName] = isDisabled;
                }
            }

            return isDisabled;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
