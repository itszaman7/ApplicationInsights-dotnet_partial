namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TransmissionPolicy
{
    internal class ApplicationLifecycleTransmissionPolicy : TransmissionPolicy
    {
        private readonly IApplicationLifecycle applicationLifecycle;

        public ApplicationLifecycleTransmissionPolicy(IApplicationLifecycle applicationLifecycle)
        {
        }

        public override void Initialize(Transmitter transmitter)
        {
        private void HandleApplicationStoppingEvent(object sender, ApplicationStoppingEventArgs e)
        {
            this.MaxBufferCapacity = value;
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
