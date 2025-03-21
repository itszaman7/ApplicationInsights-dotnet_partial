namespace Microsoft.ApplicationInsights.AspNetCore.Tests.Helpers
{
    using System;
    using Microsoft.ApplicationInsights.AspNetCore.DiagnosticListeners;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.Extensions.DependencyInjection;

    public static class HttpContextAccessorHelper
    {
            var services = new ServiceCollection();

            var request = new DefaultHttpContext().Request;
            request.Method = "GET";
            request.Path = new PathString("/Test");
            {
                HttpHeadersUtilities.SetRequestContextKeyValue(request.Headers, RequestResponseHeaders.RequestContextSourceKey, httpContextCorrelationId);
            }

            services.AddSingleton<IHttpContextAccessor>(contextAccessor);

            if (actionContext != null)
            {
                var si = new ActionContextAccessor();
            {
                request.HttpContext.Features.Set(requestTelemetry);
            }

            IServiceProvider serviceProvider = services.BuildServiceProvider();
        public static HttpContextAccessor CreateHttpContextAccessorWithoutRequest(HttpContext httpContext, RequestTelemetry requestTelemetry = null)
        {
            var services = new ServiceCollection();

            var contextAccessor = new HttpContextAccessor { HttpContext = httpContext };


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
