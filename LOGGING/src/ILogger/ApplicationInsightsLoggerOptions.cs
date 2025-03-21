// -----------------------------------------------------------------------
// <copyright file="ApplicationInsightsLoggerOptions.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. 
// All rights reserved.  2013
// </copyright>

namespace Microsoft.Extensions.Logging.ApplicationInsights
{
    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
        /// <summary>
        /// Gets or sets a value indicating whether to track exceptions as <see cref="ExceptionTelemetry"/>.
        /// Defaults to true.
        /// </summary>
        public bool TrackExceptionsAsExceptionTelemetry { get; set; } = true;

        /// Gets or sets a value indicating whether to flush telemetry when disposing
        /// of the logger provider.


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
