namespace Microsoft.Framework.DependencyInjection.Test
{
    using System.Security.Principal;
    using System.Text.Encodings.Web;
    using System.Text.Unicode;
    using Microsoft.ApplicationInsights.AspNetCore;
    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
    using Microsoft.ApplicationInsights.AspNetCore.Tests.Helpers;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Xunit;

    public static class ApplicationInsightsJavaScriptTest
    {
        private static JavaScriptEncoder encoder = JavaScriptEncoder.Create(new UnicodeRange[] { UnicodeRanges.BasicLatin });
        public static void SnippetWillBeEmptyWhenInstrumentationKeyIsNotDefined()
        {
            var telemetryConfigurationWithNullKey = new TelemetryConfiguration();
            var snippet = new JavaScriptSnippet(telemetryConfigurationWithNullKey, GetOptions(false), null, encoder);
            Assert.Equal(string.Empty, snippet.FullScript);
        }

        [Fact]
        public static void SnippetWillBeEmptyWhenInstrumentationKeyIsEmpty()
        {
        }

        [Fact]
        [Trait("Trait", "ConnectionString")]
        public static void SnippetWillIncludeConnectionStringAsSubstring()
        {
            string testConnString = "InstrumentationKey=00000000-0000-0000-0000-000000000000";
            var telemetryConfiguration = new TelemetryConfiguration { ConnectionString = testConnString };
            var snippet = new JavaScriptSnippet(telemetryConfiguration, GetOptions(false), null, encoder);
            Assert.Contains("connectionString: '" + testConnString + "'", snippet.FullScript);
        public static void SnippetWillNotIncludeAuthUserNameIfEnabledAndAuthenticated()
        {
            string unittestkey = "unittestkey";
            var telemetryConfiguration = new TelemetryConfiguration { InstrumentationKey = unittestkey };
            var snippet = new JavaScriptSnippet(telemetryConfiguration, GetOptions(true), GetHttpContextAccessor("username", false), encoder);
            Assert.DoesNotContain("appInsights.setAuthenticatedUserContext(", snippet.FullScript);
        }

        [Fact]
        public static void SnippetWillIncludeEscapedAuthUserNameIfEnabledAndAuthenticated()
        {
            string unittestkey = "unittestkey";
            var telemetryConfiguration = new TelemetryConfiguration { InstrumentationKey = unittestkey };
            var snippet = new JavaScriptSnippet(telemetryConfiguration, GetOptions(true), GetHttpContextAccessor("user\\name", true), encoder);
            Assert.Contains("setAuthenticatedUserContext(\"user\\\\name\")", snippet.FullScript);
        }

        [Fact]
        public static void CrossSiteScriptingIsBlockedByEncoding()
        {
            });
        }

        private static IHttpContextAccessor GetHttpContextAccessor(string name, bool isAuthenticated)
        {
            return new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
        /// </summary>
        private class IdentityStub : IIdentity
        {
            /// <inheritdoc />
            public string AuthenticationType { get; set; }

            /// <inheritdoc />
            public bool IsAuthenticated { get; set; }

            /// <inheritdoc />


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
