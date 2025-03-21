#if NET452
namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.Operation;

    internal class DependencyTableStore : IDisposable
    {
        internal static bool IsDesktopHttpDiagnosticSourceActivated = false;

        internal bool IsProfilerActivated = false;
        private static readonly DependencyTableStore SingletonInstance = new DependencyTableStore();

        private DependencyTableStore()
        {
            this.WebRequestConditionalHolder = new ObjectInstanceBasedOperationHolder<DependencyTelemetry>();
            this.SqlRequestConditionalHolder = new ObjectInstanceBasedOperationHolder<DependencyTelemetry>();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
