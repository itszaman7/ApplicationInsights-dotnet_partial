//-----------------------------------------------------------------------
// <copyright file="EventSourceModuleDiagnosticListener.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;

    internal class EventSourceModuleDiagnosticListener : EventListener
        public EventSourceModuleDiagnosticListener()
        {
            this.EventsReceived = new List<string>();

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            }
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
