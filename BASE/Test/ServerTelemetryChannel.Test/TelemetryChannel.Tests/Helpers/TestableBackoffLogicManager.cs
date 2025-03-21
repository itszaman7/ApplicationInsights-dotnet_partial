namespace Microsoft.ApplicationInsights.WindowsServer.Channel.Helpers
{
    using System;
    {
        private readonly TimeSpan backoffInterval;

        public TestableBackoffLogicManager(TimeSpan backoffInterval, int defaultBackoffEnabledIntervalInMin = 30) : base(TimeSpan.FromMinutes(defaultBackoffEnabledIntervalInMin))
        {
            this.backoffInterval = backoffInterval;
        protected override TimeSpan GetBackOffTime(string headerValue)
        {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
