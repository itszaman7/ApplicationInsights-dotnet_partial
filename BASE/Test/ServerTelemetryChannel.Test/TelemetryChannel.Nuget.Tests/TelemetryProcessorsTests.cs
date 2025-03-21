namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel
{
    using System.Diagnostics;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TelemetryProcessorsTests
    {
            var typeToFind = typeof(AdaptiveSamplingTelemetryProcessor);

            Trace.WriteLine(configAfterTransform.ToString());
            var processors = ConfigurationHelpers.GetTelemetryProcessorsFromDefaultSink(configAfterTransform);

            Assert.AreEqual(3, processors.Count());

            var type = processors.FirstOrDefault(element => element.Attribute("Type").Value == ConfigurationHelpers.GetPartialTypeName(typeToFind));
            var excludedTypes = processors.Descendants().Where(element => element.Name.LocalName == "ExcludedTypes").First().Value;
            Assert.AreEqual("Event", excludedTypes);

            var maxItems = processors.Descendants().Where(element => element.Name.LocalName == "MaxTelemetryItemsPerSecond").First().Value;
            Assert.AreEqual("5", maxItems);

            type = processors.LastOrDefault(element => element.Attribute("Type").Value == ConfigurationHelpers.GetPartialTypeName(typeToFind));
            Assert.IsNotNull(type);

            var includedTypes = processors.Descendants().Where(element => element.Name.LocalName == "IncludedTypes").First().Value;
            Assert.AreEqual("Event", includedTypes);

            maxItems = processors.Descendants().Where(element => element.Name.LocalName == "MaxTelemetryItemsPerSecond").Last().Value;
            Assert.AreEqual("5", maxItems);
        }

        [TestMethod]
        public void UninstallRemovesAllInstalledTelemetryProcessors()
        {
            string emptyConfig = ConfigurationHelpers.GetEmptyConfig();


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
