
//------------------------------------------------------------------------------
// This code was generated by a tool.
//
//   Tool : Bond Compiler 0.10.1.0
//   File : EventData_types.cs
//
// Changes to this file may cause incorrect behavior and will be lost when
// the code is regenerated.
// <auto-generated />
//------------------------------------------------------------------------------

#region ReSharper warnings
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable InconsistentNaming
    // [global::Bond.Schema]
    [System.CodeDom.Compiler.GeneratedCode("gbc", "0.10.1.0")]
    public partial class EventData
        : Domain
    {
        public int ver { get; set; }

        // [global::Bond.Attribute("Description", "Event name. Keep it low cardinality to allow proper grouping and useful metrics.")]
        // [global::Bond.Attribute("Question", "Why Custom Event name is shorter than Request name or dependency name?")]
        // [global::Bond.Id(20), global::Bond.Required]
        public string name { get; set; }


        public EventData()
            : this("AI.EventData", "EventData")
        {}

        {
            ver = 2;
            this.name = "";
            properties = new Dictionary<string, string>();
            measurements = new Dictionary<string, double>();


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
