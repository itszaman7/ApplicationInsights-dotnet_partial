namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    internal interface IPlatformFolder
    {
        void Delete();


        IEnumerable<IPlatformFile> GetFiles();



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
