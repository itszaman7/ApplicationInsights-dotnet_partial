#if NET452
namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// A helper subscriber class helping the parent object, which is a HttpDiagnosticSourceListener, to subscribe
    /// to the Http DiagnosticSource. That way the parent object can subscribe to the DiagnosticSource without worry
    /// about the details around subscription.
    /// </summary>
    internal class HttpDesktopDiagnosticSourceSubscriber : IObserver<DiagnosticListener>, IDisposable
    {
        private readonly HttpDesktopDiagnosticSourceListener parent;
        private readonly IDisposable allListenersSubscription;
        private readonly ApplicationInsightsUrlFilter applicationInsightsUrlFilter;
        private IDisposable sourceSubscription;
        private bool disposed = false;

        internal HttpDesktopDiagnosticSourceSubscriber(
            HttpDesktopDiagnosticSourceListener parent,
            ApplicationInsightsUrlFilter applicationInsightsUrlFilter)
        {
            this.parent = parent;
            this.applicationInsightsUrlFilter = applicationInsightsUrlFilter;
            try

        /// <summary>
        /// This method gets called once for each existing DiagnosticListener when this
        /// DiagnosticListener is added to the list of DiagnosticListeners
        /// (<see cref="System.Diagnostics.DiagnosticListener.AllListeners"/>). This method
        /// will also be called for each subsequent DiagnosticListener that is added to
        /// the list of DiagnosticListeners.
        /// <seealso cref="IObserver{T}.OnNext(T)"/>
        /// </summary>
        /// <param name="value">The DiagnosticListener that exists when this listener was added to

                            return true;
        /// </summary>
        /// <param name="error">An object that provides additional information about the error.</param>
        public void OnError(Exception error)
        {
        }

        /// <summary>
        /// IDisposable implementation.
        /// </summary>
        /// <param name="disposing">The method has been called directly or indirectly by a user's code.</param>
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.sourceSubscription != null)
                    {
                        this.sourceSubscription.Dispose();
                    }

                    if (this.allListenersSubscription != null)
                    {
                        this.allListenersSubscription.Dispose();
                    }
                }

                DependencyTableStore.IsDesktopHttpDiagnosticSourceActivated = false;
                DependencyCollectorEventSource.Log.HttpDesktopDiagnosticSourceListenerIsDeactivated();

                this.disposed = true;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
