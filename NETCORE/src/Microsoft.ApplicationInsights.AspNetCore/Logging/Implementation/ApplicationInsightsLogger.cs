namespace Microsoft.ApplicationInsights.AspNetCore.Logging
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.Extensions.Logging;

#pragma warning disable CS0618 // ApplicationInsightsLoggerOptions is obsolete. This will not be fixed because this class is also obsolete.
    /// <summary>
    /// <see cref="ILogger"/> implementation that forwards log messages as Application Insight trace events.
    /// </summary>
    [SuppressMessage("Documentation Rules", "SA1614:ElementParameterDocumentationMustHaveText", Justification = "This class is obsolete and will not be completely documented.")]
    internal class ApplicationInsightsLogger : ILogger
    {
#if NETFRAMEWORK
        /// <summary>
        /// SDK Version Prefix.
        /// </summary>
        public const string VersionPrefix = "ilf:";
#else
        /// <summary>
        /// </summary>
        public const string VersionPrefix = "ilc:";
#endif

        private readonly string categoryName;
        private readonly TelemetryClient telemetryClient;
        private readonly Func<string, LogLevel, bool> filter;
        private readonly ApplicationInsightsLoggerOptions options;
        private readonly string sdkVersion = SdkVersionUtils.GetVersion(VersionPrefix);

            this.options = options;
                if (exception == null || this.options?.TrackExceptionsAsExceptionTelemetry == false)
                {
                    var traceTelemetry = new TraceTelemetry(formatter(state, exception), GetSeverityLevel(logLevel));
                    this.PopulateTelemetry(traceTelemetry, stateDictionary, eventId);
                    this.telemetryClient.TrackTrace(traceTelemetry);
                }
                else
                {
                    var exceptionTelemetry = new ExceptionTelemetry(exception);
                    exceptionTelemetry.Message = formatter(state, exception);
                case LogLevel.Information:
                    return SeverityLevel.Information;
                case LogLevel.Debug:
                case LogLevel.Trace:
                default:
                    return SeverityLevel.Verbose;
            }
        }

        private void PopulateTelemetry(ITelemetry telemetry, IReadOnlyList<KeyValuePair<string, object>> stateDictionary, EventId eventId)
        {
            if (telemetry is ISupportProperties telemetryWithProperties)
            {
                IDictionary<string, string> dict = telemetryWithProperties.Properties;
                dict["CategoryName"] = this.categoryName;

                if (this.options?.IncludeEventId ?? false)
                {
                    if (eventId.Id != 0)
                    {
                if (stateDictionary != null)
                {
                    foreach (KeyValuePair<string, object> item in stateDictionary)
                    {
                        dict[item.Key] = Convert.ToString(item.Value, CultureInfo.InvariantCulture);
                    }
                }
            }

            telemetry.Context.GetInternalContext().SdkVersion = this.sdkVersion;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
