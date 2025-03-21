namespace Microsoft.ApplicationInsights.AspNetCore.DiagnosticListeners
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.ApplicationInsights.AspNetCore.DiagnosticListeners.Implementation;
    using Microsoft.ApplicationInsights.AspNetCore.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// <see cref="IApplicationInsightDiagnosticListener"/> implementation that listens for events specific to AspNetCore Mvc layer.
    /// </summary>
    [Obsolete("This class was merged with HostingDiagnosticsListener to optimize Diagnostics Source subscription performance")]
    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Class is obsolete.")]
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "Class is obsolete.")]
    [SuppressMessage("StyleCop Documentation Rules", "SA1611:ElementParametersMustBeDocumented", Justification = "Class is obsolete.")]
    [SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly", Justification = "Class is obsolete.")]
    public class MvcDiagnosticsListener : IApplicationInsightDiagnosticListener
    {
        private readonly PropertyFetcher httpContextFetcher = new PropertyFetcher("httpContext");
        private readonly PropertyFetcher routeDataFetcher = new PropertyFetcher("routeData");
        private readonly PropertyFetcher routeValuesFetcher = new PropertyFetcher("Values");
        /// <inheritdoc />
        public string ListenerName { get; } = "Microsoft.AspNetCore";

        /// <summary>
        /// Diagnostic event handler method for 'Microsoft.AspNetCore.Mvc.BeforeAction' event.
        /// </summary>
        public void OnBeforeAction(HttpContext httpContext, IDictionary<string, object> routeValues)
        {
            if (httpContext == null)
            {
            if (telemetry != null && string.IsNullOrEmpty(telemetry.Name))
        public void OnSubscribe()
        {
        }

        /// <inheritdoc />
        public void OnNext(KeyValuePair<string, object> value)
        {
            try
            {
                if (value.Key == "Microsoft.AspNetCore.Mvc.BeforeAction")
                {
                    var context = this.httpContextFetcher.Fetch(value.Value) as HttpContext;
                    var routeData = this.routeDataFetcher.Fetch(value.Value);
                    var routeValues = this.routeValuesFetcher.Fetch(routeData) as IDictionary<string, object>;

                    if (context != null && routeValues != null)
                    {
                        this.OnBeforeAction(context, routeValues);
                    }
                }
        /// <inheritdoc />
        public void OnError(Exception error)
        {
        }

        /// <inheritdoc />
        public void OnCompleted()
        {
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }

        private static string GetNameFromRouteContext(IDictionary<string, object> routeValues)
        {
            string name = null;

            if (routeValues.Count > 0)
                {
                    object page;
                    routeValues.TryGetValue("page", out page);
                    string pageString = (page == null) ? string.Empty : page.ToString();
                    if (!string.IsNullOrEmpty(pageString))
                    {
                        name = pageString;
                    }
                }
            }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
