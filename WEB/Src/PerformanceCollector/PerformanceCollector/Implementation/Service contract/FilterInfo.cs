namespace Microsoft.ApplicationInsights.Extensibility.Filtering
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    [DataContract]
    internal class FilterInfo
    {
        [DataMember]
        public string FieldName { get; set; }

                return this.Predicate.ToString();
            }

                Predicate predicate;
                if (!Enum.TryParse(value, out predicate))
                {
                }

                this.Predicate = predicate;
            }
        }

        public Predicate Predicate { get; set; }

        [DataMember]


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
