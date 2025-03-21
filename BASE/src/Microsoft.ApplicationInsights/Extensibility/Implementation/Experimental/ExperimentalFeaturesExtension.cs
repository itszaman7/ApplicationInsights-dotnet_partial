namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Experimental
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    /// </summary>
    /// <remarks>
    /// This allows the dev team to ship and evaluate features before adding these to the public API.
    /// We are not committing to support any features enabled through this property.
    /// Use this at your own risk.
    /// </remarks>
        /// <param name="featureName">Name of the feature to evaluate.</param>
        /// <returns>Returns a boolean value indicating if the feature name exists in the provided configuration.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool EvaluateExperimentalFeature(this TelemetryConfiguration telemetryConfiguration, string featureName)
        {
            if (telemetryConfiguration == null)
                throw new ArgumentNullException(nameof(telemetryConfiguration));
            }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
