namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    // This writer produces Dictionary<string, string> properties and Dictionary<string, double> metrics from the given ISerializableWithWriter implementation.
    // Typical usage of this class is to flatten IExtension or unknown ITelemetry into the set of properties and metrics.
    internal class DictionarySerializationWriter : ISerializationWriter
    {   
        internal const string DefaultKey = "Key";
        internal const string DefaultObjectKey = "Obj";

        private readonly Stack<string> lastPrefix = new Stack<string>(); // Stores previous prefix for the property Key
        private readonly Stack<long> lastIndex = new Stack<long>(); // Stores previous index for unnamed property
        private string currentPrefix = string.Empty; // Prefix for the property name to distinguish names in depth
        private long currentIndex = 1; // Index for DefaultKey or DefaultObj to provide a property name for properties or objects with no names

        internal DictionarySerializationWriter()
        {
            this.AccumulatedDictionary = new Dictionary<string, string>();
            this.AccumulatedMeasurements = new Dictionary<string, double>();
        }

        internal Dictionary<string, string> AccumulatedDictionary { get; }
        
        internal Dictionary<string, double> AccumulatedMeasurements { get; }

        public void WriteProperty(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("This argument requires a non-empty value", nameof(name));
            }

            string key = this.GetKey(name);

            if (!this.AccumulatedDictionary.ContainsKey(key))
            {
                this.AccumulatedDictionary[key] = value;
            }
        }

        public void WriteProperty(string name, double? value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("This argument requires a non-empty value", nameof(name));
            }

            string key = this.GetKey(name);

            if (!this.AccumulatedDictionary.ContainsKey(key))
            {
                if (value.HasValue)
                {
                    this.AccumulatedDictionary[key] = value.Value.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    this.AccumulatedDictionary[key] = null;
            }

            string key = this.GetKey(name);

            if (!this.AccumulatedDictionary.ContainsKey(key))
            {
                if (value.HasValue)
                {
                    this.AccumulatedDictionary[key] = value.Value.ToString(CultureInfo.InvariantCulture);
                }

            if (!this.AccumulatedDictionary.ContainsKey(key))
            {
                if (value.HasValue)
                {
                    this.AccumulatedDictionary[key] = value.Value.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    this.AccumulatedDictionary[key] = null;
                }
            }
        }

        public void WriteProperty(string name, TimeSpan? value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("This argument requires a non-empty value", nameof(name));
            }

            string key = this.GetKey(name);

            if (!this.AccumulatedDictionary.ContainsKey(key))
            {
                if (value.HasValue)
                {
                    this.AccumulatedDictionary[key] = value.Value.ToString();
                }
                else
                {
                    this.AccumulatedDictionary[key] = null;
                }
            }
        }

        public void WriteProperty(string name, DateTimeOffset? value)
            if (!this.AccumulatedDictionary.ContainsKey(key))
            {
                if (value.HasValue)
                {
                    this.AccumulatedDictionary[key] = value.Value.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    this.AccumulatedDictionary[key] = null;
                }
                        this.AccumulatedDictionary[key] = items[i];
                    }
                }
            }
            else
            {
                this.WriteProperty(this.GetKey(name), (string)null);
            }
        }

        public void WriteProperty(string name, IList<ISerializableWithWriter> items)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("This argument requires a non-empty value", nameof(name));
            }

            if (items != null)
            {
                for (int i = 0; i < items.Count; i++)
                        this.currentPrefix = this.lastPrefix.Pop();
                    }
                    else
                    {
                        this.AccumulatedDictionary[this.GetKey(name + i.ToString(CultureInfo.InvariantCulture))] = null;
                    }
                }
            }
            else
            {
            } // It is common to store property name without value; however, storing metric name without value has less benefits.
        }

        public void WriteStartObject(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("This argument requires a non-empty value", nameof(name));
            }

            this.lastPrefix.Push(this.currentPrefix);
            this.currentPrefix = this.GetKey(name);
            this.lastIndex.Push(this.currentIndex);
            this.currentIndex = 1;
        }

        public void WriteStartObject()
        {
            this.lastPrefix.Push(this.currentPrefix);
            this.currentPrefix = this.GenerateSequencialPrefixForObject();
            this.currentPrefix = this.lastPrefix.Pop();
            this.currentIndex = this.lastIndex.Pop();
        }

        private string GetKey(string fieldName)
        {
            return string.IsNullOrEmpty(this.currentPrefix) ? fieldName : string.Concat(this.currentPrefix, ".", fieldName);
        }

        private string GenerateSequencialPrefix()


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
