namespace Microsoft.ApplicationInsights.AspNetCore.Extensions
{
    using System;

    /// <summary>
    /// Request collection options define the custom behavior or non-default features of request collection.
        /// Initializes a new instance of the <see cref="RequestCollectionOptions"/> class
        /// and populates default values.
        /// </summary>

            // In NetStandard20, ApplicationInsightsLoggerProvider is enabled by default,
            // which captures Exceptions. Disabling it in RequestCollectionModule to avoid duplication.
            this.TrackExceptions = false;
        }

        /// Gets or sets a value indicating whether the Request-Context header is to be injected into the response.
        /// </summary>
        public bool InjectResponseHeaders { get; set; }
        public bool TrackExceptions { get; set; }
        /// </summary>
        [Obsolete("This flag is obsolete and noop. Use System.Diagnostics.Activity.DefaultIdFormat (along with ForceDefaultIdFormat) flags instead.")]
        public bool EnableW3CDistributedTracing { get; set; } = true;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
