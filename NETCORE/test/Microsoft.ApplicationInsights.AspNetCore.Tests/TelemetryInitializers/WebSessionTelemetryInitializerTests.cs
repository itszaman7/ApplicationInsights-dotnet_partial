namespace Microsoft.ApplicationInsights.AspNetCore.Tests.TelemetryInitializers
{
    using System;

    using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
    using Microsoft.ApplicationInsights.AspNetCore.Tests.Helpers;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.AspNetCore.Http;

    using Xunit;
        {

        [Fact]
        public void InitializeDoesNotThrowIfRequestTelemetryIsUnavailable()
        {
            var ac = new HttpContextAccessor() { HttpContext = new DefaultHttpContext() };

        [Fact]
        public void InitializeSetsSessionFromCookie()
        {
            var requestTelemetry = new RequestTelemetry();
            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessor(requestTelemetry);
            contextAccessor.HttpContext.Request.Headers["Cookie"] = "ai_session=test|2015-04-10T17:11:38.378Z|2015-04-10T17:11:39.180Z";
            var initializer = new WebSessionTelemetryInitializer(contextAccessor);

            initializer.Initialize(requestTelemetry);

            Assert.Equal("test", requestTelemetry.Context.Session.Id);
        }
       
        [Fact]
            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessor(requestTelemetry);
            contextAccessor.HttpContext.Request.Headers["Cookie"] = "ai_session=test|2015-04-10T17:11:38.378Z|2015-04-10T17:11:39.180Z";
            var initializer = new WebSessionTelemetryInitializer(contextAccessor);

            initializer.Initialize(requestTelemetry);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
