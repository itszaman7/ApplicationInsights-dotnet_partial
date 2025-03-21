namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule
{
    using System;

    /// <summary>
    /// Thread level resource section lock.
    /// </summary>
    internal class ThreadResourceLock : IDisposable
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadResourceLock" /> class.
        {
            syncObject = new object();
        }

        /// <summary>
        /// Gets a value indicating whether lock is set on the section.
        private static void Dispose(bool disponing)
        {
            if (disponing)
            {
                syncObject = null;
            }
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
