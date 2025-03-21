namespace Microsoft.ApplicationInsights.Tests
{
#if NET452
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// DependencyTrackingTelemetryModule .Net 4.6 specific tests. 
    /// </summary>
    [TestClass]
    public class DependencyTrackingTelemetryModuleHttpTest
    {
        private const string IKey = "F8474271-D231-45B6-8DD4-D344C309AE69";
        private const string FakeProfileApiEndpoint = "https://dc.services.visualstudio.com/v2/track";
        private const string LocalhostUrlDiagSource = "http://localhost:8088/";
        private const string LocalhostUrlEventSource = "http://localhost:8090/";
        private const string expectedAppId = "someAppId";

        private readonly OperationDetailsInitializer operationDetailsInitializer = new OperationDetailsInitializer();
        private readonly DictionaryApplicationIdProvider appIdProvider = new DictionaryApplicationIdProvider();
        private StubTelemetryChannel channel;
        private TelemetryConfiguration config;
        private List<ITelemetry> sentTelemetry;

        [TestInitialize]
        public void Initialize()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;

            ServicePointManager.DefaultConnectionLimit = 1000;
            this.sentTelemetry = new List<ITelemetry>();
            this.channel = new StubTelemetryChannel
            {
                OnSend = telemetry => this.sentTelemetry.Add(telemetry),
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

            DependencyTableStore.IsDesktopHttpDiagnosticSourceActivated = false;
        }

        [TestCleanup]
        public void Cleanup()
        {
            while (Activity.Current != null)
            {
                Activity.Current.Stop();
            }

            DependencyTableStore.IsDesktopHttpDiagnosticSourceActivated = false;
            GC.Collect();
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestBasicDependencyCollectionDiagnosticSource()
        {
            this.TestCollectionSuccessfulResponse(
                enableDiagnosticSource: true,
                url: LocalhostUrlDiagSource,
                statusCode: 200,
                enableW3C: true,
                enableRequestIdInW3CMode: true,
                injectLegacyHeaders: false);
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestDependencyCollectionDiagnosticSourceBacksOffWhenTraceParentPresent()
        {
            using (this.CreateDependencyTrackingModule(true, true, true, false))
            {
                HttpWebRequest request = WebRequest.CreateHttp(LocalhostUrlDiagSource);
                request.Headers.Add("traceparent", "00-0123456789abcdef0123456789abcdef-0123456789abcdef-00");

                using (new LocalServer(
                    new Uri(LocalhostUrlDiagSource).GetLeftPart(UriPartial.Authority) + "/",
                    ctx =>
                    {
                        ctx.Response.StatusCode = 200;
                    }))
                {
                    try
                    {
                        using (request.GetResponse())
                        {
                        }
                    }
                    catch (WebException)
                    {
                        // ignore and let ValidateTelemetry method check status code
                    }
                }

                Assert.IsFalse(this.sentTelemetry.Any());
            }
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestDependencyCollectionDiagnosticSourceTraceParentPresentW3COff()
        {
            using (this.CreateDependencyTrackingModule(true, false, true, false))
            {
                HttpWebRequest request = WebRequest.CreateHttp(LocalhostUrlDiagSource);
                request.Headers.Add("traceparent", "00-0123456789abcdef0123456789abcdef-0123456789abcdef-00");

                using (new LocalServer(
                    new Uri(LocalhostUrlDiagSource).GetLeftPart(UriPartial.Authority) + "/",
                    ctx =>
                    {
                        ctx.Response.StatusCode = 200;
                    }))
                {
                    try
                    {
                        using (request.GetResponse())
                        {
                        }
                    }
                    catch (WebException)
                    {
                        // ignore and let ValidateTelemetry method check status code
        [Timeout(5000)]
        public void TestBasicDependencyCollectionDiagnosticSourceLegacyHeaders()
        {
            this.TestCollectionSuccessfulResponse(
                enableDiagnosticSource: true,
                url: LocalhostUrlDiagSource,
                statusCode: 200,
                enableW3C: true,
                enableRequestIdInW3CMode: true,
                injectLegacyHeaders: true);
        }

        [TestMethod]
        [Ignore]
        // Active bug in .NET Fx diagnostics hook: https://github.com/dotnet/corefx/pull/40777
        // Application Insights has to inject Request-Id to work it around
        [Timeout(5000)]
        public void TestBasicDependencyCollectionW3COnRequestIdOffDiagnosticSource()
        {
            this.TestCollectionSuccessfulResponse(
                enableDiagnosticSource: true,
                url: LocalhostUrlDiagSource,
                statusCode: 200,
                enableW3C: true,
                enableRequestIdInW3CMode: false,
                injectLegacyHeaders: true);
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestBasicDependencyCollectionW3COffLegacyOnDiagnosticSource()
        {
            this.TestCollectionSuccessfulResponse(
                enableDiagnosticSource: true,
                url: LocalhostUrlDiagSource,
                statusCode: 200,
                enableW3C: false,
                enableRequestIdInW3CMode: true,
                injectLegacyHeaders: true);
        }
                contentLength: 0,
                expectResponse: false,
                expectHeadersDetail: true);
        }

        [TestMethod]
        [Timeout(5000)]
        public async Task TestZeroAndNonZeroContentResponseDiagnosticSource()
        {
            await this.TestZeroContentResponseAfterNonZeroResponse(LocalhostUrlDiagSource, 200);
            var parent = new Activity("parent").AddBaggage("k", "v").Start();
            parent.TraceStateString = "state=some";

            this.TestCollectionSuccessfulResponse(
                enableDiagnosticSource: true,
                url: LocalhostUrlDiagSource,
                statusCode: 200,
                enableW3C: true,
                enableRequestIdInW3CMode: true,
                injectLegacyHeaders: false);
        [Timeout(5000)]
        public void TestBasicDependencyCollectionEventSource()
        {
            this.TestCollectionSuccessfulResponse(false, LocalhostUrlEventSource, 200);
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestBasicDependencyCollectionW3COffEventSource()
        {
        public void TestBasicDependencyCollectionEventSourceWithParentActivityTracestateAndCc()
        {
            var parent = new Activity("parent").AddBaggage("k", "v").Start();
            parent.TraceStateString = "state=some";

            this.TestCollectionSuccessfulResponse(
                enableDiagnosticSource: true,
                url: LocalhostUrlDiagSource,
                statusCode: 200,
                enableW3C: true,
        {
            this.TestCollectionSuccessfulResponse(true, LocalhostUrlDiagSource, 404);
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestNoDependencyCollectionDiagnosticSourceNoResponseClose()
        {
            using (this.CreateDependencyTrackingModule(
                enableDiagnosticSource: true,
                enableW3C: true,
                enableRequestIdInW3CMode: true, 
                injectLegacyHeaders: false))
            {
                HttpWebRequest request = WebRequest.CreateHttp(LocalhostUrlDiagSource);

                using (new LocalServer(LocalhostUrlDiagSource))
                {
                    request.GetResponse();
                }
            HttpWebRequest request1 = WebRequest.CreateHttp(LocalhostUrlEventSource);
            using (new LocalServer(LocalhostUrlEventSource))
            {
                request1.GetResponse().Dispose();
            }

            // initialize dependency collector
            using (this.CreateDependencyTrackingModule(
                enableDiagnosticSource: true,
                enableW3C: true,
                enableRequestIdInW3CMode: true,
                injectLegacyHeaders: false))
            {
                HttpWebRequest request2 = WebRequest.CreateHttp(LocalhostUrlEventSource);

                using (new LocalServer(LocalhostUrlEventSource))
                {
                    request2.GetResponse().Dispose();
                }

        {
            await this.TestCollectionDnsIssue(true);
        }

        [TestMethod]
        [Timeout(10000)]
        public async Task TestDependencyCollectionDnsIssueRequestEventSource()
        {
            await this.TestCollectionDnsIssue(false);
        }
        public async Task TestDependencyCollectionCanceledRequestDiagnosticSource()
        {
            await this.TestCollectionCanceledRequest(true, LocalhostUrlDiagSource);
        }

        [TestMethod]
        [Timeout(5000)]
        public async Task TestDependencyCollectionCanceledRequestEventSource()
        {
            await this.TestCollectionCanceledRequest(false, LocalhostUrlEventSource);
        [Timeout(5000)]
        public void OnBeginOnEndAreNotCalledForAppInsightsUrl()
        {
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.config);

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(DependencyCollectorEventSource.Log, EventLevel.Verbose, DependencyCollectorEventSource.Keywords.RddEventKeywords);
                        Assert.IsFalse(message.Message.Contains("HttpDesktopDiagnosticSourceListener: End callback called for id"));
                    }
                }
            }
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestDependencyCollectorPostRequestsAreCollectedDiagnosticSource()
        {
        public void TestHttpRequestsWithQueryStringAreCollectedEventSource()
        {
            this.TestCollectionSuccessfulResponse(false, LocalhostUrlEventSource + "123?q=123", 200);
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestDependencyCollectionDiagnosticSourceRedirect()
        {
            this.TestCollectionResponseWithRedirects(true, LocalhostUrlDiagSource);
        }

        [TestMethod]
                    }

                    using (request.GetResponse())
                    {
                    }
                }

                this.ValidateTelemetry(
                    diagnosticSource: enableDiagnosticSource,
                    item: (DependencyTelemetry)this.sentTelemetry.Single(),
                    url: new Uri(url), 
                    requestMsg: request, 
                    success: true,
                    resultCode: "200",
                    w3CHeadersExpected: true,
                    responseExpected: true,
                    headersDetailExpected: false,
                    legacyHeadersExpected: false,
                    requestIdHeaderExpected: true);
            }
        }

        private void TestCollectionResponseWithRedirects(bool enableDiagnosticSource, string url)
        {
            using (this.CreateDependencyTrackingModule(
                enableDiagnosticSource: enableDiagnosticSource,
                enableW3C: true,
                enableRequestIdInW3CMode: true,
                injectLegacyHeaders: false))
            {

                int count = 0;
                Action<HttpListenerContext> onRequest = (context) =>
                {
                    if (count == 0)
                    {
                        context.Response.StatusCode = 302;
                        context.Response.RedirectLocation = url;
                    }
                    else
                    w3CHeadersExpected: true,
                    responseExpected: true,
                    headersDetailExpected: false,
                    legacyHeadersExpected: false,
                    requestIdHeaderExpected: true);
            }
        }

        private void TestCollectionSuccessfulResponse(
            bool enableDiagnosticSource, 
            string url, 
            int statusCode, 
            bool enableW3C = true,
            bool enableRequestIdInW3CMode = true,
            bool injectLegacyHeaders = false)
        {
            using (this.CreateDependencyTrackingModule(enableDiagnosticSource, enableW3C, enableRequestIdInW3CMode, injectLegacyHeaders))
            {
                HttpWebRequest request = WebRequest.CreateHttp(url);

                using (new LocalServer(
                    new Uri(url).GetLeftPart(UriPartial.Authority) + "/",
                    ctx =>
                    {
                        if (!enableDiagnosticSource && statusCode != 200)
                        {
                            // https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/548
                            // for quick unsuccessful response OnEnd is fired too fast after begin (before it's completed)
                            // first begin may take a while because of lazy initializations and jit compiling
                            // let's wait a bit here.
                        }
                    }
                    catch (WebException)
                    {
                        // ignore and let ValidateTelemetry method check status code
                    }
                }

                this.ValidateTelemetry(
                    diagnosticSource: true,
                    item: (DependencyTelemetry)this.sentTelemetry.Single(),
                    url: new Uri(url),
                    requestMsg: null,
                    success: statusCode >= 200 && statusCode < 300,
                    resultCode: statusCode.ToString(CultureInfo.InvariantCulture),
                    w3CHeadersExpected: true,
                    responseExpected: expectResponse,
                    headersDetailExpected: expectHeadersDetail,
                    legacyHeadersExpected: injectLegacyHeaders,
                    requestIdHeaderExpected: true);
        {
            using (this.CreateDependencyTrackingModule(
                enableDiagnosticSource: true,
                enableW3C: true,
                enableRequestIdInW3CMode: true,
                injectLegacyHeaders: false))
            {
                using (HttpClient client = new HttpClient())
                {
                    using (new LocalServer(
                        url,
                        context =>
                        {
                            context.Response.ContentLength64 = 0;
                            context.Response.StatusCode = statusCode;
                        }))
                    {
                        try
                        {
                            using (HttpResponseMessage response = await client.GetAsync(url))
                            {
                                Assert.AreEqual(0, response.Content.Headers.ContentLength);
                            }
                        }
                        catch (WebException)
                        {
                            // ignore and let ValidateTelemetry method check status code
                        }
                    }
                }
                    item: (DependencyTelemetry)this.sentTelemetry.Last(),
                    url: new Uri(url),
                    requestMsg: null,
                    success: statusCode >= 200 && statusCode < 300,
                    resultCode: statusCode.ToString(CultureInfo.InvariantCulture),
                    w3CHeadersExpected: true,
                    responseExpected: false,
                    headersDetailExpected: true,
                    legacyHeadersExpected: false,
                    requestIdHeaderExpected: true);
                using (new LocalServer(
                    url,
                    ctx =>
                    {
                        if (!enableDiagnosticSource)
                        {
                            // https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/548
                            // for quick unsuccesfull response OnEnd is fired too fast after begin (before it's completed)
                            // first begin may take a while because of lazy initializations and jit compiling
                            // let's wait a bit here.
                var url = new Uri($"http://{Guid.NewGuid()}/");
                HttpClient client = new HttpClient();
                await client.GetAsync(url).ContinueWith(t => { });

                if (enableDiagnosticSource)
                {
                    // here the start of dependency is tracked with HttpDesktopDiagnosticSourceListener, 
                    // so the expected SDK version should have DiagnosticSource 'rdddsd' prefix. 
                    // however the end is tracked by FrameworkHttpEventListener
                    this.ValidateTelemetry(
            bool legacyHeadersExpected = false,
            bool requestIdHeaderExpected = true)
        {
            Assert.AreEqual(url, item.Data);

            if (url.Port == 80 || url.Port == 443)
            {
                Assert.AreEqual($"{url.Host}", item.Target);
            }
            else
            {
                Assert.AreEqual($"{url.Host}:{url.Port}", item.Target);
            }

            Assert.IsTrue(item.Duration > TimeSpan.FromMilliseconds(0), "Duration has to be positive");
            Assert.AreEqual(RemoteDependencyConstants.HTTP, item.Type, "HttpAny has to be dependency kind as it includes http and azure calls");
            Assert.IsTrue(
                DateTime.UtcNow.Subtract(item.Timestamp.UtcDateTime).TotalMilliseconds <
                TimeSpan.FromMinutes(1).TotalMilliseconds,
                "timestamp < now");
                }
                else
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

            if (diagnosticSource)
            {
                this.operationDetailsInitializer.ValidateOperationDetailsDesktop(item, responseExpected, headersDetailExpected);
                this.ValidateTelemetryForDiagnosticSource(item, url, requestMsg, legacyHeadersExpected, w3CHeadersExpected, requestIdHeaderExpected);
            }
            else
                SdkVersionHelper.GetExpectedSdkVersion(typeof(DependencyTrackingTelemetryModule), "rdddsd:"),
                item.Context.GetInternalContext().SdkVersion);

            if (requestMsg != null)
            {
                var requestIdHeader = requestMsg.Headers[RequestResponseHeaders.RequestIdHeader];
                string expectedRequestId;

                if (expectW3CHeaders)
                {
                }
                else
                {
                    Assert.IsNull(requestMsg.Headers[RequestResponseHeaders.RequestIdHeader]);
                }

                if (expectLegacyHeaders)
                {
                    Assert.AreEqual(item.Id, requestMsg.Headers[RequestResponseHeaders.StandardParentIdHeader]);
                    Assert.AreEqual(item.Context.Operation.Id, requestMsg.Headers[RequestResponseHeaders.StandardRootIdHeader]);
                    {
                        Assert.IsTrue(correlationContextHeader.Contains(baggageItem));
                    }
                }
                else
                {
                    Assert.IsNull(requestMsg.Headers[RequestResponseHeaders.CorrelationContextHeader]);
                }
            }
        }
        private DependencyTrackingTelemetryModule CreateDependencyTrackingModule(
            bool enableDiagnosticSource,
            bool enableW3C,
            bool enableRequestIdInW3CMode,
            bool injectLegacyHeaders)
        {
            Activity.DefaultIdFormat = enableW3C ? ActivityIdFormat.W3C : ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;

            var module = new DependencyTrackingTelemetryModule();

            module.Initialize(this.config);
            Assert.AreEqual(enableDiagnosticSource, DependencyTableStore.IsDesktopHttpDiagnosticSourceActivated);

            return module;
        }

        private class LocalServer : IDisposable
        {
            private readonly HttpListener listener;
                                onRequest(context);
                            }
                            else
                            {
                                context.Response.StatusCode = 200;
                            }

                            context.Response.OutputStream.Close();
                            context.Response.Close();
                        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
