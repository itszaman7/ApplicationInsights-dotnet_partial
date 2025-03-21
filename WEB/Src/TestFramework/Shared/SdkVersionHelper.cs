namespace Microsoft.ApplicationInsights.TestFramework
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    using System;
#if NETCOREAPP
    public static class SdkVersionHelper
    {
#else
            object[] assemblyCustomAttributes = assemblyType.Assembly.GetCustomAttributes(false);
            string versionStr = assemblyCustomAttributes
                .OfType<AssemblyFileVersionAttribute>()
                .First()
                .Version;

            var expected = prefix + string.Join(".", versionParts[0], versionParts[1], versionParts[2]) + "-" + versionParts[3];
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
