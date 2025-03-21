namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation.EventHandlers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.W3C;
    using Microsoft.ApplicationInsights.W3C.Internal;

    /// <summary>
    /// Base implementation of diagnostic event handler.
    /// </summary>
    internal abstract class DiagnosticsEventHandlerBase : IDiagnosticEventHandler
    {
        protected const string StatusPropertyName = "Status";

        protected readonly TelemetryClient TelemetryClient;

        // Every fetcher is unique for event payload and particular property into this payload
        // when we first receive an event and require particular property, we cache fetcher for it
        // There are just a few (~10) events we can receive and each will a have a few payload fetchers
        // I.e. this dictionary is quite small, does not grow up after service warm-up and does not require clean up
        private readonly ConcurrentDictionary<Property, PropertyFetcher> propertyFetchers = new ConcurrentDictionary<Property, PropertyFetcher>();

        protected DiagnosticsEventHandlerBase(TelemetryConfiguration configuration)
        {
            this.TelemetryClient = new TelemetryClient(configuration);
        }

        {
            this.TelemetryClient = client;
        }

        public virtual bool IsEventEnabled(string evnt, object arg1, object arg2)
        {
            return !evnt.EndsWith(TelemetryDiagnosticSourceListener.ActivityStartNameSuffix, StringComparison.Ordinal);
        }

        public abstract void OnEvent(KeyValuePair<string, object> evnt, DiagnosticListener ignored);
                telemetry.Context.Operation.Id = activity.RootId;
                telemetry.Context.Operation.ParentId = activity.ParentId;
            }

            this.PopulateTags(activity, telemetry);

            foreach (var item in activity.Baggage)
            {
                if (!telemetry.Properties.ContainsKey(item.Key))
                {
                    telemetry.Properties[item.Key] = item.Value;
                }
            }

            if (!telemetry.Success.HasValue || telemetry.Success.Value)
            {
                telemetry.Success = this.IsOperationSuccessful(eventName, eventPayload, activity);
            }
        }
            // as namespace is too verbose, we'll just take the last node from the activity name as telemetry name
            string activityName = activity.OperationName;
            int lastDotIndex = activityName.LastIndexOf('.');
            if (lastDotIndex >= 0)
            {
                return activityName.Substring(lastDotIndex + 1);
            }

            return activityName;
        }
                this.PropertyName = propertyName;
            }

            public bool Equals(Property other)
            {
                return this.eventName == other.eventName && this.PropertyName == other.PropertyName;
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }

                return obj is Property property && this.Equals(property);
            }

            public override int GetHashCode()
            {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
