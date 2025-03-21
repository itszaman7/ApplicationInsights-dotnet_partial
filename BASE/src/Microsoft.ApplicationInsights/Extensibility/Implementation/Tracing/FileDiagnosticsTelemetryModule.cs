namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Tracing;
    using System.IO;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.FileDiagnosticsModule;

    using static System.FormattableString;

    /// <summary>
    /// Diagnostics telemetry module for azure web sites.
    /// </summary>
    public class FileDiagnosticsTelemetryModule : IDisposable, ITelemetryModule
    {
        private readonly TraceSourceForEventSource traceSource = new TraceSourceForEventSource(EventLevel.Error);
        private readonly DefaultTraceListener listener = new DefaultTraceListener();

        private string logFileName;
        private string logFilePath;

        /// <summary>

            this.listener.TraceOutputOptions |= TraceOptions.DateTime | TraceOptions.ProcessId;
            this.traceSource.Listeners.Add(this.listener);
        }

        /// <summary>
        /// Gets or sets diagnostics Telemetry Module LogLevel configuration setting.
        /// </summary>
        public string Severity
        {
        /// <summary>
        /// Gets or sets log file name.
        /// </summary>
        public string LogFileName
        {
            get => this.logFileName;

            set
            {
                if (this.SetAndValidateLogsFolder(this.logFilePath, value))
                {
                    this.logFileName = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets log file path.
        /// </summary>
        public string LogFilePath
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        /// Disposes event listener.
        /// </summary>
        protected virtual void Dispose(bool disposeManaged = true)
        {
            if (disposeManaged)
            {
                this.listener.Dispose();
                this.traceSource.Dispose();
            }
        }
                    FileHelper.TestDirectoryPermissions(logsDirectory);

                    string fullLogFileName = Path.Combine(filePath, fileName);
                    CoreEventSource.Log.LogsFileName(fullLogFileName);

                    // Set
                    this.listener.LogFileName = fullLogFileName;

                    result = true;
                }
                // ArgumentException: // Path does not specify a valid file path or contains invalid DirectoryInfo characters.
                // DirectoryNotFoundException: The specified path is invalid, such as being on an unmapped drive.
                // IOException: The subdirectory cannot be created. -or- A file or directory already has the name specified by path. -or-  The specified path, file name, or both exceed the system-defined maximum length.
                // SecurityException: The caller does not have code access permission to create the directory.

                CoreEventSource.Log.LogStorageAccessDeniedError(
                    error: Invariant($"Path: {this.logFilePath} File: {this.logFileName}; Error: {ex.Message}{Environment.NewLine}"),
                    user: FileHelper.IdentityName);
            }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
