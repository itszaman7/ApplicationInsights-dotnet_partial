#if NET452
namespace Microsoft.ApplicationInsights.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.PerfLib;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CategorySampleTests
    {
        [TestMethod]
        public void CategorySampleReadsDataCorrectly()
        {
            // ARRANGE
                    stream = null;

                    dataList.AddRange(reader.ReadToEnd().Split(',').Select(byte.Parse));
                }
            }
            finally
            {
                stream?.Dispose();
            byte[] data = dataList.ToArray();

            var perfLib = PerfLib.GetPerfLib();



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
