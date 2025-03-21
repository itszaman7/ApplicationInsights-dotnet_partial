namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using KellermanSoftware.CompareNetObjects;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DependencyTelemetryTest
    {
        /// <summary>
        /// The SDKs (and our customers) expect specific default values.
        /// This test is to verify that changes to the schema don't unexpectedly change our public api.
        /// </summary>
        [TestMethod]
        public void VerifyExpectedDefaultValue()
        {
            var defaultDependencyTelemetry = new DependencyTelemetry();
            Assert.AreEqual(true, defaultDependencyTelemetry.Success, "Success is expected to be true");
            Assert.IsNotNull(defaultDependencyTelemetry.Target);
            Assert.IsNotNull(defaultDependencyTelemetry.Name);
            Assert.IsNotNull(defaultDependencyTelemetry.Data);
            Assert.IsNotNull(defaultDependencyTelemetry.ResultCode);            
            Assert.IsNotNull(defaultDependencyTelemetry.Type);
            Assert.IsNotNull(defaultDependencyTelemetry.Id);
            Assert.AreEqual(SamplingDecision.None, defaultDependencyTelemetry.ProactiveSamplingDecision);
            Assert.AreEqual(SamplingTelemetryItemTypes.RemoteDependency, defaultDependencyTelemetry.ItemTypeFlag);
            Assert.IsTrue(defaultDependencyTelemetry.Id.Length >= 1);
        }

        [TestMethod]
        public void DependencyTelemetryITelemetryContractConsistentlyWithOtherTelemetryTypes()
        {
            new ITelemetryTest<DependencyTelemetry, AI.RemoteDependencyData>().Run();
        }

        [TestMethod]
        public void DependencyTelemetryPropertiesFromContextAndItemSerializesToPropertiesInJson()
        {
            var expected = CreateRemoteDependencyTelemetry();

            ((ITelemetry)expected).Sanitize();
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.AvailabilityData>(expected);

            // Items added to both dependency.Properties, and dependency.Context.GlobalProperties are serialized to properties.
            // IExtension object in CreateDependencyTelemetry adds 2 more properties: myIntField and myStringField
            Assert.AreEqual(4, item.data.baseData.properties.Count);            
            Assert.IsTrue(item.data.baseData.properties.ContainsKey("TestPropertyGlobal"));
            Assert.IsTrue(item.data.baseData.properties.ContainsKey("TestProperty"));
        }

        [TestMethod]
            Assert.AreEqual(expected.Context.InstrumentationKey, item.iKey);
            AssertEx.AreEqual(expected.Context.SanitizedTags.ToArray(), item.tags.ToArray());
            Assert.AreEqual(nameof(AI.RemoteDependencyData), item.data.baseType);

            Assert.AreEqual(expected.Id, item.data.baseData.id);
            Assert.AreEqual(expected.ResultCode, item.data.baseData.resultCode);
            Assert.AreEqual(expected.Name, item.data.baseData.name);
            Assert.AreEqual(expected.Duration, TimeSpan.Parse(item.data.baseData.duration));
            Assert.AreEqual(expected.Type, item.data.baseData.type);
            Assert.AreEqual(expected.Success, item.data.baseData.success);

            AssertEx.AreEqual(expected.Properties.ToArray(), item.data.baseData.properties.ToArray());
        }

        [TestMethod]
        /// Test validates that if Serialize is called multiple times, and telemetry is modified
        /// in between, serialize always gives the latest state.
        public void RemoteDependencySerializationPicksUpCorrectState()
        {
            DependencyTelemetry expected = this.CreateRemoteDependencyTelemetry();

            // Change the telemetry after serialization.
            expected.Name = expected.Name + "new";

            // Validate that the newly updated Name is picked up.
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.RemoteDependencyData>(expected);

            Assert.AreEqual<DateTimeOffset>(expected.Timestamp, DateTimeOffset.Parse(item.time, null, System.Globalization.DateTimeStyles.AssumeUniversal));
            Assert.AreEqual(expected.Sequence, item.seq);
            Assert.AreEqual(expected.Context.InstrumentationKey, item.iKey);
            DependencyTelemetry expected = this.CreateRemoteDependencyTelemetry();
            expected.Context.InstrumentationKey = "AIC-" + expected.Context.InstrumentationKey;
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.RemoteDependencyData>(expected);

            Assert.AreEqual(expected.Context.InstrumentationKey, item.iKey);
        }

        [TestMethod]
        public void RemoteDependencyTelemetrySerializeCommandNameToJson()
        {
            AI.RemoteDependencyData dp = item.data.baseData;
            Assert.AreEqual(expected.Data, dp.data);
        }

        [TestMethod]
        public void RemoteDependencyTelemetrySerializeNullCommandNameToJson()
        {
            DependencyTelemetry expected = this.CreateRemoteDependencyTelemetry(null);
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.RemoteDependencyData>(expected);
            AI.RemoteDependencyData dp = item.data.baseData;
            Assert.IsTrue(string.IsNullOrEmpty(dp.data));
        }

        [TestMethod]
        public void DependencyTypeNameDefaultsToEmptyInConstructor()
        {
#pragma warning disable 618
            var dependency = new DependencyTelemetry("name", "command name", DateTimeOffset.Now, TimeSpan.FromSeconds(42), false);

            AssertEx.IsEmpty(dependency.DependencyKind);
#pragma warning restore 618

            AssertEx.IsEmpty(dependency.Type);
        }

        [TestMethod]
        public void SerttingDependencyKindSetsDependencyTypeName()
        {
            DependencyTelemetry expected = new DependencyTelemetry();
#pragma warning disable 618

            Assert.AreSame(telemetry.Properties, telemetry.Properties);

            Assert.AreEqual(2, telemetry.Metrics.Count);
            var keys = telemetry.Metrics.Keys.OrderBy(s => s).ToArray();
            Assert.AreEqual(new string('Y', Property.MaxDictionaryNameLength), keys[1]);
            Assert.AreEqual(new string('Y', Property.MaxDictionaryNameLength - 3) + "1", keys[0]);
        }

        [TestMethod]

            Assert.IsNotNull(telemetry as ISupportSampling);
        }

        [TestMethod]
        public void DependencyTelemetryImplementsISupportAdvancedSamplingContract()
        {
            var telemetry = new DependencyTelemetry();

            Assert.IsNotNull(telemetry as ISupportAdvancedSampling);
            var telemetry = this.CreateRemoteDependencyTelemetry("mycommand");
            ((ISupportSampling)telemetry).SamplingPercentage = 10;

            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.RemoteDependencyData>(telemetry);

            Assert.AreEqual(10, item.sampleRate);
        }

        [TestMethod]
        public void DependencyTelemetrySetGetOperationDetail()
        {
            const string key = "foo";
            const string detail = "bar";

            var telemetry = this.CreateRemoteDependencyTelemetry("mycommand");
            telemetry.SetOperationDetail(key, detail);
            Assert.IsTrue(telemetry.TryGetOperationDetail(key, out object retrievedValue));
            Assert.IsNotNull(retrievedValue);
            Assert.AreEqual(detail, retrievedValue.ToString());

        }

        [TestMethod]
        public void DependencyTelemetryGetUnsetOperationDetail()
        {
            const string key = "foo";

            var telemetry = this.CreateRemoteDependencyTelemetry("mycommand");
            Assert.IsFalse(telemetry.TryGetOperationDetail(key, out object retrievedValue));
            Assert.IsNull(retrievedValue);
        [TestMethod]
        public void DependencyTelemetryDeepCloneCopiesAllProperties()
        {
            DependencyTelemetry telemetry = CreateRemoteDependencyTelemetry();
            DependencyTelemetry other = (DependencyTelemetry)telemetry.DeepClone();

            ComparisonConfig comparisonConfig = new ComparisonConfig();
            CompareLogic deepComparator = new CompareLogic(comparisonConfig);

            ComparisonResult result = deepComparator.Compare(telemetry, other);
        }

        private DependencyTelemetry CreateRemoteDependencyTelemetry()
        {
            DependencyTelemetry item = new DependencyTelemetry
                                            {
                                                Timestamp = DateTimeOffset.Now,


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
