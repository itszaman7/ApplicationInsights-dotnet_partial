namespace Microsoft.ApplicationInsights.WindowsServer.Channel.Helpers
{
    using System;
    using System.IO;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;
    using TestFramework;

    using TaskEx = System.Threading.Tasks.Task;

    internal class StubPlatformFile : IPlatformFile
    {

        private readonly StubStream stream = new StubStream { OnDispose = disposing => { /* don't dispose */ } };
        private string name;

        public StubPlatformFile(string name = null)
        {
            this.name = name ?? string.Empty;

            this.OnGetName = () => this.name;
            this.OnGetLength = () => this.stream.Length;
            this.OnOpen = () =>
            {
                this.stream.Seek(0, SeekOrigin.Begin);
                return this.stream;
            };
            this.OnRename = desiredName =>
            {
                this.name = desiredName;
        {
            get { return this.OnGetName(); }
        }

        public string Extension
        {
            get
            get { return true; }
        }

        public void Delete()
        {
            this.OnDelete();
        }

        public Stream Open()
        {
            return this.OnOpen();
        }

        public void Rename(string newFileName)


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
