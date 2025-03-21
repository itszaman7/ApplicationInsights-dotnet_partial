namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// This API supports the AI Framework infrastructure and is not intended to be used directly from your code.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class TelemetryModules
    {
        private static TelemetryModules instance;


        /// <summary>
        /// Gets the TelemetryModules collection.
        /// </summary>
        public IList<ITelemetryModule> Modules { get; private set; }        
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
