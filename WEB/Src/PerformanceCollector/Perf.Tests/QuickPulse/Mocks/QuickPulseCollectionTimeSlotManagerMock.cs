namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    internal class QuickPulseCollectionTimeSlotManagerMock : QuickPulseCollectionTimeSlotManager
    {
        private readonly TimeSpan interval;

        public override DateTimeOffset GetNextCollectionTimeSlot(DateTimeOffset currentTime)
        {
            return currentTime + this.interval;
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
