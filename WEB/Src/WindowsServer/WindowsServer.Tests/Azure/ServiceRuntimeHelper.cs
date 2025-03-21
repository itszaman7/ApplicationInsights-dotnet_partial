#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer.Azure
{
    using System;
    using System.IO;

    using Microsoft.ApplicationInsights.WindowsServer.Azure.Emulation;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation;

            IsAvailable = true;

            // Will set up the IPC channel to which our mirror Azure SDK will connect (from the secondary app domain),
            // interception points for the root level methods we're interested in intercepting, and set up a test folder containing AI.DLL, 
            // register interceptors for all implemented properties on RoleEnvironment. Child objects are returned as serialized rather than MarshalByRef 
            // and as such don't need to be intercepted.
            WindowsAzure.ServiceRuntime.Mirror.LookingGlass.Register<bool, bool>(
                                    source: typeof(WindowsAzure.ServiceRuntime.RoleEnvironment).GetProperty("IsAvailable"),
                                    handler: b => IsAvailable);

            WindowsAzure.ServiceRuntime.Mirror.LookingGlass.Register<string, string>(
                                    source: typeof(WindowsAzure.ServiceRuntime.RoleEnvironment).GetProperty("DeploymentId"),
                                    handler: s => DeploymentId);

            WindowsAzure.ServiceRuntime.Mirror.LookingGlass.Register<WindowsAzure.ServiceRuntime.RoleInstance, WindowsAzure.ServiceRuntime.RoleInstance>(
                                    source: typeof(WindowsAzure.ServiceRuntime.RoleEnvironment).GetProperty("CurrentRoleInstance"),
                                    handler: r =>
                                    {
                                        TestRole testRole = new TestRole(RoleName);
                                        TestRoleInstance testRoleInstance = new TestRoleInstance(testRole, RoleInstanceOrdinal);
            // create a temp path first.
            TestWithServiceRuntimePath = Path.GetDirectoryName(typeof(ServiceRuntimeTests).Assembly.Location);
        }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
