#if NET452
namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.Operation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class FrameworkSqlProcessingTest : IDisposable
    {
        private const int TimeAccuracyMilliseconds = 50;
        private const int SleepTimeMsecBetweenBeginAndEnd = 100;
        private TelemetryConfiguration configuration;
        private List<ITelemetry> sendItems;
        private FrameworkSqlProcessing sqlProcessingFramework;

        [TestInitialize]
        public void TestInitialize()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;

            this.configuration = new TelemetryConfiguration();
            this.sendItems = new List<ITelemetry>();
            this.configuration.TelemetryChannel = new StubTelemetryChannel { OnSend = item => this.sendItems.Add(item) };
        {
            Stopwatch stopwatchMax = Stopwatch.StartNew();
            this.sqlProcessingFramework.OnBeginExecuteCallback(
                id: 1111,
                database: "mydatabase",
                dataSource: "ourdatabase.database.windows.net",
                commandText: string.Empty);
            Stopwatch stopwatchMin = Stopwatch.StartNew();

            Thread.Sleep(SleepTimeMsecBetweenBeginAndEnd);
            this.sqlProcessingFramework.OnEndExecuteCallback(id: 1111, success: true, sqlExceptionNumber: 0);
            stopwatchMax.Stop();

            Assert.IsNull(Activity.Current);
            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
            ValidateTelemetryPacket(
                this.sendItems[0] as DependencyTelemetry,
                "ourdatabase.database.windows.net | mydatabase",
                "ourdatabase.database.windows.net | mydatabase",
                RemoteDependencyConstants.SQL,
                null);
        }

        [TestMethod]
        public void RddTestSqlProcessingFrameworkSendsCorrectTelemetrySqlQuerySuccessParentActivity()
        {
            var parentActivity = new Activity("parent").Start();
            parentActivity.AddBaggage("k", "v");
            parentActivity.TraceStateString = "tracestate";


        /// <summary>
        /// Validates SQLProcessingFramework sends correct telemetry for non stored procedure in async call.
        /// </summary>
        [TestMethod]
        public void RddTestSqlProcessingFrameworkSendsCorrectTelemetrySqlQuerySuccessW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;

            var parentActivity = new Activity("parent").Start();
            parentActivity.AddBaggage("k", "v");

            Stopwatch stopwatchMax = Stopwatch.StartNew();
            this.sqlProcessingFramework.OnBeginExecuteCallback(
                id: 1111,
                database: "mydatabase",
                dataSource: "ourdatabase.database.windows.net",
                commandText: string.Empty);
            Stopwatch stopwatchMin = Stopwatch.StartNew();

            Thread.Sleep(SleepTimeMsecBetweenBeginAndEnd);

            stopwatchMin.Stop();
            this.sqlProcessingFramework.OnEndExecuteCallback(id: 1111, success: true, sqlExceptionNumber: 0);
            stopwatchMax.Stop();

            Assert.AreEqual(parentActivity, Activity.Current);
            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
            ValidateTelemetryPacket(
                "ourdatabase.database.windows.net | mydatabase",
                "ourdatabase.database.windows.net | mydatabase",
                RemoteDependencyConstants.SQL,
                true,
                stopwatchMin.Elapsed.TotalMilliseconds,
                stopwatchMax.Elapsed.TotalMilliseconds,
                string.Empty,
                parentActivity);
        }

        /// </summary>
        [TestMethod]
        [Description("Validates SQLProcessingFramework sends correct telemetry for non stored procedure in async call.")]
        public void RddTestSqlProcessingFrameworkSendsCorrectTelemetryMultipleItems()
        {
            var parent = new Activity("parent").Start();

            for (int i = 0; i < 10; i++)
            {
                Stopwatch stopwatchMax = Stopwatch.StartNew();
                ValidateTelemetryPacket(
                    dependencyTelemetry,
                    "ourdatabase.database.windows.net | mydatabase",
                    "ourdatabase.database.windows.net | mydatabase",
                    RemoteDependencyConstants.SQL,
                    true,
                    stopwatchMin.Elapsed.TotalMilliseconds,
                    stopwatchMax.Elapsed.TotalMilliseconds,
                    string.Empty,
                    parent);
            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
            ValidateTelemetryPacket(
                this.sendItems[0] as DependencyTelemetry,
                true,
                stopwatchMin.Elapsed.TotalMilliseconds,
                stopwatchMax.Elapsed.TotalMilliseconds,
                string.Empty,
                null);
        }

        /// <summary>
        /// Validates SQLProcessingFramework sends correct telemetry for non stored procedure in failed call.
        /// </summary>
                stopwatchMax.Elapsed.TotalMilliseconds,
                "1",
                null);
        }

#if !NET452
        /// <summary>
        /// Validates SQLProcessingFramework sends correct telemetry.
        /// </summary>
        [TestMethod]
            Stopwatch stopwatchMin = Stopwatch.StartNew();

            Thread.Sleep(SleepTimeMsecBetweenBeginAndEnd);

            stopwatchMin.Stop();
            this.sqlProcessingFramework.OnEndExecuteCallback(id: 1111, success: true, sqlExceptionNumber: 0);
            stopwatchMax.Stop();

            Assert.IsNull(Activity.Current);
            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
                stopwatchMin.Elapsed.TotalMilliseconds,
                stopwatchMax.Elapsed.TotalMilliseconds,
                string.Empty,
                null);
        }
#endif

        #endregion

        #region Disposable
            GC.SuppressFinalize(this);
        }
        #endregion Disposable

        #region Helpers

        private static void ValidateTelemetryPacket(
            DependencyTelemetry remoteDependencyTelemetryActual,
            string target,
            string name,
            string type,
            bool success,
            double minDependencyDurationMs,
            double maxDependencyDurationMs,
            string errorCode,
            Activity parentActivity)
        {
            Assert.AreEqual(name, remoteDependencyTelemetryActual.Name, true, "Resource name in the sent telemetry is wrong");
            Assert.AreEqual(target, remoteDependencyTelemetryActual.Target, true, "Resource target in the sent telemetry is wrong");
            Assert.AreEqual(type.ToString(), remoteDependencyTelemetryActual.Type, "DependencyKind in the sent telemetry is wrong");
            Assert.AreEqual(expectedVersion, remoteDependencyTelemetryActual.Context.GetInternalContext().SdkVersion);

            if (parentActivity != null)
            {
                if (parentActivity.IdFormat == ActivityIdFormat.W3C)
                {
                    Assert.AreEqual(parentActivity.TraceId.ToHexString(), remoteDependencyTelemetryActual.Context.Operation.Id);
                    Assert.AreEqual(parentActivity.SpanId.ToHexString(), remoteDependencyTelemetryActual.Context.Operation.ParentId);
                    if (parentActivity.TraceStateString != null)
                    {
                }
                else
                {
                    Assert.AreEqual(parentActivity.RootId, remoteDependencyTelemetryActual.Context.Operation.Id);
                    Assert.AreEqual(parentActivity.Id, remoteDependencyTelemetryActual.Context.Operation.ParentId);
                }
            }
            else
            {
                Assert.IsNotNull(remoteDependencyTelemetryActual.Context.Operation.Id);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
