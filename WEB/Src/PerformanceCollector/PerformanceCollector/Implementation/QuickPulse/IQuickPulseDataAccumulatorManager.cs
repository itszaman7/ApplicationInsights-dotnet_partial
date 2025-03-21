namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse
{
    using Microsoft.ApplicationInsights.Extensibility.Filtering;
        /// <summary>
        /// Locks in the current data accumulator and moves it into the Complete slot.
        /// Resets the Current slot to a new zeroed-out accumulator.
        /// </summary>
        /// <param name="collectionConfiguration">The collection configuration to be used for the next accumulator.</param>
        /// <returns>The newly completed accumulator.</returns>
        QuickPulseDataAccumulator CompleteCurrentDataAccumulator(CollectionConfiguration collectionConfiguration);
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
