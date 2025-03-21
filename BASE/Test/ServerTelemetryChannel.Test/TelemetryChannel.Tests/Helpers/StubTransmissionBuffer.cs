namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;
    using TaskEx = System.Threading.Tasks.Task;

    internal class StubTransmissionBuffer : TransmissionBuffer
    {
        public Func<Transmission> OnDequeue = () => null;
        public Func<int> OnGetCapacity;
        public Action<int> OnSetCapacity;
        public Func<long> OnGetSize = () => 0;

        private int maxNumberOfTransmissions;

        public StubTransmissionBuffer()
        {
        {
            get { return this.OnGetCapacity(); }
            set { this.OnSetCapacity(value); }
        public override long Size
        {
            get { return this.OnGetSize(); }
        public override bool Enqueue(Func<Transmission> getTransmissionAsync)
        {
            return this.OnEnqueue(getTransmissionAsync);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
