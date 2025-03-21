// <copyright file="ILoggerIntegrationTests.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Microsoft.ApplicationInsights
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.ApplicationInsights;
    using Microsoft.Extensions.Options;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the <see cref="ILogger"/> integration for Application Insights.
    /// </summary>
    [TestClass]
    public class ILoggerIntegrationTests
    {
        /// <summary>
        /// Ensures that <see cref="ApplicationInsightsLogger"/> populates params for structured logging into custom properties <see cref="ILogger"/>.
        /// </summary>
        [TestMethod]
        [TestCategory("ILogger")]
        public void ApplicationInsightsLoggerPopulateStructureLoggingParamsIntoCustomProperties()
        {
            List<ITelemetry> itemsReceived = new List<ITelemetry>();

            // Scopes are enabled.
            IServiceProvider serviceProvider = ILoggerIntegrationTests.SetupApplicationInsightsLoggerIntegration(
                (telemetryItem, telemetryProcessor) => itemsReceived.Add(telemetryItem),
                configureTelemetryConfiguration: null,
                configureApplicationInsightsOptions: (appInsightsLoggerOptions) => appInsightsLoggerOptions.IncludeScopes = true);

            ILogger<ILoggerIntegrationTests> testLogger = serviceProvider.GetRequiredService<ILogger<ILoggerIntegrationTests>>();
            testLogger.LogInformation("Testing structured with {CustomerName} {Age}", "TestCustomerName", 20);

            Assert.AreEqual("Testing structured with TestCustomerName 20", (itemsReceived[0] as TraceTelemetry).Message);
            var customProperties = (itemsReceived[0] as TraceTelemetry).Properties;
            Assert.IsTrue(customProperties["CustomerName"].Equals("TestCustomerName"));
            Assert.IsTrue(customProperties["Age"].Equals("20"));
        }

        /// <summary>
        /// Ensures that <see cref="ApplicationInsightsLogger"/> populates params for structured logging into custom properties <see cref="ILogger"/>.
        /// </summary>
        [TestMethod]
        [TestCategory("ILogger")]
        public void ApplicationInsightsLoggerPopulateStructureLoggingParamsIntoCustomPropertiesWhenScopeDisabled()
        {
            List<ITelemetry> itemsReceived = new List<ITelemetry>();

            // Disable scope
            IServiceProvider serviceProvider = ILoggerIntegrationTests.SetupApplicationInsightsLoggerIntegration(
                (telemetryItem, telemetryProcessor) => itemsReceived.Add(telemetryItem),
                configureTelemetryConfiguration: null,
                configureApplicationInsightsOptions: (appInsightsLoggerOptions) => appInsightsLoggerOptions.IncludeScopes = false);

            ILogger<ILoggerIntegrationTests> testLogger = serviceProvider.GetRequiredService<ILogger<ILoggerIntegrationTests>>();
            testLogger.LogInformation("Testing structured with {CustomerName} {Age}", "TestCustomerName", 20);

            Assert.AreEqual("Testing structured with TestCustomerName 20", (itemsReceived[0] as TraceTelemetry).Message);
            var customProperties = (itemsReceived[0] as TraceTelemetry).Properties;
            Assert.IsTrue(customProperties["CustomerName"].Equals("TestCustomerName"));
            Assert.IsTrue(customProperties["Age"].Equals("20"));
            Assert.IsTrue(customProperties.ContainsKey("OriginalFormat"));
            Assert.IsFalse(customProperties.ContainsKey("{OriginalFormat}"));
        }

        /// <summary>
        /// Ensures that <see cref="ApplicationInsightsLogger"/> is invoked when user logs using <see cref="ILogger"/>.
        /// </summary>
        [TestMethod]
        [TestCategory("ILogger")]
        public void ApplicationInsightsLoggerIsInvokedWhenUsingILogger()
        {
            List<ITelemetry> itemsReceived = new List<ITelemetry>();

            IServiceProvider serviceProvider = ILoggerIntegrationTests.SetupApplicationInsightsLoggerIntegration((telemetryItem, telemetryProcessor) =>
            {
                itemsReceived.Add(telemetryItem);
            });

            ILogger<ILoggerIntegrationTests> testLogger = serviceProvider.GetRequiredService<ILogger<ILoggerIntegrationTests>>();

            testLogger.LogInformation("Testing");
            testLogger.LogError(new Exception("ExceptionMessage"), "LoggerMessage");
            testLogger.LogInformation(new EventId(100, "TestEvent"), "TestingEvent");
            testLogger.LogCritical("Critical");
            testLogger.LogTrace("Trace");
            testLogger.LogWarning("Warning");
            testLogger.LogDebug("Debug");
            testLogger.Log(LogLevel.None, "None");

            Assert.AreEqual(SeverityLevel.Verbose, (itemsReceived[4] as TraceTelemetry).SeverityLevel);
            Assert.AreEqual(SeverityLevel.Warning, (itemsReceived[5] as TraceTelemetry).SeverityLevel);
            Assert.AreEqual("ExceptionMessage", (itemsReceived[1] as ExceptionTelemetry).Message);
            Assert.AreEqual("LoggerMessage", (itemsReceived[1] as ExceptionTelemetry).Properties["FormattedMessage"]);
            Assert.AreEqual("TestingEvent", (itemsReceived[2] as TraceTelemetry).Message);
            Assert.AreEqual("Critical", (itemsReceived[3] as TraceTelemetry).Message);
            Assert.AreEqual("Trace", (itemsReceived[4] as TraceTelemetry).Message);
            Assert.AreEqual("Warning", (itemsReceived[5] as TraceTelemetry).Message);
            Assert.AreEqual("Debug", (itemsReceived[6] as TraceTelemetry).Message);
        }

        /// <summary>
                itemsReceived.Add(telemetryItem);
            }, configuration =>
            {
                configuration.DisableTelemetry = true;
            });

            ILogger<ILoggerIntegrationTests> testLogger = serviceProvider.GetRequiredService<ILogger<ILoggerIntegrationTests>>();

            testLogger.LogInformation("Testing");
            testLogger.LogError(new Exception("ExceptionMessage"), "LoggerMessage");
            testLogger.LogTrace("Trace");
            testLogger.LogWarning("Warning");
            testLogger.LogDebug("Debug");
            testLogger.Log(LogLevel.None, "None");

            Assert.AreEqual(0, itemsReceived.Count);
        }

        /// <summary>
        /// Ensures that the <see cref="ApplicationInsightsLoggerOptions.TrackExceptionsAsExceptionTelemetry"/> switch is honored
        /// Ensures that the <see cref="ApplicationInsightsLoggerOptions.IncludeScopes"/> switch is honored and scopes are added
        /// when switch is true.
        /// </summary>
        [TestMethod]
        [TestCategory("ILogger")]
        public void ApplicationInsightsLoggerAddsScopeWhenSwitchIsTrue()
        {
            List<ITelemetry> itemsReceived = new List<ITelemetry>();

            // Case where Scope is included.
            IServiceProvider serviceProvider = ILoggerIntegrationTests.SetupApplicationInsightsLoggerIntegration(
                (telemetryItem, telemetryProcessor) => itemsReceived.Add(telemetryItem),
                configureTelemetryConfiguration: null,
                configureApplicationInsightsOptions: (appInsightsLoggerOptions) => appInsightsLoggerOptions.IncludeScopes = true);

            ILogger<ILoggerIntegrationTests> testLogger = serviceProvider.GetRequiredService<ILogger<ILoggerIntegrationTests>>();

            using (testLogger.BeginScope("TestScope"))
            {
                using (testLogger.BeginScope<IReadOnlyCollection<KeyValuePair<string, object>>>(new Dictionary<string, object> { { "Key", "Value" } }))
            Assert.AreEqual("ExceptionMessage", (itemsReceived[1] as ExceptionTelemetry).Message);
        }

        /// <summary>
        /// Ensures that the <see cref="ApplicationInsightsLoggerOptions.IncludeScopes"/> switch is honored and scopes are excluded
        /// when switch is false.
        /// </summary>
        [TestMethod]
        [TestCategory("ILogger")]
        public void ApplicationInsightsLoggerDoesNotAddScopeWhenSwitchIsFalse()
        {
            List<ITelemetry> itemsReceived = new List<ITelemetry>();

            // Case where Scope is NOT Included
            IServiceProvider serviceProvider = ILoggerIntegrationTests.SetupApplicationInsightsLoggerIntegration(
                (telemetryItem, telemetryProcessor) => itemsReceived.Add(telemetryItem),
                configureTelemetryConfiguration: null,
                configureApplicationInsightsOptions: (appInsightsLoggerOptions) => appInsightsLoggerOptions.IncludeScopes = false);

            ILogger<ILoggerIntegrationTests> testLogger = serviceProvider.GetRequiredService<ILogger<ILoggerIntegrationTests>>();

            using (testLogger.BeginScope("TestScope"))
            {
                using (testLogger.BeginScope<IReadOnlyCollection<KeyValuePair<string, object>>>(new Dictionary<string, object> { { "Key", "Value" } }))
                {
                    testLogger.LogInformation("Testing");
                    testLogger.LogError(new Exception("ExceptionMessage"), "LoggerMessage");
                }
            }

        public void ApplicationInsightsLoggerInstrumentationKeyIsSetCorrectly()
        {
            // Create DI container.
            IServiceCollection services = new ServiceCollection();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddApplicationInsights("TestAIKey");
            });

        [TestMethod]
        [TestCategory("ILogger")]
        public void ApplicationInsightsLoggerTelemetryConfigurationIsSetCorrectly()
        {
            // Create DI container.
            IServiceCollection services = new ServiceCollection();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddApplicationInsights(

            Assert.AreEqual("TestAIKey", actualTelemetryConfiguration.InstrumentationKey);
        }

        /// <summary>
        /// Ensures that the default <see cref="ApplicationInsightsLoggerOptions"/> are as expected.
        /// </summary>
        [TestMethod]
        [TestCategory("ILogger")]
        public void DefaultLoggerOptionsAreCorrectlyRegistered()
        {
            IServiceProvider serviceProvider = ILoggerIntegrationTests.SetupApplicationInsightsLoggerIntegration(
                (telemetryItem, telemetryProcessor) => { });

            IOptions<ApplicationInsightsLoggerOptions> registeredOptions =
                serviceProvider.GetRequiredService<IOptions<ApplicationInsightsLoggerOptions>>();

            Assert.IsTrue(registeredOptions.Value.TrackExceptionsAsExceptionTelemetry);
            Assert.IsTrue(registeredOptions.Value.IncludeScopes);
        }

        [TestMethod]
        [TestCategory("ILogger")]
        public void TelemetryChannelIsFlushedWhenServiceProviderIsDisposed()
        {
            TestTelemetryChannel testTelemetryChannel = new TestTelemetryChannel();

            using (ServiceProvider serviceProvider = ILoggerIntegrationTests.SetupApplicationInsightsLoggerIntegration(
                delegate { },
                telemetryConfiguration => telemetryConfiguration.TelemetryChannel = testTelemetryChannel))
        }

        [TestMethod]
        [TestCategory("ILogger")]
        public void TelemetryChannelIsNotFlushedWhenFlushOnDisposeIsFalse()
        {
            TestTelemetryChannel testTelemetryChannel = new TestTelemetryChannel();

            using (ServiceProvider serviceProvider = ILoggerIntegrationTests.SetupApplicationInsightsLoggerIntegration(
                delegate { },
            Action<ITelemetry, ITelemetryProcessor> telemetryActionCallback,
            Action<TelemetryConfiguration> configureTelemetryConfiguration = null,
            Action<ApplicationInsightsLoggerOptions> configureApplicationInsightsOptions = null,
            Func<IServiceCollection, IServiceCollection> configureServices = null)
        {
            // Create DI container.
            IServiceCollection services = new ServiceCollection();

            // Configure the Telemetry configuration to be used to send data to AI.
            services.Configure<TelemetryConfiguration>(telemetryConfiguration =>
                }

                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
            });

            if (configureServices != null)
            {
                services = configureServices.Invoke(services);
            }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
