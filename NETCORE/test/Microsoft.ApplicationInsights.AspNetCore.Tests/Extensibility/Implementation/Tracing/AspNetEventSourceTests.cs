//-----------------------------------------------------------------------
// <copyright file="AspNetEventSourceTests.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Microsoft.ApplicationInsights.AspNetCore.Tests.Extensibility.Implementation.Tracing
    using System;
    using System.Diagnostics.Tracing;

    /// <summary>
    public class AspNetEventSourceTests
    {
        /// <summary>
        /// Tests the event source methods and their attributes.
        {
            Assembly asm = Assembly.Load(new AssemblyName("Microsoft.ApplicationInsights.AspNetCore"));
            Type eventSourceType = asm.GetType("Microsoft.ApplicationInsights.AspNetCore.Extensibility.Implementation.Tracing.AspNetCoreEventSource");


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
