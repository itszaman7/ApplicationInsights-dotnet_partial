namespace Microsoft.ApplicationInsights.Extensibility.Filtering
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract]
    internal class CalculatedMetricInfo
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember(Name = "TelemetryType")]
        public string TelemetryTypeForSerialization
        {
            get
            {
                return this.TelemetryType.ToString();
            }

            set
        }

        public TelemetryType TelemetryType { get; set; }

        /// <summary>
        /// Gets or sets an OR-connected collection of FilterConjunctionGroupInfo objects.
        /// </summary>
        [DataMember]
        public FilterConjunctionGroupInfo[] FilterGroups { get; set; }

        [DataMember]
        public string Projection { get; set; }

        [DataMember(Name = "Aggregation")]
        public string AggregationForSerialization
        {
            set
            {
                AggregationType aggregation;
                if (!Enum.TryParse(value, out aggregation))
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                this.TelemetryType,
                this.Projection,
                this.Aggregation,
                this.FilterGroupsToString());
        }

        private string FilterGroupsToString()


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
