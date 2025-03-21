// -----------------------------------------------------------------------
// <copyright file="ApplicationInsightsTraceListener.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. 
// All rights reserved.  2013
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.TraceListener
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;

    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Implementation;

    /// <summary>
    /// Listener that routes all tracing and debugging output to ApplicationInsights logging framework.
    /// The messages will be uploaded to the Application Insights cloud service.
    /// </summary>
    public sealed class ApplicationInsightsTraceListener : TraceListener
    {
        /// <summary>
        /// Initializes a new instance of the ApplicationInsightsTraceListener class, without specifying
        /// an instrumentation key.
        /// </summary>
        public ApplicationInsightsTraceListener() : this(string.Empty)
        {
        }


        /// <summary>
        /// Writes trace information, a message, and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A TraceEventCache object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the TraceEventType values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="format">A format string that contains zero or more format items, which correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects to format.</param>
        /// <param name="message">A message to write.</param>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if (this.Filter != null &&
                this.Filter.ShouldTrace(eventCache, source, eventType, id, message, null, null, null) == false)
            {
                return;
            }

            var trace = new TraceTelemetry(message);
            }

            this.TraceData(eventCache, source, eventType, id, new[] { data });
        }

        /// <summary>
        /// Writes trace data to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A TraceEventCache object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
            CreateTraceData(eventType, id, trace);                       
            this.TelemetryClient.Track(trace);
        }

        /// <summary>
        /// Writes the specified message to the listener.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void Write(string message)
        {
            if (this.Filter != null &&
                this.Filter.ShouldTrace(null, string.Empty, TraceEventType.Verbose, 0, message, null, null, null) == false)
            {
                return;
            }

            if (!string.IsNullOrEmpty(message))
            {
                message = message.TrimEnd();
            }

            var trace = new TraceTelemetry(message);
            CreateTraceData(TraceEventType.Verbose, null, trace);
            this.TelemetryClient.Track(trace);
        }

        /// <summary>
        /// Writes the specified message to the listener followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write.</param>
        /// <summary>
        /// Flushes the in-memory buffer.
        /// </summary>
        public override void Flush()
        {
            this.TelemetryClient.Flush();
        }

        private static void CreateTraceData(TraceEventType eventType, int? id, TraceTelemetry trace)
        {
            trace.SeverityLevel = GetSeverityLevel(eventType);
            
            IDictionary<string, string> metaData = trace.Properties;
            
            if (id.HasValue)
            {
                metaData.Add("EventId", id.Value.ToString(CultureInfo.InvariantCulture));
            }
        }

        private static SeverityLevel GetSeverityLevel(TraceEventType eventType)
        {
            // TraceEventType.Resume, TraceEventType.Start, TraceEventType.Stop,
            // TraceEventType.Suspend, TraceEventType.Transfer, TraceEventType.Verbose
            // will fall into default Verbose
            switch (eventType)
            {
                case TraceEventType.Information:
                    return SeverityLevel.Information;
                case TraceEventType.Warning:


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
