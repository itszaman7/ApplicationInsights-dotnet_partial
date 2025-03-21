namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;


    internal class QuickPulseTopCpuCollectorMock : IQuickPulseTopCpuCollector
    {
        public List<Tuple<string, int>> TopProcesses { get; set; } = new List<Tuple<string, int>>();
        public bool AccessDenied { get; }

        {
            return this.TopProcesses;
        {
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
