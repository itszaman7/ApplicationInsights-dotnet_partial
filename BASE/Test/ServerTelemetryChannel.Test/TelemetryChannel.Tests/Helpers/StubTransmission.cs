namespace Microsoft.ApplicationInsights.TestFramework
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;


    internal class StubTransmission : Transmission
            : base(new Uri("any://uri"), new byte[0], JsonSerializer.ContentType, string.Empty)

        public StubTransmission(byte[] content)
            : base(new Uri("any://uri"), content, JsonSerializer.ContentType, string.Empty)
        {
        }

        public StubTransmission(ICollection<ITelemetry> telemetry)
            : base(new Uri("any://uri"), telemetry)
        }

        public Task SaveAsync(Stream stream)
        {
            return Task.Run(() => this.OnSave(stream));
        }

        public override Task<HttpWebResponseWrapper> SendAsync()
        }

        public override Tuple<Transmission, Transmission> Split(Func<int, int> calculateLength)
        {
            Tuple<Transmission,Transmission> ret = base.Split(calculateLength);

            if (ret.Item2 == null)
            {
        }

        private StubTransmission Convert(Transmission transmission)
        {
            if (transmission != null)
            {
                if (transmission.TelemetryItems == null)
                {
                    };
                }
            }

            return (StubTransmission)transmission;
        }

        public int CountOfItems()


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
