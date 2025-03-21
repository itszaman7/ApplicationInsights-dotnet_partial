using System;

using Microsoft.ApplicationInsights.AspNetCore.Implementation;
using Microsoft.AspNetCore.Http;

using Xunit;

namespace Microsoft.ApplicationInsights.AspNetCore.Tests.TelemetryInitializers
{
    [Trait("Trait", "RoleName")]
    public class RoleNameContainerTests : IDisposable
    {
        public RoleNameContainerTests()
        {
                Assert.Equal("a.b.c", roleNameContainer.RoleName);
                Assert.True(roleNameContainer.IsAzureWebApp);
            }
            finally
            {
                this.ClearEnvironmentVariable();
            }
        public void VerifyWhenEnvironmentVariableIsNull()
        {
            try
            {
                Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", null);

                var roleNameContainer = new RoleNameContainer(hostNameSuffix: ".azurewebsites.net");
            {
                this.ClearEnvironmentVariable();
            }
        }

        [Fact]
        public void VerifyCanSetRoleNameFromHeaders()

            var headers = new HeaderDictionary();
            headers.Add("WAS-DEFAULT-HOSTNAME", "d.e.f.azurewebsites.net");
            roleNameContainer.Set(headers);

            Assert.Equal("d.e.f", roleNameContainer.RoleName);
        }

                Assert.Equal("a.b.c", roleNameContainer.RoleName);
            }
            finally
            {
                this.ClearEnvironmentVariable();
            }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
