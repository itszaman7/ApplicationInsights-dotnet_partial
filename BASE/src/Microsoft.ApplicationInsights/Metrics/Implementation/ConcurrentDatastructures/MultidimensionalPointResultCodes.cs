namespace Microsoft.ApplicationInsights.Metrics.ConcurrentDatastructures
{
    using System;

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1008: Enums should have zero value",
            Justification = "Crafted these flags to fit into a byte to make the struct container cheaper.")]
        /// A new point was created and returned in this result.
        /// The newly created point exceeded the specified dimension values count limit for one or more dimensions,
        /// but it was capped with a fallback value.
        /// </summary>
        Success_NewPointCreatedAboveDimCapLimit = 4,


        /// <summary>
        /// A point could not be retreived becasue it does not exist and creation was not requested.
        /// </summary>
        Failure_PointDoesNotExistCreationNotRequested = 32,

        /// <summary>
        /// Timeout reached.
        /// </summary>
        Failure_AsyncTimeoutReached = 128,
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
