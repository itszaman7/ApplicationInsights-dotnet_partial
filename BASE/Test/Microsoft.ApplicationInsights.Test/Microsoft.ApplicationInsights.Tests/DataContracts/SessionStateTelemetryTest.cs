namespace Microsoft.ApplicationInsights.DataContracts
{
    using AI;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    using KellermanSoftware.CompareNetObjects;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    [TestClass]
    public class SessionStateTelemetryTest
    {
        [TestMethod]
        public void SessionStateTelemetryImplementsITelemetryContract()
        {
            var test = new ITelemetryTest<SessionStateTelemetry, AI.EventData>();
            test.Run();
        }

        [TestMethod]
        public void ConstructorInitializesStateWithSpecifiedValue()
        {
            var telemetry = new SessionStateTelemetry(SessionState.End);
            Assert.AreEqual(SessionState.End, telemetry.State);
            var telemetry = new SessionStateTelemetry(SessionState.Start);
            Assert.IsNotNull(telemetry.Context);
        }

        [TestMethod]
        public void SessionStateIsStartByDefault()
        }

        [TestMethod]
        public void SessionStateCanBeSetByUser()
        {
            var telemetry = new SessionStateTelemetry();

            CompareLogic deepComparator = new CompareLogic();

            var result = deepComparator.Compare(telemetry, other);
            Assert.IsTrue(result.AreEqual, result.DifferencesString);
        }

        [TestMethod]
        public void SessionStateTelemetryDeepCloneWithNullExtensionDoesNotThrow()
        {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
