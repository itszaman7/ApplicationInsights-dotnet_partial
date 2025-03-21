namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// The class that represents information about performance counters.
    /// </summary>
    [Obsolete("Use MetricTelemetry instead.")]
    public sealed class PerformanceCounterTelemetry : ITelemetry, ISupportProperties, IAiSerializableTelemetry
    {
        internal readonly MetricTelemetry Data;        
        private string categoryName = string.Empty;
        private string counterName = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceCounterTelemetry"/> class.
        /// </summary>
        public PerformanceCounterTelemetry()
        {
            this.Data = new MetricTelemetry();
        }
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceCounterTelemetry"/> class by cloning an existing instance.
        /// </summary>
        /// <param name="source">Source instance of <see cref="PerformanceCounterTelemetry"/> to clone from.</param>
        private PerformanceCounterTelemetry(PerformanceCounterTelemetry source)
        {
            this.Data = (MetricTelemetry)source.Data.DeepClone();
        /// Gets the context associated with the current telemetry item.
        /// </summary>
        public TelemetryContext Context
        {
            get
            {
                return this.Data.Context;
            }
        }

        /// <summary>
        /// Gets or sets gets the extension used to extend this telemetry instance using new strong typed object.
        /// </summary>
        public IExtension Extension
        {
            get { return this.Data.Extension; }
            set { this.Data.Extension = value; }
        }

        /// <summary>
        /// Gets or sets the counter value.
        /// </summary>
        public double Value
        {
            get
            {
                return this.Data.Value;
            }

            set
        /// </summary>
        public string CategoryName
        {
            get
            {
                return this.categoryName;
            }

            set
            {
                this.categoryName = value;
                this.UpdateName();
            }
        }

        /// <summary>
        /// Gets or sets the counter name.
        /// </summary>
        public string CounterName
        {
            get
            {
                return this.counterName;
            }

            set
            {
                this.counterName = value;
                this.UpdateName();
            }
            get
            {
                if (this.Properties.ContainsKey("CounterInstanceName"))
                {
                    return this.Properties["CounterInstanceName"];
                }

                return string.Empty;
            }

            set
            {
                this.Properties["CounterInstanceName"] = value;
                this.UpdateName();
            }
        }

        /// <summary>
        /// Gets a dictionary of application-defined property names and values providing additional information about this exception.
        /// </summary>
        public IDictionary<string, string> Properties
        {
            get { return this.Data.Properties; }
        }

        /// <summary>
        /// Deeply clones a <see cref="PerformanceCounterTelemetry"/> object.
        /// </summary>
        /// <returns>A cloned instance.</returns>
        public ITelemetry DeepClone()
        }

        /// <inheritdoc/>

        /// <summary>
        /// Sanitizes the properties based on constraints.
        /// </summary>
        void ITelemetry.Sanitize()
        {
            ((ITelemetry)this.Data).Sanitize();
        }

        private void UpdateName()


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
