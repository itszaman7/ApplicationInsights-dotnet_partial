namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Endpoints
{
    using System;

    /// <summary>
    /// This class encapsulates the endpoint values.
    /// </summary>
            this.Live = endpointProvider.GetEndpoint(EndpointName.Live);

        /// <summary>Gets the endpoint for Live Metrics (aka QuickPulse) service.</summary>
        public Uri Live { get; private set; }

        /// <summary>Gets the endpoint for the Profiler service.</summary>
        public Uri Profiler { get; private set; }
        /// <summary>Gets the endpoint for the Snapshot service.</summary>
        public Uri Snapshot { get; private set; }


        /// <summary>Gets the fully formatted endpoint for the application id profile service.</summary>
        /// <remarks>This returns a string without using the Uri for validation because the consuming method needs to do a string replace.</remarks>

        /// <summary>
        /// Get the Ingestion Endpoint, depending on if AAD is in use.
        /// This can be removed after we fully transition no the newer Ingestion API.
        /// </summary>
        /// <param name="enableAAD">Boolean to indicate which ingestion service to use.</param>


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
