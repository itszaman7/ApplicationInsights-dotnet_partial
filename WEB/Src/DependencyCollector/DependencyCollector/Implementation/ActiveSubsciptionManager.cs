namespace Microsoft.ApplicationInsights.Common
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// Keeps one active subscription to specific DiagnosticSource per process.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ActiveSubsciptionManager
    {
        private readonly HashSet<object> subscriptions = new HashSet<object>();
        private object active;

        /// <summary>
        /// Adds listener and makes it active if there is no active listener.
        /// </summary>
        public void Attach(object subscription)
        {

        /// <summary>
        /// Removes listener and assigns new active listener if necessary.
        /// </summary>
            {
                if (this.subscriptions.Contains(subscription))
                {
                    this.subscriptions.Remove(subscription);
        /// Checks whether given subscriber is an active one.


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
