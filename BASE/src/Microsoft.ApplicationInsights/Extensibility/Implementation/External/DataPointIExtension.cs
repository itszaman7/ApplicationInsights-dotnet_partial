namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{

    /// <summary>
    /// Partial class to implement ISerializableWithWriter.
    /// </summary>
        public void Serialize(ISerializationWriter serializationWriter)
        {
            serializationWriter.WriteProperty("value", this.value);
            serializationWriter.WriteProperty("count", this.count.HasValue ? this.count : 1);
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
