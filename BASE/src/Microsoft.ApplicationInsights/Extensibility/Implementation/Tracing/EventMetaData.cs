namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{

    /// <summary>
    /// Event metadata from event source method attribute.
    /// </summary>
    internal class EventMetaData
    {

        public EventLevel Level { get; set; }

        public bool IsUserActionable() => (this.Keywords & EventSourceKeywords.UserActionable) == EventSourceKeywords.UserActionable;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
