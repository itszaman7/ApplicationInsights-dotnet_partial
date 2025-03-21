namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.SelfDiagnostics
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [TestClass]
    public class SelfDiagnosticsConfigRefresherTest
    {
        private static readonly string ConfigFilePath = SelfDiagnosticsConfigParser.ConfigFileName;
        private static readonly byte[] MessageOnNewFile = MemoryMappedFileHandler.MessageOnNewFile;
        private static readonly string MessageOnNewFileString = Encoding.UTF8.GetString(MessageOnNewFile);


        [TestMethod]
        public void SelfDiagnosticsConfigRefresher_OmitAsConfigured()
        {
            try
            {
                CreateConfigFile();
                using (var configRefresher = new SelfDiagnosticsConfigRefresher())
                {
                    // Emitting event of EventLevel.Warning
                    // The event was omitted
                    Assert.AreEqual('\0', (char)actualBytes[MessageOnNewFile.Length]);
                }
            }
            finally
            {
                CleanupConfigFile();
            }
        }

            try
            {
                CreateConfigFile();
                using (var configRefresher = new SelfDiagnosticsConfigRefresher())
                {
                    // Emitting event of EventLevel.Error
                    CoreEventSource.Log.InvalidOperationToStopError();

                    var filePath = configRefresher.CurrentFilePath;

            var val = @"C:\home\LogFiles\SelfDiagnostics";
            Environment.SetEnvironmentVariable(key, val);

            try
            {
                CreateConfigFile(false, val);
                using (var configRefresher = new SelfDiagnosticsConfigRefresher())
                {
                    // Emitting event of EventLevel.Error
                    CoreEventSource.Log.InvalidOperationToStopError();
                    var filePath = configRefresher.CurrentFilePath;

                    int bufferSize = 512;
                    byte[] actualBytes = ReadFile(filePath, bufferSize);
                    string logText = Encoding.UTF8.GetString(actualBytes);
                    Assert.IsTrue(logText.StartsWith(MessageOnNewFileString));

                    // The event was captured
                    string logLine = logText.Substring(MessageOnNewFileString.Length);
                    string logMessage = ParseLogMessage(logLine);
                }
            }
            finally
            {
                Environment.SetEnvironmentVariable(key, null);
                Platform.PlatformSingleton.Current = null; // Force reinitialization in future tests so that new environment variables will be loaded.
                CleanupConfigFile();
            }
        {
            int timestampPrefixLength = "2020-08-14T20:33:24.4788109Z:".Length;
            Assert.IsTrue(TimeStringRegex.IsMatch(logLine.Substring(0, timestampPrefixLength)));
            return logLine.Substring(timestampPrefixLength);
        }

        private static byte[] ReadFile(string filePath, int byteCount)
        {
            using (var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
            {
                FileSize = 1024,
                LogLevel = "Error"
            };

            if (userDefinedLogDirectory)
            {
                configFileObj.LogDirectory = ".";
            }
            else


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
