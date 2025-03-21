namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// PerformanceCollectorModules tests.
    /// </summary>
    [TestClass]
    public class PerformanceCollectorModulesTests
    {
#if NETFRAMEWORK
        [TestMethod]
        [SuppressMessage(category: "Microsoft.Globalization", checkId: "CA1305:SpecifyIFormatProvider", Justification = "Don't care about invariant in unit tests.")]
        public void TimerTest()
        {
            var collector = CreatePerformanceCollector();
            var configuration = CreateTelemetryConfiguration();
            var telemetryChannel = configuration.TelemetryChannel as StubTelemetryChannel;

            Exception assertionsFailure = null;

            telemetryChannel.OnSend = telemetry =>
                {
                    // validate that a proper telemetry item is being sent
                    // module will swallow any exception that we throw here, so catch and rethrow later
                    try
                    {
                        Assert.AreEqual(configuration.InstrumentationKey, telemetry.Context.InstrumentationKey);

                        Assert.IsInstanceOfType(telemetry, typeof(MetricTelemetry));

                        var perfTelemetry = telemetry as MetricTelemetry;

                        Assert.AreEqual((double)perfTelemetry.Name.GetHashCode(), perfTelemetry.Sum);
                    }
                    catch (AssertFailedException e)
                    {
                        // race condition, but we don't care who wins
                        assertionsFailure = e;
                    throw assertionsFailure;
                }
            }
        }

        [TestMethod]
        public void ConfigurationTest()
        {
            var collector = CreatePerformanceCollector();

            var configuration = CreateTelemetryConfiguration();
            configuration.InstrumentationKey = string.Empty;

            using (var module = CreatePerformanceCollectionModule(collector))
            {
                // start the module
                module.Initialize(configuration);

                // wait 1s to let the module finish initializing
                Thread.Sleep(TimeSpan.FromSeconds(1));

        [TestMethod]
        public void DefaultCountersTest()
        {
            var collector = CreatePerformanceCollector();

            var configuration = CreateTelemetryConfiguration();

            using (var module = CreatePerformanceCollectionModule(collector))
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));

                // now wait to let the module's timer run
                Thread.Sleep(TimeSpan.FromSeconds(3));

                lock (collector.Sync)
                {
                    // check that the default counter list has been registered
                    Assert.AreEqual(module.DefaultCounters.Count(), collector.Counters.Count);
                }
                                         new PerformanceCounterCollectionRequest(
                                             @"\Process(??APP_WIN32_PROC??)\% Processor Time",
                                             "CounterTwo")
                                     };

            using (var module = CreatePerformanceCollectionModule(collector, customCounters))
            {
                // start the module
                module.Initialize(configuration);

        [TestMethod]
        public void CustomCountersDuplicatesTest()
        {
            var collector = CreatePerformanceCollector();

            var configuration = CreateTelemetryConfiguration();

            var customCounters = new List<PerformanceCounterCollectionRequest>()
                                     {
                                         new PerformanceCounterCollectionRequest(
                                             "CounterTwo"),
                                         new PerformanceCounterCollectionRequest(
                                             @"\CategoryName2\CounterName2",
                                             "CounterX"),
                                         new PerformanceCounterCollectionRequest(
                                             @"\CategoryName4\CounterName4",
                                             "CounterThree"),
                                         new PerformanceCounterCollectionRequest(
                                             @"\CategoryName3\CounterName3",
                                             "CounterThree"),
                Thread.Sleep(TimeSpan.FromSeconds(3));
                {
                    // check that the configured counter list has been registered
                    Assert.AreEqual(4, module.Counters.Count());

                    Assert.AreEqual(@"\CategoryName1\CounterName1", module.Counters[0].PerformanceCounter);
                    Assert.AreEqual("CounterOne", module.Counters[0].ReportAs);

                    Assert.AreEqual(@"\CategoryName2\CounterName2", module.Counters[1].PerformanceCounter);
                    Assert.AreEqual("CounterTwo", module.Counters[1].ReportAs);

                    Assert.AreEqual("CounterThree", module.Counters[3].ReportAs);
                }
            }
        }

        [TestMethod]
        [Timeout(5000)]
        public void UnicodeSupportTest()
        {
            var collector = CreatePerformanceCollector();

            var configuration = CreateTelemetryConfiguration();

            var customCounters = new List<PerformanceCounterCollectionRequest>()
                                     {
                                         new PerformanceCounterCollectionRequest(
                                             @"\CategoryName1\CounterName1",
                                             "CounterOne"),
                                         new PerformanceCounterCollectionRequest(
                                             @"\CategoryNameTwo\CounterNameTwo",
                                             @"\CategoryName3\CounterName3",
                                             null),
                                         new PerformanceCounterCollectionRequest(
                                             @"\CategoryName4\CounterName4",
                                             " Counter 4"),
                                         new PerformanceCounterCollectionRequest(
                                             @"\CategoryName5\CounterName5",
                                             " Counter5"),
                                         new PerformanceCounterCollectionRequest(
                                             @"\Категория6\Счетчик6",
                // and wait to let the module's timer run
                while (collector.Counters.Count < 8)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(100));
                }

                lock (collector.Sync)
                {
                    // check that the configured counter list has been registered
                    // check sanitization rules
                    Assert.AreEqual(@"\Категория6\Счетчик6", collector.Counters[7].Item1.OriginalString);
                    Assert.AreEqual(@"Только юникод первый", collector.Counters[7].Item1.ReportAs);

                    Assert.AreEqual(@"\Категория7\Счетчик7", collector.Counters[8].Item1.OriginalString);
                    Assert.AreEqual(@"Только юникод второй", collector.Counters[8].Item1.ReportAs);

                    Assert.AreEqual(
                        @"\CategoryNameAnother8%\CounterNameAnother8%",
                        collector.Counters[9].Item1.OriginalString);
                    Assert.AreEqual(@"CategoryNameAnother8% - CounterNameAnother8%", collector.Counters[9].Item1.ReportAs);
            var configuration = CreateTelemetryConfiguration();

            var customCounters = new List<PerformanceCounterCollectionRequest>()
                                     {
                                         new PerformanceCounterCollectionRequest(
                                             @"\CategoryName1(InstanceName1)\CounterName1",
                                             null),
                                         new PerformanceCounterCollectionRequest(
                                             @"\Process(??APP_WIN32_PROC??)\% Processor Time",
                                             null)

                lock (collector.Sync)
                {
                    // nothing should have been registered
                    Assert.AreEqual(0, collector.Counters.Count);
                }
            }
        }

        [TestMethod]
        public void TelemetryModuleIsNotInitializedTwiceToPreventTimerBeingRecreated()
        {
            var module = new PerformanceCollectorModule();
            PrivateObject privateObject = new PrivateObject(module);

            module.Initialize(TelemetryConfiguration.CreateDefault());
            object config1 = privateObject.GetField("telemetryConfiguration");

            module.Initialize(TelemetryConfiguration.CreateDefault());
            object config2 = privateObject.GetField("telemetryConfiguration");

            Assert.AreSame(config1, config2);
        }

        private static TelemetryConfiguration CreateTelemetryConfiguration()
        {
            var configuration = new TelemetryConfiguration();

            configuration.InstrumentationKey = "56D500C1-0F6C-46D1-A1F2-250D65075E0F";
            configuration.TelemetryChannel = new StubTelemetryChannel();

            return configuration;
        }

        private static PerformanceCollectorMock CreatePerformanceCollector()
        {
            return new PerformanceCollectorMock();
        }

        private static PerformanceCollectorModule CreatePerformanceCollectionModule(IPerformanceCollector collector, List<PerformanceCounterCollectionRequest> customCounterList = null)
                module.Initialize(new TelemetryConfiguration());

                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\Process(??APP_WIN32_PROC??)\% Processor Time"));
                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\Process(??APP_WIN32_PROC??)\% Processor Time Normalized"));
                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\Process(??APP_WIN32_PROC??)\Private Bytes"));
                Assert.AreEqual(3, module.DefaultCounters.Count);
            }
            finally
            {
                PerformanceCounterUtility.IsWindows = original;
            Environment.SetEnvironmentVariable("WEBSITE_SITE_NAME", "something");
            var module = new PerformanceCollectorModule();
            try
            {
                module.Initialize(new TelemetryConfiguration());

                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\Process(??APP_WIN32_PROC??)\% Processor Time"));
                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\Process(??APP_WIN32_PROC??)\% Processor Time Normalized"));
                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\Process(??APP_WIN32_PROC??)\Private Bytes"));

                Environment.SetEnvironmentVariable("WEBSITE_SITE_NAME", string.Empty);
                Task.Delay(1000).Wait();
            }
        }

        [TestMethod]
        public void PerformanceCollectorModuleDefaultContainsExpectedCountersWindows()
        {
            PerformanceCounterUtility.isAzureWebApp = null;
            var module = new PerformanceCollectorModule();
                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Request Execution Time"));
                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Requests In Application Queue"));
#endif

                Assert.IsTrue(ContainsPerfCounter(module.DefaultCounters, @"\Processor(_Total)\% Processor Time"));
#if NETFRAMEWORK
                Assert.AreEqual(10, module.DefaultCounters.Count);
#else
                Assert.AreEqual(6, module.DefaultCounters.Count);
#endif
            foreach (var counter in counters)
            {
                if (counter.PerformanceCounter.Equals(name))
                {
                    return true;
                }
            }

            return false;
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
