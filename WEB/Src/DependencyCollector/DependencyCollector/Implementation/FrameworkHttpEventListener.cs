#if NET452
namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using System.Diagnostics.Tracing;
    using System.Globalization;

    /// <summary>
    /// Provides methods for listening to events from FrameworkEventSource for HTTP.
    /// </summary>
    internal class FrameworkHttpEventListener : EventListener
    {
        /// <summary>
        /// The Http processor.
        /// </summary>
        internal readonly FrameworkHttpProcessing HttpProcessingFramework;

        /// <summary>
        /// The Framework EventSource name. 
        /// </summary>
        private const string FrameworkEventSourceName = "System.Diagnostics.Eventing.FrameworkEventSource";

        /// <summary>
        /// BeginGetResponse Event ID.
        /// </summary>
        /// BeginGetRequestStream Event ID.
        /// </summary>
        private const int BeginGetRequestStreamEventId = 142;

        /// <summary>
        /// EndGetRequestStream Event ID.
        /// </summary>
        private const int EndGetRequestStreamEventId = 143;

        internal FrameworkHttpEventListener(FrameworkHttpProcessing frameworkHttpProcessing)
        {
            this.HttpProcessingFramework = frameworkHttpProcessing;
        }

        /// <summary>
        /// Enables HTTP event source when EventSource is created. Called for all existing 
        /// event sources when the event listener is created and when a new event source is attached to the listener.
        /// </summary>
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
                DependencyCollectorEventSource.Log.RemoteDependencyModuleVerbose("HttpEventListener initialized for event source:" + FrameworkEventSourceName);
            }

            base.OnEventSourceCreated(eventSource);
        }

        /// <summary>
        /// Called whenever an event has been written by an event source for which the event listener has enabled events.
        /// </summary>
                    case BeginGetRequestStreamEventId:
                        if (!DependencyTableStore.IsDesktopHttpDiagnosticSourceActivated)
                        {
                            // request is handled by Desktop DiagnosticSource Listener
                            this.OnBeginGetRequestStream(eventData);
                        }

                        break;
                    case EndGetRequestStreamEventId:
                        break;
        /// <param name="eventData">The event arguments that describe the event.</param>
        private void OnEndGetResponse(EventWrittenEventArgs eventData)
        {
            if (eventData.Payload.Count >= 1)
            {
                long id = Convert.ToInt64(eventData.Payload[0], CultureInfo.InvariantCulture);

                int? statusCode = null;

                // .NET 4.6 onwards will be passing the following additional params.
                else
                {
                    // This case is for .NET 4.5.1-4.5.2
                }

                this.HttpProcessingFramework?.OnEndHttpCallback(id, statusCode);
            }
        }

        /// <summary>
        /// Called when a postfix of a (HttpWebRequest|FileWebRequest|FtpWebRequest).BeginGetRequestStream method has been invoked.
        /// </summary>
        /// <param name="eventData">The event arguments that describe the event.</param>
        private void OnBeginGetRequestStream(EventWrittenEventArgs eventData)
        {
            if (eventData.Payload.Count >= 2)
            {
                long id = Convert.ToInt64(eventData.Payload[0], CultureInfo.InvariantCulture);
                string uri = Convert.ToString(eventData.Payload[1], CultureInfo.InvariantCulture);



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
