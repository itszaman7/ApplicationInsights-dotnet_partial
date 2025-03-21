#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer.Implementation
{
    using System.Reflection;
    /// <summary>
    /// The wrapper for the Azure Service Runtime.
    internal class ServiceRuntime
    {
        private Assembly loadedAssembly;

        /// <summary>
        /// Gets the role environment.
        /// </summary>        
        /// The role environment object.
        /// </returns>
            return new RoleEnvironment(this.loadedAssembly);
        }        


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
