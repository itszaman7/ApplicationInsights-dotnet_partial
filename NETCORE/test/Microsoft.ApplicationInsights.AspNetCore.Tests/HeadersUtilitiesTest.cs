namespace Microsoft.ApplicationInsights.AspNetCore.Tests
{
    using System.Collections.Generic;
    using DiagnosticListeners;
    using Xunit;

    public class HeadersUtilitiesTest
    {
        public void ShouldReturnHeaderValueWhenKeyExists()
        {
            List<string> headers = new List<string>() {
                "Key = Value"
            };
        [Fact]
        public void ShouldReturnHeadersWhenNoHeaderValues()
        {
            string[] newHeaders = HeadersUtilities.SetHeaderKeyValue(null, "Key", "Value");

            Assert.Equal("Key=Value", newHeaders[0]);
        public void ShouldAppendHeaders()
        {
            string[] existing = new string[] { "ExistKey=ExistValue" };
            string[] result = HeadersUtilities.SetHeaderKeyValue(existing, "NewKey", "NewValue");

            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            Assert.Equal("ExistKey=ExistValue", result[0]);
            Assert.Equal("NewKey=NewValue", result[1]);
        }

            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            Assert.Equal("ExistKey=NewValue", result[0]);
            Assert.Equal("NoiseKey=NoiseValue", result[1]);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
