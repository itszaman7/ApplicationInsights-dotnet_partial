#if NET452
namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using Microsoft.Diagnostics.Instrumentation.Extensions.Intercept;

    internal static class ProfilerRuntimeInstrumentation
    {
        internal static void DecorateProfilerForHttp(ref ProfilerHttpProcessing httpCallbacks)
        {
            // Decorates Http GetResponse, 0 params
            Functions.Decorate(
                "System",
                "System.dll",
                "System.Net.HttpWebRequest.GetResponse",
                httpCallbacks.OnBeginForGetResponse,
                httpCallbacks.OnEndForGetResponse,
                httpCallbacks.OnExceptionForGetResponse,
                isStatic: false);

            // Decorates Http GetRequestStream, 1 param
            Functions.Decorate(
                "System",
                "System.dll",
                "System.Net.HttpWebRequest.GetRequestStream",
                httpCallbacks.OnBeginForGetRequestStream,
                null,
                httpCallbacks.OnExceptionForGetRequestStream,
                isStatic: false);

            // Decorates Http BeginGetResponse, 2 params
            Functions.Decorate(
                "System",
                "System.dll",
                "System.Net.HttpWebRequest.BeginGetResponse",
                httpCallbacks.OnBeginForBeginGetResponse,
                null,
                null,
                isStatic: false);

            // Decorates Http EndGetResponse, 1 param
            Functions.Decorate(
                "System",
                "System.dll",
                "System.Net.HttpWebRequest.EndGetResponse",
                isStatic: false);

            // Decorates Http BeginGetRequestStream
            Functions.Decorate(
                "System",
                "System.dll",
                "System.Net.HttpWebRequest.BeginGetRequestStream",
                httpCallbacks.OnBeginForBeginGetRequestStream,
                null,
                null,
                isStatic: false);

            // Decorates Http EndGetRequestStream, 2 params
            Functions.Decorate(
                "System",
                "System.dll",
                "System.Net.HttpWebRequest.EndGetRequestStream",
                null,
                null,
                httpCallbacks.OnExceptionForEndGetRequestStream,
                isStatic: false);
        }

        internal static void DecorateProfilerForSqlCommand(ref ProfilerSqlCommandProcessing sqlCallbacks)
        {
                sqlCallbacks.OnBeginForOneParameter,
                null,
                null,
                isStatic: false);

            // Decorates Sql BeginExecuteNonQuery(AsyncCallback, Object), 2 params (+this)
            Functions.Decorate(
                "System.Data",
                "System.Data.dll",
                "System.Data.SqlClient.SqlCommand.BeginExecuteNonQuery",

            // Read comment above. Decorate BeginExecuteNonQueryAsync, 2 param (+this)
            Functions.Decorate(
                "System.Data",
                "System.Data.dll",
                "System.Data.SqlClient.SqlCommand.BeginExecuteNonQueryAsync",
                sqlCallbacks.OnBeginForThreeParameters,
                null,
                null,
                isStatic: false);
                isStatic: false);

            //// ___ ExecuteReader ___ ////

            // Decorates Sql BeginExecuteReader, 0 param (+this)
            Functions.Decorate(
                "System.Data",
                "System.Data.dll",
                "System.Data.SqlClient.SqlCommand.BeginExecuteReader",
                sqlCallbacks.OnBeginForOneParameter,
                isStatic: false);

            // Decorates Sql BeginExecuteReader(AsyncCallback, Object), 2 param (+this)
            Functions.Decorate(
                "System.Data",
                "System.Data.dll",
                "System.Data.SqlClient.SqlCommand.BeginExecuteReader",
                sqlCallbacks.OnBeginForThreeParameters,
                null,
                null,
                isStatic: false);

            // Decorates Sql BeginExecuteReader(AsyncCallback, Object, CommandBehavior), 3 params (+this)
            Functions.Decorate(
                "System.Data",
                "System.Data.dll",
                "System.Data.SqlClient.SqlCommand.BeginExecuteReader",
                sqlCallbacks.OnBeginForFourParameters,
                null,
                null,

            // Decorates Sql EndExecuteReader, 1 param (+this)
            Functions.Decorate(
                "System.Data",
                "System.Data.dll",
                "System.Data.SqlClient.SqlCommand.EndExecuteReader",
                null,
                sqlCallbacks.OnEndForTwoParameters,
                sqlCallbacks.OnExceptionForTwoParameters,
                isStatic: false);
                "System.Data.SqlClient.SqlCommand.EndExecuteReaderAsync",
                null,
                sqlCallbacks.OnEndForTwoParameters,
                sqlCallbacks.OnExceptionForTwoParameters,
                isStatic: false);

            //// ___ ExecuteScalar ___ ////

            // Decorate Sql ExecuteScalar, 0 params(+this)
            Functions.Decorate(

            //// ___ ExecuteXmlReader ___ ////

            // Decorates Sql BeginExecuteXmlReader, 0 params(+this)
            Functions.Decorate(
                "System.Data",
                "System.Data.dll",
                "System.Data.SqlClient.SqlCommand.BeginExecuteXmlReader",
                sqlCallbacks.OnBeginForOneParameter,
                null,
                sqlCallbacks.OnExceptionForTwoParameters,
                isStatic: false);

            // Decorate Sql ExecuteXmlReader, 0 params(+this)
            Functions.Decorate(
                "System.Data",
                "System.Data.dll",
                "System.Data.SqlClient.SqlCommand.ExecuteXmlReader",
                sqlCallbacks.OnBeginForOneParameter,
                sqlCallbacks.OnEndForOneParameter,
            // Should be replaced with public method when InstrumentationEngine supports Tasks.
            // Decorate EndExecuteXmlReaderAsync, 1 param (+this)
            Functions.Decorate(
                "System.Data",
                "System.Data.dll",
                "System.Data.SqlClient.SqlCommand.EndExecuteXmlReaderAsync",
                null,
                sqlCallbacks.OnEndForTwoParameters,
                sqlCallbacks.OnExceptionForTwoParameters,
                isStatic: false);
        }

        internal static void DecorateProfilerForSqlConnection(ref ProfilerSqlConnectionProcessing sqlCallbacks)
        {
            // Decorates SqlConnection.Open, 0 params(+this)
            // Tracks dependency call only in case of failure
            Functions.Decorate(
                "System.Data",
                "System.Data.dll",
                "System.Data.SqlClient.SqlConnection.Open",

            // Decorates SqlConnection.OpenAsync, 1 param(+this)
            // Tracks dependency call only in case of failure
            Functions.Decorate(
                "System.Data",
                "System.Data.dll",
                "System.Data.SqlClient.SqlConnection.OpenAsync",
                sqlCallbacks.OnBeginForTwoParameters,
                sqlCallbacks.OnEndExceptionAsyncForTwoParameters,
                null,


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
