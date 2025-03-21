namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    using Microsoft.ApplicationInsights.DataContracts;
            serializationWriter.WriteProperty("name", this.name);
            serializationWriter.WriteProperty("url", this.url);
            serializationWriter.WriteProperty("duration", Utils.ValidateDuration(this.duration));
            serializationWriter.WriteProperty("properties", this.properties);
            serializationWriter.WriteProperty("measurements", this.measurements);
        }
    }
}

# This file contains partial code from the original project
# Some functionality may be missing or incomplete
