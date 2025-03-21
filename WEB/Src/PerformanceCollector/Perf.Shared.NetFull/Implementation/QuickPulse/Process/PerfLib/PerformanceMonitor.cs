namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.PerfLib
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;
    using System.Security.Permissions;
    using System.Threading;

    using Microsoft.Win32;

    /// <summary>
    /// Represents the low-level performance monitor.
    /// </summary>
    internal class PerformanceMonitor
    {
        private RegistryKey perfDataKey;


        /// we wait may not be sufficient if the Win32 code keeps running into this deadlock again 
        /// and again. A condition very rare but possible in theory. We would get back to the user 
        /// in this case with InvalidOperationException after the wait time expires.
        /// </summary>
        public byte[] GetData(string categoryIndex)
        {
            int waitRetries = 3; // 17;   //2^16*10ms == approximately 10mins
            int waitSleep = 0;
            int error = 0;

            // no need to revert here since we'll fall off the end of the method
            new RegistryPermission(PermissionState.Unrestricted).Assert();
            while (waitRetries > 0)
            {
                try
                {
                    return (byte[])this.perfDataKey.GetValue(categoryIndex);
                }
                    switch (error)
                    {
                        case NativeMethods.RPC_S_CALL_FAILED:
                        case NativeMethods.ERROR_INVALID_HANDLE:
                        case NativeMethods.RPC_S_SERVER_UNAVAILABLE:
                            this.Init();
                            goto case NativeMethods.WAIT_TIMEOUT;

                        case NativeMethods.WAIT_TIMEOUT:
                        case NativeMethods.ERROR_NOT_READY:
                        case NativeMethods.ERROR_LOCK_FAILED:
                        case NativeMethods.ERROR_BUSY:
                            --waitRetries;
                            if (waitSleep == 0)
                            {
                                waitSleep = 10;
                            }
                            else
            throw SharedUtils.CreateSafeWin32Exception(error);
        }

        [ResourceExposure(ResourceScope.None)]
        [ResourceConsumption(ResourceScope.Machine, ResourceScope.Machine)]
        private void Init()
        {
            try
            {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
