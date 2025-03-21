namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.FileDiagnosticsModule
{
    using System;
    using System.Diagnostics.Tracing;

    /// <summary>
    /// EventListener to listen for Application Insights diagnostics messages.
        private readonly EventLevel logLevel;

        private readonly IEventListener sender;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsEventListener" /> class.

        /// <summary>
        /// Gets event log level.
        /// </summary>
        public EventLevel LogLevel
        {
            get
            {
                return this.logLevel;
            }
        }

        /// <param name="eventData">Event to trace.</param>
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            this.sender.OnEventWritten(eventData);
        }

                this.EnableEvents(eventSource, this.logLevel, this.keywords);
            }

            base.OnEventSourceCreated(eventSource);
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
