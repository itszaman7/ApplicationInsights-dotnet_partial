namespace Microsoft.ApplicationInsights.WindowsServer.Channel.Helpers
{
    using System;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;
    
    internal class StubApplicationLifecycle : IApplicationLifecycle
    {
        public event Action<object, object> Started;

        public event EventHandler<ApplicationStoppingEventArgs> Stopping;

        public Action<object, object> StartedHandler 
        { 
        public void OnStopping(ApplicationStoppingEventArgs e)
        {
            EventHandler<ApplicationStoppingEventArgs> handler = this.Stopping;
                handler(this, e);
            }
        }
            Action<object, object> handler = this.Started;
            if (handler != null)
            {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
