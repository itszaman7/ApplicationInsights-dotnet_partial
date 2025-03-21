namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;

    [TestClass]
    public class AzureServiceBusHttpParserTests
    {
        [TestMethod]
        public void AzureServiceBusHttpParserSupportsNationalClouds()
        {
            var testCases = new List<Tuple<string, string, string>>()
            {
                Tuple.Create("Send message", "POST", "https://myaccount.servicebus.windows.net/myQueue/messages"),
                Tuple.Create("Send message", "POST", "https://myaccount.servicebus.chinacloudapi.cn/myQueue/messages"),
                Tuple.Create("Send message", "POST", "https://myaccount.servicebus.cloudapi.de/myQueue/messages"),
                this.AzureServiceBusHttpParserConvertsValidDependencies(
                    testCase.Item1,
                    testCase.Item2,
                    testCase.Item3);
            }
        }
            var d = new DependencyTelemetry(
                dependencyTypeName: RemoteDependencyConstants.HTTP,
                target: parsedUrl.Host,
                dependencyName: verb + " " + parsedUrl.AbsolutePath,
                data: parsedUrl.OriginalString);

            Assert.IsTrue(success, operation);
            Assert.AreEqual(RemoteDependencyConstants.AzureServiceBus, d.Type, operation);
            Assert.AreEqual(parsedUrl.Host, d.Target, operation);
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
