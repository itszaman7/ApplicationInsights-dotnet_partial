namespace Microsoft.Extensions.DependencyInjection
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    /// The <see cref="IOptions{TelemetryConfiguration}"/> implementation that create new <see cref="TelemetryConfiguration"/> every time when called".
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "This class is instantiated by Dependency Injection.")]
    internal class TelemetryConfigurationOptions : IOptions<TelemetryConfiguration>
        public TelemetryConfigurationOptions(IEnumerable<IConfigureOptions<TelemetryConfiguration>> configureOptions)
        {
            var configureOptionsArray = configureOptions.ToArray();
            foreach (var c in configureOptionsArray)
            {
                c.Configure(this.Value);

        /// <inheritdoc />
        public TelemetryConfiguration Value { get; }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
