namespace Microsoft.ApplicationInsights.WindowsServer.Channel.Helpers
{
    using System.Globalization;

    public static class BackendResponseHelper
    {
        public static string CreateBackendResponse(int itemsReceived, int itemsAccepted, string[] errorCodes,
            int indexStartWith = 0)
        {

            string errorList = string.Empty;
                if (!string.IsNullOrEmpty(errorList))
                {

            return
                "\"itemsReceived\": " + itemsReceived + "," +
                "\"itemsAccepted\": " + itemsAccepted + "," +
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
