namespace FunctionalTests.Utils
{
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using AI;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;
    using Xunit.Abstractions;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;

    public abstract class TelemetryTestsBase
    {
        public const int TestListenerTimeoutInMs = 10000;
        protected readonly ITestOutputHelper output;
        
        public TelemetryTestsBase(ITestOutputHelper output)
        {
            this.output = output;
        }
        public TelemetryItem<RequestData> ValidateRequestWithHeaders(InProcessServer server, string requestPath, Dictionary<string, string> requestHeaders, RequestTelemetry expected, bool expectRequestContextInResponse = true)
        {
            // Subtract 50 milliseconds to hack around strange behavior on build server where the RequestTelemetry.Timestamp is somehow sometimes earlier than now by a few milliseconds.
            expected.Timestamp = DateTimeOffset.Now.Subtract(TimeSpan.FromMilliseconds(50));
            Stopwatch timer = Stopwatch.StartNew();

            var response = this.ExecuteRequest(server.BaseHost + requestPath, requestHeaders);

            var actual = server.Listener.ReceiveItemsOfType<TelemetryItem<RequestData>>(1, TestListenerTimeoutInMs);
            timer.Stop();
            Assert.Equal(expected.Url, new Uri(data.url));
            Assert.Equal(expectRequestContextInResponse, response.Headers.Contains("Request-Context"));
            if (expectRequestContextInResponse)
            {
                Assert.True(response.Headers.TryGetValues("Request-Context", out var appIds));
                Assert.Equal($"appId={InProcessServer.AppId}", appIds.Single());
            }

            output.WriteLine("actual.Duration: " + data.duration);
            output.WriteLine("timer.Elapsed: " + timer.Elapsed);
            Assert.Equal(requestTelemetry.tags["ai.operation.id"], dependencyTelemetry.tags["ai.operation.id"]);
            Assert.Contains(dependencyTelemetry.data.baseData.id, requestTelemetry.tags["ai.operation.parentId"]);

            return (requestTelemetry, dependencyTelemetry);
        }

        public void ValidatePerformanceCountersAreCollected(string assemblyName)
        {
            using (var server = new InProcessServer(assemblyName, this.output))
            {
                var actual = server.Listener.ReceiveItems(TestListenerTimeoutInMs);
                this.DebugTelemetryItems(actual);
                Assert.True(actual.Length > 0);
            }
        }

        protected HttpResponseMessage ExecuteRequest(string requestPath, Dictionary<string, string> headers = null)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.UseDefaultCredentials = true;
                    {
                        request.Headers.Add(h.Key, h.Value);
                    }
                }

                var task = httpClient.SendAsync(request);
                task.Wait(TestListenerTimeoutInMs);
                this.output.WriteLine($"{DateTime.Now:MM/dd/yyyy hh:mm:ss.fff tt}: Ended request: {requestPath}");

                return task.Result;

        protected void DebugTelemetryItems(Envelope[] telemetries)
        {
            StringBuilder builder = new StringBuilder();
            foreach (Envelope telemetry in telemetries)
            {
                TelemetryItem<RemoteDependencyData> dependency = telemetry as TelemetryItem<RemoteDependencyData>;
                if (dependency != null)
                {                    
                }
                else
                {
                    TelemetryItem<RequestData> request = telemetry as TelemetryItem<RequestData>;
                    if (request != null)
                    {
                        var data = ((TelemetryItem<RequestData>)request).data.baseData;
                        builder.AppendLine($"{request} - {data.url} - {((TelemetryItem<RequestData>)request).time} - {data.duration} - {data.id} - {data.name} - {data.success} - {data.responseCode}");
                    }
                    else
                        {
                            var data = ((TelemetryItem<ExceptionData>)exception).data.baseData;
                            builder.AppendLine($"{exception} - {data.exceptions[0].message} - {data.exceptions[0].stack} - {data.exceptions[0].typeName} - {data.severityLevel}");
                        }
                        else
                        {
                            TelemetryItem<MessageData> message = telemetry as TelemetryItem<MessageData>;
                            if (message != null)
                            {
                                var data = ((TelemetryItem<MessageData>)message).data.baseData;
                                else
                                {
                                    builder.AppendLine($"{telemetry.ToString()} - {telemetry.time}");
                                }
                            }
                        }
                    }
                }
            }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
