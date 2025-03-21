namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    internal class StubTelemetrySerializer : TelemetrySerializer
        public Func<IEnumerable<ITelemetry>, CancellationToken, Task<bool>> OnSerializeAsync;

        public override void Serialize(ICollection<ITelemetry> items)
        {

        public override Task<bool> SerializeAsync(ICollection<ITelemetry> items, CancellationToken cancellationToken)


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
