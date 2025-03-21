namespace Microsoft.ApplicationInsights
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Web;
    using Microsoft.ApplicationInsights.Web.Helpers;
    using Microsoft.ApplicationInsights.Web.Implementation;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.AspNet.TelemetryCorrelation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AspNetDiagnosticTelemetryModuleTest : IDisposable
    {
        private FakeAspNetDiagnosticSource aspNetDiagnosticsSource;
        private TelemetryConfiguration configuration;
        private IList<ITelemetry> sendItems;
        private AspNetDiagnosticTelemetryModule module;

        [TestInitialize]
        public void TestInit()
        {
            this.aspNetDiagnosticsSource = new FakeAspNetDiagnosticSource();
            this.sendItems = new List<ITelemetry>();
            var stubTelemetryChannel = new StubTelemetryChannel { OnSend = item => this.sendItems.Add(item) };

            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;
            this.configuration = new TelemetryConfiguration
            {
                InstrumentationKey = Guid.NewGuid().ToString(),
                TelemetryChannel = stubTelemetryChannel
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.Dispose(true);
            while (Activity.Current != null)
            {
                Activity.Current.Stop();
            }
        }

        [TestMethod]
        public void InitializeWithoutRequestAndExceptionModulesMakesModuleNoop()
        {
            this.module = new AspNetDiagnosticTelemetryModule();
            this.module.Initialize(this.configuration);

            this.aspNetDiagnosticsSource.StartActivity();
            Assert.AreEqual(0, this.sendItems.Count);
        }

        [TestMethod]
        public void IsEnabledWithNullActivityDoesNotThrow()
        {
            this.module = this.CreateModule();

            Assert.IsTrue(this.aspNetDiagnosticsSource.IsEnabled(FakeAspNetDiagnosticSource.IncomingRequestEventName));
            Assert.AreEqual(0, this.sendItems.Count);
        }

        [TestMethod]
        public void BeginEndRequestReportsTelemetry()
        {
            this.module = this.CreateModule();

            Assert.IsTrue(this.aspNetDiagnosticsSource.StartActivity());
            this.aspNetDiagnosticsSource.StopActivity();
            Assert.AreEqual(1, this.sendItems.Count);
        }

        [TestMethod]
        public void ChildTelemetryIsReportedProperlyBetweenBeginEndRequest()
        {
            this.module = this.CreateModule();

            this.aspNetDiagnosticsSource.StartActivity();

            var trace = new TraceTelemetry();
            var client = new TelemetryClient(this.configuration);
            client.TrackTrace(trace);

            this.aspNetDiagnosticsSource.StopActivity();
            Assert.AreEqual(2, this.sendItems.Count);

            var requestTelemetry = this.sendItems[0] as RequestTelemetry ?? this.sendItems[1] as RequestTelemetry;
            Assert.IsNotNull(requestTelemetry);

            Assert.AreEqual(requestTelemetry.Id, trace.Context.Operation.ParentId);
            Assert.AreEqual(requestTelemetry.Context.Operation.Id, trace.Context.Operation.Id);
        }

        // When telemetry is reported before AspNetDiagnosticsSource gets Start event
        // we create an activity in App Insights.
        // If this activity is lost on the way to BeginRequest on TelemteryCorrelation module
        // there is no way to correlate before/after telemetry -
        // TelemetryCorrelation module must be first in the pipeline, otherwise correlation is not guaranteed
        // see https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/1049
        [Ignore]
        [TestMethod]
        public async Task TelemetryReportedBeforeAndAfterOnBeginAndLostActivity()
        {
            this.module = this.CreateModule();
            var client = new TelemetryClient(this.configuration);

            var trace1 = new TraceTelemetry("test1");
            await Task.Run(() =>
            {

            Assert.AreEqual(requestTelemetry.Context.Operation.Id, trace1.Context.Operation.Id);
            Assert.AreEqual(requestTelemetry.Context.Operation.Id, trace2.Context.Operation.Id);

            Assert.AreEqual(requestTelemetry.Id, trace1.Context.Operation.ParentId);
            Assert.AreEqual(requestTelemetry.Id, trace2.Context.Operation.ParentId);
        }

        [TestMethod]
        public void IsEnabledIsFalseIfRequestTelemetryIsCreatedAndCurrentActivityIsFromTelemetryCorrelation()

        [TestMethod]
        public void DoubleBeginEndRequestReportsOneTelemetry()
        {
            this.module = this.CreateModule();

            Assert.IsTrue(this.aspNetDiagnosticsSource.StartActivity());
            var activity = Activity.Current;
            Assert.IsTrue(this.aspNetDiagnosticsSource.StartActivity());

                HttpModuleHelper.GetFakeHttpContext(new Dictionary<string, string>
                {
                    ["Request-Id"] = "|guid2.",
                    ["x-ms-request-id"] = "guid1",
                    ["x-ms-request-root-id"] = "guid2"
                });

            this.module = this.CreateModule("x-ms-request-root-id", "x-ms-request-id");

            var activity = new Activity(FakeAspNetDiagnosticSource.IncomingRequestEventName);
            Assert.IsNotNull(requestTelemetry);
            Assert.AreEqual("guid2", requestTelemetry.Context.Operation.Id);
            Assert.AreEqual("|guid2.", requestTelemetry.Context.Operation.ParentId);
            Assert.AreEqual(activity.Id, requestTelemetry.Id);
        }

        [TestMethod]
        public void RequestTelemetryCustomHeadersW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;

            this.module = this.CreateModule("rootHeaderName", "parentHeaderName");
            this.aspNetDiagnosticsSource.FakeContext =
                HttpModuleHelper.GetFakeHttpContext(new Dictionary<string, string>
                {
                    ["parentHeaderName"] = "ParentId",
                    ["rootHeaderName"] = "RootId"
                });

            this.aspNetDiagnosticsSource.StartActivityWithoutChecks(activity);
            Assert.AreEqual(activity, Activity.Current);
            this.aspNetDiagnosticsSource.StopActivity();

            Assert.AreEqual("RootId", activity.ParentId);
            Assert.AreEqual(1, this.sendItems.Count);

            var requestTelemetry = this.sendItems[0] as RequestTelemetry;
            Assert.IsNotNull(requestTelemetry);
            Assert.AreEqual("RootId", requestTelemetry.Context.Operation.Id);
                    ["x-ms-request-id"] = "legacy-id",
                    ["x-ms-request-root-id"] = "legacy-root-id"
                });

            this.module = this.CreateModule();

            var activity = new Activity(FakeAspNetDiagnosticSource.IncomingRequestEventName);
            Assert.IsTrue(this.aspNetDiagnosticsSource.IsEnabled(FakeAspNetDiagnosticSource.IncomingRequestEventName, activity));

            this.aspNetDiagnosticsSource.StartActivityWithoutChecks(activity);
            Assert.AreEqual(activity, Activity.Current);
            this.aspNetDiagnosticsSource.StopActivity();

            Assert.AreEqual(1, this.sendItems.Count);

            var requestTelemetry = this.sendItems[0] as RequestTelemetry;
            Assert.IsNotNull(requestTelemetry);
            Assert.AreEqual("requestId", requestTelemetry.Context.Operation.Id);
            Assert.AreEqual("|requestId.", requestTelemetry.Context.Operation.ParentId);
            Assert.AreEqual(activity.Id, requestTelemetry.Id);
        [TestMethod]
        public void TestActivityIdGenerationWithEmptyHeaders()
        {
            this.module = this.CreateModule();

            this.aspNetDiagnosticsSource.StartActivity();
            var activity = Activity.Current;
            this.aspNetDiagnosticsSource.StopActivity();
            
            Assert.AreEqual(1, this.sendItems.Count);
        }

        [TestMethod]
        public void TestActivityIdGenerationWithW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;
            this.module = this.CreateModule();
            
            this.aspNetDiagnosticsSource.StartActivity();
            Activity activity = Activity.Current;

            this.aspNetDiagnosticsSource.StopActivity();

            var request = this.sendItems.OfType<RequestTelemetry>().Single();

            Assert.AreEqual(activity.RootId, request.Context.Operation.Id);
            Assert.AreEqual(activity.ParentId, request.Context.Operation.ParentId);
            Assert.AreEqual(activity.Id, request.Id);
        }
                HttpModuleHelper.GetFakeHttpContext(new Dictionary<string, string>
                {
                    ["traceparent"] = "00-4bf92f3577b34da6a3ce929d0e0e4736-00f067aa0ba902b7-01",
                    ["x-ms-request-id"] = "legacy-id",
                    ["x-ms-request-rooit-id"] = "legacy-root-id"
                });

            this.module = this.CreateModule("x-ms-request-root-id", "x-ms-request-id");

            var activity = new Activity(FakeAspNetDiagnosticSource.IncomingRequestEventName);

            Assert.AreEqual(1, this.sendItems.Count);

            var requestTelemetry = this.sendItems[0] as RequestTelemetry;
            Assert.IsNotNull(requestTelemetry);
            Assert.AreEqual("4bf92f3577b34da6a3ce929d0e0e4736", requestTelemetry.Context.Operation.Id);
            Assert.AreEqual("00f067aa0ba902b7", requestTelemetry.Context.Operation.ParentId);
            Assert.AreEqual(activity.SpanId.ToHexString(), requestTelemetry.Id);

            Assert.AreEqual(0, requestTelemetry.Properties.Count);
        public void W3CHeadersWinOverRequestId()
        {
            this.aspNetDiagnosticsSource.FakeContext =
                HttpModuleHelper.GetFakeHttpContext(new Dictionary<string, string>
                {
                    ["traceparent"] = "00-4bf92f3577b34da6a3ce929d0e0e4736-00f067aa0ba902b7-01",
                    ["Request-Id"] = "|requestId."
                });

            this.module = this.CreateModule();
            Assert.AreEqual(activity, Activity.Current);
            this.aspNetDiagnosticsSource.StopActivity();

            Assert.AreEqual(1, this.sendItems.Count);

            var requestTelemetry = this.sendItems[0] as RequestTelemetry;
            Assert.IsNotNull(requestTelemetry);
            Assert.AreEqual(activity.TraceId.ToHexString(), requestTelemetry.Context.Operation.Id);
            Assert.AreEqual("|requestId.", requestTelemetry.Context.Operation.ParentId);
            Assert.AreEqual(activity.SpanId.ToHexString(), requestTelemetry.Id);
            // here is where OnError happens
            var exceptionTelemetry = this.sendItems.OfType<ExceptionTelemetry>().FirstOrDefault();

            Assert.IsNotNull(requestTelemetry);
            Assert.AreEqual("4bf92f3577b34da6a3ce929d0e0e4736", requestTelemetry.Context.Operation.Id);
            Assert.AreEqual("|4bf92f3577b34da6a3ce929d0e0e4736.", requestTelemetry.Context.Operation.ParentId);
            Assert.AreEqual(appInsightsActivity.SpanId.ToHexString(), requestTelemetry.Id);

            Assert.IsNotNull(exceptionTelemetry);
            Assert.AreEqual("4bf92f3577b34da6a3ce929d0e0e4736", exceptionTelemetry.Context.Operation.Id);
            Assert.AreEqual(requestTelemetry.Id, exceptionTelemetry.Context.Operation.ParentId);
        }

        [TestMethod]
        public void RequestIdBecomesParentAndRootIfCompatibleWhenThereAreNoW3CHeaders()
        {
            this.aspNetDiagnosticsSource.FakeContext =
                HttpModuleHelper.GetFakeHttpContext(new Dictionary<string, string>
                {
                    ["Request-Id"] = "|4bf92f3577b34da6a3ce929d0e0e4736.",
                    ["Correlation-Context"] = "k=v",
            Assert.AreEqual("0000000000000000", currentActivity.ParentSpanId.ToHexString());

            Assert.AreEqual(1, this.sendItems.Count);

            var requestTelemetry = this.sendItems[0] as RequestTelemetry;
            Assert.IsNotNull(requestTelemetry);
            Assert.AreEqual("4bf92f3577b34da6a3ce929d0e0e4736", requestTelemetry.Context.Operation.Id);
            Assert.AreEqual("|4bf92f3577b34da6a3ce929d0e0e4736.", requestTelemetry.Context.Operation.ParentId);
            Assert.AreEqual(currentActivity.SpanId.ToHexString(), requestTelemetry.Id);

        {
            this.aspNetDiagnosticsSource.FakeContext =
                HttpModuleHelper.GetFakeHttpContext(new Dictionary<string, string>
                {
                    ["rootHeaderName"] = "root",
                    ["parentHeaderName"] = "parent"
                });
            this.module = this.CreateModule("rootHeaderName", "parentHeaderName");

            var activity = new Activity(FakeAspNetDiagnosticSource.IncomingRequestEventName);
            activity.Extract(HttpContext.Current.Request.Headers);

            Assert.IsTrue(this.aspNetDiagnosticsSource.IsEnabled(FakeAspNetDiagnosticSource.IncomingRequestEventName, activity));
            this.aspNetDiagnosticsSource.StartActivityWithoutChecks(activity);
            Assert.AreEqual(activity, Activity.Current);
            this.aspNetDiagnosticsSource.StopActivity();

            Assert.AreEqual(1, this.sendItems.Count);

            var requestTelemetry = this.sendItems[0] as RequestTelemetry;

            Assert.IsTrue(requestTelemetry.Properties.TryGetValue("ai_legacyRootId", out var legacyRootId));
            Assert.AreEqual("root", legacyRootId);
            Assert.AreEqual(1, requestTelemetry.Properties.Count);
        }

        [TestMethod]
        public void CorrelationContextIsReadWithoutTraceparentAndRequestId()
        {
            this.aspNetDiagnosticsSource.FakeContext =
                HttpModuleHelper.GetFakeHttpContext(new Dictionary<string, string>
                {
                    ["Correlation-Context"] = "k=v",
                });
            this.module = this.CreateModule();

            var activity = new Activity(FakeAspNetDiagnosticSource.IncomingRequestEventName);
            Assert.IsTrue(this.aspNetDiagnosticsSource.IsEnabled(FakeAspNetDiagnosticSource.IncomingRequestEventName, activity));
            this.aspNetDiagnosticsSource.StartActivityWithoutChecks(activity);
            var currentActivity = Activity.Current;

            Assert.AreEqual(1, activity.Baggage.Count());
            Assert.AreEqual("k", activity.Baggage.Single().Key);
            Assert.AreEqual("v", activity.Baggage.Single().Value);

            this.aspNetDiagnosticsSource.StopActivity();

            Assert.AreEqual(1, this.sendItems.Count);

            var requestTelemetry = this.sendItems[0] as RequestTelemetry;
            Assert.IsNotNull(requestTelemetry);
            Assert.AreEqual(currentActivity.TraceId.ToHexString(), requestTelemetry.Context.Operation.Id);
            Assert.IsNull(requestTelemetry.Context.Operation.ParentId);
            Assert.AreEqual(currentActivity.SpanId.ToHexString(), requestTelemetry.Id);

            Assert.AreEqual(1, requestTelemetry.Properties.Count);
            Assert.IsTrue(requestTelemetry.Properties.TryGetValue("k", out var v));
            Assert.AreEqual("v", v);
        }

        [TestMethod]
        public void CorrelationContextIsReadWithoutTraceparentAndRequestIdW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;
            this.aspNetDiagnosticsSource.FakeContext =
                HttpModuleHelper.GetFakeHttpContext(new Dictionary<string, string>
                {
                    ["Correlation-Context"] = "k=v",
                });
            this.module = this.CreateModule();

            var activity = new Activity(FakeAspNetDiagnosticSource.IncomingRequestEventName);
            Assert.IsTrue(this.aspNetDiagnosticsSource.IsEnabled(FakeAspNetDiagnosticSource.IncomingRequestEventName, activity));
            this.aspNetDiagnosticsSource.StartActivityWithoutChecks(activity);
            var currentActivity = Activity.Current;

            Assert.AreEqual(1, activity.Baggage.Count());
            Assert.AreEqual("k", activity.Baggage.Single().Key);
            Assert.AreEqual("v", activity.Baggage.Single().Value);

        private AspNetDiagnosticTelemetryModule CreateModule(string rootIdHeaderName = null, string parentIdHeaderName = null)
        {
            var initializer = new Web.OperationCorrelationTelemetryInitializer();
            if (rootIdHeaderName != null)
            {
                initializer.RootOperationIdHeaderName = rootIdHeaderName;
            }

            if (parentIdHeaderName != null)
            TelemetryModules.Instance.Modules.Add(requestModule);
            TelemetryModules.Instance.Modules.Add(exceptionModule);

            result.Initialize(this.configuration);

            return result;
        }

        private void Dispose(bool dispose)
        {
            {
                this.listener = new DiagnosticListener(AspNetListenerName);
                this.fakeContext = HttpModuleHelper.GetFakeHttpContext();
                HttpContext.Current = this.fakeContext;
            }

            public HttpContext FakeContext
            {
                get => this.fakeContext;



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
