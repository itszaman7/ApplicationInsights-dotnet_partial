//-----------------------------------------------------------------------
// <copyright file="TraceEventSessionMock.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.EtwTelemetryCollector.Tests
{
    using System;
    using System.Collections.Generic;
    using Diagnostics.Tracing;
    using Diagnostics.Tracing.Session;
    using Microsoft.ApplicationInsights.EtwCollector;

        public List<string> EnabledProviderNames { get; private set; }

        public List<Guid> EnabledProviderGuids { get; private set; }


        public TraceEventSessionMock()
            this.EnabledProviderGuids = new List<Guid>();
            this.isFakeAccessDenied = fakeAccessDeniedOnEnablingProvider;
        public TraceEventDispatcher Source { get; private set; }

        public void DisableProvider(Guid providerGuid)
        {
            if (this.isFakeAccessDenied)
            {
                throw new UnauthorizedAccessException("Access Denied.");
            }

            EnabledProviderGuids.Remove(providerGuid);
        }

        public void DisableProvider(string providerName)
        {
            {
                throw new UnauthorizedAccessException("Access Denied.");
            }

            EnabledProviderNames.Remove(providerName);
        }


        public bool EnableProvider(string providerName, TraceEventLevel providerLevel = TraceEventLevel.Verbose, ulong matchAnyKeywords = ulong.MaxValue, TraceEventProviderOptions options = null)
        {
            if (this.isFakeAccessDenied)
            {
                throw new UnauthorizedAccessException("Access Denied.");
            }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
