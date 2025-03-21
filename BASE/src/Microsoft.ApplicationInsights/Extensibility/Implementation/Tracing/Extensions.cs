namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Threading;

    /// <summary>
    /// Provides a set of extension methods for tracing.
    {
        /// <summary>
        public static string ToInvariantString(this Exception exception)
        {
            if (exception == null)
            try
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                return exception.ToString();
            finally
            {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
