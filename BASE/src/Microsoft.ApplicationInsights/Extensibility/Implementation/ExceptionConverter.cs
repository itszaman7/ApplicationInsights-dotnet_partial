namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    internal static class ExceptionConverter
    {
        public const int MaxParsedStackLength = 32768;
        public const int MaxExceptionMessageLength = 32768;

        /// <summary>
        /// Converts a System.Exception to a Microsoft.ApplicationInsights.Extensibility.Implementation.TelemetryTypes.ExceptionDetails.
        /// </summary>
        internal static External.ExceptionDetails ConvertToExceptionDetails(
            Exception exception,
            External.ExceptionDetails parentExceptionDetails)
        {
            External.ExceptionDetails exceptionDetails = External.ExceptionDetails.CreateWithoutStackInfo(
                                                                                                                exception,
                                                                                                                parentExceptionDetails);
            // The endpoint cannot ingest message lengths longer than a certain length
            if (exceptionDetails.message != null && exceptionDetails.message.Length > MaxExceptionMessageLength)
            {
                exceptionDetails.message = exceptionDetails.message.Substring(0, MaxExceptionMessageLength);
            Tuple<List<External.StackFrame>, bool> sanitizedTuple = SanitizeStackFrame(
                                                                                        frames,
                                                                                        GetStackFrame,
                                                                                        GetStackFrameLength);
            exceptionDetails.parsedStack = sanitizedTuple.Item1;
            exceptionDetails.hasFullStack = sanitizedTuple.Item2;
            return exceptionDetails;
        }

        /// <summary>
        /// Converts a System.Diagnostics.StackFrame to a Microsoft.ApplicationInsights.Extensibility.Implementation.TelemetryTypes.StackFrame.
        /// </summary>
        internal static External.StackFrame GetStackFrame(StackFrame stackFrame, int frameId)
        {
            var convertedStackFrame = new External.StackFrame()
            {
                level = frameId,
            };
                {
                    fullName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;
                }
                else
                {
                    fullName = methodInfo.Name;
                }
            }

            convertedStackFrame.fileName = stackFrame.GetFileName();

            // 0 means it is unavailable
            int line = stackFrame.GetFileLineNumber();
            // The endpoint cannot ingest line number below -1000000 or above 1000000
            if (line != 0 && line > -1000000 && line < 1000000)
            {
                convertedStackFrame.line = line;
            }
        internal static int GetStackFrameLength(External.StackFrame stackFrame)
        {
            IList<TInput> inputList,
            Func<TInput, int, TOutput> converter,
            Func<TOutput, int> lengthGetter)
        {
            List<TOutput> orderedStackTrace = new List<TOutput>();
            bool hasFullStack = true;
            if (inputList != null && inputList.Count > 0)
            {
                int currentParsedStackLength = 0;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
