//-----------------------------------------------------------------------
// <copyright file="AITraceEventSession.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.EtwCollector
{
    using System;
    using Microsoft.Diagnostics.Tracing;
    using Microsoft.Diagnostics.Tracing.Session;

    /// <summary>
    internal sealed class AITraceEventSession : ITraceEventSession, IDisposable
    {
        private TraceEventSession session;

        public AITraceEventSession(TraceEventSession traceEventSession)
        {
        public static bool? IsElevated()
        {
            return TraceEventSession.IsElevated();
        }

        public void DisableProvider(string providerName)
        {
            this.session.DisableProvider(providerName);
        }
        {
            this.session.Dispose();
        }

        public bool EnableProvider(Guid providerGuid, TraceEventLevel providerLevel = TraceEventLevel.Verbose, ulong matchAnyKeywords = ulong.MaxValue, TraceEventProviderOptions options = null)
        {
            return this.session.EnableProvider(providerName, providerLevel, matchAnyKeywords, options);
        }

        public bool Stop(bool noThrow = false)
        {
            return this.session.Stop(noThrow);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
