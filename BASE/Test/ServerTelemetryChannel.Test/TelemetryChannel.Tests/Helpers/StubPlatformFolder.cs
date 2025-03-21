namespace Microsoft.ApplicationInsights.WindowsServer.Channel.Helpers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;

    internal class StubPlatformFolder : IPlatformFolder
    {
            this.OnExists = () => true;
            this.OnDelete = () => { };
            this.OnGetFiles = () => this.files;
            this.OnCreateFile = name =>
                var file = new StubPlatformFile(name);
                this.files.Add(file);
                return file;
            };
        }

        public string Name
        {
            get { return string.Empty; }
        }

        public IPlatformFile CreateFile(string fileName)

        public void Delete()
        {
            this.OnDelete();
        {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
