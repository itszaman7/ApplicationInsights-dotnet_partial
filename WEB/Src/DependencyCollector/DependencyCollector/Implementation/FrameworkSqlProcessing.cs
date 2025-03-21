#if NET452
namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.Operation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    internal sealed class FrameworkSqlProcessing
    {
        internal CacheBasedOperationHolder TelemetryTable;
        private readonly TelemetryClient telemetryClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkSqlProcessing"/> class.
        /// </summary>
        internal FrameworkSqlProcessing(TelemetryConfiguration configuration, CacheBasedOperationHolder telemetryTupleHolder)
        {
            if (configuration == null)
                var telemetryTuple = this.TelemetryTable.Get(id);
                if (telemetryTuple == null)
                {
                    var telemetry = ClientServerDependencyTracker.BeginTracking(this.telemetryClient);
                    telemetry.Name = resourceName;
                    telemetry.Target = string.Join(" | ", dataSource, database);
                    telemetry.Type = RemoteDependencyConstants.SQL;
                    telemetry.Data = commandText;
                    this.TelemetryTable.Store(id, new Tuple<DependencyTelemetry, bool>(telemetry, false));
                }
            }
            catch (Exception exception)
            {
                DependencyCollectorEventSource.Log.CallbackError(id, "OnBeginSql", exception);
            }
            finally
            {
                Activity current = Activity.Current;
                if (current?.OperationName == ClientServerDependencyTracker.DependencyActivityName)
                {
                    current.Stop();
                }
            }
        }

        /// <summary>
        /// On end callback from Framework event source.
        /// </summary>        
        /// <param name="id">Identifier of SQL connection object.</param>
            {
                DependencyCollectorEventSource.Log.EndCallbackWithNoBegin(id.ToString(CultureInfo.InvariantCulture));
                return;
            }

            if (!telemetryTuple.Item2)
            {
                this.TelemetryTable.Remove(id);
                var telemetry = telemetryTuple.Item1;
                telemetry.Success = success;
                telemetry.ResultCode = sqlExceptionNumber != 0 ? sqlExceptionNumber.ToString(CultureInfo.InvariantCulture) : string.Empty;
                DependencyCollectorEventSource.Log.AutoTrackingDependencyItem(telemetry.Name);
                ClientServerDependencyTracker.EndTracking(this.telemetryClient, telemetry);
            }
        }

        #endregion



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
