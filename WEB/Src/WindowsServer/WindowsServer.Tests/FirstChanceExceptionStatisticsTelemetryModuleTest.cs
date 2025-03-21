#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Runtime.ExceptionServices;
    using System.Threading.Tasks;
    using DataContracts;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TestFramework;
    using Assert = Xunit.Assert;

    [TestClass]
    public class FirstChanceExceptionStatisticsTelemetryModuleTest : IDisposable
    {
        internal static readonly string TestOperationName = "FirstChanceExceptionTestOperation";

        private TelemetryConfiguration configuration;
        private ConcurrentBag<ITelemetry> items;

        [TestInitialize]
        public void TestInitialize()
        {
            this.items = new ConcurrentBag<ITelemetry>();

            this.configuration = new TelemetryConfiguration();

            this.configuration.TelemetryChannel = new StubTelemetryChannel
            {
                OnSend = telemetry => this.items.Add(telemetry),
                EndpointAddress = "http://test.com"
            };
            this.configuration.InstrumentationKey = "MyKey";
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.configuration = null;
            this.items = new ConcurrentBag<ITelemetry>();
        }

        [TestMethod]
        public void FirstChanceExceptionStatisticsTelemetryModuleDoNotThrowOnNullException()
        {
            EventHandler<FirstChanceExceptionEventArgs> handler = null;
            using (var module = new FirstChanceExceptionStatisticsTelemetryModule(
                h => handler = h,
                _ => { }))
            {
                module.Initialize(this.configuration);
                handler.Invoke(null, new FirstChanceExceptionEventArgs(null));
            }
        }

        [TestMethod]
        public void FirstChanceExceptionStatisticsTelemetryModuleDoNotCauseStackOverflow()
        {
            this.configuration.TelemetryInitializers.Add(new StubTelemetryInitializer()
            {
                OnInitialize = (item) =>
                {
                    throw new Exception("this exception may cause stack overflow as will be thrown during the processing of another exception");
                }
            });

            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);

                try
                {
                    // FirstChanceExceptionStatisticsTelemetryModule will process this exception
                    throw new Exception("test");
                }
                catch (Exception exc)
                {
                    // make sure it is the same exception as was initially thrown
                    Assert.Equal("test", exc.Message);
                }
            }
        }

        [TestMethod]
        public void FirstChanceExceptionStatisticsTelemetryModuleTracksMetricWithTypeAndMethodOnException()
        {
            var metrics = new List<KeyValuePair<Metric, double>>();
            StubMetricProcessor stub = new StubMetricProcessor()
            {
                OnTrack = (m, v) =>
                {
                    metrics.Add(new KeyValuePair<Metric, double>(m, v));
                }
            };

            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);
                module.MetricManager.MetricProcessors.Add(stub);

                try
                {
                    // FirstChanceExceptionStatisticsTelemetryModule will process this exception
                    throw new Exception("test");
                }
                catch (Exception exc)
                {
                    // code to prevent compiler optimizations
                    Assert.Equal("test", exc.Message);
                }
            }

            Assert.Single(metrics);
            Assert.Equal("Exceptions thrown", metrics[0].Key.Name);

            var dims = metrics[0].Key.Dimensions;
            Assert.Equal(1, dims.Count);

            Assert.StartsWith(typeof(Exception).FullName, dims["problemId"], StringComparison.Ordinal);

            int nameStart = dims["problemId"].IndexOf(" at ", StringComparison.OrdinalIgnoreCase) + 4;

            Assert.StartsWith(typeof(FirstChanceExceptionStatisticsTelemetryModuleTest).FullName + "." + nameof(this.FirstChanceExceptionStatisticsTelemetryModuleTracksMetricWithTypeAndMethodOnException), dims["problemId"].Substring(nameStart), StringComparison.Ordinal);
        }

        [TestMethod]
        public void FirstChanceExceptionStatisticsTelemetryModuleUsesSetsInternalOperationName()
        {
            var metrics = new List<KeyValuePair<Metric, double>>();
            StubMetricProcessor stub = new StubMetricProcessor()
            {
                OnTrack = (m, v) =>
                {
                    metrics.Add(new KeyValuePair<Metric, double>(m, v));
                }
            };

            this.configuration.TelemetryInitializers.Add(new StubTelemetryInitializer()
            {
                OnInitialize = (item) =>
                {
                    item.Context.Operation.Name = TestOperationName;
                }
            });

            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);
                module.MetricManager.MetricProcessors.Add(stub);

                SdkInternalOperationsMonitor.Enter();
                try
                {
                    try
                    {

            var dims = metrics[0].Key.Dimensions;
            Assert.Equal(2, dims.Count);

            Assert.True(dims.Contains(new KeyValuePair<string, string>(FirstChanceExceptionStatisticsTelemetryModule.OperationNameTag, "AI (Internal)")));
        }

        [TestMethod]
        public void FirstChanceExceptionStatisticsTelemetryModuleUsesOperationNameAsDimension()
        {

                try
                {
                    // FirstChanceExceptionStatisticsTelemetryModule will process this exception
                    throw new Exception("test");
                }
                catch (Exception exc)
                {
                    // code to prevent profiler optimizations
                    Assert.Equal("test", exc.Message);
            }

            Assert.Single(metrics);
            Assert.Equal("Exceptions thrown", metrics[0].Key.Name);

            var dims = metrics[0].Key.Dimensions;
            Assert.Equal(2, dims.Count);

            Assert.True(dims.Contains(new KeyValuePair<string, string>(FirstChanceExceptionStatisticsTelemetryModule.OperationNameTag, TestOperationName)));
        }
            {
                module.Initialize(this.configuration);
                module.MetricManager.MetricProcessors.Add(stub);

                try
                {
                    SdkInternalOperationsMonitor.Enter();

                    // FirstChanceExceptionStatisticsTelemetryModule will process this exception
                    throw new Exception("test");
                    SdkInternalOperationsMonitor.Exit();
                }
            }

            Assert.Single(metrics);
            Assert.Equal("Exceptions thrown", metrics[0].Key.Name);

            var dims = metrics[0].Key.Dimensions;
            Assert.Equal(2, dims.Count);

                }
            };

            int operationId = 0;

            this.configuration.TelemetryInitializers.Add(new StubTelemetryInitializer()
            {
                OnInitialize = (item) =>
                {
                    item.Context.Operation.Name = "operationName " + (operationId++);
                }
            });

            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);
                module.MetricManager.MetricProcessors.Add(stub);

                for (int i = 0; i < 200; i++)
                {
                    catch (Exception exc)
                    {
                        // code to prevent profiler optimizations
                        Assert.Equal("test", exc.Message);
                    }
                }
            }

            Assert.Equal(200, metrics.Count);
            Assert.Equal(2, this.items.Count);
                {
                    if (i == 101)
                    {
                        module.DimCapTimeout = DateTime.UtcNow.Ticks - 1;
                    }

                    try
                    {
                        // FirstChanceExceptionStatisticsTelemetryModule will process this exception
                        throw new Exception("test");
                    }
                    catch (Exception exc)
                    {
                        // code to prevent profiler optimizations
                        Assert.Equal("test", exc.Message);
                    }
                }
            }

            Assert.Equal(200, metrics.Count);
            Assert.Equal(400, this.items.Count);
        }

        [TestMethod]
        public void FirstChanceExceptionStatisticsTelemetryExceptionsAreThrottled()
        {
            var metrics = new List<KeyValuePair<Metric, double>>();
            StubMetricProcessor stub = new StubMetricProcessor()
            {
                OnTrack = (m, v) =>
                {
                    metrics.Add(new KeyValuePair<Metric, double>(m, v));
                }
            };

            this.configuration.TelemetryInitializers.Add(new StubTelemetryInitializer()
            {
                OnInitialize = (item) =>
                {
                    item.Context.Operation.Name = TestOperationName;
                {
                    countThrottled++;
                }
                else
                {
                    countProcessed++;
                }
            }

            // The test starts with the current moving average being 0. With the setting of the
                    metrics.Add(new KeyValuePair<Metric, double>(m, v));
                }
            };

            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);
                module.MetricManager.MetricProcessors.Add(stub);

                try
                        throw new Exception("test");
                    }
                    catch (Exception ex)
                    {
                        // this assert is neede to avoid code optimization
                        Assert.Equal("test", ex.Message);
                        throw;
                    }
                }
                catch (Exception exc)

            Assert.Single(metrics);
            Assert.Equal("Exceptions thrown", metrics[0].Key.Name);

            Assert.Equal(1, metrics[0].Value, 15);

            Assert.Equal(2, this.items.Count);

            ITelemetry[] testItems = this.items.ToArray();

            foreach (ITelemetry i in testItems)
            {
                if (i is MetricTelemetry)
                {
                    Assert.Equal(1, ((MetricTelemetry)i).Count);
                }
            }
        }

        [TestMethod]
                OnTrack = (m, v) =>
                {
                    metrics.Add(new KeyValuePair<Metric, double>(m, v));
                }
            };

            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);
                module.MetricManager.MetricProcessors.Add(stub);
                        try
                        {
                            task2.Wait();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    catch (Exception)
        [TestMethod]
        public void FirstChanceExceptionStatisticsTelemetryModuleWasTrackedReturnsTrueForTheSameException()
        {
            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);

        }

        //// This test is temporarily removed until WasExceptionTracked method is enhanced.
        ////[TestMethod]
        ////public void FirstChanceExceptionStatisticsTelemetryModuleWasTrackedReturnsTrueForInnerException()
        ////{
        ////    using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
        ////    {
        ////        module.Initialize(this.configuration);

        ////        var exception = new Exception();

        ////        Assert.False(module.WasExceptionTracked(exception));

        ////        var wrapper = new Exception("wrapper", exception);

        ////        Assert.True(module.WasExceptionTracked(wrapper));
        ////        Assert.True(module.WasExceptionTracked(wrapper));
        ////    }
        ////}

        //// Temporarily removing the test until WasExceptionTracked is refined.
        ////[TestMethod]
        ////public void FirstChanceExceptionStatisticsTelemetryModuleWasTrackedReturnsTrueForAggExc()
        ////{
        ////    using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
        ////    {
        ////        module.Initialize(this.configuration);

        ////        var exception = new Exception();
            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);

                var exception = new Exception();

                var aggExc = new AggregateException(exception);
                Assert.False(FirstChanceExceptionStatisticsTelemetryModule.WasExceptionTracked(aggExc));
            }
        }
        public void FirstChanceExceptionStatisticsTelemetryCallMultipleMethods()
        {
            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);

                for (int i = 0; i < 200; i++)
                {
                    try
                    {
                        this.Method1();
                    }
                    catch
                    {
                    }

                    try
                    {
                        throw new Exception("New exception from Method1");
                    }
                    {
                    }
                }
            }

            // There should be three unique TrackExceptions and three Metrics
            Assert.Equal(6, this.items.Count);
        }

        [TestMethod]
                h => handler = h,
                _ => { }))
            {
                module.Initialize(this.configuration);
            }

            Assert.NotNull(handler);
        }

        [TestMethod]
        public void DisposeCallsUnregister()
        {
            EventHandler<FirstChanceExceptionEventArgs> handler = null;
            using (var module = new FirstChanceExceptionStatisticsTelemetryModule(
                _ => { },
                h => handler = h))
            {
                module.Initialize(this.configuration);
            }

            if (disposing)
            {
                if (this.configuration != null)
                {
                    this.configuration.Dispose();
                }
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
    internal class StubMetricProcessor : IMetricProcessor
    {
        public Action<Metric, double> OnTrack = (metric, value) => { };

        public void Track(Metric metric, double value)
        {
            this.OnTrack(metric, value);
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
