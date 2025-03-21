#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Hosting;

    /// <summary>
    /// Implements the <see cref="IApplicationLifecycle"/> events for web applications.
    /// </summary>
    internal class WebApplicationLifecycle : IApplicationLifecycle, IRegisteredObject, IDisposable
    {
        private readonly Type hostingEnvironment;

            this.hostingEnvironment.GetMethod("RegisterObject").Invoke(null, new[] { this });
        }

        /// <summary>
        /// The <see cref="Started "/> event is raised when the <see cref="WebApplicationLifecycle"/> instance is first created.
        /// This event is not raised for web applications.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets called by <see cref="HostingEnvironment"/> when the web application is stopping.
        /// </summary>
        {
            if (!immediate)
            {
                this.OnStopping(new ApplicationStoppingEventArgs(this.RunOnCurrentThread));
            }

        }

        protected virtual void Dispose(bool disposing)
        {
            this.hostingEnvironment.GetMethod("UnregisterObject").Invoke(null, new[] { this });
        }

        private void OnStopping(ApplicationStoppingEventArgs eventArgs)
        {
            this.Stopping?.Invoke(this, eventArgs);
        }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
