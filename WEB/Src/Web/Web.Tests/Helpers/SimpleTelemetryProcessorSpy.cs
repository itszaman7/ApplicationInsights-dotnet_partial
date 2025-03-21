namespace Microsoft.ApplicationInsights.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Microsoft.ApplicationInsights.Channel;
        private readonly object lockObject = new object();

        private readonly List<Channel.ITelemetry> receivedItems = new List<ITelemetry>();

        public int ReceivedCalls
        {
                {
                    return this.receivedItems.Count;
                }
        }

        public List<Channel.ITelemetry> ReceivedItems
            }
        } 

        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
