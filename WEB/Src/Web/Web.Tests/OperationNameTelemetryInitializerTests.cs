namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Diagnostics;
    using System.Web;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Helpers;
    using Microsoft.ApplicationInsights.Web.Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class OperationNameTelemetryInitializerTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            while (Activity.Current != null)
            {
                Activity.Current.Stop();
            }
        }

        [TestMethod]
        public void InitializeDoesNotThrowWhenHttpContextIsNull()
        {
            var requestTelemetry = new RequestTelemetry();

            var source = new OperationNameTelemetryInitializer();
            source.Initialize(requestTelemetry);

            Assert.AreEqual(string.Empty, requestTelemetry.Name);
        }

        public void TestRequestNameWithControllerAndWithAction()
        {
            var platformContext = HttpModuleHelper.GetFakeHttpContext();

            platformContext.Request.RequestContext.RouteData.Values.Add("controller", "Controller");
            platformContext.Request.RequestContext.RouteData.Values.Add("action", "Action");
            platformContext.Request.RequestContext.RouteData.Values.Add("id2", 10);
            platformContext.Request.RequestContext.RouteData.Values.Add("id1", 10);

            string requestName = platformContext.CreateRequestNamePrivate();
            Assert.AreEqual("GET Controller/Action", requestName);
        }

        public void TestRequestNameWithNoControllerAndWithAction()
            platformContext.Request.RequestContext.RouteData.Values.Add("controller", "Controller");
            
            string requestName = platformContext.CreateRequestNamePrivate();
            Assert.AreEqual("GET Controller", requestName);
        }

        public void TestRequestNameWithControllerAndWithNoActionWithParameters()
        {
            var platformContext = HttpModuleHelper.GetFakeHttpContext();

            platformContext.Request.RequestContext.RouteData.Values.Add("controller", "Controller");
            
            // Note that parameters are not sorted here:
            platformContext.Request.RequestContext.RouteData.Values.Add("id2", 10);
            platformContext.Request.RequestContext.RouteData.Values.Add("id1", 10);

            string requestName = platformContext.CreateRequestNamePrivate();
            Assert.AreEqual("GET Controller [id1/id2]", requestName);
        }

        public void TestRequestNameWithNoControllerAndWithNoAction()
        {
            var platformContext = HttpModuleHelper.GetFakeHttpContext();

            platformContext.Request.RequestContext.RouteData.Values.Add("id2", 10);
            platformContext.Request.RequestContext.RouteData.Values.Add("id1", 10);

            string requestName = platformContext.CreateRequestNamePrivate();
            Assert.AreEqual("GET " + HttpModuleHelper.UrlPath, requestName);
        }

        public void TestRequestNameRouteDataEmpty()
        {
            var platformContext = HttpModuleHelper.GetFakeHttpContext();

            string requestName = platformContext.CreateRequestNamePrivate();
            Assert.AreEqual("GET " + HttpModuleHelper.UrlPath, requestName);
        }

        [TestMethod]

            Assert.AreEqual("GET " + HttpModuleHelper.UrlPath, requestTelemetry.Context.Operation.Name);
        }

        [TestMethod]
        public void InitializeSetsCustomerRequestOperationNameFromContextIfRootRequestNameIsEmpty()
        {
            var source = new TestableOperationNameTelemetryInitializer();
            var rootRequest = source.FakeContext.CreateRequestTelemetryPrivate();
            Assert.AreEqual(string.Empty, rootRequest.Name);
            RequestTelemetry customerRequestTelemetry = new RequestTelemetry();

            source.Initialize(customerRequestTelemetry);

            Assert.AreEqual("GET " + HttpModuleHelper.UrlPath, customerRequestTelemetry.Context.Operation.Name);
        }

        [TestMethod]
        public void InitializeSetsCustomerRequestOperationNameFromRequestIfRequestNameIsNotEmpty()
        {

            source.Initialize(customerTelemetry);

            Assert.AreEqual("Name", customerTelemetry.Context.Operation.Name);
        }

        [TestMethod]
        public void InitializeSetsExceptionOperationName()
        {
            var exceptionTelemetry = CreateExceptionTelemetry();
            var source = new TestableOperationNameTelemetryInitializer();
            source.FakeContext.CreateRequestTelemetryPrivate();

            source.Initialize(exceptionTelemetry);

            Assert.AreEqual("GET " + HttpModuleHelper.UrlPath, exceptionTelemetry.Context.Operation.Name);
        }

        [TestMethod]
        public void InitializeSetsOperationNameWhenRequestTelemetryIsMissingInHttpContext()
        {
            var telemetry = CreateRequestTelemetry();

            var source = new TestableOperationNameTelemetryInitializer();
            source.Initialize(telemetry);

            Assert.AreEqual("GET " + HttpModuleHelper.UrlPath, telemetry.Context.Operation.Name);
        }

        private static RequestTelemetry CreateRequestTelemetry()
            item.Properties.Add("httpMethod", "GET");


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
