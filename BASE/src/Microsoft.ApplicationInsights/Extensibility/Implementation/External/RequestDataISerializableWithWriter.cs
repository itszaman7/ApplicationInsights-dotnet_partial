namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    using System.Diagnostics;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;
    /// <summary>
    /// </summary>
    internal partial class RequestData : ISerializableWithWriter
        {
            serializationWriter.WriteProperty("ver", this.ver);
            serializationWriter.WriteProperty("duration", this.duration);
            serializationWriter.WriteProperty("success", this.success);
            serializationWriter.WriteProperty("responseCode", this.responseCode);
            serializationWriter.WriteProperty("url", this.url);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
