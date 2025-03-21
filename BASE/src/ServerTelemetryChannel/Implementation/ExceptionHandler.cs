namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Threading.Tasks;
    
    internal static class ExceptionHandler
    {
        /// <summary>
        /// </summary>
        public static void Start(Func<Task> asyncMethod)
        {
            try
                {
                    asyncTask.ContinueWith(
                        task => TelemetryChannelEventSource.Log.ExceptionHandlerStartExceptionWarning(task.Exception.ToString()),
                        TaskContinuationOptions.OnlyOnFaulted);
                    TaskContinuationOptions.OnlyOnFaulted);
            }
            catch (Exception exp)
            {
                TelemetryChannelEventSource.Log.ExceptionHandlerStartExceptionWarning(exp.ToString());
            }
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
