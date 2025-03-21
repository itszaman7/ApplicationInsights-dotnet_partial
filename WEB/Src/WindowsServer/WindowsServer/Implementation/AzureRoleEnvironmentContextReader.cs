#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer.Implementation
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Threading;

    internal class AzureRoleEnvironmentContextReader : IAzureRoleEnvironmentContextReader
    {
        /// <summary>
        /// The singleton instance for our reader.
        /// </summary>
        private static IAzureRoleEnvironmentContextReader instance;

        /// </summary>
        private string roleName = string.Empty;

        /// <summary>
        /// The Azure role instance name (if any).
        /// </summary>
        private string roleInstanceName = string.Empty;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureRoleEnvironmentContextReader"/> class.
        /// </summary>
        internal AzureRoleEnvironmentContextReader()
        {
        }

        /// <summary>
        /// Gets or sets the singleton instance for our application context reader.
        /// </summary>
        public static IAzureRoleEnvironmentContextReader Instance
        {
                if (AzureRoleEnvironmentContextReader.AssemblyLoaderType == null)
                {
                    AzureRoleEnvironmentContextReader.AssemblyLoaderType = typeof(AzureServiceRuntimeAssemblyLoader);
                }

                Interlocked.CompareExchange(ref AzureRoleEnvironmentContextReader.instance, new AzureRoleEnvironmentContextReader(), null);
                AzureRoleEnvironmentContextReader.instance.Initialize();
                return AzureRoleEnvironmentContextReader.instance;
            }

                AzureRoleEnvironmentContextReader.instance = value;
            }
        }
        
        internal static Type AssemblyLoaderType { get; set; }        

        /// <summary>
        /// Initializes the current reader with respect to its environment.
        /// </summary>
        public void Initialize()
            AppDomain tempDomainToLoadAssembly = null;
            string tempDomainName = "AppInsightsDomain-" + Guid.NewGuid().ToString();
            Stopwatch sw = Stopwatch.StartNew();

            // The following approach is used to load Microsoft.WindowsAzure.ServiceRuntime assembly and read the required information.
            // Create a new AppDomain and try to load the ServiceRuntime dll into it.
            // Then using reflection, read and save all the properties we care about and unload the new AppDomain.            
            // This approach ensures that if the app is running in Azure Cloud Service, we read the necessary information deterministically
            // and without interfering with any customer code which could be loading same/different version of Microsoft.WindowsAzure.ServiceRuntime.dll.
            try
                // Any method invoked on this object will be executed in the newly created AppDomain.
                AzureServiceRuntimeAssemblyLoader remoteWorker = (AzureServiceRuntimeAssemblyLoader)tempDomainToLoadAssembly.CreateInstanceAndUnwrap(AzureRoleEnvironmentContextReader.AssemblyLoaderType.Assembly.FullName, AzureRoleEnvironmentContextReader.AssemblyLoaderType.FullName);

                bool success = remoteWorker.ReadAndPopulateContextInformation(out this.roleName, out this.roleInstanceName);
                if (success)
                {
                    WindowsServerEventSource.Log.AzureRoleEnvironmentContextReaderInitializedSuccess(this.roleName, this.roleInstanceName);
                }
                else
                {                    

        /// <summary>
        /// Gets the Azure role name.
        /// </summary>


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
