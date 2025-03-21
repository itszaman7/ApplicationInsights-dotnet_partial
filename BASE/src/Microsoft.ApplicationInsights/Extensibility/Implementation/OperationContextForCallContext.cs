namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Operation class that holds operation id and operation name for the current call context.
    /// </summary>
        /// <summary>
        /// Operation id that will be assigned to all the child telemetry items.
        public string ParentOperationId;

        /// </summary>
        public string RootOperationId;
        public string RootOperationName;

        /// <summary>
        /// Context that is propagated with HTTP outbound calls, check for null.


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
