namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse
{
    using System;
    using System.Collections.Concurrent;

    using Microsoft.ApplicationInsights.Extensibility.Filtering;
    using Microsoft.ManagementServices.RealTimeDataProcessing.QuickPulseService;

    /// </summary>
        public long AIRequestSuccessCount;

        public long AIRequestFailureCount;

        // MSB for the sign, 19 bits for dependency call count, 44 LSBs for duration in ticks
        public long AIDependencyCallCountAndDurationInTicks;

        public long AIDependencyCallSuccessCount;

        public long AIDependencyCallFailureCount;

        public long AIExceptionCount;

        public bool GlobalDocumentQuotaReached;

        /// <summary>
        /// MaxCount = 2^19 - 1.
        /// </summary>
        private const long MaxCount = 524287;

        /// <summary>
        }

        public long AIRequestCount => QuickPulseDataAccumulator.DecodeCountAndDuration(this.AIRequestCountAndDurationInTicks).Item1;

        public long AIRequestDurationInTicks => QuickPulseDataAccumulator.DecodeCountAndDuration(this.AIRequestCountAndDurationInTicks).Item2;

        public long AIDependencyCallCount => QuickPulseDataAccumulator.DecodeCountAndDuration(this.AIDependencyCallCountAndDurationInTicks).Item1;
            {
                // this should never happen, but better have a 0 than garbage
                return 0;
            }

            return (count << 44) + duration;
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
