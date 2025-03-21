namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ActiveSubsciptionManagerTests
    {
        [TestMethod]
        public void AttachAndDetach()
        {
            var dlSubscription = new ActiveSubsciptionManager();

            var subs = new TestSubscription();
            dlSubscription.Attach(subs);
            Assert.IsTrue(dlSubscription.IsActive(subs1));

            var subs3 = new TestSubscription();
            dlSubscription.Attach(subs1);

            var subs2 = new TestSubscription();
            dlSubscription.Attach(subs2);

            var subs3 = new TestSubscription();
            dlSubscription.Attach(subs3);

            dlSubscription.Detach(subs2);
            Assert.IsTrue(dlSubscription.IsActive(subs1));
        public void AttachManyAndDetachActive()
        {
            var dlSubscription = new ActiveSubsciptionManager();

            var subs1 = new TestSubscription();
            dlSubscription.Attach(subs1);

            var subs2 = new TestSubscription();
            dlSubscription.Attach(subs2);

        public void DetachTwiceInactive()
        {
            var dlSubscription = new ActiveSubsciptionManager();

            var subs1 = new TestSubscription();
            dlSubscription.Attach(subs1);

            var subs2 = new TestSubscription();
            dlSubscription.Attach(subs2);

            Assert.IsTrue(dlSubscription.IsActive(subs1));
        }

        [TestMethod]
        public void DetachTwiceActive()
        {
            var dlSubscription = new ActiveSubsciptionManager();

            var subs1 = new TestSubscription();
            dlSubscription.Attach(subs1);
            Assert.IsTrue(dlSubscription.IsActive(subs1));

            var subs2 = new TestSubscription();
            dlSubscription.Attach(subs2);

            dlSubscription.Detach(subs1);
            Assert.IsTrue(dlSubscription.IsActive(subs2));
        }

        [TestMethod]

        [TestMethod]
        public void AllDetached()
        {
            var dlSubscription = new ActiveSubsciptionManager();

            var subs1 = new TestSubscription();
            dlSubscription.Attach(subs1);
            dlSubscription.Detach(subs1);

            Assert.IsFalse(dlSubscription.IsActive(subs1));
        }

        private class TestSubscription : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
