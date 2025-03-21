#if NET452
namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Net;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    /// <summary>
    /// Tests for client server dependency tracker.
    /// </summary>
    [TestClass]
    public class ClientServerDependencyTrackerTests : IDisposable
    {
        private List<ITelemetry> sendItems;
        private TelemetryClient telemetryClient;
        private WebRequest webRequest;
        private SqlCommand sqlRequest;

        [TestInitialize]
        public void TestInitialize()
        {
            var configuration = new TelemetryConfiguration();
            this.sendItems = new List<ITelemetry>();
            configuration.TelemetryChannel = new StubTelemetryChannel { OnSend = item => this.sendItems.Add(item) };
            configuration.InstrumentationKey = Guid.NewGuid().ToString();
            configuration.TelemetryInitializers.Add(new MockTelemetryInitializer());
            configuration.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
            this.telemetryClient = new TelemetryClient(configuration);
            this.webRequest = WebRequest.Create(new Uri("http://bing.com"));
            this.sqlRequest = new SqlCommand("select * from table;");
            ClientServerDependencyTracker.PretendProfilerIsAttached = true;
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;

            while (Activity.Current != null)
            {
                Activity.Current.Stop();
            }

            ClientServerDependencyTracker.PretendProfilerIsAttached = false;
        }

#if !NET452
        /// <summary>
        /// Tests if BeginWebTracking() returns operation with associated telemetry item (with start time and time stamp).
        /// </summary>
        [TestMethod]
        public void BeginWebTrackingReturnsOperationItemWithTelemetryItem()
        {
            var telemetry = ClientServerDependencyTracker.BeginTracking(this.telemetryClient);
            Assert.AreEqual(telemetry.Timestamp, telemetry.Timestamp);

            Assert.AreEqual(currentActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
            Assert.AreEqual(currentActivity.ParentSpanId.ToHexString(), telemetry.Context.Operation.ParentId);

            var properties = telemetry.Properties;
            Assert.AreEqual(2, properties.Count);
            Assert.AreEqual("v", properties["k"]);
            Assert.AreEqual("state=some", properties["tracestate"]);
            parentActivity.Stop();
        }


            Assert.AreEqual(currentActivity.SpanId.ToHexString(), telemetry.Id);
            Assert.AreEqual(currentActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
            Assert.IsNull(telemetry.Context.Operation.ParentId);

            var properties = telemetry.Properties;
            Assert.AreEqual(0, properties.Count);
        }

        [TestMethod]
            activity.Start();

            var telemetry = ClientServerDependencyTracker.BeginTracking(this.telemetryClient);

            Assert.AreEqual(activity.SpanId.ToHexString(), telemetry.Id);
            Assert.AreEqual(activity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
            Assert.AreEqual(activity.ParentSpanId.ToHexString(), telemetry.Context.Operation.ParentId);

            var properties = telemetry.Properties;
            Assert.AreEqual(1, properties.Count);
        [TestMethod]
        public void BeginWebTrackingWithDesktopParentActivityReturnsOperationItemWithTelemetryItemW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;
            var parentActivity = new Activity("System.Net.Http.Desktop.HttpRequestOut");
            parentActivity.SetParentId("|guid.1234_");
            parentActivity.AddBaggage("k", "v");

            parentActivity.Start();

            var telemetry = ClientServerDependencyTracker.BeginTracking(this.telemetryClient);
            Assert.AreEqual(parentActivity.Id, telemetry.Id);
            Assert.AreEqual(parentActivity.RootId, telemetry.Context.Operation.Id);
            Assert.AreEqual(parentActivity.ParentId, telemetry.Context.Operation.ParentId);

            var properties = telemetry.Properties;
            Assert.AreEqual(1, properties.Count);
            Assert.AreEqual("v", properties["k"]);
            parentActivity.Stop();
        }

        /// <summary>
        /// Tests if EndTracking() sends telemetry item on success for web and SQL requests.
        /// </summary>
        [TestMethod]
        public void EndTrackingSendsTelemetryItemOnSuccess()
        {
            var telemetry = ClientServerDependencyTracker.BeginTracking(this.telemetryClient);
            ClientServerDependencyTracker.EndTracking(this.telemetryClient, telemetry);

            telemetry = ClientServerDependencyTracker.BeginTracking(this.telemetryClient);
            ClientServerDependencyTracker.EndTracking(this.telemetryClient, telemetry);
            Assert.AreEqual(2, this.sendItems.Count);
        }

        /// <summary>
        /// Tests if EndTracking() computes the Duration of the telemetry item.
        /// </summary>
        [TestMethod]
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetTupleForSqlDependenciesThrowsArgumentNullExceptionForNullSqlRequest()
        {
            ClientServerDependencyTracker.GetTupleForSqlDependencies(null);
        }

        [TestMethod]
        }

        [TestMethod]
        public void GetTupleForWebDependenciesReturnsNullIfEntryDoesNotExistInTables()
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddTupleForSqlDependenciesThrowsArgumentNullExceptionForNullSqlRequest()
        {
            ClientServerDependencyTracker.AddTupleForSqlDependencies(null, new DependencyTelemetry(), false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddTupleForWebDependenciesThrowsArgumentNullExceptionForNullWebRequest()
        {
            var tuple = ClientServerDependencyTracker.GetTupleForWebDependencies(this.webRequest);
            Assert.IsNotNull(tuple);
            Assert.IsNotNull(tuple.Item1);
            Assert.AreEqual(telemetry, tuple.Item1);
        }

        [TestMethod]
        public void GetTupleForWebDependenciesReturnsNullIfTheItemDoesNotExistInTheTable()
        {
            var tuple = ClientServerDependencyTracker.GetTupleForWebDependencies(this.webRequest);
            ClientServerDependencyTracker.AddTupleForSqlDependencies(this.sqlRequest, telemetry, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTupleForWebDependenciesThrowsExceptionIfExists()
        {
            var telemetry = new DependencyTelemetry();
            var falseTelemetry = new DependencyTelemetry();
            ClientServerDependencyTracker.AddTupleForWebDependencies(this.webRequest, falseTelemetry, false);
            ClientServerDependencyTracker.AddTupleForWebDependencies(this.webRequest, telemetry, false);
        }

        [TestMethod]
        public void GetTupleForSqlDependenciesReturnsNullIfTheItemDoesNotExistInTheTable()
        {
            var tuple = ClientServerDependencyTracker.GetTupleForSqlDependencies(this.sqlRequest);
            Assert.IsNull(tuple);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this); 
        }

        private void Dispose(bool dispose)
        {
            if (dispose)
            {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
