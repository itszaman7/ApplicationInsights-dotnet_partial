namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;

    
    using DpSeverityLevel = Microsoft.ApplicationInsights.DataContracts.SeverityLevel;
    
    [TestClass]
    public class SeverityLevelExtensionsTest
    {
        [TestMethod]
        [TestMethod]
        public void TranslateSeverityLevelConvertsAllValueFromDpToSdk()
            Assert.AreEqual(SeverityLevel.Verbose, SeverityLevelExtensions.TranslateSeverityLevel(DpSeverityLevel.Verbose));
            Assert.AreEqual(SeverityLevel.Warning, SeverityLevelExtensions.TranslateSeverityLevel(DpSeverityLevel.Warning));
            Assert.AreEqual(SeverityLevel.Information, SeverityLevelExtensions.TranslateSeverityLevel(DpSeverityLevel.Information));
            Assert.AreEqual(SeverityLevel.Error, SeverityLevelExtensions.TranslateSeverityLevel(DpSeverityLevel.Error));


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
