#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Assert = Xunit.Assert;
    using TaskEx = System.Threading.Tasks.Task;

    [TestClass]
    public class UnhandledExceptionTelemetryModuleTest
    {
        private StubTelemetryChannel moduleChannel;
        private IList<ITelemetry> items;

        [TestInitialize]
        {
            this.moduleChannel = null;
            this.items.Clear();
        }

        [TestMethod]
        public void EndpointAddressFromConfigurationActiveIsUsedForSending()
        {
            UnhandledExceptionEventHandler handler = null;
            using (var module = new UnhandledExceptionTelemetryModule(
                h => handler = h,
                _ => { },
                this.moduleChannel))
            {
                handler.Invoke(null, new UnhandledExceptionEventArgs(null, true));
            }

            Assert.Equal("http://test.com", this.moduleChannel.EndpointAddress);
        }

        [TestMethod]
        public void TelemetryInitializersFromConfigurationActiveAreUsedForSending()
        {
            bool called = false;
            var telemetryInitializer = new StubTelemetryInitializer { OnInitialize = item => called = true };

            TelemetryConfiguration.Active.TelemetryInitializers.Add(telemetryInitializer);
            
            UnhandledExceptionEventHandler handler = null;
            using (var module = new UnhandledExceptionTelemetryModule(
        [TestMethod]
        public void InstrumentationKeyCanBeOverridenInCodeAfterModuleIsCreated()
        {
            // This scenario is important for CloudApps where everything exept iKey comes from ai.config
            UnhandledExceptionEventHandler handler = null;
            using (var module = new UnhandledExceptionTelemetryModule(
                h => handler = h,
                _ => { },
                this.moduleChannel))
            {

        [TestMethod]
        public void TrackedExceptionsHavePrefixUsedForTelemetry()
        {
            string expectedVersion = SdkVersionHelper.GetExpectedSdkVersion(typeof(UnhandledExceptionTelemetryModule), prefix: "unhnd:");
            
            UnhandledExceptionEventHandler handler = null;
            using (new UnhandledExceptionTelemetryModule(
                h => handler = h,
                _ => { },
            }

            Assert.Equal(expectedVersion, this.items[0].Context.GetInternalContext().SdkVersion);
        }

        [TestMethod]
        public void TrackedExceptionsHaveCorrectMessage()
        {
            UnhandledExceptionEventHandler handler = null;
            using (var module = new UnhandledExceptionTelemetryModule(
        public void ModuleConstructorCallsRegister()
        {
            UnhandledExceptionEventHandler handler = null;
            using (var module = new UnhandledExceptionTelemetryModule(
                h => handler = h,
                _ => { },
                this.moduleChannel))
            {
            }

        }
                _ => { },
                _ => { },
                this.moduleChannel))
            {
            }

            Assert.True(called);
        }

        [TestMethod]


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
