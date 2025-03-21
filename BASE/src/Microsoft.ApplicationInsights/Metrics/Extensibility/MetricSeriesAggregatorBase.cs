﻿namespace Microsoft.ApplicationInsights.Metrics.Extensibility
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using static System.FormattableString;  

    /// <summary>The abstract base contain functionaity shared by most aggregators.</summary>
    /// <seealso cref="IMetricSeriesAggregator"/>
    /// <typeparam name="TBufferedValue">The actual type of the metric values. For most common metrics it's <c>double</c>.
    /// However, for example a metric collecting strings to d-count the number of distinct entities might have <c>string</c>.</typeparam>
    /// @PublicExposureCandidate
    internal abstract class MetricSeriesAggregatorBase<TBufferedValue> : IMetricSeriesAggregator
    {
        private readonly MetricSeries dataSeries;
        private readonly MetricAggregationCycleKind aggregationCycleKind;
        private readonly bool isPersistent;
        private readonly Func<MetricValuesBufferBase<TBufferedValue>> metricValuesBufferFactory;

        private DateTimeOffset periodStart;
        private IMetricValueFilter valueFilter;

        private volatile MetricValuesBufferBase<TBufferedValue> metricValuesBuffer;
        private volatile MetricValuesBufferBase<TBufferedValue> metricValuesBufferRecycle = null;

        /// <summary>Initializes an aggregator instance.</summary>
        /// <param name="metricValuesBufferFactory">Creates a values buffer appropriate for this aggregator.</param>
        /// <param name="configuration">Configuration of the metric series ggregated by this aggregator.</param>
        /// <param name="dataSeries">The metric series ggregated by this aggregator.</param>
        /// <param name="aggregationCycleKind">The kind of the aggregation cycle that uses this aggregator.</param>
        protected MetricSeriesAggregatorBase(
                                        Func<MetricValuesBufferBase<TBufferedValue>> metricValuesBufferFactory,
                                        IMetricSeriesConfiguration configuration,
                                        MetricSeries dataSeries,
                                        MetricAggregationCycleKind aggregationCycleKind)
        {
            Util.ValidateNotNull(metricValuesBufferFactory, nameof(metricValuesBufferFactory));
            Util.ValidateNotNull(configuration, nameof(configuration));

            this.dataSeries = dataSeries;
            this.aggregationCycleKind = aggregationCycleKind;
            this.isPersistent = configuration.RequiresPersistentAggregation;

            this.metricValuesBufferFactory = metricValuesBufferFactory;
            this.metricValuesBuffer = this.InvokeMetricValuesBufferFactory();

            this.Reset(default(DateTimeOffset), default(IMetricValueFilter));
        }

        /// <summary>Gets the data series aggregated by this aggregator.</summary>
        public MetricSeries DataSeries
        {
            get { return this.dataSeries; }
        }

        /// <summary>Finishes the aggregation cycle.</summary>
        /// <param name="periodEnd">Cycle end timestamp.</param>
        /// <returns>The aggregate summarizing the completed cycle.</returns>
        public MetricAggregate CompleteAggregation(DateTimeOffset periodEnd)
        {
            if (!this.isPersistent)
            {
                this.DataSeries.ClearAggregator(this.aggregationCycleKind);
            }

            MetricAggregate aggregate = this.CreateAggregateUnsafe(periodEnd);
            return aggregate;
        }

        /// <summary>Clear the state of this aggregator to allow it to be reused for a new aggregation cycle.</summary>
        /// <param name="periodStart">start time of the new cycle.</param>
        public void Reset(DateTimeOffset periodStart)
        {
            this.periodStart = periodStart;

            this.metricValuesBuffer.ResetIndicesAndData();

            this.ResetAggregate();
        }

        /// <summary>Clear the state of this aggregator to allow it to be reused for a new aggregation cycle.</summary>
        /// <param name="periodStart">Start time of the new cycle.</param>
        /// <param name="valueFilter">Values filter for the new cycle.</param>
        public void Reset(DateTimeOffset periodStart, IMetricValueFilter valueFilter)
            {
                return false;
            }

            this.Reset(default(DateTimeOffset), default(IMetricValueFilter));
            return true;
        }

        /// <summary>Get an aggregate of the values tracked so far without completing the ongoing aggregation cycle.
        /// May not be thread-safe.</summary>

            return this.CreateAggregate(periodEnd);
        }

        #region Abstract Methods

        /// <summary>Concrete aggregator imlemenations override this to actually create an aggregate form the tracked matric values.</summary>
        /// <param name="periodEnd">Timestamp of the aggregation period end.</param>
        /// <returns>A new metric aggregate.</returns>
        protected abstract MetricAggregate CreateAggregate(DateTimeOffset periodEnd);
        /// <summary>
        /// Aggregators implement updating aggregate state from buffer by implemenmting this method (<c>UpdateAggregate_Stage1</c>)
        /// <param name="buffer">Interval values buffer.</param>
        /// <param name="minFlushIndex">Buffer index start to process. {764}.</param>
        /// <param name="maxFlushIndex">Buffer index end to process.</param>
        /// <returns>State for passing data from stage 1 to stage 2.</returns>
        protected abstract object UpdateAggregate_Stage1(MetricValuesBufferBase<TBufferedValue> buffer, int minFlushIndex, int maxFlushIndex);

        /// <summary>
        /// Aggregators implement updating aggregate state from buffer by implemenmting this method (<c>UpdateAggregate_Stage2</c>)
        /// and its sibling method <c>UpdateAggregate_Stage1</c>. Stage 1 is the part of the update that needs to happen while holding
        /// a lock on the <c>metric values buffer</c> (e.g. extracting a summary from the buffer). Stage 2 is the part of the update
        protected abstract void UpdateAggregate_Stage2(object stage1Result);

        #endregion Abstract Methods

        /// <summary>Populate common fields of just-constructed metric aggregate.</summary>
        /// <param name="aggregate">Aggregate being initialized.</param>
        /// <param name="periodEnd">End of the cycle represented by the aggregate.</param>
        protected void AddInfo_Timing_Dimensions_Context(MetricAggregate aggregate, DateTimeOffset periodEnd)
        {
            if (aggregate == null)
                    }
                }
            }
        }

#if DEBUG
#pragma warning disable SA1307 // Accessible fields must begin with upper-case letter
#pragma warning disable SA1201 // Elements must appear in the correct order
#pragma warning disable CS3026 // CLS-compliant field cannot be volatile

        private void TrackFilteredConvertedValue(TBufferedValue metricValue)
        {
            // Get reference to the current buffer:
            MetricValuesBufferBase<TBufferedValue> buffer = this.metricValuesBuffer;

            // Get the index at which to store metricValue into the buffer:
            int index = buffer.IncWriteIndex();

            // Check to see whether we are past the end of the buffer. 
            // If we are, it means that some *other* thread hit exactly the end (wrote the last value that fits into the buffer) and is currently flushing.
                    unchecked
                    {
                        Interlocked.Increment(ref countBufferWaitSpinCycles);
                    }
#endif
                    if (spinWait.Count % 100 == 0)
                    {
                        // In tests (including stress tests) we always finished wating before 100 cycles.
                        // However, this is a protection against en extreme case on a slow machine.
                        // We will back off and sleep for a few millisecs to give the machine a chance to finish current tasks.
                    index = buffer.IncWriteIndex();
                }
#if DEBUG
                unchecked
                {
                    int periodMillis = Environment.TickCount - startMillis;
                    int currentSpinMillis = timeBufferWaitSpinMillis;
                    int prevSpinMillis = Interlocked.CompareExchange(ref timeBufferWaitSpinMillis, currentSpinMillis + periodMillis, currentSpinMillis);
                    while (prevSpinMillis != currentSpinMillis)
                    {
            buffer.WriteValue(index, metricValue);

            // If this was the last value that fits into the buffer, we must flush the buffer:
            if (index == buffer.Capacity - 1)
            {
                // Before we begin flushing (which is can take time), we update the _metricValuesBuffer to a fresh buffer that is ready to take values.
                // That way threads do notneed to spin and wait until we flush and can begin writing values.

                // We try to recycle a previous buffer to lower stress on GC and to lower Gen-2 heap fragmentation.
                // The lifetime of an buffer can easily be a minute or so and then it can get into Gen-2 GC heap.
                // If we then, keep throwing such buffers away we can fragment the Gen-2 heap. To avoid this we employ
                // a simple form of best-effort object pooling.

                // Get buffer from pool and reset the pool:
                MetricValuesBufferBase<TBufferedValue> newBufer = Interlocked.Exchange(ref this.metricValuesBufferRecycle, null);
                
                if (newBufer != null)
                {
                    // If we were succesful in getting a recycled buffer from the pool, we will try to use it as the new buffer.
                    // If we successfully the the recycled buffer to be the new buffer, we will reset it to prepare for data.

                    MetricValuesBufferBase<TBufferedValue> prevBuffer = Interlocked.CompareExchange(ref this.metricValuesBuffer, newBufer, buffer);
                    if (prevBuffer == buffer)
                    {
                        newBufer.ResetIndices();
                    }
                }
                else
                {
                    // If we were succesful in getting a recycled buffer from the pool, we will create a new one.

                    newBufer = this.InvokeMetricValuesBufferFactory();
                    Interlocked.CompareExchange(ref this.metricValuesBuffer, newBufer, buffer);
                }

                // Ok, now we have either set a new buffer that is ready to be used, or we have determined using CompareExchange
                // that another thread set a new buffer and we do not need to do it here.

                // Now we can actually flush the buffer:

#if DEBUG
            unchecked
            {
                Interlocked.Increment(ref countBufferFlushes);
            }
#endif

            object stage1Result;

            // This lock is only contended if a user called CreateAggregateUnsafe or CompleteAggregation.
            // This is very unlikely to be the case in a tight loop.
            lock (buffer)
            {
                int maxFlushIndex = Math.Min(buffer.PeekLastWriteIndex(), buffer.Capacity - 1);
                int minFlushIndex = buffer.NextFlushIndex;

                if (minFlushIndex > maxFlushIndex)
                {
                    return;
                }

            if (buffer == null)
            {
                throw new InvalidOperationException(Invariant($"{nameof(this.metricValuesBufferFactory)}-delegate returned null. This is not allowed. Bad aggregator?"));
            }

            return buffer;
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
