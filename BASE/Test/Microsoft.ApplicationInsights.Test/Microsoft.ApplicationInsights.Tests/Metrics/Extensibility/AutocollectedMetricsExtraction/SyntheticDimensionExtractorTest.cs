namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

        public void NullSynthetic()
        {
            var item = new RequestTelemetry();
            var item = new RequestTelemetry();
            item.Context.Operation.SyntheticSource = string.Empty;
            var extractor = new SyntheticDimensionExtractor();
            Assert.AreEqual(bool.FalseString, extractedDimension);
        }

        public void Synthetic()
        {
            var item = new RequestTelemetry();
            Assert.AreEqual(bool.TrueString, extractedDimension);
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
