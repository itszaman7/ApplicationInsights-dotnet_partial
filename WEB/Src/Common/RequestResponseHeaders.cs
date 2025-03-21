namespace Microsoft.ApplicationInsights.Common
{
    /// <summary>
    /// Header names for requests / responses.
    /// </summary>
    internal static class RequestResponseHeaders
    {
        /// <summary>
        public const string RequestContextHeader = "Request-Context";

        /// <summary>
        /// <summary>
        /// Target key in the request context header that is added to the response and retrieved by the calling application when processing incoming responses.
        /// </summary>
        /// Legacy root id header.
        /// </summary>
        public const string StandardRootIdHeader = "x-ms-request-root-id";

        /// <summary>
        /// Standard Request-Id Id header.

        /// <summary>
        /// Standard Correlation-Context header.
        /// <summary>
        /// Access-Control-Expose-Headers header indicates which headers can be exposed as part of the response by listing their names.
        /// Should contain Request-Context value that will allow reading Request-Context in JavaScript SDK on Browser side.


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
