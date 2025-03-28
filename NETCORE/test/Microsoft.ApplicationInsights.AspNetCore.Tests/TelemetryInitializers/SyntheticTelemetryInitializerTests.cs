﻿namespace Microsoft.ApplicationInsights.AspNet.Tests.TelemetryInitializers
{
    using System;
    using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
    using Microsoft.ApplicationInsights.AspNetCore.Tests.Helpers;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.AspNetCore.Http;
    using Xunit;

    public class SyntheticTelemetryInitializerTests
    {
        [Fact]
        public void InitializeThrowIfHttpContextAccessorIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => { var initializer = new SyntheticTelemetryInitializer(null); });
        }

        [Fact]
        public void InitializeDoesNotThrowIfHttpContextIsUnavailable()

        [Fact]
        public void InitializeDoesNotThrowIfRequestTelemetryIsUnavailable()
        {
            var ac = new HttpContextAccessor() { HttpContext = new DefaultHttpContext() };

            var initializer = new SyntheticTelemetryInitializer(ac);

            initializer.Initialize(new RequestTelemetry());
            var initializer = new SyntheticTelemetryInitializer(contextAccessor);

            initializer.Initialize(requestTelemetry);

            Assert.Equal("some value", requestTelemetry.Context.Operation.SyntheticSource);
        }

        [Fact]
        public void InitializeRequestDoesNotHaveRunIdHeaderButHasLocationHeader()
        {
            var requestTelemetry = new RequestTelemetry();
            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessor(requestTelemetry);
            requestTelemetry.Context.Operation.SyntheticSource = null;
            contextAccessor.HttpContext.Request.Headers.Add("SyntheticTest-Location", new string[] { "location" });
            contextAccessor.HttpContext.Request.Headers.Remove("SyntheticTest-RunId");

            var initializer = new SyntheticTelemetryInitializer(contextAccessor);

            initializer.Initialize(requestTelemetry);
            contextAccessor.HttpContext.Request.Headers.Remove("SyntheticTest-Location");
            contextAccessor.HttpContext.Request.Headers.Add("SyntheticTest-RunId", new string[] { "runId" });

            var initializer = new SyntheticTelemetryInitializer(contextAccessor);

            initializer.Initialize(requestTelemetry);

            Assert.Null(requestTelemetry.Context.Operation.SyntheticSource);
        }

            requestTelemetry.Context.Operation.SyntheticSource = null;
            contextAccessor.HttpContext.Request.Headers.Add("SyntheticTest-Location", new string[] { "location" });
            contextAccessor.HttpContext.Request.Headers.Add("SyntheticTest-RunId", new string[] { "runId" });

            var initializer = new SyntheticTelemetryInitializer(contextAccessor);

            initializer.Initialize(requestTelemetry);



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
