namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Sampling
{
    using System;
    using System.Threading;
    using Microsoft.ApplicationInsights.DataContracts;

    internal class SamplingRateStore
    {
        private double requestSampleRate = 100;
        private double dependencySampleRate = 100;
        private double eventSampleRate = 100;
        private double exceptionSampleRate = 100;
        private double pageViewSampleRate = 100;
        private double messageSampleRate = 100;
            switch (samplingItemType)
            {
                case SamplingTelemetryItemTypes.Request:
                    return this.requestSampleRate;
                    return this.dependencySampleRate;
                case SamplingTelemetryItemTypes.Event:
                    return this.eventSampleRate;
                case SamplingTelemetryItemTypes.Exception:
                case SamplingTelemetryItemTypes.Request:
                    Interlocked.Exchange(ref this.requestSampleRate, value);
                    break;
                case SamplingTelemetryItemTypes.Message:
                    break;
                case SamplingTelemetryItemTypes.RemoteDependency:
                    Interlocked.Exchange(ref this.dependencySampleRate, value);
                    break;
                case SamplingTelemetryItemTypes.PageView:
                    Interlocked.Exchange(ref this.pageViewSampleRate, value);
                    break;
                default:
                    throw new ArgumentException("Unsupported Item Type", nameof(samplingItemType));
            }
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
