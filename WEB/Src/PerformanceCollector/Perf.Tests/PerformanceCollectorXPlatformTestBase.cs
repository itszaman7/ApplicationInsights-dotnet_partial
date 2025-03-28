﻿#if NETCOREAPP
namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation;    

    /// <summary>
    /// PerformanceCollector test base.
    /// </summary>
    public class PerformanceCollectorXPlatformTestBase
    {
        internal void PerformanceCollectorSanityTest(IPerformanceCollector collector, string counter, string categoryName, string counterName, string instanceName)
            for (int i = 0; i < CounterCount; i++)
            {
                string error;
                collector.RegisterCounter(
                    counter,
                    null,
                    out error,
                    false);                
            }

            var results = collector.Collect().ToList();            

            Assert.AreEqual(CounterCount, results.Count);

            foreach (var result in results)
            {
                var value = result.Item2;

                Assert.AreEqual(categoryName, result.Item1.PerformanceCounter.CategoryName);
                Assert.AreEqual(counterName, result.Item1.PerformanceCounter.CounterName);

                    counterRequest.PerformanceCounter,
                    counterRequest.ReportAs,
                    out error,

            Assert.AreEqual(2, twoCounters.Count());
            Assert.AreEqual(@"\Process(??APP_WIN32_PROC??)\Private Bytes", twoCounters[0].OriginalString);
            Assert.AreEqual(@"\Process(??APP_WIN32_PROC??)\% Processor Time", twoCounters[1].OriginalString);

            Assert.AreEqual(@"\Process(??APP_WIN32_PROC??)\% Processor Time", oneCounter.Single().OriginalString);
        }        


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
