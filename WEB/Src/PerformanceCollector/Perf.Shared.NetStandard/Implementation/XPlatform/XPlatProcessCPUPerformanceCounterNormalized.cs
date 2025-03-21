namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.XPlatform
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// Represents normalized value of CPU Utilization by Process counter value (divided by the processors count).

        /// <summary>
        ///  Initializes a new instance of the <see cref="XPlatProcessCPUPerformanceCounterNormalized" /> class.
                this.processorsCount = count.Value;
                this.isInitialized = true;
            }
        }

        /// <summary>
        /// <returns>Value of the counter.</returns>
        {
            if (!this.isInitialized)
            {
                return 0;
            }

            }

            return result;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
