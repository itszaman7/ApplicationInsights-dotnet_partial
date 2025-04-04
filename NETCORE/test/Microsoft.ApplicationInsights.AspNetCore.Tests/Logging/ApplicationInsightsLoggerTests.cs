﻿using System;
using Microsoft.ApplicationInsights.AspNetCore.Logging;
using Microsoft.ApplicationInsights.AspNetCore.Tests.Helpers;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Microsoft.ApplicationInsights.AspNetCore.Tests.Logging
{
#pragma warning disable CS0618 // ApplicationInsightsLoggerOptions is obsolete. Are we ready to delete these tests?
    /// <summary>
    /// Tests for the Application Insights ILogger implementation.
    /// </summary>
    public class ApplicationInsightsLoggerTests
    {
        private static readonly ApplicationInsightsLoggerOptions defaultOptions = new ApplicationInsightsLoggerOptions();

        /// <summary>
        /// Tests that the SDK version is correctly set on the telemetry context when messages are logged to AI.
        /// </summary>
        [Fact]
        public void TestLoggerSetsSdkVersionOnLoggedTelemetry()
        {
            bool isCorrectVersion = false;
            TelemetryClient client = CommonMocks.MockTelemetryClient((t) =>
            {
                isCorrectVersion = t.Context.GetInternalContext().SdkVersion.StartsWith(ApplicationInsightsLogger.VersionPrefix);
            });

            ILogger logger = new ApplicationInsightsLogger("test", client, (s, l) => { return true; }, null);
            logger.LogTrace("This is a test.", new object[] { });
            Assert.True(isCorrectVersion);
        }

        /// <summary>
        /// Tests that logging an information results in tracking an <see cref="TraceTelemetry"/> instance.
        /// </summary>
        [Fact]
        public void TestLoggerCreatesTraceTelemetryOnLoggedInfo()
        {
            TelemetryClient client = CommonMocks.MockTelemetryClient((t) =>
            {
                Assert.IsType<TraceTelemetry>(t);
                var traceTelemetry = (TraceTelemetry)t;
                Assert.Equal("This is an information", traceTelemetry.Message);
                Assert.Equal(SeverityLevel.Information, traceTelemetry.SeverityLevel);
                Assert.Equal("test-category", traceTelemetry.Properties["CategoryName"]);
            });

            ILogger logger = new ApplicationInsightsLogger("test-category", client, (s, l) => { return true; }, null);
            logger.LogInformation(0, "This is an information");
        }

        /// <summary>
        /// Tests that logging a warning with parameters results in tracking an <see cref="TraceTelemetry"/> instance with telemetry properties.
        /// </summary>
        [Fact]
        public void TestLoggerCreatesTraceTelemetryOnLoggedWarningWithParameters()
        {
            TelemetryClient client = CommonMocks.MockTelemetryClient((t) =>
            {
                Assert.IsType<TraceTelemetry>(t);
                var traceTelemetry = (TraceTelemetry)t;
                Assert.Equal("This is 123 value: Hello, World!", traceTelemetry.Message);
                Assert.Equal(SeverityLevel.Warning, traceTelemetry.SeverityLevel);
                Assert.Equal("test-category", traceTelemetry.Properties["CategoryName"]);

                Assert.Equal("123", traceTelemetry.Properties["testParam"]);
                Assert.Equal("Hello, World!", traceTelemetry.Properties["param2"]);
            });

            ILogger logger = new ApplicationInsightsLogger("test-category", client, (s, l) => { return true; }, null);
            logger.LogWarning(0, "This is {testParam} value: {param2}", 123, "Hello, World!");
        }
        [Fact]
        public void TestLoggerCreatesTraceTelemetryWithoutEventIdOnLoggedDebug()
        {
            TelemetryClient client = CommonMocks.MockTelemetryClient((t) =>
            {
                Assert.IsType<TraceTelemetry>(t);
                var traceTelemetry = (TraceTelemetry)t;
                Assert.Equal("This is an information", traceTelemetry.Message);
                Assert.Equal(SeverityLevel.Verbose, traceTelemetry.SeverityLevel);
                Assert.Equal("test-category", traceTelemetry.Properties["CategoryName"]);

                Assert.False(traceTelemetry.Properties.ContainsKey("EventId"));
                Assert.False(traceTelemetry.Properties.ContainsKey("EventName"));
            });

            ILogger logger = new ApplicationInsightsLogger("test-category", client, (s, l) => { return true; }, null);
            logger.LogDebug(new EventId(22, "TestEvent"), "This is an information");
        }

        /// <summary>
        [Fact]
        public void TestLoggerIncludingEventIdCreatesTraceTelemetryWithEventIdOnLoggedError()
        {
            TelemetryClient client = CommonMocks.MockTelemetryClient((t) =>
            {
                Assert.IsType<TraceTelemetry>(t);
                var traceTelemetry = (TraceTelemetry)t;
                Assert.Equal("This is an error", traceTelemetry.Message);
                Assert.Equal(SeverityLevel.Error, traceTelemetry.SeverityLevel);
                Assert.Equal("test-category", traceTelemetry.Properties["CategoryName"]);
            {
                Assert.IsType<TraceTelemetry>(t);
                var traceTelemetry = (TraceTelemetry)t;
                Assert.Equal("This is an information", traceTelemetry.Message);
                Assert.Equal(SeverityLevel.Information, traceTelemetry.SeverityLevel);
                Assert.Equal("test-category", traceTelemetry.Properties["CategoryName"]);

                Assert.False(traceTelemetry.Properties.ContainsKey("EventId"));
                Assert.False(traceTelemetry.Properties.ContainsKey("EventName"));
            });
        /// <summary>
        /// Tests that logging an information with emtpy event name results in tracking an <see cref="TraceTelemetry"/> instance.
        /// </summary>
        [Fact]
        public void TestLoggerIncludingEventIdCreatesTraceTelemetryWithoutEventNameOnLoggedInfo()
        {
            TelemetryClient client = CommonMocks.MockTelemetryClient((t) =>
            {
                Assert.IsType<TraceTelemetry>(t);
                var traceTelemetry = (TraceTelemetry)t;
        /// Tests that logging an exception results in tracking an <see cref="ExceptionTelemetry"/> instance.
        /// </summary>
        [Fact]
        public void TestLoggerCreatesExceptionTelemetryOnLoggedError()
        {
            TelemetryClient client = CommonMocks.MockTelemetryClient((t) =>
            {
                Assert.IsType<ExceptionTelemetry>(t);
                var exceptionTelemetry = (ExceptionTelemetry)t;
                Assert.Equal(SeverityLevel.Error, exceptionTelemetry.SeverityLevel);
                Assert.Equal("test-category", exceptionTelemetry.Properties["CategoryName"]);
                Assert.Equal("System.Exception: This is an error", exceptionTelemetry.Properties["Exception"]);
                Assert.Equal("Error: This is an error", exceptionTelemetry.Message);
            });

            ILogger logger = new ApplicationInsightsLogger("test-category", client, (s, l) => { return true; }, defaultOptions);
            var exception = new Exception("This is an error");
            logger.LogError(0, exception, "Error: " + exception.Message);
        }

                Assert.Equal(SeverityLevel.Error, exceptionTelemetry.SeverityLevel);
                Assert.Equal("test-category", exceptionTelemetry.Properties["CategoryName"]);
                Assert.Equal("System.Exception: This is an error", exceptionTelemetry.Properties["Exception"]);
                Assert.Equal("Error: This is an error, Value: 123", exceptionTelemetry.Message);

                Assert.Equal("This is an error", exceptionTelemetry.Properties["ex"]);
                Assert.Equal("123", exceptionTelemetry.Properties["value"]);
            });

            ILogger logger = new ApplicationInsightsLogger("test-category", client, (s, l) => { return true; }, defaultOptions);
                Assert.Equal(SeverityLevel.Error, exceptionTelemetry.SeverityLevel);
                Assert.Equal("test-category", exceptionTelemetry.Properties["CategoryName"]);
                Assert.Equal("System.Exception: This is an error", exceptionTelemetry.Properties["Exception"]);
                Assert.Equal("Error: This is an error", exceptionTelemetry.Message);

                Assert.False(exceptionTelemetry.Properties.ContainsKey("EventId"));
                Assert.False(exceptionTelemetry.Properties.ContainsKey("EventName"));
            });

            ILogger logger = new ApplicationInsightsLogger("test-category", client, (s, l) => { return true; }, defaultOptions);

                Assert.Equal("22", exceptionTelemetry.Properties["EventId"]);
                Assert.Equal("TestEvent", exceptionTelemetry.Properties["EventName"]);
            });
            var options = new ApplicationInsightsLoggerOptions { IncludeEventId = true };

            ILogger logger = new ApplicationInsightsLogger("test-category", client, (s, l) => { return true; }, options);
            var exception = new Exception("This is an error");
            logger.LogError(new EventId(22, "TestEvent"), exception, "Error: " + exception.Message);
        }
            });
            var options = new ApplicationInsightsLoggerOptions { IncludeEventId = true };

            ILogger logger = new ApplicationInsightsLogger("test-category", client, (s, l) => { return true; }, options);
            var exception = new Exception("This is an error");
            logger.LogError(0, exception, "Error: " + exception.Message);
        }

        /// <summary>
        /// Tests that logging an exception with empty event name results in tracking an <see cref="ExceptionTelemetry"/> instance, when includeEventId is <c>true</c>.
        /// </summary>
        [Fact]
        public void TestLoggerIncludingEventIdCreatesExceptionWithoutEventNameTelemetryOnLoggedError()
        {
            TelemetryClient client = CommonMocks.MockTelemetryClient((t) =>
            {
                Assert.IsType<ExceptionTelemetry>(t);
                var exceptionTelemetry = (ExceptionTelemetry)t;
                Assert.Equal(SeverityLevel.Error, exceptionTelemetry.SeverityLevel);
                Assert.Equal("test-category", exceptionTelemetry.Properties["CategoryName"]);
                Assert.Equal("System.Exception: This is an error", exceptionTelemetry.Properties["Exception"]);
                Assert.Equal("Error: This is an error", exceptionTelemetry.Message);

                Assert.Equal("100", exceptionTelemetry.Properties["EventId"]);
                Assert.False(exceptionTelemetry.Properties.ContainsKey("EventName"));
            });
            var options = new ApplicationInsightsLoggerOptions { IncludeEventId = true };

            ILogger logger = new ApplicationInsightsLogger("test-category", client, (s, l) => { return true; }, options);
            var exception = new Exception("This is an error");
            logger.LogError(new EventId(100, string.Empty), exception, "Error: " + exception.Message);
        }

        /// <summary>
        /// Tests that logging an exception with <see cref="EventId"/> results in tracking an <see cref="ExceptionTelemetry"/> instance,
        /// when includeEventId is <c>true</c> and parameter is EventName.
        /// </summary>
        [Fact]
        public void TestLoggerIncludingEventIdCreatesExceptionWithEventIdTelemetryOnLoggedErrorWithParameters()
        {
                Assert.Equal("Error: ERROR This is an error", exceptionTelemetry.Message);

            logger.LogError(new EventId(22, "TestEvent"), exception, "Error: {EventName} {Message}", "ERROR", exception.Message);
        }

        /// <summary>
        /// Tests that logging an exception results in not tracking an <see cref="ExceptionTelemetry"/> instance, when filter returns <c>false</c>.
        /// </summary>
        [Fact]
        public void TestLoggerWithNegativeFilterDoesNotCreateExceptionTelemetryOnLoggedError()
        {
            bool trackedTelemetry = false;

        [Fact]
        public void TestLoggerCreatesExceptionTelemetryOnLoggedErrorWhenTrackExceptionsAsExceptionTelemetryIsSetToTrue()
        {
            TelemetryClient client = CommonMocks.MockTelemetryClient((t) =>
            {
                Assert.IsType<ExceptionTelemetry>(t);
                var exceptionTelemetry = (ExceptionTelemetry)t;

                Assert.Equal("Error: This is an error", exceptionTelemetry.Message);
            var exception = new Exception("This is an error");

            logger.LogError(0, exception, "Error: " + exception.Message);
        }

        /// <summary>
        /// Tests that logging an exception with parameters results in tracking an <see cref="ExceptionTelemetry"/> instance with parameters.
        /// </summary>
        [Fact]
        public void TestLoggerAdditionalExceptionDataIsAddedToTelemetry()
        }

        /// <summary>
        /// Tests that an incorrectly constructed or uninitialized Application Insights ILogger does not throw exceptions.
        /// </summary>
        [Fact]
        public void TestUninitializedLoggerDoesNotThrowExceptions()
        {
            ILogger logger = new ApplicationInsightsLogger("test", null, null, null);
            logger.LogTrace("This won't do anything.", new object[] { });


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
