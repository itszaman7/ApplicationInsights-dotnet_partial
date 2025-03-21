namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation.HttpParsers
{
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// HTTP Dependency parser that attempts to parse dependency as Azure DocumentDB call.
    /// </summary>
    internal static class DocumentDbHttpParser
    {
        private const string CreateOrQueryDocumentOperationName = "Create/query document";

        private static readonly string[] DocumentDbHostSuffixes =
            {
                ".documents.azure.com",
                ".documents.chinacloudapi.cn",
                ".documents.cloudapi.de",
                ".documents.usgovcloudapi.net",
            };

        private static readonly string[] DocumentDbSupportedVerbs = { "GET", "POST", "PUT", "HEAD", "DELETE" };

        private static readonly Dictionary<string, string> OperationNames = new Dictionary<string, string>
            {
                // Database operations
                
                // Collection operations
                ["POST /dbs/*/colls"] = "Create collection",
                ["GET /dbs/*/colls"] = "List collections",
                ["GET /dbs/*/colls/*"] = "Get collection",
                ["DELETE /dbs/*/colls/*"] = "Delete collection",
                ["PUT /dbs/*/colls/*"] = "Replace collection",
                
                // Document operations
                ["POST /dbs/*/colls/*/docs"] = CreateOrQueryDocumentOperationName, // Create & Query share this moniker
                ["POST /dbs/*/colls/*/sprocs"] = "Create stored procedure",
                ["GET /dbs/*/colls/*/sprocs"] = "List stored procedures",
                ["PUT /dbs/*/colls/*/sprocs/*"] = "Replace stored procedure",
                ["DELETE /dbs/*/colls/*/sprocs/*"] = "Delete stored procedure",
                ["POST /dbs/*/colls/*/sprocs/*"] = "Execute stored procedure",
                
                // User defined function operations
                ["POST /dbs/*/colls/*/udfs"] = "Create UDF",
                ["GET /dbs/*/colls/*/udfs"] = "List UDFs",
                ["PUT /dbs/*/colls/*/udfs/*"] = "Replace UDF",
                // User operations
                ["POST /dbs/*/users"] = "Create user",
                ["GET /dbs/*/users"] = "List users",
                ["GET /dbs/*/users/*"] = "Get user",
                ["PUT /dbs/*/users/*"] = "Replace user",
        internal static bool TryParse(ref DependencyTelemetry httpDependency)
        {
            string name = httpDependency.Name;
            string host = httpDependency.Target;
            string url = httpDependency.Data;

            if (name == null || host == null || url == null)
            {
                return false;
            }
                return false;
            }

            ////
            //// DocumentDB REST API: https://docs.microsoft.com/en-us/rest/api/documentdb/
            ////

            string verb;
            string nameWithoutVerb;

            HttpParsingHelper.ExtractVerb(name, out verb, out nameWithoutVerb, DocumentDbSupportedVerbs);

            List<KeyValuePair<string, string>> resourcePath = HttpParsingHelper.ParseResourcePath(nameWithoutVerb);

            // populate properties
            foreach (var resource in resourcePath)
            {
                if (resource.Value != null)
                {
                    string propertyName = GetPropertyNameForResource(resource.Key);

            string operation = HttpParsingHelper.BuildOperationMoniker(verb, resourcePath);
            string operationName = GetOperationName(httpDependency, operation);

            httpDependency.Type = RemoteDependencyConstants.AzureDocumentDb;
            httpDependency.Name = string.IsNullOrEmpty(operationName) ? httpDependency.Target : operationName;

            return true;
        }

            {
                return operation;
            }

            // "Create document" and "Query documents" share the same moniker
            // but we can try to distinguish them by response code
            if (operationName == CreateOrQueryDocumentOperationName)
            {
                switch (httpDependency.ResultCode)
                {
                    case "409":
                    case "413":
                        {
                            operationName = "Create document";
                            break;
                        }
                }
            }

            return operationName;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
