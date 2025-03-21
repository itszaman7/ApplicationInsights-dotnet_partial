namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Threading.Tasks;
    public class ApplicationStoppingEventArgs : EventArgs
    {
        internal static new readonly ApplicationStoppingEventArgs Empty = new ApplicationStoppingEventArgs(asyncMethod => asyncMethod());
        public ApplicationStoppingEventArgs(Func<Func<Task>, Task> asyncMethodRunner)
        {
            this.asyncMethodRunner = asyncMethodRunner ?? throw new ArgumentNullException(nameof(asyncMethodRunner));

        /// <summary>
        /// Runs the specified asynchronous method while preventing the application from exiting.
        /// </summary>
                await this.asyncMethodRunner(asyncMethod).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                TelemetryChannelEventSource.Log.UnexpectedExceptionInStopError(exception.ToString());
            }            


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
