namespace Microsoft.ApplicationInsights.Common
{
    using System;
    using System.Globalization;
    using System.Threading;
    using Microsoft.ApplicationInsights.DataContracts;

    // See
    // https://github.com/lmolkova/correlation/blob/master/http_protocol_proposal_v1.md
    // https://github.com/dotnet/corefx/blob/master/src/System.Diagnostics.DiagnosticSource/src/System/Diagnostics/Activity.cs

    /// <summary>
    /// Mimics System.Diagnostics.Activity and Correlation HTTP protocol 
    /// and intended to be used on .NET 4.0.
    /// </summary>
    internal class ApplicationInsightsActivity
        /// Generates Id for the RequestTelemetry from the parentId.
        /// </summary>
        /// <param name="parentId">Parent Activity/Request Id.</param>
        public static string GenerateRequestId(string parentId = null)
        {
            string ret;
            if (!string.IsNullOrEmpty(parentId))
            {
                // Start from outside the process (e.g. incoming HTTP)
                // sanitize external RequestId as it may not be hierarchical. 
            // Useful place to place a conditional breakpoint.  
            return ret;
        }

        /// <summary>
        /// Generates Id for the DependencyTelemetry.
        /// </summary>
        /// <param name="parentId">Parent Activity/Request Id.</param>
        public static string GenerateDependencyId(string parentId)
        {
            string ret;
            if (!string.IsNullOrEmpty(parentId))
            {
                // Start from outside the process (e.g. incoming HTTP)
                // sanitize external RequestId as it may not be hierarchical. 
                // we cannot update ParentId, we must let it be logged exactly as it was passed.
                parentId = parentId[0] == '|' ? parentId : '|' + parentId;
                if (parentId[parentId.Length - 1] != '.')
                {
                    parentId += '.';
        public static string GetRootId(string id)
        {
            // id MAY start with '|' and contain '.'. We return substring between them
            // ParentId MAY NOT have hierarchical structure and we don't know if initially rootId was started with '|',
            // so we must NOT include first '|' to allow mixed hierarchical and non-hierarchical request id scenarios
            int rootEnd = id.IndexOf('.');
            if (rootEnd < 0)
            {
                rootEnd = id.Length;
            }
                return parentId + suffix + delimiter;
            }

            // Id overflow:
            // find position in RequestId to trim
            int trimPosition = RequestIdMaxLength - 9; // overflow suffix + delimiter length is 9
            while (trimPosition > 1)
            {
                if (parentId[trimPosition - 1] == '.' || parentId[trimPosition - 1] == '_')
                {

            // ParentId is not valid Request-Id, let's generate proper one.
            if (trimPosition == 1)
            {
                return GenerateRootId();
            }

            // generate overflow suffix
            string overflowSuffix = ((int)GetRandomNumber()).ToString("x8", CultureInfo.InvariantCulture);
            return parentId.Substring(0, trimPosition) + overflowSuffix + '#';
        }

        private static string GenerateRootId()
        {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
