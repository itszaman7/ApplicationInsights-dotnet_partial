//-----------------------------------------------------------------------
// <copyright file="OtherTestEventSource.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


    [EventSource(Name = OtherTestEventSource.ProviderName)]
    {
        public const string ProviderName = "Microsoft-ApplicationInsights-Extensibility-EventSourceListener-Tests-Other";
        [Event(3, Level = EventLevel.Informational, Message = "{0}")]
        public void Message(string message)


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
