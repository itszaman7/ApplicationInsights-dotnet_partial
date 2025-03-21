namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// Telemetry type used to track user sessions.
    /// </summary>
    [Obsolete("Session state events are no longer used. This telemetry item will be sent as EventTelemetry.")]
    public sealed class SessionStateTelemetry : ITelemetry, IAiSerializableTelemetry
    {
        internal readonly EventTelemetry Data;

        private readonly string startEventName = "Session started";
        private readonly string endEventName = "Session ended";

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionStateTelemetry"/> class.
        /// </summary>
        public SessionStateTelemetry()
            : this(SessionState.Start)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionStateTelemetry"/> class with the specified <paramref name="state"/>.
        /// </summary>
        /// <param name="state">
        /// </summary>
        /// <param name="source">Source instance of <see cref="SessionStateTelemetry"/> to clone from.</param>
        private SessionStateTelemetry(SessionStateTelemetry source)
        {
            this.Data = (EventTelemetry)source.Data.DeepClone();
        }

        /// <inheritdoc />
        string IAiSerializableTelemetry.TelemetryName
        {
        {
            get
        /// Gets the <see cref="TelemetryContext"/> of the application when the session state was recorded.
        /// </summary>
        public TelemetryContext Context
        {
            get { return this.Data.Context; }
        }

        /// <summary>
        /// Gets or sets gets the extension used to extend this telemetry instance using new strong typed object.
        /// </summary>
            get
            {
                if (this.Data.Name == this.startEventName)
                {
                    return SessionState.Start;
                }
                else
                {
                    return SessionState.End;
                }
            }

            set
            {
                if (value == SessionState.Start)
                {
                    this.Data.Name = this.startEventName;
                }
                else
                {
        /// <summary>
        /// Deeply clones a <see cref="SessionStateTelemetry"/> object.
        /// </summary>
        /// <returns>A cloned instance.</returns>
        public ITelemetry DeepClone()
        {
            return new SessionStateTelemetry(this);
        }

        /// <summary>
            ((ITelemetry)this.Data).Sanitize();
        }

        /// <inheritdoc/>
        public void SerializeData(ISerializationWriter serializationWriter)
        {
            this.Data.SerializeData(serializationWriter);
        }
    }
}

# This file contains partial code from the original project
# Some functionality may be missing or incomplete
