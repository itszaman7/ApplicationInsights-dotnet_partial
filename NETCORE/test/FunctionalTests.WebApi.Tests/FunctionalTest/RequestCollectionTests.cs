namespace FunctionalTests.WebApi.Tests.FunctionalTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.ApplicationInsights.DataContracts;
    using Xunit;
    using Xunit.Abstractions;
    using System.Reflection;
    using global::FunctionalTests.Utils;

    public class RequestCollectionTests : TelemetryTestsBase
    {
        private readonly string assemblyName;

        public RequestCollectionTests(ITestOutputHelper output) : base (output)
        {
            this.assemblyName = this.GetType().GetTypeInfo().Assembly.GetName().Name;
        }

        [Fact]
        public void TestIfPerformanceCountersAreCollected()
        {
            this.output.WriteLine("Validating perfcounters");
            ValidatePerformanceCountersAreCollected(assemblyName);
        }

        [Fact]
        public void TestBasicRequestPropertiesAfterRequestingControllerThatThrows()
        {
            using (var server = new InProcessServer(assemblyName, this.output))
            }
        }

        [Fact]
        public void TestBasicExceptionPropertiesAfterRequestingControllerThatThrows()
        {
            using (var server = new InProcessServer(assemblyName, this.output))
            {
                var expectedExceptionTelemetry = new ExceptionTelemetry();
                expectedExceptionTelemetry.Exception = new InvalidOperationException();
            {
                const string RequestPath = "/api/values";

                var expectedRequestTelemetry = new RequestTelemetry();
                expectedRequestTelemetry.Name = "GET Values/Get";
                expectedRequestTelemetry.ResponseCode = "200";
                expectedRequestTelemetry.Success = true;
                expectedRequestTelemetry.Url = new System.Uri(server.BaseHost + RequestPath);
                {
                    { "Request-Context", "appId=value"},
                };

                this.ValidateRequestWithHeaders(server, RequestPath, requestHeaders, expectedRequestTelemetry, expectRequestContextInResponse: true);
            }
        }

        [Fact]
        public void TestBasicRequestPropertiesAfterRequestingNotExistingController()
                Dictionary<string, string> requestHeaders = new Dictionary<string, string>()
                {
                    { "Request-Context", "appId=value"},
                };

                this.ValidateRequestWithHeaders(server, RequestPath, requestHeaders, expectedRequestTelemetry, expectRequestContextInResponse: true);
            }
        }

        [Fact]
                const string RequestPath = "/api/values/1";

                var expectedRequestTelemetry = new RequestTelemetry();
                expectedRequestTelemetry.Name = "GET Values/Get [id]";
                expectedRequestTelemetry.ResponseCode = "200";
                expectedRequestTelemetry.Success = true;
                expectedRequestTelemetry.Url = new System.Uri(server.BaseHost + RequestPath);

                Dictionary<string, string> requestHeaders = new Dictionary<string, string>()
                {
        }

        [Fact]
        public void TestNoHeadersInjectedInResponseWhenConfiguredAndWithIncomingRequestContext()
        {
            using (var server = new InProcessServer(assemblyName, this.output, (aiOptions) => aiOptions.RequestCollectionOptions.InjectResponseHeaders = false))
            {
                const string RequestPath = "/api/values/1";

                var expectedRequestTelemetry = new RequestTelemetry();


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
