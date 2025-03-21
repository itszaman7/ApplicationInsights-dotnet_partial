using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.ApplicationInsights.WorkerService.Tests
{
    public class FunctionalTests
    {
        protected readonly ITestOutputHelper output;

        public FunctionalTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact(Skip = "Temporarily skipping. This fails when run inside same process as DepCollector ignores the 2nd host on the same process. validated locally.")]
        public void BasicCollectionTest()
        {
            ConcurrentBag<ITelemetry> sentItems = new ConcurrentBag<ITelemetry>();

            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ITelemetryChannel>(new StubChannel()
            // The worker would have completed 5 loops in 5 sec,
            // each look making dependency call and some ilogger logs,
            // inside "myoperation"
            Assert.True(sentItems.Count > 0);
            PrintItems(sentItems);

            // Validate
            var reqs = GetTelemetryOfType<RequestTelemetry>(sentItems);
            Assert.True(reqs.Count >= 1);
            var traces = GetTelemetryOfType<TraceTelemetry>(sentItems);
            Assert.True(traces.Count >= 1);
            var deps = GetTelemetryOfType<DependencyTelemetry>(sentItems);
            Assert.True(deps.Count >= 1);

            // Pick one RequestTelemetry and validate that trace/deps are found which are child of the parent request.
            var reqOperationId = reqs[0].Context.Operation.Id;
            var reqId = reqs[0].Id;
            var trace = traces.Find((tr) => tr.Context.Operation.Id != null && tr.Context.Operation.Id.Equals(reqOperationId));
            Assert.NotNull(trace);
            trace = traces.Find((tr) => tr.Context.Operation.ParentId != null && tr.Context.Operation.ParentId.Equals(reqId));
                    foundItems.Add((T)item);
                }
            }

            return foundItems;
        }

        private void PrintItems(ConcurrentBag<ITelemetry> items)
        {
            int i = 1;
                    this.output.WriteLine("RequestTelemetry");
                    this.output.WriteLine(req.Name);
                    this.output.WriteLine(req.Id);
                    PrintOperation(item);
                    this.output.WriteLine(req.Duration.ToString());
                }
                else if (item is DependencyTelemetry dep)
                    this.output.WriteLine(dep.Data);
                    PrintOperation(item);
                }
                else if (item is TraceTelemetry trace)
                {
                    this.output.WriteLine("TraceTelemetry");
                    this.output.WriteLine(trace.Message);
                    PrintOperation(item);
                }
                else if (item is ExceptionTelemetry exc)
                    this.output.WriteLine(exc.Message);
                    PrintOperation(item);
                }
                else if (item is MetricTelemetry met)
                {
                    this.output.WriteLine("MetricTelemetry");
                    this.output.WriteLine(met.Name + "" + met.Sum);
                    PrintOperation(item);
                }

        }

        private void PrintProperties(ISupportProperties itemProps)
        {
            foreach (var prop in itemProps.Properties)
            {
                this.output.WriteLine(prop.Key + ":" + prop.Value);
            }
        }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
