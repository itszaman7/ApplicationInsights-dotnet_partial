#if NETCOREAPP
namespace Microsoft.ApplicationInsights.TestFramework.Channel
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Authentication;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [TestCategory("WindowsOnly")] // The LocalInProcHttpServer does not perform well on Linux.
    public class RedirectHttpHandlerTests
    {
        private const string helloString = "Hello World!";

        private const string LocalUrl1 = "http://localhost:1111";
        private const string LocalUrl2 = "http://localhost:2222";

        /// <summary>
        /// Verify behavior of HttpClient without <see cref="RedirectHttpHandler"/>.
        /// Setup two local servers, where server #1 will redirect requests to #2.
        /// </summary>
        [TestMethod]
        public async Task DefaultUseCase()
        {
            using var localServer1 = LocalInProcHttpServer.MakeRedirectServer(url: LocalUrl1, redirectUrl: LocalUrl2, cacheExpirationDuration: TimeSpan.FromDays(1));
            using var localServer2 = LocalInProcHttpServer.MakeTargetServer(url: LocalUrl2, response: helloString);

            var client = new MyCustomClient(url: LocalUrl1);


            // Default behavior. Nothing is cached, repeat previous workflow.
            var testStr2 = await client.GetAsync();
            Assert.AreEqual(helloString, testStr2);
            Assert.AreEqual(2, localServer1.RequestCounter);
            Assert.AreEqual(2, localServer2.RequestCounter);
        }

        /// <summary>
        /// Verify behavior of HttpClient and <see cref="RedirectHttpHandler"/>.
        /// Setup two local servers, where server #1 will redirect requests to #2.
        /// After the first request, it is expected that the client will cache the redirect.
        /// Additional requests should skip server #1 and go to server #2.
        /// </summary>
        [TestMethod]
        public async Task VerifyRedirect()
        {
            using var localServer1 = LocalInProcHttpServer.MakeRedirectServer(url: LocalUrl1, redirectUrl: LocalUrl2, cacheExpirationDuration: TimeSpan.FromDays(1));
            using var localServer2 = LocalInProcHttpServer.MakeTargetServer(url: LocalUrl2, response: helloString);

            // Default behavior. 1st server will redirect to 2nd.
            var testStr1 = await client.GetAsync();
            Assert.AreEqual(helloString, testStr1);
            Assert.AreEqual(1, localServer1.RequestCounter);
            Assert.AreEqual(1, localServer2.RequestCounter);

            // Redirect is cached. Request will go to 2nd server.
            var testStr2 = await client.GetAsync();
            Assert.AreEqual(helloString, testStr2);
            Assert.AreEqual(1, localServer1.RequestCounter, "redirect should be cached");
            Assert.AreEqual(2, localServer2.RequestCounter);
        }

        /// <summary>
        /// Verify behavior of HttpClient and <see cref="RedirectHttpHandler"/>.
        /// Setup two local servers, where server #1 will redirect requests to #2.
        /// Server #1 is missing the cache header.
        /// In this case, we will use a default cache and redirect will continue.
        /// </summary>
        [TestMethod]
        {
            using var localServer1 = new LocalInProcHttpServer(url: LocalUrl1)
            {
                ServerLogic = async (httpContext) =>
                {
                    // Returns status code and location, without cache header.
                    httpContext.Response.StatusCode = StatusCodes.Status308PermanentRedirect;
                    httpContext.Response.Headers.Add("Location", LocalUrl2);
                    await httpContext.Response.WriteAsync("redirect");
                },
            Assert.AreEqual(helloString, testStr1);
            Assert.AreEqual(1, localServer1.RequestCounter);
            Assert.AreEqual(1, localServer2.RequestCounter);
        }

        /// <summary>
        /// Verify behavior of HttpClient and <see cref="RedirectHttpHandler"/>.
        /// Setup two local servers, where server #1 will redirect requests to #2.
        /// Server #1 is missing the redirect uri header.
        /// In this case, redirect will fail.
            Assert.AreEqual("redirect", testStr1);
            Assert.AreEqual(1, localServer1.RequestCounter);
            Assert.AreEqual(0, localServer2.RequestCounter);
        }

        /// <summary>
        /// Verify behavior of HttpClient and <see cref="RedirectHttpHandler"/>.
        /// Create two local servers that redirect to each other.
        /// It is expected that every request will cause the cache to be updated.
        /// This test is attempting to cause deadlocks around that cache.
            for (int i = 0; i < numOfRequests; i++)
            {
                tasks.Add(client.GetAsync());
            }

            Task.WaitAll(tasks.ToArray());

            Assert.IsTrue(localServer1.RequestCounter > 0, $"{nameof(localServer1)} did not receive any requests");
            Assert.IsTrue(localServer2.RequestCounter > 0, $"{nameof(localServer2)} did not receive any requests");

        }

        /// <summary>
        /// Verify behavior of HttpClient and <see cref="RedirectHttpHandler"/>.
        /// Setup two local servers, where server #1 will redirect requests to #2.
        /// After the first request, it is expected that the client will cache the redirect.
        /// After this cache expries, requests will go to server #1.
        /// </summary>
        [TestMethod]
        public async Task VerifyRedirectCache()

            // Default behavior. 1st server will redirect to 2nd.
            var testStr1 = await client.GetAsync();
            Assert.AreEqual(helloString, testStr1);
            Assert.AreEqual(1, localServer1.RequestCounter);
            Assert.AreEqual(1, localServer2.RequestCounter);

        /// Verify that <see cref="RedirectHttpHandler.MaxRedirect"/> is enforced.
        /// </summary>
        [TestMethod]
        public async Task VerifyMaxRedirects()
        {
            using var localServer1 = LocalInProcHttpServer.MakeRedirectServer(url: LocalUrl1, redirectUrl: LocalUrl1, cacheExpirationDuration: TimeSpan.FromDays(1));

            var client = new MyCustomClient(url: LocalUrl1, new RedirectHttpHandler());

            var testStr1 = await client.GetAsync();
        /// Here, i'm testing that if an auth header is present, it MUST be preserved for every request.
        /// </summary>
        [TestMethod]
        public async Task VerifyAuthHeaderPreserved()
        {
            var testAuthToken = "ABCD1234";

            using var localServer1 = LocalInProcHttpServer.MakeRedirectServer(url: LocalUrl1, redirectUrl: LocalUrl1, cacheExpirationDuration: TimeSpan.FromDays(1));
            localServer1.ServerSideAsserts = (httpContext) =>
            {
                    : new HttpClient(httpMessageHandler);
            }

            public async Task<string> GetAsync()
            {
                var result = await this.httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, this.uri));
                return await result.Content.ReadAsStringAsync();
            }

            public async Task<string> GetAsync(string authToken)


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
