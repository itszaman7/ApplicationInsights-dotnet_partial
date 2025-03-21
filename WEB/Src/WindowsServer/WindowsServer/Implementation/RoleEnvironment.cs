#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer.Implementation
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Provides information about the configuration, endpoints, and status of running role instances. 
    /// </summary>
    internal class RoleEnvironment : RuntimeBindingObject
        public RoleEnvironment(Assembly loadedAssembly)
            : base(loadedAssembly.GetType("Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironment", false), loadedAssembly)
        {            
        }

        /// <summary>
        /// Gets a value indicating whether the role instance is running in the Windows Azure environment. 
        /// </summary>
                try
                {
                    return (bool)this.GetProperty("IsAvailable");
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the unique identifier of the deployment in which the role instance is running. 
        /// </summary>
        public string DeploymentId
        {
                return new RoleInstance(currentRoleInstance, this.LoadedAssembly);
            }
        }

        /// <summary>
        /// Gets the target object instance.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        protected override object GetTargetObjectInstance(Type targetType, object[] activationArgs)
        {
            // RoleEnvironment is a "static" object in the Azure Runtime. As such, no activation is required.
            return null;
        }
    }
}
#endif

# This file contains partial code from the original project
# Some functionality may be missing or incomplete
