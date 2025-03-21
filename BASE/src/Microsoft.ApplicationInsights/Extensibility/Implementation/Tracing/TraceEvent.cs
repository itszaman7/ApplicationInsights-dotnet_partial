namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System.Globalization;
    using System.Linq;

    /// </summary>
    internal class TraceEvent
    {
        /// <summary>
        /// Prefix for user-actionable traces.
        /// </summary>
        private const string AiPrefix = "AI: ";

        /// </summary>
        public object[] Payload { get; set; }

        public override string ToString()
            // Add "AI: " prefix (if keyword does not contain UserActionable = (EventKeywords)0x1, than prefix should be "AI (Internal):" )
            string message = this.MetaData.IsUserActionable()
                ? AiPrefix
                : AiNonUserActionable + '[' + this.MetaData.EventSourceName + "] ";
                this.MetaData.MessageFormat;

            return message;
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
