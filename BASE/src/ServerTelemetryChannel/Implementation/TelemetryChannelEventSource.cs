namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Tracing;

#if REDFIELD
    [EventSource(Name = "Redfield-Microsoft-ApplicationInsights-WindowsServer-TelemetryChannel")]
#else
    [EventSource(Name = "Microsoft-ApplicationInsights-WindowsServer-TelemetryChannel")]
#endif
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "appDomainName is required")]
    internal sealed class TelemetryChannelEventSource : EventSource
    {
        public static readonly TelemetryChannelEventSource Log = new TelemetryChannelEventSource();
        public readonly string ApplicationName;

        private TelemetryChannelEventSource()
        {
            this.ApplicationName = GetApplicationName();
        }

        public static bool IsVerboseEnabled
        {
            [NonEvent]
            get
            {
                return Log.IsEnabled(EventLevel.Verbose, (EventKeywords)(-1));
            }
        }

        // Verbosity is Error - so it is always sent to portal; Keyword is Diagnostics so throttling is not applied.
        [Event(1,
            Message = "Diagnostic message: backoff logic disabled, transmission will be resolved.",
            Level = EventLevel.Error,
            Keywords = Keywords.Diagnostics | Keywords.UserActionable)]
        public void BackoffDisabled(string appDomainName = "Incorrect")
        {
            this.WriteEvent(1, this.ApplicationName);
        }

        // Verbosity is Error - so it is always sent to portal; Keyword is Diagnostics so throttling is not applied.
        [Event(2,
            Message = "Diagnostic message: backoff logic was enabled. Backoff internal exceeded {0} min. Last status code received from the backend was {1}.",
            Level = EventLevel.Error,
            Keywords = Keywords.Diagnostics | Keywords.UserActionable)]
        public void BackoffEnabled(double intervalInMin, int statusCode, string appDomainName = "Incorrect")
        {
            this.WriteEvent(2, intervalInMin, statusCode, this.ApplicationName);
        }

        [Event(3, Message = "Sampling skipped: {0}.", Level = EventLevel.Verbose)]
        public void SamplingSkippedByType(string telemetryType, string appDomainName = "Incorrect")
        {
            this.WriteEvent(3, telemetryType ?? string.Empty, this.ApplicationName);
        }

        [Event(4, Message = "Backoff interval in seconds {0}.", Level = EventLevel.Verbose)]
        public void BackoffInterval(double intervalInSec, string appDomainName = "Incorrect")
        {
            this.WriteEvent(4, intervalInSec, this.ApplicationName);
        }

        [Event(5, Message = "Backend response {1} was not parsed. Some items may be dropped: {0}.", Level = EventLevel.Warning)]
        public void BreezeResponseWasNotParsedWarning(string exception, string response, string appDomainName = "Incorrect")
        {
            this.WriteEvent(5, exception ?? string.Empty, response ?? string.Empty, this.ApplicationName);
        }

        [Event(6, Message = "Unexpected backend response. Items # in batch {0} <= Error index in response: {1}.", Level = EventLevel.Warning)]
        public void UnexpectedBreezeResponseWarning(int size, int index, string appDomainName = "Incorrect")
        {
            this.WriteEvent(6, size, index, this.ApplicationName);
        }

        [Event(7, Message = "Item was rejected by endpoint. Message: {0}", Level = EventLevel.Warning)]
        public void ItemRejectedByEndpointWarning(string message, string appDomainName = "Incorrect")
        {
            this.WriteEvent(7, message ?? string.Empty, this.ApplicationName);
        }

        [Event(8, Keywords = Keywords.UserActionable, Message = "User-defined sampling callback failed. Exception: {0}.", Level = EventLevel.Error)]
        public void SamplingCallbackError(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(8, exception, this.ApplicationName);
        public void BufferEnqueued(string transmissionId, int transmissionCount, string appDomainName = "Incorrect")
        {
            this.WriteEvent(12, transmissionId ?? string.Empty, transmissionCount, this.ApplicationName);
        }

        [Event(13, Message = "BufferEnqueueNoCapacity. Size: {0}. Capacity: {1}.", Level = EventLevel.Warning)]
        public void BufferEnqueueNoCapacityWarning(long size, int capacity, string appDomainName = "Incorrect")
        {
            this.WriteEvent(13, size, capacity, this.ApplicationName);
        }
        [Event(14, Message = "UnauthorizedAccessExceptionOnTransmissionSave. TransmissionId: {0}. Message: {1}.", Level = EventLevel.Warning)]
        public void UnauthorizedAccessExceptionOnTransmissionSaveWarning(string transmissionId, string message, string appDomainName = "Incorrect")
        {
            this.WriteEvent(14, transmissionId ?? string.Empty, message ?? string.Empty, this.ApplicationName);
        }

        [Event(15, Message = "StorageSize. StorageSize: {0}.", Level = EventLevel.Verbose)]
        public void StorageSize(long size, string appDomainName = "Incorrect")
        {
            this.WriteEvent(15, size, this.ApplicationName);
        }

        [Event(16, Message = "SenderEnqueueNoCapacity. TransmissionCount: {0}. Capacity: {1}.", Level = EventLevel.Warning)]
        public void SenderEnqueueNoCapacityWarning(int transmissionCount, int capacity, string appDomainName = "Incorrect")
        {
            this.WriteEvent(16, transmissionCount, capacity, this.ApplicationName);
        }

        [Event(17, Message = "TransmissionSendStarted. TransmissionId: {0}.", Level = EventLevel.Verbose)]
        public void TransmissionSendStarted(string id, string appDomainName = "Incorrect")
                transmissionId ?? string.Empty,
                exceptionMessage ?? string.Empty,
                statusCode,
                description ?? string.Empty,
                this.ApplicationName);
        }

        [Event(24, Message = "Transmission policy failed with parsing Retry-After http header: '{0}'", Level = EventLevel.Warning)]
        public void TransmissionPolicyRetryAfterParseFailedWarning(string retryAfterHeader, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                24,
                retryAfterHeader ?? string.Empty,
                this.ApplicationName);
        }

        [Event(25, Message = "StorageEnqueueNoCapacity. Size: {0}. Capacity: {1}.", Level = EventLevel.Warning)]
        public void StorageEnqueueNoCapacityWarning(long size, long capacity, string appDomainName = "Incorrect")
        {
            this.WriteEvent(25, size, capacity, this.ApplicationName);
        }

        [Event(26, Message = "TransmissionSavedToStorage. TransmissionId: {0}.", Level = EventLevel.Verbose)]
        public void TransmissionSavedToStorage(string transmissionId, string appDomainName = "Incorrect")
        {
            this.WriteEvent(26, transmissionId ?? string.Empty, this.ApplicationName);
        }

        [Event(27, Message = "{0} changed sender capacity to {1}", Level = EventLevel.Verbose)]
        public void SenderCapacityChanged(string policyName, int newCapacity, string appDomainName = "Incorrect")
        [Event(31, Message = "BackoffTimeSetInSeconds: {0}", Level = EventLevel.Verbose)]
        public void BackoffTimeSetInSeconds(double seconds, string appDomainName = "Incorrect")
        {
            this.WriteEvent(31, seconds, this.ApplicationName);
        }

        [Event(32, Message = "NetworkIsNotAvailable", Level = EventLevel.Warning)]
        public void NetworkIsNotAvailableWarning(string appDomainName = "Incorrect")
        {
            this.WriteEvent(32, this.ApplicationName);
        }

        [Event(33, Message = "StorageCapacityReset: {0}", Level = EventLevel.Verbose)]
        public void StorageCapacityReset(string policyName, string appDomainName = "Incorrect")
        {
            this.WriteEvent(33, policyName ?? string.Empty, this.ApplicationName);
        }

        [Event(34, Message = "{0} changed storage capacity to {1}", Level = EventLevel.Verbose)]
        public void StorageCapacityChanged(string policyName, int newCapacity, string appDomainName = "Incorrect")
        {
            this.WriteEvent(34, policyName ?? string.Empty, newCapacity, this.ApplicationName);
        }

        [Event(35, Message = "ThrottlingRetryAfterParsedInSec: {0}", Level = EventLevel.Verbose)]
        public void ThrottlingRetryAfterParsedInSec(double retryAfter, string appDomainName = "Incorrect")
        {
            this.WriteEvent(35, retryAfter, this.ApplicationName);
        }

        public void SubscribeToNetworkFailureWarning(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(38, exception ?? string.Empty, this.ApplicationName);
        }

        [Event(39, Message = "SubscribeToNetworkFailure: {0}", Level = EventLevel.Warning)]
        public void ExceptionHandlerStartExceptionWarning(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(39, exception ?? string.Empty, this.ApplicationName);
        }

        [Event(45, Message = "Transmission polices failed to execute. Exception:{0}", Level = EventLevel.Error)]
        public void ApplyPoliciesError(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(45, exception ?? string.Empty, this.ApplicationName);
        }

        [Event(46, Message = "Retry-After http header: '{0}'. Transmission will be stopped.", Level = EventLevel.Warning)]
        public void RetryAfterHeaderIsPresent(string retryAfterHeader, string appDomainName = "Incorrect")
        {
        {
            this.WriteEvent(52, this.ApplicationName);
        }

        [Event(53, Message = "UnauthorizedAccessExceptionOnCalculateSizeWarning. Message: {0}.", Level = EventLevel.Warning)]
        public void UnauthorizedAccessExceptionOnCalculateSizeWarning(string message, string appDomainName = "Incorrect")
        {
            this.WriteEvent(53, message ?? string.Empty, this.ApplicationName);
        }

            Message = "Local storage access has resulted in an error. Error Info: {0}. User: {1}.",
            Level = EventLevel.Warning)]
        public void TransmissionStorageIssuesWarning(string error, string user, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                56,
                error ?? string.Empty,
                user ?? string.Empty,
                this.ApplicationName);
        }
            Keywords = Keywords.UserActionable,
            Message = "Server telemetry channel was not initialized. So persistent storage is turned off. You need to call ServerTelemetryChannel.Initialize(). Currently monitoring will continue but if telemetry cannot be sent it will be dropped.",
            Level = EventLevel.Error)]
        public void StorageNotInitializedError(string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                57,
                this.ApplicationName);
        }

        [Event(58, Message = "Telemetry item is going to storage. Last backend status code: {0}. Current delay in sec: {1}.", Level = EventLevel.Verbose)]
        public void LastBackendResponseWhenPutToStorage(int statusCode, double currentDelayInSeconds, string appDomainName = "Incorrect")
        {
            this.WriteEvent(58, statusCode, currentDelayInSeconds, this.ApplicationName);
        }

        [Event(59, Message = "Error dequeuing file: {0}. Exception: {1}.", Level = EventLevel.Warning)]
        public void TransmissionStorageDequeueIOError(string fileName, string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(59, fileName, exception, this.ApplicationName);
        public void TransmissionStorageInaccessibleFile(string fileName, string appDomainName = "Incorrect")
        {
            this.WriteEvent(61, fileName, this.ApplicationName);
        }

        [Event(
            62,
            Keywords = Keywords.Diagnostics,
            Message = "Transmission storage file '{0}' has expired and been deleted.  It was created on {1}.",
            Level = EventLevel.Warning)]
                limit,
                attempted,
                accepted,
                this.ApplicationName);
        }

        [Event(65, Message = "The backlog of unsent items has reached maximum size of {0}. Items will be dropped until the backlog is cleared.",
        Level = EventLevel.Error)]
        public void ItemDroppedAsMaximumUnsentBacklogSizeReached(int maxBacklogSize, string appDomainName = "Incorrect")
        {
                maxBacklogSize,
        public void LogError(string msg, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                66,
                msg ?? string.Empty,
                this.ApplicationName);
        }

        [Event(67, Message = "Item was rejected because it has no instrumentation key set. Item: {0}", Level = EventLevel.Verbose)]
        public void ItemRejectedNoInstrumentationKey(string item, string appDomainName = "Incorrect")
        }

        [Event(68, Message = "Failed to set access permissions on storage directory {0}. Error : {1}.", Level = EventLevel.Warning)]
        public void FailedToSetSecurityPermissionStorageDirectory(string directory, string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(68, directory, error, this.ApplicationName);
        }

        [Event(69, Message = "TransmissionDataLossError. Telemetry items are being lost here due to unknown error. TransmissionId: {0}. Error Message: {1}.", Level = EventLevel.Error)]
        public void TransmissionDataLossError(string transmissionId, string message, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                70,
                transmissionId ?? string.Empty,
                message ?? string.Empty,
                this.ApplicationName);
        }

        [Event(71, Message = "TransmissionDataLossError. Telemetry items are being lost here as the response code is not in the subset of retriable codes." +
                             "TransmissionId: {0}. Status Code: {1}.", Level = EventLevel.Warning)]
            this.WriteEvent(72, telemetryType ?? string.Empty, this.ApplicationName);
        }

        [Event(73, Message = "Configuration Error: Cannot specify both Included and Excluded types in the sampling processor. Included will be ignored.", Level = EventLevel.Warning)]
        public void SamplingConfigErrorBothTypes(string appDomainName = "Incorrect")
        {
            this.WriteEvent(73, this.ApplicationName);
        }

        [Event(74, Message = "TelemetryChannel found a telemetry item without an InstrumentationKey. This is a required field and must be set in either your config file or at application startup.", Level = EventLevel.Error, Keywords = Keywords.UserActionable)]
        public void TelemetryChannelNoInstrumentationKey(string appDomainName = "Incorrect")
        {
            this.WriteEvent(74, this.ApplicationName);
        }

        [Event(
            75,
            Keywords = Keywords.UserActionable,
            Message = "Unable to use configured StorageFolder: {2}. Please make sure the folder exist and the application has read/write permissions to the same. Currently monitoring will continue but if telemetry cannot be sent it will be dropped. User: {1} Error message: {0}.",
            Level = EventLevel.Error)]
        public void TransmissionCustomStorageError(string error, string user, string customFolder, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                75,
                error ?? string.Empty,
                user ?? string.Empty,
                customFolder ?? string.Empty,
                this.ApplicationName);
        }

        [Event(78, Message = "AuthenticatedTransmissionError. Received a failed ingestion response. TransmissionId: {0}. Status Code: {1}. Status Description: {2}", Level = EventLevel.Warning)]
        public void AuthenticationPolicyCaughtFailedIngestion(string transmissionId, string statusCode, string statusDescription, string appDomainName = "Incorrect")
        {
            this.WriteEvent(78, transmissionId ?? string.Empty, statusCode ?? string.Empty, statusDescription ?? string.Empty, this.ApplicationName);
        }

        [Event(79, Message = "Unexpected backend response. Invalid Error index in response: {0}.", Level = EventLevel.Warning)]
        public void UnexpectedBreezeResponseErrorIndexWarning(int index, string appDomainName = "Incorrect")
        {
            this.WriteEvent(79, index, this.ApplicationName);
        }

        private static string GetApplicationName()
        {
            //// We want to add application name to all events BUT
            //// It is prohibited by EventSource rules to have more parameters in WriteEvent that in event source method
            //// Parameter will be available in payload but in the next versions EventSource may
            //// start validating that number of parameters match
            //// It is not allowed to call additional methods, only WriteEvent



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
