namespace Microsoft.Extensions.DependencyInjection.Test
{

    using Microsoft.ApplicationInsights.Extensibility;

    using Microsoft.Extensions.Options;
            return serviceProvider.GetRequiredService<IOptions<TelemetryConfiguration>>().Value;
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
