using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using AI;
using FunctionalTests.Utils;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;

using Xunit;
using Xunit.Abstractions;

namespace FunctionalTests.WebApi.Tests.FunctionalTest
{
    public class MultipleWebHostsTests : TelemetryTestsBase
    {
        private readonly string assemblyName;
        private const string requestPath = "/api/dependency";

        public MultipleWebHostsTests(ITestOutputHelper output) : base(output)
        {
            this.assemblyName = this.GetType().GetTypeInfo().Assembly.GetName().Name;
        }

        [Fact]

        [Fact]
        public void TwoWebHostsCreatedInParallel()
        {
            using (var server1 = new InProcessServer(assemblyName, this.output))
            using (var server2 = new InProcessServer(assemblyName, this.output))
            {
                this.ExecuteRequest(server1.BaseHost + requestPath);
                this.ExecuteRequest(server2.BaseHost + requestPath);


                Assert.DoesNotContain(telemetry1, t => t is TelemetryItem<ExceptionData>);

                var request1 = telemetry1.First(t => t is TelemetryItem<RequestData>);
                var request2 = telemetry1.Last(t => t is TelemetryItem<RequestData>);
                Assert.Equal("200", ((TelemetryItem<RequestData>)request1).data.baseData.responseCode);
                Assert.Equal("200", ((TelemetryItem<RequestData>)request2).data.baseData.responseCode);
            }
        }

        [Fact]

                using (var server2 = new InProcessServer(assemblyName, this.output))
                {
                    var config2 = (TelemetryConfiguration) server2.ApplicationServices.GetService(typeof(TelemetryConfiguration));

                    Assert.NotEqual(config1, config2);
                    Assert.NotEqual(config1.TelemetryChannel, config2.TelemetryChannel);

                    this.ExecuteRequest(server2.BaseHost);
                }
                this.ExecuteRequest(server1.BaseHost + requestPath);

                var telemetry = server1.Listener.ReceiveItems(TestListenerTimeoutInMs);
                this.DebugTelemetryItems(telemetry);

                Assert.NotEmpty(telemetry.Where(t => t is TelemetryItem<RequestData>));
                var request = telemetry.Single(IsValueControllerRequest);
                Assert.Equal("200", ((TelemetryItem<RequestData>) request).data.baseData.responseCode);

                Assert.DoesNotContain(telemetry, t => t is TelemetryItem<ExceptionData>);
            {
                this.ExecuteRequest(server.BaseHost + requestPath);

                server.DisposeHost();
                Assert.NotNull(activeConfig.TelemetryChannel);

                var telemetryClient = new TelemetryClient(activeConfig);
                telemetryClient.TrackTrace("some message after web host is disposed");

                var message = server.Listener.ReceiveItemsOfType<TelemetryItem<MessageData>>(1, TestListenerTimeoutInMs);
                Assert.Single(message);

                this.output.WriteLine(((TelemetryItem<MessageData>)message.Single()).data.baseData.message);

                Assert.Equal("some message after web host is disposed", ((TelemetryItem<MessageData>)message.Single()).data.baseData.message);
            }
        }

        private bool IsServiceDependencyCall(Envelope item)
        {
            }

            var url = dependency.data.baseData.data;

            // check if it's not tracked call from service to the test and a not call to get appid
            return url.Contains("microsoft.com");
        }

        private bool IsValueControllerRequest(Envelope item)
        {
                return false;
            }

            var url = dependency.data.baseData.url;

            // check if it's not tracked call from service to the test and a not call to get appid
            return url.Contains(requestPath);
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
