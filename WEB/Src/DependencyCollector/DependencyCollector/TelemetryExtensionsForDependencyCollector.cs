﻿#if NET452
namespace Microsoft.ApplicationInsights.DependencyCollector
{
    using System;
    using System.Data.SqlClient;
    using System.Net;
    using Microsoft.ApplicationInsights.DataContracts;
    public static class TelemetryExtensionsForDependencyCollector
    {
        /// <summary>
        /// When there is an existing telemetry item in the corresponding to the given WEB REQUEST, we return the existing telemetry and associate the same with the WEB REQUEST.
        /// </summary>
        /// <param name="telemetry">Telemetry object that needs to be associated with the web request.</param>
        /// <param name="setCookies">Set cookies enables the process of setting the cookies to the web request. By default it is set to false.</param>
        /// <param name="setCorrelationContext">Set request headers to correlate dependency telemetry item with the request telemetry item that will process this http request.</param>
        /// <returns>Dependency telemetry item with an associated dependency telemetry item.</returns>
        [Obsolete("This method is obsolete. Http dependency tracking, correlation and HTTP header injection are supported out of the box. Use TelemetryClient.StartOperation for manual tracking and correlation", true)]
        public static DependencyTelemetry AssociateTelemetryWithWebRequest(this DependencyTelemetry telemetry, WebRequest webRequest, bool setCookies = false, bool setCorrelationContext = false)
        {
            return telemetry;
        }
        /// </summary>
        /// <param name="telemetry">Telemetry object that needs to be associated with the web request.</param>
        /// <param name="sqlRequest">SQL request object which is used as a key to store in the tables.</param>


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
