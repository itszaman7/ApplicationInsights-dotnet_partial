#if NET452
namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Common;
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
    public class DesktopDiagnosticSourceHttpProcessingTests
    {
        #region Fields
        private const int TimeAccuracyMilliseconds = 50;
        private const string TestInstrumentationKey = nameof(TestInstrumentationKey);
        private const string TestApplicationId = nameof(TestApplicationId);
        private readonly OperationDetailsInitializer operationDetailsInitializer = new OperationDetailsInitializer();
        private Uri testUrl = new Uri("http://www.microsoft.com/");
        private int sleepTimeMsecBetweenBeginAndEnd = 100;
        private TelemetryConfiguration configuration;
        private List<ITelemetry> sendItems;
        private DesktopDiagnosticSourceHttpProcessing httpDesktopProcessingFramework;
        #endregion //Fields

        #region TestInitialize

        [TestInitialize]
        public void TestInitialize()
        {
            this.sendItems = new List<ITelemetry>();

            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;

            this.configuration = new TelemetryConfiguration()
            {
                TelemetryChannel = new StubTelemetryChannel
                {
                    OnSend = telemetry => this.sendItems.Add(telemetry)
                },
                InstrumentationKey = TestInstrumentationKey,
                ApplicationIdProvider = new MockApplicationIdProvider(TestInstrumentationKey, TestApplicationId)
            };

            this.configuration.TelemetryInitializers.Add(this.operationDetailsInitializer);

            this.httpDesktopProcessingFramework = new DesktopDiagnosticSourceHttpProcessing(
                this.configuration, 
                new CacheBasedOperationHolder("testCache", 100 * 1000), 
                setCorrelationHeaders: true,
                correlationDomainExclusionList: new List<string>(),
                injectLegacyHeaders: false,
                injectRequestIdInW3cMode: true);
            DependencyTableStore.IsDesktopHttpDiagnosticSourceActivated = false;
        }

        [TestCleanup]
        public void Cleanup()
        {
            DependencyTableStore.IsDesktopHttpDiagnosticSourceActivated = false;
        }
        #endregion //TestInitiliaze

        /// <summary>
        /// Validates that OnRequestSend and OnResponseReceive sends valid telemetry.
        /// </summary>
        [TestMethod]
        public void RddTestHttpDesktopProcessingFrameworkUpdateTelemetryName()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.testUrl);

            var stopwatchMax = Stopwatch.StartNew();
            this.httpDesktopProcessingFramework.OnBegin(request);
            var stopwatchMin = Stopwatch.StartNew();
            Thread.Sleep(2 * this.sleepTimeMsecBetweenBeginAndEnd);
            Assert.AreEqual(0, this.sendItems.Count, "No telemetry item should be processed without calling End");
            var response = TestUtils.GenerateHttpWebResponse(HttpStatusCode.OK);
            stopwatchMin.Stop();
            this.httpDesktopProcessingFramework.OnEndResponse(request, response);
            stopwatchMax.Stop();

            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
            this.httpDesktopProcessingFramework.OnBegin(request);
            Stopwatch stopwatchMin = Stopwatch.StartNew();
            Thread.Sleep(2 * this.sleepTimeMsecBetweenBeginAndEnd);
            this.httpDesktopProcessingFramework.OnBegin(request);
            this.httpDesktopProcessingFramework.OnBegin(request);
            this.httpDesktopProcessingFramework.OnBegin(request);
            this.httpDesktopProcessingFramework.OnBegin(request);
            Thread.Sleep(this.sleepTimeMsecBetweenBeginAndEnd);
            Assert.AreEqual(0, this.sendItems.Count, "No telemetry item should be processed without calling End");
            stopwatchMin.Stop();

        /// <summary>
        /// Validates if DependencyTelemetry sent contains the cross component correlation ID.
        /// </summary>
        [TestMethod]
        [Description("Validates if DependencyTelemetry sent contains the cross component correlation ID.")]
        public void RddTestHttpDesktopProcessingFrameworkOnEndAddsAppIdToTargetField()
        {
            // Here is a sample App ID, since the test initialize method adds a random ikey and our mock getAppId method pretends that the appId for a given ikey is the same as the ikey.
            // This will not match the current component's App ID. Hence represents an external component.
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { RequestResponseHeaders.RequestContextHeader, this.GetCorrelationIdHeaderValue(appId) }
            };

            var response = TestUtils.GenerateHttpWebResponse(HttpStatusCode.OK, headers);

            this.httpDesktopProcessingFramework.OnBegin(request);
            this.httpDesktopProcessingFramework.OnEndResponse(request, response);
            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
        [TestMethod]
        [Description("Ensures that the source request header is added when request is sent.")]
        public void RddTestHttpDesktopProcessingFrameworkOnBeginAddsSourceHeader()
        {
            var request = WebRequest.Create(this.testUrl);

            Assert.IsNull(request.Headers[RequestResponseHeaders.RequestContextHeader]);

            this.httpDesktopProcessingFramework.OnBegin(request);
            Assert.IsNotNull(request.Headers.GetNameValueHeaderValue(

                var actualRootIdHeader = request.Headers[RequestResponseHeaders.StandardRootIdHeader];
                var actualParentIdHeader = request.Headers[RequestResponseHeaders.StandardParentIdHeader];
                var actualRequestIdHeader = request.Headers[RequestResponseHeaders.RequestIdHeader];
                Assert.IsNotNull(actualRootIdHeader);
                Assert.IsNotNull(actualParentIdHeader);
                Assert.IsNotNull(actualRequestIdHeader);

                Assert.AreNotEqual(actualParentIdHeader, op.Telemetry.Context.Operation.Id);


                var actualRequestIdHeader = request.Headers[RequestResponseHeaders.RequestIdHeader];
                var actualTraceparentHeader = request.Headers[W3C.W3CConstants.TraceParentHeader];

                Assert.IsNull(request.Headers[RequestResponseHeaders.StandardParentIdHeader]);
                Assert.IsNull(request.Headers[RequestResponseHeaders.StandardRootIdHeader]);
                Assert.IsNull(request.Headers[W3C.W3CConstants.TraceStateHeader]);

            var client = new TelemetryClient(this.configuration);
            using (var op = client.StartOperation<RequestTelemetry>("request"))
            {
                this.httpDesktopProcessingFramework.OnBegin(request);

                var actualRequestIdHeader = request.Headers[RequestResponseHeaders.RequestIdHeader];

                Assert.IsNull(request.Headers[RequestResponseHeaders.StandardParentIdHeader]);
                Assert.IsNull(request.Headers[RequestResponseHeaders.StandardRootIdHeader]);
                Assert.AreNotEqual(parentActivity.Id, actualRequestIdHeader);
            }
        }

        [TestMethod]
        public void RddTestHttpDesktopProcessingFrameworkOnBeginW3COnRequestIdOff()
        {
            var request = WebRequest.Create(this.testUrl);

            Assert.IsNull(request.Headers[RequestResponseHeaders.StandardParentIdHeader]);

            var client = new TelemetryClient(this.configuration);
            using (var op = client.StartOperation<RequestTelemetry>("request"))
            {
                var httpDesktopProcessingFrameworkRequestIdOff = new DesktopDiagnosticSourceHttpProcessing(
                    this.configuration,
                    new CacheBasedOperationHolder("testCache", 100 * 1000),
                    setCorrelationHeaders: true,
                    correlationDomainExclusionList: new List<string>(),
                    injectLegacyHeaders: false,
        }

        /// <summary>
        /// Ensures that the source request header is not added, as per the config, when request is sent.
        /// </summary>
        [TestMethod]
        [Description("Ensures that the source request header is not added when the config commands as such")]
        public void RddTestHttpDesktopProcessingFrameworkOnBeginSkipsAddingSourceHeaderPerConfig()
        {
            string hostnamepart = "partofhostname";
                setCorrelationHeaders: true,
                correlationDomainExclusionList: exclusionList,
                injectLegacyHeaders: false,
                injectRequestIdInW3cMode: true);

            localHttpProcessingFramework.OnBegin(request);
            Assert.IsNull(request.Headers[RequestResponseHeaders.RequestContextHeader]);
            Assert.AreEqual(0, request.Headers.Keys.Cast<string>().Count(x => x.StartsWith("x-ms-", StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// Ensures that the source request header is not overwritten if already provided by the user.
        /// </summary>
        [TestMethod]
        [Description("Ensures that the source request header is not overwritten if already provided by the user.")]
        public void RddTestHttpDesktopProcessingFrameworkOnBeginDoesNotOverwriteExistingSource()
        {
            string sampleHeaderValueWithAppId = RequestResponseHeaders.RequestContextCorrelationSourceKey + "=HelloWorld";
            var request = WebRequest.Create(this.testUrl);

            this.httpDesktopProcessingFramework.OnBegin(request);
            var actualHeaderValue = request.Headers[RequestResponseHeaders.RequestContextHeader];

            Assert.IsNotNull(actualHeaderValue);
            Assert.AreEqual(sampleHeaderValueWithAppId, actualHeaderValue);

            string sampleHeaderValueWithoutAppId = "helloWorld";
            request = WebRequest.Create(this.testUrl);

            request.Headers.Add(RequestResponseHeaders.RequestContextHeader, sampleHeaderValueWithoutAppId);

            this.httpDesktopProcessingFramework.OnBegin(request);
            actualHeaderValue = request.Headers[RequestResponseHeaders.RequestContextHeader];

            Assert.IsNotNull(actualHeaderValue);
            Assert.AreNotEqual(sampleHeaderValueWithAppId, actualHeaderValue);
        }

        private void ValidateTelemetryPacketForOnRequestSend(
            DependencyTelemetry remoteDependencyTelemetryActual,
        private void ValidateTelemetryPacket(
            DependencyTelemetry remoteDependencyTelemetryActual,
            Uri url,
            string kind,
            bool? success,
            double minDependencyDurationMs,
            double maxDependencyDurationMs,
            string statusCode, 
            string expectedVersion,
            bool responseExpected = true)
                remoteDependencyTelemetryActual.Duration.TotalMilliseconds <= maxDependencyDurationMs,
                $"Dependency duration {remoteDependencyTelemetryActual.Duration.TotalMilliseconds} must be smaller than time between before-start and after-end: '{maxDependencyDurationMs}'");

            Assert.IsTrue(
                remoteDependencyTelemetryActual.Duration.TotalMilliseconds >= minDependencyDurationMs,
                $"Dependency duration {remoteDependencyTelemetryActual.Duration.TotalMilliseconds} must be bigger than time between after-start and before-end '{minDependencyDurationMs}'");

            Assert.AreEqual(expectedVersion, remoteDependencyTelemetryActual.Context.GetInternalContext().SdkVersion);
        }

            return string.Format(CultureInfo.InvariantCulture, "cid-v1:{0}", appId);
        }

        private string GetCorrelationIdHeaderValue(string appId)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}=cid-v1:{1}", RequestResponseHeaders.RequestContextCorrelationTargetKey, appId);
        }
    }
}
#endif

# This file contains partial code from the original project
# Some functionality may be missing or incomplete
