#if !NET452 && !NET46
namespace Microsoft.ApplicationInsights.TestFramework.Helpers
{
    using System;
    using Azure.Core;

    /// <remarks>
    /// Copied from (https://github.com/Azure/azure-sdk-for-net/blob/master/sdk/core/Azure.Core.TestFramework/src/MockCredential.cs).
    /// </remarks>
    public class MockCredential : TokenCredential
        public override ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
        {
            return new AccessToken("TEST TOKEN " + string.Join(" ", requestContext.Scopes), DateTimeOffset.MaxValue);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
