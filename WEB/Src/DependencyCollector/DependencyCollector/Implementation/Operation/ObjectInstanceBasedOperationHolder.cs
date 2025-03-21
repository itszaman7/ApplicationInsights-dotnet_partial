namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation.Operation
{
    using System;
    using System.Runtime.CompilerServices;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    internal class ObjectInstanceBasedOperationHolder<TTelemetry> where TTelemetry : OperationTelemetry
    {
        private ConditionalWeakTable<object, Tuple<TTelemetry, bool>> weakTableForCorrelation = new ConditionalWeakTable<object, Tuple<TTelemetry, bool>>();

            return result;
        }

        public bool Remove(object holderInstance)
        {
            if (holderInstance == null)
            {
                throw new ArgumentNullException(nameof(holderInstance));
            }

            return this.weakTableForCorrelation.Remove(holderInstance);
        }

            if (telemetryTuple == null)
            {
                throw new ArgumentNullException(nameof(telemetryTuple));
            }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
