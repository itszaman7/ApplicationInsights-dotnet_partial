namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RequestSuccessDimensionExtractorTest
    {
        public void NullSuccess()
        {
            var item = new RequestTelemetry();
            var extractor = new RequestSuccessDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual(bool.TrueString, extractedDimension);
        }

        [TestMethod]
            item.Success = true;
            var extractor = new RequestSuccessDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);

        [TestMethod]
        public void Null()
            Assert.AreEqual(bool.TrueString, extractedDimension);
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
