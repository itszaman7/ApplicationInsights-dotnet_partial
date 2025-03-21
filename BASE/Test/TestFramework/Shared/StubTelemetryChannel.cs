namespace Microsoft.ApplicationInsights.TestFramework
{
    using System;

    using Microsoft.ApplicationInsights.Channel;

    /// <summary>
    /// A stub of <see cref="ITelemetryChannel"/>.
            this.OnFlush = () => { };

        /// <summary>
        /// Gets or sets a value indicating whether this channel is in developer mode.
        /// </summary>
        public bool? DeveloperMode { get; set; }

        /// <summary>
        public int IntegerProperty { get; set; }
    
        /// <summary>
        /// Gets or sets the callback invoked by the <see cref="Send"/> method.
        /// </summary>
        public Action<ITelemetry> OnSend { get; set; }

        public Action OnFlush { get; set; }

        public Action OnDispose { get; set; }

        /// <summary>
        /// Implements the <see cref="ITelemetryChannel.Send"/> method by invoking the <see cref="OnSend"/> callback.
        /// </summary>
        public void Send(ITelemetry item)
        {
            if (this.ThrowError)
            {
                throw new Exception("test error");
            }

        /// <summary>
        /// Implements  the <see cref="ITelemetryChannel.Flush" /> method.
        /// </summary>
        public void Flush()
        {
            this.OnFlush();
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
