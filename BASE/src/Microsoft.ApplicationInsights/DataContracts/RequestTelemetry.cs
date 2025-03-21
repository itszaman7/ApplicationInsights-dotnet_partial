namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics;

    /// <summary>
    /// Encapsulates information about a web request handled by the application.
    /// </summary>
    /// <remarks>
    /// You can send information about requests processed by your web application to Application Insights by
    /// passing an instance of the <see cref="RequestTelemetry"/> class to the <see cref="TelemetryClient.TrackRequest(RequestTelemetry)"/>
    /// method.
    /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#trackrequest">Learn more</a>
    /// </remarks>
    public sealed class RequestTelemetry : OperationTelemetry, ITelemetry, ISupportProperties, ISupportMetrics, ISupportAdvancedSampling, IAiSerializableTelemetry
    {
        internal const string EtwEnvelopeName = "Request";
        internal string EnvelopeName = "AppRequests";

        private readonly TelemetryContext context;
        private RequestData dataPrivate;
        private bool successFieldSet;
        private IExtension extension;
        private double? samplingPercentage;
        private bool success = true;
        private IDictionary<string, double> measurementsValue;

        /// <summary>
            this.Duration = System.TimeSpan.Zero;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestTelemetry"/> class with the given <paramref name="name"/>,
        /// <paramref name="startTime"/>, <paramref name="duration"/>, <paramref name="responseCode"/> and <paramref name="success"/> property values.
        /// </summary>
        public RequestTelemetry(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success)
            : this()
        {
            this.Name = name; // Name is optional but without it UX does not make much sense
            this.Timestamp = startTime;
            this.Duration = duration;
            this.ResponseCode = responseCode;
            this.Success = success;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestTelemetry"/> class by cloning an existing instance.
        /// </summary>
                Utils.CopyDictionary(source.Metrics, this.Metrics);
            }

            this.Name = source.Name;
            this.context = source.context.DeepClone();
            this.ResponseCode = source.ResponseCode;
            this.Source = source.Source;
            this.Success = source.Success;
            this.Url = source.Url;
            this.Sequence = source.Sequence;
            this.Timestamp = source.Timestamp;
            this.successFieldSet = source.successFieldSet;
            this.extension = source.extension?.DeepClone();
            this.samplingPercentage = source.samplingPercentage;
            this.ProactiveSamplingDecision = source.ProactiveSamplingDecision;
        }

        /// <inheritdoc />
        string IAiSerializableTelemetry.TelemetryName
        {
                return this.EnvelopeName;
            }

            set
            {
                this.EnvelopeName = value;
            }
        }

        /// <inheritdoc />
        /// Gets or sets Request ID.
        /// </summary>
        public override string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets human-readable name of the requested page.
            set;
        }

        /// <summary>
        /// Gets or sets response code returned by the application after handling the request.
        /// </summary>
        public string ResponseCode
        {
            get;
            set;
        /// </summary>
        public override bool? Success
        {
            get
            {
                if (this.successFieldSet)
                {
                    return this.success;
                }

                return null;
            }

            set
                    this.success = value.Value;
                    this.successFieldSet = true;
                }
                else
                {
                    this.success = true;
                    this.successFieldSet = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets the amount of time it took the application to handle the request.
        /// </summary>
        public override TimeSpan Duration
        {
            get;
            set;
        }


        /// <summary>
        /// Gets a dictionary of application-defined request metrics.
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#properties">Learn more</a>
        /// </summary>
        public override IDictionary<string, double> Metrics
        {
            get { return LazyInitializer.EnsureInitialized(ref this.measurementsValue, () => new ConcurrentDictionary<string, double>()); }
        }

        /// <summary>
        /// Gets or sets data sampling percentage (between 0 and 100).
        /// </summary>
        double? ISupportSampling.SamplingPercentage
        {
            get { return this.samplingPercentage; }
            set { this.samplingPercentage = value; }
        }

        /// <summary>

        /// <inheritdoc/>
        public SamplingDecision ProactiveSamplingDecision { get; set; }

        /// <summary>
        /// Gets or sets the source for the request telemetry object. This often is a hashed instrumentation key identifying the caller.
        /// </summary>
        public string Source
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the MetricExtractorInfo.
        /// </summary>
        internal string MetricExtractorInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Deeply clones a <see cref="RequestTelemetry"/> object.
        /// </summary>
        /// <returns>A cloned instance.</returns>
        public override ITelemetry DeepClone()
        {
            return new RequestTelemetry(this);
        }
            }
            else
            {
                this.measurementsValue = new Dictionary<string, double>();
            }

            this.Url = this.Url.SanitizeUri();

            // Set for backward compatibility:
            this.Id = this.Id.SanitizeName();


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
