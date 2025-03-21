namespace Microsoft.ApplicationInsights.Metrics
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;

    using static System.FormattableString;

    /// <summary>A metric identifier encapsulates all information required to uniquely identify a metric.
    /// A metric is identified by its name/id, its namespace and the names of its dimensions.</summary>
    public sealed class MetricIdentifier : IEquatable<MetricIdentifier>
    {
        /// <summary>@Max number of dimensions supported.</summary>
        public const int MaxDimensionsCount = 10;

        private const string NoNamespaceIdentifierStringComponent = "<NoNamespace>";

        private static readonly char[] InvalidMetricChars = new char[]
            {
                        '\0', '"', '\'', '(', ')', '[', ']', '{', '}', '<', '>', '=', ',',
                        '`',  '~', '!',  '@', '#', '$', '%', '^', '&', '*', '+', '?',
            };

        private static string defaultMetricNamespace = String.Empty;

        /// <summary>
        /// Gets or sets the namespace used for metrics of no namespace was explicitly speified.
        /// </summary>
        public static string DefaultMetricNamespace
        {
            get
            {
                return defaultMetricNamespace;
            }

            set
            {
                ValidateLiteral(value, nameof(value), allowEmpty: true);
                defaultMetricNamespace = value.Trim();
            }
        }

        /// <summary>Validates if a metric id / name / namespace is valid and if not, throws an <c>ArgumentException</c>. </summary>
        /// @PublicExposureCandidate
        private static void ValidateLiteral(string partValue, string partName, bool allowEmpty)
        {
            if (partValue == null)
            {
                throw new ArgumentNullException(partName);
            }

            if (allowEmpty)
            {
                if (partValue.Length > 0 && String.IsNullOrWhiteSpace(partValue))
                {
                    throw new ArgumentException(Invariant($"{partName} may not be non-empty, but whitespace-only."));
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(partValue))
                {
                    throw new ArgumentException(Invariant($"{partName} may not be empty or whitespace-only."));
                }
            }

            int pos = partName.IndexOfAny(InvalidMetricChars);
            if (pos >= 0)
            {
                throw new ArgumentException(Invariant($"{partName} (\"{partValue}\") contains a disallowed character at position {pos}."));
            }
        }

        // These objects may be created frequently.
        // We want to avoid the allocation of an array every time we use an ID, so we unwind all loops and list all 10 names explicitly.
        // There is no plan to support more dimension names any time soon.
#pragma warning disable SA1201 // Elements must appear in the correct order: We want these fields after the above statics.
        private readonly string dimension1Name;
        private readonly string dimension2Name;
        private readonly string dimension3Name;
        private readonly string dimension4Name;
        private readonly string dimension5Name;
        private readonly string dimension6Name;
        private readonly string dimension7Name;
        private readonly string dimension8Name;
        private readonly string dimension9Name;
        private readonly string dimension10Name;

        private readonly string identifierString;
        private readonly int hashCode;
#pragma warning restore SA1201 // Elements must appear in the correct order

        /// <summary>Initializes a new metric identifier.</summary>
        public MetricIdentifier(string metricId)
            : this(metricNamespace: null,
                   metricId: metricId,
                   dimension1Name: null,
                   dimension2Name: null,
                   dimension3Name: null,
                   dimension4Name: null,
                   dimension5Name: null,
                   dimension6Name: null,
                   dimension7Name: null,
                   dimension8Name: null,
                   dimension9Name: null,
                   dimension10Name: null)
        {
        }

        /// <summary>Initializes a new metric identifier.</summary>
        public MetricIdentifier(string metricNamespace, string metricId)
            : this(metricNamespace,
                   metricId,
                   dimension1Name: null,
                   dimension2Name: null,
                   dimension3Name: null,
                   dimension4Name: null,
                   dimension5Name: null,
                   dimension6Name: null,
                   dimension7Name: null,
                   dimension8Name: null,
                   dimension9Name: null,
                   dimension10Name: null)
        {
        }
                   metricId,
                   dimension1Name,
                   dimension2Name,
                   dimension3Name: null,
                   dimension4Name: null,
                   dimension5Name: null,
                   dimension6Name: null,
                   dimension7Name: null,
                   dimension8Name: null,
                   dimension9Name: null,
                   dimension10Name: null)
        {
        }

        /// <summary>Initializes a new metric identifier.</summary>
        public MetricIdentifier(
                            string metricNamespace,
                            string metricId,
                            string dimension1Name,
                            string dimension2Name,
                   dimension7Name: null,
                   dimension8Name: null,
                   dimension9Name: null,
                   dimension10Name: null)
        {
        }

        /// <summary>Initializes a new metric identifier.</summary>
        public MetricIdentifier(
                            string metricNamespace,
                   dimension8Name: null,
                   dimension9Name: null,
                   dimension10Name: null)
        {
        }

        /// <summary>Initializes a new metric identifier.</summary>
        public MetricIdentifier(
                            string metricNamespace,
                            string metricId,
                   metricId,
                   dimension1Name,
                   dimension2Name,
                   dimension3Name,
                   dimension4Name,
                   dimension5Name,
                   dimension6Name: null,
                   dimension7Name: null,
                   dimension8Name: null,
                   dimension9Name: null,
                   dimension10Name: null)
        {
        }

        /// <summary>Initializes a new metric identifier.</summary>
        public MetricIdentifier(
                            string metricNamespace,
                            string metricId,
                            string dimension1Name,
                            string dimension2Name,
                   metricId,
                   dimension1Name,
                   dimension2Name,
                   dimension3Name,
                   dimension4Name,
                   dimension5Name,
                   dimension6Name,
                   dimension7Name: null,
                   dimension8Name: null,
                   dimension9Name: null,
                   dimension10Name: null)
        {
        }

        /// <summary>Initializes a new metric identifier.</summary>
        public MetricIdentifier(
                            string metricNamespace,
                            string metricId,
                            string dimension1Name,
                            string dimension2Name,
                            string dimension6Name,
                            string dimension7Name)
            : this(metricNamespace,
                   metricId,
                   dimension1Name,
                   dimension2Name,
                   dimension3Name,
                   dimension4Name,
                   dimension5Name,
                   dimension6Name,
                   dimension7Name,
                   dimension8Name: null,
                   dimension9Name: null,
                   dimension10Name: null)
        {
        }

        /// <summary>Initializes a new metric identifier.</summary>
        public MetricIdentifier(
                            string metricNamespace,
        }

        /// <summary>Initializes a new metric identifier.</summary>
        public MetricIdentifier(
                            string metricNamespace,
                            string metricId,
                            string dimension1Name,
                            string dimension2Name,
                            string dimension3Name,
                            string dimension4Name,
                            string dimension5Name,
                            string dimension6Name,
                            string dimension7Name,
                            string dimension8Name,
                            string dimension9Name)
            : this(metricNamespace,
                   metricId,
                   dimension1Name,
                   dimension2Name,
                   dimension3Name,
        {
        }

        /// <summary>Initializes a new metric identifier.</summary>
        public MetricIdentifier(
                        string metricNamespace,
                        string metricId,
                        string dimension1Name,
                        string dimension2Name,
                        string dimension3Name,
                                ref dimension7Name,
                                ref dimension8Name,
                                ref dimension9Name,
                                ref dimension10Name);

            this.MetricNamespace = metricNamespace;
            this.MetricId = metricId;
            this.DimensionsCount = dimCount;

            this.dimension1Name = dimension1Name;

        /// <summary>
        /// Gets the dimensionality of this metric.
        /// </summary>
        public int DimensionsCount { get; }

        /// <summary>
        /// Get an enumeration of the dimension names contained in this identity. The enumeration will have <c>DimensionsCount</c> elements.
        /// </summary>
        /// <returns>An enumeration of the dimension names contained in this identity.</returns>
        public IEnumerable<string> GetDimensionNames()
        {
            for (int d = 1; d <= this.DimensionsCount; d++)
            {
                yield return this.GetDimensionName(d);
            }
        }

        /// <summary>
        /// Gets the name of a dimension identified by the specified 1-based dimension index.
        /// For zero-dimensional metrics, this method will always fail.
        /// </summary>
        /// <param name="dimensionNumber">1-based dimension number. Allowed values are <c>1</c> through <c>10</c>.</param>
        /// <returns>The name of the specified dimension.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233", Justification = "dimensionNumber is validated.")]
        public string GetDimensionName(int dimensionNumber)
        {
            this.ValidateDimensionNumberForGetter(dimensionNumber);

            switch (dimensionNumber)
                case 1: return this.dimension1Name;
                case 2: return this.dimension2Name;
                case 3: return this.dimension3Name;
                case 4: return this.dimension4Name;
                case 5: return this.dimension5Name;
                case 6: return this.dimension6Name;
                case 7: return this.dimension7Name;
                case 8: return this.dimension8Name;
                case 9: return this.dimension9Name;
                case 10: return this.dimension10Name;
        }

        /// <summary>
        /// Gets the hash code for this <c>MetricIdentifier</c> instance.
        /// </summary>
        /// <returns>Hash code for this <c>MetricIdentifier</c> instance.</returns>
        public override int GetHashCode()
        {
            return this.hashCode;
        }
        /// Determines whether the specified object is a <c>MetricIdentifier</c> that is equal to this <c>MetricIdentifier</c> based on the
        /// respective metric namespaces, metric IDs and the number and the names of dimensions.
        /// </summary>
        /// <param name="otherObj">Another object.</param>
        /// <returns>Whether the specified other object is equal to this object based on the respective namespaces, IDs and dimension names.</returns>
        public override bool Equals(object otherObj)
        {
            if (otherObj is MetricIdentifier otherMetricIdentifier)
            {
                return this.Equals(otherMetricIdentifier);
                return base.Equals(otherObj);
            }
        }

        /// <summary>
        /// Determines whether the specified object is a <c>MetricIdentifier</c> that is equal to this <c>MetricIdentifier</c> based on the
        /// respective metric namespaces, metric IDs and the number and the names of dimensions.
        /// </summary>
        /// <param name="otherMetricIdentifier">Another object.</param>
        /// <returns>Whether the specified other object is equal to this object based on the respective namespaces, IDs and dimension names.</returns>
            if (dimensionNumber < 1)
            {
                throw new ArgumentOutOfRangeException(
                                nameof(dimensionNumber),
                                Invariant($"{dimensionNumber} is an invalid {nameof(dimensionNumber)}. Note that {nameof(dimensionNumber)} is a 1-based index."));
            }

            if (dimensionNumber > MetricIdentifier.MaxDimensionsCount)
            {
                throw new ArgumentOutOfRangeException(
                                                ref string dimension10Name)
        {
            dimensionCount = 0;
            EnsureDimensionNameValid(ref dimensionCount, ref dimension10Name, 10);
            EnsureDimensionNameValid(ref dimensionCount, ref dimension9Name, 9);
            EnsureDimensionNameValid(ref dimensionCount, ref dimension8Name, 8);
            EnsureDimensionNameValid(ref dimensionCount, ref dimension7Name, 7);
            EnsureDimensionNameValid(ref dimensionCount, ref dimension6Name, 6);
            EnsureDimensionNameValid(ref dimensionCount, ref dimension5Name, 5);
            EnsureDimensionNameValid(ref dimensionCount, ref dimension4Name, 4);

            if (this.MetricNamespace.Length > 0)
            {
                idStr.Append(this.MetricNamespace);
            }
            else
            {
                idStr.Append(MetricIdentifier.NoNamespaceIdentifierStringComponent);
            }
            


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
