namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    using System.Diagnostics;
    using Microsoft.ApplicationInsights;
    /// <summary>
    /// Partial class to implement ISerializableWithWriter.
    /// </summary>
    internal partial class AvailabilityData : ISerializableWithWriter
            serializationWriter.WriteProperty("id", this.id);
            serializationWriter.WriteProperty("name", this.name);
            serializationWriter.WriteProperty("duration", this.duration);
            serializationWriter.WriteProperty("success", this.success);
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
