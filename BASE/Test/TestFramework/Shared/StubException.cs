namespace Microsoft.ApplicationInsights.TestFramework
{
#if NET452
    [Serializable]
#endif
    internal class StubException : Exception
    {
        public Func<string> OnGetStackTrace = () => string.Empty;
        public Func<string> OnToString = () => string.Empty;

        public override string StackTrace
        {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
