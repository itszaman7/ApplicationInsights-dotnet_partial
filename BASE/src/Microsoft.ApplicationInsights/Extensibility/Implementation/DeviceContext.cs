namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;

    /// <summary>
    /// Encapsulates information about a device where an application is running.
    /// </summary>
    public sealed class DeviceContext
    {
        private readonly IDictionary<string, string> properties;

        private string type;
        private string id;
        private string operatingSystem;
        private string oemName;
        private string model;

        internal DeviceContext(IDictionary<string, string> properties)
        {
            this.properties = properties;
        }
            set { this.type = value; }
        }
            get { return string.IsNullOrEmpty(this.id) ? null : this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// Gets or sets the operating system name.
        /// </summary>
        public string OperatingSystem
        }

        /// <summary>
        /// Gets or sets the device OEM for the current device.
        /// </summary>
        public string OemName
        {
            get { return string.IsNullOrEmpty(this.oemName) ? null : this.oemName; }
        {
            get { return string.IsNullOrEmpty(this.model) ? null : this.model; }
            set { this.model = value; }
        }

        /// <summary>
        /// Gets or sets the <a href="http://www.iana.org/assignments/ianaiftype-mib/ianaiftype-mib">IANA interface type</a>
        /// for the internet connected network adapter.
            set { this.properties.SetTagValueOrRemove("ai.device.network", value); }
        }

        /// <summary>
        /// Gets or sets the current application screen resolution.
        /// </summary>
        [Obsolete("Use custom properties.")]
        public string ScreenResolution
        {
            get { return this.properties.GetTagValueOrNull("ai.device.screenResolution"); }
            set { this.properties.SetStringValueOrRemove("ai.device.screenResolution", value); }
        }

        /// <summary>
        /// Gets or sets the current display language of the operating system.
        /// </summary>


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
