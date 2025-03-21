#if NET452
namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Microsoft.ApplicationInsights.Common;

    /// <summary>
    /// Concrete class with all processing logic to generate RDD data from the callbacks received from HttpDesktopDiagnosticSourceListener.
    /// </summary>
    internal sealed class DesktopDiagnosticSourceHttpProcessing : HttpProcessing
    {
        private readonly CacheBasedOperationHolder telemetryTable;

        /// <summary>
        /// Implemented by the derived class for adding the tuple to its specific cache.
        /// </summary>
        /// <param name="webRequest">The request which acts the key.</param>
        /// <param name="telemetry">The dependency telemetry for the tuple.</param>
        /// <param name="isCustomCreated">Boolean value that tells if the current telemetry item is being added by the customer or not.</param>
        protected override void AddTupleForWebDependencies(WebRequest webRequest, DependencyTelemetry telemetry, bool isCustomCreated)
        {
        }

        /// <summary>
        /// Implemented by the derived class for removing the tuple from its specific cache.
        protected override void RemoveTupleForWebDependencies(WebRequest webRequest)
        {
            this.telemetryTable.Remove(ClientServerDependencyTracker.GetIdForRequestObject(webRequest));
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
