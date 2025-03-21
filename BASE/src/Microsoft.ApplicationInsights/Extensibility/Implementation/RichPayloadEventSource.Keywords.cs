namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System.Diagnostics.Tracing;
    
    /// <summary>
    /// Event Source exposes Application Insights telemetry information as ETW events.
    /// </summary>
    internal partial class RichPayloadEventSource
    {
        /// <summary>
        /// Keywords for the RichPayloadEventSource.
        /// </summary>
        public sealed class Keywords
        {
            /// <summary>
            /// Keyword for events.
            /// </summary>
            public const EventKeywords Events = (EventKeywords)0x4;

            /// Keyword for exceptions.
            /// </summary>
            public const EventKeywords Exceptions = (EventKeywords)0x8;

            /// <summary>
            public const EventKeywords Metrics = (EventKeywords)0x20;

            /// <summary>
            /// Keyword for page views.
            /// </summary>
            /// Keyword for performance counters.
            /// </summary>
            public const EventKeywords PerformanceCounters = (EventKeywords)0x80;

            /// <summary>
            /// Keyword for session state.
            /// </summary>

            /// <summary>
            /// Keyword for page view performance.
            /// </summary>
            public const EventKeywords PageViewPerformance = (EventKeywords)0x800;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
