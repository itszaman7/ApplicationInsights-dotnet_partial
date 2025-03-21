namespace Microsoft.ApplicationInsights
{
#pragma warning disable CA1034 // "Do not nest type" - part of the public API and too late to change.
    using System;
    using Microsoft.ApplicationInsights.Metrics;

    /// <summary>
    /// Contains constants used to refer to metric dimensions with special significance.
    /// </summary>
    public static class MetricDimensionNames
    {
        /// <summary>
        /// <p>Contains constants used to refer to metric dimensions that will be mapped to fields
        /// within the <see cref="Microsoft.ApplicationInsights.DataContracts.TelemetryContext"/> attached to Application
        /// Insights metric telemetry item that represents <see cref="MetricAggregate" /> objects sent to the Application Insights
        /// cloud ingestion endpoint.</p>
        /// <p>When a metric has a dimension with this a name that equals to one of the constants defined here, the value of that
        /// dimension will not be sent as a regular dimension-value, but as the value of the corresponding field within the telemetry
        /// item's <c>TelemetryContext</c>. Note that this applies only to metrics that belong to Application Insights components and
        /// that are sent via the Application Insights ingestion endpoint.
        /// Metrics sent via other channels are not subject to this transformation.</p>
        /// </summary>
        public static class TelemetryContext
        {
            // Documentation in this file repeatedly refers to the description of the containing class. This will generate a style error. Disable.
#pragma warning disable SA1625  // Element Documentation Must Not Be Copied And Pasted

            /// <summary>See details about the static class <see cref="MetricDimensionNames.TelemetryContext"/> for information about this constant.</summary>
            public const string InstrumentationKey = TelemetryContextPrefix + "InstrumentationKey";

            private const string TelemetryContextPrefix = "TelemetryContext.";
            private const string PropertyPrefix = TelemetryContextPrefix + "Property_";
            private const string PropertyPostfix = "_";

            /// <summary>If you name a metric dimension named <c>propertyName</c> you can apply this method to it
            /// to generate a name that will be moved to TelemetryContext just like its standard fields.</summary>
            /// <param name="propertyName">A metric dimension.</param>
            /// <returns>A dimension name based on the specified <c>propertyName</c> that will be moved to TelemetryContext
            }

            /// <summary>Provides structure for constants defined in <see cref="MetricDimensionNames.TelemetryContext"/>. See there for more info.</summary>
            public static class Cloud
            {
                /// <summary>See details about the static class <see cref="MetricDimensionNames.TelemetryContext"/> for information about this constant.</summary>
                public const string RoleInstance = CloudPrefix + "RoleInstance";

                /// <summary>See details about the static class <see cref="MetricDimensionNames.TelemetryContext"/> for information about this constant.</summary>
                public const string RoleName = CloudPrefix + "RoleName";
                /// <summary>See details about the static class <see cref="MetricDimensionNames.TelemetryContext"/> for information about this constant.</summary>
                public const string Language = DevicePrefix + "Language";

                /// <summary>See details about the static class <see cref="MetricDimensionNames.TelemetryContext"/> for information about this constant.</summary>
                public const string Model = DevicePrefix + "Model";

                /// <summary>See details about the static class <see cref="MetricDimensionNames.TelemetryContext"/> for information about this constant.</summary>
                public const string NetworkType = DevicePrefix + "NetworkType";

                /// <summary>See details about the static class <see cref="MetricDimensionNames.TelemetryContext"/> for information about this constant.</summary>
                public const string OemName = DevicePrefix + "OemName";

                /// <summary>See details about the static class <see cref="MetricDimensionNames.TelemetryContext"/> for information about this constant.</summary>
                public const string OperatingSystem = DevicePrefix + "OperatingSystem";

                /// <summary>See details about the static class <see cref="MetricDimensionNames.TelemetryContext"/> for information about this constant.</summary>
                public const string ScreenResolution = DevicePrefix + "ScreenResolution";

                /// <summary>See details about the static class <see cref="MetricDimensionNames.TelemetryContext"/> for information about this constant.</summary>
                public const string Type = DevicePrefix + "Type";
            }

            /// <summary>Provides structure for constants defined in <see cref="MetricDimensionNames.TelemetryContext"/>. See there for more info.</summary>
            public static class Location
            {
                /// <summary>See details about the static class <see cref="MetricDimensionNames.TelemetryContext"/> for information about this constant.</summary>
                public const string Ip = LocationPrefix + "Ip";

                private const string LocationPrefix = TelemetryContextPrefix + "Location.";
            }

            /// <summary>Provides structure for constants defined in <see cref="MetricDimensionNames.TelemetryContext"/>. See there for more info.</summary>
            public static class Operation
            {
                /// <summary>See details about the static class <see cref="MetricDimensionNames.TelemetryContext"/> for information about this constant.</summary>
                public const string CorrelationVector = OperationPrefix + "CorrelationVector";

                /// <summary>See details about the static class <see cref="MetricDimensionNames.TelemetryContext"/> for information about this constant.</summary>
                public const string Id = OperationPrefix + "Id";

                /// <summary>See details about the static class <see cref="MetricDimensionNames.TelemetryContext"/> for information about this constant.</summary>
                public const string Name = OperationPrefix + "Name";

                /// <summary>See details about the static class <see cref="MetricDimensionNames.TelemetryContext"/> for information about this constant.</summary>
                public const string ParentId = OperationPrefix + "ParentId";

            }

            /// <summary>Provides structure for constants defined in <see cref="MetricDimensionNames.TelemetryContext"/>. See there for more info.</summary>
            public static class User
            {
                /// <summary>See details about the static class <see cref="MetricDimensionNames.TelemetryContext"/> for information about this constant.</summary>
                public const string AccountId = UserPrefix + "AccountId";

                /// <summary>See details about the static class <see cref="MetricDimensionNames.TelemetryContext"/> for information about this constant.</summary>
                public const string AuthenticatedUserId = UserPrefix + "AuthenticatedUserId";


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
