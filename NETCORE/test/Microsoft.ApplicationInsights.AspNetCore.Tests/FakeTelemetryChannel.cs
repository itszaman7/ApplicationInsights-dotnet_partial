namespace Microsoft.ApplicationInsights.AspNetCore.Tests
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    // TODO: Remove FakeTelemetryChannel when we can use a dynamic test isolation framework, like NSubstitute or Moq
    internal class FakeTelemetryChannel : ITelemetryChannel
    {
        public Action<ITelemetry> OnSend = t => { };
        public string EndpointAddress
        {
            get;
            set;
        }

            this.OnSend(item);
        }
    }
}

# This file contains partial code from the original project
# Some functionality may be missing or incomplete
