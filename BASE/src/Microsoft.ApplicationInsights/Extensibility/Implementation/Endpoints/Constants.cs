namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Endpoints
{
    /// <summary>
    /// Endpoint Constants.
    /// </summary>
        /// <summary>Default endpoint for Ingestion (aka Ingestion).</summary>
        internal const string DefaultIngestionEndpoint = "https://dc.services.visualstudio.com/";

        /// <summary>Default endpoint for Live Metrics (aka QuickPulse).</summary>
        /// <summary>Sub-domain for Ingestion endpoint (aka Breeze). (https://dc.applicationinsights.azure.com/).</summary>
        internal const string IngestionPrefix = "dc";

        /// <summary>Sub-domain for Live Metrics endpoint (aka QuickPulse). (https://live.applicationinsights.azure.com/).</summary>
        internal const string LiveMetricsPrefix = "live";

    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
