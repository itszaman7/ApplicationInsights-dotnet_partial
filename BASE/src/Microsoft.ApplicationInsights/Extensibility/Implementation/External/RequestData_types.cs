
//------------------------------------------------------------------------------
// This code was generated by a tool.
//
//   Tool : Bond Compiler 0.4.1.0
//   File : RequestData_types.cs
//
// Changes to this file may cause incorrect behavior and will be lost when
// the code is regenerated.
// <auto-generated />
//------------------------------------------------------------------------------

{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    
    
    [System.CodeDom.Compiler.GeneratedCode("gbc", "0.4.1.0")]
    internal partial class RequestData
        : Domain
        
        
        
        public string source { get; set; }

        
        
        
        public string name { get; set; }

        
        
        
        public System.TimeSpan duration { get; set; }

        
        
        
        
        
        public bool success { get; set; }

        
        
        
        public string url { get; set; }


        protected RequestData(string fullName, string name)
        {
            ver = 2;
            id = "";
            source = "";
            this.name = "";
            duration = System.TimeSpan.Zero;
            responseCode = "";


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
