namespace Microsoft.ApplicationInsights.Common
{
    using System;
    using System.Globalization;
    using System.Linq;
        internal static string GetSdkVersion(string versionPrefix)
        {
            // Since dependencySource is no longer set, sdk version is prepended with information which can identify whether RDD was collected by profiler/framework
            // For directly using TrackDependency(), version will be simply what is set by core
                    .Version;

            return (versionPrefix ?? string.Empty) + version.ToString(3) + "-" + postfix;
        }
    }
}

# This file contains partial code from the original project
# Some functionality may be missing or incomplete
