namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;

    /// <summary>
        /// Constructs an instance.
        /// </summary>
        public StackFrame(string assembly, string fileName, int level, int line, string method)
        {
            this.Data = new Extensibility.Implementation.External.StackFrame()
                line = line,
                method = method,
        }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
