namespace Microsoft.ApplicationInsights.Common
{
    using System;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
        public void VerifyInterlockedWorksAsExpected()
        {
            int testInterval = 10;
            var counter = 0;

            var its = new InterlockedThrottle(TimeSpan.FromSeconds(testInterval));
            its.PerformThrottledAction(() => counter++);
            its.PerformThrottledAction(() => counter++);

            its.PerformThrottledAction(() => counter++);
            its.PerformThrottledAction(() => counter++);
            its.PerformThrottledAction(() => counter++);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
