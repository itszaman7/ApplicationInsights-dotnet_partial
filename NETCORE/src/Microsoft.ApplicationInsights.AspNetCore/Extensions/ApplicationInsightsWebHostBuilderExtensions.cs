namespace Microsoft.AspNetCore.Hosting
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    public static class ApplicationInsightsWebHostBuilderExtensions
    {
        /// <summary>
        /// Configures <see cref="IWebHostBuilder"/> to use Application Insights services.
        {
            if (webHostBuilder == null)
            {
                throw new ArgumentNullException(nameof(webHostBuilder));
            }

            webHostBuilder.ConfigureServices(collection =>
            {

        /// <returns>The <see cref="IWebHostBuilder"/>.</returns>
        [Obsolete("This method is deprecated in favor of AddApplicationInsightsTelemetry(string instrumentationKey) extension method on IServiceCollection.")]
        public static IWebHostBuilder UseApplicationInsights(this IWebHostBuilder webHostBuilder, string instrumentationKey)
        {
                throw new ArgumentNullException(nameof(webHostBuilder));
            }

            webHostBuilder.ConfigureServices(collection => collection.AddApplicationInsightsTelemetry(instrumentationKey));


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
