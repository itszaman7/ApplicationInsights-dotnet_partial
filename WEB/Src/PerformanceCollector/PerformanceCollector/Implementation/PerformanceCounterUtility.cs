namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.StandardPerfCollector;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector;
#if NETSTANDARD2_0
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.XPlatform;
#endif

    /// <summary>
    /// Utility functionality for performance counter collection.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "This class has different code for Net452/NetCore")]
    internal static class PerformanceCounterUtility
    {
#if NETSTANDARD2_0
        public static bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif
        // Internal for testing
        internal static bool? isAzureWebApp = null;

        private const string Win32ProcessInstancePlaceholder = @"APP_WIN32_PROC";
        private const string ClrProcessInstancePlaceholder = @"APP_CLR_PROC";
        private const string W3SvcProcessInstancePlaceholder = @"APP_W3SVC_PROC";

        private const string Win32ProcessCategoryName = "Process";
        private const string ClrProcessCategoryName = ".NET CLR Memory";
        private const string Win32ProcessCounterName = "ID Process";
        private const string ClrProcessCounterName = "Process ID";
#if NETSTANDARD2_0
        private const string StandardSdkVersionPrefix = "pccore:";
#else
        private const string StandardSdkVersionPrefix = "pc:";
#endif
        private const string AzureWebAppSdkVersionPrefix = "azwapc:";
        private const string AzureWebAppCoreSdkVersionPrefix = "azwapccore:";

        private const string WebSiteEnvironmentVariable = "WEBSITE_SITE_NAME";
        private const string WebSiteIsolationEnvironmentVariable = "WEBSITE_ISOLATION";
        private const string WebSiteIsolationHyperV = "hyperv";

        private static readonly ConcurrentDictionary<string, Tuple<DateTime, PerformanceCounterCategory, InstanceDataCollectionCollection>> cache = new ConcurrentDictionary<string, Tuple<DateTime, PerformanceCounterCategory, InstanceDataCollectionCollection>>();
        private static readonly ConcurrentDictionary<string, string> PlaceholderCache =
            new ConcurrentDictionary<string, string>();

        private static readonly Regex InstancePlaceholderRegex = new Regex(
            @"^\?\?(?<placeholder>[a-zA-Z0-9_]+)\?\?$",
            RegexOptions.Compiled);

        private static readonly Regex PerformanceCounterRegex =
            new Regex(
                @"^\\(?<categoryName>[^(]+)(\((?<instanceName>[^)]+)\)){0,1}\\(?<counterName>[\s\S]+)$",
            return collector;
                    // WebApp For windows
                    collector = (IPerformanceCollector)new WebAppPerformanceCollector();
                    PerformanceCollectorEventSource.Log.InitializedWithCollector(collector.GetType().Name);
                }
                else
                {
                    // We are in WebApp, but not Windows. Use XPlatformPerfCollector.
                    collector = (IPerformanceCollector)new PerformanceCollectorXPlatform();
                    PerformanceCollectorEventSource.Log.InitializedWithCollector(collector.GetType().Name);
                }
            }

            return collector;
        }
#endif

        /// <summary>
        /// Formats a counter into a readable string.
        /// </summary>
        /// <param name="pc">Performance counter structure.</param>
        public static string FormatPerformanceCounter(PerformanceCounterStructure pc)
        {
            return FormatPerformanceCounter(pc.CategoryName, pc.CounterName, pc.InstanceName);
        }

        /// <summary>
        /// Searches for the environment variable specific to Azure Web App.
        /// </summary>
        /// <returns>Boolean, which is true if the current application is an Azure Web App.</returns>
        public static bool IsWebAppRunningInAzure()
        {
            if (!isAzureWebApp.HasValue)
            {
                try
                {
                    // Presence of "WEBSITE_SITE_NAME" indicate web apps.
                    // "WEBSITE_ISOLATION"!="hyperv" indicate premium containers. In this case, perf counters
                    // can be read using regular mechanism and hence this method retuns false for
                    // premium containers.
                    isAzureWebApp = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(WebSiteEnvironmentVariable)) &&
                catch (Exception ex)
                {
                    PerformanceCollectorEventSource.Log.AccessingEnvironmentVariableFailedWarning(WebSiteEnvironmentVariable, ex.ToString());
                    return false;
                }
            }

            return (bool)isAzureWebApp;
        }

        public static int? GetProcessorCount()
        {
            int count;
            try
            {
                count = Environment.ProcessorCount;
            }
            catch (Exception ex)
            {
                PerformanceCollectorEventSource.Log.ProcessorsCountIncorrectValueError(ex.ToString());
#endif
            }
            else
            {
                return StandardSdkVersionPrefix;
            }
        }

        /// <summary>
        /// Formats a counter into a readable string.
        /// </summary>
        public static string FormatPerformanceCounter(string categoryName, string counterName, string instanceName)
        {
            if (string.IsNullOrWhiteSpace(instanceName))
            {
                return string.Format(CultureInfo.InvariantCulture, @"\{0}\{1}", categoryName, counterName);
            }

            return string.Format(
                CultureInfo.InvariantCulture,
            out string error)
        {
            error = null;

            try
            {
                return PerformanceCounterUtility.ParsePerformanceCounter(
                    perfCounterName,
                    win32Instances,
                    clrInstances,
                error = e.Message;

                return null;
            }
        }

        /// <summary>
        /// Parses a performance counter canonical string into a PerformanceCounter object.
        /// </summary>
        /// <remarks>This method also performs placeholder expansion.</remarks>
            IEnumerable<string> clrInstances,
            bool supportInstanceNames,
            out bool usesInstanceNamePlaceholder)
        {
            var match = PerformanceCounterRegex.Match(performanceCounter);

            if (!match.Success)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        @"Invalid performance counter name format: {0}. Expected formats are \category(instance)\counter or \category\counter",
                        performanceCounter),
                    nameof(performanceCounter));
            }

            return new PerformanceCounterStructure()
            {
                CategoryName = match.Groups["categoryName"].Value,
                InstanceName =
                        clrInstances,
                        supportInstanceNames,
                        out usesInstanceNamePlaceholder),
                CounterName = match.Groups["counterName"].Value,
            };
        }

        /// <summary>
        /// Invalidates placeholder cache.
        /// </summary>
        public static void InvalidatePlaceholderCache()
        {
            PlaceholderCache.Clear();
        }

        /// <summary>
        /// Matches an instance name against the placeholder regex.
        /// </summary>
        /// <param name="instanceName">Instance name to match.</param>
        /// <returns>Regex match.</returns>
            var match = InstancePlaceholderRegex.Match(instanceName);

            if (!match.Success || !match.Groups["placeholder"].Success)
            {
                return null;
            }

            return match;
        }

            string name = AppDomain.CurrentDomain.FriendlyName;
#endif

            return GetInstanceFromApplicationDomain(name);
        }

        internal static string GetInstanceFromApplicationDomain(string domainFriendlyName)
        {
            const string Separator = "-";

                Separator,
                segments.Take(segments.Length > 2 ? segments.Length - 2 : segments.Length));

            return nameWithoutTrailingData.Replace('/', '_');
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "This method has different code for Net452/NetCore")]
        internal static string GetInstanceForWin32Process(IEnumerable<string> win32Instances)
        {
            return FindProcessInstance(
        }

        internal static IList<string> GetClrProcessInstances()
        {
            return GetInstances(ClrProcessCategoryName);
        }

        private static string ExpandInstanceName(
            string instanceName,
            IEnumerable<string> win32Instances,
                // not a placeholder, do not expand
                usesPlaceholder = false;
                return instanceName;
            }

            usesPlaceholder = true;

            var placeholder = match.Groups["placeholder"].Value;

            // use a cached value if available
            string cachedResult;
            if (PlaceholderCache.TryGetValue(placeholder, out cachedResult))
            {
                return cachedResult;
            }

            // expand
            if (string.Equals(placeholder, Win32ProcessInstancePlaceholder, StringComparison.OrdinalIgnoreCase))
            {
                cachedResult = GetInstanceForWin32Process(win32Instances);
            }
            else if (string.Equals(placeholder, ClrProcessInstancePlaceholder, StringComparison.OrdinalIgnoreCase))
            {
                cachedResult = GetInstanceForClrProcess(clrInstances);
            }
            else if (string.Equals(placeholder, W3SvcProcessInstancePlaceholder, StringComparison.OrdinalIgnoreCase))
            {
                cachedResult = GetInstanceForCurrentW3SvcWorker();
            }
            else
        {
            var cat = new PerformanceCounterCategory() { CategoryName = categoryName };

            try
            {
                return cat.GetInstanceNames();
            }
            catch (Exception)
            {
                // something went wrong and the category hasn't been found


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
