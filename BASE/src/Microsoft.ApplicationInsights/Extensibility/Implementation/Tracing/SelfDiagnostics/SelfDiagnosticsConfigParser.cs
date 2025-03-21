namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.SelfDiagnostics
{
    using System;
    using System.Diagnostics.Tracing;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Platform;

    internal class SelfDiagnosticsConfigParser
    {
        public const string ConfigFileName = "ApplicationInsightsDiagnostics.json";
        private const int FileSizeLowerLimit = 1024;  // Lower limit for log file size in KB: 1MB
        private const int FileSizeUpperLimit = 128 * 1024;  // Upper limit for log file size in KB: 128MB

        private const string LogDiagnosticsEnvironmentVariable = "APPLICATIONINSIGHTS_LOG_DIAGNOSTICS";

        /// <summary>
        /// ConfigBufferSize is the maximum bytes of config file that will be read.
        /// </summary>
        private const int ConfigBufferSize = 4 * 1024;

        private static readonly Regex LogDirectoryRegex = new Regex(
            @"""LogDirectory""\s*:\s*""(?<LogDirectory>.*?)""", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex FileSizeRegex = new Regex(
                var configFilePath = ConfigFileName;

                // First, check whether the enviornment variable was set.
                if (PlatformSingleton.Current.TryGetEnvironmentVariable(LogDiagnosticsEnvironmentVariable, out string logDiagnosticsPath))
                {
                    configFilePath = Path.Combine(logDiagnosticsPath, ConfigFileName);
                    logDirectory = logDiagnosticsPath;
                }

                // Second, check using current working directory.
                    if (!File.Exists(configFilePath))
                    {
                        return false;
                    }
                }

                using (FileStream file = File.Open(configFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                {
                    var buffer = this.configBuffer;
                    if (buffer == null)
                    {
                        buffer = new byte[ConfigBufferSize]; // Fail silently if OOM
                        this.configBuffer = buffer;
                    }

                    file.Read(buffer, 0, buffer.Length);
                    string configJson = Encoding.UTF8.GetString(buffer);
                    
                    if (logDirectory == null && !TryParseLogDirectory(configJson, out logDirectory))
                    {
            }

            return false;
        }

        internal static bool TryParseLogDirectory(string configJson, out string logDirectory)
        {
            var logDirectoryResult = LogDirectoryRegex.Match(configJson);
            logDirectory = logDirectoryResult.Groups["LogDirectory"].Value;
            return logDirectoryResult.Success && !string.IsNullOrWhiteSpace(logDirectory);
        }

        internal static bool TryParseFileSize(string configJson, out int fileSizeInKB)
        {
            fileSizeInKB = 0;
            var fileSizeResult = FileSizeRegex.Match(configJson);
            return fileSizeResult.Success && int.TryParse(fileSizeResult.Groups["FileSize"].Value, out fileSizeInKB);
        }

        internal static bool TryParseLogLevel(string configJson, out string logLevel)


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
