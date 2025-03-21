// -----------------------------------------------------------------------
// <copyright file="ApplicationInsightsTraceFilterTests.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. 
// All rights reserved.  2013
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.TraceListener.Tests
{
    using System;
    using System.Diagnostics;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Tracing.Tests;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Disposing the object on the TestCleanup method")]
    public class ApplicationInsightsTraceFilterTests
            this.adapterHelper = new AdapterHelper();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.adapterHelper.Dispose();
        }
        [TestMethod]
        [TestCategory("TraceListener")]
        public void RespectFilterForWrite()
        {
            this.TraceFilterTestHelper(
                (ApplicationInsightsTraceListener traceListener, TraceEventCache shimTraceEventCache) =>
                    traceListener.Write("message"),
                    false,
        public void TreatWriteAsVerbose()
        {
            this.TraceFilterTestHelper(
                (ApplicationInsightsTraceListener traceListener, TraceEventCache shimTraceEventCache) =>
                    traceListener.Write("message"),
                    true,
                    SourceLevels.Verbose);
        }
            Action<ApplicationInsightsTraceListener, TraceEventCache> callTraceEent,
            bool shouldTrace,
            SourceLevels filterLevel = SourceLevels.Warning)
        {
            TraceEventCache shimTraceEventCache = new TraceEventCache();

            using (var traceListener = new ApplicationInsightsTraceListener(this.adapterHelper.InstrumentationKey))
            {

                traceListener.TelemetryClient = new TelemetryClient(telemetryConfiguration);

                var traceFilter = new EventTypeFilter(filterLevel);
                traceListener.Filter = traceFilter;

                callTraceEent(traceListener, shimTraceEventCache);



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
