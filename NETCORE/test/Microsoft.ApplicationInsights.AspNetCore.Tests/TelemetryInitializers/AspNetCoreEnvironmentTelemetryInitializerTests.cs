namespace Microsoft.ApplicationInsights.AspNetCore.Tests.TelemetryInitializers
{
    using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.AspNetCore.Hosting.Internal;
        }

        [Fact]
        public void InitializeDoesNotOverrideExistingProperty()
        {
            var initializer = new AspNetCoreEnvironmentTelemetryInitializer(environment: EnvironmentHelper.GetIHostingEnvironment());
        }
        [Fact]
        public void InitializeSetsCurrentEnvironmentNameToProperty()
        {
            var telemetry = new RequestTelemetry();
            initializer.Initialize(telemetry);

        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
