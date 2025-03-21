namespace FunctionalTests.MVC.Tests.FunctionalTest
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using AI;

        public RequestTelemetryMvcTests(ITestOutputHelper output) : base(output)
        {
            this.assemblyName = this.GetType().GetTypeInfo().Assembly.GetName().Name;

        [Fact]
                var telemetries = server.Listener.ReceiveItems(TestListenerTimeoutInMs);
                this.DebugTelemetryItems(telemetries);

                // In linux 6 items as it'll always have an Error trace from SDK itself about
                // local storage folder.
                Assert.InRange(telemetries.Length, 5, 6);

                Assert.Contains(telemetries.OfType<TelemetryItem<RemoteDependencyData>>(),
                    t => ((TelemetryItem<RemoteDependencyData>)t).data.baseData.name == "GET /Home/Contact");

                Assert.Contains(telemetries.OfType<TelemetryItem<RequestData>>(),
                    t => ((TelemetryItem<RequestData>)t).data.baseData.name == "GET Home/Contact");
                Assert.Contains(telemetries.OfType<TelemetryItem<MessageData>>(),
                    t => ((TelemetryItem<MessageData>)t).data.baseData.message == "Fetched contact details." && ((TelemetryItem<MessageData>)t).data.baseData.severityLevel == AI.SeverityLevel.Information);
            }
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
