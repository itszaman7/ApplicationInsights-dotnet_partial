namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using Microsoft.ApplicationInsights.Channel;

    internal class CloudRoleInstanceDimensionExtractor : IDimensionExtractor
        {
            return item.Context.Cloud.RoleInstance;
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
