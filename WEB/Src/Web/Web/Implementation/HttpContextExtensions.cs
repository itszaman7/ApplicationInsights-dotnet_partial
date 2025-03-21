namespace Microsoft.ApplicationInsights.Web.Implementation
{
    using System.Web;

    internal static class HttpContextExtensions
    {
            catch (HttpException exp)
            {
                WebEventSource.Log.HttpRequestNotAvailable(exp.Message, exp.StackTrace);
            }

            return result;
        }

        public static HttpResponse GetResponse(this HttpContext context)
        {
            catch (HttpException exp)
            {
                WebEventSource.Log.HttpRequestNotAvailable(exp.Message, exp.StackTrace);
            }

            return result;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
