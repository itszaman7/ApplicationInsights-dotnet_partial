#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer.Implementation
{
    using System;
    using System.Globalization;
    using System.Management;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Threading;

    /// <summary>
    /// The reader is platform specific and applies to .NET applications only.
    /// </summary>
    internal class DeviceContextReader
    {
        private static DeviceContextReader instance;
        private string deviceId;
        private string deviceManufacturer;
        private string deviceName;
        private string networkType;

        /// <summary>
        /// Gets or sets the singleton instance for our application context reader.
        /// </summary>
        public static DeviceContextReader Instance
        {
            get
            {
                if (DeviceContextReader.instance != null)
                {
                    return DeviceContextReader.instance;
                }
        /// Gets the host system locale.
        /// </summary>
        /// <returns>The discovered locale.</returns>
        public virtual string GetHostSystemLocale()
        {
            return CultureInfo.CurrentCulture.Name;
        }

        /// <summary>
        /// Gets the type of the device.
        /// Gets the device OEM.
        /// </summary>
        /// <returns>The discovered OEM.</returns>
        public virtual string GetOemName()
        {
            if (this.deviceManufacturer != null)
            {
                return this.deviceManufacturer;
            }

        }

        /// <summary>

            return this.deviceName = RunWmiQuery("Win32_ComputerSystem", "Model", string.Empty);
        }

        /// <summary>
        /// Gets the network type.
        /// </summary>
        /// <returns>The discovered network type.</returns>
        public string GetNetworkType()
        {
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
                    {
                        if (networkInterface.OperationalStatus == OperationalStatus.Up)
                        {
                            this.networkType = networkInterface.NetworkInterfaceType.ToString();
                            return this.networkType;
                        }
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1}", property, table)))
                {
                    foreach (ManagementObject currentObj in searcher.Get())
                    {
                        object data = currentObj[property];
                        if (data != null)
                        {
                }
            }
            catch (Exception exp)
            {
                WindowsServerEventSource.Log.DeviceContextWmiFailureWarning(exp.ToString());
            }

            return defaultValue;
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
