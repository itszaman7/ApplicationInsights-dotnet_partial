namespace Microsoft.ApplicationInsights.DataContracts
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;

    /// <summary>
    /// Wrapper class for <see cref="ExceptionData"/> that lets user provide exception data without having the actual Exception object.
    /// </summary>
    internal sealed class ExceptionInfo
    {
        private readonly ExceptionData data;
        
        /// <summary>
        /// Constructs the instance of <see cref="ExceptionInfo"/>.
        /// </summary>
        public ExceptionInfo(IEnumerable<ExceptionDetailsInfo> exceptionDetailsInfoList, SeverityLevel? severityLevel, string problemId,
            IDictionary<string, string> properties, IDictionary<string, double> measurements)
        {
            this.data = new ExceptionData
            {
                exceptions = exceptionDetailsInfoList.Select(edi => edi.ExceptionDetails).ToList(),
                severityLevel = severityLevel.TranslateSeverityLevel(),
                problemId = problemId,
                properties = new ConcurrentDictionary<string, string>(properties),
                measurements = new ConcurrentDictionary<string, double>(measurements),
            this.data = data;
        }

        /// <summary>
        /// Gets a list of <see cref="ExceptionDetailsInfo"/> to modify as needed.
        /// </summary>

        /// <summary>
        /// Gets or sets Exception severity level.
        /// </summary>
        public SeverityLevel? SeverityLevel
        {
            set => this.data.properties = value;
        }

        /// <summary>
        /// Gets or sets measurements collection.
        /// </summary>


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
