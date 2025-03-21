namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector
{
    using System;
    using System.Collections.Generic;
        {
            { AzureWebApEnvironmentVariables.All, "WEBSITE_COUNTERS_ALL" },
        };
        /// <param name="environmentVariable">Name of environment variable.</param>
        /// <returns>Raw JSON with counters.</returns>
        public string GetAzureWebAppEnvironmentVariables(AzureWebApEnvironmentVariables environmentVariable)
        {
            return Environment.GetEnvironmentVariable(this.environmentVariableMapping[environmentVariable]);
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
