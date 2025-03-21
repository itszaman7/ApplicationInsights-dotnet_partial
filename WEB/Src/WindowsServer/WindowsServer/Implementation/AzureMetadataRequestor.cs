namespace Microsoft.ApplicationInsights.WindowsServer.Implementation
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net.Http;
    using System.Runtime.Serialization.Json;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation.DataContracts;

    internal class AzureMetadataRequestor : IAzureMetadataRequestor
    {
        /// </summary>
        /// <remarks>
        /// At this time, this service does not support https. We should monitor their website for more information. https://docs.microsoft.com/azure/virtual-machines/windows/instance-metadata-service .
        /// </remarks>
        internal string BaseAimsUri { get; set; } = "http://169.254.169.254/metadata/instance/compute";

        public Task<AzureInstanceComputeMetadata> GetAzureComputeMetadataAsync()
        {
            try
            {
                requestResult = await this.MakeWebRequestAsync(metadataRequestUrl).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                WindowsServerEventSource.Log.AzureInstanceMetadataRequestFailure(
                    metadataRequestUrl, ex.Message, ex.InnerException != null ? ex.InnerException.Message : string.Empty);
        }

        private async Task<AzureInstanceComputeMetadata> MakeWebRequestAsync(string requestUrl)
        {
            AzureInstanceComputeMetadata azureIms = null;
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(AzureInstanceComputeMetadata));

            using (var azureImsClient = this.GetHttpClient())
            {
                azureImsClient.MaxResponseContentBufferSize = AzureMetadataRequestor.AzureImsMaxResponseBufferSize;
                azureImsClient.DefaultRequestHeaders.Add("Metadata", "True");
                azureImsClient.Timeout = this.AzureImsRequestTimeout;

                Stream content = await azureImsClient.GetStreamAsync(new Uri(requestUrl)).ConfigureAwait(false);
                azureIms = (AzureInstanceComputeMetadata)deserializer.ReadObject(content);
                content.Dispose();
                if (azureIms == null)
                {
                    WindowsServerEventSource.Log.CannotObtainAzureInstanceMetadata();
                }
            }

            return azureIms;
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
