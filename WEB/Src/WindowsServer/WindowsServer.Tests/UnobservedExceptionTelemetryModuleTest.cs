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
    [TestClass]
    public class UnobservedExceptionTelemetryModuleTest
    {
        private TelemetryConfiguration moduleConfiguration;
        private IList<ITelemetry> items;

        [TestInitialize]
        public void TestInitialize()
        {
            this.items = new List<ITelemetry>();
            {
                OnSend = telemetry => this.items.Add(telemetry),
                EndpointAddress = "http://test.com"
            };

            this.moduleConfiguration = new TelemetryConfiguration
            {
                TelemetryChannel = moduleChannel,
                InstrumentationKey = "MyKey",
            };
            string expectedVersion = SdkVersionHelper.GetExpectedSdkVersion(typeof(UnobservedExceptionTelemetryModule), prefix: "unobs:");

            EventHandler<UnobservedTaskExceptionEventArgs> handler = null;
            using (var module = new UnobservedExceptionTelemetryModule(
                h => handler = h,
                _ => { }))
            {
                module.Initialize(this.moduleConfiguration);
                handler.Invoke(null, new UnobservedTaskExceptionEventArgs(new AggregateException(string.Empty)));
            }
            using (var module = new UnobservedExceptionTelemetryModule(
                h => handler = h,
                _ => { }))
            {
                module.Initialize(this.moduleConfiguration);
            }

            Assert.NotNull(handler);
        }

        public void InitializeCallsRegisterOnce()
        {
            int count = 0;

            using (var module = new UnobservedExceptionTelemetryModule(
                _ => ++count,
                _ => { }))
            {
                for (int i = 0; i < 50; ++i)
                {
                    tasks[i] = System.Threading.Tasks.Task.Run(() => module.Initialize(this.moduleConfiguration));
                }

                System.Threading.Tasks.Task.WhenAll(tasks).Wait();
            }


        [TestMethod]
        public void DisposeCallsUnregister()
        {
            EventHandler<UnobservedTaskExceptionEventArgs> handler = null;
            using (var module = new UnobservedExceptionTelemetryModule(
                _ => { },
                h => handler = h))
            {
                module.Initialize(this.moduleConfiguration);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
