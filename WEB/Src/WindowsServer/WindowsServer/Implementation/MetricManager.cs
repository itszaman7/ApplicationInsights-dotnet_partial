#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Platform;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    using TaskEx = System.Threading.Tasks.Task;

    /// <summary>
    /// Provides functionality to process metric values prior to aggregation.
    /// </summary>
    internal interface IMetricProcessor
    {
        /// <summary>
        /// Process metric value.
        /// </summary>
        /// <param name="metric">Metric definition.</param>
        /// <param name="value">Metric value.</param>
        void Track(Metric metric, double value);
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "This is a temporary private MetricManager")]
    internal static class MetricTerms
    {
        private const string MetricPropertiesNamePrefix = "_MS";

        public static class Aggregation
        {
            public static class Interval
            {
                public static class Moniker
                {
                    public const string Key = MetricPropertiesNamePrefix + ".AggregationIntervalMs";
                }
            }
        }

        public static class Extraction
        {
            public static class ProcessedByExtractors
            {
                public static class Moniker
                {
                    public const string Key = MetricPropertiesNamePrefix + ".ProcessedByMetricExtractors";
                    public const string ExtractorInfoTemplate = "(Name:'{0}', Ver:'{1}')";      // $"(Name:'{ExtractorName}', Ver:'{ExtractorVersion}')"
                }
            }
        }

        public static class Autocollection
        {
            public static class Moniker
            {
                public const string Key = MetricPropertiesNamePrefix + ".IsAutocollected";
                public const string Value = "True";
            }

            public static class MetricId
            {
                public static class Moniker
                {
                    public const string Key = MetricPropertiesNamePrefix + ".MetricId";
                }
            }

            public static class Metric
            {
                public static class RequestDuration
                {
                    public const string Name = "Server response time";
                    public const string Id = "requests/duration";
                }

                public static class DependencyCallDuration
                {
                    public const string Name = "Dependency duration";
                    public const string Id = "dependencies/duration";
                }
            }

            public static class Request
            {
                public static class PropertyNames
                {
                    public const string Success = "Request.Success";
                }
            }

            public static class DependencyCall
            {
                public static class PropertyNames
                {
                    public const string Success = "Dependency.Success";
                    public const string TypeName = "Dependency.Type";
                }

                public static class TypeNames
                {
                    public const string Other = "Other";
                    public const string Unknown = "Unknown";
                }
            }
        }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "This is a temporary private MetricManager")]
    internal static class EventSourceKeywords
    {
        public const long UserActionable = 0x1;

        public const long Diagnostics = 0x2;

        public const long VerboseFailure = 0x4;

        public const long ErrorFailure = 0x8;
    /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#send-metrics">Learn more</a>
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "This is a temporary private MetricManager")]
    internal sealed class MetricManager : IDisposable
    {
        /// <summary>
        /// Value of the property indicating 'app insights version' allowing to tell metric was built using metric manager.
        /// </summary>
        private static string sdkVersionPropertyValue = SdkVersionUtils.GetSdkVersion("m-agg:");

        /// <summary>
        /// Reporting frequency.
        /// </summary>
        private static TimeSpan aggregationPeriod = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Telemetry client used to track resulting aggregated metrics.
        /// </summary>
        private readonly TelemetryClient telemetryClient;

        /// <summary>
        /// Telemetry config for this telemetry client.
        /// </summary>
        private readonly TelemetryConfiguration telemetryConfig;

        private SnapshottingList<IMetricProcessor> metricProcessors = new SnapshottingList<IMetricProcessor>();

        /// <summary>
        /// Metric aggregation snapshot task.
        /// </summary>
        private TaskTimerInternal snapshotTimer;

        /// <summary>
        /// Last time snapshot was initiated.
        /// </summary>
        private DateTimeOffset lastSnapshotStartDateTime;

        /// <summary>
        /// A dictionary of all metrics instantiated via this manager.
        /// </summary>
            this.snapshotTimer = new TaskTimerInternal() { Delay = GetWaitTime() };
            this.snapshotTimer.Start(this.SnapshotAndReschedule);
        }

        /// <summary>
        /// Gets a list of metric processors associated
        /// with this instance of <see cref="MetricManager"/>.
        /// </summary>
        internal IList<IMetricProcessor> MetricProcessors
        {
            {
                this.Snapshot();
                this.telemetryClient.Flush();
            }
            catch (Exception ex)
            {
                WindowsServerCoreEventSource.Log.FailedToFlushMetricAggregators(ex.ToString());
            }
        }

        internal SimpleMetricStatisticsAggregator GetStatisticsAggregator(Metric metric)
        {
            return this.metricDictionary.GetOrAdd(metric, (m) => { return new SimpleMetricStatisticsAggregator(); });
        }

        /// <summary>
        /// Calculates wait time until next snapshot of the aggregators.
        /// </summary>
        /// <returns>Wait time.</returns>
        private static TimeSpan GetWaitTime()
        }

        /// <summary>
        /// Takes a snapshot of aggregators collected by this instance of the manager
        /// and schedules the next snapshot.
        /// </summary>
        private Task SnapshotAndReschedule()
        {
            return Task.Factory.StartNew(
                () =>
                    }
                    finally
                    {
                        this.snapshotTimer.Delay = GetWaitTime();
                        this.snapshotTimer.Start(this.SnapshotAndReschedule);
                    }
                });
        }

        /// <summary>
        /// </summary>
        private void Snapshot()
        {
            ConcurrentDictionary<Metric, SimpleMetricStatisticsAggregator> aggregatorSnapshot =
                Interlocked.Exchange(ref this.metricDictionary, new ConcurrentDictionary<Metric, SimpleMetricStatisticsAggregator>());

            // calculate aggregation interval duration interval
                        this.telemetryClient.Track(aggregatedMetricTelemetry);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Represents mechanism to calculate basic statistical parameters of a series of numeric values.
    /// </summary>
    {
        /// <summary>
        /// Lock to make Track() method thread-safe.
        /// </summary>
        private SpinLock trackLock = new SpinLock();

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleMetricStatisticsAggregator"/> class.
        /// </summary>
        internal SimpleMetricStatisticsAggregator()
        /// <summary>
        /// Gets sum of squares of the samples.
        /// </summary>
        internal double SumOfSquares { get; private set; }

        /// <summary>
        /// Gets minimum sample value.
        /// </summary>
        internal double Min { get; private set; }

        /// <summary>
        /// Gets maximum sample value.
        /// </summary>
        internal double Max { get; private set; }

        /// <summary>
        /// Gets arithmetic average value in the population.
        /// </summary>
        internal double Average
        {
            get
            {
                return this.Count == 0 ? 0 : this.Sum / this.Count;
            }
        }

        /// <summary>
        /// Gets variance of the values in the population.
        /// </summary>
        internal double Variance
            get
            {
                return this.Count == 0 ? 0 : (this.SumOfSquares / this.Count) - (this.Average * this.Average);
            }
        }

        /// <summary>
        /// Gets standard deviation of the values in the population.
        /// </summary>
        internal double StandardDeviation
            bool lockAcquired = false;

            try
            {
                this.trackLock.Enter(ref lockAcquired);

                if ((this.Count == 0) || (value < this.Min))
                {
                    this.Min = value;
                }
                {
                    this.Max = value;
                }

                this.Count++;
                this.Sum += value;
                this.SumOfSquares += value * value;
            }
            finally
            {
    {
        /// <summary>
        /// Aggregator manager for the aggregator.
        /// </summary>
        private readonly MetricManager manager;

        /// <summary>
        /// Metric aggregator id to look for in the aggregator dictionary.
        /// </summary>
        private readonly string aggregatorId;
            this.aggregatorId = Metric.GetAggregatorId(name, dimensions);
            this.hashCode = this.aggregatorId.GetHashCode();
        }

        /// <summary>
        /// Gets metric name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        public void Track(double value)
        {
            SimpleMetricStatisticsAggregator aggregator = this.manager.GetStatisticsAggregator(this);
            aggregator.Track(value);

            this.ForwardToProcessors(value);
        }

        /// <summary>
        /// Returns the hash code for this object.
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public bool Equals(Metric other)
        {
            if (other == null)
            {
                return false;
            }

            return this.aggregatorId.Equals(other.aggregatorId, StringComparison.Ordinal);
        }
        /// <param name="obj">The object to compare with the current object. </param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))

            return this.Equals(obj as Metric);
        }

        /// <summary>
        /// Generates id of the aggregator serving time series specified in the parameters.
        /// </summary>
        /// <param name="name">Metric name.</param>
        /// <param name="dimensions">Optional metric dimensions.</param>
        /// <returns>Aggregator id that can be used to get aggregator.</returns>

            if (dimensions != null)
            {
                var sortedDimensions = dimensions.OrderBy((pair) => { return pair.Key; });

                foreach (KeyValuePair<string, string> pair in sortedDimensions)
                {
                    aggregatorIdBuilder.AppendFormat(CultureInfo.InvariantCulture, "\n{0}\t{1}", pair.Key ?? string.Empty, pair.Value ?? string.Empty);
                }
            }
                        processor.Track(this, value);
                    }
                    catch (Exception ex)
                    {
                        WindowsServerCoreEventSource.Log.FailedToRunMetricProcessor(processor.GetType().FullName, ex.ToString());
                    }
                }
            }
        }
    }
                this.Collection.Clear();
                this.snapshot = default(TCollection);
            }
        }

        public bool Contains(TItem item)
        {
            return this.GetSnapshot().Contains(item);
        }


        public bool Remove(TItem item)
        {
            lock (this.Collection)
            {
                bool removed = this.Collection.Remove(item);
                if (removed)
                {
                    this.snapshot = default(TCollection);
                }

                return removed;
            }
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return this.GetSnapshot().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        protected abstract TCollection CreateSnapshot(TCollection collection);

        protected TCollection GetSnapshot()
        {
            TCollection localSnapshot = this.snapshot;
                {
                    this.snapshot = this.CreateSnapshot(this.Collection);
                    localSnapshot = this.snapshot;
                }
            }

            return localSnapshot;
        }
    }

            {
                return this.GetSnapshot()[index];
            }

            set
            {
                lock (this.Collection)
                {
                    this.Collection[index] = value;
                    this.snapshot = null;
            return this.GetSnapshot().IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            lock (this.Collection)
            {
                this.Collection.Insert(index, item);
                this.snapshot = null;
            }
        }

        public void RemoveAt(int index)
        {
            lock (this.Collection)
            {
                this.Collection.RemoveAt(index);
                this.snapshot = null;
            }
        }
    }

    /// <summary>
    /// Runs a task after a certain delay and log any error.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "This is a temporary private MetricManager")]
    internal class TaskTimerInternal : IDisposable
    {
        /// <summary>
        /// Represents an infinite time span.

        /// <summary>
        /// Gets or sets the delay before the task starts. 
        /// </summary>
        public TimeSpan Delay
        {
            get
            {
                return this.delay;
            }

            set
            {
                if ((value <= TimeSpan.Zero || value.TotalMilliseconds > int.MaxValue) && value != InfiniteTimeSpan)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.delay = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether value that indicates if a task has already started.
        /// </summary>
        public bool IsStarted
        {
            get { return this.tokenSource != null; }
        }

        /// Start the task.
        /// </summary>
        /// <param name="elapsed">The task to run.</param>
        public void Start(Func<Task> elapsed)
        {
            var newTokenSource = new CancellationTokenSource();

            TaskEx.Delay(this.Delay, newTokenSource.Token)
                .ContinueWith(
                    async previousTask =>
                            }
                            catch (Exception exception)
                            {
                                LogException(exception);
                            }
                        },
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Default);

            CancelAndDispose(Interlocked.Exchange(ref this.tokenSource, newTokenSource));
        }

        /// <summary>
        /// Cancels the current task.
        /// </summary>
        public void Cancel()
        {
            CancelAndDispose(Interlocked.Exchange(ref this.tokenSource, null));
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Log exception thrown by outer code.
        /// </summary>
        /// <param name="exception">Exception to log.</param>
        private static void LogException(Exception exception)
        {
            var aggregateException = exception as AggregateException;
            if (aggregateException != null)
            {
                aggregateException = aggregateException.Flatten();
                {
                    WindowsServerCoreEventSource.Log.LogError(e.ToInvariantString());
                }
            }

            WindowsServerCoreEventSource.Log.LogError(exception.ToInvariantString());
        }

        private static void CancelAndDispose(CancellationTokenSource tokenSource)
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
                tokenSource.Dispose();
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Cancel();
            }
        }
    }
    
    [EventSource(Name = "Microsoft-ApplicationInsights-WindowsServer-Core")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "This is a temporary private MetricManager")]
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "appDomainName is required")]
    internal sealed class WindowsServerCoreEventSource : EventSource
    {
        public static readonly WindowsServerCoreEventSource Log = new WindowsServerCoreEventSource();

        private readonly ApplicationNameProvider nameProvider = new ApplicationNameProvider();

        public static bool IsVerboseEnabled
        {
            [NonEvent]
            get
            {
                return Log.IsEnabled(EventLevel.Verbose, (EventKeywords)(-1));
            }
        }

        /// <summary>
        /// Logs the information when there operation to track is null.
        /// </summary>
        [Event(1, Message = "Operation object is null.", Level = EventLevel.Warning)]
        public void OperationIsNullWarning(string appDomainName = "Incorrect")
        {
            this.WriteEvent(1, this.nameProvider.Name);
        }

        /// <summary>
        /// Logs the information when there operation to stop does not match the current operation.
        /// </summary>
        [Event(2, Message = "Operation to stop does not match the current operation.", Level = EventLevel.Error)]
        public void InvalidOperationToStopError(string appDomainName = "Incorrect")
        {
            this.WriteEvent(2, this.nameProvider.Name);
        [Event(
            4,
            Keywords = Keywords.Diagnostics | Keywords.UserActionable,
            Message = "Diagnostics event throttling has been started for the event {0}",
            Level = EventLevel.Informational)]
        public void DiagnosticsEventThrottlingHasBeenStartedForTheEvent(
            string eventId,
            string appDomainName = "Incorrect")
        {
            this.WriteEvent(4, eventId ?? "NULL", this.nameProvider.Name);
            string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                6,
                exception ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            7,
            8,
            Keywords = Keywords.Diagnostics,
            Message = "A scheduler timer was removed",
            Level = EventLevel.Verbose)]
        public void DiagnoisticsEventThrottlingSchedulerTimerWasRemoved(string appDomainName = "Incorrect")
        {
            this.WriteEvent(8, this.nameProvider.Name);
        }

        [Event(
        }

        [Event(
           12,
           Message = "Telemetry tracking was disabled. Message is dropped.",
           Level = EventLevel.Verbose)]
        public void TrackingWasDisabled(string appDomainName = "Incorrect")
        {
            this.WriteEvent(12, this.nameProvider.Name);
        }
           Message = "Telemetry tracking was enabled. Messages are being logged.",
           Level = EventLevel.Verbose)]
        public void TrackingWasEnabled(string appDomainName = "Incorrect")
        {
            this.WriteEvent(13, this.nameProvider.Name);
        }

        [Event(
            14,
            Keywords = Keywords.ErrorFailure,
        {
            this.WriteEvent(
                14,
                msg ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            15,
            Keywords = Keywords.UserActionable,
        public void IncorrectInstanceAtributesConfigurationError(string definition, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                18,
                definition ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            19,
            Level = EventLevel.Error)]
        public void LoadInstanceFromValueConfigurationError(string element, string contents, string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                19,
                element ?? string.Empty,
                contents ?? string.Empty,
                error ?? string.Empty,
                this.nameProvider.Name);
        }
            23,
            Message = "ApplicationInsights configuration file '{0}' was not found.",
            Level = EventLevel.Warning)]
        public void ApplicationInsightsConfigNotFoundWarning(string file, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                23,
                file ?? string.Empty,
                this.nameProvider.Name);
        }
           25,
           Message = "Exception happened during getting the machine name: '{0}'.",
           Level = EventLevel.Error)]
        public void FailedToGetMachineName(string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                25,
                error ?? string.Empty,
                this.nameProvider.Name);
        }
        {
            this.WriteEvent(
                26,
                ex ?? string.Empty,
                this.nameProvider.Name);
        }

        [Event(
            27,
            Message = "Failed to snapshot aggregated metrics. Exception: {0}.",
        }

        [Event(
            30,
            Message = "Flush was called on the telemetry channel (InMemoryChannel) after it was disposed.",
            Level = EventLevel.Warning)]
        public void InMemoryChannelFlushedAfterBeingDisposed(string appDomainName = "Incorrect")
        {
            this.WriteEvent(30, this.nameProvider.Name);
        }

        [Event(
            31,
            Message = "Send was called on the telemetry channel (InMemoryChannel) after it was disposed, the telemetry data was dropped.",
            Level = EventLevel.Warning)]
        public void InMemoryChannelSendCalledAfterBeingDisposed(string appDomainName = "Incorrect")
        {
            this.WriteEvent(31, this.nameProvider.Name);
        }

        [Event(
            32,
            Message = "Failed to get environment variables due to security exception; code is likely running in partial trust. Exception: {0}.",
            Level = EventLevel.Warning)]
        public void FailedToLoadEnvironmentVariables(string ex, string appDomainName = "Incorrect")
        {
            this.WriteEvent(32, ex, this.nameProvider.Name);
        }

        // Verbosity is Error - so it is always sent to portal; Keyword is Diagnostics so throttling is not applied.


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
