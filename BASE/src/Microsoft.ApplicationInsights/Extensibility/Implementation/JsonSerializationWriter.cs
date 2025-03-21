namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{    
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using Microsoft.ApplicationInsights.Extensibility;

    internal class JsonSerializationWriter : ISerializationWriter
    {
        private readonly TextWriter textWriter;
        private bool currentObjectHasProperties;

        public JsonSerializationWriter(TextWriter textWriter)
        {
            this.textWriter = textWriter;            
        }

        /// <inheritdoc/>
        public void WriteStartObject()
        {        
            this.textWriter.Write('{');
            this.currentObjectHasProperties = false;
        }

        /// <inheritdoc/>
        public void WriteStartObject(string name)
        {
            this.WritePropertyName(name);
            this.textWriter.Write('{');
            this.currentObjectHasProperties = false;
        }        

        /// <inheritdoc/>
        public void WriteProperty(string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                this.WritePropertyName(name);
                this.WriteString(value);
            }
        }

        /// <inheritdoc/>
        public void WriteProperty(string name, int? value)
        {
            if (value.HasValue)
            {
                this.WritePropertyName(name);
                this.textWriter.Write(value.Value.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <inheritdoc/>
        public void WriteProperty(string name, bool? value)
        public void WriteProperty(string name, double? value)
        {
            if (value.HasValue)
            {
                this.WritePropertyName(name);
                this.textWriter.Write(value.Value.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <inheritdoc/>
        {
            if (value.HasValue)
            {
                this.WriteProperty(name, value.Value.ToString(string.Empty, CultureInfo.InvariantCulture));
            }
        }

        /// <inheritdoc/>
        public void WriteProperty(string name, DateTimeOffset? value)
        {
                this.WritePropertyName(name);
                this.WriteStartArray();
                foreach (var item in items)
                {                    
                    if (commaNeeded)
                    {
                        this.WriteComma();
                    }

                    this.WriteStartObject();
                    item.Serialize(this);
                    commaNeeded = true;
                    this.WriteEndObject();
                }

            }
        }

        /// <inheritdoc/>
        public void WriteProperty(string name, ISerializableWithWriter value)
        {
            if (value != null)
            {
                this.WriteStartObject(name);
                value.Serialize(this);
        /// <inheritdoc/>
        public void WriteProperty(ISerializableWithWriter value)
        {
            if (value != null)
            {                
                value.Serialize(this);                
            }
        }

        /// <inheritdoc/>
                    this.WriteProperty(item.Key, item.Value);
                }

                this.WriteEndObject();
            }
        }

        /// <inheritdoc/>
        public void WriteProperty(string name, IDictionary<string, string> values)
        {
                this.textWriter.Write(',');
            }
            else
            {
                this.currentObjectHasProperties = true;
            }

            this.WriteString(name);
            this.textWriter.Write(':');
        }

        internal void WriteStartArray()
        {
            this.textWriter.Write('[');
        }

        internal void WriteEndArray()
        {
            this.textWriter.Write(']');
        }

        internal void WriteComma()
        {
            this.textWriter.Write(',');
        }

        internal void WriteRawValue(object value)
        {
            this.textWriter.Write(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }
                    case '\\':
                        this.textWriter.Write("\\\\");
                        break;
                    case '"':
                        this.textWriter.Write("\\\"");
                        break;
                    case '\n':
                        this.textWriter.Write("\\n");
                        break;
                    case '\b':


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
