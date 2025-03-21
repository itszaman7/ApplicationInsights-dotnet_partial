namespace Microsoft.ApplicationInsights.Channel
{
    using System;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
        IExtension Extension { get; set; }

        /// <summary>
        /// Gets or sets the value that defines absolute order of the telemetry item.
        /// </summary>
        /// <remarks>
        /// The sequence is used to track absolute order of uploaded telemetry items. It is a two-part value that includes 
        /// a stable identifier for the current boot session and an incrementing identifier for each event added to the upload queue:
        /// For UTC this would increment for all events across the system.
        /// For Persistence this would increment for all events emitted from the hosting process.    
        /// From <a href="https://microsoft.sharepoint.com/teams/CommonSchema/Shared%20Documents/Schema%20Specs/Common%20Schema%202%20-%20Language%20Specification.docx"/>.
        /// </remarks>
        string Sequence { get; set; }

        /// <summary>
        /// Sanitizes the properties of the telemetry item based on DP constraints.
        /// </summary>
        void Sanitize();

        /// <summary>
        /// <returns>The cloned object.</returns>
        ITelemetry DeepClone();
        
        /// <summary>
        /// Writes serialization info about the data class of the implementing type using the given <see cref="ISerializationWriter"/>.


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
