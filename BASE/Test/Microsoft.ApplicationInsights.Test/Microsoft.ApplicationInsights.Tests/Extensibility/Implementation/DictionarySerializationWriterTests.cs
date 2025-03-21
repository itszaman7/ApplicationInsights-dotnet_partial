namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DictionarySerializationWriterTests
    {
        [TestMethod]
        public void WritesStringProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("name", "value");

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", "value"}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesMultipleStringProperties()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("name", "value");
            dsw.WriteProperty("anotherName", "anotherValue");

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", "value"},
                { "anotherName", "anotherValue"}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesNullStringProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("name", (string)null);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", null}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]        
        public void ThrowsIfNameIsNullOrEmptyForStringProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(null, "value"));
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(string.Empty, "value"));
        }

        [TestMethod]
        public void WritesDoubleProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("name", 1.2);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", "1.2"}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesMultipleDoubleProperties()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("name", 1.2);
            dsw.WriteProperty("anotherName", 2.1);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", "1.2"},
                { "anotherName", "2.1"}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesNullDoubleProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("name", (double?)null);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", null}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]        
        public void ThrowsIfNameIsNullOrEmptyForDoubleProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(null, 1.2));
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(string.Empty, 1.2));
        }


        [TestMethod]
        public void WritesIntProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("name", 12);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", "12"}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesMultipleIntProperties()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("name", 12);
            dsw.WriteProperty("anotherName", 21);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", "12"},
                { "anotherName", "21"}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesNullIntProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("name", (int?)null);
            {
                { "name", null}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]        
        public void ThrowsIfNameIsNullOrEmptyForIntProperty()
        }

        [TestMethod]
        public void WritesNullBoolProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("name", (bool?)null);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {

        [TestMethod]        
        public void ThrowsIfNameIsNullOrEmptyForBoolProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(null, true));
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(string.Empty, false));
        }

        [TestMethod]
            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesMultipleTimespanProperties()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            TimeSpan value = new TimeSpan(1, 2, 3, 4, 5);
            TimeSpan anotherValue = new TimeSpan(5, 4, 3, 2, 1);

            dsw.WriteProperty("name", value);
            dsw.WriteProperty("anotherName", anotherValue);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", value.ToString()},
                { "anotherName", anotherValue.ToString()}
            };

        public void WritesTimeOffsetProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            DateTimeOffset value = new DateTimeOffset();

            dsw.WriteProperty("name", value);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", value.ToString(CultureInfo.InvariantCulture)}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }
        
        [TestMethod]
        public void WritesMultipleTimeOffsetProperties()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            DateTimeOffset? value = null;

            dsw.WriteProperty("name", value);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", null}
            };

            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            List<string> value = new List<string> { "value12", "value21" };
            List<string> anotherValue = new List<string> { "value34", "value43" };

            dsw.WriteProperty("name", value);
            dsw.WriteProperty("anotherName", anotherValue);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name0", "value12" },
        public void WritesListPropertyWithNullInTheList()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            List<string> value = new List<string> { "value12", null };

            dsw.WriteProperty("name", value);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name0", "value12" },
            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]        
        public void ThrowsIfNameIsNullOrEmptyForListProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            List<string> value = new List<string> { "value12", "value21" };

            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(null, value));
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(string.Empty, value));
        }

        [TestMethod]
        public void WritesDictionaryProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            Dictionary<string, string> value = new Dictionary<string, string>
            {
                { "name.key1", "value12" },
                { "name.key2", "value21" },
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesMultipleDictionaryProperties()

            Dictionary<string, string> anotherValue = new Dictionary<string, string>
            {
                { "key1", "value34" },
                { "key2", "value43" }
            };

            dsw.WriteProperty("name", value);
            dsw.WriteProperty("anotherName", anotherValue);


            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesNullDictionaryProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            Dictionary<string, string> value = null;

            dsw.WriteProperty("name", value);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", null }                
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            Dictionary<string, string> value = new Dictionary<string, string>
            {
                { "key1", null },
                { "key2", "value21" }
            };

            dsw.WriteProperty("name", value);

            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(null, value));
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(string.Empty, value));
        }

        [TestMethod]
        public void WritesISerializableProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            MySubSubExtension value = new MySubSubExtension { Field3 = "Value1", Field4 = 42.42 };

            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesMultipleISerializableProperties()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            MySubSubExtension value = new MySubSubExtension { Field3 = "Value1", Field4 = 42.42 };
            MySubSubExtension anotherValue = new MySubSubExtension { Field3 = "Value2", Field4 = 24.24 };

            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesNullISerializableProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();            
            MySubSubExtension value = null;

            dsw.WriteProperty("Serializable", value);            

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "Serializable", null }
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }


            dsw.WriteProperty("Serializable", value);            

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {                
                { "Serializable.Field3", null },
                { "Serializable.Field4", "42.42" }
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]        
        public void ThrowsIfNameIsNullOrEmptyForISerializableProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            MySubSubExtension value = new MySubSubExtension { Field3 = "Value1", Field4 = 42.42 };

            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(null, value));

            dsw.WriteProperty("SerializableList", listValue);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "SerializableList", null }                
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
            };

            dsw.WriteProperty("SerializableList", listValue);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "SerializableList0.Field3", "value" },
                { "SerializableList0.Field4", "42.42" },
                { "SerializableList1", null }
            };
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            MySubSubExtension value = null;

            dsw.WriteProperty(value);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>(); // Generated name would not indicate which object is missing, hence useless, returning empty dictionary
            
            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

            dsw.WriteProperty(value);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { DictionarySerializationWriter.DefaultKey + "1.Field3", null },
                { DictionarySerializationWriter.DefaultKey + "1.Field4", "42.42" }
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesMeasurements()
        {
            Dictionary<string, double> metrics = new Dictionary<string, double>
            {
                { "Metric1", 1.2 },
                { "Metric2", 2.1 }
            };

            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("Metrics", metrics);

            Dictionary<string, double> excpectedSerialization = new Dictionary<string, double>()
            // Unlike properties, the absence of metric does not indicate something
            {
                { "Metrics.Metric1", 1.2},
                { "Metrics.Metric2", 2.1}

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedMeasurements);
            AssertEx.IsEmpty(dsw.AccumulatedDictionary);
        }

        [TestMethod]
        public void WritesMultipleMeasurements()
        {
            Dictionary<string, double> metrics = new Dictionary<string, double>
            {
                { "Metrics.Metric1", 1.2},
                { "Metrics.Metric2", 2.1},
                { "otherMetrics.Metric1", 3.4},
                { "otherMetrics.Metric2", 4.3},
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedMeasurements);
            AssertEx.IsEmpty(dsw.AccumulatedDictionary);
        }

        [TestMethod]
        public void DoesNotWriteNullMeasurements()
        {
            Dictionary<string, double> metrics = null;            

            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("Metrics", metrics);

            Dictionary<string, double> excpectedSerialization = new Dictionary<string, double>();


        [TestMethod]
        public void IndentsKeyWhenWritesStartObject()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteStartObject("MyObject");
            dsw.WriteProperty("name", "value");
            dsw.WriteEndObject();

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
                { "MyObject.name", "value"}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void IndentsKeyWhenWritesStartObjectWithNoName()
        {

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void IndentsKeyMultipleTimesWhenWritesMultipleStartObject()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteStartObject("First");
            dsw.WriteProperty("name", "value");
            dsw.WriteStartObject("Second");
            dsw.WriteProperty("name", "value");
            dsw.WriteEndObject();
            dsw.WriteEndObject();

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "First.name", "value"},
                { "First.Second.name", "value"}
        [TestMethod]        
        public void ThrowsIfIndentNameIsNullOrEmpty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteStartObject(null));
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteStartObject(string.Empty));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
            complexExtension.MyTimeSpanField = TimeSpan.FromSeconds(2);
            complexExtension.MySubExtensionField = mySubExtension1;
            complexExtension.MyExtensionListField = listExtension;
            complexExtension.MyStringListField = listString;

            var dicString = new Dictionary<string, string>();
            dicString.Add("Item1", "Value1");
            dicString.Add("Item2", "Value2");
            complexExtension.MyStringDictionaryField = dicString;

        [TestMethod]
        public void DoesNowWriteDuplicateName()
        {
            TimeSpan ts = new TimeSpan(1, 2, 3);
            DateTimeOffset dto = new DateTimeOffset();
            List<string> strings = new List<string> { "ListValue", "ListAnotherValue" };
            Dictionary<string, string> dict = new Dictionary<string, string> { { "name", "value" }, { "anotherName", "anotherValue"} };
            MySubSubExtension ext = new MySubSubExtension() { Field3 = "extValue", Field4 = 1.2 };
            List<ISerializableWithWriter> extensions = new List<ISerializableWithWriter> { ext, ext };

            dsw.WriteProperty("name.dupe", 12);
            dsw.WriteProperty("name.dupe", true);
            dsw.WriteProperty("name.dupe", ts);
            dsw.WriteProperty("name.dupe", dto);
            dsw.WriteProperty("name.dupe0", "value"); // Dupe of the list entries below
            dsw.WriteProperty("name.dupe1", "value"); // Dupe of the list entries below
            dsw.WriteProperty("name.dupe", strings);            
            dsw.WriteProperty("name.dupe.name", "value"); // Dupe of the dict entry below
            dsw.WriteProperty("name.dupe.anotherName", "value"); // Dupe of the dict entry below
            dsw.WriteProperty("name.dupe", dict);
            dsw.WriteProperty("name.dupe.Field3", "value"); // Dupe of the extensions list entry below
            dsw.WriteProperty("name.dupe.Field4", "value"); // Dupe of the extensions list entry below
            dsw.WriteProperty("name.dupe", ext);
            dsw.WriteProperty("name.dupe0.Field3", "value"); // Dupe of the extensions list entry below
            dsw.WriteProperty("name.dupe0.Field4", "value"); // Dupe of the extensions list entry below
            dsw.WriteProperty("name.dupe1.Field3", "value"); // Dupe of the extensions list entry below
            dsw.WriteProperty("name.dupe1.Field4", "value"); // Dupe of the extensions list entry below
            dsw.WriteProperty("name.dupe", extensions);

            dsw.WriteProperty(DictionarySerializationWriter.DefaultKey + "1.Field3", "value"); // Dupe of no-name extension entry below
            dsw.WriteProperty("dupe", 12);
            dsw.WriteProperty("dupe", true);
            dsw.WriteProperty("dupe", ts);
            dsw.WriteProperty("dupe", dto);
            dsw.WriteProperty("dupe", strings);
            dsw.WriteProperty("dupe", dict);
            dsw.WriteProperty("dupe", ext);
            dsw.WriteProperty("dupe", extensions);
            dsw.WriteStartObject("dupe");
            dsw.WriteProperty(ext);
            dsw.WriteProperty("name.dupe", metrics);

            Dictionary<string, string> expectedProperties = new Dictionary<string, string>
            {
                { "name.dupe", "value"},
                { "name.dupe0", "value"},
                { "name.dupe1", "value"},
                { "name.dupe.name", "value"},
                { "name.dupe.anotherName", "value"},
                { "name.dupe.Field3", "value"},
                { "name.dupe0.Field3", "value"},
                { "name.dupe0.Field4", "value"},
                { "name.dupe1.Field3", "value"},
                { "name.dupe1.Field4", "value"},
                { DictionarySerializationWriter.DefaultKey + "1.Field3", "value"},
                { DictionarySerializationWriter.DefaultKey + "1.Field4", "value"},
                { "name.dupe." + DictionarySerializationWriter.DefaultKey + "1.Field3", "value"},
                { "name.dupe." + DictionarySerializationWriter.DefaultKey + "1.Field4", "value"}
            };

            Dictionary<string, double> expectedMetrics = new Dictionary<string, double>
            {
                { "name.dupe.metric", 1.2}
            };

            AssertEx.AreEqual(expectedProperties, dsw.AccumulatedDictionary);
            AssertEx.AreEqual(expectedMetrics, dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesMultiLevelObjectWithVariableDepths()
        {
            // Trying to serialize an object with back and forth depth:
            //{
            //    "name" : "value1"
            //    "depth1" : {
            //        "name" : "value2"
            //        "depth2" : {
            //            "name": "value3"
            //            "key" : "value4"
            //        }
            //        "key" : "value5",
            //        "depth2again" : {
            //            "name" : "value6"
            //            "depth3" :{
            //                "name" : "value7"
            //            }
            //            "key":"value8"
            //        }
            //        "item" : "value9"
            //    }
            //    "key" : "value10"
            //    "item" : "value11"
            //    "depth1again" : {
            //        "depth2again" : {
            //            "depth3again" : {
            //                "name" : "value12"
            //            }
            //            "key" : "value13"
            //        }
            //        "item" : "value14"
            //    }
            //}

            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            
            dsw.WriteProperty("name", "value1");
            dsw.WriteStartObject("depth1");
                dsw.WriteProperty("name", "value2");
                dsw.WriteStartObject("depth2");
                    dsw.WriteProperty("name", "value3");
                    dsw.WriteProperty("key", "value4");
                dsw.WriteEndObject();
                dsw.WriteProperty("key", "value5");
                dsw.WriteStartObject("depth2again");
                    dsw.WriteProperty("name", "value6");
                    dsw.WriteStartObject("depth3");
                        dsw.WriteProperty("name", "value7");
                    dsw.WriteEndObject();
                    dsw.WriteProperty("key", "value8");
                dsw.WriteEndObject();
                dsw.WriteProperty("item", "value9");
            dsw.WriteEndObject();
            dsw.WriteProperty("key", "value10");
            dsw.WriteProperty("item", "value11");
            dsw.WriteStartObject("depth1again");
                dsw.WriteStartObject("depth2again");
                    dsw.WriteStartObject("depth3again");
                        dsw.WriteProperty("name", "value12");
                    dsw.WriteEndObject();
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesMultiLevelObjectWithVariableDepthsAndDynamicNames()
        {
            // Trying to serialize an object with back and forth depth:
            //{
            //    "name" : "value1"
            //    {
            //            "name" : "value6"
            //            {
            //                "name" : "value7"
            //            }
            //            "key":"value8"
            //        }
            //        "item" : "value9"
            //    }
            //    "key" : "value10"
            //    "item" : "value11"
            //    {
            //        {
            //            {
            //                "name" : "value12"
            //            }
            //            "key" : "value13"
            //        }
            //        "item" : "value14"
            //    }
            //}
                dsw.WriteStartObject();
                    dsw.WriteProperty("name", "value3");
                    dsw.WriteProperty("key", "value4");
                dsw.WriteEndObject();
                dsw.WriteProperty("key", "value5");
                dsw.WriteStartObject();
                    dsw.WriteProperty("name", "value6");
                    dsw.WriteStartObject();
                        dsw.WriteProperty("name", "value7");
                    dsw.WriteEndObject();
                    dsw.WriteProperty("key", "value8");
                dsw.WriteEndObject();
                dsw.WriteProperty("item", "value9");
            dsw.WriteEndObject();
            dsw.WriteProperty("key", "value10");
            dsw.WriteProperty("item", "value11");
            dsw.WriteStartObject();
                dsw.WriteStartObject();
                    dsw.WriteStartObject();
                        dsw.WriteProperty("name", "value12");
                {"Obj2.Obj1.Obj1.name", "value12" },
                {"Obj2.Obj1.key", "value13" },
                {"Obj2.item", "value14" }
            };

            AssertEx.AreEqual(expectedDictionary, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
