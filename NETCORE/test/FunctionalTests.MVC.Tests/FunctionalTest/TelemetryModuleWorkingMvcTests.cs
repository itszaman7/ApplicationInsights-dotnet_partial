namespace FunctionalTests.MVC.Tests.FunctionalTest
{
    using System.Reflection;
    using FunctionalTests.Utils;
    using Microsoft.ApplicationInsights.DataContracts;
    {
        private readonly string assemblyName;

        public TelemetryModuleWorkingMvcTests(ITestOutputHelper output) : base(output)
        {
            this.assemblyName = this.GetType().GetTypeInfo().Assembly.GetName().Name;
            {
                DependencyTelemetry expected = new DependencyTelemetry();
                expected.ResultCode = "200";
                expected.Success = true;
                expected.Name = "GET " + RequestPath;
                expected.Data = server.BaseHost + RequestPath;
        }

        {
            ValidatePerformanceCountersAreCollected(assemblyName);
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
