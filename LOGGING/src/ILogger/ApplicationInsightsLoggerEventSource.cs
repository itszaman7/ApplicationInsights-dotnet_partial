// -----------------------------------------------------------------------
// <copyright file="ApplicationInsightsLoggerEventSource.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. 
// All rights reserved.  2013
// </copyright>
// -----------------------------------------------------------------------

    /// <summary>
    /// EventSource for reporting errors and warnings from Logging module.
    /// </summary>
    [EventSource(Name = "Redfield-Microsoft-ApplicationInsights-LoggerProvider")]
#else
    [EventSource(Name = "Microsoft-ApplicationInsights-LoggerProvider")]
#endif
    internal sealed class ApplicationInsightsLoggerEventSource : EventSource
    {
        [Event(1, Message = "Sending log to ApplicationInsightsLoggerProvider has failed. Error: {0}", Level = EventLevel.Error)]
        public void FailedToLog(string error, string applicationName = null) => this.WriteEvent(1, error, applicationName ?? this.ApplicationName);

        {
            try
            {
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
