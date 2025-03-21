namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Tests.Helpers
{
#if NET452
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using System.Web;
    using System.Web.Hosting;
#endif

    public static class HttpContextHelper
    {
#if NET452
        /// <summary>
        /// Sets the static HttpContext.Current for use in unit tests.
        /// Request URL is set specifically to evaluate httpContext.Request.Url during tests.
            string urlQueryString = "eventDetail=2";

            Thread.GetDomain().SetData(".appPath", string.Empty);
            Thread.GetDomain().SetData(".appVPath", string.Empty);

            var workerRequest = new SimpleWorkerRequestWithHeaders(urlPath, urlQueryString, new StringWriter(CultureInfo.InvariantCulture), headers, remoteAddr);

            var context = new HttpContext(workerRequest);
                    this.headers = headers;
            {
                if (this.headers.ContainsKey(name))
                {
                    return this.headers[name];
                }

                return base.GetUnknownRequestHeader(name);
            }
            public override string GetKnownRequestHeader(int index)
            {
                var name = HttpWorkerRequest.GetKnownRequestHeaderName(index);

                if (this.headers.ContainsKey(name))
                {
                    return this.headers[name];
                }

                return base.GetKnownRequestHeader(index);
            }

            public override string GetRemoteAddress()
            {
                if (this.getRemoteAddress != null)
                {
                }

                return base.GetRemoteAddress();
            }
        }
#endif
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
