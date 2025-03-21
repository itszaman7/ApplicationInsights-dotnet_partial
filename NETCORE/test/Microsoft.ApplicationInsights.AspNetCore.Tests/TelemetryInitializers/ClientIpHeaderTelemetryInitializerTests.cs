namespace Microsoft.ApplicationInsights.AspNetCore.Tests.TelemetryInitializers
{
    using System;
    using System.Net;
    using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
    using Microsoft.ApplicationInsights.AspNetCore.Tests.Helpers;
    using Microsoft.ApplicationInsights.DataContracts;
    using Xunit;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;

    public class ClientIpHeaderTelemetryInitializerTests
    {
        [Fact]
        public void InitializeThrowIfHttpContextAccessorIsNull()
        {
            Assert.ThrowsAny<ArgumentNullException>(() => { var initializer = new ClientIpHeaderTelemetryInitializer(null); });
        }

        [Fact]
        public void InitializeDoesNotThrowIfHttpContextIsUnavailable()
        {
            var ac = new HttpContextAccessor { HttpContext = null };
            initializer.Initialize(new RequestTelemetry());
        }

        [Fact]
        public void InitializeDoesNotThrowIfRequestServicesAreUnavailable()
        {
            var ac = new HttpContextAccessor { HttpContext = new DefaultHttpContext() };

            var initializer = new ClientIpHeaderTelemetryInitializer(ac);


        [Fact]
        public void InitializeDoesNotThrowIfHeaderCollectionIsUnavailable()
        {
            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessorWithoutRequest(new DefaultHttpContext(), new RequestTelemetry());

            var initializer = new ClientIpHeaderTelemetryInitializer(contextAccessor);

            initializer.Initialize(new EventTelemetry());
        }
            var requestTelemetry = new RequestTelemetry();
            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessor(requestTelemetry);

            var httpConnectionFeature = new HttpConnectionFeature
            {
                RemoteIpAddress = new IPAddress(new byte[] { 1, 2, 3, 4 })
            };
            contextAccessor.HttpContext.Features.Set<IHttpConnectionFeature>(httpConnectionFeature);

            var initializer = new ClientIpHeaderTelemetryInitializer(contextAccessor);
        {
            var requestTelemetry = new RequestTelemetry();
            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessor(requestTelemetry);

            contextAccessor.HttpContext.Request.Headers.Add(headerName, new string[] { headerValue });

            var initializer = new ClientIpHeaderTelemetryInitializer(contextAccessor);
            initializer.HeaderNames.Add(headerName);
            if (separators != null)
            {

            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessor(requestTelemetry);
            contextAccessor.HttpContext.Request.Headers.Add("X-Forwarded-For", new string[] { "127.0.0.3" });

            var initializer = new ClientIpHeaderTelemetryInitializer(contextAccessor);

            initializer.Initialize(requestTelemetry);

            Assert.Equal("127.0.0.4", requestTelemetry.Context.Location.Ip);
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
