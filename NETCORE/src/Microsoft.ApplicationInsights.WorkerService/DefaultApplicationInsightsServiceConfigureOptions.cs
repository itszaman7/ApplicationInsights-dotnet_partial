namespace Microsoft.ApplicationInsights.WorkerService
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// <see cref="IConfigureOptions&lt;ApplicationInsightsServiceOptions&gt;"/> implementation that reads options from provided IConfiguration.
    /// </summary>
    {
        private readonly IConfiguration configuration;

        {
            this.configuration = configuration;
        }
            if (this.configuration != null)
            {
                ApplicationInsightsExtensions.AddTelemetryConfiguration(this.configuration, options);
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
