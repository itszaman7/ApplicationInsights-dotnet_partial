namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse
{
    using System;
    using System.Threading;


    /// <summary>
    /// Accumulator manager for QuickPulse data.
    /// </summary>
            if (collectionConfiguration == null)
            {
                throw new ArgumentNullException(nameof(collectionConfiguration));
            }

            this.currentDataAccumulator = new QuickPulseDataAccumulator(collectionConfiguration);
        }

        public QuickPulseDataAccumulator CurrentDataAccumulator => this.currentDataAccumulator;

        public QuickPulseDataAccumulator CompleteCurrentDataAccumulator(CollectionConfiguration collectionConfiguration)
        {
            /* 
                Here we need to 
                    - promote currentDataAccumulator to completedDataAccumulator
                    - reset (zero out) the new currentDataAccumulator


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
