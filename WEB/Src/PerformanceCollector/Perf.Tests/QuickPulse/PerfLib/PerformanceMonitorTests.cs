#if NET452
namespace Microsoft.ApplicationInsights.Tests
{
    [TestClass]
    public class PerformanceMonitorTests
        [TestMethod]
        public void PerformanceMonitorGetsDataFromRegistry()
            
            // ACT
            byte[] data = perfMon.GetData("230");
            perfMon.Close();

            // ASSERT


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
