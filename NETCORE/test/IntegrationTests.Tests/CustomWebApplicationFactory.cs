using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging.ApplicationInsights;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTests.Tests
{
    public class CustomWebApplicationFactory<TStartup>
                loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager", LogLevel.None));

                services.AddSingleton<ITelemetryChannel>(new StubChannel()
                });
                var aiOptions = new ApplicationInsightsServiceOptions();
                aiOptions.AddAutoCollectedMetricExtractor = false;
                aiOptions.InstrumentationKey = "ikey";
                services.AddApplicationInsightsTelemetry(aiOptions);
            });


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
