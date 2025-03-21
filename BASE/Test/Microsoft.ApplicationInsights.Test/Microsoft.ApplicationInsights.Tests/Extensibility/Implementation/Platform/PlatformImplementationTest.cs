namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Platform
{
#if NETFRAMEWORK
    using System;
    using System.IO;
    using System.Security;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Shared, platform-neutral tests for <see cref="PlatformImplementation"/> class.
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        [TestMethod]
        public void ReadConfigurationXmlReturnsContentsOfApplicationInsightsConfigFileInApplicationInstallationDirectory()
        {
            const string TestFileContent = "42";
            CreateConfigurationFile(TestFileContent);
            var platform = new PlatformImplementation();

            string s = platform.ReadConfigurationXml();
        }

        [TestMethod]
        public void FailureToReadEnvironmentVariablesDoesNotThrowExceptions()
        {
            EnvironmentPermission permission = new EnvironmentPermission(EnvironmentPermissionAccess.NoAccess, "PATH");
            try
            {
                permission.PermitOnly();
                PlatformImplementation platform = new PlatformImplementation();
            if (disposing == true)
            {
                DeleteConfigurationFile();
            }
        }

        private static void CreateConfigurationFile(string content)
        {
            }           
        }

        private static void DeleteConfigurationFile()
        {
            File.Delete(Path.Combine(Environment.CurrentDirectory, "ApplicationInsights.config"));
        }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
