namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Web;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Helpers;
    using Microsoft.ApplicationInsights.Web.Implementation;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AuthenticatedUserIdTelemetryInitializerTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            while (Activity.Current != null)
            {
                Activity.Current.Stop();
            }
        }

        [TestMethod]
        public void InitializeDoesNotThrowWhenHttpContextIsNull()
        {
            // Arrange
            HttpContext.Current = null;
            var source = new AuthenticatedUserIdTelemetryInitializer();


            // Assert
            Assert.AreEqual("2", eventTelemetry.Context.User.AuthenticatedUserId);
        }

        [TestMethod]
        public void InitializeDoesNotSetAuthIdIfCookieIsEmpty()
        {
            // Arrange
            var initializer = new TestableAuthenticatedUserIdTelemetryInitializer();
            initializer.Initialize(new StubTelemetry());

            // Assert
            Assert.AreEqual(null, requestTelemetry.Context.User.AuthenticatedUserId);
        }

        [TestMethod]
        public void InitializeDoesNotSetAuthIdIfCookieINull()
        {
            // Arrange
            string cookieString = null;
            RequestTelemetry requestTelemetry = initializer.FakeContext.WithAuthCookie(cookieString);

            // Act
            initializer.Initialize(new StubTelemetry());

            // Assert
            Assert.AreEqual(null, requestTelemetry.Context.User.AuthenticatedUserId);
        }

            var cookieString = "123|account123";
            RequestTelemetry requestTelemetry = initializer.FakeContext.WithAuthCookie(cookieString);

            // Act
            initializer.Initialize(new StubTelemetry());

            // Assert
            Assert.AreEqual("123", requestTelemetry.Context.User.AuthenticatedUserId);
        }


        [TestMethod]
        public void InitializeReadsAuthIdFromSpecialCharactersInCookie()
        {
            // Arrange
            var initializer = new TestableAuthenticatedUserIdTelemetryInitializer();
            // Arrange
            var initializer = new TestableAuthenticatedUserIdTelemetryInitializer();
            var cookieString = "|";
            RequestTelemetry requestTelemetry = initializer.FakeContext.WithAuthCookie(cookieString);

            // Act
            initializer.Initialize(new StubTelemetry());

            // Assert
            Assert.AreEqual(null, requestTelemetry.Context.User.AuthenticatedUserId);
        }

        private class TestableAuthenticatedUserIdTelemetryInitializer : AuthenticatedUserIdTelemetryInitializer
        {
            private readonly HttpContext fakeContext = HttpModuleHelper.GetFakeHttpContext();

            public HttpContext FakeContext
            {
                get { return this.fakeContext; }
            }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
