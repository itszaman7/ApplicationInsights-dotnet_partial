namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.Serialization.Json;
    using System.Threading;

    using Microsoft.ApplicationInsights.Extensibility.Filtering;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers;
    using Microsoft.ManagementServices.RealTimeDataProcessing.QuickPulseService;

    /// <summary>
    /// Service client for QPS service.
    /// </summary>
    internal sealed class QuickPulseServiceClient : IQuickPulseServiceClient
    {
        private readonly string instanceName;

        private readonly string roleName;

        private readonly string streamId;

        private readonly string machineName;

        private readonly string version;

        private readonly TimeSpan timeout = TimeSpan.FromSeconds(3);

        private readonly Clock timeProvider;

        private readonly bool isWebApp;

        private readonly int processorCount;

        private readonly DataContractJsonSerializer serializerDataPoint = new DataContractJsonSerializer(typeof(MonitoringDataPoint));

        private readonly DataContractJsonSerializer serializerDataPointArray = new DataContractJsonSerializer(typeof(MonitoringDataPoint[]));

        private readonly DataContractJsonSerializer deserializerServerResponse = new DataContractJsonSerializer(typeof(CollectionConfigurationInfo));

        private readonly Dictionary<string, string> authOpaqueHeaderValues = new Dictionary<string, string>(StringComparer.Ordinal);

        private Uri currentServiceUri;

        public QuickPulseServiceClient(
            Uri serviceUri,
            string instanceName,
            string roleName,
            string streamId,
            string machineName,
            string version,
            Clock timeProvider,
            bool isWebApp,
            int processorCount,
            TimeSpan? timeout = null)
        {
            this.CurrentServiceUri = serviceUri;
            this.instanceName = instanceName;
            this.roleName = roleName;
            this.streamId = streamId;
            this.machineName = machineName;
            this.version = version;
                this.authOpaqueHeaderValues.Add(headerName, null);
            }
        }

        public Uri CurrentServiceUri
        {
            get => Volatile.Read(ref this.currentServiceUri);
            private set => Volatile.Write(ref this.currentServiceUri, value);
        }

            string authApiKey,
            string authToken,
            out CollectionConfigurationInfo configurationInfo,
            CollectionConfigurationError[] collectionConfigurationErrors)
        {
            var requestUri = string.Format(
                CultureInfo.InvariantCulture,
                "{0}/post?ikey={1}",
                this.CurrentServiceUri.AbsoluteUri.TrimEnd('/'),
                Uri.EscapeUriString(instrumentationKey));
                HttpWebRequest request = WebRequest.Create(new Uri(requestUri)) as HttpWebRequest;
                request.Method = "POST";
                request.Timeout = (int)this.timeout.TotalMilliseconds;

                this.AddHeaders(request, includeIdentityHeaders, configurationETag, authApiKey, authToken);

                using (Stream requestStream = request.GetRequestStream())
                {
                    onWriteRequestBody(requestStream);

        {
            configurationInfo = null;
            servicePollingIntervalHint = null;

            bool isSubscribed;
            if (!bool.TryParse(response.GetResponseHeader(QuickPulseConstants.XMsQpsSubscribedHeaderName), out isSubscribed))
            {
                // could not parse the isSubscribed value

                // read the response out to avoid issues with TCP connections not being freed up
                try
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        using (var sr = new StreamReader(responseStream))
                        {
                            sr.ReadToEnd();
                        }
                    }
                }
                return null;
            }

            foreach (string headerName in QuickPulseConstants.XMsQpsAuthOpaqueHeaderNames)
            {
                this.authOpaqueHeaderValues[headerName] = response.GetResponseHeader(headerName);
            }

            string configurationETagHeaderValue = response.GetResponseHeader(QuickPulseConstants.XMsQpsConfigurationETagHeaderName);

            {
                servicePollingIntervalHint = TimeSpan.FromMilliseconds(servicePollingIntervalHintInMs);
            }

            if (Uri.TryCreate(response.GetResponseHeader(QuickPulseConstants.XMsQpsServiceEndpointRedirectHeaderName), UriKind.Absolute, out Uri serviceEndpointRedirect))
            {
                this.CurrentServiceUri = serviceEndpointRedirect;
            }

            try
                }
            }
            catch (Exception e)
            {
                // couldn't read or deserialize the response
                QuickPulseEventSource.Log.ServiceCommunicationFailedEvent(e.ToInvariantString());
            }

            return isSubscribed;
        }
        {
            return Math.Round(value, 4, MidpointRounding.AwayFromZero);
        }

        private void WritePingData(DateTimeOffset timestamp, Stream stream)
        {
            var dataPoint = new MonitoringDataPoint
            {
                Version = this.version,
                InvariantVersion = MonitoringDataPoint.CurrentInvariantVersion,
                MachineName = this.machineName,
                Timestamp = timestamp.UtcDateTime,
                IsWebApp = this.isWebApp,
                PerformanceCollectionSupported = true,
                ProcessorCount = this.processorCount,
            };

            this.serializerDataPoint.WriteObject(stream, dataPoint);
        }

        {
            var monitoringPoints = new List<MonitoringDataPoint>();

            foreach (var sample in samples)
            {
                var metricPoints = new List<MetricPoint>();

                metricPoints.AddRange(CreateDefaultMetrics(sample));

                metricPoints.AddRange(
                    sample.TopCpuData.Select(p => new ProcessCpuData() { ProcessName = p.Item1, CpuPercentage = p.Item2, }).ToArray();

                var dataPoint = new MonitoringDataPoint
                {
                    Version = this.version,
                    InvariantVersion = MonitoringDataPoint.CurrentInvariantVersion,
                    InstrumentationKey = instrumentationKey,
                    Instance = this.instanceName,
                    RoleName = this.roleName,
                    StreamId = this.streamId,
                    GlobalDocumentQuotaReached = sample.GlobalDocumentQuotaReached,
                };

                monitoringPoints.Add(dataPoint);
            }

            this.serializerDataPointArray.WriteObject(stream, monitoringPoints.ToArray());
        }

        private static IEnumerable<MetricPoint> CreateCalculatedMetrics(QuickPulseDataSample sample)
        {
                        Value = metricAccumulatedValues.CalculateAggregation(out long count),
                        Weight = (int)count,
                    };

                    metrics.Add(metricPoint);
                }
                catch (Exception e)
                {
                    // skip this metric
                    QuickPulseEventSource.Log.UnknownErrorEvent(e.ToString());
            if (includeIdentityHeaders)
            {
                request.Headers.Add(QuickPulseConstants.XMsQpsInstanceNameHeaderName, this.instanceName);
                request.Headers.Add(QuickPulseConstants.XMsQpsStreamIdHeaderName, this.streamId);
                request.Headers.Add(QuickPulseConstants.XMsQpsMachineNameHeaderName, this.machineName);
                request.Headers.Add(QuickPulseConstants.XMsQpsRoleNameHeaderName, this.roleName);
                request.Headers.Add(QuickPulseConstants.XMsQpsInvariantVersionHeaderName,
                    MonitoringDataPoint.CurrentInvariantVersion.ToString(CultureInfo.InvariantCulture));
            }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
