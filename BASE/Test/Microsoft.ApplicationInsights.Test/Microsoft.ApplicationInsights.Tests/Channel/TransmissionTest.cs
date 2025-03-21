namespace Microsoft.ApplicationInsights.Channel
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.CodeDom;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using System.Diagnostics.Tracing;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Runtime.Serialization.Json;

    public class TransmissionTest
    {
        [TestClass]
        public class Constructor
        {
            private readonly Uri testUri = new Uri("https://127.0.0.1/");

            [TestMethod]
            public void SetsEndpointAddressPropertyToSpecifiedValue()
            {
                var transmission = new Transmission(testUri, new byte[1], "content/type", "content/encoding");
                Assert.AreEqual(testUri, transmission.EndpointAddress);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsArgumentNullExceptionWhenEndpointAddressIsNull() => new Transmission(null, new byte[1], "content/type", "content/encoding");

            [TestMethod]
            public void SetsContentPropertyToSpecifiedValue()
            {
                var expectedContent = new byte[42];
                var transmission = new Transmission(testUri, expectedContent, "content/type", "content/encoding");
                Assert.AreSame(expectedContent, transmission.Content);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsArgumentNullExceptionWhenContentIsNull() => new Transmission(testUri, (byte[])null, "content/type", "content/encoding");

            [TestMethod]
            public void SetsContentTypePropertyToSpecifiedValue()
            {
                string expectedContentType = "TestContentType123";
                var transmission = new Transmission(testUri, new byte[1], expectedContentType, "content/encoding");
                Assert.AreSame(expectedContentType, transmission.ContentType);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsArgumentNullExceptionWhenContentTypeIsNull() => new Transmission(testUri, new byte[1], null, "content/encoding");

            [TestMethod]
            public void SetContentEncodingPropertyToSpecifiedValue()
            {
                string expectedContentEncoding = "gzip";
                var transmission = new Transmission(testUri, new byte[1], "any/content", expectedContentEncoding);
                Assert.AreSame(expectedContentEncoding, transmission.ContentEncoding);
            }

            [TestMethod]
            public void SetsTimeoutTo100SecondsByDefaultToMatchHttpWebRequest()
            {
                var transmission = new Transmission(testUri, new byte[1], "content/type", "content/encoding");
                Assert.AreEqual(TimeSpan.FromSeconds(100), transmission.Timeout);
            }

            [TestMethod]
            public void SetsTimeoutToSpecifiedValue()
            {

        [TestClass]
        [TestCategory("WindowsOnly")] // these tests are not reliable and block PRs
        public class SendAsync
        {
            private readonly Uri testUri = new Uri("https://127.0.0.1/");
            private const long AllKeywords = -1;

            [TestMethod]
            public async Task SendAsyncUsesPostMethodToSpecifiedHttpEndpoint()

                using (var fakeHttpClient = new HttpClient(handler))
                {
                    var items = new List<ITelemetry> { new EventTelemetry(), new EventTelemetry() };

                    // Instantiate Transmission with the mock HttpClient
                    Transmission transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, string.Empty, string.Empty);
                    // transmission.Timeout = TimeSpan.FromMilliseconds(1);

                    HttpWebResponseWrapper result = await transmission.SendAsync();
                    var transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, expectedContentType, expectedContentEncoding);

                    HttpWebResponseWrapper result = await transmission.SendAsync();
                }
            }

            [TestMethod]
            public async Task SendAsyncUsesEmptyContentTypeIfNoneSpecifiedInConstructor()
            {
                var handler = new HandlerForFakeHttpClient
                {
                    InnerHandler = new HttpClientHandler(),
                    OnSendAsync = (req, cancellationToken) =>
                    {
                        // VALIDATE
                        Assert.IsNull(req.Content.Headers.ContentType);

                        return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage());
                    }
                };

                using (var fakeHttpClient = new HttpClient(handler))
                {
                    var items = new List<ITelemetry> { new EventTelemetry(), new EventTelemetry() };

                    // Instantiate Transmission with the mock HttpClient
                    var transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, string.Empty, "ContentEncoding");

                    HttpWebResponseWrapper result = await transmission.SendAsync();
                }
            }

            [TestMethod]
            public async Task ThrowsInvalidOperationExceptionWhenTransmissionIsAlreadySending()
            {
                Transmission transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, new HttpClient(), string.Empty, string.Empty); FieldInfo isSendingField = typeof(Transmission).GetField("isSending", BindingFlags.NonPublic | BindingFlags.Instance);
                isSendingField.SetValue(transmission, 1);
                await AssertEx.ThrowsAsync<InvalidOperationException>(() => transmission.SendAsync());
            }

            [TestMethod]
            public async Task SendAsyncHandleResponseForPartialContentResponse()
            {
                var handler = new HandlerForFakeHttpClient
                {
                    InnerHandler = new HttpClientHandler(),
                    OnSendAsync = (req, cancellationToken) =>
                    {
                        HttpResponseMessage response = new HttpResponseMessage();
                        response.StatusCode = HttpStatusCode.PartialContent;
                    // Instantiate Transmission with the mock HttpClient
                    Transmission transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, string.Empty, string.Empty);

                    // ACT
                    HttpWebResponseWrapper result = await transmission.SendAsync();
                    // VALIDATE
                    Assert.AreEqual(206, result.StatusCode);
                    Assert.AreEqual("5", result.RetryAfterHeader);

#if NET6_0_OR_GREATER
                    Assert.IsTrue(result.Content == string.Empty);
#else
                    Assert.IsNull(result.Content);
#endif
                }
                        byte[] actualContent = await req.Content.ReadAsByteArrayAsync();
                        AssertEx.AreEqual(expectedContent, actualContent);
                        return await Task.FromResult<HttpResponseMessage>(response);
                    }
                };

                using (var fakeHttpClient = new HttpClient(handler))
                {
                    // Instantiate Transmission with the mock HttpClient
                    Transmission transmission = new Transmission(testUri, expectedContent, fakeHttpClient, string.Empty, string.Empty);
                    Transmission transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, string.Empty,
                        string.Empty, TimeSpan.FromMilliseconds(clientTimeoutInMillisecs));

                    // ACT
                    HttpWebResponseWrapper result = await transmission.SendAsync();

                    // VALIDATE
                    Assert.IsNotNull(result);
                    Assert.AreEqual((int) HttpStatusCode.RequestTimeout, result.StatusCode);
                    Assert.IsNull(result.Content, "Content is not to be read except in partial response (206) status.");
                };

                using (var fakeHttpClient = new HttpClient())
                {
                    // Instantiate Transmission with the mock HttpClient and Timeout to be just 1 msec to force Timeout.
                    Transmission transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, string.Empty,
                        string.Empty);

                    // ACT & VALIDATE
                    await AssertEx.ThrowsAsync<HttpRequestException>(() => transmission.SendAsync());
                // ARRANGE
                var handler = new HandlerForFakeHttpClient
                {
                    InnerHandler = new HttpClientHandler(),
                    OnSendAsync = (req, cancellationToken) =>
                    {
                        HttpResponseMessage response = new HttpResponseMessage();
                        response.StatusCode = HttpStatusCode.ServiceUnavailable;
                        return Task.FromResult<HttpResponseMessage>(response);
                    }
                };

                using (var fakeHttpClient = new HttpClient(handler))
                {
                    // Instantiate Transmission with the mock HttpClient
                    Transmission transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, string.Empty, string.Empty);

                    // ACT
                    HttpWebResponseWrapper result = await transmission.SendAsync();

                    Assert.AreEqual(503, result.StatusCode);
                    Assert.AreEqual("5", result.RetryAfterHeader);
                    Assert.IsNull(result.Content, "Content is not to be read except in partial response (206) status.");
                }

            }

#if NETCOREAPP
            [TestMethod]
            public async Task SendAsyncLogsIngestionReponseTimeEventCounter()
                    OnSendAsync = (req, cancellationToken) =>
                    {
                        Thread.Sleep(TimeSpan.FromMilliseconds(30));
                        return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage());
                    }
                };

                using (var fakeHttpClient = new HttpClient(handler))
                {
                    // Instantiate Transmission with the mock HttpClient
                    Transmission transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, string.Empty, string.Empty);

                    using (var listener = new EventCounterListener())
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            HttpWebResponseWrapper result = await transmission.SendAsync();
                        }
                        //Sleep for few seconds as the event counter is sampled on a second basis
                        Thread.Sleep(TimeSpan.FromSeconds(3));
#if NETCOREAPP
                        Assert.IsTrue((double)payload["Max"] >= 30);
#endif
                    }
                }
            }

            [TestMethod]
            public async Task SendAsyncLogsIngestionReponseTimeOnFailureEventCounter()
            {
                            HttpWebResponseWrapper result = await transmission.SendAsync();
                        }
                        //Sleep for few seconds as the event counter is sampled on a second basis
                        Thread.Sleep(TimeSpan.FromSeconds(3));

                        // VERIFY
                        // We validate by checking SDK traces.
                        var allTraces = listener.EventsReceived.ToList();
                        var traces = allTraces.Where(item => item.EventName == "EventCounters").ToList();
                        Assert.IsTrue(traces?.Count >= 1);
                        var payload = (IDictionary<string, object>)traces[0].Payload[0];
                        Assert.AreEqual("IngestionEndpoint-ResponseTimeMsec", payload["Name"].ToString());
                        Assert.IsTrue((int)payload["Count"] >= 5);
                        // Mean should be more than 30 ms, as we introduced a delay of 30ms in SendAsync.
#if NETCOREAPP
                        Assert.IsTrue((double)payload["Mean"] >= 30);
#endif
                    }
                }
            }
#endif
            [TestMethod]
            public async Task SendAsyncLogsIngestionReponseTimeAndStatusCode()
            {
                var handler = new HandlerForFakeHttpClient
                {
                    InnerHandler = new HttpClientHandler(),
                    OnSendAsync = (req, cancellationToken) =>
                    {
                        return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage());
                    {
                        var eventCounterArguments = new Dictionary<string, string>
                        {
                            {"EventCounterIntervalSec", "1"}
                        };

                        listener.EnableEvents(CoreEventSource.Log, EventLevel.LogAlways, (EventKeywords)AllKeywords, eventCounterArguments);

                        HttpWebResponseWrapper result = await transmission.SendAsync();

                        // We validate by checking SDK traces.
                        var allTraces = listener.Messages.ToList();
                        // Event 67 is logged after response from Ingestion Service.
                        var traces = allTraces.Where(item => item.EventId == 67).ToList();
                        Assert.AreEqual(1, traces.Count);
                    }
                }
            }

            [TestMethod]
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));
                    }
                };

                using (var fakeHttpClient = new HttpClient(handler))
                {
                    // Instantiate Transmission with the mock HttpClient                  
                    Transmission transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, string.Empty, string.Empty);
                    transmission.Timeout = TimeSpan.Zero;

                    // VALIDATE
                    transmission.TransmissionStatusEvent += delegate (object sender, TransmissionStatusEventArgs args)
                    {
                        Assert.AreEqual((int)HttpStatusCode.RequestTimeout, args.Response.StatusCode);
                        Assert.AreEqual(0, args.ResponseDurationInMs);
                    };

                    // ACT
                    }
                }
            }

            [TestMethod]
            public async Task TestTransmissionStatusEventHandlerFails()
            {
                // ARRANGE
                var handler = new HandlerForFakeHttpClient
                {
                    InnerHandler = new HttpClientHandler(),
                    OnSendAsync = (req, cancellationToken) =>
                    {
                        return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage());
                    }
                };

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(CoreEventSource.Log, EventLevel.LogAlways,
                        (EventKeywords)AllKeywords);

                    using (var fakeHttpClient = new HttpClient(handler))
                    {
                        // Instantiate Transmission with the mock HttpClient                  
                        Transmission transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, string.Empty, string.Empty);

                        // VALIDATE
                        transmission.TransmissionStatusEvent += delegate (object sender, TransmissionStatusEventArgs args)
                        {
                        };

                        // ACT
                        HttpWebResponseWrapper result = await transmission.SendAsync();
                    }

                    // Assert:
                    var allTraces = listener.Messages.ToList();
                    var traces = allTraces.Where(item => item.EventId == 71).ToList();
                    Assert.AreEqual(1, traces.Count);
                    "\"index\": 2,\r\n      \"statusCode\": 503,\r\n      \"message\": \"Error 2\"\r\n    },\r\n    {\r\n      " +
                    "\"index\": 3,\r\n      \"statusCode\": 500,\r\n      \"message\": \"Error 3\"\r\n    }\r\n  ]\r\n}";

                // Fake HttpClient will respond back with partial content
                var handler = new HandlerForFakeHttpClient
                {
                    InnerHandler = new HttpClientHandler(),
                    OnSendAsync = (req, cancellationToken) =>
                    {
                        return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage { StatusCode = HttpStatusCode.PartialContent, Content = new StringContent(ingestionResponse) });
                    eventTelemetry5.Context.InstrumentationKey = "IKEY_1";
                    telemetryItems.Add(eventTelemetry5);

                    // Serialize the telemetry items before passing to transmission
                    var serializedData = JsonSerializer.Serialize(telemetryItems, true);

                    // Instantiate Transmission with the mock HttpClient                  
                    Transmission transmission = new Transmission(testUri, serializedData, fakeHttpClient, string.Empty, string.Empty);

                    // VALIDATE
                    {
                        var sendertransmission = sender as Transmission;
                        // convert raw JSON response to Backendresponse object
                        BackendResponse backendResponse = GetBackendResponse(args.Response.Content);

                        // Deserialize telemetry items to identify which items has failed
                        string[] items = JsonSerializer
                                            .Deserialize(sendertransmission.Content)
                                            .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                        int totalItemsForIkey = items.Where(x => x.Contains("IKEY_1")).Count();
                        int failedItemsForIkey = failedItems.Where(x => x.Contains("IKEY_1")).Count();
                        Assert.AreEqual(2, totalItemsForIkey);
                        Assert.AreEqual(1, failedItemsForIkey);

                        //IKEY_2
                        totalItemsForIkey = items.Where(x => x.Contains("IKEY_2")).Count();
                        failedItemsForIkey = failedItems.Where(x => x.Contains("IKEY_2")).Count();
                        Assert.AreEqual(2, totalItemsForIkey);
                        Assert.AreEqual(1, failedItemsForIkey);

                        //IKEY_3
                        totalItemsForIkey = items.Where(x => x.Contains("IKEY_3")).Count();
                        failedItemsForIkey = failedItems.Where(x => x.Contains("IKEY_3")).Count();
                        Assert.AreEqual(1, totalItemsForIkey);
                        Assert.AreEqual(1, failedItemsForIkey);
                    };

                    // ACT
                    HttpWebResponseWrapper result = await transmission.SendAsync();
                }
            }

            /// <summary>
            /// Serializes response from ingestion service to BackendResponse object.
            /// </summary>
            /// <param name="response">Response from ingestion service.</param>
            /// <returns></returns>
            private BackendResponse GetBackendResponse(string responseContent)
            {
                        {
                            backendResponse = Serializer.ReadObject(ms) as BackendResponse;
                        }
                    }
                }
                catch
                {
                    backendResponse = null;
                }

            public int Index { get; set; }

            [DataMember(Name = "statusCode")]
            public int StatusCode { get; set; }

            [DataMember(Name = "message")]
            public string Message { get; set; }
        }
    }

    internal class HandlerForFakeHttpClient : DelegatingHandler
    {
        public Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> OnSendAsync;
        protected async override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return await OnSendAsync(request, cancellationToken);
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
