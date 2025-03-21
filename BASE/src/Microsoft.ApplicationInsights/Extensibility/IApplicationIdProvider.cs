namespace Microsoft.ApplicationInsights.Extensibility
{
        /// <summary>
        /// Retrieves the Application Id to be used for Request.Source or Dependency.Target.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="instrumentationKey">Instrumentation Key string used to lookup associated Application Id.</param>
        /// <returns>TRUE if Application Id was successfully retrieved; FALSE otherwise.</returns>
        bool TryGetApplicationId(string instrumentationKey, out string applicationId);
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
