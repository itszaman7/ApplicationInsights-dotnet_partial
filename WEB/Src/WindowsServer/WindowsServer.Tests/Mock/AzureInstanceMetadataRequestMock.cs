namespace Microsoft.ApplicationInsights.WindowsServer.Mock
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation;

    internal class AzureInstanceMetadataRequestMock : IAzureMetadataRequestor
    {
        public AzureInstanceComputeMetadata ComputeMetadata;
                Name = "vm-testRg-num1",
                Offer = "OneYouCannotPassUp",
                OsType = "Windows",
                PlacementGroupId = "plat-grp-id",
                PlatformFaultDomain = "0",
                PlatformUpdateDomain = "0",
                Publisher = "Microsoft-Vancouver",
                ResourceGroupName = "testRg",
                SubscriptionId = Guid.NewGuid().ToString(),
                Version = "0.0.0",
                VmId = Guid.NewGuid().ToString(),
                VmSize = "A01",
                VmScaleSetName = "ScaleName"
        {
            return Task.FromResult(this.getComputeMetadata());
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
