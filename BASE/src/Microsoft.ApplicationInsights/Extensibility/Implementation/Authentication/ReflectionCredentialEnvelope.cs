namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Authentication
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// This is an envelope for an instance of Azure.Core.TokenCredential.
    /// This class uses reflection to interact with the Azure.Core library.
    /// </summary>
    /// <remarks>
    /// Our SDK currently targets net452, net46, and netstandard2.0.
    /// Azure.Core.TokenCredential is only available for netstandard2.0.
    /// </remarks>
    internal class ReflectionCredentialEnvelope : CredentialEnvelope
    {
#if REDFIELD
        private static volatile string azureCoreAssemblyName = "Azure.Identity.ILRepack";
#else
        private static volatile string azureCoreAssemblyName = "Azure.Core";
#endif

        private readonly object tokenCredential;
        private readonly object tokenRequestContext;

        /// <summary>
        /// Create an instance of <see cref="ReflectionCredentialEnvelope"/>.
        /// </summary>
        /// <param name="tokenCredential">An instance of Azure.Core.TokenCredential.</param>
        public ReflectionCredentialEnvelope(object tokenCredential)
        {
            this.tokenCredential = tokenCredential ?? throw new ArgumentNullException(nameof(tokenCredential));

            if (IsValidType(tokenCredential))
            {
                this.tokenRequestContext = AzureCore.MakeTokenRequestContext(scopes: AuthConstants.GetScopes());
            }
        /// <returns>A valid Azure.Core.AccessToken.</returns>
        public override AuthToken GetToken(CancellationToken cancellationToken = default)
        {
            try
            {
                return AzureCore.InvokeGetToken(this.tokenCredential, this.tokenRequestContext, cancellationToken);
            }
            catch (Exception ex)
            {
                CoreEventSource.Log.FailedToGetToken(ex.ToInvariantString());
        /// <returns>A valid Azure.Core.AccessToken.</returns>
        public override async Task<AuthToken> GetTokenAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await AzureCore.InvokeGetTokenAsync(this.tokenCredential, this.tokenRequestContext, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                CoreEventSource.Log.FailedToGetToken(ex.ToInvariantString());
            {
                throw new Exception("An error has occurred while trying to get type Azure.Core.TokenCredential. See inner exception.", ex);
            }

            if (typeTokenCredential == null)
            {
                if (AppDomain.CurrentDomain.GetAssemblies().Any(x => x.FullName.StartsWith(azureCoreAssemblyName, StringComparison.Ordinal)))
                {
                    throw new Exception("An unknown error has occurred. Failed to get type Azure.Core.TokenCredential. Detected that Azure.Core is loaded in AppDomain.CurrentDomain.");
                }
                {
                    throw new Exception("Failed to get type Azure.Core.TokenCredential. Azure.Core is not found in AppDomain.CurrentDomain.");
                }
            }

            return typeTokenCredential;
        }

        /// <summary>
        /// This class provides Reflection based wrappers around types found in the Azure.Core library.
        /// (https://docs.microsoft.com/dotnet/csharp/programming-guide/concepts/expression-trees/).
        /// (https://docs.microsoft.com/dotnet/csharp/expression-trees).
        /// </summary>
        internal static class AzureCore
        {
            private static readonly Delegate GetTokenValue = BuildDelegateGetToken();
            private static readonly Delegate GetTokenAsyncAsTask = BuildDelegateGetTokenAsync();
            private static readonly Delegate GetTaskResult = BuildGetTaskResult();
            private static readonly Delegate AccessTokenToAuthToken = BuildDelegateAccessTokenToAuthToken();

                await task.ConfigureAwait(false);

                var objAccessToken = GetTaskResult.DynamicInvoke(task);
                return (AuthToken)AccessTokenToAuthToken.DynamicInvoke(objAccessToken);
            }

            /// <summary>
            /// This is a wrapper for the following constructor:
            /// <code>public TokenRequestContext (string[] scopes, string? parentRequestId = default, string? claims = default);</code>
            /// (https://docs.microsoft.com/dotnet/api/azure.core.tokenrequestcontext.-ctor).
            /// <summary>
            /// This is a wrapper for Azure.Core.AccessToken:
            /// <code>public struct AccessToken</code>
            /// (https://docs.microsoft.com/dotnet/api/azure.core.accesstoken).
            /// </summary>
            /// <returns>
            /// Returns a delegate that receives an Azure.Core.AccessToken and emits an <see cref="AuthToken"/>.
            /// </returns>
            private static Delegate BuildDelegateAccessTokenToAuthToken()
            {
                Type typeAccessToken = Type.GetType($"Azure.Core.AccessToken, {azureCoreAssemblyName}");

                var parameterExpression_AccessToken = Expression.Parameter(typeAccessToken, "parameterExpression_AccessToken");

                var exprTokenProperty = Expression.Property(
                    expression: parameterExpression_AccessToken,
                    propertyName: "Token");

                var exprExpiresOnProperty = Expression.Property(
                    expression: parameterExpression_AccessToken,
            /// This creates a wrapper for the following method:
            /// <code>public abstract Azure.Core.AccessToken GetToken (Azure.Core.TokenRequestContext requestContext, System.Threading.CancellationToken cancellationToken).</code>
            /// (https://docs.microsoft.com/dotnet/api/azure.core.tokencredential.gettoken).
            /// </summary>
            /// <returns>
            /// Returns a delegate that receives an Azure.Core.TokenCredential and emits an Azure.Core.AccessToken.
            /// </returns>
            private static Delegate BuildDelegateGetToken()
            {
                Type typeTokenCredential = Type.GetType($"Azure.Core.TokenCredential, {azureCoreAssemblyName}");
                Type typeTokenRequestContext = Type.GetType($"Azure.Core.TokenRequestContext, {azureCoreAssemblyName}");
                Type typeCancellationToken = typeof(CancellationToken);

                var parameterExpression_tokenCredential = Expression.Parameter(type: typeTokenCredential, name: "parameterExpression_TokenCredential");
                var parameterExpression_requestContext = Expression.Parameter(type: typeTokenRequestContext, name: "parameterExpression_RequestContext");
                var parameterExpression_cancellationToken = Expression.Parameter(type: typeCancellationToken, name: "parameterExpression_CancellationToken");

                Type typeTokenRequestContext = Type.GetType($"Azure.Core.TokenRequestContext, {azureCoreAssemblyName}");
                Type typeCancellationToken = typeof(CancellationToken);

                var parameterExpression_TokenCredential = Expression.Parameter(type: typeTokenCredential, name: "parameterExpression_TokenCredential");
                var parameterExpression_RequestContext = Expression.Parameter(type: typeTokenRequestContext, name: "parameterExpression_RequestContext");
                var parameterExpression_CancellationToken = Expression.Parameter(type: typeCancellationToken, name: "parameterExpression_CancellationToken");

                var methodInfo_GetTokenAsync = typeTokenCredential.GetMethod(name: "GetTokenAsync", types: new Type[] { typeTokenRequestContext, typeCancellationToken });

                var exprGetTokenAsync = Expression.Call(
                    instance: exprGetTokenAsync,
                    method: methodInfo_AsTask);

                return Expression.Lambda(
                    body: exprAsTask,
                    parameters: new ParameterExpression[]
                    {
                        parameterExpression_TokenCredential,
                        parameterExpression_RequestContext,
                        parameterExpression_CancellationToken,
            /// <returns>
            /// Returns a delegate which receives a <see cref="Task"/> and emits an Azure.Core.AccessToken.
            /// </returns>
            private static Delegate BuildGetTaskResult()
            {
                Type typeTokenCredential = Type.GetType($"Azure.Core.TokenCredential, {azureCoreAssemblyName}");
                Type typeTokenRequestContext = Type.GetType($"Azure.Core.TokenRequestContext, {azureCoreAssemblyName}");
                Type typeCancellationToken = typeof(CancellationToken);
                var methodInfo_GetTokenAsync = typeTokenCredential.GetMethod(name: "GetTokenAsync", types: new Type[] { typeTokenRequestContext, typeCancellationToken });
                var methodInfo_AsTask = methodInfo_GetTokenAsync.ReturnType.GetMethod("AsTask");
                var exprResultProperty = Expression.Property(
                    expression: parameterExpression_Task,
                    propertyName: "Result");

                return Expression.Lambda(
                    body: exprResultProperty,
                    parameters: new ParameterExpression[]
                    {
                        parameterExpression_Task,
                    }).Compile();


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
