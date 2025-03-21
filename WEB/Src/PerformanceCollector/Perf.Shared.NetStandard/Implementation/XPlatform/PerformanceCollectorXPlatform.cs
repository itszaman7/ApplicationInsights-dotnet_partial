namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.XPlatform
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Microsoft.ApplicationInsights.Common;

    internal class PerformanceCollectorXPlatform : IPerformanceCollector
    {
        private readonly List<Tuple<PerformanceCounterData, ICounterValue>> performanceCounters = new List<Tuple<PerformanceCounterData, ICounterValue>>();

        /// <summary>
        /// Gets a collection of registered performance counters.
        /// </summary>
        public IEnumerable<PerformanceCounterData> PerformanceCounters
        {
            get { return this.performanceCounters.Select(t => t.Item1).ToList(); }
        }

        /// <summary>
        /// Performs collection for all registered counters.
        /// </summary>
        /// <param name="onReadingFailure">Invoked when an individual counter fails to be read.</param>
        public IEnumerable<Tuple<PerformanceCounterData, double>> Collect(
            Action<string, Exception> onReadingFailure = null)
        {
            return this.performanceCounters.Where(pc => !pc.Item1.IsInBadState).SelectMany(
                counter =>
                    {
                        double value;

                        try
                        {
                            value = CollectCounter(counter.Item1.OriginalString, counter.Item2);
                        }
                        catch (InvalidOperationException e)
                        {
                            onReadingFailure?.Invoke(counter.Item1.OriginalString, e);
                
                if (pc != null)
                {
                    this.RegisterPerformanceCounter(perfCounter, GetCounterReportAsName(perfCounter, reportAs), pc.CategoryName, pc.CounterName, pc.InstanceName, useInstancePlaceHolder);
                }
                else
                {
                    // Even if validation failed, we might still be able to collect perf counter in WebApp.
                    this.RegisterPerformanceCounter(perfCounter, GetCounterReportAsName(perfCounter, reportAs), string.Empty, perfCounter, string.Empty, useInstancePlaceHolder);
                }                
            }
            catch (Exception e)
            {
                PerformanceCollectorEventSource.Log.WebAppCounterRegistrationFailedEvent(
                    e.Message,
                    perfCounter);
                error = e.Message;
            }
        }


            if (keyToRemove != null)
            {
                this.performanceCounters.Remove(keyToRemove);
            }
        }

        /// <summary>
        /// Rebinds performance counters to Windows resources.
        /// </summary>
        /// </summary>
        private static double CollectCounter(string counterOriginalString, ICounterValue counter)
        {
            try
            {
                return counter.Collect();
            }
            catch (Exception e)
            {
                 throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        "Failed to perform a read for web app performance counter {0}",
                        counterOriginalString),
                    e);
            }
        }

        /// <summary>
        /// Gets metric alias to be the value given by the user.
            ICounterValue counter = null;

            try
            {
                counter = CounterFactoryXPlatform.GetCounter(originalString);
            }
            catch
            {
                PerformanceCollectorEventSource.Log.CounterNotXPlatformSupported(originalString);
                return;

            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        "Failed to perform the first read for web app performance counter. Please make sure it exists. Counter: {0}",
                        counterName),
                    e);
            }
            finally
            {
                PerformanceCounterData perfData = new PerformanceCounterData(
                        originalString,
                        reportAs,
                        usesInstanceNamePlaceholder,
                        !firstReadOk,
                        categoryName,
                        counterName,
                        instanceName);

                this.performanceCounters.Add(new Tuple<PerformanceCounterData, ICounterValue>(perfData, counter));


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
