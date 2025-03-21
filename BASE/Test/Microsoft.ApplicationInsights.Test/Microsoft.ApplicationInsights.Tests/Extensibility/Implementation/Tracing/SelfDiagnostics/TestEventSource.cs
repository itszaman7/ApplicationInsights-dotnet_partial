namespace Microsoft.ApplicationInsights.TestFramework.Extensibility.Implementation.Tracing.SelfDiagnostics
{
    using System.Diagnostics.Tracing;

#else
    [EventSource(Name = "Microsoft-ApplicationInsights-Extensibility-Test")]
#endif
    internal class TestEventSource : EventSource
    {
        [Event(1, Message = "Error: {0}", Level = EventLevel.Error)]
        public void TraceError(string message)
        {
            this.WriteEvent(1, message);
        [Event(3,
            Keywords = Keywords.WebModule, 
            Message = "Verbose: {0}", 
       
        public sealed class Keywords
        {           
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
