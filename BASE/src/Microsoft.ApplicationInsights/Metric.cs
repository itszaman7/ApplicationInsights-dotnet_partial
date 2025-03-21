namespace Microsoft.ApplicationInsights
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.ApplicationInsights.Metrics;
    using Microsoft.ApplicationInsights.Metrics.ConcurrentDatastructures;

    using static System.FormattableString;

    /// <summary>
    /// Represents a zero- or multi-dimensional metric.<br />
    /// Contains convenience methods to track, aggregate and send values.<br />
    /// A <c>Metric</c> instance groups one or more <c>MetricSeries</c> that actually track and aggregate values along with
    /// naming and configuration attributes that identify the metric and define how it will be aggregated. 
    /// </summary>
    public sealed class Metric
    {
#pragma warning disable SA1401, SA1304, SA1307 // intended to be an internal, lower-case field 
        internal readonly MetricConfiguration configuration;
#pragma warning restore SA1307, SA1304, SA1401

        // private const string NullMetricObjectId = "null";

        private readonly MetricSeries zeroDimSeries;
        private readonly IReadOnlyList<KeyValuePair<string[], MetricSeries>> zeroDimSeriesList;

        private readonly MultidimensionalCube2<MetricSeries> metricSeries;

        private readonly MetricManager metricManager;

        [SuppressMessage("Microsoft.Performance", "CA1825: Avoid unnecessary zero-length array allocations", Justification = "Array.Empty<string>() not supported in Net4.5")]
        internal Metric(MetricManager metricManager, MetricIdentifier metricIdentifier, MetricConfiguration configuration)
        {
            Util.ValidateNotNull(metricManager, nameof(metricManager));
            Util.ValidateNotNull(metricIdentifier, nameof(metricIdentifier));
            EnsureConfigurationValid(metricIdentifier.DimensionsCount, configuration);

            this.metricManager = metricManager;
            this.Identifier = metricIdentifier;
            this.configuration = configuration;

            this.zeroDimSeries = this.metricManager.CreateNewSeries(
                                                        new MetricIdentifier(this.Identifier.MetricNamespace, this.Identifier.MetricId),
                                                        dimensionNamesAndValues: null,
                                                        config: this.configuration.SeriesConfig);

            if (metricIdentifier.DimensionsCount == 0)
            {
                this.metricSeries = null;
                this.zeroDimSeriesList = new KeyValuePair<string[], MetricSeries>[1] { new KeyValuePair<string[], MetricSeries>(new string[0], this.zeroDimSeries) };
            }
            else
            {
                int[] dimensionValuesCountLimits = new int[metricIdentifier.DimensionsCount];
                for (int d = 0; d < metricIdentifier.DimensionsCount; d++)
                {
                    dimensionValuesCountLimits[d] = configuration.GetValuesPerDimensionLimit(d + 1);
                }

                this.metricSeries = new MultidimensionalCube2<MetricSeries>(
                            totalPointsCountLimit: configuration.SeriesCountLimit - 1,
                            pointsFactory: this.CreateNewMetricSeries,
                            applyDimensionCapping: configuration.ApplyDimensionCapping,
                            dimensionCapValue: configuration.DimensionCappedString,
                            dimensionValuesCountLimits: dimensionValuesCountLimits);

                this.zeroDimSeriesList = null;
            }
        }

        /// <summary>
        /// Gets the identifier of a metric groups together the MetricNamespace, the MetricId, and the dimensions of the metric, if any.
        /// </summary>
        public MetricIdentifier Identifier { get; }

        /// <summary>
        /// Gets the current number of metric series contained in this metric. 
        /// Each metric contains a special zero-dimension series, plus one series per unique dimension-values combination.
        /// </summary>
        public int SeriesCount
        {
            get { return 1 + (this.metricSeries?.TotalPointsCount ?? 0); }
        }

        /// <summary>
        /// Gets the values known for dimension identified by the specified 1-based dimension index.
        /// </summary>
        /// <param name="dimensionNumber">1-based dimension number. Currently it can be <c>1</c> ... <c>10</c>.</param>
        /// <returns>The values known for the specified dimension.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233", Justification = "dimensionNumber is validated.")]
        public IReadOnlyCollection<string> GetDimensionValues(int dimensionNumber)
        {
            this.Identifier.ValidateDimensionNumberForGetter(dimensionNumber);

            int dimensionIndex = dimensionNumber - 1;
            return this.metricSeries.GetDimensionValues(dimensionIndex);
        }

        /// <summary>
        /// Gets all metric series contained in this metric.
        /// Each metric contains a special zero-dimension series, plus one series per unique dimension-values combination.
        /// </summary>
        /// <returns>All metric series contained in this metric.
        /// Each metric contains a special zero-dimension series, plus one series per unique dimension-values combination.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024: Use properties where appropriate", Justification = "Completes with non-trivial effort. Method is appropriate.")]
        [SuppressMessage("Microsoft.Performance", "CA1825: Avoid unnecessary zero-length array allocations", Justification = "Array.Empty<string>() not supported in Net4.5")]
        public IReadOnlyList<KeyValuePair<string[], MetricSeries>> GetAllSeries()
        {
            if (this.Identifier.DimensionsCount == 0)
            {
                return this.zeroDimSeriesList;
            }

            var series = new List<KeyValuePair<string[], MetricSeries>>(this.SeriesCount)
            {
                new KeyValuePair<string[], MetricSeries>(new string[0], this.zeroDimSeries),
            };
            this.metricSeries.GetAllPoints(series);
            return series;
        }

        /// <summary>
        /// Gets a <c>MetricSeries</c> associated with this metric.<br />
        /// This overload gets the zero-dimensional <c>MetricSeries</c> associated with this metric.
        /// Every metric, regardless of its dimensionality, has such a zero-dimensional <c>MetricSeries</c>.
        /// </summary>
        /// <param name="series">Will be set to the zero-dimensional <c>MetricSeries</c> associated with this metric.</param>
        /// <param name="dimension1Value">The value of the 1st dimension.</param>
        /// <returns><c>True</c> if the <c>MetricSeries</c> indicated by the specified dimension name could be retrieved (or created);
        /// <c>False</c> if the indicated series could not be retrieved or created because a dimension cap or a metric series cap was reached.</returns>
        /// <exception cref="ArgumentException">If the number of specified dimension names does not match the dimensionality of this <c>Metric</c>.</exception>
        public bool TryGetDataSeries(out MetricSeries series, string dimension1Value)
        {
            return this.TryGetDataSeries(out series, true, dimension1Value);
        }

        /// <summary>
        /// <param name="dimension2Value">The value of the 2nd dimension.</param>
        /// <param name="dimension3Value">The value of the 3rd dimension.</param>
        /// <returns><c>True</c> if the <c>MetricSeries</c> indicated by the specified dimension name could be retrieved (or created);
        /// <c>False</c> if the indicated series could not be retrieved or created because a dimension cap or a metric series cap was reached.</returns>
        /// <exception cref="ArgumentException">If the number of specified dimension names does not match the dimensionality of this <c>Metric</c>.</exception>
        public bool TryGetDataSeries(out MetricSeries series, string dimension1Value, string dimension2Value, string dimension3Value)
        {
            return this.TryGetDataSeries(out series, true, dimension1Value, dimension2Value, dimension3Value);
        }

        /// <summary>
        /// Gets or creates the <c>MetricSeries</c> associated with the specified dimension values.<br />
        /// This overload may only be used with 4-dimensional metrics. Use other overloads to specify a matching number of dimension values for this metric.
        /// </summary>
        /// <param name="series">If this method returns <c>True</c>: Will be set to the <c>MetricSeries</c> associated with the specified dimension value.<br />
        /// Otherwise: Will be set to <c>null</c>.</param>
        /// <param name="dimension1Value">The value of the 1st dimension.</param>
        /// <param name="dimension2Value">The value of the 2nd dimension.</param>
        /// <param name="dimension3Value">The value of the 3rd dimension.</param>
        /// <param name="dimension4Value">The value of the 4th dimension.</param>
        public bool TryGetDataSeries(out MetricSeries series, string dimension1Value, string dimension2Value, string dimension3Value, string dimension4Value)
        {
            return this.TryGetDataSeries(out series, true, dimension1Value, dimension2Value, dimension3Value, dimension4Value);
        }

        /// <summary>
        /// Gets or creates the <c>MetricSeries</c> associated with the specified dimension values.<br />
        /// This overload may only be used with 5-dimensional metrics. Use other overloads to specify a matching number of dimension values for this metric.
        /// </summary>
        /// <param name="series">If this method returns <c>True</c>: Will be set to the <c>MetricSeries</c> associated with the specified dimension value.<br />
        /// <c>False</c> if the indicated series could not be retrieved or created because a dimension cap or a metric series cap was reached.</returns>
        /// <exception cref="ArgumentException">If the number of specified dimension names does not match the dimensionality of this <c>Metric</c>.</exception>
        public bool TryGetDataSeries(
                                out MetricSeries series,
                                string dimension1Value,
                                string dimension2Value,
                                string dimension3Value,
                                string dimension4Value,
                                string dimension5Value)
        {
            return this.TryGetDataSeries(out series, true, dimension1Value, dimension2Value, dimension3Value, dimension4Value, dimension5Value);
        }

        /// <summary>
        /// Gets or creates the <c>MetricSeries</c> associated with the specified dimension values.<br />
        /// This overload may only be used with 6-dimensional metrics. Use other overloads to specify a matching number of dimension values for this metric.
        /// </summary>
        /// <param name="series">If this method returns <c>True</c>: Will be set to the <c>MetricSeries</c> associated with the specified dimension value.<br />
        /// Otherwise: Will be set to <c>null</c>.</param>
        /// <param name="dimension1Value">The value of the 1st dimension.</param>
        /// <param name="dimension2Value">The value of the 2nd dimension.</param>
        /// <param name="dimension3Value">The value of the 3rd dimension.</param>
        /// <param name="dimension4Value">The value of the 4th dimension.</param>
        /// <param name="dimension5Value">The value of the 5th dimension.</param>
        /// <param name="dimension6Value">The value of the 6th dimension.</param>
        /// <param name="dimension7Value">The value of the 7th dimension.</param>
        /// <returns><c>True</c> if the <c>MetricSeries</c> indicated by the specified dimension name could be retrieved (or created);
        /// <c>False</c> if the indicated series could not be retrieved or created because a dimension cap or a metric series cap was reached.</returns>
        /// <exception cref="ArgumentException">If the number of specified dimension names does not match the dimensionality of this <c>Metric</c>.</exception>
        public bool TryGetDataSeries(
        /// This overload may only be used with 8-dimensional metrics. Use other overloads to specify a matching number of dimension values for this metric.
        /// </summary>
        /// <param name="series">If this method returns <c>True</c>: Will be set to the <c>MetricSeries</c> associated with the specified dimension value.<br />
        /// Otherwise: Will be set to <c>null</c>.</param>
        /// <param name="dimension1Value">The value of the 1st dimension.</param>
        /// <param name="dimension2Value">The value of the 2nd dimension.</param>
        /// <param name="dimension3Value">The value of the 3rd dimension.</param>
        /// <param name="dimension4Value">The value of the 4th dimension.</param>
        /// <param name="dimension5Value">The value of the 5th dimension.</param>
        /// <param name="dimension6Value">The value of the 6th dimension.</param>
        /// <param name="dimension5Value">The value of the 5th dimension.</param>
        /// <param name="dimension6Value">The value of the 6th dimension.</param>
        /// <param name="dimension7Value">The value of the 7th dimension.</param>
        /// <param name="dimension8Value">The value of the 8th dimension.</param>
        /// <param name="dimension9Value">The value of the 9th dimension.</param>
        /// <returns><c>True</c> if the <c>MetricSeries</c> indicated by the specified dimension name could be retrieved (or created);
        /// <c>False</c> if the indicated series could not be retrieved or created because a dimension cap or a metric series cap was reached.</returns>
        /// <exception cref="ArgumentException">If the number of specified dimension names does not match the dimensionality of this <c>Metric</c>.</exception>
        public bool TryGetDataSeries(
                                out MetricSeries series,
                                string dimension1Value,
                                string dimension2Value,
                                string dimension3Value,
                                string dimension4Value,
                                string dimension5Value,
                                string dimension6Value,
                                string dimension7Value,
                                string dimension8Value,
                                string dimension9Value)
        {
                            dimension2Value,
                            dimension3Value,
                            dimension4Value,
                            dimension5Value,
                            dimension6Value,
                            dimension7Value,
                            dimension8Value,
                            dimension9Value);
        }

                                out MetricSeries series,
                                string dimension1Value,
                                string dimension2Value,
                                string dimension3Value,
                                string dimension4Value,
                                string dimension5Value,
                                string dimension6Value,
                                string dimension7Value,
                                string dimension8Value,
                                string dimension9Value,
                            out series,
                            true,
                            dimension1Value,
                            dimension2Value,
                            dimension3Value,
                            dimension4Value,
                            dimension5Value,
                            dimension6Value,
                            dimension7Value,
                            dimension8Value,
        /// <c>False</c> if the indicated series could not be retrieved or created because <c>createIfNotExists</c> is <c>false</c>
        /// or because a dimension cap or a metric series cap was reached.</returns>
        /// <exception cref="ArgumentException">If the number of specified dimension names does not match the dimensionality of this <c>Metric</c>.</exception>
        public bool TryGetDataSeries(out MetricSeries series, bool createIfNotExists, params string[] dimensionValues)
        {
            if (dimensionValues == null || dimensionValues.Length == 0)
            {
                series = this.zeroDimSeries;
                return true;
            }
                {
                    throw new ArgumentException(Invariant($"{nameof(dimensionValues)}[{d}]") + " may not be empty.");
                }

                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(Invariant($"{nameof(dimensionValues)}[{d}]") + " may not be whitespace only.");
                }
            }

        /// <summary>
        /// Tracks the specified value.<br />
        /// An aggregate representing tracked values will be automatically sent to the cloud ingestion endpoint at the end of each aggregation period.<br />
        /// This method uses the zero-dimensional <c>MetricSeries</c> associated with this metric.
        /// Use <c>TrackValue(..)</c> to track values into <c>MetricSeries</c> associated with specific dimension-values in multi-dimensional metrics.
        /// </summary>
        /// <param name="metricValue">The value to be aggregated.</param>
        public void TrackValue(double metricValue)
        {
            this.zeroDimSeries.TrackValue(metricValue);
        /// Use <c>TrackValue(..)</c> to track values into <c>MetricSeries</c> associated with specific dimension-values in multi-dimensional metrics.
        /// </summary>
        /// <param name="metricValue">The value to be aggregated.</param>
        public void TrackValue(object metricValue)
        {
            this.zeroDimSeries.TrackValue(metricValue);
        }

        /// <summary>
        /// Tracks the specified value using the <c>MetricSeries</c> associated with the specified dimension value.<br />
        /// An aggregate representing tracked values will be automatically sent to the cloud ingestion endpoint at the end of each aggregation period.
        /// This overload may only be used with 1-dimensional metrics. Use other overloads to specify a matching number of dimension values for this metric.
        /// </summary>
        /// <param name="metricValue">The value to be aggregated.</param>
        /// <param name="dimension1Value">The value of the 1st dimension.</param>
        /// <returns><c>True</c> if the specified value was added to the <c>MetricSeries</c> indicated by the specified dimension name;
        /// <c>False</c> if the indicated series could not be created because a dimension cap or a metric series cap was reached.</returns>
        /// <exception cref="ArgumentException">If the number of specified dimension names does not match the dimensionality of this <c>Metric</c>.</exception>
        public bool TrackValue(double metricValue, string dimension1Value)
        {
            MetricSeries series;
            bool canTrack = this.TryGetDataSeries(out series, dimension1Value);
            if (canTrack)
            {
                series.TrackValue(metricValue);
            }

            return canTrack;
        }


            return canTrack;
        }

        /// <summary>
        /// Tracks the specified value using the <c>MetricSeries</c> associated with the specified dimension values.<br />
        /// An aggregate representing tracked values will be automatically sent to the cloud ingestion endpoint at the end of each aggregation period.
        /// This overload may only be used with 2-dimensional metrics. Use other overloads to specify a matching number of dimension values for this metric.
        /// </summary>
        /// <param name="metricValue">The value to be aggregated.</param>
        /// <param name="dimension1Value">The value of the 1st dimension.</param>
        /// <param name="dimension2Value">The value of the 2nd dimension.</param>
        /// <returns><c>True</c> if the specified value was added to the <c>MetricSeries</c> indicated by the specified dimension name;
        /// <c>False</c> if the indicated series could not be created because a dimension cap or a metric series cap was reached.</returns>
        /// <exception cref="ArgumentException">If the number of specified dimension names does not match the dimensionality of this <c>Metric</c>.</exception>
        public bool TrackValue(object metricValue, string dimension1Value, string dimension2Value)
        {
            MetricSeries series;
            if (this.TryGetDataSeries(out series, dimension1Value, dimension2Value))
            {

        /// <summary>
        /// Tracks the specified value using the <c>MetricSeries</c> associated with the specified dimension values.<br />
        /// An aggregate representing tracked values will be automatically sent to the cloud ingestion endpoint at the end of each aggregation period.
        /// This overload may only be used with 3-dimensional metrics. Use other overloads to specify a matching number of dimension values for this metric.
        /// </summary>
        /// <param name="metricValue">The value to be aggregated.</param>
        /// <param name="dimension1Value">The value of the 1st dimension.</param>
        /// <param name="dimension2Value">The value of the 2nd dimension.</param>
        /// <c>False</c> if the indicated series could not be created because a dimension cap or a metric series cap was reached.</returns>
        /// <exception cref="ArgumentException">If the number of specified dimension names does not match the dimensionality of this <c>Metric</c>.</exception>
        public bool TrackValue(double metricValue, string dimension1Value, string dimension2Value, string dimension3Value)
        {
            MetricSeries series;
            bool canTrack = this.TryGetDataSeries(out series, dimension1Value, dimension2Value, dimension3Value);
            if (canTrack)
            {
                series.TrackValue(metricValue);
            }
        /// An aggregate representing tracked values will be automatically sent to the cloud ingestion endpoint at the end of each aggregation period.
        /// This overload may only be used with 3-dimensional metrics. Use other overloads to specify a matching number of dimension values for this metric.
        /// </summary>
        /// <param name="metricValue">The value to be aggregated.</param>
        /// <param name="dimension1Value">The value of the 1st dimension.</param>
        /// <param name="dimension2Value">The value of the 2nd dimension.</param>
        /// <param name="dimension3Value">The value of the 3rd dimension.</param>
        /// <returns><c>True</c> if the specified value was added to the <c>MetricSeries</c> indicated by the specified dimension name;
        /// <c>False</c> if the indicated series could not be created because a dimension cap or a metric series cap was reached.</returns>
        /// <exception cref="ArgumentException">If the number of specified dimension names does not match the dimensionality of this <c>Metric</c>.</exception>

        /// <summary>
        /// Tracks the specified value using the <c>MetricSeries</c> associated with the specified dimension values.<br />
        /// An aggregate representing tracked values will be automatically sent to the cloud ingestion endpoint at the end of each aggregation period.
        /// This overload may only be used with 4-dimensional metrics. Use other overloads to specify a matching number of dimension values for this metric.
        /// </summary>
        /// <param name="metricValue">The value to be aggregated.</param>
        /// <param name="dimension1Value">The value of the 1st dimension.</param>
        /// <param name="dimension2Value">The value of the 2nd dimension.</param>
        /// <param name="dimension3Value">The value of the 3rd dimension.</param>
        /// <c>False</c> if the indicated series could not be created because a dimension cap or a metric series cap was reached.</returns>
        /// <exception cref="ArgumentException">If the number of specified dimension names does not match the dimensionality of this <c>Metric</c>.</exception>
        public bool TrackValue(double metricValue, string dimension1Value, string dimension2Value, string dimension3Value, string dimension4Value)
        {
            MetricSeries series;
            bool canTrack = this.TryGetDataSeries(out series, dimension1Value, dimension2Value, dimension3Value, dimension4Value);
            if (canTrack)
            {
                series.TrackValue(metricValue);
            }
        /// This overload may only be used with 5-dimensional metrics. Use other overloads to specify a matching number of dimension values for this metric.
        /// </summary>
        /// <param name="metricValue">The value to be aggregated.</param>
        /// <param name="dimension1Value">The value of the 1st dimension.</param>
        /// <param name="dimension2Value">The value of the 2nd dimension.</param>
        /// <param name="dimension3Value">The value of the 3rd dimension.</param>
        /// <param name="dimension4Value">The value of the 4th dimension.</param>
        /// <param name="dimension5Value">The value of the 5th dimension.</param>
        /// <returns><c>True</c> if the specified value was added to the <c>MetricSeries</c> indicated by the specified dimension name;
        /// <c>False</c> if the indicated series could not be created because a dimension cap or a metric series cap was reached.</returns>

        /// <summary>
        /// Tracks the specified value using the <c>MetricSeries</c> associated with the specified dimension values.<br />
        /// An aggregate representing tracked values will be automatically sent to the cloud ingestion endpoint at the end of each aggregation period.
        /// This overload may only be used with 5-dimensional metrics. Use other overloads to specify a matching number of dimension values for this metric.
        /// </summary>
        /// <param name="metricValue">The value to be aggregated.</param>
        /// <param name="dimension1Value">The value of the 1st dimension.</param>
        /// <param name="dimension2Value">The value of the 2nd dimension.</param>
        /// <param name="dimension3Value">The value of the 3rd dimension.</param>
        /// <param name="dimension4Value">The value of the 4th dimension.</param>
        /// <param name="dimension5Value">The value of the 5th dimension.</param>
        /// <returns><c>True</c> if the specified value was added to the <c>MetricSeries</c> indicated by the specified dimension name;
        /// <c>False</c> if the indicated series could not be created because a dimension cap or a metric series cap was reached.</returns>
        /// <exception cref="ArgumentException">If the number of specified dimension names does not match the dimensionality of this <c>Metric</c>.</exception>
        public bool TrackValue(
                                object metricValue,
                                string dimension1Value,
                                string dimension2Value,
                                string dimension3Value,
        }

        /// <summary>
        /// Tracks the specified value using the <c>MetricSeries</c> associated with the specified dimension values.<br />
        /// An aggregate representing tracked values will be automatically sent to the cloud ingestion endpoint at the end of each aggregation period.
        /// This overload may only be used with 6-dimensional metrics. Use other overloads to specify a matching number of dimension values for this metric.
        /// </summary>
        /// <param name="metricValue">The value to be aggregated.</param>
        /// <param name="dimension1Value">The value of the 1st dimension.</param>
        /// <param name="dimension2Value">The value of the 2nd dimension.</param>
                                string dimension2Value,
                                string dimension3Value,
                                string dimension4Value,
                                string dimension5Value,
                                string dimension6Value)
        {
            MetricSeries series;
            bool canTrack = this.TryGetDataSeries(out series, dimension1Value, dimension2Value, dimension3Value, dimension4Value, dimension5Value, dimension6Value);
            if (canTrack)
            {
                series.TrackValue(metricValue);
            }

            return canTrack;
        }

        /// <summary>
        /// Tracks the specified value using the <c>MetricSeries</c> associated with the specified dimension values.<br />
        /// An aggregate representing tracked values will be automatically sent to the cloud ingestion endpoint at the end of each aggregation period.
        /// This overload may only be used with 6-dimensional metrics. Use other overloads to specify a matching number of dimension values for this metric.
        /// <param name="dimension6Value">The value of the 6th dimension.</param>
        /// <returns><c>True</c> if the specified value was added to the <c>MetricSeries</c> indicated by the specified dimension name;
        /// <c>False</c> if the indicated series could not be created because a dimension cap or a metric series cap was reached.</returns>
        /// <exception cref="ArgumentException">If the number of specified dimension names does not match the dimensionality of this <c>Metric</c>.</exception>
        public bool TrackValue(
                                object metricValue,
                                string dimension1Value,
                                string dimension2Value,
                                string dimension3Value,
                                string dimension4Value,
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tracks the specified value using the <c>MetricSeries</c> associated with the specified dimension values.<br />
        /// An aggregate representing tracked values will be automatically sent to the cloud ingestion endpoint at the end of each aggregation period.
        /// This overload may only be used with 7-dimensional metrics. Use other overloads to specify a matching number of dimension values for this metric.
                                string dimension3Value,
                                string dimension4Value,
                                string dimension5Value,
                                string dimension6Value,
                                string dimension7Value)
        {
            MetricSeries series;
            bool canTrack = this.TryGetDataSeries(
                                        out series,
                                        dimension1Value,
                                        dimension7Value);
            if (canTrack)
            {
                series.TrackValue(metricValue);
            }

            return canTrack;
        }

        /// <summary>
        /// </summary>
        /// <param name="metricValue">The value to be aggregated.</param>
        /// <param name="dimension1Value">The value of the 1st dimension.</param>
        /// <param name="dimension2Value">The value of the 2nd dimension.</param>
        /// <param name="dimension3Value">The value of the 3rd dimension.</param>
        /// <param name="dimension4Value">The value of the 4th dimension.</param>
        /// <param name="dimension5Value">The value of the 5th dimension.</param>
        /// <param name="dimension6Value">The value of the 6th dimension.</param>
        /// <param name="dimension7Value">The value of the 7th dimension.</param>
        /// <returns><c>True</c> if the specified value was added to the <c>MetricSeries</c> indicated by the specified dimension name;
                                string dimension7Value)
        {
            MetricSeries series;
            if (this.TryGetDataSeries(
                            out series,
                            dimension1Value,
                            dimension2Value,
                            dimension3Value,
                            dimension4Value,
                            dimension5Value,
        /// This overload may only be used with 8-dimensional metrics. Use other overloads to specify a matching number of dimension values for this metric.
        /// </summary>
        /// <param name="metricValue">The value to be aggregated.</param>
        /// <param name="dimension1Value">The value of the 1st dimension.</param>
        /// <param name="dimension2Value">The value of the 2nd dimension.</param>
        /// <param name="dimension3Value">The value of the 3rd dimension.</param>
        /// <param name="dimension4Value">The value of the 4th dimension.</param>
        /// <param name="dimension5Value">The value of the 5th dimension.</param>
        /// <param name="dimension6Value">The value of the 6th dimension.</param>
        /// <param name="dimension7Value">The value of the 7th dimension.</param>
                                string dimension2Value,
                                string dimension3Value,
                                string dimension4Value,
                                string dimension5Value,
                                string dimension6Value,
                                string dimension7Value,
                                string dimension8Value)
        {
            MetricSeries series;
            bool canTrack = this.TryGetDataSeries(

            return canTrack;
        }

        /// <summary>
        /// Tracks the specified value using the <c>MetricSeries</c> associated with the specified dimension values.<br />
        /// An aggregate representing tracked values will be automatically sent to the cloud ingestion endpoint at the end of each aggregation period.
        /// This overload may only be used with 8-dimensional metrics. Use other overloads to specify a matching number of dimension values for this metric.
        /// </summary>
        /// <param name="metricValue">The value to be aggregated.</param>
        /// <param name="dimension1Value">The value of the 1st dimension.</param>
        /// <param name="dimension2Value">The value of the 2nd dimension.</param>
        /// <param name="dimension3Value">The value of the 3rd dimension.</param>
        /// <param name="dimension4Value">The value of the 4th dimension.</param>
        /// <param name="dimension5Value">The value of the 5th dimension.</param>
        /// <param name="dimension6Value">The value of the 6th dimension.</param>
        /// <param name="dimension7Value">The value of the 7th dimension.</param>
        /// <param name="dimension8Value">The value of the 8th dimension.</param>
        /// <returns><c>True</c> if the specified value was added to the <c>MetricSeries</c> indicated by the specified dimension name;
        /// <c>False</c> if the indicated series could not be created because a dimension cap or a metric series cap was reached.</returns>
        /// <exception cref="ArgumentException">If the number of specified dimension names does not match the dimensionality of this <c>Metric</c>.</exception>
        public bool TrackValue(
                                object metricValue,
                                string dimension1Value,
                                string dimension2Value,
                                string dimension3Value,
                                string dimension4Value,
                                string dimension5Value,
                                string dimension6Value,
                                string dimension7Value,
                            dimension4Value,
                            dimension5Value,
                            dimension6Value,
                            dimension7Value,
                            dimension8Value))
            {
                series.TrackValue(metricValue);
                return true;
            }

        /// Tracks the specified value using the <c>MetricSeries</c> associated with the specified dimension values.<br />
        /// An aggregate representing tracked values will be automatically sent to the cloud ingestion endpoint at the end of each aggregation period.
        /// This overload may only be used with 9-dimensional metrics. Use other overloads to specify a matching number of dimension values for this metric.
        /// </summary>
        /// <param name="metricValue">The value to be aggregated.</param>
        /// <param name="dimension1Value">The value of the 1st dimension.</param>
        /// <param name="dimension2Value">The value of the 2nd dimension.</param>
        /// <param name="dimension3Value">The value of the 3rd dimension.</param>
        /// <param name="dimension4Value">The value of the 4th dimension.</param>
        /// <param name="dimension5Value">The value of the 5th dimension.</param>
                                string dimension2Value,
                                string dimension3Value,
                                string dimension4Value,
                                string dimension5Value,
                                string dimension6Value,
                                string dimension7Value,
                                string dimension8Value,
                                string dimension9Value)
        {
            MetricSeries series;
        /// </summary>
        /// <param name="metricValue">The value to be aggregated.</param>
        /// <param name="dimension1Value">The value of the 1st dimension.</param>
        /// <param name="dimension2Value">The value of the 2nd dimension.</param>
        /// <param name="dimension3Value">The value of the 3rd dimension.</param>
        /// <param name="dimension4Value">The value of the 4th dimension.</param>
        /// <param name="dimension5Value">The value of the 5th dimension.</param>
        /// <param name="dimension6Value">The value of the 6th dimension.</param>
        /// <param name="dimension7Value">The value of the 7th dimension.</param>
        /// <param name="dimension8Value">The value of the 8th dimension.</param>
        /// Tracks the specified value using the <c>MetricSeries</c> associated with the specified dimension values.<br />
        /// An aggregate representing tracked values will be automatically sent to the cloud ingestion endpoint at the end of each aggregation period.
        /// This overload may only be used with 10-dimensional metrics. Use other overloads to specify a matching number of dimension values for this metric.
        /// </summary>
        /// <param name="metricValue">The value to be aggregated.</param>
        /// <param name="dimension1Value">The value of the 1st dimension.</param>
        /// <param name="dimension2Value">The value of the 2nd dimension.</param>
        /// <param name="dimension3Value">The value of the 3rd dimension.</param>
        /// <param name="dimension4Value">The value of the 4th dimension.</param>
        /// <param name="dimension5Value">The value of the 5th dimension.</param>
        public bool TrackValue(
                                double metricValue,
                                string dimension1Value,
                                string dimension2Value,
                                string dimension3Value,
                                string dimension4Value,
                                string dimension5Value,
                                string dimension6Value,
                                string dimension7Value,
                                string dimension8Value,
                                string dimension9Value,
                                string dimension10Value)
        {
            MetricSeries series;
            bool canTrack = this.TryGetDataSeries(
                                        out series,
                                        dimension1Value,
                                        dimension2Value,
                                        dimension3Value,
                                        dimension4Value,
                                        dimension5Value,
                                        dimension6Value,
                                        dimension7Value,
                                        dimension8Value,
                                        dimension9Value,
                                        dimension10Value);
            if (canTrack)
            {
                series.TrackValue(metricValue);
            }

            return canTrack;
        }

        /// <summary>
        /// Tracks the specified value using the <c>MetricSeries</c> associated with the specified dimension values.<br />
        /// An aggregate representing tracked values will be automatically sent to the cloud ingestion endpoint at the end of each aggregation period.
        /// This overload may only be used with 10-dimensional metrics. Use other overloads to specify a matching number of dimension values for this metric.
        /// </summary>
        /// <param name="metricValue">The value to be aggregated.</param>
                                string dimension6Value,
                                string dimension7Value,
                                string dimension8Value,
                                string dimension9Value,
                                string dimension10Value)
        {
            MetricSeries series;
            if (this.TryGetDataSeries(
                            out series,
                            dimension1Value,
                            dimension5Value,
                            dimension6Value,
                            dimension7Value,
                            dimension8Value,
                            dimension9Value,
                            dimension10Value))
            {
                series.TrackValue(metricValue);
                return true;
            }
        }

        private static void EnsureConfigurationValid(
                                    int dimensionsCount,
                                    MetricConfiguration configuration)
        {
            Util.ValidateNotNull(configuration, nameof(configuration));
            Util.ValidateNotNull(configuration.SeriesConfig, nameof(configuration.SeriesConfig));

            for (int d = 0; d < dimensionsCount; d++)
            {
                if (configuration.GetValuesPerDimensionLimit(d + 1) < 1)
                {
                    throw new ArgumentException("Multidimensional metrics must allow at least one dimension-value per dimension"
                                    + Invariant($" (but {nameof(configuration.GetValuesPerDimensionLimit)}({d + 1})")
                                    + Invariant($" = {configuration.GetValuesPerDimensionLimit(d + 1)} was specified."));
                }
            }

            if (configuration.SeriesCountLimit < 1)

        private MetricSeries CreateNewMetricSeries(string[] dimensionValues)
        {
            KeyValuePair<string, string>[] dimensionNamesAndValues = null;

            if (dimensionValues != null)
            {
                dimensionNamesAndValues = new KeyValuePair<string, string>[dimensionValues.Length];

                for (int d = 0; d < dimensionValues.Length; d++)
                    if (dimensionValue == null)
                    {
                        throw new ArgumentNullException(Invariant($"{nameof(dimensionValues)}[{d}]"));
                    }

                    if (string.IsNullOrWhiteSpace(dimensionValue))
                    {
                        throw new ArgumentException(Invariant($"The value for dimension number {d} is empty or white-space."));
                    }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
