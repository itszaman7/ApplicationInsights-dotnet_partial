namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Diagnostics;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests corresponding to TelemetryClientExtension methods.
    /// </summary>
    [TestClass]
    public class OperationHolderTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ActivityFormatHelper.EnableW3CFormatInActivity();
        public void CreatingOperationItemDoesNotThrowOnPassingValidArguments()
        {
            var operationItem = new OperationHolder<DependencyTelemetry>(new TelemetryClient(TelemetryConfiguration.CreateDefault()), new DependencyTelemetry());
        }

        [TestMethod]
        public void CreatingOperationHolderWithDetachedOriginalActivityRestoresIt()
        {
            var client = new TelemetryClient(TelemetryConfiguration.CreateDefault());

            var originalActivity = new Activity("original").Start();
            var operation = new OperationHolder<DependencyTelemetry>(client, new DependencyTelemetry(), originalActivity);

            var newActivity = new Activity("new").SetParentId("detached-parent").Start();
            Assert.AreEqual(Activity.Current, originalActivity);
        }

        [TestMethod]
        public void CreatingOperationHolderWithNullOriginalActivityDoesNotRestoreIt()
        {
            var client = new TelemetryClient(TelemetryConfiguration.CreateDefault());
            operation.Telemetry.Id = newActivity.SpanId.ToHexString();

            operation.Dispose();

            //Assert.IsNotNull(Activity.Current);
        }

        [TestMethod]
        public void CreatingOperationHolderWithParentActivityRestoresIt()
        {
            var client = new TelemetryClient(TelemetryConfiguration.CreateDefault());

            var originalActivity = new Activity("original").Start();
            var operation = new OperationHolder<DependencyTelemetry>(client, new DependencyTelemetry(), originalActivity);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
