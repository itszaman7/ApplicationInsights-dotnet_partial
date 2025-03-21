#if DEPENDENCY_COLLECTOR
namespace Microsoft.ApplicationInsights.W3C
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;

    /// <summary>
    /// Extends Activity to support W3C distributed tracing standard.
    /// </summary>
    [Obsolete("Use Microsoft.ApplicationInsights.Extensibility.W3C.W3CActivityExtensions in Microsoft.ApplicationInsights package instead")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class W3CActivityExtensions
    {
        /// <summary>
        /// Generate new W3C context.
        [Obsolete("Use Microsoft.ApplicationInsights.Extensibility.W3C.W3CActivityExtensions.IsW3CActivity in Microsoft.ApplicationInsights package instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool IsW3CActivity(this Activity activity) => Extensibility.W3C.W3CActivityExtensions.IsW3CActivity(activity);

        /// <summary>
        /// Updates context on the Activity based on the W3C Context in the parent Activity tree.
        /// </summary>
        /// <param name="activity">Activity to update W3C context on.</param>
        /// <returns>The same Activity for chaining.</returns>
            "Use Microsoft.ApplicationInsights.Extensibility.W3C.W3CActivityExtensions.UpdateContextOnActivity in Microsoft.ApplicationInsights package instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Activity UpdateContextOnActivity(this Activity activity) =>
            Extensibility.W3C.W3CActivityExtensions.UpdateContextOnActivity(activity);

        /// <summary>
        /// Gets traceparent header value for the Activity or null if there is no W3C context on it.
        /// </summary>
        /// <param name="activity">Activity to read W3C context from.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetTraceparent(this Activity activity, string value) => Extensibility.W3C.W3CActivityExtensions.SetTraceparent(activity, value);

        /// <summary>
        /// Gets tracestate header value from the Activity.
        /// </summary>
        /// <param name="activity">Activity to get tracestate from.</param>
        /// <returns>tracestate header value.</returns>
        [Obsolete("Use Microsoft.ApplicationInsights.Extensibility.W3C.W3CActivityExtensions.GetTracestate in Microsoft.ApplicationInsights package instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string GetTracestate(this Activity activity) => Extensibility.W3C.W3CActivityExtensions.GetTracestate(activity);

        /// <summary>
        /// Sets tracestate header value on the Activity.
        /// </summary>
        /// <param name="activity">Activity to set tracestate on.</param>
        /// <param name="value">tracestate header value.</param>
        [Obsolete("Use Microsoft.ApplicationInsights.Extensibility.W3C.W3CActivityExtensions.SetTracestate in Microsoft.ApplicationInsights package instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetTracestate(this Activity activity, string value) => Extensibility.W3C.W3CActivityExtensions.SetTracestate(activity, value);

        /// <summary>
        /// Gets TraceId from the Activity.
        /// Use carefully: if may cause iteration over all tags!.
        /// </summary>
        /// <param name="activity">Activity to get traceId from.</param>
        /// <returns>TraceId value or null if it does not exist.</returns>


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
