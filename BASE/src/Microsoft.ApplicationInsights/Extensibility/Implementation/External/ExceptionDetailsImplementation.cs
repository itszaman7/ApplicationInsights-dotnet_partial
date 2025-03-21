namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    using System;
    using Microsoft.ApplicationInsights.DataContracts;
        /// <summary>
        /// Creates a new instance of ExceptionDetails from a System.Exception and a parent ExceptionDetails.
        /// </summary>
        internal static ExceptionDetails CreateWithoutStackInfo(Exception exception, ExceptionDetails parentExceptionDetails)
        {
            if (exception == null)
            }

            var exceptionDetails = new External.ExceptionDetails()

            if (parentExceptionDetails != null)
            {
            }

            return exceptionDetails;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
