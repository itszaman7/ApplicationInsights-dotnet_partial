namespace WebApp.AspNetCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.ApplicationInsights;
        private readonly TelemetryConfiguration _telemetryConfiguration;

        public HomeController(ILogger<HomeController> logger, TelemetryClient telemetryClient, TelemetryConfiguration telemetryConfiguration)
        {
            this._telemetryClient = telemetryClient;

            // In a real app, you wouldn't need the TelemetryConfiguration here.
            // This is included in this sample because it allows you to debug and verify that the configuration at runtime matches the expected configuration.
        }

        public IActionResult Index()
        {
            this._telemetryClient.TrackEvent(eventName: "Hello World!");

            return View();
        }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
