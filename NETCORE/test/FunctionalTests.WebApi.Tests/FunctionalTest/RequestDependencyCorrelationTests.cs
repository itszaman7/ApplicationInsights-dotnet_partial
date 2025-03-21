namespace FunctionalTests.WebApi.Tests.FunctionalTest
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.ApplicationInsights.AspNetCore;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.Extensions.DependencyInjection;
    {
        private readonly string assemblyName;
        {
            const string RequestPath = "/api/values";

            using (var server = new InProcessServer(assemblyName, this.output))
            {
                DependencyTelemetry expected = new DependencyTelemetry();
                expected.ResultCode = "200";
                expected.Success = true;
                expected.Name = "GET " + RequestPath;
                expected.Data = server.BaseHost + RequestPath;

                this.ValidateBasicDependency(server, RequestPath, expected);
                    Data = server.BaseHost + RequestPath
                };

                var activity = new Activity("dummy")
                    .Start();

                var (request, dependency) = this.ValidateBasicDependency(server, RequestPath, expected);
                string expectedTraceId = activity.TraceId.ToHexString();
                string expectedParentSpanId = activity.SpanId.ToHexString();

                Assert.Equal(expectedTraceId, request.tags["ai.operation.id"]);
                Assert.Equal(expectedTraceId, dependency.tags["ai.operation.id"]);
                Assert.Equal(expectedParentSpanId, dependency.tags["ai.operation.parentId"]);
            }
        }

        public void Dispose()
        {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
