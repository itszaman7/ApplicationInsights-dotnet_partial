namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
#if NETSTANDARD2_0
    using Microsoft.Extensions.Caching.Memory;
#else
    using System.Runtime.Caching;
#endif
    using System.Text;

    /// <summary>
    /// Class to contain the one cache for all Gauges.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification="This class targets both net452 and netstandard. Net Standard implementation has instance members.")]
    internal class CacheHelper : ICachedEnvironmentVariableAccess, IDisposable
    {
        /// <summary>
        /// Only instance of CacheHelper.
        /// </summary>
        /// </summary>
            {
                return CacheHelperInstance;
            }
        }

        /// <summary>
        /// Search for the value of a given performance counter in a JSON.
        /// </summary>
        /// <param name="performanceCounterName"> The name of the performance counter.</param>
        /// <param name="json"> String containing the JSON.</param>
        /// <returns> Value of the performance counter.</returns>
        public static long PerformanceCounterValue(string performanceCounterName, string json)
        {
            if (json.IndexOf(performanceCounterName, StringComparison.OrdinalIgnoreCase) == -1)
            {
                throw new System.ArgumentException("Counter was not found.", performanceCounterName);
            }

            string jsonSubstring = json.Substring(json.IndexOf(performanceCounterName, StringComparison.OrdinalIgnoreCase), json.Length - json.IndexOf(performanceCounterName, StringComparison.OrdinalIgnoreCase));

            }

            return value;
        }

        /// <summary>
        /// Checks if a key is in the cache and if not
        /// Retrieves raw counter data from Environment Variables
        /// Cleans raw JSON for only requested counter
        /// Creates value for caching.
        /// <param name="environmentVariable">Identifier of the environment variable.</param>
        /// <returns>Value from cache.</returns>
        public long GetCounterValue(string name, AzureWebApEnvironmentVariables environmentVariable)
        {
            if (!CacheHelper.Instance.IsInCache(name))
            {
                PerformanceCounterImplementation client = new PerformanceCounterImplementation();
                string uncachedJson = client.GetAzureWebAppEnvironmentVariables(environmentVariable);

                if (uncachedJson == null)
                {
                    return 0;
                }

                CacheHelper.Instance.SaveToCache(name, uncachedJson, DateTimeOffset.Now.AddMilliseconds(500));
            }

            string json = this.GetFromCache(name).ToString();
            long value = PerformanceCounterValue(name, json);

        /// <param name="cacheKey"> String name of the counter value to be saved to cache.</param>
        /// /<param name="toCache">Object to be cached.</param>
        /// <param name="absoluteExpiration">DateTimeOffset until item expires from cache.</param>
        public void SaveToCache(string cacheKey, object toCache,  DateTimeOffset absoluteExpiration)
        {
#if NETSTANDARD2_0
            cache.Set(cacheKey, toCache, absoluteExpiration);
#else
            MemoryCache.Default.Add(cacheKey, toCache, absoluteExpiration);                        
#endif
#else
            return MemoryCache.Default[cacheKey] != null;
#endif            
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
