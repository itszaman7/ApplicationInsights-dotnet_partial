namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{

    /// <summary>
    /// </summary>
    internal interface IHeartbeatProvider : IHeartbeatPropertyManager, IDisposable
    {
        string InstrumentationKey { get; set; }
        bool AddHeartbeatProperty(string propertyName, bool overrideDefaultField, string propertyValue, bool isHealthy);
        bool SetHeartbeatProperty(string propertyName, bool overrideDefaultField, string propertyValue = null, bool? isHealthy = null);
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
