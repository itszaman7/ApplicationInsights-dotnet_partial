namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation.HttpParsers
{
    using System;
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;

    /// <summary>
    /// HTTP Dependency parser that attempts to parse dependency as Azure Blob call.
    /// </summary>
    internal static class AzureBlobHttpParser
    {
        private static readonly string[] AzureBlobHostSuffixes =
            {
                ".blob.core.windows.net",
                ".blob.core.chinacloudapi.cn",
                ".blob.core.cloudapi.de",

            if (!HttpParsingHelper.EndsWithAny(host, AzureBlobHostSuffixes))
            {
                return false;
            }

            ////

            string account = host.Substring(0, host.IndexOf('.'));

            string verb;
            string nameWithoutVerb;

            // try to parse out the verb
            List<string> pathTokens = HttpParsingHelper.TokenizeRequestPath(nameWithoutVerb);

            if (pathTokens.Count == 1)
            {
                container = pathTokens[0];
            } 
            else if (pathTokens.Count > 1)
            {
                Dictionary<string, string> queryParameters = HttpParsingHelper.ExtractQuryParameters(url);
                string resType;
                if (queryParameters == null || !queryParameters.TryGetValue("restype", out resType)
                    || !string.Equals(resType, "container", StringComparison.OrdinalIgnoreCase))
                {
                    // if restype != container then the last path entry is blob name
                    blob = pathTokens[pathTokens.Count - 1];
                    httpDependency.Properties["Blob"] = blob;
            // Possible improvements:
            //
            // 1. Use specific name for specific operations. Like "Lease Blob" for "?comp=lease" query parameter
            // 2. Use account name as a target instead of "account.blob.core.windows.net"
            httpDependency.Type = RemoteDependencyConstants.AzureBlob;
            httpDependency.Name = string.IsNullOrEmpty(verb) ? account : verb + " " + account;



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
