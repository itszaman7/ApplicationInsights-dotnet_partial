namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class MyTestExtension : IExtension
    {
        public int myIntField;
        public string myStringField;
        public IExtension DeepClone()
        {
            var other = new MyTestExtension();
            other.myIntField = this.myIntField;
            other.myStringField = this.myStringField;

            return other;
        }
        public double MyDoubleField;
        public bool MyBoolField;
        public TimeSpan MyTimeSpanField;
        public DateTimeOffset MyDateTimeOffsetField;
        public MySubExtension MySubExtensionField;
        public IList<string> MyStringListField;
        public IList<MySubExtension> MyExtensionListField;
        public IDictionary<string, string> MyStringDictionaryField;

        public void Serialize(ISerializationWriter serializationWriter)
        {
            serializationWriter.WriteProperty("MyStringField", MyStringField);
            serializationWriter.WriteProperty("MyIntField", MyIntField);
            serializationWriter.WriteProperty("MyDoubleField", MyDoubleField);
            serializationWriter.WriteProperty("MyBoolField", MyBoolField);
            serializationWriter.WriteProperty("MyTimeSpanField", MyTimeSpanField);
        public string Field1;
        public int Field2;
        public ISerializableWithWriter MySubSubExtension;

        public void Serialize(ISerializationWriter serializationWriter)
        {
            serializationWriter.WriteProperty("Field1", Field1);
            serializationWriter.WriteProperty("Field2", Field2);
        }
    }

    public class MySubSubExtension : ISerializableWithWriter
    {
        public string Field3;
        public double Field4;



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
