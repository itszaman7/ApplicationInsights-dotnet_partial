﻿namespace Microsoft.ApplicationInsights.Metrics.Extensibility
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Some methods on <c>MetricManager.AggregationManager</c> need public access.
    /// However, the property <c>AggregationManager</c> on <c>MetricManager</c> is not public,
    /// nor is the type of that property (<c>MetricAggregationManager</c>).
    /// This class exposes the necesary APIs in a specialized namespace, while avoiding polluting
    /// the API surface shown by Intellisense for users who do not import the ...Extensibility namespace.
    /// </summary>
    /// @PublicExposureCandidate
    internal static class MetricManagerExtensions
        /// <summary>Stop the specified aggregation cycle for the specified manager and return the aggregates.</summary>
        /// <param name="metricManager">The metric manager.</param>
        /// <param name="aggregationCycleKind">The kind of the cycle to stop.</param>
        /// <param name="tactTimestamp">Timestamp that will be the end of the astopped aggregation cycle for all respective aggregators.</param>
        /// <returns>A holder that contains all the stopped aggregaors.</returns>
        /// <param name="metricManager">The metric manager that owns the aggregation cycle.</param>
        /// <param name="aggregationCycleKind">The kind of the aggregation cycle to start or cycle.</param>
        /// <param name="tactTimestamp">Timestamp to b used as cycle sart for all respective aggregators.</param>
        /// <param name="futureFilter">Filter to be used for the new cycle.</param>
        /// <returns>A holder containing aggregates for the previous cycle, if any.</returns>
        /// Metric Manager does not encapsulate any disposable or native resourses. However, it encapsulates a managed thread.
        /// In normal cases, a metric manager is accessed via convenience methods and consumers never need to worry about that thread.
        /// However, advanced scenarios may explicitly create a metric manager instance. In such cases, consumers may need to call
        /// this method on the explicitly created instance to let the thread know that it no longer needs to run. The thread will not
        /// be aborted proactively. Instead, it will complete the ongoing aggregation cycle and then gracfully exit instead of scheduling
        /// <param name="metricManager">The metric manager.</param>
        /// <returns>
        /// You can await the returned Task if you want to be sure that the encapsulated thread completed.
        /// If you just want to notify the thread to stop without waiting for it, do not await this method.
        /// </returns>
        {
            Util.ValidateNotNull(metricManager, nameof(metricManager));

            metricManager.Flush();
            return metricManager.AggregationCycle.StopAsync();


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
