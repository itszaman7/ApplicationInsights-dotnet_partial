﻿#if NET452
namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Text;
    using Microsoft.ApplicationInsights.Common;
    using VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HeaderCollectionManipulationTests
    {
        /// <summary>
        /// Ensures that the GetNameValueHeaderValue extension methods works as expected.
        /// </summary>
        [TestMethod]
        public void GetNameValueHeaderWorksCorrectly()
        {
            WebHeaderCollection headers = new WebHeaderCollection();

            // header collection empty
            Assert.IsNull(headers.GetNameValueHeaderValue("someName", "someKey"));

            headers.Add("header-one", "value1");
            headers.Add("header-two", "value2");

            // header not found
            Assert.IsNull(headers.GetNameValueHeaderValue("myheader", "key1"));

            // header key not found
            Assert.IsNull(headers.GetNameValueHeaderValue("header-two", "key1"));

            // header should be found. We test cases were there are spaces around delimeters.
            headers.Add("headerThree", "key1=value1, key2=value2");
            headers.Add("Header-Four", "key1=value1,key2=value2,key3=value3");
            headers.Add("Header-Five", "key1=value1,key2 = value2,key3=value3");

            Assert.AreEqual("value2", headers.GetNameValueHeaderValue("headerThree", "key2"));
            Assert.AreEqual("value2", headers.GetNameValueHeaderValue("Header-Four", "key2"));
            Assert.AreEqual("value2", headers.GetNameValueHeaderValue("Header-Five", "key2"));

            // header with key value format but missing key
            Assert.IsNull(headers.GetNameValueHeaderValue("headerThree", "keyX"));
        }

        /// <summary>
        /// Ensures that the SetNameValueHeaderValue works as expected.
        /// </summary>
        [TestMethod]
        public void SetNameValueHeaderWorksCorrectly()
        {
            // Collection empty
            WebHeaderCollection headers = new WebHeaderCollection();

            headers.SetNameValueHeaderValue("Request-Context", "appId", "appIdValue");
            Assert.AreEqual(1, headers.Keys.Count);
            Assert.AreEqual("appId=appIdValue", headers["Request-Context"]);

            // Non empty collection - adding new key
            headers.SetNameValueHeaderValue("Request-Context", "roleName", "workerRole");
            Assert.AreEqual(1, headers.Keys.Count);
            Assert.AreEqual("appId=appIdValue,roleName=workerRole", headers["Request-Context"]);

            // overwritting existing key
            headers.SetNameValueHeaderValue("Request-Context", "roleName", "webRole");
            headers.SetNameValueHeaderValue("Request-Context", "appId", "udpatedAppId");
            Assert.AreEqual(1, headers.Keys.Count);
            Assert.AreEqual("roleName=webRole,appId=udpatedAppId", headers["Request-Context"]);
        }

        /// <summary>
        /// Tests that GetCollectionFromHeaderNoDuplicates gets collection of key-value pairs from existing, non-empty header.
        /// </summary>
        [TestMethod]
        public void GetCollectionFromHeaderNoDuplicates()
        {
            WebHeaderCollection headers = new WebHeaderCollection();
        public void GetCollectionFromEmptyHeader()
        {
            WebHeaderCollection headers = new WebHeaderCollection();
            Assert.IsNull(headers.GetNameValueCollectionFromHeader("Correlation-Context"));

            headers["Correlation-Context"] = "   ";
            var correlationContext = headers.GetNameValueCollectionFromHeader("Correlation-Context");
            Assert.IsTrue(correlationContext == null || !correlationContext.Any());
        }

            Assert.AreEqual("k2=v2", values.Last());
        }

        [TestMethod]
        public void GetHeaderValueMaxLenTruncatesEnd_Test1()
        {
            this.GetHeaderValueMaxLenTruncatesEnd("k1=v1,k2=v2,".Length);
        }

        [TestMethod]
        {
            this.GetHeaderValueMaxLenTruncatesEndInvalid(0);
        }

        [TestMethod]
        public void GetHeaderValueMaxLenTruncatesEndInvalid_Test2()
        {
            this.GetHeaderValueMaxLenTruncatesEndInvalid(3);
        }

            Assert.AreEqual("k1=v1", values.First());
            Assert.AreEqual("k2=v2", values.Last());
        }

        [TestMethod]
        public void ReadCorrelationContextBasicParsing_Test1()
        {
            this.ReadCorrelationContextBasicParsing("k1=v1,k2=v2");
        }

        [TestMethod]
        public void ReadCorrelationContextBasicParsing_Test2()
        {
            this.ReadCorrelationContextBasicParsing(" k1= v1 , k2 =v2 ,");
        }

        [TestMethod]
        public void ReadCorrelationContextBasicParsing_Test3()
        {
            this.ReadCorrelationContextBasicParsing(", , k1=v1,,k2=v2,,");
        }

        [TestMethod]
        public void ReadCorrelationContextBasicParsing_Test4()
        {
            this.ReadCorrelationContextBasicParsing(",123, k1=v1,,k2=v2,, 456");
        }

        [TestMethod]
        public void ReadCorrelationContextBasicParsing_Test5()
        {
            this.ReadCorrelationContextBasicParsing("123=,k1=v1,k2=v2,=456");
        }

        [TestMethod]
        public void ReadCorrelationContextBasicParsing_Test6()
        {
            this.ReadCorrelationContextBasicParsing("123= ,k1=v1,k2=v2, =456");
        }

        [TestMethod]
        public void ReadCorrelationContextBasicParsing_Test7()
        {
            this.ReadCorrelationContextBasicParsing("k1=v1,k2=v2,k3==v3,k4=1=2");
        }

        [TestMethod]
        public void ReadCorrelationContextBasicParsingGarbage_Test1()
        {
            this.ReadCorrelationContextBasicParsingGarbage(string.Empty);

        [TestMethod]
        public void ReadCorrelationContextBasicParsingGarbage_Test2()
        {
            this.ReadCorrelationContextBasicParsingGarbage(null);
        }

        [TestMethod]
        public void ReadCorrelationContextBasicParsingGarbage_Test3()
        {
            this.ReadCorrelationContextBasicParsingGarbage(", , ,,");
        }

        [TestMethod]
        public void ReadCorrelationContextBasicParsingGarbage_Test4()
        {
            this.ReadCorrelationContextBasicParsingGarbage(",123,    , 456");
        }

        [TestMethod]
        public void ReadCorrelationContextBasicParsingGarbage_Test5()
        {
            this.ReadCorrelationContextBasicParsingGarbage("=,=,");
        }

        [TestMethod]
        public void ReadCorrelationContextBasicParsingGarbage_Test6()
        {
            this.ReadCorrelationContextBasicParsingGarbage("==,");
        }
                { RequestResponseHeaders.CorrelationContextHeader, "k2=v2" }
            };

            var activity = new Activity("foo");
            headers.ReadActivityBaggage(activity);

            var baggage = activity.Baggage.ToArray();
            Assert.AreEqual(2, baggage.Length);
            Assert.IsNotNull(baggage.SingleOrDefault(i => i.Key == "k1" && i.Value == "v1"));
            Assert.IsNotNull(baggage.SingleOrDefault(i => i.Key == "k2" && i.Value == "v2"));
            }
        }

        [TestMethod]
        public void ReadCorrelationContextMultiHeaderTooLong()
        {
            var pairs = new List<KeyValuePair<string, string>>();
            var header1 = new StringBuilder();
            while (header1.Length < 8140)
            {

            NameValueCollection headers = new NameValueCollection
            {
                { RequestResponseHeaders.CorrelationContextHeader, header1.ToString() },
                { RequestResponseHeaders.CorrelationContextHeader, header2 }
            };

            var activity = new Activity("foo");
            headers.ReadActivityBaggage(activity);

        {
            var pairs = new KeyValuePair<string, string>[181];
            var header = new StringBuilder();
            for (int i = 0; i < pairs.Length - 1; i++)
            {
                var pair = new KeyValuePair<string, string>(i.ToString(), i.ToString());

                // last pair should be ignored
                pairs[i] = pair;
                header.Append($"{pair.Key}={pair.Value},");
            headers.ReadActivityBaggage(activity);

            var baggage = activity.Baggage.ToArray();
            Assert.AreEqual(180, baggage.Length);
            for (int i = 0; i < 180; i++)
            {
                Assert.IsNotNull(baggage.SingleOrDefault(kvp => kvp.Key == pairs[i].Key && kvp.Value == pairs[i].Value));
            }
        }

                // last pair should be ignored
                pairs[i] = pair;
                header.Append($"{pair.Key}={pair.Value},");
            }

            NameValueCollection headers = new NameValueCollection
            {
                [RequestResponseHeaders.CorrelationContextHeader] = header.ToString()
            };

            headers.ReadActivityBaggage(activity);

            var baggage = activity.Baggage.ToArray();
            Assert.AreEqual(180, baggage.Length);
            for (int i = 0; i < 180; i++)
            {
                Assert.IsNotNull(baggage.SingleOrDefault(kvp => kvp.Key == pairs[i].Key && kvp.Value == pairs[i].Value));
            }
        }

        public void ReadCorrelationContextTooLongOneItem()
        {
            NameValueCollection headers = new NameValueCollection
            {
                [RequestResponseHeaders.CorrelationContextHeader] = new string('x', 8190) + '=' + "123"
            };

            var activity = new Activity("foo");
            headers.ReadActivityBaggage(activity);

            WebHeaderCollection headers = new WebHeaderCollection { [W3C.W3CConstants.TraceStateHeader] = "k1=v1,k2=v2,k3=v3,k4=v4" };
            var values = headers.GetHeaderValue(W3C.W3CConstants.TraceStateHeader, maxLength)?.ToList();
            Assert.IsNotNull(values);
            Assert.AreEqual(2, values.Count);
            Assert.AreEqual("k1=v1", values.First());
            Assert.AreEqual("k2=v2", values.Last());
        }

        private void GetHeaderValueMaxLenTruncatesEndInvalid(int maxLength)
        {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
