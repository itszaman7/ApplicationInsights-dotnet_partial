﻿#if !NET452 && !NET46 && !REDFIELD
namespace Microsoft.ApplicationInsights.TestFramework.Implementation
{
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.TestFramework.Helpers;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// These tests verify that <see cref="TelemetryConfiguration"/> can receive and store an instance of <see cref="Azure.Core.TokenCredential"/>.
    /// </summary>
    /// <remarks>
    /// These tests do not run in NET452 OR NET46. 
    /// In these cases, the test runner is NET452 or NET46 and Azure.Core.TokenCredential is NOT SUPPORTED in these frameworks.
    /// This does not affect the end user because we REQUIRE the end user to create their own instance of TokenCredential.
    /// This ensures that the end user is consuming the AI SDK in one of the newer frameworks.
    /// </remarks>
    [TestClass]

            // ACT
            // set credential first
            tc.SetAzureTokenCredential(new MockCredential());
            Assert.IsTrue(tc.TelemetryChannel.EndpointAddress.Contains("v2.1")); // api switch
            // test new channel
            var channel = new ServerTelemetryChannel();
            Assert.IsNull(channel.EndpointAddress); // new channel defaults null
            // SETUP
            var tc = TelemetryConfiguration.CreateDefault();
            Assert.IsInstanceOfType(tc.TelemetryChannel, typeof(InMemoryChannel));
            Assert.IsTrue(tc.TelemetryChannel.EndpointAddress.Contains("v2")); // defaults to old api

            // ACT
            // set new channel first
            var channel = new ServerTelemetryChannel();
            Assert.IsNull(channel.EndpointAddress); // new channel defaults null



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
