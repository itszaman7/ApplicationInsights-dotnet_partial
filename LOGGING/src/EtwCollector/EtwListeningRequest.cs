//-----------------------------------------------------------------------
// <copyright file="EtwListeningRequest.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

    using Microsoft.Diagnostics.Tracing;

    /// <summary>
    /// Represents a request to listen to specific ETW provider.
    /// </summary>
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// Gets or sets the guid of the provider to listen to.
        /// <returns>True if the request is valid, otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Throws when the object is not valid.</exception>
        public bool Validate(out string errorMessage)
        {
            if (this.ProviderGuid == Guid.Empty && string.IsNullOrEmpty(this.ProviderName))
            {
                errorMessage = "ProviderGuid and ProviderName can't be null at the same time.";
                return false;
            }

            errorMessage = null;
            return true;
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
