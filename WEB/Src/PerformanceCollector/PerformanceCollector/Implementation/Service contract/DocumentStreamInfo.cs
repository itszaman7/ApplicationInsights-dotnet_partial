namespace Microsoft.ApplicationInsights.Extensibility.Filtering
{
    {
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets an OR-connected collection of filter groups.
        /// Telemetry types that are not mentioned in this array will NOT be included in the stream.
        /// Telemetry types that are mentioned in this array once or more will be included if any of the mentioning DocumentFilterConjunctionGroupInfo's pass.
        public DocumentFilterConjunctionGroupInfo[] DocumentFilterGroups { get; set; }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
