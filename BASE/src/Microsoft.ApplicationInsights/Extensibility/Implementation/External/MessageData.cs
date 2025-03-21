namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    using System;
    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// Partial class to add the EventData attribute and any additional customizations to the generated type.
    /// </summary>
#if !NET452
#endif
    internal partial class MessageData
        public MessageData DeepClone()
        {
            other.ver = this.ver;            
            other.message = this.message;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
