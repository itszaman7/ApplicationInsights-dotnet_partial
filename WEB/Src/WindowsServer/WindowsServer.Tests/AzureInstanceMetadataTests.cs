namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
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

    [TestClass]
    public class AzureInstanceMetadataTests
    {
        [TestMethod]
        public void GetAzureInstanceMetadataFieldsAsExpected()
        {
            HeartbeatProviderMock hbeatMock = new HeartbeatProviderMock();
            AzureInstanceMetadataRequestMock azureInstanceRequestorMock = new AzureInstanceMetadataRequestMock();
            AzureComputeMetadataHeartbeatPropertyProvider azureIMSFields = new AzureComputeMetadataHeartbeatPropertyProvider(azureInstanceRequestorMock);

            var taskWaiter = azureIMSFields.SetDefaultPayloadAsync(hbeatMock).ConfigureAwait(false);
            Assert.True(taskWaiter.GetAwaiter().GetResult()); // no await for tests

            foreach (string fieldName in azureIMSFields.ExpectedAzureImsFields)
            {
                string expectedFieldName = string.Concat(AzureComputeMetadataHeartbeatPropertyProvider.HeartbeatPropertyPrefix, fieldName);
                Assert.True(hbeatMock.HbeatProps.ContainsKey(expectedFieldName));
                Assert.False(string.IsNullOrEmpty(hbeatMock.HbeatProps[expectedFieldName]));
            }
        }

        [TestMethod]
        public void FailToObtainAzureInstanceMetadataFieldsAltogether()
        {
            HeartbeatProviderMock hbeatMock = new HeartbeatProviderMock();
            AzureInstanceMetadataRequestMock azureInstanceRequestorMock = new AzureInstanceMetadataRequestMock(
                getComputeMetadata: () =>
                {
                    try
                    {
                        throw new System.Exception("Failure");
                    }
                    catch
                    {
                    }

                    return null;
                });
            var azureIMSFields = new AzureComputeMetadataHeartbeatPropertyProvider(azureInstanceRequestorMock);
            var defaultFields = azureIMSFields.ExpectedAzureImsFields;

            // not adding the fields we're looking for, simulation of the Azure Instance Metadata service not being present...
            var taskWaiter = azureIMSFields.SetDefaultPayloadAsync(hbeatMock).ConfigureAwait(false);
            Assert.False(taskWaiter.GetAwaiter().GetResult()); // nop await for tests

                getComputeMetadata: () =>
                {
                    return expected;
                });
            var azureIMSFields = new AzureComputeMetadataHeartbeatPropertyProvider(azureInstanceRequestorMock);
            var defaultFields = azureIMSFields.ExpectedAzureImsFields;

            // not adding the fields we're looking for, simulation of the Azure Instance Metadata service not being present...
            var taskWaiter = azureIMSFields.SetDefaultPayloadAsync(hbeatMock).ConfigureAwait(false);
            Assert.True(taskWaiter.GetAwaiter().GetResult()); // nop await for tests
            AzureInstanceComputeMetadata md = new AzureInstanceComputeMetadata();

            List<string> acceptableNames = new List<string>
            {
                "acceptable-(Name)_Here",
                "A",
                "0123456789012345678901234567890123456789012345678901234567890123",
                "(should-work-fine)"
            };

            {
                md.Name = goodName;
                Assert.Equal(md.Name, md.VerifyExpectedValue("name"));
            }

            List<string> acceptableResourceGroupNames = new List<string>
            {
                "1",
                "acceptable_resourceGr0uP.Name-Here",
                "012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789",
            {
                subId.ToString(),
                subId.ToString().ToLowerInvariant(),
                subId.ToString().ToUpperInvariant()
            };

            foreach (string goodSubscriptionId in acceptableSubscriptionIds)
            {
                md.SubscriptionId = goodSubscriptionId;
                Assert.Equal(md.SubscriptionId, md.VerifyExpectedValue("subscriptionId"));
            }
        }

        [TestMethod]
        public void AzureIMSTestFieldBadValuesFailVerification()
        {
            // there are three fields we verify within the AzureInstanceComputeMetadata class, test the
            // verification routines
            AzureInstanceComputeMetadata md = new AzureInstanceComputeMetadata();

        public void AzureIMSReturnsExpectedValuesForEachFieldAfterSerialization()
        {
            AzureInstanceComputeMetadata expectMetadata = new AzureInstanceComputeMetadata()
            {
                Location = "US-West",
                Name = "test-vm01",
                Offer = "D9_USWest",
                OsType = "Linux",
                PlacementGroupId = "placement-grp",
                PlatformFaultDomain = "0",
                PlatformUpdateDomain = "0",
                Publisher = "Microsoft",
                ResourceGroupName = "test.resource-group_01",
                Sku = "Windows_10",
                SubscriptionId = Guid.NewGuid().ToString(),
                Tags = "thisTag;thatTag",
                Version = "10.8a",
                VmId = Guid.NewGuid().ToString(),
                VmSize = "A8",
                VmScaleSetName = "ScaleName"
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(AzureInstanceComputeMetadata));

            // use the expected JSON field name style, uses camelCase...
            string jsonFormatString =
@"{{ 
  ""resourceGroupName"": ""{14}"",
  ""vmScaleSetName"": ""{15}""
}}";
            string json = string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                jsonFormatString,
                expectMetadata.OsType,
                expectMetadata.Location,
                expectMetadata.Name,
                expectMetadata.Offer,

            var jsonBytes = Encoding.UTF8.GetBytes(json);
            MemoryStream jsonStream = new MemoryStream(jsonBytes, 0, jsonBytes.Length);

            AzureInstanceComputeMetadata compareMetadata = (AzureInstanceComputeMetadata)deserializer.ReadObject(jsonStream);

            AzureComputeMetadataHeartbeatPropertyProvider heartbeatProps = new AzureComputeMetadataHeartbeatPropertyProvider();
            foreach (string fieldName in heartbeatProps.ExpectedAzureImsFields)
            {
                Assert.Equal(expectMetadata.GetValueForField(fieldName), compareMetadata.GetValueForField(fieldName));
            }
        }

        [TestMethod]
        public async Task AzureIMSGetFailsWithException()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())

            var requestor = mockAzureMetadataRequestor.Object;

            try
            {
                var result = await requestor.GetAzureComputeMetadataAsync();
                Assert.Null(result);
            }
            catch
            {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
