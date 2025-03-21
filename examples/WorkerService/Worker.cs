using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Net.Http;
using System.Threading;
    {
        private readonly ILogger<Worker> _logger;
        private TelemetryClient tc;
        {
            _logger = logger;
            this.tc = tc;
                // However the following Info level will be captured by ApplicationInsights,
                // as appsettings.json configured Information level for the category 'WorkerServiceSampleWithApplicationInsights.Worker'
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                {
                    var res = httpClient.GetAsync("https://bing.com").Result.StatusCode;
                    _logger.LogInformation("bing http call completed with status:" + res);
            }
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
