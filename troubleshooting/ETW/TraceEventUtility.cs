    using System;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.Diagnostics.Tracing;
    using Microsoft.Diagnostics.Tracing.Session;

    /// <summary>
    /// This utility will handle the management of <see cref="TraceEventSession" /> to subscribe to ETW events.
    /// This will create two TraceEventSessions, one to write to an *.etl file and a second to output to console in real-time.
    /// </summary>
    /// <remarks>
    /// <see href="https://github.com/microsoft/perfview/blob/master/documentation/TraceEvent/TraceEventProgrammersGuide.md" />
    /// <see href="https://github.com/microsoft/perfview/blob/master/src/TraceEvent/Samples/22_ObserveEventSource.cs" />
    /// </remarks>
    internal class TraceEventUtility : IDisposable
    {
        private const string TraceFileSessionName = "ApplicationInsights_etlFile_TraceSession";
        private const string TraceConsoleSessionName = "ApplicationInsights_Console_TraceSession";

        private readonly bool shouldLogToFile;
        private readonly bool shouldLogToConsole;
        private readonly string logDirectory;

        private TraceEventSession fileSession;
        private TraceEventSession consoleSession;

                };
                EnableProviders(this.fileSession);
            }

            if (this.shouldLogToConsole)
            {
                // https://blogs.msdn.microsoft.com/vancem/2012/12/20/an-end-to-end-etw-tracing-example-eventsource-and-traceevent/
                this.consoleSession = new TraceEventSession(sessionName: TraceConsoleSessionName)
                {
                    StopOnDispose = true
                };
                EnableProviders(this.consoleSession);
                this.consoleSession.Source.Dynamic.All += this.ConsoleEventHandler;
                Task.Run(() => this.consoleSession.Source.Process()); // Source.Process() will block the thread, so execute on a new thread.
            }
        }

        /// <summary>
        /// Find sessions if they exist and stop them.
        /// </summary>
        public void Stop()
        {
            this.consoleSession?.Stop();
            this.fileSession?.Stop();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Stop();
            this.consoleSession?.Dispose();
            this.fileSession?.Dispose();
        }

        /// <summary>
        /// Subscribe to these providers.
        /// </summary>
        /// <param name="session">Session to enable logging.</param>
        /// <remarks>
        /// These identifiers come from the EventSource attribute on an EventSource class.

        /// <summary>
        /// Event handler for the Console session.
        /// Manifest data is not relevant to the life console so we filter out these messages.
        /// </summary>
            {
                // this runs on a separate thread so cannot use the cmdlet logger (ui thread).
                Console.WriteLine($"{data.TimeStamp.ToLongTimeString()} EVENT: {data.ProviderName} {data.EventName} {data.FormattedMessage ?? string.Empty}");
            }
        }

        private string GetFilePath()
        {
            // TODO: If implement sdk/redfield toggle, should update this name.
            string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
