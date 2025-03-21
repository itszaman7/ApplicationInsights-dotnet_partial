namespace Microsoft.ApplicationInsights.WindowsServer.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    internal class AzureComputeMetadataHeartbeatPropertyProvider
    {
        internal const string HeartbeatPropertyPrefix = "azInst_"; // to ensure no collisions with base heartbeat properties

        /// <summary>
        /// Expected fields extracted from Azure IMS to add to the heartbeat properties. 
        /// Set as internal for testing.
        /// </summary>
        internal readonly IReadOnlyCollection<string> ExpectedAzureImsFields = new string[]
        {
            "offer",
            "osType",
            "placementGroupId",
            "platformFaultDomain",
            "platformUpdateDomain",
            "publisher",
            "resourceGroupName",
        /// <param name="azureInstanceMetadataHandler">For testing: Azure metadata request handler to use when requesting data from azure specifically. If left as null, an instance of AzureMetadataRequestor is used.</param>
        internal AzureComputeMetadataHeartbeatPropertyProvider(IAzureMetadataRequestor azureInstanceMetadataHandler = null)
        {
            this.azureInstanceMetadataRequestor = azureInstanceMetadataHandler ?? new AzureMetadataRequestor();
        }

        /// <summary>

            try
            {
                if (!this.isAzureMetadataCheckCompleted)
                {
                    this.isAzureMetadataCheckCompleted = true;

                        var enabledImdsFields = this.ExpectedAzureImsFields.Except(provider.ExcludedHeartbeatProperties);
                        foreach (string field in enabledImdsFields)
                        {
                            string verifiedValue = azureComputeMetadata.VerifyExpectedValue(field);
                            if (!addedProperty)
                            {
                                WindowsServerEventSource.Log.AzureInstanceMetadataWasntAddedToHeartbeatProperties(field, verifiedValue);
                            }

                            hasSetFields = hasSetFields || addedProperty;
                        }
                        WindowsServerEventSource.Log.AzureInstanceMetadataNotAdded();
                    }
                }
            }
            catch (Exception setPayloadException)
            {
                WindowsServerEventSource.Log.AzureInstanceMetadataFailureSettingDefaultPayload(setPayloadException.Message, setPayloadException.InnerException?.Message);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
