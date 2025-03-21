// -----------------------------------------------------------------------
// <copyright file="ApplicationInsightsLoggerProvider.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. 
// All rights reserved.  2013
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Extensions.Logging.ApplicationInsights
{
    using System;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Implementation;
    using Microsoft.Extensions.Options;

        /// The application insights logger options.
        /// </summary>
        private readonly ApplicationInsightsLoggerOptions applicationInsightsLoggerOptions;
        /// <exception cref="System.ArgumentNullException">
        /// telemetryConfiguration
        /// or
        /// loggingFilter
        /// or
        /// applicationInsightsLoggerOptions.
        /// </exception>
        public ApplicationInsightsLoggerProvider(
            IOptions<TelemetryConfiguration> telemetryConfigurationOptions,
            IOptions<ApplicationInsightsLoggerOptions> applicationInsightsLoggerOptions)
        {
            if (telemetryConfigurationOptions?.Value == null)
            {
                throw new ArgumentNullException(nameof(telemetryConfigurationOptions));
            }

            this.applicationInsightsLoggerOptions = applicationInsightsLoggerOptions?.Value ?? throw new ArgumentNullException(nameof(applicationInsightsLoggerOptions));

        /// <summary>
        /// Creates a new <see cref="ILogger" /> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns>An <see cref="ILogger"/> instance to be used for logging.</returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new ApplicationInsightsLogger(
                    categoryName,
                    this.applicationInsightsLoggerOptions)
            {
                ExternalScopeProvider = this.externalScopeProvider,
            };
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="releasedManagedResources">Release managed resources.</param>
        protected virtual void Dispose(bool releasedManagedResources)
        {
            if (releasedManagedResources && this.applicationInsightsLoggerOptions.FlushOnDispose)
            {
                this.telemetryClient.Flush();

                // With the ServerTelemetryChannel, Flush pushes buffered telemetry to the Transmitter,
                // but it doesn't guarantee that all events have been transmitted to the endpoint.


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
