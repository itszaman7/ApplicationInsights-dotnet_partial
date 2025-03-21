namespace Microsoft.ApplicationInsights.WindowsServer.Channel.Helpers
{
    using System;
    using System.Net.NetworkInformation;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;

    internal class StubNetwork : INetwork
    {
        public Func<bool> OnIsAvailable = () => true;

        public StubNetwork()
        {
        public void AddAddressChangedEventHandler(NetworkAddressChangedEventHandler handler)
        {
            this.OnAddAddressChangedEventHandler(handler);
        }

        public void RemoveAddressChangeEventHandler(NetworkAddressChangedEventHandler handler)
        {
            this.OnRemoveAddressChangedEventHandler(handler);
        }

        public bool IsAvailable()


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
