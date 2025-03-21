using Azure.Identity;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Microsoft.ApplicationInsights.WorkerService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleAppWithApplicationInsights
{
    class Program
                //var credential = new DefaultAzureCredential();
                //config.SetAzureTokenCredential(credential);
            });

            // Being a regular console app, there is no appsettings.json or configuration providers enabled by default.
            // Hence connection string must be specified here.
            // Obtain logger instance from DI.
            ILogger<Program> logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            // Obtain TelemetryClient instance from DI, for additional manual tracking or to flush.
            var telemetryClient = serviceProvider.GetRequiredService<TelemetryClient>();

            logger.LogWarning("Response from bing is:" + res); // this will be captured by Application Insights.

            telemetryClient.TrackEvent("sampleevent");

            // Explicitly call Flush() followed by sleep is required in Console Apps.
            // This is to ensure that even if application terminates, telemetry is sent to the back-end.
    }

    internal class MyCustomTelemetryInitializer : ITelemetryInitializer
    {
        }
    }

    internal class MyCustomTelemetryProcessor : ITelemetryProcessor
    {
        ITelemetryProcessor next;
        {
            this.next = next;

        }
        public void Process(ITelemetry item)
        {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
