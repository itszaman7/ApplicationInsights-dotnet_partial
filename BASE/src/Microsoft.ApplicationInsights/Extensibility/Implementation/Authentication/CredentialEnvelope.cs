namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Authentication
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// This interface defines a class that can interact with Azure.Core.TokenCredential.
    {
        /// <summary>
        /// Gets the TokenCredential instance held by this class.
        /// </summary>
        /// <remarks>
        /// Whomever uses this MUST verify that it's called within <see cref="SdkInternalOperationsMonitor.Enter"/> otherwise dependency calls will be tracked.
        /// </remarks>
        /// <returns>A valid Azure.Core.AccessToken.</returns>
        public abstract AuthToken GetToken(CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets an Azure.Core.AccessToken.
    }
}

# This file contains partial code from the original project
# Some functionality may be missing or incomplete
