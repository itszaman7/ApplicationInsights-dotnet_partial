// -----------------------------------------------------------------------
// <copyright file="SdkVersionUtils.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. 
// All rights reserved.  2013
// </copyright>
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

        internal static string GetSdkVersion(string versionPrefix)
        {
#else
            string versionStr = typeof(SdkVersionUtils).GetTypeInfo().Assembly.GetCustomAttributes<AssemblyFileVersionAttribute>().First().Version;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
