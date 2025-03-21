#if NET452
namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.Operation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Disposing TelemetryConfiguration after each test.")]
    [TestClass]
    public sealed class ProfilerSqlProcessingTest
    {
        private const string DatabaseServer = "ourdatabase.database.windows.net";
        private const string DataBaseName = "mydatabase";
        private const string MyStoredProcName = "apm.MyFavouriteStoredProcedure";

        [SuppressMessage("Microsoft.Security", "CS002:SecretInNextLine", Justification = "Fake password used for testing.")]
        private static readonly string ConnectionString = string.Format(CultureInfo.InvariantCulture, "Server={0};DataBase={1};User=myusername;Password=supersecret", DatabaseServer, DataBaseName);
        private static readonly string ExpectedResourceName = DatabaseServer + " | " + DataBaseName + " | " + MyStoredProcName;
        private static readonly string ExpectedData = MyStoredProcName;
        private static readonly string ExpectedTarget = DatabaseServer + " | " + DataBaseName;
        private static readonly string ExpectedConnectionName = DatabaseServer + " | " + DataBaseName + " | Open";
        private static readonly string ExpectedType = RemoteDependencyConstants.SQL;
        private static readonly string ExpectedCommandTextForSqlConnection = "Open";

        private TelemetryConfiguration configuration;
        private List<ITelemetry> sendItems;
        private ProfilerSqlCommandProcessing sqlCommandProcessingProfiler;
        private ProfilerSqlCommandProcessing sqlCommandProcessingProfilerWithDisabledCommandText;
        private ProfilerSqlConnectionProcessing sqlConnectionProcessingProfiler;

        [TestInitialize]
        public void TestInitialize()
        {
            this.configuration = new TelemetryConfiguration();
            this.sendItems = new List<ITelemetry>();
            this.configuration.TelemetryChannel = new StubTelemetryChannel { OnSend = item => this.sendItems.Add(item) };
            this.configuration.InstrumentationKey = Guid.NewGuid().ToString();
            this.sqlCommandProcessingProfiler = new ProfilerSqlCommandProcessing(this.configuration, null, new ObjectInstanceBasedOperationHolder<DependencyTelemetry>(), true);
            this.sqlCommandProcessingProfilerWithDisabledCommandText = new ProfilerSqlCommandProcessing(this.configuration, null, new ObjectInstanceBasedOperationHolder<DependencyTelemetry>(), false);
            this.sqlConnectionProcessingProfiler = new ProfilerSqlConnectionProcessing(this.configuration, null, new ObjectInstanceBasedOperationHolder<DependencyTelemetry>());
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.configuration.Dispose();      
        }

        [TestMethod]
        public void OnBeginSavesTelemetryInWeakTable_1ArgumentOverride()
        {
            var command = GetSqlCommandTestForStoredProc();
            this.sqlCommandProcessingProfiler.OnBeginForOneParameter(command);
            DependencyTelemetry operationReturned = this.sqlCommandProcessingProfiler.TelemetryTable.Get(command).Item1;

            Assert.IsNotNull(operationReturned);
            Assert.AreEqual(ExpectedResourceName, operationReturned.Name, true, CultureInfo.InvariantCulture);
            Assert.AreEqual(ExpectedTarget, operationReturned.Target, true, CultureInfo.InvariantCulture);
            Assert.AreEqual(ExpectedType, operationReturned.Type, true, CultureInfo.InvariantCulture);
            Assert.AreEqual(ExpectedData, operationReturned.Data, true, CultureInfo.InvariantCulture);
        }

        [TestMethod]
        public void OnBeginSavesTelemetryInWeakTable_2ArgumentsOverride()
        {
            var command = GetSqlCommandTestForStoredProc();
            this.sqlCommandProcessingProfiler.OnBeginForTwoParameters(command, null);
            DependencyTelemetry operationReturned = this.sqlCommandProcessingProfiler.TelemetryTable.Get(command).Item1;

            Assert.IsNotNull(operationReturned);
            Assert.AreEqual(ExpectedResourceName, operationReturned.Name, true, CultureInfo.InvariantCulture);
            Assert.AreEqual(ExpectedTarget, operationReturned.Target, true, CultureInfo.InvariantCulture);
            Assert.AreEqual(ExpectedType, operationReturned.Type, true, CultureInfo.InvariantCulture);
            Assert.AreEqual(ExpectedData, operationReturned.Data, true, CultureInfo.InvariantCulture);
        }

        [TestMethod]
        public void OnBeginSavesTelemetryInWeakTable_3ArgumentsOverride()
        {
            var command = GetSqlCommandTestForStoredProc();
            this.sqlCommandProcessingProfiler.OnBeginForThreeParameters(command, null, null);
            DependencyTelemetry operationReturned = this.sqlCommandProcessingProfiler.TelemetryTable.Get(command).Item1;

            Assert.IsNotNull(operationReturned);
            Assert.AreEqual(ExpectedResourceName, operationReturned.Name, true, CultureInfo.InvariantCulture);
            Assert.AreEqual(ExpectedTarget, operationReturned.Target, true, CultureInfo.InvariantCulture);
            Assert.AreEqual(ExpectedType, operationReturned.Type, true, CultureInfo.InvariantCulture);
            Assert.AreEqual(ExpectedData, operationReturned.Data, true, CultureInfo.InvariantCulture);
        }

        [TestMethod]
        public void OnBeginSavesTelemetryInWeakTable_4ArgumentsOverride()
        {
            var command = GetSqlCommandTestForStoredProc();
            this.sqlCommandProcessingProfiler.OnBeginForFourParameters(command, null, null, null);
            DependencyTelemetry operationReturned = this.sqlCommandProcessingProfiler.TelemetryTable.Get(command).Item1;

            Assert.IsNotNull(operationReturned);
            Assert.AreEqual(ExpectedResourceName, operationReturned.Name, true, CultureInfo.InvariantCulture);
            Assert.AreEqual(ExpectedTarget, operationReturned.Target, true, CultureInfo.InvariantCulture);
            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");

            ValidateTelemetryPacket(
                this.sendItems[0] as DependencyTelemetry,
                expectedName: GetResourceNameForStoredProcedure(command),
                expectedSuccess: true,
                expectedResultCode: string.Empty,
                timeBetweenBeginEndInMs: stopwatch.Elapsed.TotalMilliseconds);            
        }

                operationCreated,
                expectedName: GetResourceNameForStoredProcedure(command),
                expectedSuccess: true,
                expectedResultCode: string.Empty,
                timeBetweenBeginEndInMs: stopwatch.Elapsed.TotalMilliseconds);
        }

        [TestMethod]
        public void OnEndSendsCorrectTelemetry_2ArgumentsOverride()
        {

            ValidateTelemetryPacket(
                this.sendItems[0] as DependencyTelemetry,
                expectedName: GetResourceNameForStoredProcedure(command),
                expectedSuccess: true,
                expectedResultCode: string.Empty,
                timeBetweenBeginEndInMs: stopwatch.Elapsed.TotalMilliseconds);
        }

        [TestMethod]
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var context = this.sqlCommandProcessingProfiler.OnBeginForOneParameter(command);
            this.sqlCommandProcessingProfiler.OnExceptionForThreeParameters(
                context,
                TestUtils.GenerateSqlException(10),
                command,
                null,
                null);
            stopwatch.Stop();

        [TestMethod]
        public void ResourceNameForNonStoredProcIsCollectedCorrectly()
        {
            SqlCommand command = GetSqlCommandTestForQuery();
            string actualResourceName = this.sqlCommandProcessingProfiler.GetDependencyName(command);
            Assert.AreEqual(ExpectedTarget, actualResourceName);
        }

        [TestMethod]
            SqlCommand command = GetMoreComplexSqlCommandTestForQuery();
            string actualCommandName = this.sqlCommandProcessingProfiler.GetCommandName(command);
            Assert.AreEqual(command.CommandText, actualCommandName);
        }

        [TestMethod]
        public void CommandNameForStoredProcIsCollectedCorrectly()
        {
            SqlCommand command = GetSqlCommandTestForStoredProc();
            string actualCommandName = this.sqlCommandProcessingProfiler.GetCommandName(command);
            Assert.AreEqual(command.CommandText, actualCommandName);
        }

        [TestMethod]
        public void CommandNameForNonStoredProcIsNotCollectedWhenDisabled()
        {
            SqlCommand command = GetMoreComplexSqlCommandTestForQuery();
            string actualCommandName = this.sqlCommandProcessingProfilerWithDisabledCommandText.GetCommandName(command);
            Assert.AreEqual(string.Empty, actualCommandName);
        }
                    this.sqlCommandProcessingProfiler.OnBeginForOneParameter(null);
                }
                catch (Exception)
                {
                    Assert.Fail("Should not be throwing unhandled exceptions.");
                }

                var message = listener.Messages.First(item => item.EventId == 14);
                Assert.IsNotNull(message);
            }
        }

        [TestMethod]
        public void OnEndLogsWarningWhenNullObjectPassedToEndMethods()
        {
            using (var listener = new TestEventListener())
            {
                    this.sqlCommandProcessingProfiler.OnEndForOneParameter(new object(), new object(), null);
                }
                catch (Exception)
                {
                    Assert.Fail("Should not be throwing unhandled exceptions.");
                }

                var message = listener.Messages.First(item => item.EventId == 14);
                Assert.IsNotNull(message);
            }
                catch (Exception)
                {
                    Assert.Fail("Should not be throwing unhandled exceptions.");
                }

                var message = listener.Messages.First(item => item.EventId == 12);
                Assert.IsNotNull(message);
            }
        }

            string actualCommandText = this.sqlConnectionProcessingProfiler.GetCommandName(connection);
            Assert.AreEqual(ExpectedCommandTextForSqlConnection, actualCommandText);
        }

        [TestMethod]
        public void OnEndAsyncSendsCorrectTelemetry_1ArgumentOverride()
        {
            var command = GetSqlCommandTestForStoredProc();
            
            Stopwatch stopwatch = new Stopwatch();
        [TestMethod]
        public void OnEndAsyncSendsCorrectTelemetry_2ArgumentsOverride()
        {
            var command = GetSqlCommandTestForStoredProc();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var returnTaskPassed = Task.Factory.StartNew(() => { });

            this.sqlCommandProcessingProfiler.OnBeginForTwoParameters(command, null);
            var objectReturned = this.sqlCommandProcessingProfiler.OnEndAsyncForTwoParameters(returnTaskPassed, command);

            stopwatch.Stop();

            // wait for OnEnd async completion
            Thread.Sleep(50);

            Assert.AreSame(returnTaskPassed, objectReturned, "Object returned is not the same as expected return object");
            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");

            ValidateTelemetryPacket(
                this.sendItems[0] as DependencyTelemetry,
                expectedName: GetResourceNameForStoredProcedure(command),
                expectedSuccess: true,
                expectedResultCode: string.Empty,
                timeBetweenBeginEndInMs: stopwatch.Elapsed.TotalMilliseconds,
                async: true);
        }

            stopwatch.Stop();

            // wait for OnEnd async completion
            Thread.Sleep(50);

            Assert.AreSame(returnTaskPassed, objectReturned, "Object returned is not the same as expected return object");
            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");

            ValidateTelemetryPacket(
                this.sendItems[0] as DependencyTelemetry,

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var returnTaskPassed = Task.Factory.StartNew(() => { throw TestUtils.GenerateSqlException(10); });

            this.sqlCommandProcessingProfiler.OnBeginForTwoParameters(command, null);
            var objectReturned = this.sqlCommandProcessingProfiler.OnEndAsyncForTwoParameters(returnTaskPassed, command);

            stopwatch.Stop();
                Assert.IsNotNull(message);
            }
        }

        [TestMethod]
        public void OnEndExceptionAsyncDoesNotSendTelemetryIfSuccess_1ArgumentOverride()
        {
            var command = GetSqlCommandTestForStoredProc();

            Stopwatch stopwatch = new Stopwatch();

            var objectReturned = this.sqlCommandProcessingProfiler.OnEndExceptionAsyncForOneParameter(returnTaskPassed, command);

            stopwatch.Stop();

            // wait for OnEnd async completion
            Thread.Sleep(50);

            Assert.AreSame(returnTaskPassed, objectReturned, "Object returned is not the same as expected return object");
            Assert.AreEqual(0, this.sendItems.Count, "No telemetry items should be sent");
                operationCreated,
                expectedName: GetResourceNameForStoredProcedure(command),
                expectedSuccess: true,
                expectedResultCode: string.Empty,
                timeBetweenBeginEndInMs: stopwatch.Elapsed.TotalMilliseconds,
                async: true);
        }

        [TestMethod]
        public void OnEndExceptionAsyncDoesNotSendTelemetryIfSuccess_2ArgumentsOverride()
            stopwatch.Start();

            var returnTaskPassed = Task.Factory.StartNew(() => { });

            var context = this.sqlCommandProcessingProfiler.OnBeginForTwoParameters(command, null);

            DependencyTelemetry operationCreated = this.sqlCommandProcessingProfiler.TelemetryTable.Get(command).Item1;
            Assert.AreEqual(TimeSpan.Zero, operationCreated.Duration, "Duration is zero as operation has not been stopped.");

            var objectReturned = this.sqlCommandProcessingProfiler.OnEndExceptionAsyncForTwoParameters(context, returnTaskPassed, command, null);

            stopwatch.Stop();

            // wait for OnEnd async completion
            Thread.Sleep(50);

            Assert.AreSame(returnTaskPassed, objectReturned, "Object returned is not the same as expected return object");
            Assert.AreEqual(0, this.sendItems.Count, "No telemetry items should be sent");

            // validates that duration more then zero as operation was stopped
                operationCreated,
                expectedName: GetResourceNameForStoredProcedure(command),
                expectedSuccess: true,
                expectedResultCode: string.Empty,
                timeBetweenBeginEndInMs: stopwatch.Elapsed.TotalMilliseconds,
                async: true);
        }

        [TestMethod]
        public void OnEndExceptionAsyncSendsCorrectTelemetryIfException_1ArgumentOverride()

            stopwatch.Stop();

            // wait for OnEnd async completion
            Thread.Sleep(50);

            Assert.AreSame(returnTaskPassed, objectReturned, "Object returned is not the same as expected return object");
            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");

            ValidateTelemetryPacket(
        [TestMethod]
        public void OnEndExceptionAsyncSendsCorrectTelemetryIfException_2ArgumentsOverride()
        {
            var command = GetSqlCommandTestForStoredProc();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var returnTaskPassed = Task.Factory.StartNew(() => { throw TestUtils.GenerateSqlException(10); });

            var objectReturned = this.sqlCommandProcessingProfiler.OnEndExceptionAsyncForTwoParameters(context, returnTaskPassed, command, null);

            stopwatch.Stop();

            // wait for OnEnd async completion
            Thread.Sleep(50);

            Assert.AreSame(returnTaskPassed, objectReturned, "Object returned is not the same as expected return object");
            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");

                    this.sqlCommandProcessingProfiler.OnEndAsyncForOneParameter(null, null);
                }
                catch (Exception)
                {
                    Assert.Fail("Should not be throwing unhandled exceptions.");
                }

                var message = listener.Messages.First(item => item.EventId == 14);
                Assert.IsNotNull(message);
            }
        }

        private static void ValidateTelemetryPacket(
            DependencyTelemetry remoteDependencyTelemetryActual, 
            string expectedName, 
            bool expectedSuccess,
            string expectedResultCode,
            double timeBetweenBeginEndInMs,
            bool async = false)
        {            
            Assert.AreEqual(expectedName, remoteDependencyTelemetryActual.Name, true, "Resource name in the sent telemetry is wrong");
            Assert.AreEqual(ExpectedType, remoteDependencyTelemetryActual.Type, "DependencyKind in the sent telemetry is wrong");
            Assert.AreEqual(expectedSuccess, remoteDependencyTelemetryActual.Success, "Success in the sent telemetry is wrong");
            Assert.AreEqual(expectedResultCode, remoteDependencyTelemetryActual.ResultCode, "ResultCode in the sent telemetry is wrong");

            if (!async)
            {
                Assert.IsTrue(remoteDependencyTelemetryActual.Duration.TotalMilliseconds <= timeBetweenBeginEndInMs + 1, "Incorrect duration. Collected " + remoteDependencyTelemetryActual.Duration.TotalMilliseconds + " should be less than max " + timeBetweenBeginEndInMs);
            }

            Assert.IsTrue(remoteDependencyTelemetryActual.Duration.TotalMilliseconds >= 0);

            string expectedVersion = SdkVersionHelper.GetExpectedSdkVersion(typeof(DependencyTrackingTelemetryModule), prefix: "rddp:");
            Assert.AreEqual(expectedVersion, remoteDependencyTelemetryActual.Context.GetInternalContext().SdkVersion);
        }       

        private static SqlCommand GetSqlCommandTestForQuery()
        {
            var con = new SqlConnection(ConnectionString);
            SqlCommand command = con.CreateCommand();
        private static SqlCommand GetSqlCommandTestForStoredProc()
        {
            var con = new SqlConnection(ConnectionString);
            SqlCommand command = con.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = MyStoredProcName;
            return command;
        }

        private static SqlConnection GetSqlConnectionTest()
        {
            return string.Join(
                " | ",
                command.Connection.DataSource,
                command.Connection.Database,
                command.CommandText);
        }
    }
}
#endif

# This file contains partial code from the original project
# Some functionality may be missing or incomplete
