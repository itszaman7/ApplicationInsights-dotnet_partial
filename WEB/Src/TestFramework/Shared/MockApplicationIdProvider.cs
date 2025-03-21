namespace Microsoft.ApplicationInsights.Web.TestFramework
{
    using Microsoft.ApplicationInsights.Extensibility;

    {
        private readonly string expectedInstrumentationKey;
        public MockApplicationIdProvider(string expectedInstrumentationKey, string applicationId)
            this.expectedInstrumentationKey = expectedInstrumentationKey;
            this.applicationId = applicationId;
            if (this.expectedInstrumentationKey == instrumentationKey)
            {
                applicationId = this.applicationId;
                return true;
            applicationId = null;
            return false;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
