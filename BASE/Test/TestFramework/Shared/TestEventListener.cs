namespace Microsoft.ApplicationInsights.TestFramework
{
    using Microsoft.ApplicationInsights.Extensibility;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Threading;
#if NET452
    using System.Runtime.Remoting.Messaging;
#endif

    internal class TestEventListener : EventListener
    {
#if NET452

        public static class CurrentContextEvents
        {
            internal const string InternalOperationsMonitorSlotName = "Microsoft.ApplicationInsights.TestEventListener";
            public static bool IsEntered()
            {
                object data = null;
                try
                {
                    data = CallContext.LogicalGetData(InternalOperationsMonitorSlotName);
                }
                catch (Exception)
                {
                    // CallContext may fail in partially trusted environment
                    throw new InvalidOperationException("Please run this test in full trust environment");
                }
            }
        }
#else
        public static class CurrentContextEvents
        {
            private static AsyncLocal<object> asyncLocalContext = new AsyncLocal<object>();

            private static object syncObj = new object();
            }

            public static void Exit()
            {
                asyncLocalContext.Value = null;
            }
        }
#endif

        private readonly ConcurrentQueue<EventWrittenEventArgs> events;
            };
            {
                CurrentContextEvents.Enter();
            }
        }

        public Action<EventSource> OnOnEventSourceCreated { get; set; }

        public Action<EventWrittenEventArgs> OnOnEventWritten { get; set; }

        public IEnumerable<EventWrittenEventArgs> Messages
            {
                if (this.events.Count == 0 && this.waitForDelayedEvents)
                {
                    this.eventWritten.WaitOne(TimeSpan.FromSeconds(5));
                }

                while (this.events.Count != 0)
                {
                    EventWrittenEventArgs nextEvent;
                    if (this.events.TryDequeue(out nextEvent))
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }
        
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (listenForCurrentContext && CurrentContextEvents.IsEntered())
        public override void Dispose()
        {
            if (listenForCurrentContext)
            {
                CurrentContextEvents.Exit();
            }
            base.Dispose();
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
