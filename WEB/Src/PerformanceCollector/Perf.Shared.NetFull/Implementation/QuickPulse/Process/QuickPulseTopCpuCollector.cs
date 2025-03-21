namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers;

    /// <summary>
    /// Top CPU collector.
    /// </summary>
    internal sealed class QuickPulseTopCpuCollector : IQuickPulseTopCpuCollector
    {
        // process name => (last observation timestamp, last observation value)
        internal readonly Dictionary<string, TimeSpan> ProcessObservations = new Dictionary<string, TimeSpan>(StringComparer.Ordinal);
        private readonly TimeSpan accessDeniedRetryInterval = TimeSpan.FromMinutes(1);

        private readonly Clock timeProvider;

        private readonly IQuickPulseProcessProvider processProvider;

        private DateTimeOffset prevObservationTime;

        private TimeSpan? prevOverallTime;


            this.InitializationFailed = false;
            this.AccessDenied = false;
        }

        /// <summary>
        /// Gets a value indicating whether the initialization has failed.
        /// </summary>
        public bool InitializationFailed { get; private set; }

        /// Gets a value indicating whether the Access Denied error has taken place.
        /// </summary>
        public bool AccessDenied { get; private set; }

        /// <summary>
        /// Gets top N processes by CPU consumption.
        /// </summary>
        /// <param name="topN">Top N processes.</param>
        /// <returns>List of top processes by CPU consumption.</returns>
        public IEnumerable<Tuple<string, int>> GetTopProcessesByCpu(int topN)
            {
                DateTimeOffset now = this.timeProvider.UtcNow;

                if (this.InitializationFailed)
                {
                    // the initialization has failed, so we never attempt to do anything
                    return Enumerable.Empty<Tuple<string, int>>();
                }

                if (this.AccessDenied && now - this.lastReadAttempt < this.accessDeniedRetryInterval)
                {
                    // not enough time has passed since we got denied access, so don't retry just yet
                    return Enumerable.Empty<Tuple<string, int>>();
                }

                var procData = new List<Tuple<string, double>>();
                var encounteredProcs = new HashSet<string>();
                
                this.lastReadAttempt = now;


                this.AccessDenied = false;

                // TODO: implement partial sort instead of full sort below
                return procData.OrderByDescending(p => p.Item2).Select(p => Tuple.Create(p.Item1, (int)(p.Item2 * 100))).Take(topN);
            }
            catch (Exception e)
            {
                QuickPulseEventSource.Log.ProcessesReadingFailedEvent(e.ToInvariantString());

            this.AccessDenied = false;
            
            try
            {
                this.processProvider.Initialize();
            }
            catch (Exception e)
            {
                QuickPulseEventSource.Log.ProcessesReadingFailedEvent(e.ToInvariantString());

        /// <summary>
        /// Closes the top CPU collector.
        /// </summary>
        public void Close()
        {
            this.processProvider.Close();
        }

        private void CleanState(HashSet<string> encounteredProcs)
        {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
