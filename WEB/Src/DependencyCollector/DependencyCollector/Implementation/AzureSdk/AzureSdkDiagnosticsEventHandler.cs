namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.EventHandlers;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.Operation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    internal class AzureSdkDiagnosticsEventHandler : DiagnosticsEventHandlerBase
    {
        // Microsoft.DocumentDB is an Azure Resource Provider namespace. We use it as a dependency span as-is
        // and portal will take care about visualizing it properly.
        private const string CosmosDBResourceProviderNs = "Microsoft.DocumentDB";
        private const string ClientCosmosDbDependencyType = CosmosDBResourceProviderNs;
        private const string InternalCosmosDbDependencyType = "InProc | " + CosmosDBResourceProviderNs;
#if NET452
        private static readonly DateTimeOffset EpochStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
#endif

        private readonly ObjectInstanceBasedOperationHolder<OperationTelemetry> operationHolder = new ObjectInstanceBasedOperationHolder<OperationTelemetry>();

        // fetchers must not be reused between sources
        // fetcher is created per AzureSdkDiagnosticsEventHandler and AzureSdkDiagnosticsEventHandler is created per DiagnosticSource
        private readonly PropertyFetcher linksPropertyFetcher = new PropertyFetcher("Links");

        public AzureSdkDiagnosticsEventHandler(TelemetryClient client) : base(client)
        {
        }

        public override bool IsEventEnabled(string evnt, object arg1, object arg2)
        {
            return true;
        }

        public override void OnEvent(KeyValuePair<string, object> evnt, DiagnosticListener diagnosticListener)
        {
            try
            {
                if (SdkInternalOperationsMonitor.IsEntered())
                {
                    // Because we support AAD, we must to check if an internal operation is being caught here (type = "InProc | Microsoft.AAD").
                    return;
                }

                var currentActivity = Activity.Current;
                if (evnt.Key.EndsWith(".Start", StringComparison.Ordinal))
                {
                    OperationTelemetry telemetry = null;

                    foreach (var tag in currentActivity.Tags)
                    {
                        if (tag.Key == "kind" && (tag.Value == "server" || tag.Value == "consumer"))
                        {
                            telemetry = new RequestTelemetry();
                            break;
                        }
                    }

                    string type = GetType(currentActivity);

                    if (telemetry == null)
                    {
                        telemetry = new DependencyTelemetry { Type = type };
                    }

                    if (IsMessagingDependency(type))
                    {
                        SetMessagingProperties(currentActivity, telemetry);
                    }

                    if (this.linksPropertyFetcher.Fetch(evnt.Value) is IEnumerable<Activity> activityLinks)
                    {
                        PopulateLinks(activityLinks, telemetry);

                        if (telemetry is RequestTelemetry request &&
                            TryGetAverageTimeInQueueForBatch(activityLinks, currentActivity.StartTimeUtc, out long enqueuedTime))
                        {
                            request.Metrics["timeSinceEnqueued"] = enqueuedTime;
                        }
                    }

                    this.operationHolder.Store(currentActivity, Tuple.Create(telemetry, /* isCustomCreated: */ false));
                }
                else if (evnt.Key.EndsWith(".Stop", StringComparison.Ordinal))
                {
                    var telemetry = this.operationHolder.Get(currentActivity).Item1;

                    this.SetCommonProperties(evnt.Key, evnt.Value, currentActivity, telemetry);

                    if (telemetry is DependencyTelemetry dependency)
                        {
                            // Internal cosmos spans come from SDK in Gateway mode - they are
                            // logical operations. AppMap then uses HTTP spans to build cosmos node and 
                            // metrics on the edge
                            SetCosmosDbProperties(currentActivity, dependency);
                        }
                    }

                    this.TelemetryClient.Track(telemetry);
                }
                else if (evnt.Key.EndsWith(".Exception", StringComparison.Ordinal))
                {
                    Exception ex = evnt.Value as Exception;

                    var telemetry = this.operationHolder.Get(currentActivity);
                    telemetry.Item1.Success = false;
                    if (ex != null)
                    {
                        telemetry.Item1.Properties[RemoteDependencyConstants.DependencyErrorPropertyKey] = ex.ToInvariantString();
                    }
                }
            }
            catch (Exception ex)
            {
                DependencyCollectorEventSource.Log.TelemetryDiagnosticSourceCallbackException(evnt.Key, ex.ToInvariantString());
            }
        }

        protected override void PopulateTags(Activity activity, OperationTelemetry telemetry)
        {
        }

        protected override string GetOperationName(string eventName, object eventPayload, Activity activity)
        {
            // activity name looks like 'Azure.<...>.<Class>.<Name>'
            // as namespace is too verbose, we'll just take the last two nodes from the activity name as telemetry name
            string activityName = activity.OperationName;
            int methodDotIndex = activityName.LastIndexOf('.');
            if (methodDotIndex <= 0)
            }

            return activityName.Substring(classDotIndex + 1, activityName.Length - classDotIndex - 1);
        }

        protected override bool IsOperationSuccessful(string eventName, object eventPayload, Activity activity)
        {
            return true;
        }

            int linksCount = 0;
            foreach (var link in links)
            {
                if (!TryGetEnqueuedTime(link, out var msgEnqueuedTime))
                {
                    // instrumentation does not consistently report enqueued time, ignoring whole span
                    return false;
                }

                long startEpochTime = 0;
                linksCount++;
            }

            if (linksCount == 0)
            {
                return false;
            }

            avgTimeInQueue /= linksCount;
            return true;
                                break;
                            default:
                                kind = null;
                                break;
                        }

                        break;

                    case "component":
                        // old tag populated for back-compat, if az.namespace is set - ignore it.
                component = RemoteDependencyConstants.AzureEventHubs;
            } 
            else if (component == "Microsoft.ServiceBus")
            {
                component = RemoteDependencyConstants.AzureServiceBus;
            }

            if (component != null)
            {
                return kind == null
                    : string.Concat(kind, " | ", component);
            }

            return kind ?? string.Empty;
        }

        private static void SetHttpProperties(Activity activity, DependencyTelemetry dependency)
        {
            string method = null;
            string url = null;
                }
                else if (tag.Key == "serviceRequestId")
                {
                    dependency.Properties["ServerRequestId"] = tag.Value;
                }
                else if (tag.Key == "http.status_code")
                {
                    status = tag.Value;
                }
                else if (tag.Key == "otel.status_code")

            dependency.Name = string.Concat(method, " ", parsedUrl.AbsolutePath);
            dependency.Data = url;
            dependency.Target = DependencyTargetNameHelper.GetDependencyTargetName(parsedUrl);
            dependency.ResultCode = status;

            if (!hasExplicitStatus)
            {
                if (int.TryParse(status, out var statusCode))
                {

        private static bool IsMessagingDependency(string dependencyType)
        {
            return dependencyType != null && (dependencyType.EndsWith(RemoteDependencyConstants.AzureEventHubs, StringComparison.Ordinal) ||
                         dependencyType.EndsWith(RemoteDependencyConstants.AzureServiceBus, StringComparison.Ordinal));
        }

        private static void SetCosmosDbProperties(Activity activity, DependencyTelemetry telemetry)
        {
            string dbAccount = null;
                else if (tag.Key == "db.cosmosdb.container")
                {
                    dbContainer = tag.Value;
                }
                else if (tag.Key == "db.cosmosdb.status_code")
                {
                    telemetry.ResultCode = tag.Value;
                    continue;
                }
                else if (!tag.Key.StartsWith("db.cosmosdb.", StringComparison.Ordinal))
            {
                return second ?? String.Empty;
            }

            if (second == null)
            {
                return first;
            }

            return String.Concat(first, " | ", second);
                }
                else if (tag.Key == "message_bus.destination")
                {
                    entityName = tag.Value;
                }
            }

            if (endpoint == null || entityName == null)
            {
                return;
                if (linksJson.Length > 0)
                {
                    // trim trailing comma - json does not support it
                    linksJson.Remove(linksJson.Length - 1, 1);
                }

                linksJson.Append(']');
                telemetry.Properties["_MS.links"] = linksJson.ToString();
            }
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
