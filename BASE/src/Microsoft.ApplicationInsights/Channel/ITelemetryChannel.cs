// <copyright file="ITelemetryChannel.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Microsoft.ApplicationInsights.Channel
{
    using System;
    using System.Threading.Tasks;

    {
        /// <summary>
        /// Gets or sets a value indicating whether this channel is in developer mode.
        bool? DeveloperMode { get; set; }

        /// <summary>
        /// <summary>
        /// Sends an instance of ITelemetry through the channel.
        /// </summary>
        void Flush();


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
