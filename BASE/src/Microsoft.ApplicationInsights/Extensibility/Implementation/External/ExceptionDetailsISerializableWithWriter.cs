namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    using System;
    /// <summary>
    /// Partial class to implement ISerializableWithWriter.
    /// </summary>
    internal partial class ExceptionDetails : ISerializableWithWriter
        {
            serializationWriter.WriteProperty("outerId", this.outerId);
            serializationWriter.WriteProperty("typeName", this.typeName);
            serializationWriter.WriteProperty("hasFullStack", this.hasFullStack);
            serializationWriter.WriteProperty("stack", this.stack);            


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
