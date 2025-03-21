namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QuickPulseQuotaTrackerTests
    {
        [TestMethod]
        public void QuickPulseQuotaTrackerQuotaNotEmptyAtStart()
        {
            // ARRANGE
            int startQuota = 10;
            bool counted;
            var mockTimeProvider = new ClockMock();
            QuickPulseQuotaTracker quotaTracker = new QuickPulseQuotaTracker(mockTimeProvider, 30, startQuota);

            // ACT & ASSERT
            while (startQuota > 0)
            {
                Assert.IsFalse(quotaTracker.QuotaExhausted);
                counted = quotaTracker.ApplyQuota();
                Assert.IsTrue(counted);
                --startQuota;
            }

            // Quota should be exhausted.
            counted = quotaTracker.ApplyQuota();
            Assert.IsFalse(counted);
            Assert.AreEqual(0f, quotaTracker.CurrentQuota);
            // ARRANGE
            var mockTimeProvider = new ClockMock();
            QuickPulseQuotaTracker quotaTracker = new QuickPulseQuotaTracker(mockTimeProvider, 30, 0);
            bool counted;

            // ACT & ASSERT
            Assert.AreEqual(0, quotaTracker.CurrentQuota);
            Assert.IsTrue(quotaTracker.QuotaExhausted);

            counted = quotaTracker.ApplyQuota();
            Assert.IsFalse(counted); // No quota yet
            Assert.AreEqual(0, quotaTracker.CurrentQuota);
            Assert.IsTrue(quotaTracker.QuotaExhausted);

            mockTimeProvider.FastForward(TimeSpan.FromSeconds(1)); // 0.5 quota accumulated
            
            counted = quotaTracker.ApplyQuota();
            Assert.IsFalse(counted); // No quota yet
            Assert.AreEqual(0.5f, quotaTracker.CurrentQuota);
            Assert.IsTrue(quotaTracker.QuotaExhausted);
            var mockTimeProvider = new ClockMock();
            QuickPulseQuotaTracker quotaTracker = new QuickPulseQuotaTracker(mockTimeProvider, maxQuota, startQuota);
            bool counted;

            mockTimeProvider.FastForward(TimeSpan.FromDays(1));

            // ACT & ASSERT
            while (maxQuota > 0)
            {
                counted = quotaTracker.ApplyQuota();
            // only one document every 2nd second (quota = 30 documents per min).
            for (int i = 0; i < 1000; i++)
            {
                int count = 0;
                for (int j = 0; j < 100; j++)
                {
                    bool counted = quotaTracker.ApplyQuota();
                    if (counted)
                    {
                        ++count;
                    }
                }

                Assert.AreEqual((i % 2) == 0 ? 0 : 1, count);

                mockTimeProvider.FastForward(TimeSpan.FromSeconds(1));
            }
        }

        [TestMethod]
                var tasks = new List<Action>();
                for (int j = 0; j < concurrency; j++)
                {
                    tasks.Add(() => quotaApplicationResults.Enqueue(quotaTracker.ApplyQuota()));
                }

                Parallel.Invoke(new ParallelOptions() { MaxDegreeOfParallelism = concurrency }, tasks.ToArray());
            }

            // ACT
            for (int i = 0; i < experimentLengthInSeconds; i++)
            {
                mockTimeProvider.FastForward(TimeSpan.FromSeconds(1));

                quotaTracker.ApplyQuota();
            }

            // ASSERT


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
