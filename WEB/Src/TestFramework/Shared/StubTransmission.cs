namespace Microsoft.ApplicationInsights.Web.TestFramework
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

        }

        public StubTransmission(byte[] content)
            : base(new Uri("any://uri"), content, string.Empty, string.Empty)
        {
        }

        public Task SaveAsync(Stream stream)
        {
            return TaskEx.Run(() => this.OnSave(stream));
        }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
