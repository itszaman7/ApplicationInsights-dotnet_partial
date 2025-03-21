namespace Microsoft.ApplicationInsights.DependencyCollector
{
    using System;
    using System.Collections.Concurrent;
    using System.Net;
    using System.Net.Http;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector;
        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry is DependencyTelemetry dependency)
            {
                dependency.TryGetOperationDetail(OperationDetailConstants.HttpRequestOperationDetailName, out var request);
                dependency.TryGetOperationDetail(OperationDetailConstants.HttpResponseOperationDetailName, out var response);
                dependency.TryGetOperationDetail(OperationDetailConstants.HttpResponseHeadersOperationDetailName, out var responseHeaders);

                this.operationDetails.AddOrUpdate(dependency, newDetails, (d, o) => newDetails);
            }
        }

        public void ValidateOperationDetailsCore(DependencyTelemetry telemetry, bool responseExpected = true)
        {
            Assert.IsTrue(this.TryGetDetails(telemetry, out var request, out var response, out var responseHeaders));

            {
                Assert.IsNull(response, "Response was present and not expected.");
            }
        }

        public void ValidateOperationDetailsDesktop(DependencyTelemetry telemetry, bool responseExpected = true, bool headersExpected = false)
        {
            Assert.IsTrue(this.TryGetDetails(telemetry, out var request, out var response, out var responseHeaders));
                Assert.IsNotNull(response, "Response was not present and expected.");
                Assert.IsNotNull(response as HttpWebResponse, "Response was not the expected type.");
            }
            else
            {
                Assert.IsNull(response, "Response was present and not expected.");
            }

        }
            out object responseHeaders)
        {
            request = response = responseHeaders = null;
            if (this.operationDetails.TryGetValue(depednency, out var tuple))
            {
                request = tuple.Item1;
                response = tuple.Item2;
                responseHeaders = tuple.Item3;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
