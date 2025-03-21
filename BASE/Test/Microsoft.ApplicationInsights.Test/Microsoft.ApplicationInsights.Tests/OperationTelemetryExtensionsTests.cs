namespace Microsoft.ApplicationInsights
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    

    /// <summary>
    /// Tests corresponding to OperationExtension methods.
    /// </summary>
    [TestClass]
    public class OperationTelemetryExtensionsTests
    {
        /// <summary>
        /// Tests the scenario if StartOperation returns operation with telemetry item of same type.
        /// </summary>
        [TestMethod]
        public void OperationTelemetryStartInitializesTimeStampAndStartTimeToTelemetry()
        {
            var telemetry = new DependencyTelemetry();
            Assert.AreEqual(DateTimeOffset.MinValue, telemetry.Timestamp);
            telemetry.Start();
            Assert.AreNotEqual(DateTimeOffset.MinValue, telemetry.Timestamp);
        }
        /// <summary>
        /// Tests the scenario if Stop does not change start time and timestamp after start is called.
        /// </summary>
        [TestMethod]
        public void OperationTelemetryStopDoesNotAffectTimeStampAndStartTimeAfterStart()
        {
            var telemetry = new DependencyTelemetry();
            telemetry.Start();
            DateTimeOffset actualTime = telemetry.Timestamp;
            telemetry.Stop();
            telemetry.Stop();
            Assert.AreNotEqual(DateTimeOffset.MinValue, telemetry.Timestamp);
            Assert.AreEqual(telemetry.Duration, TimeSpan.Zero);
        }

        /// <summary>
        /// Tests the scenario if Start assigns current *precise* time to start time.
        /// </summary>
        [TestMethod]
        public void StartTimeIsPrecise()
        }

        /// <summary>
        /// Tests the scenario if Stop computes the duration of the telemetry when timestamps are supplied to Start and Stop.
        /// </summary>
        [TestMethod]
        public void OperationTelemetryStopWithTimestampComputesDurationAfterStartWithTimestamp()
        {
            var telemetry = new DependencyTelemetry();

        {
            var telemetry = new DependencyTelemetry();
            telemetry.Stop(timestamp: 123456789012345L); // timestamp is ignored because Start was not called

            Assert.AreEqual(TimeSpan.Zero, telemetry.Duration);
        }

        /// <summary>
        /// Tests the scenario if durations can be recorded more precisely than 1ms
        /// </summary>
        [TestMethod]
        public void OperationTelemetryCanRecordPreciseDurations()
        {
            var telemetry = new DependencyTelemetry();

            long startTime = Stopwatch.GetTimestamp();
            telemetry.Start(timestamp: startTime);

            // Note: Do not use TimeSpan.FromSeconds because it rounds to the nearest millisecond.
            var expectedDuration = TimeSpan.Parse("00:00:00.1234560");

            // Ensure we choose a time that has a fractional (non-integral) number of milliseconds
            Assert.AreNotEqual(Math.Round(expectedDuration.TotalMilliseconds), expectedDuration.TotalMilliseconds);

            double durationInStopwatchTicks = Stopwatch.Frequency * expectedDuration.TotalSeconds;

            long stopTime = (long)Math.Round(startTime + durationInStopwatchTicks);
            telemetry.Stop(timestamp: stopTime);

                Assert.IsTrue(difference.Ticks < 10);
            }
        }

        private double ComputeSomethingHeavy()
        {
            var random = new Random();
            double res = 0;
            for (int i = 0; i < 10000; i++)
            {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
