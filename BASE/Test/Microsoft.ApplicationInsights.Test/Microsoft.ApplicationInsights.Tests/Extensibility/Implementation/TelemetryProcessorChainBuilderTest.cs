namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System.Text;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class TelemetryProcessorChainBuilderTest
    {
        [TestMethod]
        public void NoExceptionOnReturningNullFromUse()
        {
            var configuration = new TelemetryConfiguration();

            var builder = new TelemetryProcessorChainBuilder(configuration);
            Assert.AreEqual(1, configuration.TelemetryProcessors.Count); // Transmission is added by default
        }

        [TestMethod]
        {
            var configuration = new TelemetryConfiguration();

            var builder = new TelemetryProcessorChainBuilder(configuration);
            builder.Use(next => new StubTelemetryProcessor(next));
            builder.Use(next => null);
            builder.Use(next => new StubTelemetryProcessor(next));
        public void TransmissionProcessorIsAddedDefaultWhenNoOtherTelemetryProcessorsAreConfigured()
        {
            var config = new TelemetryConfiguration();
            var builder = new TelemetryProcessorChainBuilder(config);
            builder.Build();
            AssertEx.IsType<TransmissionProcessor>(config.DefaultTelemetrySink.TelemetryProcessorChain.FirstTelemetryProcessor);
        }

        [TestMethod]
        public void UsesTelemetryProcessorGivenInUseToBuild()
        {
            var config = new TelemetryConfiguration();
            var builder = new TelemetryProcessorChainBuilder(config);
            builder.Use(next => new StubTelemetryProcessor(next));

            Assert.AreNotSame(tc1.TelemetryProcessors, tc2.TelemetryProcessors);
        }

        [TestMethod]
        public void BuildOrdersTelemetryChannelsInOrderOfUseCalls()
        {
            StringBuilder outputCollector = new StringBuilder();
            var builder = new TelemetryProcessorChainBuilder(config);
            builder.Use((next) => new StubTelemetryProcessor(next) { OnProcess = (item) => { outputCollector.Append("processor1"); } });
            builder.Use((next) => new StubTelemetryProcessor(next) { OnProcess = (item) => { outputCollector.Append("processor2"); } });
            builder.Build();
            config.TelemetryProcessorChain.Process(new StubTelemetry());



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
