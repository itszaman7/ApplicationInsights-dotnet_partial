namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.Remoting.Messaging;
    using System.Web;

    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AzureAppServiceRoleNameFromHostNameHeaderInitializerTest
    {
        [TestCleanup]
        public void Cleanup()
        {
            Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", null);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", "SomeName");            
        }

        [TestMethod]
        public void InitializeDoesNotThrowIfHttpContextIsUnavailable()
        {
            Func<HttpContext> nullContext = () => { return null; };

            var initializer = new TestableAzureAppServiceRoleNameFromHostNameHeaderInitializer(null, nullContext);
        }

        [TestMethod]
        public void InitializeFallsbackToEnvIfHttpContextIsUnavailable()
        {
            try
            {
                Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", "RoleNameEnv");
                Func<HttpContext> nullContext = () => { return null; };


        [TestMethod]
        public void InitializeFallsbackToEnvIfHttpContextIsUnavailableWithAzureWebsitesHostnameending()
        {
            try
            {
                Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", "RoleNameEnv.azurewebsites.net");
                Func<HttpContext> nullContext = () => { return null; };

                var initializer = new TestableAzureAppServiceRoleNameFromHostNameHeaderInitializer(null, nullContext);
                });

            source.Initialize(eventTelemetry);
        }

        [TestMethod]
        public void InitializeFallsbackToEnvIfHostNameHeaderIsEmpty()
        {
            try
            {
                });

                source.Initialize(eventTelemetry);
                Assert.AreEqual("RoleNameEnv", eventTelemetry.Context.Cloud.RoleName);
            }
            finally
            {
                Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", null);
            }
        }
        }

        [TestMethod]
        public void InitializeFallsbackToEnvIfHostNameHeaderIsNull()
        {
            try
            {
                Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", "RoleNameEnv");
                var eventTelemetry = new EventTelemetry("name");
                var source = new TestableAzureAppServiceRoleNameFromHostNameHeaderInitializer(new Dictionary<string, string>
            }
        }

        [TestMethod]
        public void InitializeSetsRoleNameFromHostNameHeader()
        {
            var eventTelemetry = new EventTelemetry("name");
            var source = new TestableAzureAppServiceRoleNameFromHostNameHeaderInitializer(new Dictionary<string, string>
                {
                    { "WAS-DEFAULT-HOSTNAME", "RoleNameEnv" }
                });

            source.Initialize(eventTelemetry);

            Assert.AreEqual("RoleNameEnv", eventTelemetry.Context.Cloud.RoleName);
        }


            var eventTelemetry = new EventTelemetry("name");
            var source = new TestableAzureAppServiceRoleNameFromHostNameHeaderInitializer(resolveContext: nullContextAfterFirstCall);
            source.Initialize(eventTelemetry);
            Assert.AreEqual("RoleNameFromFirst", eventTelemetry.Context.Cloud.RoleName);

            var newEventTelemetry = new EventTelemetry("name");
            source.Initialize(newEventTelemetry);
            Assert.AreEqual("RoleNameFromFirst", newEventTelemetry.Context.Cloud.RoleName);
        }
                });

            source.Initialize(eventTelemetry);

            Assert.AreEqual("RoleNameEnv", eventTelemetry.Context.Cloud.RoleName);
        }

        [TestMethod]
        public void InitializeSetsRoleNameFromRequestTelemetryIfPresent()
        {
            var eventTelemetry = new EventTelemetry("name");
            var source = new TestableAzureAppServiceRoleNameFromHostNameHeaderInitializer(new Dictionary<string, string>
                {
                    { "WAS-DEFAULT-HOSTNAME", "RoleNameFromHostHeader" }
                }, getRequestFromContext: requestFromContext);

            source.Initialize(eventTelemetry);

            Assert.AreEqual("RoleNameFromHostHeader", eventTelemetry.Context.Cloud.RoleName);
            Assert.AreEqual("RoleNameFromHostHeader", requestTelemetry.Context.Cloud.RoleName);
                });

            source.WebAppSuffix = ".azurewebsites.us";
            source.Initialize(eventTelemetry);

            Assert.AreEqual("appserviceslottest-ppe", eventTelemetry.Context.Cloud.RoleName);
        }

        [TestMethod]
        public void InitializeSetsRoleNameFromEnvWithAzureWebsitesCustom()
                Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", null);
            }
        }

        [TestMethod]
        public void InitializeDoesNotOverrideRoleName()
        {
            var requestTelemetry = new RequestTelemetry();
            requestTelemetry.Context.Cloud.RoleName = "ExistingRoleName";
            var source = new TestableAzureAppServiceRoleNameFromHostNameHeaderInitializer(new Dictionary<string, string>
                {
                    { "WAS-DEFAULT-HOSTNAME", "appserviceslottest-ppe.azurewebsites.us" }
                });
            source.Initialize(requestTelemetry);

            Assert.AreEqual("ExistingRoleName", requestTelemetry.Context.Cloud.RoleName);
        }

        [TestMethod]
        public void InitializeReturnsIfNonWebApp()
            Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", null);
            var requestTelemetry = new RequestTelemetry();

            var source = new TestableAzureAppServiceRoleNameFromHostNameHeaderInitializer(new Dictionary<string, string>
                {
                    { "WAS-DEFAULT-HOSTNAME", "appserviceslottest-ppe.azurewebsites.us" }
                });
            source.Initialize(requestTelemetry);

            Assert.IsNull(requestTelemetry.Context.Cloud.RoleName);
            protected override HttpContext ResolvePlatformContext()
            {
                if (this.resolvePlatformContext != null)
                {
                    return this.resolvePlatformContext();
                }
                else
                {
                    return this.fakeContext;
                }
                if (this.getRequestFromContext != null)
                {
                    return this.getRequestFromContext(ctx);
                }
                else
                {
                    return null;
                }
            }
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
