namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.HttpParsers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DocumentDbHttpParserTests
    {
        [TestMethod]
        public void DocumentDbHttpParserConvertsValidDependencies()
        {
            Dictionary<string, string> defaultProperties = null;
            var databaseProperties 
                = new Dictionary<string, string> { ["Database"] = "myDatabase" };
            var collectionProperties 
                = new Dictionary<string, string> { ["Database"] = "myDatabase", ["Collection"] = "myCollection" };
            var sprocProperties 
                = new Dictionary<string, string> { ["Database"] = "myDatabase", ["Collection"] = "myCollection", ["Stored procedure"] = "mySproc" };
            var udfProperties 
                = new Dictionary<string, string> { ["Database"] = "myDatabase", ["Collection"] = "myCollection", ["User defined function"] = "myUdf" };
            var triggerProperties 
                = new Dictionary<string, string> { ["Database"] = "myDatabase", ["Collection"] = "myCollection", ["Trigger"] = "myTrigger" };

            string defaultResultCode = "200";

            var testCases = new List<Tuple<string, string, string, Dictionary<string, string>, string>>()
                Tuple.Create("Create collection",           "POST",     "https://myaccount.documents.azure.com/dbs/myDatabase/colls", databaseProperties, defaultResultCode),
                Tuple.Create("List collections",            "GET",      "https://myaccount.documents.azure.com/dbs/myDatabase/colls", databaseProperties, defaultResultCode),
                Tuple.Create("Get collection",              "GET",      "https://myaccount.documents.azure.com/dbs/myDatabase/colls/myCollection", collectionProperties, defaultResultCode),
                Tuple.Create("Delete collection",           "DELETE",   "https://myaccount.documents.azure.com/dbs/myDatabase/colls/myCollection", collectionProperties, defaultResultCode),
                Tuple.Create("Replace collection",          "PUT",      "https://myaccount.documents.azure.com/dbs/myDatabase/colls/myCollection", collectionProperties, defaultResultCode),
                
                // Document operations
                Tuple.Create("Create document",             "POST",     "https://myaccount.documents.azure.com/dbs/myDatabase/colls/myCollection/docs", collectionProperties, "201"),
                Tuple.Create("Query documents",             "POST",     "https://myaccount.documents.azure.com/dbs/myDatabase/colls/myCollection/docs", collectionProperties, "200"),
                Tuple.Create("Create/query document",       "POST",     "https://myaccount.documents.azure.com/dbs/myDatabase/colls/myCollection/docs", collectionProperties, "400"),
                
                // Stored procedure operations
                Tuple.Create("Create stored procedure",     "POST",     "https://myaccount.documents.azure.com/dbs/myDatabase/colls/myCollection/sprocs", collectionProperties, defaultResultCode),
                Tuple.Create("List stored procedures",      "GET",      "https://myaccount.documents.azure.com/dbs/myDatabase/colls/myCollection/sprocs", collectionProperties, defaultResultCode),
                Tuple.Create("Replace stored procedure",    "PUT",      "https://myaccount.documents.azure.com/dbs/myDatabase/colls/myCollection/sprocs/mySproc", sprocProperties, defaultResultCode),
                Tuple.Create("Delete stored procedure",     "DELETE",   "https://myaccount.documents.azure.com/dbs/myDatabase/colls/myCollection/sprocs/mySproc", sprocProperties, defaultResultCode),
                Tuple.Create("Execute stored procedure",    "POST",     "https://myaccount.documents.azure.com/dbs/myDatabase/colls/myCollection/sprocs/mySproc", sprocProperties, defaultResultCode),
                
                // User defined function operations
                Tuple.Create("Create UDF",                  "POST",     "https://myaccount.documents.azure.com/dbs/myDatabase/colls/myCollection/udfs", collectionProperties, defaultResultCode),
                Tuple.Create("Get offer",                   "GET",      "https://myaccount.documents.azure.com/offers/myOffer", defaultProperties, defaultResultCode),
                Tuple.Create("Replace offer",               "PUT",      "https://myaccount.documents.azure.com/offers/myOffer", defaultProperties, defaultResultCode),
            };

            foreach (var testCase in testCases)
            {
                this.DocumentDbHttpParserConvertsValidDependencies(
                    testCase.Item1,
                    testCase.Item2,
                    testCase.Item3,
                this.DocumentDbHttpParserConvertsValidDependencies(
                    testCase.Item1,
                    testCase.Item2,
                    testCase.Item3,
                    testCase.Item4,
                    testCase.Item5);
            }
        }

        private void DocumentDbHttpParserConvertsValidDependencies(
            var d = new DependencyTelemetry(
                dependencyTypeName: RemoteDependencyConstants.HTTP,
                target: parsedUrl.Host,
                dependencyName: verb + " " + parsedUrl.AbsolutePath,
                data: parsedUrl.OriginalString)
            {
                ResultCode = resultCode ?? "200"
            };

            bool success = DocumentDbHttpParser.TryParse(ref d);
            Assert.IsTrue(success, operation);
            Assert.AreEqual(RemoteDependencyConstants.AzureDocumentDb, d.Type, operation);
            {
                foreach (var property in properties)
                {
                    string value = null;
                    Assert.IsTrue(d.Properties.TryGetValue(property.Key, out value), operation);
                    Assert.AreEqual(property.Value, value, operation);
                }
            }

            // Parse without verb
            d = new DependencyTelemetry(
                dependencyTypeName: RemoteDependencyConstants.HTTP,
                target: parsedUrl.Host,
                dependencyName: parsedUrl.AbsolutePath,
                data: parsedUrl.OriginalString)
            {
                ResultCode = resultCode ?? "200"
            };

            success = DocumentDbHttpParser.TryParse(ref d);
            if (properties != null)
            {
                foreach (var property in properties)
                {
                    string value = null;
                    Assert.IsTrue(d.Properties.TryGetValue(property.Key, out value), operation);
                    Assert.AreEqual(property.Value, value, operation);
                }
            }
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
