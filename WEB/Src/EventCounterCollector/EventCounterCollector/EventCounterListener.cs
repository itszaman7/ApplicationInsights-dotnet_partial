namespace Microsoft.ApplicationInsights.Extensibility.EventCounterCollector.Implementation
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.Text;
    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// Implementation to listen to EventCounters.
    /// </summary>
    internal class EventCounterListener : EventListener
    {
        private static readonly object LockObj = new object();
        private readonly string refreshIntervalInSecs;
        private readonly int refreshInternalInSecInt;
        private readonly EventLevel level = EventLevel.Critical;
        private bool isInitialized = false;
        private bool useEventSourceNameAsMetricsNamespace = false;
        private TelemetryClient telemetryClient;
        private Dictionary<string, string> refreshIntervalDictionary;

        // Thread-safe variable to hold the list of all EventSourcesCreated.
        // This class may not be instantiated at the time of EventSource creation, so the list of EventSources should be stored to be enabled after initialization.
        private ConcurrentQueue<EventSource> allEventSourcesCreated;

        // EventSourceNames from which counters are to be collected are the keys for this IDictionary.
        // The value will be the corresponding ICollection of counter names.
        private IDictionary<string, ICollection<string>> countersToCollect = new Dictionary<string, ICollection<string>>();

        public EventCounterListener(TelemetryClient telemetryClient, IList<EventCounterCollectionRequest> eventCounterCollectionRequests, int refreshIntervalSecs, bool useEventSourceNameAsMetricsNamespace)
        {
            try
            {
                this.refreshInternalInSecInt = refreshIntervalSecs;
                    if (!this.countersToCollect.ContainsKey(collectionRequest.EventSourceName))
                    {
                        this.countersToCollect.Add(collectionRequest.EventSourceName, new HashSet<string>() { collectionRequest.EventCounterName });
                    }
                    else
                    {
                        this.countersToCollect[collectionRequest.EventSourceName].Add(collectionRequest.EventCounterName);
                    }
                }

                EventCounterCollectorEventSource.Log.EventCounterInitializeSuccess();
                this.isInitialized = true;

                // Go over every EventSource created before we finished initialization, and enable if required.
                // This will take care of all EventSources created before initialization was done.
                foreach (var eventSource in this.allEventSourcesCreated)
                {
                    this.EnableIfRequired(eventSource);
                }
            }
            catch (Exception ex)
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            // Keeping track of all EventSources here, as this call may happen before initialization.
            lock (LockObj)
            {
                if (this.allEventSourcesCreated == null)
                {
                    this.allEventSourcesCreated = new ConcurrentQueue<EventSource>();
                }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            // Ignore events if initialization not done yet. We may lose the 1st event if it happens before initialization, in multi-thread situations.
            // Since these are counters, losing the 1st event will not have noticeable impact.
            if (this.isInitialized)
            {
                if (this.countersToCollect.ContainsKey(eventData.EventSource.Name))
                {
                    IDictionary<string, object> eventPayload = eventData.Payload[0] as IDictionary<string, object>;
                    if (eventPayload != null)
                        this.ExtractAndPostMetric(eventData.EventSource.Name, eventPayload);
                    }
                    else
                    {
                        EventCounterCollectorEventSource.Log.IgnoreEventWrittenAsEventPayloadNotParseable(eventData.EventSource.Name);
                    }
                }
                else
                {
                    EventCounterCollectorEventSource.Log.IgnoreEventWrittenAsEventSourceNotInConfiguredList(eventData.EventSource.Name);
                }
            }
            else
            {
                EventCounterCollectorEventSource.Log.IgnoreEventWrittenAsNotInitialized(eventData.EventSource.Name);
            }
        }

        private void EnableIfRequired(EventSource eventSource)
        {
            try
            {
                // The EventSourceName is in the list we want to collect some counters from.
                if (this.countersToCollect.ContainsKey(eventSource.Name))
                {
                    // Unlike regular Events, the only relevant parameter here for EventCounter is the dictionary containing EventCounterIntervalSec.
                    this.EnableEvents(eventSource, this.level, (EventKeywords)(-1), this.refreshIntervalDictionary);

                    EventCounterCollectorEventSource.Log.EnabledEventSource(eventSource.Name);
                }
                else
                {
                    EventCounterCollectorEventSource.Log.NotEnabledEventSource(eventSource.Name);
                }
            }
            catch (Exception ex)
            {
                EventCounterCollectorEventSource.Log.EventCounterCollectorError("EventCounterListener EnableEventSource", ex.Message);
            }
        }
                double actualValue = 0.0;
                double actualInterval = 0.0;
                int actualCount = 0;
                string counterName = string.Empty;
                string counterDisplayName = string.Empty;
                string counterDisplayUnit = string.Empty;
                foreach (KeyValuePair<string, object> payload in eventPayload)
                {
                    var key = payload.Key;
                    if (key.Equals("Name", StringComparison.OrdinalIgnoreCase))
                        }
                    }
                    else if (key.Equals("DisplayName", StringComparison.OrdinalIgnoreCase))
                    {
                        counterDisplayName = payload.Value.ToString();
                    }
                    else if (key.Equals("DisplayUnits", StringComparison.OrdinalIgnoreCase))
                    {
                        counterDisplayUnit = payload.Value.ToString();
                    }
                            var keyValuePairStrings = metadata.Split(',');
                            foreach (var keyValuePairString in keyValuePairStrings)
                            {
                                var keyValuePair = keyValuePairString.Split(':');
                                if (!metricTelemetry.Properties.ContainsKey(keyValuePair[0]))
                                {
                                    metricTelemetry.Properties.Add(keyValuePair[0], keyValuePair[1]);
                                }
                            }
                        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
