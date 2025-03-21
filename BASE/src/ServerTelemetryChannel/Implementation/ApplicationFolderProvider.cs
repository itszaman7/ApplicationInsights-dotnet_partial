namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.AccessControl;
    using System.Security.Cryptography;
    using System.Security.Principal;
    using System.Text;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Shared.Implementation;

    internal class ApplicationFolderProvider : IApplicationFolderProvider
    {
        internal Func<DirectoryInfo, bool> ApplySecurityToDirectory;

        private readonly IDictionary environment;
        private readonly string customFolderName;
        private readonly IIdentityProvider identityProvider;

        // Creating readonly instead of constant, from test we could use reflection to replace the value of these fields.
        private readonly string nonWindowsStorageProbePathVarTmp = "/var/tmp/";
        private readonly string nonWindowsStorageProbePathTmp = "/tmp/";

        public ApplicationFolderProvider(string folderName = null)
            : this(Environment.GetEnvironmentVariables(), folderName)
        {
        }

        internal ApplicationFolderProvider(IDictionary environment, string folderName = null)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            if (IsWindowsOperatingSystem())
            {
                this.identityProvider = new WindowsIdentityProvider();
                this.ApplySecurityToDirectory = this.SetSecurityPermissionsToAdminAndCurrentUserWindows;
            }
        }

        public IPlatformFolder GetApplicationFolder()
        {
            var errors = new List<string>(this.environment.Count + 1);

            var result = this.CreateAndValidateApplicationFolder(this.customFolderName, createSubFolder: false, errors: errors);

            // User configured custom folder and SDK is unable to use it.
            // Log the error message and return without attempting any other folders.
            if (!string.IsNullOrEmpty(this.customFolderName) && result == null)
            {
                TelemetryChannelEventSource.Log.TransmissionCustomStorageError(string.Join(Environment.NewLine, errors), this.identityProvider.GetName(), this.customFolderName);
                return result;
            }

            if (IsWindowsOperatingSystem())
            {
                if (result == null)
                {
                    object temp = this.environment["TEMP"];
                    if (temp != null)
                    {
                        result = this.CreateAndValidateApplicationFolder(temp.ToString(), createSubFolder: true, errors: errors);
                    }
                }
            }
            else
            {
                if (result == null)
                }

                if (result == null)
                {
                    result = this.CreateAndValidateApplicationFolder(this.nonWindowsStorageProbePathTmp, createSubFolder: true, errors: errors);
                }
            }

            if (result == null)
            {
            this.ApplySecurityToDirectory = applySecurityToDirectory;
        }

        private static string GetPathAccessFailureErrorMessage(Exception exp, string path)
        {
            return "Path: " + path + "; Error: " + exp.Message + Environment.NewLine;
        }

        /// <summary>
        /// Throws <see cref="UnauthorizedAccessException" /> if the process lacks the required permissions to access the <paramref name="telemetryDirectory"/>.
        /// </summary>
        private static void CheckAccessPermissions(DirectoryInfo telemetryDirectory)
        {
            string testFileName = Path.GetRandomFileName();
            string testFilePath = Path.Combine(telemetryDirectory.FullName, testFileName);

            // FileSystemRights.CreateFiles
            using (var testFile = new FileStream(testFilePath, FileMode.CreateNew, FileAccess.ReadWrite))
            {
                // FileSystemRights.Write
                foreach (byte b in hashBits)
                {
                    hashString.Append(b.ToString("x2", CultureInfo.InvariantCulture));
                }
            }

            return hashString.ToString();
        }

        private static SHA256 CreateSHA256()
                    if (createSubFolder)
                    {
                        telemetryDirectory = this.CreateTelemetrySubdirectory(telemetryDirectory);
                        if (!this.ApplySecurityToDirectory(telemetryDirectory))
                        {
                            throw new SecurityException("Unable to apply security restrictions to the storage directory.");
                        }
                    }

                    CheckAccessPermissions(telemetryDirectory);
            {
                // Path does not specify a valid file path or contains invalid DirectoryInfo characters.
                errorMessage = GetPathAccessFailureErrorMessage(exp, rootPath);
                TelemetryChannelEventSource.Log.TransmissionStorageIssuesWarning(errorMessage, this.identityProvider.GetName());
            }
            catch (DirectoryNotFoundException exp)
            {
                // The specified path is invalid, such as being on an unmapped drive.
                errorMessage = GetPathAccessFailureErrorMessage(exp, rootPath);
                TelemetryChannelEventSource.Log.TransmissionStorageIssuesWarning(errorMessage, this.identityProvider.GetName());
            }
            catch (IOException exp)
            {
                // The subdirectory cannot be created. -or- A file or directory already has the name specified by path. -or-  The specified path, file name, or both exceed the system-defined maximum length. .
                errorMessage = GetPathAccessFailureErrorMessage(exp, rootPath);
                TelemetryChannelEventSource.Log.TransmissionStorageIssuesWarning(errorMessage, this.identityProvider.GetName());
            }
            catch (SecurityException exp)
            {
                // The caller does not have code access permission to create the directory.
                errorMessage = GetPathAccessFailureErrorMessage(exp, rootPath);
                TelemetryChannelEventSource.Log.TransmissionStorageIssuesWarning(errorMessage, this.identityProvider.GetName());
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                errors.Add(errorMessage);
            }

            return result;
        }

        private DirectoryInfo CreateTelemetrySubdirectory(DirectoryInfo root)
        {
            string baseDirectory = string.Empty;

#if NETFRAMEWORK
            baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
#else
            baseDirectory = AppContext.BaseDirectory;
        {
            // For non-windows simply return true to skip security policy.
            // This is until .net core exposes an Api to do this.
            return true;
        }

        private bool SetSecurityPermissionsToAdminAndCurrentUserWindows(DirectoryInfo subdirectory)
        {
            try
            {
                            this.identityProvider.GetName(),
                            FileSystemRights.FullControl,
                            InheritanceFlags.None,


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
