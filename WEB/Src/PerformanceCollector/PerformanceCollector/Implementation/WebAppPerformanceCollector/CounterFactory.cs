﻿namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector
{
    using System;

    /// <summary>
    /// Factory to create different counters.
    /// </summary>
    internal static class CounterFactory
    {
        /// <summary>
        /// Gets a counter.
        /// </summary>
        /// <param name="counterName">Name of the counter to retrieve.</param>
        /// <param name="reportAs">Alias to report the counter under.</param>
        /// <returns>The counter identified by counter name.</returns>
        public static ICounterValue GetCounter(string counterName, string reportAs)
        {
            switch (counterName)
            {
                // Default performance counters
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Request Execution Time":
                    return new RawCounterGauge(
                        reportAs,
                        "appRequestExecTime",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Requests In Application Queue":
                    return new RawCounterGauge(
                        reportAs,
                        "requestsInApplicationQueue",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Requests/Sec":
                    return new RateCounterGauge(
                        reportAs,
                        "requestsTotal",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\.NET CLR Exceptions(??APP_CLR_PROC??)\# of Exceps Thrown / sec":
                    return new RateCounterGauge(
                        reportAs,
                        "exceptionsThrown",
                        AzureWebApEnvironmentVariables.CLR);
                case @"\Process(??APP_WIN32_PROC??)\Private Bytes":
                    return new RawCounterGauge(
                        reportAs,
                        "privateBytes",
                        AzureWebApEnvironmentVariables.App);
                case @"\Process(??APP_WIN32_PROC??)\IO Data Bytes/sec":
                    return new RateCounterGauge(
                        reportAs,
                        "ioDataBytesRate",
                        AzureWebApEnvironmentVariables.App,
                        new SumUpCountersGauge(
                            "ioDataBytesRate",
                            new RawCounterGauge(
                                "readIoBytes",
                                "readIoBytes",
                                AzureWebApEnvironmentVariables.App),
                            new RawCounterGauge(
                                "writeIoBytes",
                                "writeIoBytes",
                                AzureWebApEnvironmentVariables.App),
                            new RawCounterGauge(
                                "otherIoBytes",
                                "otherIoBytes",
                                AzureWebApEnvironmentVariables.App)));
                case @"\Memory\Available Bytes":
                    return new RawCounterGauge(reportAs, "availMemoryBytes", AzureWebApEnvironmentVariables.App);

                ////$set = Get-Counter -ListSet "ASP.NET Applications"
                ////$set.Paths
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Anonymous Requests":
                    return new RawCounterGauge(
                        reportAs,
                        "anonymousRequests",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Anonymous Requests / Sec":
                    return new RateCounterGauge(
                        reportAs,
                        "anonymousRequests",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Cache Total Entries":
                    return new RawCounterGauge(
                        reportAs,
                        "totalCacheEntries",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Cache Total Turnover Rate":
                    return new RateCounterGauge(
                        reportAs,
                        "totalCacheTurnoverRate",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Cache Total Hits":
                    return new RawCounterGauge(
                        reportAs,
                        "totalCacheHits",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Cache Total Misses":
                    return new RawCounterGauge(
                        reportAs,
                        "totalCacheMisses",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Cache API Entries":
                    return new RawCounterGauge(
                        reportAs,
                        "apiCacheEntries",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Cache API Turnover Rate":
                    return new RawCounterGauge(
                        reportAs,
                        "apiCacheTurnoverRate",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Cache API Hits":
                    return new RawCounterGauge(
                        reportAs,
                        "apiCacheHits",
                        AzureWebApEnvironmentVariables.AspDotNet);
                    return new RawCounterGauge(
                        reportAs,
                        "outputCacheEntries",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Output Cache Turnover Rate":
                    return new RawCounterGauge(
                        reportAs,
                        "outputCacheTurnoverRate",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Output Cache Hits":
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Output Cache Misses":
                    return new RawCounterGauge(
                        reportAs,
                        "outputCacheMisses",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Compilations Total":
                    return new RawCounterGauge(
                        reportAs,
                        "compilations",
                    return new RawCounterGauge(
                        reportAs,
                        "errorsPreProcessing",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Errors During Compilation":
                    return new RawCounterGauge(
                        reportAs,
                        "errorsCompiling",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Errors During Execution":
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Errors Total / Sec":
                    return new RateCounterGauge(
                        reportAs,
                        "errorsTotal",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Pipeline Instance Count":
                        reportAs,
                        "pipelines",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Request Bytes In Total":
                    return new RawCounterGauge(
                        reportAs,
                        "requestBytesIn",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Request Bytes Out Total":
                    return new RawCounterGauge(
                    return new RawCounterGauge(
                        reportAs,
                        "requestsTimedOut",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Requests Succeeded":
                    return new RawCounterGauge(
                        reportAs,
                        "requestsSucceded",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Requests Total":
                        "requestsTotal",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Sessions Active":
                    return new RawCounterGauge(
                        reportAs,
                        "sessionsActive",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Sessions Abandoned":
                    return new RawCounterGauge(
                        reportAs,
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Transactions Pending":
                    return new RawCounterGauge(
                        reportAs,
                        "transactionsPending",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Transactions Total":
                    return new RawCounterGauge(
                        reportAs,
                        "transactionsTotal",
                        AzureWebApEnvironmentVariables.AspDotNet);
                        reportAs,
                        "eventsTotal",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Events Raised / Sec":
                    return new RateCounterGauge(
                        reportAs,
                        "eventsTotal",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Application Lifetime Events":
                    return new RawCounterGauge(
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Application Lifetime Events / Sec":
                    return new RateCounterGauge(
                        reportAs,
                        "eventsApp",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Error Events Raised":
                    return new RawCounterGauge(
                        reportAs,
                        "eventsError",
                        "eventsHttpInfraError",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Request Events Raised":
                    return new RawCounterGauge(
                        reportAs,
                        "eventsWebReq",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Request Events Raised / Sec":
                    return new RateCounterGauge(
                        reportAs,
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Audit Success Events Raised":
                    return new RawCounterGauge(
                        reportAs,
                        "auditSuccess",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Audit Failure Events Raised":
                    return new RawCounterGauge(
                        reportAs,
                        "auditFail",
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Membership Authentication Failure":
                    return new RawCounterGauge(
                        reportAs,
                        "memberFail",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Forms Authentication Success":
                    return new RawCounterGauge(
                        reportAs,
                        "formsAuthSuccess",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Forms Authentication Failure":
                    return new RawCounterGauge(
                        reportAs,
                        "formsAuthFail",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Viewstate MAC Validation Failure":
                    return new RawCounterGauge(
                        reportAs,
                        "viewstateMacFail",
                        AzureWebApEnvironmentVariables.AspDotNet);
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Output Cache Trims":
                    return new RawCounterGauge(
                        reportAs,
                        "cacheOutputTrims",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\% Managed Processor Time(estimated)":
                    // maybe appCpuUsed and appCpuUsedBase
                    throw new ArgumentException("Performance counter was not found.", counterName);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Managed Memory Used(estimated)":
                    return new RawCounterGauge(
                        reportAs,
                        "appMemoryUsed",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Request Bytes In Total(WebSockets)":
                    return new RawCounterGauge(
                        reportAs,
                        "requestBytesInWebsockets",
                        AzureWebApEnvironmentVariables.AspDotNet);
                case @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Request Bytes Out Total(WebSockets)":
                        AzureWebApEnvironmentVariables.AspDotNet);

                //// $set = Get-Counter -ListSet Process
                //// $set.Paths
                case @"\Process(??APP_WIN32_PROC??)\% Processor Time":
                    return new CPUPercenageGauge(
                        reportAs, 
                        new SumUpCountersGauge(
                            reportAs,
                            new RawCounterGauge("kernelTime", "kernelTime", AzureWebApEnvironmentVariables.App),
                case @"\Process(??APP_WIN32_PROC??)\IO Write Operations / sec":
                    return new RateCounterGauge(
                        reportAs,
                        "writeIoOperations",
                        AzureWebApEnvironmentVariables.App);
                case @"\Process(??APP_WIN32_PROC??)\IO Other Operations / sec":
                    return new RateCounterGauge(
                        reportAs,
                        "otherIoOperations",
                        AzureWebApEnvironmentVariables.App);
                        reportAs,
                        "readIoBytes",
                        AzureWebApEnvironmentVariables.App);
                case @"\Process(??APP_WIN32_PROC??)\IO Write Bytes / sec":
                    return new RateCounterGauge(
                        reportAs,
                        "writeIoBytes",
                        AzureWebApEnvironmentVariables.App);
                case @"\Process(??APP_WIN32_PROC??)\IO Other Bytes / sec":
                    return new RateCounterGauge(
                        reportAs,
                        "otherIoBytes",
                        AzureWebApEnvironmentVariables.App);
                case @"\Process(??APP_WIN32_PROC??)\Working Set - Private":

                ////$set = Get - Counter - ListSet ".NET CLR Memory"
                ////$set.Paths
                case @"\.NET CLR Memory(??APP_CLR_PROC??)\# Gen 0 Collections":
                    return new RawCounterGauge(
                        reportAs,
                        "gen0Collections",
                        AzureWebApEnvironmentVariables.CLR);
                case @"\.NET CLR Memory(??APP_CLR_PROC??)\# Gen 1 Collections":
                    return new RawCounterGauge(
                        reportAs,
                        "gen1Collections",
                        AzureWebApEnvironmentVariables.CLR);
                case @"\.NET CLR Memory(??APP_CLR_PROC??)\# Gen 2 Collections":
                    return new RawCounterGauge(
                        reportAs,
                        "gen2Collections",
                        AzureWebApEnvironmentVariables.CLR);
                case @"\.NET CLR Memory(??APP_CLR_PROC??)\Gen 0 heap size":
                    return new RawCounterGauge(
                        reportAs,
                        "gen0HeapSize",
                        AzureWebApEnvironmentVariables.CLR);
                case @"\.NET CLR Memory(??APP_CLR_PROC??)\Gen 1 heap size":
                    return new RawCounterGauge(
                        reportAs,
                        "gen1HeapSize",
                        AzureWebApEnvironmentVariables.CLR);
                case @"\.NET CLR Memory(??APP_CLR_PROC??)\Gen 2 heap size":
                    return new RawCounterGauge(
                        reportAs,
                        "gen2HeapSize",
                        AzureWebApEnvironmentVariables.CLR);
                case @"\.NET CLR Memory(??APP_CLR_PROC??)\Large Object Heap size":
                    return new RawCounterGauge(
                        reportAs,
                case @"\.NET CLR Memory(??APP_CLR_PROC??)\# Total reserved Bytes":
                    return new RawCounterGauge(
                        reportAs,
                        "reservedBytes",
                        AzureWebApEnvironmentVariables.CLR);
                case @"\.NET CLR Memory(??APP_CLR_PROC??)\# of Pinned Objects":
                    return new RawCounterGauge(
                        reportAs,
                        "pinnedObjects",
                        AzureWebApEnvironmentVariables.CLR);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
