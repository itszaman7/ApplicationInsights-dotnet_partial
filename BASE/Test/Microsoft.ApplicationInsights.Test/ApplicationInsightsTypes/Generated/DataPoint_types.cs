
//------------------------------------------------------------------------------
// This code was generated by a tool.
//
//   Tool : Bond Compiler 0.10.1.0
//   File : DataPoint_types.cs
//
// Changes to this file may cause incorrect behavior and will be lost when
// the code is regenerated.
// <auto-generated />
//------------------------------------------------------------------------------


// suppress "Missing XML comment for publicly visible type or member"
#pragma warning disable 1591


        // [global::Bond.Attribute("Description", "Metric type. Single measurement or the aggregated value.")]
        // [global::Bond.Id(20)]
        public DataPointType kind { get; set; }

        // [global::Bond.Attribute("Description", "Single value for measurement. Sum of individual measurements for the aggregation.")]
        // [global::Bond.Id(30), global::Bond.Required]
        public double value { get; set; }

        // [global::Bond.Attribute("Description", "Metric weight of the aggregated metric. Should not be set for a measurement.")]
        // [global::Bond.Id(40), global::Bond.Type(typeof(global::Bond.Tag.nullable<int>))]
        public int? count { get; set; }

        // [global::Bond.Attribute("Description", "Minimum value of the aggregated metric. Should not be set for a measurement.")]
        // [global::Bond.Id(50), global::Bond.Type(typeof(global::Bond.Tag.nullable<double>))]
        public double? min { get; set; }

        // [global::Bond.Attribute("Description", "Maximum value of the aggregated metric. Should not be set for a measurement.")]
        // [global::Bond.Id(60), global::Bond.Type(typeof(global::Bond.Tag.nullable<double>))]
        public double? max { get; set; }

        // [global::Bond.Attribute("Description", "Standard deviation of the aggregated metric. Should not be set for a measurement.")]
        // [global::Bond.Id(70), global::Bond.Type(typeof(global::Bond.Tag.nullable<double>))]

        public DataPoint()
            : this("AI.DataPoint", "DataPoint")
        {}



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
