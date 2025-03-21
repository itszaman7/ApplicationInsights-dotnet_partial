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
    public class PageViewTelemetryTest
    {
        [TestMethod]
        public void VerifyExpectedDefaultValue()
        {
            new ITelemetryTest<PageViewTelemetry, AI.PageViewData>().Run();
        }

        [TestMethod]
        public void PageViewTelemetryIsPublic()
        {
            Assert.IsTrue(typeof(PageViewTelemetry).GetTypeInfo().IsPublic);
        }

        [TestMethod]
        public void PageViewTelemetryImplementsISupportMetrics()
        {
            PageViewTelemetry item = new PageViewTelemetry();
            item.Metrics.Add("Test", 10);

            Assert.IsNotNull(item as ISupportMetrics);
            Assert.AreEqual(10, (item as ISupportMetrics).Metrics["Test"]);
        }

        [TestMethod]
        public void PageViewTelemetrySerializesToJsonCorrectly()
        {
            var expected = new PageViewTelemetry("My Page");
            expected.Url = new Uri("http://temp.org/page1");
            expected.Duration = TimeSpan.FromSeconds(123);
            expected.Metrics.Add("Metric1", 30);
            expected.Properties.Add("Property1", "Value1");

            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.PageViewData>(expected);

            AssertEx.AreEqual(expected.Properties.ToArray(), item.data.baseData.properties.ToArray());
        }

        [TestMethod]
        public void PageViewTelemetryTelemetryPropertiesFromContextAndItemSerializesToPropertiesInJson()
        {
            var expected = new PageViewTelemetry();
            expected.Context.GlobalProperties.Add("TestPropertyGlobal", "contextpropvalue");
            expected.Properties.Add("TestProperty", "TestPropertyValue");
            ((ITelemetry)expected).Sanitize();

        [TestMethod]
        public void SerializePopulatesRequiredFieldsOfPageViewTelemetry()
        {
            using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var pvTelemetry = new PageViewTelemetry();
                pvTelemetry.Context.InstrumentationKey = Guid.NewGuid().ToString();
                ((ITelemetry)pvTelemetry).Sanitize();
                var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.PageViewData>(pvTelemetry);
            Assert.AreEqual(new string('X', Property.MaxDictionaryNameLength), keys[1]);
            Assert.AreEqual(new string('X', Property.MaxValueLength), values[1]);
            Assert.AreEqual(new string('X', Property.MaxDictionaryNameLength - 3) + "1", keys[0]);
            Assert.AreEqual(new string('X', Property.MaxValueLength), values[0]);

            Assert.AreEqual(2, telemetry.Metrics.Count);
            keys = telemetry.Metrics.Keys.OrderBy(s => s).ToArray();
            Assert.AreEqual(new string('Y', Property.MaxDictionaryNameLength), keys[1]);
            Assert.AreEqual(new string('Y', Property.MaxDictionaryNameLength - 3) + "1", keys[0]);

            Assert.AreEqual(new Uri("http://foo.com/" + new string('Y', Property.MaxUrlLength - 15)), telemetry.Url);
        }

        [TestMethod]
        public void SanitizePopulatesNameWithErrorBecauseItIsRequiredByEndpoint()
        {
            var telemetry = new PageViewTelemetry { Name = null };

            ((ITelemetry)telemetry).Sanitize();

            Assert.AreEqual("n/a", telemetry.Name);
        }

        [TestMethod]
        public void PageViewTelemetryImplementsISupportSamplingContract()
        {
            var telemetry = new PageViewTelemetry();

            Assert.IsNotNull(telemetry as ISupportSampling);
        }

        [TestMethod]
        public void PageViewTelemetryImplementsISupportAdvancedSamplingContract()
        {
            var telemetry = new PageViewTelemetry();

            Assert.IsNotNull(telemetry as ISupportAdvancedSampling);
        }

        [TestMethod]
        public void PageViewTelemetryHasCorrectValueOfSamplingPercentageAfterSerialization()
        {
            pageView.Url = new Uri("http://temp.org/page1");
            pageView.Duration = TimeSpan.FromSeconds(123);
            pageView.Metrics.Add("Metric1", 30);
            pageView.Properties.Add("Property1", "Value1");
            pageView.Extension = new MyTestExtension();
            PageViewTelemetry other = (PageViewTelemetry)pageView.DeepClone();

            CompareLogic deepComparator = new CompareLogic();
            var result = deepComparator.Compare(pageView, other);
            Assert.IsTrue(result.AreEqual, result.DifferencesString);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
