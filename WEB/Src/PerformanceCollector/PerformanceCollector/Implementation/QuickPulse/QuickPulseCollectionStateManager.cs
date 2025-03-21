namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Extensibility.Filtering;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Authentication;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers;

    internal class QuickPulseCollectionStateManager
    {
        private readonly IQuickPulseServiceClient serviceClient;

        private readonly Clock timeProvider;

        private readonly QuickPulseTimings timings;

        private readonly Action onStartCollection;

        private readonly Action onStopCollection;

        private readonly Func<IList<QuickPulseDataSample>> onSubmitSamples;

        private readonly Action<IList<QuickPulseDataSample>> onReturnFailedSamples;

        private readonly Func<CollectionConfigurationInfo, CollectionConfigurationError[]> onUpdatedConfiguration;

        private readonly Action<Uri> onUpdatedServiceEndpoint;

        private readonly TimeSpan coolDownTimeout;

        private readonly List<CollectionConfigurationError> collectionConfigurationErrors = new List<CollectionConfigurationError>();

        private readonly TelemetryConfiguration telemetryConfiguration;

        private DateTimeOffset lastSuccessfulPing;

        private DateTimeOffset lastSuccessfulSubmit;

        private bool isCollectingData;

        private bool firstStateUpdate = true;

        private string currentConfigurationETag = string.Empty;

        private TimeSpan? latestServicePollingIntervalHint = null;

        public QuickPulseCollectionStateManager(
            TelemetryConfiguration telemetryConfiguration,
            IQuickPulseServiceClient serviceClient,
            Clock timeProvider,
            QuickPulseTimings timings,
            Action onStartCollection,
            Action onStopCollection,
            Func<IList<QuickPulseDataSample>> onSubmitSamples,
            Action<IList<QuickPulseDataSample>> onReturnFailedSamples,
            Func<CollectionConfigurationInfo, CollectionConfigurationError[]> onUpdatedConfiguration,
            Action<Uri> onUpdatedServiceEndpoint)
        {
        {
            get
            {
                return this.isCollectingData;
            }

            private set
            {
                if (value != this.isCollectingData)
                {

            if (this.firstStateUpdate)
            {
                this.ResetLastSuccessful();

                this.firstStateUpdate = false;
            }

            AuthToken authToken = default;
                authToken = this.telemetryConfiguration.CredentialEnvelope.GetToken();
                if (authToken == default)
                {
                    // If a credential has been set on the configuration and we fail to get a token, do net send.
                    QuickPulseEventSource.Log.FailedToGetAuthToken();
                    return this.DetermineBackOffs();
                }
            }

            CollectionConfigurationInfo configurationInfo;
            {
                // we are currently collecting
                IList<QuickPulseDataSample> dataSamplesToSubmit = this.onSubmitSamples();

                if (!dataSamplesToSubmit.Any())
                {
                    // no samples to submit, do nothing
                    return this.DetermineBackOffs();
                }
                else
                    }
                }

                bool? keepCollecting = this.serviceClient.SubmitSamples(
                    dataSamplesToSubmit,
                    instrumentationKey,
                    this.currentConfigurationETag,
                    authApiKey,
                    authToken.Token,
                    out configurationInfo,
                        this.onReturnFailedSamples(dataSamplesToSubmit);
                        break;

                    case true:
                        // the service wants us to keep collecting
                        this.UpdateConfiguration(configurationInfo);
                        break;

                    case false:
                        // the service wants us to stop collection
                    this.currentConfigurationETag,
                    authApiKey,
                    authToken.Token,
                    out configurationInfo,
                    out TimeSpan? servicePollingIntervalHint);

                this.latestServicePollingIntervalHint = servicePollingIntervalHint ?? this.latestServicePollingIntervalHint;

                QuickPulseEventSource.Log.PingSentEvent(this.currentConfigurationETag, configurationInfo?.ETag, startCollection.ToString());


                    case true:
                        // the service wants us to start collection now
                        this.UpdateConfiguration(configurationInfo);
                        this.onStartCollection();
                        break;

                    case false:
                        // the service wants us to remain idle and keep pinging
                        break;
                        CollectionConfigurationError.CreateError(
                            CollectionConfigurationErrorType.CollectionConfigurationFailureToCreateUnexpected,
                            string.Format(CultureInfo.InvariantCulture, "Unexpected error applying configuration. ETag: {0}", configurationInfo.ETag ?? string.Empty),
                            e,
                            Tuple.Create("ETag", configurationInfo.ETag)));
                }

                if (errors != null)
                {
                    this.collectionConfigurationErrors.AddRange(errors);
            if (this.IsCollectingData)
            {
                TimeSpan timeSinceLastSuccessfulSubmit = this.timeProvider.UtcNow - this.lastSuccessfulSubmit;
                if (timeSinceLastSuccessfulSubmit < this.timings.TimeToCollectionBackOff)
                {
                    return this.timings.CollectionInterval;
                }

                QuickPulseEventSource.Log.TroubleshootingMessageEvent("Collection is failing. Back off.");



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
