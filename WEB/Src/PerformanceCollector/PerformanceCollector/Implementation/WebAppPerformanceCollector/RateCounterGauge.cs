namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Struct for metrics dependant on time.
    /// </summary>
    internal class RateCounterGauge : ICounterValue
    {
        /// <summary>
        /// Name of the counter.
        /// </summary>
        private string name;

        /// <summary>
        /// JSON identifier of the counter variable.
        /// </summary>
        private string jsonId;

        /// <summary>
        private ICachedEnvironmentVariableAccess cacheHelper;

        /// <summary>
        /// DateTime object to keep track of the last time this metric was retrieved.
        /// </summary>
        private DateTimeOffset lastCollectedTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="RateCounterGauge"/> class.
        /// <summary>
        /// Initializes a new instance of the <see cref="RateCounterGauge"/> class. 
        /// This constructor is intended for Unit Tests.
        /// </summary>
        /// <param name="name"> Name of the counter variable.</param>
        /// <param name="jsonId">JSON identifier of the counter variable.</param>
        /// <param name="environmentVariable"> Identifier of the corresponding environment variable.</param>
        /// <param name="counter">Dependant counter.</param>
        /// <param name="cache"> Cache object.</param>
        internal RateCounterGauge(string name, string jsonId, AzureWebApEnvironmentVariables environmentVariable, ICounterValue counter, ICachedEnvironmentVariableAccess cache)
        {
            this.name = name;
            this.jsonId = jsonId;
            this.environmentVariable = environmentVariable;
            this.counter = counter;
            this.cacheHelper = cache;
        }

                    PerformanceCollectorEventSource.Log.WebAppCounterNegativeValue(
                    this.lastValue,
                    previouslyCollectedValue,
                    this.name);
                }
                else
                {
                    value = timeDifferenceInSeconds != 0 ? (double)(diff / timeDifferenceInSeconds) : 0;
                }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
