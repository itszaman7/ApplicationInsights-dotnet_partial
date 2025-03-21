//-----------------------------------------------------------------------
// <copyright file="EventSourceModuleDiagnosticListener.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
namespace Microsoft.ApplicationInsights.EtwTelemetryCollector.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;

    internal class EventSourceModuleDiagnosticListener : EventListener
        }


        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
#if REDFIELD
#endif   
            {
                EnableEvents(eventSource, EventLevel.LogAlways);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
