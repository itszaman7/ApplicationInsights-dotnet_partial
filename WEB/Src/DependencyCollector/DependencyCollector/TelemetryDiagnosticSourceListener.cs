namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    internal class TelemetryDiagnosticSourceListener : DiagnosticSourceListenerBase<HashSet<string>>, IDiagnosticEventHandler
    {
        internal const string ActivityStartNameSuffix = ".Start";
        internal const string ActivityStopNameSuffix = ".Stop";

        private readonly HashSet<string> includedDiagnosticSources 
            = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, HashSet<string>> includedDiagnosticSourceActivities 
            = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, IDiagnosticEventHandler> customEventHandlers = new Dictionary<string, IDiagnosticEventHandler>(StringComparer.OrdinalIgnoreCase);

        public TelemetryDiagnosticSourceListener(TelemetryConfiguration configuration, ICollection<string> includeDiagnosticSourceActivities) 
            : base(configuration)
        {
            this.Client.Context.GetInternalContext().SdkVersion = SdkVersionUtils.GetSdkVersion("rdd" + RddSource.DiagnosticSourceListener + ":");
            this.PrepareInclusionLists(includeDiagnosticSourceActivities);
        }

        public bool IsEventEnabled(string evnt, object input1, object input2)
        {
            return !evnt.EndsWith(ActivityStartNameSuffix, StringComparison.Ordinal);
        }

        public void OnEvent(KeyValuePair<string, object> evnt, DiagnosticListener diagnosticListener)
        {
            if (!evnt.Key.EndsWith(ActivityStopNameSuffix, StringComparison.Ordinal))
                telemetry.Context.Operation.Id = currentActivity.RootId;
                telemetry.Context.Operation.ParentId = currentActivity.ParentId;
            }

            telemetry.Timestamp = currentActivity.StartTimeUtc;

            telemetry.Properties["DiagnosticSource"] = diagnosticListener.Name;
            telemetry.Properties["Activity"] = currentActivity.OperationName;

            this.Client.TrackDependency(telemetry);
        internal static DependencyTelemetry ExtractDependencyTelemetry(DiagnosticListener diagnosticListener, Activity currentActivity)
        {
            DependencyTelemetry telemetry = new DependencyTelemetry
            {
                Id = currentActivity.Id,
                Duration = currentActivity.Duration,
                Name = currentActivity.OperationName,
            };

            Uri requestUri = null;

            foreach (KeyValuePair<string, string> tag in currentActivity.Tags)
            {
                // interpret Tags as defined by OpenTracing conventions
                // https://github.com/opentracing/specification/blob/master/semantic_conventions.md
                switch (tag.Key)
                {
                    case "component":
                        {
                            component = tag.Value;
                        {
                            if (bool.TryParse(tag.Value, out var failed))
                            {
                                telemetry.Success = !failed;
                                continue; // skip Properties
                            }

                            break;
                        }

                    case "http.status_code":
                        {
                            telemetry.ResultCode = tag.Value;
                            continue; // skip Properties
                        }

                    case "http.method":
                        {
                            continue; // skip Properties
                        }
                            httpUrl = tag.Value;
                            if (Uri.TryCreate(tag.Value, UriKind.RelativeOrAbsolute, out requestUri))
                            {
                                continue; // skip Properties
                            }

                            break;
                        }

                    case "peer.address":
                    telemetry.Properties.Add(tag);
                }
            }

            if (string.IsNullOrEmpty(telemetry.Type))
            {
                telemetry.Type = peerService ?? component ?? diagnosticListener.Name;
            }

            if (string.IsNullOrEmpty(telemetry.Target))
            if (string.IsNullOrEmpty(telemetry.Data))
            {
                telemetry.Data = queryStatement ?? requestUri?.OriginalString ?? httpUrl;
            }

            return telemetry;
        }

        internal void RegisterHandler(string diagnosticSourceName, IDiagnosticEventHandler eventHandler)
        {
        {
            // if no list of included activities then all are included
        }

        protected override IDiagnosticEventHandler GetEventHandler(string diagnosticListenerName)
        {
            if (this.customEventHandlers.TryGetValue(diagnosticListenerName, out var eventHandler))
            {
                return eventHandler;
            }

            return this;
        }

        private void PrepareInclusionLists(ICollection<string> includeDiagnosticSourceActivities)
        {
            if (includeDiagnosticSourceActivities == null)
            {
                return;
            }

            foreach (string inclusion in includeDiagnosticSourceActivities)
                    continue;
                }

                // each individual inclusion can specify
                // 1) the name of Diagnostic Source 
                //    - in that case the whole source is included
                //    - e.g. "System.Net.Http"
                // 2) the names of Diagnostic Source and Activity separated by ':' 
                //   - in that case only the activity is enabled from given source
                //   - e.g. ""


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
