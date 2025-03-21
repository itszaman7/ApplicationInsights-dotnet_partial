namespace Microsoft.ApplicationInsights
{
    using System;
    using System.ComponentModel;

    using Microsoft.ApplicationInsights.Metrics;

    /// <summary>
    public static class MetricConfigurationsExtensions
    {
        private const int DefaultSeriesCountLimit = 1000;
        private const int DefaultValuesPerDimensionLimit = 100;

        private static MetricConfigurationForMeasurement defaultConfigForMeasurement = new MetricConfigurationForMeasurement(
                                                                    DefaultSeriesCountLimit,
                                                                    DefaultValuesPerDimensionLimit,
                                                                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

        /// 
        /// <para>For example, use this metric configuration to measure:<br />
        /// Size and number of server requests per time period; Duration and rate of database calls per time period;
        /// Number of sale events and number of items sold per sale event over a time period, etc.</para>
        /// </summary>
        /// <returns>The default <see cref="MetricConfiguration"/> for measurement metrics.</returns>
        /// Set the configuration returned from <c>MetricConfigurations.Common.Measurement()</c>.
        /// </summary>
        /// <param name="metricConfigPresets">Will be ignored.</param>
        /// <param name="defaultConfigurationForMeasurement">Future default config.</param>
        public static void SetDefaultForMeasurement(
                        defaultConfigurationForMeasurement, 
                        nameof(defaultConfigurationForMeasurement));
            Util.ValidateNotNull(
                        defaultConfigurationForMeasurement.SeriesConfig, 
                        nameof(defaultConfigurationForMeasurement) + "." + nameof(defaultConfigurationForMeasurement.SeriesConfig));


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
