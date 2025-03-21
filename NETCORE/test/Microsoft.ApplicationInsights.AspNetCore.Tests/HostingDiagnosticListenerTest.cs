namespace UnitTests
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.AspNetCore.DiagnosticListeners;
    using Microsoft.ApplicationInsights.AspNetCore.Implementation;
    using Microsoft.ApplicationInsights.AspNetCore.Tests;
    using Microsoft.ApplicationInsights.AspNetCore.Tests.Helpers;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;

    using Xunit;
    using Xunit.Abstractions;

    using AspNetCoreMajorVersion = Microsoft.ApplicationInsights.AspNetCore.Tests.AspNetCoreMajorVersion;
    using RequestResponseHeaders = Microsoft.ApplicationInsights.AspNetCore.DiagnosticListeners.RequestResponseHeaders;


    public class HostingDiagnosticListenerTest : IDisposable
    {
        private const string HttpRequestScheme = "http";
        private const string ExpectedAppId = "cid-v1:some-app-id";
        private const string ActivityCreatedByHostingDiagnosticListener = "ActivityCreatedByHostingDiagnosticListener";

        private static readonly HostString HttpRequestHost = new HostString("testHost");
        private static readonly PathString HttpRequestPath = new PathString("/path/path");
        private static readonly QueryString HttpRequestQueryString = new QueryString("?query=1");

        private readonly ITestOutputHelper output;

        public HostingDiagnosticListenerTest(ITestOutputHelper output)
        {
            this.output = output;
        }


        private static Uri CreateUri(string scheme, HostString host, PathString? path = null, QueryString? query = null)
        {
            string uriString = string.Format(CultureInfo.InvariantCulture, "{0}://{1}", scheme, host);
            if (path != null)
            {
                uriString += path.Value;
            }
            if (query != null)
            {
                uriString += query.Value;
            }
            return new Uri(uriString);
        }

        private HttpContext CreateContext(string scheme, HostString host, PathString? path = null, QueryString? query = null, string method = null)
        {
            HttpContext context = new DefaultHttpContext();
            context.Request.Scheme = scheme;
            context.Request.Host = host;

            if (path.HasValue)
            {
                context.Request.Path = path.Value;
            }

            if (query.HasValue)
            {
                context.Request.QueryString = query.Value;
            }

            if (!string.IsNullOrEmpty(method))
            {
                context.Request.Method = method;
            }

            Assert.Null(context.Features.Get<RequestTelemetry>());

            return context;
        }

        private ConcurrentQueue<ITelemetry> sentTelemetry = new ConcurrentQueue<ITelemetry>();

        private HostingDiagnosticListener CreateHostingListener(AspNetCoreMajorVersion aspNetCoreMajorVersion, TelemetryConfiguration config = null, bool isW3C = true)
        {
            HostingDiagnosticListener hostingListener;
            if (config != null)
            {
                hostingListener = new HostingDiagnosticListener(
                    config,
                    CommonMocks.MockTelemetryClient(telemetry => this.sentTelemetry.Enqueue(telemetry), isW3C),
                    CommonMocks.GetMockApplicationIdProvider(),
                    injectResponseHeaders: true,
                    trackExceptions: true,
                    enableW3CHeaders: false,
                    aspNetCoreMajorVersion: GetAspNetCoreMajorVersion(aspNetCoreMajorVersion));
            }
            else
            {
                hostingListener = new HostingDiagnosticListener(
                    CommonMocks.MockTelemetryClient(telemetry => this.sentTelemetry.Enqueue(telemetry), isW3C),
                    CommonMocks.GetMockApplicationIdProvider(),
                    injectResponseHeaders: true,
                    trackExceptions: true,
                    enableW3CHeaders: false,
                    aspNetCoreMajorVersion: GetAspNetCoreMajorVersion(aspNetCoreMajorVersion));
            }

            hostingListener.OnSubscribe();
            return hostingListener;
        }

        [Theory]
        [InlineData(AspNetCoreMajorVersion.Two)]
        [InlineData(AspNetCoreMajorVersion.Three)]
        public void TestConditionalAppIdFlagIsRespected(AspNetCoreMajorVersion aspNetCoreMajorVersion)
        {
            HttpContext context = CreateContext(HttpRequestScheme, HttpRequestHost);
            TelemetryConfiguration config = TelemetryConfiguration.CreateDefault();
            // This flag tells sdk to not add app id in response header, unless its received in incoming headers.
            // For this test, no incoming headers is add, so the response should not have app id as well.
            config.ExperimentalFeatures.Add("conditionalAppId");

            using (var hostingListener = CreateHostingListener(aspNetCoreMajorVersion, config))
            {
                HandleRequestBegin(hostingListener, context, 0, aspNetCoreMajorVersion);

                Assert.NotNull(context.Features.Get<RequestTelemetry>());

                // VALIDATE
                Assert.Null(HttpHeadersUtilities.GetRequestContextKeyValue(context.Response.Headers,
                        RequestResponseHeaders.RequestContextTargetKey));

                HandleRequestEnd(hostingListener, context, 0, aspNetCoreMajorVersion);
            }

            Assert.Single(sentTelemetry);
            Assert.IsType<RequestTelemetry>(this.sentTelemetry.First());

            RequestTelemetry requestTelemetry = this.sentTelemetry.First() as RequestTelemetry;
            Assert.True(string.IsNullOrEmpty(requestTelemetry.Source));
            Assert.True(requestTelemetry.Duration.TotalMilliseconds >= 0);
            Assert.True(requestTelemetry.Success);
            Assert.Equal(CommonMocks.InstrumentationKey, requestTelemetry.Context.InstrumentationKey);            
            Assert.Equal(CreateUri(HttpRequestScheme, HttpRequestHost), requestTelemetry.Url);
            Assert.NotEmpty(requestTelemetry.Context.GetInternalContext().SdkVersion);
            Assert.Contains(SdkVersionTestUtils.VersionPrefix, requestTelemetry.Context.GetInternalContext().SdkVersion);
        }

        [Theory]
        [InlineData(AspNetCoreMajorVersion.Two)]
        [InlineData(AspNetCoreMajorVersion.Three)]
        public void TestSdkVersionIsPopulatedByMiddleware(AspNetCoreMajorVersion aspNetCoreMajorVersion)
                HandleRequestEnd(hostingListener, context, 0, aspNetCoreMajorVersion);
            }

            Assert.Single(sentTelemetry);
            Assert.IsType<RequestTelemetry>(this.sentTelemetry.First());

            RequestTelemetry requestTelemetry = this.sentTelemetry.First() as RequestTelemetry;
            Assert.True(requestTelemetry.Duration.TotalMilliseconds >= 0);
            Assert.True(requestTelemetry.Success);
            Assert.Equal(CommonMocks.InstrumentationKey, requestTelemetry.Context.InstrumentationKey);
            Assert.True(string.IsNullOrEmpty(requestTelemetry.Source));
            Assert.Equal(CreateUri(HttpRequestScheme, HttpRequestHost), requestTelemetry.Url);
            Assert.NotEmpty(requestTelemetry.Context.GetInternalContext().SdkVersion);
            Assert.Contains(SdkVersionTestUtils.VersionPrefix, requestTelemetry.Context.GetInternalContext().SdkVersion);
        }

        [Theory]
        [InlineData(AspNetCoreMajorVersion.Two)]
        [InlineData(AspNetCoreMajorVersion.Three)]
        public void TestRequestUriIsPopulatedByMiddleware(AspNetCoreMajorVersion aspNetCoreMajorVersion)
                HandleRequestEnd(hostingListener, context, 0, aspNetCoreMajorVersion);
            }

            Assert.Single(sentTelemetry);
            Assert.IsType<RequestTelemetry>(this.sentTelemetry.First());
            RequestTelemetry requestTelemetry = sentTelemetry.First() as RequestTelemetry;
            Assert.NotNull(requestTelemetry.Url);
            Assert.True(requestTelemetry.Duration.TotalMilliseconds >= 0);
            Assert.True(requestTelemetry.Success);
            Assert.Equal(CommonMocks.InstrumentationKey, requestTelemetry.Context.InstrumentationKey);

        [Theory]
        [InlineData(AspNetCoreMajorVersion.Two)]
        [InlineData(AspNetCoreMajorVersion.Three)]
        public void RequestWillBeMarkedAsFailedForRunawayException(AspNetCoreMajorVersion aspNetCoreMajorVersion)
        {
            HttpContext context = CreateContext(HttpRequestScheme, HttpRequestHost);

            using (var hostingListener = CreateHostingListener(aspNetCoreMajorVersion))
            {
                HandleRequestBegin(hostingListener, context, 0, aspNetCoreMajorVersion);

                Assert.NotNull(context.Features.Get<RequestTelemetry>());
                Assert.Equal(CommonMocks.TestApplicationId,
                    HttpHeadersUtilities.GetRequestContextKeyValue(context.Response.Headers, RequestResponseHeaders.RequestContextTargetKey));

                hostingListener.OnDiagnosticsUnhandledException(context, null);
                HandleRequestEnd(hostingListener, context, 0, aspNetCoreMajorVersion);
            }

            var telemetries = sentTelemetry.ToArray();
            Assert.Equal(2, sentTelemetry.Count);
            Assert.IsType<ExceptionTelemetry>(telemetries[0]);
            
            Assert.IsType<RequestTelemetry>(telemetries[1]);
            RequestTelemetry requestTelemetry = telemetries[1] as RequestTelemetry;
            Assert.True(requestTelemetry.Duration.TotalMilliseconds >= 0);
            Assert.False(requestTelemetry.Success);
            Assert.Equal(CommonMocks.InstrumentationKey, requestTelemetry.Context.InstrumentationKey);
            Assert.True(string.IsNullOrEmpty(requestTelemetry.Source));
        [InlineData(AspNetCoreMajorVersion.Three, false)]
        public void RequestWithNoHeadersCreateNewActivityAndPopulateRequestTelemetry(AspNetCoreMajorVersion aspNetCoreMajorVersion, bool IsW3C)
        {
            // Tests Request correlation when incoming request has no correlation headers.
            HttpContext context = CreateContext(HttpRequestScheme, HttpRequestHost, "/Test", method: "POST");

            using (var hostingListener = CreateHostingListener(aspNetCoreMajorVersion, isW3C: IsW3C))
            {
                HandleRequestBegin(hostingListener, context, 0, aspNetCoreMajorVersion);


                HandleRequestEnd(hostingListener, context, 0, aspNetCoreMajorVersion);

                Assert.Single(sentTelemetry);
                var requestTelemetry = (RequestTelemetry)this.sentTelemetry.Single();

                ValidateRequestTelemetry(requestTelemetry, activity, IsW3C, expectedParentId: null, expectedSource: null);
            }
        }

                Assert.Equal("40d1a5a08a68c0998e4a3b7c91915ca6", requestTelemetry.Context.Operation.Id);
                Assert.Equal("value1", requestTelemetry.Properties["prop1"]);
                Assert.Equal("value2", requestTelemetry.Properties["prop2"]);
            }
        }

        [Theory]
        [InlineData(AspNetCoreMajorVersion.Two, true)]
        [InlineData(AspNetCoreMajorVersion.Three, true)]
        [InlineData(AspNetCoreMajorVersion.Two, false)]
            // Tests Request correlation when incoming request has only Request-ID headers and is not compatible w3c trace id
            HttpContext context = CreateContext(HttpRequestScheme, HttpRequestHost, "/Test", method: "POST");
            // requestid with rootid part NOT compatible with W3C TraceID
            var requestId = "|noncompatible.b9e41c35_1.";
            context.Request.Headers[RequestResponseHeaders.RequestIdHeader] = requestId;
            context.Request.Headers[RequestResponseHeaders.CorrelationContextHeader] = "prop1=value1, prop2=value2";

            var tags = new Dictionary<string, string>
            {
                ["tag1"] = "v1",
                HandleRequestBegin(hostingListener, context, 0, aspNetCoreMajorVersion, tags);

                var activity = Activity.Current;
                Assert.NotNull(activity);

                // Hosting in 2,3 creates Activity ignoring the request-id. The request-id is not w3c compatible
                // hence SDK also ignores them, and just uses Activity from Hosting. 
                // Validate that activity is Not created by SDK Hosting
                Assert.NotEqual(ActivityCreatedByHostingDiagnosticListener, activity.OperationName);
                Assert.Equal(tags, activity.Tags.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));

                Assert.Equal("value1", requestTelemetry.Properties["prop1"]);
                Assert.Equal("value2", requestTelemetry.Properties["prop2"]);
            }
        }

        [Theory]
        [InlineData(AspNetCoreMajorVersion.Two, true)]
        [InlineData(AspNetCoreMajorVersion.Three, true)]
        [InlineData(AspNetCoreMajorVersion.Two, false)]
        public void RequestWithNonW3CCompatibleNonHierarchicalRequestIdCreateNewActivityAndPopulateRequestTelemetry(AspNetCoreMajorVersion aspNetCoreMajorVersion, bool IsW3C)
        {
            // Tests Request correlation when incoming request has only Request-ID headers and is not compatible w3c trace id and not a hierrachical id either.
            HttpContext context = CreateContext(HttpRequestScheme, HttpRequestHost, "/Test", method: "POST");
            // requestid with rootid part NOT compatible with W3C TraceID, and not a Hierrarchical id either.
            var requestId = "somerequestidsomeformat";
            context.Request.Headers[RequestResponseHeaders.RequestIdHeader] = requestId;
            context.Request.Headers[RequestResponseHeaders.CorrelationContextHeader] = "prop1=value1, prop2=value2";

            var tags = new Dictionary<string, string>
                ["tag1"] = "v1",
                ["tag2"] = "v2",
            };

            using (var hostingListener = CreateHostingListener(aspNetCoreMajorVersion, isW3C: IsW3C))
            {
                HandleRequestBegin(hostingListener, context, 0, aspNetCoreMajorVersion, tags);

                var activity = Activity.Current;
                Assert.NotNull(activity);
                {
                    Assert.Equal("somerequestidsomeformat", requestTelemetry.Context.Operation.Id);
                }

                Assert.Equal("value1", requestTelemetry.Properties["prop1"]);
                Assert.Equal("value2", requestTelemetry.Properties["prop2"]);
            }
        }

        [Theory]
            // Tests Request correlation when incoming request has only Request-ID headers.
            HttpContext context = CreateContext(HttpRequestScheme, HttpRequestHost, "/Test", method: "POST");
            // Trace Parent
            var traceParent = "00-4e3083444c10254ba40513c7316332eb-e2a5f830c0ee2c46-00";
            context.Request.Headers[Microsoft.ApplicationInsights.W3C.W3CConstants.TraceParentHeader] = traceParent;
            context.Request.Headers[Microsoft.ApplicationInsights.W3C.W3CConstants.TraceStateHeader] = "w3cprop1=value1, w3cprop2=value2";
            context.Request.Headers[RequestResponseHeaders.CorrelationContextHeader] = "prop1=value1, prop2=value2";

            var tags = new Dictionary<string, string>
            {
            using (var hostingListener = CreateHostingListener(aspNetCoreMajorVersion, isW3C: isW3C))
            {
                HandleRequestBegin(hostingListener, context, 0, aspNetCoreMajorVersion, tags);
                var activity = Activity.Current;
                Assert.NotNull(activity);

                if (aspNetCoreMajorVersion == AspNetCoreMajorVersion.Two && isW3C)
                {
                    Assert.Equal(ActivityCreatedByHostingDiagnosticListener, activity.OperationName);
                    Assert.Equal(tags, activity.Tags.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                }

                Assert.NotNull(context.Features.Get<RequestTelemetry>());

                HandleRequestEnd(hostingListener, context, 0, aspNetCoreMajorVersion);

                Assert.Single(sentTelemetry);
                var requestTelemetry = (RequestTelemetry)this.sentTelemetry.Single();

                if (isW3C)
                {
                    ValidateRequestTelemetry(requestTelemetry, activity, false, expectedParentId: null, expectedSource: null);
                }                
            }
        }

        [Theory]
        [InlineData(AspNetCoreMajorVersion.Two, true)]
        [InlineData(AspNetCoreMajorVersion.Three, true)]
        [InlineData(AspNetCoreMajorVersion.Two, false)]
            bool isW3C)
        {
            // Tests Request correlation when incoming request has only Request-ID headers.
            HttpContext context = CreateContext(HttpRequestScheme, HttpRequestHost, "/Test", method: "POST");
            // Trace Parent which does not follow w3c spec.
            var traceParent = "004e3083444c10254ba40513c7316332eb-e2a5f830c0ee2c4600";
            context.Request.Headers[Microsoft.ApplicationInsights.W3C.W3CConstants.TraceParentHeader] = traceParent;
            context.Request.Headers[Microsoft.ApplicationInsights.W3C.W3CConstants.TraceStateHeader] = "prop1=value1, prop2=value2";

            using (var hostingListener = CreateHostingListener(aspNetCoreMajorVersion, isW3C: isW3C))

                Assert.NotNull(context.Features.Get<RequestTelemetry>());

                HandleRequestEnd(hostingListener, context, 0, aspNetCoreMajorVersion);

                Assert.Single(sentTelemetry);
                var requestTelemetry = (RequestTelemetry)this.sentTelemetry.Single();

                if (isW3C)
                {
            context.Request.Headers[RequestResponseHeaders.CorrelationContextHeader] = "prop1=value1, prop2=value2";

            using (var hostingListener = CreateHostingListener(aspNetCoreMajorVersion, isW3C: IsW3C))
            {
                HandleRequestBegin(hostingListener, context, 0, aspNetCoreMajorVersion);
                var activity = Activity.Current;
                Assert.NotNull(activity);
                
                Assert.NotNull(context.Features.Get<RequestTelemetry>());

                }
                else
                {
                    Assert.Equal("40d1a5a08a68c0998e4a3b7c91915ca6", requestTelemetry.Context.Operation.Id);
                    ValidateRequestTelemetry(requestTelemetry, activity, IsW3C, expectedParentId: requestId, expectedSource: null);                    
                }

                Assert.Equal("value1", requestTelemetry.Properties["prop1"]);
                Assert.Equal("value2", requestTelemetry.Properties["prop2"]);
            }
            {
                HandleRequestBegin(hostingListener, context, 0, aspNetCoreMajorVersion);
                var activity = Activity.Current;
                Assert.NotNull(activity);

                // Activity baggage should be populated by SDK request collection module.
                Assert.Single(activity.Baggage.Where(b => b.Key == "prop1" && b.Value == "value1"));
                Assert.Single(activity.Baggage.Where(b => b.Key == "prop2" && b.Value == "value2"));
                Assert.NotNull(context.Features.Get<RequestTelemetry>());


                HandleRequestEnd(hostingListener, context, 0, aspNetCoreMajorVersion);

                Assert.Single(sentTelemetry);
                var requestTelemetry = (RequestTelemetry)this.sentTelemetry.Single();

                ValidateRequestTelemetry(requestTelemetry, activity, true, expectedParentId: null, expectedSource: null);
            }
        }

        public void RequestPopulateCorrelationHeaderVariousInputsOne(string correlationcontext, AspNetCoreMajorVersion aspNetCoreMajorVersion)
        {
            // Tests Correlation-Context is read and populated even when neither request-id nor traceparent is present.
            HttpContext context = CreateContext(HttpRequestScheme, HttpRequestHost, "/Test", method: "POST");
            context.Request.Headers[RequestResponseHeaders.CorrelationContextHeader] = correlationcontext;

            using (var hostingListener = CreateHostingListener(aspNetCoreMajorVersion))
            {
                HandleRequestBegin(hostingListener, context, 0, aspNetCoreMajorVersion);
                var activity = Activity.Current;
                HandleRequestEnd(hostingListener, context, 0, aspNetCoreMajorVersion);

                Assert.Single(sentTelemetry);
                var requestTelemetry = (RequestTelemetry)this.sentTelemetry.Single();

                ValidateRequestTelemetry(requestTelemetry, activity, true, expectedParentId: null, expectedSource: null);
            }
        }

        [Theory]
            context.Request.Headers[RequestResponseHeaders.CorrelationContextHeader] = correlationcontext;            

            using (var hostingListener = CreateHostingListener(aspNetCoreMajorVersion))
            {
                HandleRequestBegin(hostingListener, context, 0, aspNetCoreMajorVersion);
                var activity = Activity.Current;
                Assert.NotNull(activity);

                foreach(var bag in activity.Baggage)
                {
                Assert.Single(activity.Baggage.Where(b => b.Key == "prop2" && b.Value == "value2"));
                
                Assert.NotNull(context.Features.Get<RequestTelemetry>());

                HandleRequestEnd(hostingListener, context, 0, aspNetCoreMajorVersion);

                Assert.Single(sentTelemetry);
                var requestTelemetry = (RequestTelemetry)this.sentTelemetry.Single();

                ValidateRequestTelemetry(requestTelemetry, activity, true, expectedParentId: null, expectedSource: null);
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void OnHttpRequestInStartInitializeTelemetryIfActivityParentIdIsNotNull(bool isW3C)
        {
            var context = CreateContext(HttpRequestScheme, HttpRequestHost, "/Test", method: "POST");

            var parent = "|8ee8641cbdd8dd280d239fa2121c7e4e.df07da90a5b27d93.";
            context.Request.Headers[RequestResponseHeaders.RequestIdHeader] = parent;

            {
                activity = new Activity("operation");
                
                // pretend ASP.NET Core read it
                activity.SetParentId(parent);
                activity.AddBaggage("item1", "value1");
                activity.AddBaggage("item2", "value2");

                activity.Start();
                HandleRequestBegin(hostingListener, context, 0, AspNetCoreMajorVersion.Two);
            }

            Assert.Single(sentTelemetry);
            var requestTelemetry = this.sentTelemetry.First() as RequestTelemetry;

            ValidateRequestTelemetry(requestTelemetry, activityBySDK, isW3C, expectedParentId: "|8ee8641cbdd8dd280d239fa2121c7e4e.df07da90a5b27d93.");
            Assert.Equal("8ee8641cbdd8dd280d239fa2121c7e4e", requestTelemetry.Context.Operation.Id);
            Assert.Equal(requestTelemetry.Properties.Count, activity.Baggage.Count());

            foreach (var prop in activity.Baggage)
            Assert.Equal(CommonMocks.InstrumentationKey, requestTelemetry.Context.InstrumentationKey);
            Assert.True(string.IsNullOrEmpty(requestTelemetry.Source));            
            Assert.Equal(CreateUri(HttpRequestScheme, HttpRequestHost, "/Test"), requestTelemetry.Url);
            Assert.NotEmpty(requestTelemetry.Context.GetInternalContext().SdkVersion);
            Assert.Contains(SdkVersionTestUtils.VersionPrefix, requestTelemetry.Context.GetInternalContext().SdkVersion);
            Assert.Equal("GET /Test", requestTelemetry.Name);
        }

        [Theory]
        [InlineData(AspNetCoreMajorVersion.Two)]
            HttpHeadersUtilities.SetRequestContextKeyValue(context.Request.Headers, RequestResponseHeaders.RequestContextSourceKey, "DIFFERENT_APP_ID");

            using (var hostingListener = CreateHostingListener(aspNetCoreMajorVersion))
            {
                HandleRequestBegin(hostingListener, context, 0, aspNetCoreMajorVersion);

                Assert.NotNull(context.Features.Get<RequestTelemetry>());
                Assert.Equal(CommonMocks.TestApplicationId,
                    HttpHeadersUtilities.GetRequestContextKeyValue(context.Response.Headers,
                        RequestResponseHeaders.RequestContextTargetKey));

                HandleRequestEnd(hostingListener, context, 0, aspNetCoreMajorVersion);
            }

            Assert.Single(sentTelemetry);
            Assert.IsType<RequestTelemetry>(this.sentTelemetry.First());
            RequestTelemetry requestTelemetry = this.sentTelemetry.First() as RequestTelemetry;
            Assert.True(requestTelemetry.Duration.TotalMilliseconds >= 0);
            Assert.True(requestTelemetry.Success);
            Assert.Equal(CommonMocks.InstrumentationKey, requestTelemetry.Context.InstrumentationKey);
                var task1 = Task.Run(() =>
                {
                    var act = new Activity("operation1");
                    act.Start();
                    HandleRequestBegin(hostingListener, context1, 0, aspNetCoreMajorVersion);
                    HandleRequestEnd(hostingListener, context1, 0, aspNetCoreMajorVersion);
                });

                var task2 = Task.Run(() =>
                {
            HttpContext context = CreateContext(HttpRequestScheme, HttpRequestHost);
            HttpHeadersUtilities.SetRequestContextKeyValue(context.Request.Headers, RequestResponseHeaders.RequestContextTargetKey, "someAppId");

            using (var hostingListener = CreateHostingListener(aspNetCoreMajorVersion))
            {
                HandleRequestBegin(hostingListener, context, 0, aspNetCoreMajorVersion);
                HandleRequestEnd(hostingListener, context, 0, aspNetCoreMajorVersion);
            }

            Assert.Single(sentTelemetry);

        [Theory]
        [InlineData(AspNetCoreMajorVersion.Two)]
        [InlineData(AspNetCoreMajorVersion.Three)]
        public void ResponseHeadersAreNotInjectedWhenDisabled(AspNetCoreMajorVersion aspNetCoreMajorVersion)
        {
            HttpContext context = CreateContext(HttpRequestScheme, HttpRequestHost);

            using (var noHeadersMiddleware = new HostingDiagnosticListener(
                CommonMocks.MockTelemetryClient(telemetry => this.sentTelemetry.Enqueue(telemetry)),
        [InlineData(AspNetCoreMajorVersion.Three)]
        public void DoesntAddSourceIfRequestHeadersDontHaveSource(AspNetCoreMajorVersion aspNetCoreMajorVersion)
        {
            HttpContext context = CreateContext(HttpRequestScheme, HttpRequestHost);

            using (var hostingListener = CreateHostingListener(aspNetCoreMajorVersion))
            {
                HandleRequestBegin(hostingListener, context, 0, aspNetCoreMajorVersion);
                HandleRequestEnd(hostingListener, context, 0, aspNetCoreMajorVersion);
            }

            using (var hostingListener = CreateHostingListener(aspNetCoreMajorVersion, config))
            {
                HandleRequestBegin(hostingListener, context, 0, aspNetCoreMajorVersion);

                Assert.NotNull(Activity.Current);

                var requestTelemetry = context.Features.Get<RequestTelemetry>();
                Assert.NotNull(requestTelemetry);               
                Assert.Equal(SamplingDecision.SampledOut, requestTelemetry.ProactiveSamplingDecision);

        [Theory]
        [InlineData(AspNetCoreMajorVersion.Two)]
        [InlineData(AspNetCoreMajorVersion.Three)]
        public void RequestTelemetryIsProactivelySampledInIfFeatureFlagIsOnButActivityIsRecorded(AspNetCoreMajorVersion aspNetCoreMajorVersion)
        {
            TelemetryConfiguration config = TelemetryConfiguration.CreateDefault();
            config.ExperimentalFeatures.Add("proactiveSampling");
            config.SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Request, 0);

            HttpContext context = CreateContext(HttpRequestScheme, HttpRequestHost, "/Test", method: "POST");
            var traceParent = "00-4e3083444c10254ba40513c7316332eb-e2a5f830c0ee2c46-01";
            context.Request.Headers[Microsoft.ApplicationInsights.W3C.W3CConstants.TraceParentHeader] = traceParent;

                var requestTelemetry = context.Features.Get<RequestTelemetry>();
                Assert.NotNull(requestTelemetry);
                Assert.NotEqual(SamplingDecision.SampledOut, requestTelemetry.ProactiveSamplingDecision);
                ValidateRequestTelemetry(requestTelemetry, Activity.Current, true, "e2a5f830c0ee2c46");
            }
        }

        [Theory]
        [InlineData(AspNetCoreMajorVersion.Two)]
        [InlineData(AspNetCoreMajorVersion.Three)]

            HttpContext context = CreateContext(HttpRequestScheme, HttpRequestHost, "/Test", method: "POST");

            using (var hostingListener = CreateHostingListener(aspNetCoreMajorVersion, config))
            {
                HandleRequestBegin(hostingListener, context, 0, aspNetCoreMajorVersion);

                Assert.NotNull(Activity.Current);

                var requestTelemetry = context.Features.Get<RequestTelemetry>();
        [InlineData(AspNetCoreMajorVersion.Three)]
        public void VerifyRequestsUpdateRoleNameContainer(AspNetCoreMajorVersion aspNetCoreMajorVersion)
        {
            HttpContext context = CreateContext(HttpRequestScheme, HttpRequestHost, "/Test", method: "POST");

            RoleNameContainer.Instance = new RoleNameContainer(hostNameSuffix: ".azurewebsites.net");

            using (var hostingListener = CreateHostingListener(aspNetCoreMajorVersion))
            {
                context.Request.Headers["WAS-DEFAULT-HOSTNAME"] = "a.b.c.azurewebsites.net";
        }

        [Theory]
        [InlineData(AspNetCoreMajorVersion.Two)]
        [InlineData(AspNetCoreMajorVersion.Three)]
        public void VerifyLongRunningRequestDoesNotGenerateTelemetry(AspNetCoreMajorVersion aspNetCoreMajorVersion)
        {
            HttpContext context = CreateContext(HttpRequestScheme, HttpRequestHost);
            TelemetryConfiguration config = TelemetryConfiguration.CreateDefault();

            using (var hostingListener = CreateHostingListener(aspNetCoreMajorVersion, config))
            {
                HandleRequestBegin(hostingListener, context, 0, aspNetCoreMajorVersion);

                var activity = Activity.Current;
                activity.AddTag("http.long_running", "true");
                Assert.NotNull(activity);

                Assert.NotNull(context.Features.Get<RequestTelemetry>());

        [InlineData(AspNetCoreMajorVersion.Two)]
        [InlineData(AspNetCoreMajorVersion.Three)]
        public void NullActivityDoesGenerateTelemetry(AspNetCoreMajorVersion aspNetCoreMajorVersion)
        {
            HttpContext context = CreateContext(HttpRequestScheme, HttpRequestHost);
            TelemetryConfiguration config = TelemetryConfiguration.CreateDefault();

            using (var hostingListener = CreateHostingListener(aspNetCoreMajorVersion, config))
            {
                HandleRequestBegin(hostingListener, context, 0, aspNetCoreMajorVersion);
            HttpContext context,
            long timestamp,
            AspNetCoreMajorVersion aspNetCoreMajorVersion,
            IEnumerable<KeyValuePair<string, string>> tags = null)
        {
            if (aspNetCoreMajorVersion == AspNetCoreMajorVersion.Two)
            {
                if (Activity.Current == null)
                {
                    var activity = new Activity("operation");
                    {
                        foreach (var tag in tags)
                        {
                            activity.AddTag(tag.Key, tag.Value);
                        }
                    }

                    // Simulating the behaviour of Hosting layer in 2.xx, which parses Request-Id Header and 
                    // set Activity parent.
                    if (context.Request.Headers.TryGetValue("Request-Id", out var requestId))
                    {
                        activity.SetParentId(requestId);

                        string[] baggage = context.Request.Headers.GetCommaSeparatedValues(RequestResponseHeaders.CorrelationContextHeader);
                        if (baggage != StringValues.Empty && !activity.Baggage.Any())
                        {
                            foreach (var item in baggage)
                            {
                                var parts = item.Split('=');
                                if (parts.Length == 2)
                                {
                                    var itemName = StringUtilities.EnforceMaxLength(parts[0], InjectionGuardConstants.ContextHeaderKeyMaxLength);
                                    var itemValue = StringUtilities.EnforceMaxLength(parts[1], InjectionGuardConstants.ContextHeaderValueMaxLength);
                                    activity.AddBaggage(itemName, itemValue);
                                }
                            }
                        }
                    }                    
                    activity.Start();
                    this.output.WriteLine("Test code created and started Activity to simulate HostingLayer behaviour");

                }
                hostingListener.OnHttpRequestInStart(context);
            }
            else if (aspNetCoreMajorVersion == AspNetCoreMajorVersion.Three)
            {
                if (Activity.Current == null)
                {
                    var activity = new Activity("operation");
                    if (tags != null)
                            activity.TraceStateString = traceState;
                        }

                        string[] baggage = context.Request.Headers.GetCommaSeparatedValues(RequestResponseHeaders.CorrelationContextHeader);
                        if (baggage != StringValues.Empty && !activity.Baggage.Any())
                        {
                            foreach (var item in baggage)
                            {
                                var parts = item.Split('=');
                                if (parts.Length == 2)
                    }
                    activity.Start();
                    this.output.WriteLine("Test code created and started Activity to simulate HostingLayer behaviour");

                }
                hostingListener.OnHttpRequestInStart(context);
            }
        }

        private void HandleRequestEnd(HostingDiagnosticListener hostingListener, HttpContext context, long timestamp, AspNetCoreMajorVersion aspNetCoreMajorVersion)
        {
            if (aspNetCoreMajorVersion == AspNetCoreMajorVersion.Two || aspNetCoreMajorVersion == AspNetCoreMajorVersion.Three)
            {
                hostingListener.OnHttpRequestInStop(context);
            }
            else
            {
            }
        }

                Assert.Equal(requestTelemetry.Context.Operation.Id, activity.RootId);
            }
        }

        private Microsoft.ApplicationInsights.AspNetCore.Implementation.AspNetCoreMajorVersion GetAspNetCoreMajorVersion(Microsoft.ApplicationInsights.AspNetCore.Tests.AspNetCoreMajorVersion testVersion)
        {
            switch (testVersion)
            {
                case Microsoft.ApplicationInsights.AspNetCore.Tests.AspNetCoreMajorVersion.One:
                    return Microsoft.ApplicationInsights.AspNetCore.Implementation.AspNetCoreMajorVersion.One;
                case Microsoft.ApplicationInsights.AspNetCore.Tests.AspNetCoreMajorVersion.Two:
                    return Microsoft.ApplicationInsights.AspNetCore.Implementation.AspNetCoreMajorVersion.Two;
                case Microsoft.ApplicationInsights.AspNetCore.Tests.AspNetCoreMajorVersion.Three:
                    return Microsoft.ApplicationInsights.AspNetCore.Implementation.AspNetCoreMajorVersion.Three;
                default: throw new ArgumentOutOfRangeException(nameof(testVersion));
            }
        }

        public void Dispose()
        {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
