namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;

    /// <summary>
    public static class TelemetryContextExtensions
    {
        /// Returns TelemetryContext's Internal context.
        /// </summary>
        public static InternalContext GetInternalContext(this TelemetryContext context)
        {
            {
                throw new ArgumentNullException(nameof(context));
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
