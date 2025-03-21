namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse;

    internal class QuickPulseProcessProviderMock : IQuickPulseProcessProvider
    {
        public List<QuickPulseProcess> Processes { get; set; }

        public Exception AlwaysThrow { get; set; } = null;
            {
                throw this.AlwaysThrow;
            }
        }
        {
            if (this.AlwaysThrow != null)
            {
                throw this.AlwaysThrow;
            {
                throw this.AlwaysThrow;
            }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
