namespace Microsoft.ApplicationInsights.Web
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Web;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WebTestTelemetryInitializerTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            while (Activity.Current != null)
            {
                Activity.Current.Stop();
            }
        }

                });

            source.Initialize(eventTelemetry);

            Assert.AreEqual("SOURCE", eventTelemetry.Context.Operation.SyntheticSource);
        }

        [TestMethod]
        public void SyntheticSourceIsSetToWellKnownValue()
        {
        [TestMethod]
        public void UserIdIsSetToLocationPlusRun()
        {
            var eventTelemetry = new EventTelemetry("name");
            var source = new TestableWebTestTelemetryInitializer(new Dictionary<string, string>
                {
                    { "SyntheticTest-Location", "LOCATION" },
                    { "synthetictest-runid", "ID" },
                });

            Assert.IsNull(eventTelemetry.Context.Session.Id);
            Assert.IsNull(eventTelemetry.Context.Operation.SyntheticSource);
        }

        [TestMethod]
        public void UserSessionSourceAreNotSetIfRunIsNotSet()
        {
            var eventTelemetry = new EventTelemetry("name");
            var source = new TestableWebTestTelemetryInitializer(new Dictionary<string, string>
                {
                    { "SyntheticTest-Location", "LOCATION" },
                });

            source.Initialize(eventTelemetry);

            Assert.IsNull(eventTelemetry.Context.User.Id);
            Assert.IsNull(eventTelemetry.Context.Session.Id);
            Assert.IsNull(eventTelemetry.Context.Operation.SyntheticSource);
        }

        [TestMethod]
        public void UserIdIsNotOverriden()
        {
            var eventTelemetry = new EventTelemetry("name");

            eventTelemetry.Context.User.Id = "UserId";
            eventTelemetry.Context.Operation.SyntheticSource = "SOURCE";

            var source = new TestableWebTestTelemetryInitializer(new Dictionary<string, string>
                {
                    { "synthetictest-runid", "ID" },
                });

            source.Initialize(eventTelemetry);

            Assert.AreEqual("SessionId", eventTelemetry.Context.Session.Id);
        }

        [TestMethod]
        public void SessionIdIsSetToRunId()
        {
            var eventTelemetry = new EventTelemetry("name");
            public TestableWebTestTelemetryInitializer(IDictionary<string, string> headers = null)
            {
                this.fakeContext = HttpModuleHelper.GetFakeHttpContext(headers);
            }

            protected override HttpContext ResolvePlatformContext()
            {
                return this.fakeContext;
            }
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
