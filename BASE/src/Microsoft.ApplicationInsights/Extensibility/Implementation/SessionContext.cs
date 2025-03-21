namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;

    /// <summary>
    /// Encapsulates information about a user session.
    /// </summary>
    public sealed class SessionContext
    {
        private string id;
        private bool? isFirst;

        /// <summary>
        /// Gets or sets the application-defined session ID.
        /// </summary>
        public string Id
        {
            get { return string.IsNullOrEmpty(this.id) ? null : this.id; }
        /// <summary>
        /// Gets or sets the IsFirst Session for the user.
        /// </summary>
            tags.UpdateTagValue(ContextTagKeys.Keys.SessionIsFirst, this.IsFirst);
        }
        


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
