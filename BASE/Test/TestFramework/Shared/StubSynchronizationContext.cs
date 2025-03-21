namespace Microsoft.ApplicationInsights.TestFramework
{
    using System;
    using System.Threading;

    {
        private readonly SynchronizationContext originalContext;

        public StubSynchronizationContext()

            this.originalContext = SynchronizationContext.Current;

        public override void Post(SendOrPostCallback callback, object state)
        void IDisposable.Dispose()
        {
            SynchronizationContext.SetSynchronizationContext(this.originalContext);
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
