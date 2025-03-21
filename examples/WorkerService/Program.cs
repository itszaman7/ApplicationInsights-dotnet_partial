
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>

                    // Application Insights

                    // Add custom TelemetryInitializer
                    services.AddSingleton<ITelemetryInitializer, MyCustomTelemetryInitializer>();
                // Replace with actual properties.
                (telemetry as ISupportProperties).Properties["MyCustomKey"] = "MyCustomValue";
            }
        }

        internal class MyCustomTelemetryProcessor : ITelemetryProcessor
        {
            ITelemetryProcessor next;

                this.next = next;
            }

            public void Process(ITelemetry item)
            {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
