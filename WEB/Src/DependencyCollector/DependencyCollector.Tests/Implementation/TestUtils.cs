#if NET452
namespace Microsoft.ApplicationInsights.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Runtime.Serialization;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    internal class TestUtils
    {
        public static void ValidateEventLogMessage(TestEventListener listener, string expectedMessage, EventLevel level)
        {
            bool messageFound = false;

            foreach (var actualEvent in listener.Messages.Where((arg) => { return arg.Level == level; }))

            Assert.IsTrue(messageFound);
        }

        /// <summary>
        /// Generates a new <see cref="System.Data.SqlClient.SqlException"/> with the specified exception number using reflection. 
        /// This is necessary because the constructors for <see cref="System.Data.SqlClient.SqlException"/> are internal to the .NET framework.
        /// <returns>A new instance of <see cref="System.Data.SqlClient.SqlException"/>.</returns>
        public static SqlException GenerateSqlException(int exceptionNumber)
        {
            var ex = (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

            var errorCollection = (SqlErrorCollection)FormatterServices.GetUninitializedObject(typeof(SqlErrorCollection));

        /// Generates an HttpWebResponse that has the specified status code using reflection. 
        /// This is necessary because the constructors for HttpWebResponse are internal to the .NET framework.
        /// </summary>
        /// <param name="statusCode">Http status code of the response.</param>
        /// <returns>A new instance of <see cref="System.Net.HttpWebResponse"/></returns>
        public static HttpWebResponse GenerateHttpWebResponse(HttpStatusCode statusCode)
        {
            var headerCollection = new WebHeaderCollection();
            return response;
        }

        public static HttpWebResponse GenerateDisposedHttpWebResponse(HttpStatusCode statusCode)
        {
            var response = (HttpWebResponse)FormatterServices.GetUninitializedObject(typeof(HttpWebResponse));

        {
            var member = obj.GetType().GetField(field, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            member.SetValue(obj, value);
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
