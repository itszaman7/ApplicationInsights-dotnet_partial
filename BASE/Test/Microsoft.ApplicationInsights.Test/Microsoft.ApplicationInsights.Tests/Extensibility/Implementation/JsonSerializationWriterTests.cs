namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text;


    /// <summary>
            var mySubSubExtension1 = new MySubSubExtension() { Field3 = "Value1 for field3", Field4 = 100.00 };
            var mySubSubExtension2 = new MySubSubExtension() { Field3 = "Value2 for field3", Field4 = 200.00 };
            var mySubExtension1 = new MySubExtension() { Field1 = "Value1 for field1", Field2 = 100 , MySubSubExtension = mySubSubExtension1 };
            var mySubExtension2 = new MySubExtension() { Field1 = "Value2 for field1", Field2 = 200, MySubSubExtension = mySubSubExtension2 };
            listExtension.Add(mySubExtension2);

            var listString = new List<string>();
            listString.Add("Item1");
            listString.Add("Item2");
            listString.Add("Item3");

            complexExtension.MyBoolField = true;
            var stringBuilder = new StringBuilder();
            using (StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
            {
                var jsonSerializationWriter = new JsonSerializationWriter(stringWriter);
                jsonSerializationWriter.WriteStartObject();
                complexExtension.Serialize(jsonSerializationWriter);
                jsonSerializationWriter.WriteEndObject();
            }
            Trace.WriteLine(actualJson);
            
            JObject obj = JsonConvert.DeserializeObject<JObject>(actualJson);
            
            Assert.IsNotNull(actualJson);            
            Assert.AreEqual("ValueStringField", obj["MyStringField"].ToString());
            Assert.AreEqual(100, int.Parse(obj["MyIntField"].ToString()));
            Assert.AreEqual(100.10, double.Parse(obj["MyDoubleField"].ToString()));
            Assert.AreEqual(true, bool.Parse(obj["MyBoolField"].ToString()));
            Assert.AreEqual(TimeSpan.FromSeconds(2), TimeSpan.Parse(obj["MyTimeSpanField"].ToString()));
            //Assert.AreEqual(DateTimeOffset., double.Parse(obj["MyDateTimeOffsetField"].ToString()));

            Assert.AreEqual("Value1 for field1",obj["MySubExtensionField"]["Field1"].ToString());
            Assert.AreEqual(100, int.Parse(obj["MySubExtensionField"]["Field2"].ToString()));

            Assert.AreEqual("Value1 for field3", obj["MySubExtensionField"]["MySubSubExtension"]["Field3"].ToString());
            Assert.AreEqual(100, int.Parse(obj["MyExtensionListField"][0]["MySubSubExtension"]["Field4"].ToString()));

            Assert.AreEqual("Value2 for field1", obj["MyExtensionListField"][1]["Field1"].ToString());
            Assert.AreEqual(200, int.Parse(obj["MyExtensionListField"][1]["Field2"].ToString()));
            Assert.AreEqual("Value2 for field3", obj["MyExtensionListField"][1]["MySubSubExtension"]["Field3"].ToString());
            Assert.AreEqual(200, int.Parse(obj["MyExtensionListField"][1]["MySubSubExtension"]["Field4"].ToString()));

            Assert.AreEqual("Value1", obj["MyStringDictionaryField"]["Key1"].ToString());


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
