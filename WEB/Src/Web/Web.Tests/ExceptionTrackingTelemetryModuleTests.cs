namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Web.Helpers;
        private ConcurrentQueue<ITelemetry> sendItems;

        [TestInitialize]
        public void TestInit()
        {
            this.sendItems = new ConcurrentQueue<ITelemetry>();
                InstrumentationKey = Guid.NewGuid().ToString(),
                TelemetryChannel = stubTelemetryChannel
            };
        }

        [TestMethod]

            Assert.Equal(2, this.sendItems.Count);
            Assert.Equal(exception1, ((ExceptionTelemetry)this.sendItems.First()).Exception);
        }

        [TestMethod]
            var platformContext = HttpModuleHelper.GetFakeHttpContext();
            platformContext.Response.StatusCode = 500;
            platformContext.AddError(new Exception());

            using (var module = new ExceptionTrackingTelemetryModule())
            {

            Assert.Equal(SeverityLevel.Critical, ((ExceptionTelemetry)this.sendItems.First()).SeverityLevel);
        }
            using (var module = new ExceptionTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);
                module.OnError(null); // is not supposed to throw
            }
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
