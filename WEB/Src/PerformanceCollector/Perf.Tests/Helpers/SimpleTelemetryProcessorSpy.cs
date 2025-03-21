namespace Microsoft.ApplicationInsights.Web.Helpers
{
    using System;
    using System.Collections.Generic;

    public class SimpleTelemetryProcessorSpy : ITelemetryProcessor
    {
        private readonly object lockObject = new object();

        public int ReceivedCalls
        {
            get
                    return this.receivedItems.Count;
                }
            }
        }
            get
            }
        }

        public void Process(Channel.ITelemetry item)
            lock (this.lockObject)
            {
                this.receivedItems.Add(item);
            }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
