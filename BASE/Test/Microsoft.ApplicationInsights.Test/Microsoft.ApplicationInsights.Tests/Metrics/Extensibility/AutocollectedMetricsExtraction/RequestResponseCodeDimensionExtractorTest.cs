namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
        public void NullResponseCode()
        {
            Assert.AreEqual(string.Empty,extractedDimension);
        }
        {
            var item = new RequestTelemetry();
            var item = new RequestTelemetry();
            item.ResponseCode = "ExpectedResponseCode";
            var extractor = new RequestResponseCodeDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual("ExpectedResponseCode", extractedDimension);
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
