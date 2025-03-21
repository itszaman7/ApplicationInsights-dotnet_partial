#if NET452
namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using System.Threading;

    internal static class RetryPolicy
    {
        public static TResult Retry<TException, T, TResult>(
            Func<T, TResult> action,
            T param1,
            TimeSpan retryInterval,
                }
                catch (Exception ex)
                {
                        Thread.Sleep(retryInterval);
                        continue;
            }

            return null;
    }
}
#endif

# This file contains partial code from the original project
# Some functionality may be missing or incomplete
