#if NETCOREAPP
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Net.Http.Headers;

namespace Microsoft.ApplicationInsights.TestFramework

        public Action<HttpContext> ServerSideAsserts;

        public LocalInProcHttpServer(string url)
        {
            this.cts = new CancellationTokenSource();
            this.host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls(url)
                .Configure((app) =>
                {
                    app.Run(Server);
                })
                .Build();
            catch (Exception)
            {
            }

                    // https://docs.microsoft.com/en-us/aspnet/core/performance/caching/middleware?view=aspnetcore-5.0
                    // https://docs.microsoft.com/en-us/dotnet/api/system.net.http.headers.cachecontrolheadervalue?view=net-5.0
                    httpContext.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = cacheExpirationDuration,
                    };

                    await httpContext.Response.WriteAsync("redirect");
                },
            };
        }

        public static LocalInProcHttpServer MakeTargetServer(string url, string response)
        {
            return new LocalInProcHttpServer(url)
            {
                ServerLogic = async (httpContext) => await httpContext.Response.WriteAsync(response)
            };
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
