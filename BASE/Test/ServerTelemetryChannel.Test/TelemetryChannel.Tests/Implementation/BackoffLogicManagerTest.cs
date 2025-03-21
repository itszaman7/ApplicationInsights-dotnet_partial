namespace Microsoft.ApplicationInsights.WindowsServer.Channel.Implementation
{
    using System;
    using System.Diagnostics.Tracing;

    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.WindowsServer.Channel.Helpers;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Microsoft.ApplicationInsights.Channel.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;

    public class BackoffLogicManagerTest
    {
        [TestClass]
        public class DefaultBackoffEnabledReportingInterval
        {
            [TestMethod]
            public void DefaultReportingIntervalInMinIs30Min()
            {
                Assert.AreEqual(30, new BackoffLogicManager().DefaultBackoffEnabledReportingInterval.TotalMinutes);
            }
        }

        [TestClass]
        public class GetBackendResponse
        {
            [TestMethod]
            public void ReturnNullIfArgumentIsNull()
            {
                Assert.IsNull(BackoffLogicManager.GetBackendResponse(null));
            }

            [TestMethod]
            public void ReturnNullIfArgumentEmpty()
            {
                Assert.IsNull(BackoffLogicManager.GetBackendResponse(string.Empty));
            }

            [TestMethod]
            public void IfContentCannotBeParsedNullIsReturned()
            {
                Assert.IsNull(BackoffLogicManager.GetBackendResponse("ab}{"));
            }

            [TestMethod]
            public void IfContentIsUnexpectedJsonNullIsReturned()
            {
                Assert.IsNull(BackoffLogicManager.GetBackendResponse("[1,2]"));
        {
            [TestMethod]
            public void NoErrorDelayIsSameAsSlotDelay()
            {
                var manager = new BackoffLogicManager(TimeSpan.Zero);
                manager.GetBackOffTimeInterval(string.Empty);
                Assert.AreEqual(TimeSpan.FromSeconds(10), manager.CurrentDelay);
            }

            [TestMethod]
            public void FirstErrorDelayIsSameAsSlotDelay()
            {
                var manager = new BackoffLogicManager(TimeSpan.Zero);
                manager.ReportBackoffEnabled(500);
                manager.GetBackOffTimeInterval(string.Empty);

            [TestMethod]
            public void RetryAfterFromHeadersHasMorePriorityThanExponentialRetry()
            {                
                var manager = new BackoffLogicManager(TimeSpan.Zero);
                manager.GetBackOffTimeInterval(DateTimeOffset.UtcNow.AddSeconds(30).ToString("O"));

                AssertEx.InRange(manager.CurrentDelay, TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(30));
            }

                var manager = new BackoffLogicManager(TimeSpan.Zero);
                manager.GetBackOffTimeInterval("no one can parse me");
                Assert.AreEqual(TimeSpan.FromSeconds(10), manager.CurrentDelay);
            }

            [TestMethod]
            public void RetryAfterOlderThanNowCausesDefaultDelay()
            {
                // An old date
                string retryAfterDateString = DateTime.UtcNow.AddMinutes(-1).ToString("R", CultureInfo.InvariantCulture);

                var manager = new BackoffLogicManager(TimeSpan.Zero);
                manager.GetBackOffTimeInterval(retryAfterDateString);
                Assert.AreEqual(TimeSpan.FromSeconds(10), manager.CurrentDelay);
            }
        }

        [TestClass]
        public class ReportDiagnosticMessage
        {
                {
                    const long AllKeywords = -1;
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.Error, (EventKeywords)AllKeywords);

                    var manager = new BackoffLogicManager(TimeSpan.Zero);
                    manager.ReportBackoffEnabled(200);
                    manager.ReportBackoffEnabled(200);

                    var traces = listener.Messages.ToList();

            public void ReportBackoffWriteDoesNotLogMessagesBeforeIntervalPasses()
            {
                // this test fails when run in parallel with other tests
                using (var listener = new TestEventListener(waitForDelayedEvents: false))
                {
                    const long AllKeywords = -1;
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.Error, (EventKeywords)AllKeywords);

                    var manager = new BackoffLogicManager(TimeSpan.FromSeconds(20));

                using (var listener = new TestEventListener())
                {
                    const long AllKeywords = -1;
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.Error, (EventKeywords)AllKeywords);

                    var manager = new BackoffLogicManager(TimeSpan.FromMilliseconds(10));

                    System.Threading.Thread.Sleep(10);

                    manager.ReportBackoffEnabled(200);
            public void DisableDoesNotLogMessageIfEnabledWasNotCalled()
            {
                // this test may fail when other tests running in parallel
                using (var listener = new TestEventListener(waitForDelayedEvents: false))
                {
                    const long AllKeywords = -1;
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.Error, (EventKeywords)AllKeywords);

                    var manager = new BackoffLogicManager(TimeSpan.Zero);

                    manager.ReportBackoffDisabled();
                    manager.ReportBackoffDisabled();

                    var traces = listener.Messages.ToList();
                    Assert.AreEqual(0, traces.Count);
                }
            }
        }

        [TestClass]
                for (int i = 0; i < 10; ++i)
                {
                    tasks[i] = Task.Run(() => manager.ReportBackoffEnabled(500));
                }

                Task.WaitAll(tasks);

                Assert.AreEqual(1, manager.ConsecutiveErrors);
            }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
