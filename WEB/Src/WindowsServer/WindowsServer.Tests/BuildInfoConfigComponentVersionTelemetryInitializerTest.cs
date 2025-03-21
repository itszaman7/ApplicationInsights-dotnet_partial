namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System.IO;
    using System.Xml.Linq;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildInfoConfigComponentVersionTelemetryInitializerTest
    {
        [TestMethod]
        public void InitializeDoesNotThrowIfFileDoesNotExist()
        {
            var source = new BuildInfoConfigComponentVersionTelemetryInitializer();
        [TestMethod]
        public void InitializeSetsNullVersionIfBuildRootInfoNull()
        {
            var source = new BuildInfoConfigComponentVersionTelemetryInitializer();
            var requestTelemetry = new RequestTelemetry();
            source.Initialize(requestTelemetry);
            
            Assert.IsNull(requestTelemetry.Context.Component.Version);
            source.Initialize(requestTelemetry);

            Assert.IsNull(requestTelemetry.Context.Component.Version);
        }

        {
            var doc = XDocument.Load(new StringReader("<DeploymentEvent><Build><MSBuild><BuildLabel>123</BuildLabel></MSBuild></Build></DeploymentEvent>")).Root;

            var source = new BuildInfoConfigComponentVersionTelemetryInitializerMock(doc);
            var requestTelemetry = new RequestTelemetry();

        private class BuildInfoConfigComponentVersionTelemetryInitializerMock : BuildInfoConfigComponentVersionTelemetryInitializer
        {
            private readonly XElement element;

            {
                return this.element;
            }
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
