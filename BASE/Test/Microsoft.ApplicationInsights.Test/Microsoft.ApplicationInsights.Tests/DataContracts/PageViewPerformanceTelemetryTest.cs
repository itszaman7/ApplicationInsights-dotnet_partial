namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using CompareLogic = KellermanSoftware.CompareNetObjects.CompareLogic;


    [TestClass]
    public class PageViewPerformanceTelemetryTest
    {
        [TestMethod]
        public void VerifyExpectedDefaultValue()
        {
        {
            new ITelemetryTest<PageViewPerformanceTelemetry, AI.PageViewPerfData>().Run();
        }

        [TestMethod]
        public void PageViewPerformanceTelemetryIsPublic()
        {
            Assert.IsTrue(typeof(PageViewPerformanceTelemetry).GetTypeInfo().IsPublic);
        }

        }

        [TestMethod]
        public void PageViewPerformanceTelemetrySuppliesConstructorThatTakesNameParameter()
        {
            string expectedPageName = "My page view";
            var instance = new PageViewPerformanceTelemetry(expectedPageName);
            Assert.AreEqual(expectedPageName, instance.Name);
        }

        [TestMethod]
        public void PageViewPerformanceTelemetryReturnsDefaultDurationAsTimespanZero()
        {
            PageViewPerformanceTelemetry item = new PageViewPerformanceTelemetry();
            Assert.AreEqual(TimeSpan.Zero, item.Duration);
        }

        [TestMethod]
        public void PageViewPerformanceTelemetrySerializesToJsonCorrectly()
        {
            expected.Url = new Uri("http://temp.org/page1");
            expected.Duration = TimeSpan.FromSeconds(123);
            expected.Metrics.Add("Metric1", 30);
            expected.Properties.Add("Property1", "Value1");

            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.PageViewPerfData>(expected);

            // NOTE: It's correct that we use the v1 name here, and therefore we test against it.
            Assert.AreEqual(item.name, AI.ItemType.PageViewPerformance);

            Assert.AreEqual(expected.Name, item.data.baseData.name);
            Assert.AreEqual(expected.Duration, TimeSpan.Parse(item.data.baseData.duration));
            Assert.AreEqual(expected.Url.ToString(), item.data.baseData.url);

            AssertEx.AreEqual(expected.Properties.ToArray(), item.data.baseData.properties.ToArray());
        }

        [TestMethod]
        public void PageViewPerformanceTelemetryTelemetryPropertiesFromContextAndItemSerializesToPropertiesInJson()
        {
            Assert.AreEqual(1, expected.Context.GlobalProperties.Count);

            Assert.IsTrue(expected.Properties.ContainsKey("TestProperty"));
            Assert.IsTrue(expected.Context.GlobalProperties.ContainsKey("TestPropertyGlobal"));

            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.PageViewPerfData>(expected);

            // Items added to both PageViewPerformanceTelemetry.Properties, and PageViewPerformanceTelemetry.Context.GlobalProperties are serialized to properties.
            Assert.AreEqual(2, item.data.baseData.properties.Count);
            Assert.IsTrue(item.data.baseData.properties.ContainsKey("TestPropertyGlobal"));

        [TestMethod]
        public void SerializePopulatesRequiredFieldsOfPageViewPerfTelemetry()
        {
            using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var pvTelemetry = new PageViewPerformanceTelemetry();
                pvTelemetry.Context.InstrumentationKey = Guid.NewGuid().ToString();
                ((ITelemetry)pvTelemetry).Sanitize();
                var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.PageViewPerfData>(pvTelemetry);
                Assert.AreEqual(new TimeSpan(), TimeSpan.Parse(item.data.baseData.perfTotal));
                Assert.AreEqual(new TimeSpan(), TimeSpan.Parse(item.data.baseData.receivedResponse));
                Assert.AreEqual(new TimeSpan(), TimeSpan.Parse(item.data.baseData.sentRequest));
            }
        }


        [TestMethod]
        public void SanitizeWillTrimAppropriateFields()
        {

            ((ITelemetry)telemetry).Sanitize();

            Assert.AreEqual(new string('Z', Property.MaxNameLength), telemetry.Name);

            Assert.AreEqual(2, telemetry.Properties.Count);
            string[] keys = telemetry.Properties.Keys.OrderBy(s => s).ToArray();
            string[] values = telemetry.Properties.Values.OrderBy(s => s).ToArray();
            Assert.AreEqual(new string('X', Property.MaxDictionaryNameLength), keys[1]);
            Assert.AreEqual(new string('X', Property.MaxValueLength), values[1]);


            Assert.AreEqual(new Uri("http://foo.com/" + new string('Y', Property.MaxUrlLength - 15)), telemetry.Url);
        }

        [TestMethod]
        public void SanitizePopulatesNameWithErrorBecauseItIsRequiredByEndpoint()
        {
            var telemetry = new PageViewPerformanceTelemetry { Name = null };

            ((ITelemetry)telemetry).Sanitize();


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
