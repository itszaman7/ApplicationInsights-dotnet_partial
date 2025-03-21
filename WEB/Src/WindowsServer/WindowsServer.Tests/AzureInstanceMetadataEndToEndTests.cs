namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.WindowsServer.Implementation;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation.DataContracts;
    using Microsoft.ApplicationInsights.WindowsServer.Mock;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;
    using Moq.Protected;

    using Assert = Xunit.Assert;

    /// <summary>
    /// Tests the heartbeat functionality through actual local-only Http calls to mimic
    /// end to end functionality as closely as possible.
    /// </summary>
    [TestClass]
    public class AzureInstanceMetadataEndToEndTests
    {
        [TestMethod]
        public async Task SpoofedResponseFromAzureIMSDoesntCrash()
        {
            var azureImsProps = new AzureComputeMetadataHeartbeatPropertyProvider();
            var azureIMSData = await azureIms.GetAzureComputeMetadataAsync();

            // VERIFY
            foreach (string fieldName in azureImsProps.ExpectedAzureImsFields)
            {
                string fieldValue = azureIMSData.GetValueForField(fieldName);
                Assert.NotNull(fieldValue);
                Assert.Equal(fieldValue, testMetadata.GetValueForField(fieldName));
            }
        public async Task AzureImsResponseExcludesMalformedValues()
        {
            // SETUP
            var testMetadata = GetTestMetadata();
            // make it a malicious-ish response...
            testMetadata.Name = "Not allowed for VM names";
            testMetadata.ResourceGroupName = "Not allowed for resource group name";
            testMetadata.SubscriptionId = "Definitely-not-a GUID up here";

            Mock<HttpMessageHandler> mockHttpMessageHandler = GetMockHttpMessageHandler(testMetadata);
            //var azureIms = new AzureMetadataRequestor(new HttpClient(mockHttpMessageHandler.Object));
            var azureIms = GetTestableAzureMetadataRequestor(mockHttpMessageHandler.Object);

            // ACT
            var azureImsProps = new AzureComputeMetadataHeartbeatPropertyProvider(azureIms);
            Assert.True(result);
            Assert.Empty(hbeatProvider.HbeatProps["azInst_name"]);
            Assert.Empty(hbeatProvider.HbeatProps["azInst_resourceGroupName"]);
            Assert.Empty(hbeatProvider.HbeatProps["azInst_subscriptionId"]);
        }

        [TestMethod]
        public async Task AzureImsResponseHandlesException()
        {
            // SETUP

            // ACT
            var result = await azureIms.GetAzureComputeMetadataAsync();

            // VERIFY
            Assert.Null(result);
        }

        [TestMethod]
        public async Task AzureImsResponseUnsuccessful()
        {
            // SETUP
            var testMetadata = GetTestMetadata();
            var mockHttpMessageHandler = GetMockHttpMessageHandler(testMetadata, HttpStatusCode.Forbidden);
            //var azureIms = new AzureMetadataRequestor(new HttpClient(mockHttpMessageHandler.Object));
            var azureIms = GetTestableAzureMetadataRequestor(mockHttpMessageHandler.Object);

            // ACT
            var azureIMSData = await azureIms.GetAzureComputeMetadataAsync();

        {
            var json = SerializeAsJsonString(metadata);

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = httpStatusCode,
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
            };

            else
            {
                mockHttpMessageHandler
                   .Protected()
                   .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                   .ReturnsAsync(value: response);
            }

            return mockHttpMessageHandler;
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
