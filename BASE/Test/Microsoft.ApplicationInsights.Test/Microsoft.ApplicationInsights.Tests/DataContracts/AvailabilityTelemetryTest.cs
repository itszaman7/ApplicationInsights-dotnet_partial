namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using KellermanSoftware.CompareNetObjects;

    [TestClass]
    public class AvailabilityTelemetryTest
    {
        [TestMethod]
        public void AvailabilityTelemetryPropertiesFromContextAndItemSerializesToPropertiesInJson()
        {
            var expected = CreateAvailabilityTelemetry();

            ((ITelemetry)expected).Sanitize();

            Assert.AreEqual(1, expected.Properties.Count);
            Assert.AreEqual(1, expected.Context.GlobalProperties.Count);

            Assert.IsTrue(expected.Properties.ContainsKey("TestProperty"));
            Assert.IsTrue(expected.Context.GlobalProperties.ContainsKey("TestPropertyGlobal"));

            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.AvailabilityData>(expected);

            // Items added to both availability.Properties, and availability.Context.GlobalProperties are serialized to properties.
            // IExtension object in CreateAvailabilityTelemetry adds 2 more properties: myIntField and myStringField
            Assert.AreEqual(4, item.data.baseData.properties.Count);
            Assert.IsTrue(item.data.baseData.properties.ContainsKey("TestPropertyGlobal"));
            Assert.IsTrue(item.data.baseData.properties.ContainsKey("TestProperty"));
        }
            Assert.AreEqual(expected.Success, item.data.baseData.success);
            Assert.AreEqual(expected.RunLocation, item.data.baseData.runLocation);
            Assert.AreEqual(expected.Name, item.data.baseData.name);
            Assert.AreEqual(expected.Id.ToString(), item.data.baseData.id);

            // IExtension is currently flattened into the properties by serialization
            Utils.CopyDictionary(((MyTestExtension)expected.Extension).SerializeIntoDictionary(), expected.Properties);

            AssertEx.AreEqual(expected.Properties.ToArray(), item.data.baseData.properties.ToArray());
        }

        [TestMethod]
        public void SanitizeWillTrimAppropriateFieldsForAvailability()
        {
            AvailabilityTelemetry telemetry = new AvailabilityTelemetry();
            telemetry.Message = new string('Z', Property.MaxAvailabilityMessageLength + 1);
            telemetry.RunLocation = new string('Y', Property.MaxRunLocationLength + 1);
            telemetry.Name = new string('D', Property.MaxTestNameLength + 1);
            telemetry.Properties.Add(new string('X', Property.MaxDictionaryNameLength) + 'X', new string('X', Property.MaxValueLength + 1));
            telemetry.Properties.Add(new string('X', Property.MaxDictionaryNameLength) + 'Y', new string('X', Property.MaxValueLength + 1));
            var t = new SortedList<string, string>(telemetry.Properties);

            Assert.AreEqual(new string('X', Property.MaxDictionaryNameLength), t.Keys.ToArray()[1]);
            Assert.AreEqual(new string('X', Property.MaxValueLength), t.Values.ToArray()[1]);
            Assert.AreEqual(new string('X', Property.MaxDictionaryNameLength - 3) + "1", t.Keys.ToArray()[0]);
            Assert.AreEqual(new string('X', Property.MaxValueLength), t.Values.ToArray()[0]);

            Assert.AreSame(telemetry.Properties, telemetry.Properties);
        }


            Assert.AreEqual(telemetry.Data.message, "Failed");
        }

        [TestMethod]
        public void SanitizeWillSetPassedMessageForAvailability()
        {
            AvailabilityTelemetry telemetry = CreateAvailabilityTelemetry();
            telemetry.Message = string.Empty;

        {
            AvailabilityTelemetry telemetry = new AvailabilityTelemetry();

            Assert.AreEqual(telemetry.Data.success, true);
        }

        [TestMethod]
        public void AssignmentWillCastSuccessToResult()
        {
            AvailabilityTelemetry telemetry = new AvailabilityTelemetry();
            AvailabilityTelemetry item = new AvailabilityTelemetry
            {
                Message = "Test Message",
                RunLocation = "Test Location",
                Name = "Test Name",
                Duration = TimeSpan.FromSeconds(30),
            item.Sequence = "12";
            item.Extension = new MyTestExtension() { myIntField = 42, myStringField = "value" };
            return item;
        }

        private AvailabilityTelemetry CreateAvailabilityTelemetry(string testName)
        {
            AvailabilityTelemetry item = this.CreateAvailabilityTelemetry();
            item.Name = testName;
            return item;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
