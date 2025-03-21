namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.ApplicationInsights.TestFramework;
    using System.Collections.Generic;

    [TestClass]
    public class TelemetryContextTest
    {
        [TestMethod]
        public void TelemetryContextIsPublicAndMeantToBeUsedByCustomers()
        {
            Assert.IsTrue(typeof(TelemetryContext).GetTypeInfo().IsPublic);
        }

        [TestMethod]
        public void TelemetryContextIsSealedToSupportCompilationAsWinmd()
        {
            Assert.IsTrue(typeof(TelemetryContext).GetTypeInfo().IsSealed);
        }

        [TestMethod]
        public void InstrumentationKeyIsNotNullByDefaultToPreventNullReferenceExceptionsInUserCode()
        {
            var context = new TelemetryContext();
            Assert.IsNotNull(context.InstrumentationKey);
        }

        [TestMethod]
        public void InstrumentationKeySetterThrowsArgumentNullExceptionWhenValueIsNullToPreventNullReferenceExceptionsLater()
        {
            var context = new TelemetryContext();
            AssertEx.Throws<ArgumentNullException>(() => context.InstrumentationKey = null);
        }

        [TestMethod]
        public void FlagsIsZeroByDefault()
        {
            var context = new TelemetryContext();
            Assert.AreEqual(0, context.Flags);
        }

        [TestMethod]
        public void FlagsCanBeSetAndGet()
        {
            var context = new TelemetryContext();
            context.Flags |= 0x00100000;
            Assert.AreEqual(0x00100000, context.Flags);
        }

        [TestMethod]
        public void ComponentIsNotNullByDefaultToPreventNullReferenceExceptionsInUserCode()
        {
            var context = new TelemetryContext();
            Assert.IsNotNull(context.Component);
        }

        [TestMethod]
        public void DeviceIsNotNullByDefaultToPreventNullReferenceExceptionsInUserCode()
        {
            var context = new TelemetryContext();
            Assert.IsNotNull(context.Device);
        }

        [TestMethod]
        public void SessionIsNotNullByDefaultToPreventNullReferenceExceptionsInUserCode()
        {
            var context = new TelemetryContext();
            Assert.IsNotNull(context.Session);
        }

        [TestMethod]
        public void UserIsNotNullByDefaultToPreventNullReferenceExceptionsInUserCode()
        {
            var context = new TelemetryContext();
            Assert.IsNotNull(context.User);
        }

        [TestMethod]
        public void OperationIsNotNullByDefaultToPreventNullReferenceExceptionsInUserCode()
        {
            var context = new TelemetryContext();
            Assert.IsNotNull(context.Operation);
        }

            Assert.AreEqual("TestValue", target.InstrumentationKey);
        }

        [TestMethod]
        public void InitializeInstrumentationKeySetsTelemetryInstrumentationKey()
        {
            var sourceInstrumentationKey = "TestValue";
            var target = new TelemetryContext();

            target.InitializeInstrumentationkey(sourceInstrumentationKey);
            var target = new TelemetryContext();

            target.Initialize(source, source.InstrumentationKey);

            Assert.AreEqual(0x00100000, target.Flags);
        }


        [TestMethod]
        public void InitializeSetsFlagsFromArgument()

            target.Initialize(source, source.InstrumentationKey);

            Assert.AreEqual(0x00110000, target.Flags);
        }

        [TestMethod]
        public void SerializeWritesCopiedDeviceContext()
        {
            var context = new TelemetryContext();
            var context = new TelemetryContext();
            context.Component.Version = "Test Value";
            string json = CopyAndSerialize(context);
            AssertEx.Contains("\"" + ContextTagKeys.Keys.ApplicationVersion + "\":\"Test Value\"", json, StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void SerializeWritesCopiedLocationContext()
        {
            var context = new TelemetryContext();
            context.Location.Ip = "1.2.3.4";
            string json = CopyAndSerialize(context);
            AssertEx.Contains("\"" + ContextTagKeys.Keys.LocationIp + "\":\"1.2.3.4\"", json, StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void SerializeWritesCopiedUserContext()
        {
            var context = new TelemetryContext();
            context.User.Id = "Test Value";
            string json = CopyAndSerialize(context);
            AssertEx.Contains("\"" + ContextTagKeys.Keys.UserId + "\":\"Test Value\"", json, StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void SerializeWritesCopiedOperationContext()
        {
            var context = new TelemetryContext();
            context.Operation.Id = "Test Value";
            string json = CopyAndSerialize(context);

        [TestMethod]
        public void SerializeWritesCopiedSessionContext()
        {
            var context = new TelemetryContext();
            context.Session.Id = "Test Value";
            string json = CopyAndSerialize(context);
            AssertEx.Contains("\"" + ContextTagKeys.Keys.SessionId + "\":\"Test Value\"", json, StringComparison.OrdinalIgnoreCase);
        }


            Assert.IsTrue(context.GlobalProperties.ContainsKey(expectedKeyWithSizeWithinLimit));
            var value = context.GlobalProperties[expectedKeyWithSizeWithinLimit];
            Assert.AreEqual(expectedValueWithSizeWithinLimit, value);
        }

        [TestMethod]
        public void TestStoresRawObject()
        {
            const string key = "foo";
        {
            const string key = "";
            const string detail = "bar";
            var context = new TelemetryContext();

            // These shouldn't throw.
            context.StoreRawObject(null, detail);
            context.StoreRawObject(null, detail, false);
            context.TryGetRawObject(null, out object actualDontExist);

            context.TryGetRawObject(null, out object actualDontExist1);

            context.StoreRawObject("key", null);
            Assert.IsTrue(context.TryGetRawObject("key", out object actual));
            Assert.AreSame(null, actual);

            context.StoreRawObject(key, detail);
            Assert.IsTrue(context.TryGetRawObject(key, out object actual1));
            Assert.AreSame(detail, actual1);

        [TestMethod]
        public void TestStoreRawObjectTempByDefault()
        {
            const string keyTemp = "fooTemp";
            const string detailTemp = "barTemp";
            const string keyPerm = "fooPerm";
            const string detailPerm = "barPerm";

            var context = new TelemetryContext();
            context.StoreRawObject(keyTemp, detailTemp);
            const string keyPerm = "fooPerm";
            const string detailPerm = "barPerm";

            var context = new TelemetryContext();
            context.StoreRawObject(keyTemp, detailTemp, true);
            context.StoreRawObject(keyPerm, detailPerm, false);
            
            Assert.IsTrue(context.TryGetRawObject(keyTemp, out object temp));
            Assert.IsTrue(context.TryGetRawObject(keyPerm, out object perm));

            context.ClearTempRawObjects();
            Assert.IsFalse(context.TryGetRawObject(keyTemp, out object tempAfterCleanup));
            Assert.IsTrue(context.TryGetRawObject(keyPerm, out object permAfterCleanup));
        }

        [TestMethod]
        public void TestRawObjectIsSharedOnDeepCopy()
        {
            string keyTemp = "keyTemp";
            string detailTemp = "valueTemp";
            var detailTempObj = new MyCustomObject(detailTemp);
            string keyPerm = "keyPerm";
            string detailPerm = "valuePerm";
            var detailPermObj = new MyCustomObject(detailPerm);

            var context = new TelemetryContext();
            context.StoreRawObject(keyTemp, detailTempObj);
            context.StoreRawObject(keyPerm, detailPermObj, false);
            Assert.IsTrue(context.TryGetRawObject(keyTemp, out object actualTemp));
            Assert.AreEqual(detailTempObj, actualTemp);
            // Modify the object in original context
            context.TryGetRawObject(keyTemp, out object tempObjFromOriginal);
            ((MyCustomObject)tempObjFromOriginal).v = "new temp value";

            clonedContext.TryGetRawObject(keyTemp, out object tempObjFromClone);

            // validate that modifying original context affects the clone.
            Assert.AreEqual(((MyCustomObject)tempObjFromOriginal).v, ((MyCustomObject)tempObjFromClone).v);
        }

    internal class MyCustomObject
    {
        public string v;

        public MyCustomObject(string v)
        {
            this.v = v;
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
