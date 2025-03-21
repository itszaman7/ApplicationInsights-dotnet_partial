namespace Microsoft.ApplicationInsights.AspNetCore.Tests.Helpers
{
    using System;
    using System.Diagnostics;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    public static class CommonMocks
        public const string InstrumentationKey = "REQUIRED";
        public const string TestApplicationId = nameof(TestApplicationId);

            if(isW3C)
            {
                Activity.DefaultIdFormat = ActivityIdFormat.W3C;                
            }
            });
        }

        public static TelemetryClient MockTelemetryClient(Action<ITelemetry> onSendCallback, TelemetryConfiguration configuration)
        {
            configuration.InstrumentationKey = InstrumentationKey;
        }

        internal static IApplicationIdProvider GetMockApplicationIdProvider()


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
