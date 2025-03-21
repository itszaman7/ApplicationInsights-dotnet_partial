namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System.Threading;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

            Assert.IsFalse(ThreadResourceLock.IsResourceLocked);
            using (var resourceLock = new ThreadResourceLock())
            {
                Assert.IsTrue(ThreadResourceLock.IsResourceLocked);
            }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
