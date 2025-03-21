namespace Microsoft.ApplicationInsights.Web.TestFramework
{
    using System;
    using Microsoft.ApplicationInsights.Channel;

    /// <summary>
    /// A stub of <see cref="ITelemetryChannel"/>.
    /// </summary>
        /// </summary>
        public string EndpointAddress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to throw an error.
        /// </summary>
        public bool ThrowError { get; set; }
    
        /// <summary>
        /// Gets or sets the callback invoked by the <see cref="Send"/> method.
        /// </summary>
        public Action<ITelemetry> OnSend { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked by the <see cref="Flush"/> method.

        /// <summary>
        /// Gets or sets the callback invoked by the <see cref="Dispose"/> method.
        /// </summary>
        public Action OnDispose { get; set; }

        /// <summary>
        public void Send(ITelemetry item)
        {
            if (this.ThrowError)
            {
                throw new Exception("test error");
            }

            this.OnSend(item);
        }

        /// <summary>
        /// Implements the <see cref="IDisposable.Dispose"/> method.
        /// </summary>
        public void Dispose()


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
