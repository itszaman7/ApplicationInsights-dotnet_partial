namespace Microsoft.ApplicationInsights.AspNetCore
{
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Utility class for the version information of the current assembly.
    /// </summary>
        /// </summary>
        public const string VersionPrefix = "aspnet5f:";
#else
        /// <returns>Assembly version combined with this assembly's version prefix.</returns>
        internal static string GetVersion()
        {

        /// <summary>
        /// Get the Assembly Version with given SDK prefix.
        /// <param name="versionPrefix">Prefix string to be included with the version.</param>
        /// <returns>Returns a string representing the current assembly version.</returns>
        internal static string GetVersion(string versionPrefix)
        private static string GetAssemblyVersion()
        {
            return typeof(SdkVersionUtils).GetTypeInfo().Assembly.GetCustomAttributes<AssemblyInformationalVersionAttribute>()
                      .First()
                      .InformationalVersion;
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
