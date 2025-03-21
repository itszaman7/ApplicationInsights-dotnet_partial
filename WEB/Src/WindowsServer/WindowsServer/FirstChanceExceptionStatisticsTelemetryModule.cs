#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using System.Threading;

    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation;

    /// <summary>
    /// The module subscribed to AppDomain.CurrentDomain.FirstChanceException to send exceptions statistics to ApplicationInsights.
    /// </summary>
    public sealed class FirstChanceExceptionStatisticsTelemetryModule : ITelemetryModule, IDisposable
    {
        internal const int OperationNameCacheSize = 100;
        internal const int ProblemIdCacheSize = 10000;

        internal const double CurrentWeight = .7;
        internal const double NewWeight = .3;
        internal const long TicksMovingAverage = 100000000; // 10 seconds

        internal const string OperationNameTag = "ai.operation.name";

        internal long MovingAverageTimeout;
        internal double TargetMovingAverage = 5000;

        // cheap dimension capping
        internal long DimCapTimeout;

        internal MetricManager MetricManager;

        private const int LOCKED = 1;
        private const int UNLOCKED = 0;

        /// <summary>
        /// This object prevents double entry into the exception callback.
        /// </summary>
        [ThreadStatic]
        private static int executionSyncObject;

        private readonly Action<EventHandler<FirstChanceExceptionEventArgs>> registerAction;
        private readonly Action<EventHandler<FirstChanceExceptionEventArgs>> unregisterAction;
        private readonly object lockObject = new object();
        private readonly object movingAverageLockObject = new object();

        private TelemetryClient telemetryClient;

        private bool isInitialized = false;

        private long newThreshold = 0;
        private long newProcessed = 0;
        private double currentMovingAverage = 0;

        private long cacheLifetime = 300000000; // 30 seconds in ticks

        private HashCache<string> operationNameValues = new HashCache<string>();
        private HashCache<string> problemIdValues = new HashCache<string>();
        private HashCache<string> exceptionKeyValues = new HashCache<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstChanceExceptionStatisticsTelemetryModule" /> class.
        /// </summary>
        public FirstChanceExceptionStatisticsTelemetryModule() : this(
            action => AppDomain.CurrentDomain.FirstChanceException += action,
            action => AppDomain.CurrentDomain.FirstChanceException -= action)
        {
        }

        internal FirstChanceExceptionStatisticsTelemetryModule(
            Action<EventHandler<FirstChanceExceptionEventArgs>> registerAction,
            Action<EventHandler<FirstChanceExceptionEventArgs>> unregisterAction)
        {
            this.DimCapTimeout = DateTime.UtcNow.Ticks + this.cacheLifetime;
            this.MovingAverageTimeout = DateTime.UtcNow.Ticks - 1; // Setting the timeout to be expired

            this.registerAction = registerAction;
            this.unregisterAction = unregisterAction;
        }

        /// <summary>
        /// Initializes the telemetry module.
        /// </summary>
        /// <param name="configuration">Telemetry Configuration used for creating TelemetryClient for sending exception statistics to Application Insights.</param>
                    {
                        this.telemetryClient = new TelemetryClient(configuration);
                        this.telemetryClient.Context.GetInternalContext().SdkVersion = SdkVersionUtils.GetSdkVersion("exstat:");

                        this.MetricManager = new MetricManager(this.telemetryClient, configuration);

                        this.registerAction(this.CalculateStatistics);

                        this.isInitialized = true;
                    }
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal static bool WasExceptionTracked(Exception exception)
        {
            // some exceptions like MemoryOverflow, ThreadAbort or ExecutionEngine are pre-instantiated 
            // so the .Data is not writable. Also it can be null in certain cases.
            if (exception.Data != null && !exception.Data.IsReadOnly)
                {
                    // mark exception as tracked
                    exception.Data[trackingId] = null; // The value is unimportant. It's just a sentinel.
                }
            }

            return false;

            //// This is temporarily being commented out to capture outer exceptions. It will be modified later. 
            ////if (!wasTracked)
            ////{
            ////    var innerException = exception.InnerException;
            ////    if (innerException != null)
            ////    {
            ////        wasTracked = IsTracked(innerException);
            ////    }
            ////}

            ////if (!wasTracked && exception is AggregateException)
            ////{

            ////            if (wasTracked == true)
            ////            {
            ////                break;
            ////            }
            ////        }
            ////    }
            ////}
        }

            hashCache.ValueCache.Add(dimensionValue);

            hashCache.RwLock.ExitWriteLock();

            return dimensionValue;
        }

        /// <summary>
        /// IDisposable implementation.
        /// </summary>
            if (executionSyncObject == LOCKED)
            {
                return;
            }

            try
            {
                Exception exception;
                string exceptionType;
                System.Diagnostics.StackFrame exceptionStackFrame;
                            if (this.MovingAverageTimeout + TicksMovingAverage < DateTime.UtcNow.Ticks)
                            {
                                this.currentMovingAverage = 0;
                            }
                            else
                            {
                                this.currentMovingAverage = (this.currentMovingAverage * CurrentWeight) +
                                (((double)this.newProcessed) * NewWeight);
                            }

                }

                exception = firstChanceExceptionArgs?.Exception;

                if (exception == null)
                {
                    WindowsServerEventSource.Log.FirstChanceExceptionCallbackExeptionIsNull();
                    return;
                }

                }

                this.TrackStatistics(getOperationName, problemId, exception);
            }
            catch (Exception exc)
            {
                try
                {
                    WindowsServerEventSource.Log.FirstChanceExceptionCallbackException(exc.ToInvariantString());
                }
                catch (Exception)
                {
                    // this is absolutely critical to not throw out of this method
                    // Otherwise it will affect the customer application behavior significantly
                }
            }
            finally
            {
                executionSyncObject = UNLOCKED;
            }
                dimensions.Add(OperationNameTag, refinedOperationName);
            }
            else
            {
                if (getOperationName == true)
                {
                    exceptionTelemetry = new ExceptionTelemetry(exception);
                    this.telemetryClient.Initialize(exceptionTelemetry);
                    operationName = exceptionTelemetry.Context.Operation.Name;
                }

                if (string.IsNullOrEmpty(operationName) == false)
                {
                    refinedOperationName = GetDimCappedString(operationName, this.operationNameValues, OperationNameCacheSize);

                    dimensions.Add(OperationNameTag, refinedOperationName);
                }
            }

            this.SendException(refinedOperationName, refinedProblemId, exceptionTelemetry, exception);

            metric.Track(1);
        }

        private void SendException(string operationName, string problemId, ExceptionTelemetry exceptionTelemetry, Exception exception)
        {
            string exceptionKey;

            if (string.IsNullOrEmpty(operationName) == true)
            {
                    exceptionTelemetry = new ExceptionTelemetry(exception);
                    exceptionTelemetry.Context.Operation.Name = operationName;
                    this.telemetryClient.Initialize(exceptionTelemetry);
                }

                StackTrace st = new StackTrace(3, true);
                exceptionTelemetry.SetParsedStack(st.GetFrames());

                if (string.IsNullOrEmpty(exceptionTelemetry.ProblemId) == true)
        internal class HashCache<T> : IDisposable
        {
            internal ReaderWriterLockSlim RwLock;
            internal HashSet<T> ValueCache = new HashSet<T>();

            private bool disposedValue = false; // To detect redundant calls

            internal HashCache()
            {
                this.RwLock = new ReaderWriterLockSlim();
                this.ValueCache = new HashSet<T>();
            }

            void IDisposable.Dispose()
            {
                this.Dispose(true);
            }

            // This code added to correctly implement the disposable pattern.
            internal void Dispose()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                this.Dispose(true);
            }

            internal bool Contains(T value)
            {
                bool rc;

                this.RwLock.EnterReadLock();
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!this.disposedValue)
                {
                    if (disposing)
                    {
                        this.RwLock.Dispose();
                    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
