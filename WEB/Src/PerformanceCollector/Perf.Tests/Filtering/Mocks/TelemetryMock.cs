namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;

    internal class TelemetryMock : ITelemetry
    {
        public enum EnumType
        {
        public string StringField { get; set; }

        public TimeSpan TimeSpanField { get; set; }

        public Uri UriField { get; set; }

        public IDictionary<string, string> Properties { get; } = new Dictionary<string, string>();

        public IDictionary<string, double> Metrics { get; } = new Dictionary<string, double>();

        public DateTimeOffset Timestamp { get; set; }

        public TelemetryContext Context { get; set; } = new TelemetryContext();

        public TelemetryContextMock ContextMock { get; set; } = new TelemetryContextMock();

        public string Sequence { get; set; }

        public IExtension Extension { get; set; }

        public void Sanitize()
        {
            throw new NotImplementedException();
        }
            return this;
        }

        public void SerializeData(ISerializationWriter serializationWriter)
        {
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
