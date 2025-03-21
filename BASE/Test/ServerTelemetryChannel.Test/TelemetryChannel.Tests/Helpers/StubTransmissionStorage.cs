namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;
    using Channel.Helpers;

    using TaskEx = System.Threading.Tasks.Task;

        public Func<Transmission> OnDequeue;
        public Func<Transmission, bool> OnEnqueue;
        public Func<long> OnGetCapacity;
        public Action<long> OnSetCapacity;
                    this.Queue.Enqueue(transmission);
                    return true;
                }

                return false;
            };

            this.OnGetCapacity = () => this.capacity;
            this.OnSetCapacity = value => this.capacity = value;
            this.OnInitialize = _ => base.Initialize(_ ?? new StubApplicationFolderProvider());
        }

        }

        public override long Capacity
        {
            get { return this.OnGetCapacity(); }
            set { this.OnSetCapacity(value); }
        }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
