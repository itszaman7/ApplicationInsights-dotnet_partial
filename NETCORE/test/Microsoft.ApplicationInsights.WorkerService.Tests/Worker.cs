using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.ApplicationInsights.WorkerService.Tests
{
    internal class Worker : IHostedService
    {
        private readonly ILogger _logger;

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (tc.StartOperation<RequestTelemetry>("myoperation"))
            {
               var res =  httpClient.GetAsync("http://bing.com").Result.StatusCode;
                _logger.LogWarning("warning level log - calling bing completed with status:" + res);
            }
        }
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);
        {
            _timer?.Dispose();
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
