namespace Microsoft.ApplicationInsights.Tests
{
    using System;
#if NETCOREAPP
    using System.Reflection;
#endif
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
#if !NETCOREAPP
    using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
        {
            SetPrivateStaticField(typeof(TelemetryConfiguration), "active", null);
        {
#if NETCOREAPP
            TypeInfo typeInfo = type.GetTypeInfo();
            FieldInfo field = typeInfo.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            field.SetValue(null, value);
            t.SetStaticField(fieldName, value);
#endif


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
