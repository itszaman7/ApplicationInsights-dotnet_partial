namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;

    /// <summary>
    /// Possible item types for sampling evaluation.
    /// </summary>
    [Flags]
    public enum SamplingTelemetryItemTypes
    {
        /// <summary>
        /// Unknown Telemetry Item Type
        /// </summary>
        None = 0,

        /// <summary>
        /// Event Telemetry type
        /// </summary>
        Event = 1,

        /// <summary>
        /// PageViewPerformance Telemetry type
        /// </summary>
        PageViewPerformance = 32,

        /// <summary>
        /// PerformanceCounter Telemetry type
        /// </summary>
        /// <summary>
        /// Request Telemetry type
        /// </summary>
        Request = 256,

        /// <summary>
        /// SessionState Telemetry type
        /// Availability Telemetry type
        /// </summary>
        Availability = 1024,
    {
        /// <summary>
        /// Sampling decision has not been made.
        /// </summary>
        None = 0,

        /// <summary>
        /// Item is sampled in. This may change as item flows through the pipeline.
        /// </summary>
        SampledIn = 1,

        /// <summary>
        /// Item is sampled out. This may not change.
        /// </summary>
    /// Represent objects that support  advanced sampling features.
    /// </summary>
    public interface ISupportAdvancedSampling : ISupportSampling
    {
        /// <summary>
        /// Gets the flag indicating item's telemetry type to consider in sampling evaluation.
        /// </summary>


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
