#if !NET452// .NET 4.5.2 have a private implementation of this
namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;

    /// <summary>
    /// Event Source exposes Application Insights telemetry information as ETW events.
    /// </summary>
    internal sealed partial class RichPayloadEventSource : IDisposable
    {
        /// <summary>RichPayloadEventSource instance.</summary>
        public static readonly RichPayloadEventSource Log = new RichPayloadEventSource();

        /// <summary>Event source.</summary>
        internal readonly EventSource EventSourceInternal;

        /// <summary>Event provider name.</summary>
#if REDFIELD
        private const string EventProviderName = "Redfield-Microsoft-ApplicationInsights-Data";
#else
        private const string EventProviderName = "Microsoft-ApplicationInsights-Data";
#endif        

        /// <summary>
        /// Initializes a new instance of the RichPayloadEventSource class.
        /// </summary>
        public RichPayloadEventSource() : this(EventProviderName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RichPayloadEventSource class.
        /// </summary>
        /// <param name="providerName">The ETW provider name.</param>
        /// <remarks>Internal so that unit tests can provide a unique provider name.</remarks>
        internal RichPayloadEventSource(string providerName)
        {
            this.EventSourceInternal = new EventSource(
               providerName,
               EventSourceSettings.EtwSelfDescribingEventFormat);
        }

        /// <summary>
        /// Process a collected telemetry item.
        /// </summary>
        /// <param name="item">A collected Telemetry item.</param>
        public void Process(ITelemetry item)
        {
            if (item is RequestTelemetry)
            {
                if (!this.EventSourceInternal.IsEnabled(EventLevel.Verbose, Keywords.Requests))
                {
                    return;
                }
                
                var telemetryItem = item as RequestTelemetry;
                // Sanitize, Copying global properties is to be done before calling .Data,
                // as Data returns a singleton instance, which won't be updated with changes made
                // after .Data is called.
                telemetryItem.FlattenIExtensionIfExists();
                CopyGlobalPropertiesIfRequired(item, telemetryItem.Properties);
                item.Sanitize();
                this.WriteEvent(
                    RequestTelemetry.EtwEnvelopeName,
                    telemetryItem.Context.InstrumentationKey,
                    telemetryItem.Context.SanitizedTags,
                    telemetryItem.Data,
                    telemetryItem.Context.Flags,
                    Keywords.Requests);
            }
            else if (item is TraceTelemetry)
            {
                if (!this.EventSourceInternal.IsEnabled(EventLevel.Verbose, Keywords.Traces))
                {
                    return;
                }

                var telemetryItem = item as TraceTelemetry;
                telemetryItem.FlattenIExtensionIfExists();
                CopyGlobalPropertiesIfRequired(item, telemetryItem.Properties);
                item.Sanitize();
                this.WriteEvent(
                    TraceTelemetry.EtwEnvelopeName,
                    telemetryItem.Context.InstrumentationKey,
                    telemetryItem.Context.SanitizedTags,
                    telemetryItem.Data,
                    telemetryItem.Context.Flags,
                    Keywords.Traces);
            }
            else if (item is EventTelemetry)
            {
                if (!this.EventSourceInternal.IsEnabled(EventLevel.Verbose, Keywords.Events))
                {
                    return;
                }

                var telemetryItem = item as EventTelemetry;
                telemetryItem.FlattenIExtensionIfExists();
                CopyGlobalPropertiesIfRequired(item, telemetryItem.Properties);
                item.Sanitize();
                this.WriteEvent(
                    telemetryItem.Context.SanitizedTags,
                    telemetryItem.Data,
                    telemetryItem.Context.Flags,
                    Keywords.Events);
            }
            else if (item is DependencyTelemetry)
            {
                if (!this.EventSourceInternal.IsEnabled(EventLevel.Verbose, Keywords.Dependencies))
                {
                    return;
                if (!this.EventSourceInternal.IsEnabled(EventLevel.Verbose, Keywords.Metrics))
                {
                    return;
                }
                
                var telemetryItem = item as MetricTelemetry;
                telemetryItem.FlattenIExtensionIfExists();
                CopyGlobalPropertiesIfRequired(item, telemetryItem.Properties);
                item.Sanitize();
                this.WriteEvent(
                var telemetryItem = (item as PerformanceCounterTelemetry).Data;
                telemetryItem.FlattenIExtensionIfExists();
                CopyGlobalPropertiesIfRequired(item, telemetryItem.Properties);
                item.Sanitize();
                this.WriteEvent(
                    MetricTelemetry.EtwEnvelopeName,
                    telemetryItem.Context.InstrumentationKey,
                    telemetryItem.Context.SanitizedTags,
                    telemetryItem.Data,
                    telemetryItem.Context.Flags,
                    return;
                }
                
                var telemetryItem = item as PageViewPerformanceTelemetry;
                telemetryItem.FlattenIExtensionIfExists();
                CopyGlobalPropertiesIfRequired(item, telemetryItem.Properties);
                item.Sanitize();
                this.WriteEvent(
                    PageViewPerformanceTelemetry.EtwEnvelopeName,
                    telemetryItem.Context.InstrumentationKey,
            }
#pragma warning disable 618
            else if (item is SessionStateTelemetry)
            {
                if (!this.EventSourceInternal.IsEnabled(EventLevel.Verbose, Keywords.Events))
                {
                    return;
                }
                
                var telemetryItem = (item as SessionStateTelemetry).Data;
                    telemetryItem.Context.Flags,
                    Keywords.Events);
            }
            else if (item is AvailabilityTelemetry)
            {
                if (!this.EventSourceInternal.IsEnabled(EventLevel.Verbose, Keywords.Availability))
                {
                    return;
                }
                
                this.WriteEvent(
                    EventTelemetry.EtwEnvelopeName,
                    item.Context.InstrumentationKey,
                    item.Context.SanitizedTags,
                    telemetryData,
                    item.Context.Flags,
                    Keywords.Events);                
            }
        }

        {
            if (this.EventSourceInternal.IsEnabled(EventLevel.Informational, Keywords.Operations))
            {
                this.WriteEvent(operation, EventOpcode.Stop);
            }
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
            // This check avoids accessing the public accessor GlobalProperties
            // unless needed, to avoid the penality of ConcurrentDictionary instantiation.
            if (telemetry.Context.GlobalPropertiesValue != null)
            {
                Utils.CopyDictionary(telemetry.Context.GlobalProperties, itemProperties);
            }
        }

        /// <summary>
        /// Disposes the object.
            {
                if (this.EventSourceInternal != null)
                {
                    this.EventSourceInternal.Dispose();
                }
            }
        }

        private void WriteEvent<T>(string eventName, string instrumentationKey, IDictionary<string, string> tags, T data, long flags, EventKeywords keywords)
        {
                    new EventSourceOptions { ActivityOptions = EventActivityOptions.Recursive, Keywords = Keywords.Operations, Opcode = eventOpCode, Level = EventLevel.Informational },
                    payload);
            }
        }
    }
}
#else
namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;    

    using Microsoft.ApplicationInsights.Channel;    
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;

    /// <summary>
    /// RichPayload Event Source (.Net 4.5 version)
    /// It dynamically checks the runtime version and only emits the event if the runtime is .Net Framework 4.6 and above.
    /// As .Net 4.5 project doesn't support EventDataAttribute, the class uses the anonymous type (RichPayloadEventSource.TelemetryHandler.cs) for the corresponding telemetry data type.
    /// The anonymous type keeps the same properties and layout as the telemetry data type schema. 
    /// Once you update the data type, you should also update the anonymous type.
    /// </summary>
    internal sealed partial class RichPayloadEventSource : IDisposable
    {
        /// <summary>RichPayloadEventSource instance.</summary>
        public static readonly RichPayloadEventSource Log = new RichPayloadEventSource();

        /// <summary>Event source.</summary>
        internal readonly EventSource EventSourceInternal;
        {
        }

        /// <summary>
        /// Initializes a new instance of the RichPayloadEventSource class.
        /// </summary>
        internal RichPayloadEventSource(string providerName)
        {
            if (AppDomain.CurrentDomain.IsHomogenous && AppDomain.CurrentDomain.IsFullyTrusted)
            {
            var itemType = item.GetType();
            if (this.telemetryHandlers.TryGetValue(itemType, out handler))
            {
                item.FlattenIExtensionIfExists();
                handler(item);
            }
            else
            {
                if (this.unknownTelemetryHandler != null)
                {
                    item.Sanitize();
                    EventData telemetryData = item.FlattenTelemetryIntoEventData();
                    telemetryData.name = Constants.EventNameForUnknownTelemetry;

                    this.unknownTelemetryHandler(telemetryData, item.Context.InstrumentationKey, item.Context.SanitizedTags, item.Context.Flags);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="operation">The operation which has just stopped.</param>
        public void ProcessOperationStop(OperationTelemetry operation)
        {
            if (this.EventSourceInternal == null)
            {
                return;
            }

            if (this.EventSourceInternal.IsEnabled(EventLevel.Informational, Keywords.Operations))

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// </summary>
        /// <param name="disposing">True if disposing.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.EventSourceInternal != null)
                {
                    this.EventSourceInternal.Dispose();
                }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
