
//------------------------------------------------------------------------------
// This code was generated by a tool.
//
//   Tool : Bond Compiler 0.4.1.0
//   File : DataPoint_types.cs
//
// Changes to this file may cause incorrect behavior and will be lost when
// the code is regenerated.
// <auto-generated />
//------------------------------------------------------------------------------


// suppress "Missing XML comment for publicly visible type or member"
#pragma warning disable 1591

// ReSharper disable UnusedParameter.Local
// ReSharper disable RedundantUsingDirective
#endregion

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    
    
        
        
        
        public string name { get; set; }

        public DataPointType kind { get; set; }

        
        

        
        
        public double? min { get; set; }

        public DataPoint()
            : this("AI.DataPoint", "DataPoint")
        {}

        protected DataPoint(string fullName, string name)


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
