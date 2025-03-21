namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation.HttpParsers
{
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;

    /// <summary>
    /// HTTP Dependency parser that attempts to parse dependency as Azure Table call.
                ".table.core.usgovcloudapi.net",
            };

        private static readonly string[] AzureTableSupportedVerbs = { "GET", "PUT", "OPTIONS", "HEAD", "DELETE", "MERGE", "POST" };

        /// <summary>

            if (!HttpParsingHelper.EndsWithAny(host, AzureTableHostSuffixes))
            {
                return false;
            }

            string nameWithoutVerb;

            // try to parse out the verb
            HttpParsingHelper.ExtractVerb(name, out verb, out nameWithoutVerb, AzureTableSupportedVerbs);

            List<string> pathTokens = HttpParsingHelper.TokenizeRequestPath(nameWithoutVerb);
            int idx = tableName.IndexOf('(');
            if (idx >= 0)
            {
                tableName = tableName.Substring(0, idx);
            }

            httpDependency.Type = RemoteDependencyConstants.AzureTable;
            httpDependency.Name = string.IsNullOrEmpty(verb)
                                      ? account + '/' + tableName
                                      : verb + " " + account + '/' + tableName;

            return true;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
