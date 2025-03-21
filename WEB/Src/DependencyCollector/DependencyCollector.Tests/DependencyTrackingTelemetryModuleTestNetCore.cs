namespace Microsoft.ApplicationInsights.Tests
{
#if NETCOREAPP
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId;

    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// .NET Core specific tests that verify Http Dependencies are collected for outgoing request
    /// </summary>
    [TestClass]
    public class DependencyTrackingTelemetryModuleTestNetCore
    {
        private const string IKey = "F8474271-D231-45B6-8DD4-D344C309AE69";
        private const string FakeProfileApiEndpoint = "https://dc.services.visualstudio.com/v2/track";
        private const string localhostUrl = "http://localhost:5050";
        private const string expectedAppId = "cid-v1:someAppId";

        private readonly OperationDetailsInitializer operationDetailsInitializer = new OperationDetailsInitializer();
        private readonly DictionaryApplicationIdProvider appIdProvider = new DictionaryApplicationIdProvider();
        private StubTelemetryChannel channel;
        private TelemetryConfiguration config;
        private List<DependencyTelemetry> sentTelemetry;

        /// <summary>
        /// Initialize.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;
            this.sentTelemetry = new List<DependencyTelemetry>();
            this.channel = new StubTelemetryChannel
            {
                OnSend = telemetry =>
                {
                    // The correlation id lookup service also makes http call, just make sure we skip that
                    DependencyTelemetry depTelemetry = telemetry as DependencyTelemetry;
                    if (depTelemetry != null)
                    {
                        this.sentTelemetry.Add(depTelemetry);
                    }
                },
                EndpointAddress = FakeProfileApiEndpoint
            };

            this.appIdProvider.Defined = new Dictionary<string, string>
            {
                [IKey] = expectedAppId
            };

            this.config = new TelemetryConfiguration
            {
                InstrumentationKey = IKey,
                TelemetryChannel = this.channel,
                ApplicationIdProvider = this.appIdProvider
            };

            this.config.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
            this.config.TelemetryInitializers.Add(this.operationDetailsInitializer);
        }

        /// <summary>
        /// Cleans up.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            while (Activity.Current != null)
            {
                Activity.Current.Stop();
            }
        }

        /// <summary>
        /// Tests that dependency is collected properly when there is no parent activity.
        /// </summary>
        [TestMethod]
        [Timeout(5000)]
        public async Task TestDependencyCollectionWithLegacyHeaders()
                // DiagnosticSource Response event is fired after SendAsync returns on netcoreapp1.*
                // let's wait until dependency is collected
                Assert.IsTrue(SpinWait.SpinUntil(() => this.sentTelemetry != null, TimeSpan.FromSeconds(1)));

                this.ValidateTelemetryForDiagnosticSource(
                    this.sentTelemetry.Single(), 
                    url, 
                    request, 
                    true, 
                    "200", 
                    true,
                    true,
                    true);
            }
        }

        /// <summary>
        /// Tests that dependency is collected properly when there is no parent activity.
        /// </summary>
        [TestMethod]

        [TestMethod]
        [Timeout(5000)]
        public async Task TestDependencyCollectionBackOffsWhenTraceparentHeaderIsPresent()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;

            using (var module = new DependencyTrackingTelemetryModule())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("traceparent", "00-0123456789abcdef0123456789abcdef-0123456789abcdef-01");
                using (new LocalServer(localhostUrl))
                {
                    await new HttpClient().SendAsync(request);
                }

                // DiagnosticSource Response event is fired after SendAsync returns on netcoreapp1.*
                // let's wait until dependency is collected
                Assert.IsTrue(SpinWait.SpinUntil(() => this.sentTelemetry.Count > 0, TimeSpan.FromSeconds(1)));
        [TestMethod]
        [Timeout(5000)]
        public async Task TestDependencyCollectionWhenTraceparentHeaderIsPresentAndW3COff()
        {
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.config);

                var url = new Uri(localhostUrl);
                var request = new HttpRequestMessage(HttpMethod.Get, url);
        [Timeout(5000)]
        public async Task TestDependencyCollectionWithParentActivity()
        {
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.config);

                var parent = new Activity("parent").AddBaggage("k", "v").SetParentId("|guid.").Start();

                var url = new Uri(localhostUrl);
                {
                    await new HttpClient().SendAsync(request);
                }

                // DiagnosticSource Response event is fired after SendAsync returns on netcoreapp1.*
                // let's wait until dependency is collected
                Assert.IsTrue(SpinWait.SpinUntil(() => this.sentTelemetry != null, TimeSpan.FromSeconds(1)));

                this.ValidateTelemetryForDiagnosticSource(
                    this.sentTelemetry.Single(), 
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.config);

                var url = $"http://{Guid.NewGuid()}:5050";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                await new HttpClient().SendAsync(request).ContinueWith(t => { });
                // As DNS Resolution itself failed, no response expected
                this.ValidateTelemetryForDiagnosticSource(
                    this.sentTelemetry.Single(), 
                    new Uri(url),
                    request,
                    false, 
                    "Faulted", 
                    false,
                    true,
                    true,
                    responseExpected: false);
            }
        }
                    parent);

                parent.Stop();
            }
        }

        /// <summary>
        /// Tests that dependency is collected properly when there is parent activity.
        /// </summary>
        [TestMethod]
        [Timeout(5000)]
        public async Task TestDependencyCollectionWithW3COnRequestIdOff()
        {
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.EnableRequestIdHeaderInjectionInW3CMode = false;
                module.Initialize(this.config);

                var parent = new Activity("parent")
                    .Start();

                var url = new Uri(localhostUrl);
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                using (new LocalServer(localhostUrl))
                {
                    await new HttpClient().SendAsync(request);
                }

                // DiagnosticSource Response event is fired after SendAsync returns on netcoreapp1.*
                // let's wait until dependency is collected
                    request: request,
                    success: true,
                    resultCode: "200",
                    expectLegacyHeaders: false,
                    expectW3CHeaders: true,
                    expectRequestId: false,
                    responseExpected: true,
                    parentActivity: parent);

                parent.Stop();
            }
        }

        /// <summary>
        /// Tests that dependency is collected properly when there is parent activity.
        /// </summary>
        [TestMethod]
        [Timeout(5000)]
        public async Task TestDependencyCollectionWithW3CHeadersWithState()
        {

                var url = new Uri(localhostUrl);
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                using (new LocalServer(localhostUrl))
                {
                    await new HttpClient().SendAsync(request);
                }

                // DiagnosticSource Response event is fired after SendAsync returns on netcoreapp1.*
                // let's wait until dependency is collected

            {
                if (parentActivity.IdFormat == ActivityIdFormat.W3C)
                {
                    Assert.AreEqual(parentActivity.TraceId.ToHexString(), item.Context.Operation.Id);
                    Assert.AreEqual(parentActivity.SpanId.ToHexString(), item.Context.Operation.ParentId);
                    if (parentActivity.TraceStateString != null)
                    {
                        Assert.IsTrue(item.Properties.ContainsKey("tracestate"));
                        Assert.AreEqual(parentActivity.TraceStateString, item.Properties["tracestate"]);
                    }
                {
                    Assert.AreEqual(parentActivity.RootId, item.Context.Operation.Id);
                    Assert.AreEqual(parentActivity.Id, item.Context.Operation.ParentId);
                    Assert.IsTrue(item.Id.StartsWith('|' + item.Context.Operation.Id + '.'));
                }
            }
            else
            {
                Assert.IsNotNull(item.Context.Operation.Id);
                Assert.IsNull(item.Context.Operation.ParentId);
            }

            if (request != null)
            {
                if (expectW3CHeaders)
                {
                    var traceId = item.Context.Operation.Id;
                    var spanId = item.Id;
                    var expectedTraceParentHeader = $"00-{traceId}-{spanId}-00";
                    var expectedRequestId = $"|{traceId}.{spanId}.";
#endif
                }
                else
                {
                    Assert.IsFalse(request.Headers.Contains(W3C.W3CConstants.TraceParentHeader));
                    Assert.IsFalse(request.Headers.Contains(W3C.W3CConstants.TraceStateHeader));
                    Assert.AreEqual(item.Id, request.Headers.GetValues(RequestResponseHeaders.RequestIdHeader).Single());
                }

                if (expectLegacyHeaders)
                {
                    Assert.AreEqual(item.Id, request.Headers.GetValues(RequestResponseHeaders.StandardParentIdHeader).Single());
                    Assert.AreEqual(item.Context.Operation.Id, request.Headers.GetValues(RequestResponseHeaders.StandardRootIdHeader).Single());
                }
                else
                {
                    Assert.IsFalse(request.Headers.Contains(RequestResponseHeaders.StandardParentIdHeader));
                    Assert.IsFalse(request.Headers.Contains(RequestResponseHeaders.StandardRootIdHeader));
                }

                if (parentActivity != null && parentActivity.Baggage.Any())
                {
                    var correlationContextHeader = request.Headers.GetValues(RequestResponseHeaders.CorrelationContextHeader)
                        .Single()
                        .Split(',')
                        .ToArray();
                    var baggage = Activity.Current.Baggage.Select(kvp => $"{kvp.Key}={kvp.Value}").ToArray();
                    Assert.AreEqual(baggage.Length, correlationContextHeader.Length);

                    foreach (var baggageItem in baggage)
                public void Configure(IApplicationBuilder app)
                {
                    app.Run(async (context) =>
                    {
                        await context.Response.WriteAsync("Hello World!");
                    });
                }
            }
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
