namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.ApplicationInsights.TestFramework;

    [TestClass]
    public class JsonSerializationWriterTest
    {
        [TestMethod]
        public void ClassIsInternalAndNotMeantToBeAccessedByCustomers()
        {
            Assert.IsFalse(typeof(JsonSerializationWriter).GetTypeInfo().IsPublic);
        }

        [TestMethod]
        public void WriteStartArrayWritesOpeningSquareBracket()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteStartArray();
                Assert.AreEqual("[", stringWriter.ToString());
            }
        }

        [TestMethod]
        public void WriteStartObjectWritesOpeningCurlyBrace()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteStartObject();
                Assert.AreEqual("{", stringWriter.ToString());
            }
        }

        [TestMethod]
        public void WriteEndArrayWritesClosingSquareBracket()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteEndArray();
                Assert.AreEqual("]", stringWriter.ToString());
            }
        }

        [TestMethod]
        public void WriteEndObjectWritesClosingCurlyBrace()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteEndObject();
                Assert.AreEqual("}", stringWriter.ToString());
            }        
        }

        [TestMethod]
        public void WriteRawValueWritesValueWithoutEscapingValue()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteRawValue(@"Test\Name");
                Assert.AreEqual(@"Test\Name", stringWriter.ToString());
            }
        }

        #region WriteProperty(string, int?)

        [TestMethod]
        public void WritePropertyIntWritesIntValueWithoutQuotationMark()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                const string Name = "name";
                const int Value = 42;
                new JsonSerializationWriter(stringWriter).WriteProperty(Name, Value);
                Assert.AreEqual("\"" + Name + "\":" + Value, stringWriter.ToString());
            }
        }

            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteProperty("name", (int?)null);
                Assert.AreEqual(string.Empty, stringWriter.ToString());
            }
        }

        #endregion

        #region WriteProperty(string, double?)
                new JsonSerializationWriter(stringWriter).WriteProperty("name", (string)null);
                Assert.AreEqual(string.Empty, stringWriter.ToString());
            }
        }

        [TestMethod]
        public void WritePropertyStringDoesNothingIfValueIsEmptyBecauseItAssumesPropertyIsOptional()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {

        [TestMethod]
        public void WritePropertyBooleanWritesValueWithoutQuotationMarks()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                const string Name = "name";
                const bool Value = true;
                new JsonSerializationWriter(stringWriter).WriteProperty(Name, Value);
                string expectedValue = Value.ToString().ToLowerInvariant();
                Assert.AreEqual("\"" + Name + "\":" + expectedValue, stringWriter.ToString());
            }
        }

        [TestMethod]
        public void WritePropertyBooleanDoesNothingIfValueIsNullBecauseItAssumesPropertyIsOptional()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteProperty("name", (bool?)null);
                Assert.AreEqual("\"" + Name + "\":" + expectedValue, stringWriter.ToString());
            }
        }

        #endregion

        #region WriteProperty(string, DateTimeOffset?)

        [TestMethod]
        public void WritePropertyDateTimeOffsetWritesValueInQuotationMarksAndRoundTripDateTimeFormat()
                DateTimeOffset value = DateTimeOffset.UtcNow;
                new JsonSerializationWriter(stringWriter).WriteProperty(Name, value);
                string expectedValue = value.ToString("o", CultureInfo.InvariantCulture);
                Assert.AreEqual("\"" + Name + "\":\"" + expectedValue + "\"", stringWriter.ToString());
            }
        }

        [TestMethod]
        public void WritePropertyDateTimeOffsetDoesNothingIfValueIsNullBecauseItAssumesPropertyIsOptional()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteProperty("name", (DateTimeOffset?)null);
                Assert.AreEqual(string.Empty, stringWriter.ToString());
            }
        }

        #endregion

        #region WriteProperty(string, IDictionary<string, double>)
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var writer = new JsonSerializationWriter(stringWriter);
                writer.WriteProperty("name", new Dictionary<string, double> { { "key1", 1 } });
                AssertEx.Contains("\"key1\":1", stringWriter.ToString(), StringComparison.OrdinalIgnoreCase);
            }
        }

        [TestMethod]

        [TestMethod]
        public void WritePropertyIDictionaryDoubleDoesNothingWhenDictionaryIsEmptyBecauseItAssumesPropertyIsOptional()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteProperty("name", new Dictionary<string, double>());
                Assert.AreEqual(string.Empty, stringWriter.ToString());
            }
        }
                AssertEx.EndsWith("}", stringWriter.ToString(), StringComparison.OrdinalIgnoreCase);
            }
        }

        [TestMethod]
        public void WritePropertyIDictionaryStringStringWritesValuesWithoutDoubleQuotes()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var writer = new JsonSerializationWriter(stringWriter);
        }

        [TestMethod]
        public void WritePropertyIDictionaryStringStringDoesNothingWhenDictionaryIsNullBecauseItAssumesPropertyIsOptional()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteProperty("name", (IDictionary<string, string>)null);
                Assert.AreEqual(string.Empty, stringWriter.ToString());
            }
        }

        [TestMethod]
        public void WritePropertyIDictionaryStringStringDoesNothingWhenDictionaryIsEmptyBecauseItAssumesPropertyIsOptional()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteProperty("name", new Dictionary<string, string>());
                Assert.AreEqual(string.Empty, stringWriter.ToString());
            }
        }

        #endregion

        #region WritePropertyName

        [TestMethod]
        public void WritePropertyNameWritesPropertyNameEnclosedInDoubleQuotationMarksFollowedByColon()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
        [TestMethod]
        public void WritePropertyNameDoesNotPrependPropertyNameWithComaWhenNewObjectWasStarted()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var JsonSerializationWriter = new TestableJsonSerializationWriter(stringWriter);
                JsonSerializationWriter.WritePropertyName("Property1");
                JsonSerializationWriter.WriteStartObject();
                JsonSerializationWriter.WritePropertyName("Property2");
                AssertEx.Contains("{\"Property2\"", stringWriter.ToString(), StringComparison.OrdinalIgnoreCase);
            }
        }

        #endregion

        #region WriteString

        [TestMethod]
        public void WriteStringEscapesQuotationMark()
        {
                JsonSerializationWriter.WriteString("Test\bValue");
                AssertEx.Contains("Test\\bValue", stringWriter.ToString(), StringComparison.OrdinalIgnoreCase);
            }            
        }

        [TestMethod]
        public void WriteStringEscapesFormFeed()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                AssertEx.Contains("Test\\nValue", stringWriter.ToString(), StringComparison.OrdinalIgnoreCase);
            }
        }

        [TestMethod]
        public void WriteStringEscapesCarriageReturn()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var JsonSerializationWriter = new TestableJsonSerializationWriter(stringWriter);
                AssertEx.Contains("Test\\rValue", stringWriter.ToString(), StringComparison.OrdinalIgnoreCase);
            }
        }

        [TestMethod]
        public void WriteStringEscapesHorizontalTab()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var JsonSerializationWriter = new TestableJsonSerializationWriter(stringWriter);
                JsonSerializationWriter.WriteProperty("name", "Test\tValue");
                AssertEx.Contains("Test\\tValue", stringWriter.ToString(), StringComparison.OrdinalIgnoreCase);
            }
        }

        #endregion

        private class TestableJsonSerializationWriter : JsonSerializationWriter
        {
            public TestableJsonSerializationWriter(TextWriter textWriter)


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
