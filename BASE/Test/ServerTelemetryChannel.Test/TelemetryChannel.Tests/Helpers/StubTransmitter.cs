namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers
{
    using System;
    using System.Linq;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Channel.Implementation;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TransmissionPolicy;


        public Func<int, TimeSpan> OnGetBackOffTime = timeInMs => TimeSpan.FromMilliseconds(timeInMs);

        public StubTransmitter()
            : base(new StubTransmissionSender(), new StubTransmissionBuffer(), new StubTransmissionStorage(), TransmissionPolicyCollection.Default, new BackoffLogicManager(TimeSpan.FromMinutes(30)))
        {
            
        public StubTransmitter(BackoffLogicManager backoffLogicManager)
            : base(new StubTransmissionSender(), new StubTransmissionBuffer(), new StubTransmissionStorage(), TransmissionPolicyCollection.Default, backoffLogicManager)
        {

        }

        internal override void Initialize()
        {
            this.OnInitialize();
        }

        internal override void Enqueue(Transmission transmission)


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
