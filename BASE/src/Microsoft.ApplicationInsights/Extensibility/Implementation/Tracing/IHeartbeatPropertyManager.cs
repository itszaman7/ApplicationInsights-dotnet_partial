namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines an implementation for management of the heartbeat feature of the 
    /// Application Insights SDK meant for public consumption. Add/Set properties, 
    /// disable/enable the heartbeat, and set the interval between heartbeat pulses.
    /// heartbeat feature can be extended or configured as necessary.
    /// </remarks>
    /// </summary>
    public interface IHeartbeatPropertyManager
    {

        /// <summary>
        /// Gets a list of default heartbeat property providers that are disabled and will not contribute to the
        /// <summary>
        /// Gets a list of property names that are not to be sent with the heartbeats.
        /// </summary>
        IList<string> ExcludedHeartbeatProperties { get; }

        /// <summary>
        /// </summary>
        bool AddHeartbeatProperty(string propertyName, string propertyValue, bool isHealthy);



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
