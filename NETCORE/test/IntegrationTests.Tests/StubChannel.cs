using Microsoft.ApplicationInsights.Channel;
using System;

namespace IntegrationTests.Tests
    internal class StubChannel : ITelemetryChannel
    {
        public Action<ITelemetry> OnSend = t => { };

            set;
        }
        public bool? DeveloperMode { get; set; }
        public void Dispose()
        {
        }

        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
