#if !NET452 && !NET46 && !REDFIELD
namespace Microsoft.ApplicationInsights.TestFramework.Extensibility.Implementation.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Authentication;
    /// This ensures that the end user is consuming the AI SDK in one of the newer frameworks.
    /// </remarks>
    [TestClass]
    [TestCategory("AAD")]
    public class TransmissionCredentialEnvelopeTests
    {
        private readonly Uri testUri = new Uri("https://127.0.0.1/");

        [TestMethod]
        public async Task VerifyTransmissionSendAsync_Default()
        {
            var handler = new HandlerForFakeHttpClient
            {
                InnerHandler = new HttpClientHandler(),

                    return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage());
                }
            {
                var expectedContentType = "content/type";
                var expectedContentEncoding = "contentEncoding";
                var items = new List<ITelemetry> { new EventTelemetry() };

                // Instantiate Transmission with the mock HttpClient
                var transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, expectedContentType, expectedContentEncoding);

            var handler = new HandlerForFakeHttpClient
            {
                InnerHandler = new HttpClientHandler(),
                OnSendAsync = (req, cancellationToken) =>
                {
                    // VALIDATE
                var transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, expectedContentType, expectedContentEncoding);
                transmission.CredentialEnvelope = credentialEnvelope;

                var result = await transmission.SendAsync();
            }
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
