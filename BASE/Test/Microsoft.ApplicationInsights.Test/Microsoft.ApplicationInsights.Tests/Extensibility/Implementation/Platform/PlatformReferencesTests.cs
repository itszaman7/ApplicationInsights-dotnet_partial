namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Platform
{
#if NETFRAMEWORK
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PlatformReferencesTests
        public void NoSystemWebReferences()
        {
            // Validate Platform assembly
            {
                Assert.IsTrue(!assembly.FullName.Contains("System.Web"));
    }
#endif


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
