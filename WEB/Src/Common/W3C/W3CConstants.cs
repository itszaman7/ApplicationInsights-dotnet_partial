#if DEPENDENCY_COLLECTOR
    namespace Microsoft.ApplicationInsights.W3C
#else
    namespace Microsoft.ApplicationInsights.W3C.Internal
#endif
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// W3C constants.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
#if DEPENDENCY_COLLECTOR
    public
    internal
#endif
    static class W3CConstants
    {
        /// </summary>
        public const string TraceParentHeader = "traceparent";

        /// <summary>
        /// </summary>
        [Obsolete("Dot not use.")]
        public const string AzureTracestateNamespace = "az";

        /// Separator between Azure namespace values.


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
