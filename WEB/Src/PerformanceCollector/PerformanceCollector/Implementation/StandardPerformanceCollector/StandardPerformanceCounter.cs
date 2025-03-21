namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.StandardPerfCollector
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// Interface represents the counter value.
        private PerformanceCounter performanceCounter = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardPerformanceCounter" /> class.
        /// </summary>
        /// <param name="categoryName">The counter category name.</param>
        internal StandardPerformanceCounter(string categoryName, string counterName, string instanceName)
        {
            this.performanceCounter = new PerformanceCounter(categoryName, counterName, instanceName, true);
        }

        /// <summary>
                return this.performanceCounter.NextValue();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    string.Format(
        /// <summary>
        /// Disposes resources allocated by this type.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
                    this.performanceCounter.Dispose();
                    this.performanceCounter = null;
                }
            }
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
