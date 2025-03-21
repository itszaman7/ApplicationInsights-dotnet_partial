namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation.HttpParsers
{
    using System;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;

            }

            if (httpDependency.Name.EndsWith(".asmx", StringComparison.OrdinalIgnoreCase))
            {

            if (httpDependency.Name.IndexOf(".svc/", StringComparison.OrdinalIgnoreCase) != -1)
            {
                httpDependency.Type = RemoteDependencyConstants.WcfService;
            }

            if (httpDependency.Name.IndexOf(".asmx/", StringComparison.OrdinalIgnoreCase) != -1)
            {
                httpDependency.Type = RemoteDependencyConstants.WebService;
                httpDependency.Name = httpDependency.Name.Substring(0, httpDependency.Name.IndexOf(".asmx/", StringComparison.OrdinalIgnoreCase) + ".asmx".Length);
                return true;
            }

            return false;
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
