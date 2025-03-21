namespace Microsoft.ApplicationInsights.Channel
{
    using System;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    /// Event argument to track response from ingestion endpoint.
    /// </summary>
    public class TransmissionStatusEventArgs : EventArgs
        public TransmissionStatusEventArgs(HttpWebResponseWrapper response) : this(response, default)
        {
        }
        /// <param name="response">Response from ingestion endpoint.</param>
        /// <param name="responseDurationInMs">Response duration in milliseconds.</param>
        public TransmissionStatusEventArgs(HttpWebResponseWrapper response, long responseDurationInMs)
        {
            this.Response = response;
            this.ResponseDurationInMs = responseDurationInMs;

        /// <summary>
        /// Gets the response from ingestion endpoint.
        /// </summary>
        public HttpWebResponseWrapper Response { get; }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
