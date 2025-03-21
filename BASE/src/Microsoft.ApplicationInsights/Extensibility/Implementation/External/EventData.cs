namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    using System.Diagnostics;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;

    // .NET 4.5.2 have a custom implementation of RichPayloadEventSource
    [System.Diagnostics.Tracing.EventData(Name = "PartB_EventData")]
#endif
    internal partial class EventData
            return other;
        }
            Debug.Assert(other.properties != null, "The constructor should have allocated properties dictionary");
            Debug.Assert(other.measurements != null, "The constructor should have allocated the measurements dictionary");
            Utils.CopyDictionary(this.properties, other.properties);
            Utils.CopyDictionary(this.measurements, other.measurements);
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
