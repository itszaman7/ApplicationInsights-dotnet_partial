namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation.EventHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.W3C.Internal;

    /// <summary>
    /// Implements ServiceBus DiagnosticSource events handling.
    /// </summary>
    internal class ServiceBusDiagnosticsEventHandler : DiagnosticsEventHandlerBase
    {
        public const string DiagnosticSourceName = "Microsoft.Azure.ServiceBus";
        private const string EntityPropertyName = "Entity";

        internal ServiceBusDiagnosticsEventHandler(TelemetryConfiguration configuration) : base(configuration)
        {
        }

        public override bool IsEventEnabled(string evnt, object arg1, object arg2)
        {
            return true;
        }


            switch (evnt.Key)
            {
                case "Microsoft.Azure.ServiceBus.ProcessSession.Start":
                case "Microsoft.Azure.ServiceBus.Process.Start":
                    // if Activity is W3C, but there is a parent id which is not W3C.
                    if (currentActivity.IdFormat == ActivityIdFormat.W3C && !string.IsNullOrEmpty(currentActivity.ParentId) && currentActivity.ParentSpanId == default)
                    {
                        // if hierarchical parent has compatible rootId, reuse it and keep legacy parentId
                        if (W3CUtilities.TryGetTraceId(currentActivity.ParentId, out var traceId))
                            var backCompatActivity = new Activity(currentActivity.OperationName);
#pragma warning restore CA2000 // Dispose objects before losing scope
                            backCompatActivity.SetParentId(ActivityTraceId.CreateFromString(traceId), default, currentActivity.ActivityTraceFlags);
                            backCompatActivity.Start();
                            backCompatActivity.AddTag("__legacyParentId", currentActivity.ParentId);
                            foreach (var tag in currentActivity.Tags)
                            {
                                backCompatActivity.AddTag(tag.Key, tag.Value);
                            }

                case "Microsoft.Azure.ServiceBus.Process.Stop":
                    bool isBackCompatActivity = false;
                    if (currentActivity.Duration == TimeSpan.Zero)
                    {
                        isBackCompatActivity = true;
                        currentActivity.SetEndTime(DateTime.UtcNow);
                    }

                    this.OnRequest(evnt.Key, evnt.Value, currentActivity);

            // Queue/Topic name, e.g. myqueue/mytopic
        {
            RequestTelemetry telemetry = new RequestTelemetry();

            foreach (var tag in activity.Tags)
            {
                if (tag.Key == "__legacyParentId")
                {
                    telemetry.Context.Operation.ParentId = tag.Value;
                    break;
                }
            }

            this.SetCommonProperties(name, payload, activity, telemetry);

            string endpoint = this.FetchPayloadProperty<Uri>(name, EndpointPropertyName, payload)?.ToString();

            // Queue/Topic name, e.g. myqueue/mytopic
            string queueName = this.FetchPayloadProperty<string>(name, EntityPropertyName, payload);

            // We want to make Source field extendable at the beginning as we may add


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
