namespace Microsoft.ApplicationInsights.Extensibility.Filtering
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    [DataContract]
    internal class DocumentFilterConjunctionGroupInfo
    {
        {
            get
            {
                return this.TelemetryType.ToString();
            }

                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                }

                this.TelemetryType = telemetryType;

        public override string ToString()
                "TelemetryType: '{0}', filters: '{1}'",
                this.TelemetryType,
                this.Filters?.ToString() ?? string.Empty);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
