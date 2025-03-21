namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SuccessDimensionExtractorTest
    {
            var item = new DependencyTelemetry();
            var extractor = new SuccessDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual(bool.TrueString, extractedDimension);
        }
        [TestMethod]
        public void FalseSucess()
        {
            var item = new DependencyTelemetry();
            item.Success = false;
            var extractor = new SuccessDimensionExtractor();
        }

        [TestMethod]
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual(bool.TrueString, extractedDimension);
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
