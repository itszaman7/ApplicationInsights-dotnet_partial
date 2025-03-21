namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    using System;
    using System.Diagnostics;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
#if !NET452
    // .NET 4.5.2 have a custom implementation of RichPayloadEventSource
    [System.Diagnostics.Tracing.EventData(Name = "PartB_ExceptionData")]
    internal partial class ExceptionData
    {
        public ExceptionData DeepClone()
        {
            var other = new ExceptionData();
            other.ver = this.ver;
            other.problemId = this.problemId;
            Debug.Assert(other.properties != null, "The constructor should have allocated properties dictionary");
            Debug.Assert(other.measurements != null, "The constructor should have allocated the measurements dictionary");


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
