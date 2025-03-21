namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for DependencyCollectorDiagnosticListener.
    /// </summary>
    public partial class DependencyCollectorDiagnosticListenerTests
    {
        /// <summary>
        /// Tests that OnStartActivity injects headers.
        /// </summary>
        [TestMethod]
        public void OnActivityStartInjectsHeaders()
        {
            var activity = new Activity("System.Net.Http.HttpRequestOut");
            activity.AddBaggage("k", "v");
            activity.TraceStateString = "trace=state";
            activity.Start();

            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);

            using (var listener = this.CreateHttpListener(HttpInstrumentationVersion.V2))
            {
                listener.OnActivityStart(requestMsg);

                // Request-Id and Correlation-Context are injected by HttpClient when W3C is off,
                // check W3C and legacy headers here
                var requestIds = requestMsg.Headers.GetValues(RequestResponseHeaders.RequestIdHeader).ToArray();
                Assert.AreEqual(1, requestIds.Length);
                Assert.AreEqual($"|{activity.TraceId.ToHexString()}.{activity.SpanId.ToHexString()}.", requestIds[0]);

                var traceparents = requestMsg.Headers.GetValues(W3C.W3CConstants.TraceParentHeader).ToArray();
                Assert.AreEqual(1, traceparents.Length);
                Assert.AreEqual(activity.Id, traceparents[0]);

                var tracestates = requestMsg.Headers.GetValues(W3C.W3CConstants.TraceStateHeader).ToArray();
                Assert.AreEqual(1, tracestates.Length);
                Assert.AreEqual("trace=state", tracestates[0]);

                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.StandardRootIdHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.StandardParentIdHeader));
                Assert.AreEqual(this.testApplicationId1,
                    GetRequestContextKeyValue(requestMsg, RequestResponseHeaders.RequestContextCorrelationSourceKey));
            }
        }

        /// <summary>
        /// Tests that OnStartActivity injects headers.
        /// </summary>
        [TestMethod]
        public void OnActivityStartInjectsHeadersRequestIdOff()
        {
            using (var listenerWithoutRequestId = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new[] { "excluded.host.com" },
                injectLegacyHeaders: false,
                injectRequestIdInW3CMode: false,
                HttpInstrumentationVersion.V2))
            {
                var activity = new Activity("System.Net.Http.HttpRequestOut");
                activity.Start();

                HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);
                listenerWithoutRequestId.OnActivityStart(requestMsg);

                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.RequestIdHeader));

                var traceparents = requestMsg.Headers.GetValues(W3C.W3CConstants.TraceParentHeader).ToArray();
                Assert.AreEqual(1, traceparents.Length);
                Assert.AreEqual(activity.Id, traceparents[0]);
            }
        }

        /// <summary>
        /// Tests that OnStartActivity injects headers.
        /// </summary>
        [TestMethod]
        public void OnActivityStartInjectsLegacyHeaders()
        {
            var listenerWithLegacyHeaders = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new[] { "excluded.host.com" },
                injectLegacyHeaders: true,
                injectRequestIdInW3CMode: true,
                HttpInstrumentationVersion.V2);

            using (listenerWithLegacyHeaders)
            {
                var activity = new Activity("System.Net.Http.HttpRequestOut");
                activity.AddBaggage("k", "v");
                activity.Start();
                listenerWithLegacyHeaders.OnActivityStart(requestMsg);

                // Request-Id and Correlation-Context are injected by HttpClient
                // check only legacy headers here
                Assert.AreEqual(activity.RootId,
                    requestMsg.Headers.GetValues(RequestResponseHeaders.StandardRootIdHeader).Single());
                Assert.AreEqual(activity.SpanId.ToHexString(), requestMsg.Headers.GetValues(RequestResponseHeaders.StandardParentIdHeader).Single());
                Assert.AreEqual(this.testApplicationId1, GetRequestContextKeyValue(requestMsg, RequestResponseHeaders.RequestContextCorrelationSourceKey));
            }
        }
        /// </summary>
        [TestMethod]
        public void OnActivityStartInjectsW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;

            using (var listenerWithoutW3CHeaders = this.CreateHttpListener(HttpInstrumentationVersion.V2))
            {
                var activity = new Activity("System.Net.Http.HttpRequestOut").SetParentId("|guid.").Start();

                HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);
                listenerWithoutW3CHeaders.OnActivityStart(requestMsg);

                // Request-Id and Correlation-Context are injected by HttpClient
                // check only W3C headers here
                Assert.AreEqual(this.testApplicationId1, GetRequestContextKeyValue(requestMsg, RequestResponseHeaders.RequestContextCorrelationSourceKey));
                Assert.IsFalse(requestMsg.Headers.Contains(W3C.W3CConstants.TraceParentHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(W3C.W3CConstants.TraceStateHeader));
            }
        }

        /// <summary>
        /// Tests that OnStopActivity tracks telemetry.
        /// </summary>
        [TestMethod]
        public async Task OnActivityStopTracksTelemetry()
        {
            var activity = new Activity("System.Net.Http.HttpRequestOut")
                .AddBaggage("k", "v")
                var approxStartTime = DateTime.UtcNow;
                activity = Activity.Current;

                await Task.Delay(10);

                HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.OK);
                listener.OnActivityStop(responseMsg, requestMsg, TaskStatus.RanToCompletion);

                var telemetry = this.sentTelemetry.Single() as DependencyTelemetry;

            }
        }

        /// <summary>
        /// Tests that OnStopActivity tracks telemetry.
        /// </summary>
        [TestMethod]
        public async Task OnActivityStopTracksTelemetryW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;

            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);

            using (var listener = this.CreateHttpListener(HttpInstrumentationVersion.V2))
            {
                listener.OnActivityStart(requestMsg);

                var approxStartTime = DateTime.UtcNow;
                activity = Activity.Current;

                await Task.Delay(10);

                HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.OK);
                listener.OnActivityStop(responseMsg, requestMsg, TaskStatus.RanToCompletion);

                var telemetry = this.sentTelemetry.Single() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("POST /", telemetry.Name);
                Assert.AreEqual(RequestUrl, telemetry.Target);
                listener.OnActivityStart(requestMsg);

                HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.OK);
                listener.OnActivityStop(responseMsg, requestMsg, TaskStatus.RanToCompletion);

                var telemetry = this.sentTelemetry.Single() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual(parent.RootId, telemetry.Context.Operation.Id);
                Assert.AreEqual(parent.SpanId.ToHexString(), telemetry.Context.Operation.ParentId);

        /// <summary>
        /// Tests that OnStopActivity tracks telemetry.
        /// </summary>
        [TestMethod]
        public void OnActivityStopWithParentTracksTelemetry()
        {
            var parent = new Activity("parent")
                .AddBaggage("k", "v")
                .Start();

                Assert.IsNotNull(telemetry);
                Assert.AreEqual(parent.RootId, telemetry.Context.Operation.Id);
                Assert.AreEqual(activity.ParentSpanId.ToHexString(), telemetry.Context.Operation.ParentId);
                Assert.AreEqual(activity.SpanId.ToHexString(), telemetry.Id);
                Assert.AreEqual("v", telemetry.Properties["k"]);

                string expectedVersion =
                    SdkVersionHelper.GetExpectedSdkVersion(typeof(DependencyTrackingTelemetryModule),
                        prefix: "rdddsc:");
        public void OnActivityStopTracksTelemetryForCanceledRequest()
        {
            var activity = new Activity("System.Net.Http.HttpRequestOut");
            activity.Start();

            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);

            using (var listener = this.CreateHttpListener(HttpInstrumentationVersion.V2))
            {
                listener.OnActivityStart(requestMsg);

                listener.OnActivityStop(null, requestMsg, TaskStatus.Canceled);

                var telemetry = this.sentTelemetry.Single() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("Canceled", telemetry.ResultCode);
                Assert.AreEqual(false, telemetry.Success);

                // Check the operation details
                this.operationDetailsInitializer.ValidateOperationDetailsCore(telemetry, responseExpected: false);
            }
        }

        /// <summary>
        /// Tests that OnStopActivity tracks faulted request.
        /// </summary>
        [TestMethod]
        public void OnActivityStopTracksTelemetryForFaultedRequest()
        {
                    RequestResponseHeaders.RequestContextCorrelationSourceKey));
                Assert.IsNull(HttpHeadersUtilities.GetRequestContextKeyValue(requestMsg.Headers,
                    RequestResponseHeaders.RequestIdHeader));
                Assert.IsNull(HttpHeadersUtilities.GetRequestContextKeyValue(requestMsg.Headers,
                    RequestResponseHeaders.StandardParentIdHeader));
                Assert.IsNull(HttpHeadersUtilities.GetRequestContextKeyValue(requestMsg.Headers,
                    RequestResponseHeaders.StandardRootIdHeader));

                listener.OnActivityStop(null, requestMsg, TaskStatus.Faulted);

                Assert.IsFalse(this.sentTelemetry.Any());
            }
        }

        /// <summary>
        /// Call OnStartActivity() with uri that is in the excluded domain list.
        /// </summary>
        [TestMethod]
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;

            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, "http://excluded.host.com/path/to/file.html");
            using (var listener = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new string[] { "excluded.host.com" },
                injectLegacyHeaders: false,
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.StandardParentIdHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.StandardRootIdHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(W3C.W3CConstants.TraceParentHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(W3C.W3CConstants.TraceStateHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.CorrelationContextHeader));
            }
        }

        /// <summary>
        /// Tests that if OnStopActivity is called with null Activity, dependency is not tracked
                HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.OK);
                listener.OnActivityStop(responseMsg, requestMsg, TaskStatus.RanToCompletion);

                Assert.IsFalse(this.sentTelemetry.Any());
            }
        }

        [TestMethod]
        public void MultiHost_OnlyOneListenerTracksTelemetry()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            var startEvent =
                new KeyValuePair<string, object>("System.Net.Http.HttpRequestOut.Start",
                    new { Request = request });
            var stopEvent =
                new KeyValuePair<string, object>("System.Net.Http.HttpRequestOut.Stop",
                    new { Request = request, Response = response, RequestTaskStatus = TaskStatus.RanToCompletion });

            using (var firstListener = this.CreateHttpListener(HttpInstrumentationVersion.V2))
            using (var secondListener = this.CreateHttpListener(HttpInstrumentationVersion.V2))
            {
                var activity = new Activity("System.Net.Http.HttpRequestOut").Start();

                firstListener.OnNext(startEvent);
                secondListener.OnNext(startEvent);

                firstListener.OnNext(stopEvent);
                secondListener.OnNext(stopEvent);

                Assert.AreEqual(1, this.sentTelemetry.Count(t => t is DependencyTelemetry));
            }
        }

        [TestMethod]
        public void MultiHost_TwoActiveAndOneIsDisposedStillTracksTelemetry()
        {
            var request1 = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);
            var response1 = new HttpResponseMessage(HttpStatusCode.OK);

            var startEvent1 =
                new KeyValuePair<string, object>("System.Net.Http.HttpRequestOut.Start",
                    new { Request = request1 });
            var stopEvent1 =
                new KeyValuePair<string, object>("System.Net.Http.HttpRequestOut.Stop",
                    new { Request = request1, Response = response1, RequestTaskStatus = TaskStatus.RanToCompletion });

            var firstListener = this.CreateHttpListener(HttpInstrumentationVersion.V2);
            using (var secondListener = this.CreateHttpListener(HttpInstrumentationVersion.V2))

                activity = new Activity("System.Net.Http.HttpRequestOut").Start();
                secondListener.OnNext(startEvent2);
                secondListener.OnNext(stopEvent2);

                Assert.AreEqual(2, this.sentTelemetry.Count(t => t is DependencyTelemetry));
            }
        }

        [TestMethod]
        public void MultiHost_OneListenerThenAnotherTracksTelemetry()
        {
            var request1 = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);
            var response1 = new HttpResponseMessage(HttpStatusCode.OK);

            var startEvent1 =
                new KeyValuePair<string, object>("System.Net.Http.HttpRequestOut.Start",
                    new { Request = request1 });
            var stopEvent1 =
                new KeyValuePair<string, object>("System.Net.Http.HttpRequestOut.Stop",


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
