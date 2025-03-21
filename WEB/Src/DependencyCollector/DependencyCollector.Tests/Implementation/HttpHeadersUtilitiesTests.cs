namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the HttpHeadersUtilities class.
    /// </summary>
    [TestClass]
    public class HttpHeadersUtilitiesTests
    {
        /// <summary>
        /// Ensure that GetHeaderValues() returns an empty IEnumerable when the headers argument is null.
        /// </summary>
        [TestMethod]
        public void GetHeaderValuesWithNullHeaders()
        {
            EnumerableAssert.AreEqual(Enumerable.Empty<string>(), HttpHeadersUtilities.GetHeaderValues(null, "MOCK_HEADER_NAME"));
        }

        /// <summary>
        /// Ensure that GetHeaderValues() returns an empty IEnumerable when the headers argument is empty.
        /// </summary>
        [TestMethod]
        public void GetHeaderValuesWithEmptyHeaders()
        {
            headers.Add("MOCK_HEADER_NAME", "A");
            headers.Add("MOCK_HEADER_NAME", "B");
            headers.Add("MOCK_HEADER_NAME", "C");
            EnumerableAssert.AreEqual(new[] { "A", "B", "C" }, HttpHeadersUtilities.GetHeaderValues(headers, "MOCK_HEADER_NAME"));
        }

        /// <summary>
        /// Ensure that GetHeaderKeyValue() returns null when the headers argument is null.
        /// </summary>
        [TestMethod]
        public void GetHeaderKeyValuesWithNullHeaders()
        {
            Assert.AreEqual(null, HttpHeadersUtilities.GetHeaderKeyValue(null, "HEADER_NAME", "KEY_NAME"));
        }

        /// <summary>
        /// Ensure that GetHeaderKeyValue() returns null when the headers argument is empty.
        /// </summary>
        [TestMethod]
        public void GetHeaderKeyValuesWithEmptyHeaders()
        {
            HttpHeaders headers = CreateHeaders();
            Assert.AreEqual(null, HttpHeadersUtilities.GetHeaderKeyValue(headers, "HEADER_NAME", "KEY_NAME"));
        }

        /// <summary>
        /// Ensure that GetHeaderKeyValue() returns key value when the headers argument contains header key name.
        /// </summary>
        [TestMethod]
        public void GetHeaderKeyValuesWithMatchingHeader()
        {
            HttpHeaders headers = CreateHeaders();
            headers.Add("HEADER_NAME", "KEY_NAME=KEY_VALUE");
            Assert.AreEqual("KEY_VALUE", HttpHeadersUtilities.GetHeaderKeyValue(headers, "HEADER_NAME", "KEY_NAME"));
        }

        /// <summary>
        /// Ensure that GetHeaderKeyValue() returns first key value when the headers argument contains multiple key name/value pairs for header name.
        /// </summary>
        [TestMethod]
        /// <summary>
        /// Ensure that GetHeaderKeyValue() returns first key value when the headers argument contains multiple key values for the key name.
        /// </summary>
        [TestMethod]
        public void GetHeaderKeyValuesWithMultipleMatchingHeaderNamesAndMultipleMatchingKeyNames()
        {
            HttpHeaders headers = CreateHeaders();
            headers.Add("HEADER_NAME", "A=a");
            headers.Add("HEADER_NAME", "B=b");
            headers.Add("HEADER_NAME", "C=c1");
        /// </summary>
        [TestMethod]
        public void SetHeaderKeyValueWithMultipleMatchingHeaderNamesButOnlyOneMatchingKeyName()
            Assert.IsTrue(HttpHeadersUtilities.ContainsHeaderKeyValue(headers, "HEADER_NAME", "B"));
        }

        /// <summary>
        /// Ensure that SetHeaderKeyValue() overwrites all existing key values.
        /// </summary>
        [TestMethod]
        public void SetHeaderKeyValueWithMultipleMatchingHeaderNamesAndMultipleMatchingKeyNames()
        {
            HttpHeaders headers = CreateHeaders();
        /// <summary>
        /// Create a HttpHeaders object for testing.
        /// </summary>
        private static HttpHeaders CreateHeaders()
        {
            HttpHeaders result = new HttpRequestMessage().Headers;
            Assert.IsNotNull(result);
            return result;
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
