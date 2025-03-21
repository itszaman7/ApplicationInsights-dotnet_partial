namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.HttpParsers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AzureBlobHttpParserTests
    {
        [TestMethod]
        public void AzureBlobHttpParserConvertsValidDependencies()
        {
            Dictionary<string, string> defaultProperties = null;
            var containerProperties
                = new Dictionary<string, string> { ["Container"] = "my/container" };
            var blobProperties
                = new Dictionary<string, string> { ["Container"] = "my/container", ["Blob"] = "myblob" };

            var testCases = new List<Tuple<string, string, string, string, Dictionary<string, string>>>()
            {
                ////
                //// copied from https://msdn.microsoft.com/en-us/library/azure/dd135733.aspx 9/29/2016
                ////

                Tuple.Create("List Containers",                  "GET",      "https://myaccount.blob.core.windows.net/?comp=list",                                                           "myaccount", defaultProperties),
                Tuple.Create("Set Blob Service Properties",      "PUT",      "https://myaccount.blob.core.windows.net/?restype=service&comp=properties",                                     "myaccount", defaultProperties),
                Tuple.Create("Get Blob Service Properties",      "GET",      "https://myaccount.blob.core.windows.net/?restype=service&comp=properties",                                     "myaccount", defaultProperties),
                Tuple.Create("Preflight Blob Request",           "OPTIONS",  "http://myaccount.blob.core.windows.net/my/container/myblob",                                               "myaccount", blobProperties),
                Tuple.Create("Put Block List",                   "PUT",      "https://myaccount.blob.core.windows.net/my/container/myblob?comp=blocklist",                                    "myaccount", blobProperties),
                Tuple.Create("Get Block List",                   "GET",      "https://myaccount.blob.core.windows.net/my/container/myblob?comp=blocklist",                                    "myaccount", blobProperties),
                Tuple.Create("Get Block List",                   "GET",      "https://myaccount.blob.core.windows.net/my/container/myblob?comp=blocklist&snapshot=DateTime",                  "myaccount", blobProperties),
                Tuple.Create("Put Page",                         "PUT",      "https://myaccount.blob.core.windows.net/my/container/myblob?comp=page",                                         "myaccount", blobProperties),
                Tuple.Create("Get Page Ranges",                  "GET",      "https://myaccount.blob.core.windows.net/my/container/myblob?comp=pagelist",                                     "myaccount", blobProperties),
                Tuple.Create("Get Page Ranges",                  "GET",      "https://myaccount.blob.core.windows.net/my/container/myblob?comp=pagelist&snapshot=DateTime",                   "myaccount", blobProperties),
                Tuple.Create("Get Page Ranges",                  "GET",      "https://myaccount.blob.core.windows.net/my/container/myblob?comp=pagelist&snapshot=DateTime&prevsnapshot=Date", "myaccount", blobProperties),
                Tuple.Create("Append Block",                     "PUT",      "https://myaccount.blob.core.windows.net/my/container/myblob?comp=appendblock",                                  "myaccount", blobProperties)
            };

                    testCase.Item5);
            }
        }

        [TestMethod]
        public void AzureBlobHttpParserSupportsNationalClouds()
        {
            var blobProperties
                = new Dictionary<string, string> { ["Container"] = "my/container", ["Blob"] = "myblob" };

                    testCase.Item5);
            }
        }

        public void EnsureAzureBlobHttpParserConvertsValidDependency(
            string operation,
            string verb,
            string url,
            string accountName,
            Dictionary<string, string> properties)
        {
            Uri parsedUrl = new Uri(url);

            // Parse with verb
            var d = new DependencyTelemetry(
                dependencyTypeName: RemoteDependencyConstants.HTTP,
                target: parsedUrl.Host,
                dependencyName: verb + " " + parsedUrl.AbsolutePath,
                data: parsedUrl.OriginalString);

            if (properties != null)
            {
                foreach (var property in properties)
                {
                    string value = null;
                    Assert.IsTrue(d.Properties.TryGetValue(property.Key, out value), operation);
                    Assert.AreEqual(property.Value, value, operation);
                }
            }

            Assert.AreEqual(RemoteDependencyConstants.AzureBlob, d.Type, operation);
            Assert.AreEqual(parsedUrl.Host, d.Target, operation);
            Assert.AreEqual(accountName, d.Name, operation);

            if (properties != null)
            {
                foreach (var property in properties)
                {
                    string value = null;
                    Assert.IsTrue(d.Properties.TryGetValue(property.Key, out value), operation);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
