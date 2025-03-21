namespace Microsoft.ApplicationInsights.Web
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Web;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Helpers;
    using Microsoft.ApplicationInsights.Web.Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class OperationCorrelationTelemetryInitializerTests
    {
        public void DefaultHeadersOperationCorrelationTelemetryInitializerAreNotSetByDefault()
        {
            var initializer = new OperationCorrelationTelemetryInitializer();
            Assert.IsNull(initializer.ParentOperationIdHeaderName);
            Assert.IsNull(initializer.RootOperationIdHeaderName);
        }

        [TestMethod]
        public void CustomHeadersOperationCorrelationTelemetryInitializerAreSetProperly()
        {
            initializer.ParentOperationIdHeaderName = "myParentHeader";
            initializer.RootOperationIdHeaderName = "myRootHeader";

            Assert.AreEqual("myParentHeader", ActivityHelpers.ParentOperationIdHeaderName);
            Assert.AreEqual("myRootHeader", ActivityHelpers.RootOperationIdHeaderName);

            Assert.AreEqual("myParentHeader", initializer.ParentOperationIdHeaderName);
            Assert.AreEqual("myRootHeader", initializer.RootOperationIdHeaderName);
        }

            // simulate OnBegin behavior:
            // create telemetry and start activity for children
            var requestTelemetry = source.FakeContext.CreateRequestTelemetryPrivate();
            
            // lost Acitivity / call context
            activity.Stop();

            var exceptionTelemetry = new ExceptionTelemetry();
            source.Initialize(exceptionTelemetry);

            Assert.AreEqual(2, exceptionTelemetry.Properties.Count);

            // undefined behavior for duplicates
            Assert.IsTrue(exceptionTelemetry.Properties["k1"] == "v3" || exceptionTelemetry.Properties["k1"] == "v1");
            Assert.AreEqual("v2", exceptionTelemetry.Properties["k2"]);
        }
            source.FakeContext.CreateRequestTelemetryPrivate();

            activity.Stop();

            var exceptionTelemetry = new ExceptionTelemetry();
            exceptionTelemetry.Context.Operation.Id = "guid";
            source.Initialize(exceptionTelemetry);

            Assert.IsNull(exceptionTelemetry.Context.Operation.ParentId);

            {
                get { return this.fakeContext; }
            }

            protected override HttpContext ResolvePlatformContext()
            {
                return this.fakeContext;
            }
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
