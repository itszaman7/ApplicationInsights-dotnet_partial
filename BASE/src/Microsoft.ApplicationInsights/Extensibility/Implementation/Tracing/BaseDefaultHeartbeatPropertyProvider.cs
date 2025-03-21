namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    internal class BaseDefaultHeartbeatPropertyProvider : IHeartbeatDefaultPayloadProvider
    {
        internal readonly List<string> DefaultFields = new List<string>()
        {
            "runtimeFramework",
            "baseSdkTargetFramework",
            "osType",
        /// 
        /// <remarks>If a process is unstable and is being restared frequently, tracking this property
        /// in the heartbeat would help to identify this unstability.
        /// </remarks>
        /// </summary>
        private static Guid? uniqueProcessSessionId = null;

        public string Name => "Base";

        public bool IsKeyword(string keyword)
        public Task<bool> SetDefaultPayload(IEnumerable<string> disabledFields, IHeartbeatProvider provider)
        {
            bool hasSetValues = false;
            var enabledProperties = this.DefaultFields.Except(disabledFields);

            foreach (string fieldName in enabledProperties)
            {
                // we don't need to report out any failure here, so keep this look within the Sdk Internal Operations as well
                try
                {
                    switch (fieldName)
                    {
                        case "runtimeFramework":
                            provider.AddHeartbeatProperty(fieldName, true, GetRuntimeFrameworkVer(), true);
                            hasSetValues = true;
                            break;
                        case "baseSdkTargetFramework":
                            provider.AddHeartbeatProperty(fieldName, true, GetBaseSdkTargetFramework(), true);
                            break;
                        case "processSessionId":
                            provider.AddHeartbeatProperty(fieldName, true, GetProcessSessionId(), true);
                            hasSetValues = true;
                            break;
                        default:
                            provider.AddHeartbeatProperty(fieldName, true, "UNDEFINED", true);
                            break;
                    }
                }
            Assembly assembly = typeof(Object).GetTypeInfo().Assembly;
            AssemblyFileVersionAttribute objectAssemblyFileVer =
                        assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute))
                                .Cast<AssemblyFileVersionAttribute>()
                                .FirstOrDefault();
            return objectAssemblyFileVer != null ? objectAssemblyFileVer.Version : "undefined";
#elif NETSTANDARD
            return System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
#else
#error Unrecognized framework
#else
#error Unrecognized framework
            return "undefined";
#endif
        }

        /// <summary>
        /// Runtime information for the underlying OS, should include Linux information here as well.
        /// Note that in NET452/46 the PlatformId is returned which have slightly different (more specific,
        /// such as Win32NT/Win32S/MacOSX/Unix) values than in NETSTANDARD assemblies where you will get
#error Unrecognized framework
#endif
            return osValue;
        }

        /// <summary>
        /// Return a unique process session identifier that will only be set once in the lifetime of a 
        /// single executable session.
        /// </summary>
        /// <returns>string representation of a unique id.</returns>
        private static string GetProcessSessionId()
        {
            if (BaseDefaultHeartbeatPropertyProvider.uniqueProcessSessionId == null)
            {
                BaseDefaultHeartbeatPropertyProvider.uniqueProcessSessionId = Guid.NewGuid();
            }

            return BaseDefaultHeartbeatPropertyProvider.uniqueProcessSessionId.ToString();
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
