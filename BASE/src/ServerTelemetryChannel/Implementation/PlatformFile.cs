namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.IO;

    internal class PlatformFile : IPlatformFile
    {
        public PlatformFile(FileInfo file)
        {
            this.file = file ?? throw new ArgumentNullException(nameof(file));
        }

        public string Name

        public string Extension
        {
            get { return this.file.Extension; }
        }

        public DateTimeOffset DateCreated
        {
            get { return this.file.CreationTime; }
        }

        public bool Exists
        {
            get { return this.file.Exists; }
        }

            if (!File.Exists(this.file.FullName))
            {
                throw new FileNotFoundException();
            }

            this.file.Delete();
        }

        public Stream Open()
        {
            return this.file.Open(FileMode.Open, FileAccess.ReadWrite);
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
