namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.ComponentModel;
        /// </summary>
        public static double GetLastObservedSamplingPercentage(this TelemetryConfiguration configuration, SamplingTelemetryItemTypes samplingItemType)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return configuration.LastKnownSampleRateStore.GetLastObservedSamplingPercentage(samplingItemType);
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
