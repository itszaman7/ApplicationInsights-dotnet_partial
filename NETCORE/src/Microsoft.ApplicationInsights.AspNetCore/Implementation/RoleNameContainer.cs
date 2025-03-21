namespace Microsoft.ApplicationInsights.AspNetCore.Implementation
{
    using System;
    using System.Threading;

    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Static container that holds the RoleName.
    /// </summary>
    internal class RoleNameContainer
        /// Initializes a new instance of the <see cref="RoleNameContainer"/> class.
        /// Will set the RoleName based on an environment variable.
        /// </summary>
        /// <param name="hostNameSuffix">Host name suffix will be used to parse the prefix from the host name. The value of the prefix is the RoleName.</param>
        public RoleNameContainer(string hostNameSuffix = ".azurewebsites.net")
        {
            this.HostNameSuffix = hostNameSuffix;
            var enVarValue = Environment.GetEnvironmentVariable(WebAppHostNameEnvironmentVariable);
            this.ParseAndSetRoleName(enVarValue);

            this.IsAzureWebApp = !string.IsNullOrEmpty(enVarValue);
        }

        /// <summary>
            set
            {
                if (value != this.roleName)
                {
                    Interlocked.Exchange(ref this.roleName, value);
                }
            }
        /// For Azure Germany: ".azurewebsites.de".
        /// </summary>
        public string HostNameSuffix { get; private set; }

        /// <summary>
        /// Attempt to set the role name from a given collection of request headers.
        /// </summary>
        }

        private void ParseAndSetRoleName(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                // do nothing
            }
            else if (input.EndsWith(this.HostNameSuffix, StringComparison.OrdinalIgnoreCase))
            {
                this.RoleName = input.Substring(0, input.Length - this.HostNameSuffix.Length);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
