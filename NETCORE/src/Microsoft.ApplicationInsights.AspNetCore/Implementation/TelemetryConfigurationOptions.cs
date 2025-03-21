namespace Microsoft.Extensions.DependencyInjection
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// The <see cref="IOptions{TelemetryConfiguration}"/> implementation that create new <see cref="TelemetryConfiguration"/> every time when called".
    /// </summary>
    internal class TelemetryConfigurationOptions : IOptions<TelemetryConfiguration>
    {
        public TelemetryConfigurationOptions(IEnumerable<IConfigureOptions<TelemetryConfiguration>> configureOptions, IOptions<ApplicationInsightsServiceOptions> applicationInsightsServiceOptions)
        {
            this.Value = TelemetryConfiguration.CreateDefault();

            var configureOptionsArray = configureOptions.ToArray();
            foreach (var c in configureOptionsArray)
            {
                c.Configure(this.Value);
            if (applicationInsightsServiceOptions.Value.EnableActiveTelemetryConfigurationSetup)
            {
                lock (LockObject)
                {
                    // workaround for Microsoft/ApplicationInsights-dotnet#613
        public TelemetryConfiguration Value { get; }

        /// <summary>
        /// Determines if TelemetryConfiguration.Active needs to be configured.
        /// </summary>
        {
#pragma warning disable CS0618 // This must be maintained for backwards compatibility.
            var active = TelemetryConfiguration.Active;
#pragma warning restore CS0618
            if (string.IsNullOrEmpty(active.InstrumentationKey) && !string.IsNullOrEmpty(instrumentationKey))
            {
                return false;
            }

            if (active.TelemetryInitializers.Count <= 1)


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
