namespace Microsoft.Extensions.Logging
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.ApplicationInsights;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Extension methods for <see cref="ILoggerFactory"/> that allow adding Application Insights logger.
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "SA1614:ElementParameterDocumentationMustHaveText", Justification = "Obsolete class")]
    [SuppressMessage("Microsoft.Usage", "SA1615:ElementReturnValueMustBeDocumented", Justification = "Obsolete class")]
    public static class ApplicationInsightsLoggerFactoryExtensions
    {
        /// <summary>
        }

        /// <summary>
        /// Adds an ApplicationInsights logger that is enabled as defined by the filter function.
        /// </summary>
        /// <param name="factory">Used to configure the logging system and create instances of ILogger from the registered ILoggerProviders.</param>
        /// <param name="serviceProvider">The instance of <see cref="IServiceProvider"/> to use for service resolution.</param>
        public static ILoggerFactory AddApplicationInsights(
            this ILoggerFactory factory,
            IServiceProvider serviceProvider,
            Func<string, LogLevel, bool> filter)
            IServiceProvider serviceProvider,
            Func<string, LogLevel, bool> filter,
            Action loggerAddedCallback)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            var client = serviceProvider.GetService<TelemetryClient>();
            var debugLoggerControl = serviceProvider.GetService<Microsoft.ApplicationInsights.AspNetCore.Logging.ApplicationInsightsLoggerEvents>();
            var options = serviceProvider.GetService<IOptions<Microsoft.ApplicationInsights.AspNetCore.Logging.ApplicationInsightsLoggerOptions>>();

            if (options == null)
            {
                options = Options.Create(new Microsoft.ApplicationInsights.AspNetCore.Logging.ApplicationInsightsLoggerOptions());
            }

            if (debugLoggerControl != null)
            {
                debugLoggerControl.OnLoggerAdded();

                if (loggerAddedCallback != null)
                {
                    debugLoggerControl.LoggerAdded += loggerAddedCallback;
                }
            }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
