namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector
{
    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// Gauge that gives the user an aggregate of requested counters in a cache.
    /// </summary>
    internal class RawCounterGauge : ICounterValue
        /// </summary>
        private AzureWebApEnvironmentVariables environmentVariable;

        private ICachedEnvironmentVariableAccess cacheHelper;
        /// <param name="name">Name of counter variable.</param>
        /// <param name="jsonId">JSON identifier of the counter variable.</param>
        /// <param name="environmentVariable">Identifier of the environment variable.</param>
        public RawCounterGauge(string name, string jsonId, AzureWebApEnvironmentVariables environmentVariable)

        internal RawCounterGauge(string name, string jsonId, AzureWebApEnvironmentVariables environmentVariable, ICachedEnvironmentVariableAccess cache)
            this.environmentVariable = environmentVariable;
            this.cacheHelper = cache;
        }

        /// <summary>
        /// Returns the current value of the counter as a <c ref="float"/> and resets the metric.
        /// </summary>
        /// <returns>The value of the target metric.</returns>
        public double Collect()
        {
            return this.cacheHelper.GetCounterValue(this.jsonId, this.environmentVariable);
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
