#if NET452
namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System.Data.SqlClient;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.Operation;
    using Microsoft.ApplicationInsights.Extensibility;

        private const string SqlConnectionCommandText = "Open";

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfilerSqlConnectionProcessing"/> class.
        /// </summary>
        internal ProfilerSqlConnectionProcessing(TelemetryConfiguration configuration, string agentVersion, ObjectInstanceBasedOperationHolder<DependencyTelemetry> telemetryTupleHolder)
            : base(configuration, agentVersion, telemetryTupleHolder)
        {
        }              

        /// <summary>
        /// Gets SQL connection resource name.
        /// <returns>The resource name if possible otherwise empty string.</returns>
        internal override string GetDependencyName(object thisObj)
            if (connection != null)
            {
                resource = string.Join(" | ", connection.DataSource, connection.Database, SqlConnectionCommandText);
            }

            return resource;
        /// <summary>
        /// Gets SQL connection command text.
        /// </summary>
        /// <param name="thisObj">The SQL connection.</param>
        /// <returns>Returns predefined command text.</returns>
        internal override string GetCommandName(object thisObj)
        {            
            return SqlConnectionCommandText;
        }
    }
}
#endif

# This file contains partial code from the original project
# Some functionality may be missing or incomplete
