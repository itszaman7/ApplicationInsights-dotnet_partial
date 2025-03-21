namespace Microsoft.ApplicationInsights
{
    using System.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    

    [TestClass]
    public class ActivityExtensionsTests
        public void CanLoadDiagnosticSourceAssembly()
        {
            Assert.IsTrue(ActivityExtensions.TryRun(() => Assert.IsNull(Activity.Current)));
        [TestMethod]
        public void GetOperationNameReturnsNullIfThereIsNoOperationName()
        {
        [TestMethod]
        public void SetOperationNameIsConsistentWithGetOperationName()
        {
        public void GetOperationNameReturnsFirstAddedOperationName()
        {
            var activity = new Activity("test");
#if !REDFIELD
            Assert.AreEqual("test me 1", activity.GetOperationName());
#endif


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
