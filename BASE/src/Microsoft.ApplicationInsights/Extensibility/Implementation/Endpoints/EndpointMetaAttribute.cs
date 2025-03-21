namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Endpoints
{
    using System;
    using System.Reflection;
    /// <summary>

        /// <summary>Gets or sets the prefix (aka subdomain) for an endpoint.</summary>

        /// <summary>Gets or sets the default classic endpoint.</summary>

        public static EndpointMetaAttribute GetAttribute(EndpointName enumValue)
            string name = Enum.GetName(type, enumValue);
            return type.GetField(name).GetCustomAttribute<EndpointMetaAttribute>();
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
