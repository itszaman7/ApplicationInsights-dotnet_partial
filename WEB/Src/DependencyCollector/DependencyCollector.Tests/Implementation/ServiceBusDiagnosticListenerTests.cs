namespace Microsoft.ApplicationInsights.DependencyCollector
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ServiceBusDiagnosticListenerTests
    {
        private TelemetryConfiguration configuration;
        private List<ITelemetry> sentItems;

        [TestInitialize]
        public void TestInitialize()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;
            this.configuration = new TelemetryConfiguration();
            this.sentItems = new List<ITelemetry>();
            this.configuration.TelemetryChannel = new StubTelemetryChannel { OnSend = item => this.sentItems.Add(item), EndpointAddress = "https://dc.services.visualstudio.com/v2/track" };
            this.configuration.InstrumentationKey = Guid.NewGuid().ToString();
            this.configuration.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
        }

        [TestCleanup]
        public void CleanUp()
        {
            while (Activity.Current != null)
            {
                Activity.Current.Stop();
            }
        }

        [TestMethod]
        public void DiagnosticEventWithoutActivityIsIgnored()
        {
            using (var listener = new DiagnosticListener("Microsoft.Azure.ServiceBus"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
                module.Initialize(this.configuration);

                listener.Write(
                    "Microsoft.Azure.ServiceBus.Send.Stop",
                    new
                    {
                        Entity = "queueName",
                        Endpoint = new Uri("sb://queuename.myservicebus.com/")
                module.Initialize(this.configuration);

                Activity sendActivity = null;
                Activity parentActivity = new Activity("parent").AddBaggage("k1", "v1").Start();
                parentActivity.TraceStateString = "state=some";
                var telemetry = this.TrackOperation<DependencyTelemetry>(listener,
                    "Microsoft.Azure.ServiceBus.Send", 
                    TaskStatus.RanToCompletion,
                    null,
                    () => sendActivity = Activity.Current);

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("Send", telemetry.Name);
                Assert.AreEqual(RemoteDependencyConstants.AzureServiceBus, telemetry.Type);
                Assert.AreEqual("sb://queuename.myservicebus.com/ | queueName", telemetry.Target);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.AreEqual(parentActivity.SpanId.ToHexString(), telemetry.Context.Operation.ParentId);
                Assert.AreEqual(parentActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);

        [TestMethod]
        public void ServiceBusSendHandingW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;
            using (var listener = new DiagnosticListener("Microsoft.Azure.ServiceBus"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
                module.Initialize(this.configuration);

                Activity parentActivity = new Activity("parent").AddBaggage("k1", "v1").Start();
                var telemetry = this.TrackOperation<DependencyTelemetry>(listener,
                    "Microsoft.Azure.ServiceBus.Send", TaskStatus.RanToCompletion);

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("Send", telemetry.Name);
                Assert.AreEqual(RemoteDependencyConstants.AzureServiceBus, telemetry.Type);
                Assert.AreEqual("sb://queuename.myservicebus.com/ | queueName", telemetry.Target);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.AreEqual(parentActivity.Id, telemetry.Context.Operation.ParentId);
                Assert.AreEqual(parentActivity.RootId, telemetry.Context.Operation.Id);

                Assert.AreEqual("v1", telemetry.Properties["k1"]);
                Assert.AreEqual("messageId", telemetry.Properties["MessageId"]);
            }
        }

        [TestMethod]
        public void ServiceBusSendHandingWithoutParent()
        {
            using (var listener = new DiagnosticListener("Microsoft.Azure.ServiceBus"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
                module.Initialize(this.configuration);

                Activity sendActivity = null;
                var telemetry = this.TrackOperation<DependencyTelemetry>(
                    listener,
                    "Microsoft.Azure.ServiceBus.Send",
                    TaskStatus.RanToCompletion,
                    null,
                    () => sendActivity = Activity.Current);

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("Send", telemetry.Name);
                Assert.AreEqual(RemoteDependencyConstants.AzureServiceBus, telemetry.Type);
        public void ServiceBusBadStatusHanding()
        {
            using (var listener = new DiagnosticListener("Microsoft.Azure.ServiceBus"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
                module.Initialize(this.configuration);

                Activity sendActivity = null;
                Activity parentActivity = new Activity("parent").AddBaggage("k1", "v1").Start();
                Assert.AreEqual("Send", telemetry.Name);
                Assert.AreEqual(RemoteDependencyConstants.AzureServiceBus, telemetry.Type);
                Assert.AreEqual("sb://queuename.myservicebus.com/ | queueName", telemetry.Target);
                Assert.IsFalse(telemetry.Success.Value);

                Assert.AreEqual(parentActivity.SpanId.ToHexString(), telemetry.Context.Operation.ParentId);
                Assert.AreEqual(parentActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);

                Assert.AreEqual("v1", telemetry.Properties["k1"]);
                messageActivity = Activity.Current;
                tc.TrackTrace("trace");
            }

            using (var listener = new DiagnosticListener("Microsoft.Azure.ServiceBus"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
                module.Initialize(this.configuration);


                Assert.AreEqual("v1", requestTelemetry.Properties["k1"]);

                Assert.AreEqual("messageId", requestTelemetry.Properties["MessageId"]);

                var traceTelemetry = this.sentItems.OfType<TraceTelemetry>();
                Assert.AreEqual(1, traceTelemetry.Count());

                Assert.AreEqual(requestTelemetry.Context.Operation.Id, traceTelemetry.Single().Context.Operation.Id);
                Assert.AreEqual(requestTelemetry.Id, traceTelemetry.Single().Context.Operation.ParentId);
            void TrackTraceDuringProcessing()
            {
                messageActivity = Activity.Current;
                tc.TrackTrace("trace");
            }

            using (var listener = new DiagnosticListener("Microsoft.Azure.ServiceBus"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");

                Activity parentActivity = new Activity("parent").AddBaggage("k1", "v1").Start();
                var requestTelemetry = this.TrackOperation<RequestTelemetry>(
                    listener,
                    "Microsoft.Azure.ServiceBus.Process",
                    TaskStatus.RanToCompletion,
                    operation: TrackTraceDuringProcessing);

                Assert.IsNotNull(requestTelemetry);
                Assert.AreEqual("Process", requestTelemetry.Name);
                Assert.AreEqual(parentActivity.Id, requestTelemetry.Context.Operation.ParentId);
                Assert.AreEqual(parentActivity.RootId, requestTelemetry.Context.Operation.Id);
                Assert.AreEqual(messageActivity.Id, requestTelemetry.Id);
                Assert.AreEqual("v1", requestTelemetry.Properties["k1"]);

                Assert.AreEqual("messageId", requestTelemetry.Properties["MessageId"]);

                var traceTelemetry = this.sentItems.OfType<TraceTelemetry>();
                Assert.AreEqual(1, traceTelemetry.Count());

            using (var listener = new DiagnosticListener("Microsoft.Azure.ServiceBus"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
                module.Initialize(this.configuration);

                var requestTelemetry = this.TrackOperation<RequestTelemetry>(
                    listener,
                    "Microsoft.Azure.ServiceBus.Process", 
                    TaskStatus.RanToCompletion, 
                Assert.IsNotNull(requestTelemetry);
                Assert.AreEqual("Process", requestTelemetry.Name);
                Assert.AreEqual(
                    $"type:{RemoteDependencyConstants.AzureServiceBus} | name:queueName | endpoint:sb://queuename.myservicebus.com/",
                    requestTelemetry.Source);
                Assert.IsTrue(requestTelemetry.Success.Value);

                Assert.IsTrue(requestTelemetry.Properties.TryGetValue("ai_legacyRootId", out var legacyRoot));
                Assert.AreEqual("hierarchical-parent", legacyRoot);
                Assert.AreEqual("|hierarchical-parent.", requestTelemetry.Context.Operation.ParentId);
                Assert.AreEqual(messageActivity.SpanId.ToHexString(), requestTelemetry.Id);
                Assert.AreEqual(messageActivity.TraceId.ToHexString(), requestTelemetry.Context.Operation.Id);
                Assert.AreEqual("messageId", requestTelemetry.Properties["MessageId"]);

                var traceTelemetry = this.sentItems.OfType<TraceTelemetry>();
                Assert.AreEqual(1, traceTelemetry.Count());

                Assert.AreEqual(requestTelemetry.Context.Operation.Id, traceTelemetry.Single().Context.Operation.Id);
                Assert.AreEqual(requestTelemetry.Id, traceTelemetry.Single().Context.Operation.ParentId);
            }
            {
                messageActivity = Activity.Current;
                tc.TrackTrace("trace");
            }

            using (var listener = new DiagnosticListener("Microsoft.Azure.ServiceBus"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
                module.Initialize(this.configuration);
                var traceTelemetry = this.sentItems.OfType<TraceTelemetry>();
                Assert.AreEqual(1, traceTelemetry.Count());

                Assert.AreEqual(requestTelemetry.Context.Operation.Id, traceTelemetry.Single().Context.Operation.Id);
                Assert.AreEqual(requestTelemetry.Id, traceTelemetry.Single().Context.Operation.ParentId);
            }
        }

        [TestMethod]
        public void ServiceBusProcessHandingExternalMalformedParent()
        {
            var tc = new TelemetryClient(this.configuration);

            Activity messageActivity = null;
                var requestTelemetry = this.TrackOperation<RequestTelemetry>(
                    listener,
                    "Microsoft.Azure.ServiceBus.Process",
                    TaskStatus.RanToCompletion,
                    "malformed-parent",
                    TrackTraceDuringProcessing);

                Assert.IsNotNull(requestTelemetry);
                Assert.AreEqual("Process", requestTelemetry.Name);
                Assert.AreEqual(
                    $"type:{RemoteDependencyConstants.AzureServiceBus} | name:queueName | endpoint:sb://queuename.myservicebus.com/",
                    requestTelemetry.Source);
                Assert.IsTrue(requestTelemetry.Success.Value);

                Assert.IsTrue(requestTelemetry.Properties.TryGetValue("ai_legacyRootId", out var legacyRoot));
                Assert.AreEqual("malformed-parent", legacyRoot);
                Assert.AreEqual("malformed-parent", requestTelemetry.Context.Operation.ParentId);
                Assert.AreEqual(messageActivity.SpanId.ToHexString(), requestTelemetry.Id);
                Assert.AreEqual(messageActivity.TraceId.ToHexString(), requestTelemetry.Context.Operation.Id);
                Assert.AreEqual("messageId", requestTelemetry.Properties["MessageId"]);

        [TestMethod]
        public void ServiceBusProcessHandingWithoutParent()
        {
            var tc = new TelemetryClient(this.configuration);
            Activity messageActivity = null;
            void TrackTraceDuringProcessing()
            {
                messageActivity = Activity.Current;
                tc.TrackTrace("trace");

            var tc = new TelemetryClient(this.configuration);
            Activity messageActivity = null;
            void TrackTraceDuringProcessing()
            {
                messageActivity = Activity.Current;
                tc.TrackTrace("trace");
            }

            using (var listener = new DiagnosticListener("Microsoft.Azure.ServiceBus"))
                    listener,
                    "Microsoft.Azure.ServiceBus.Process",
                    TaskStatus.RanToCompletion,
                    "parent",
                    TrackTraceDuringProcessing);

                Assert.IsNotNull(requestTelemetry);
                Assert.AreEqual("Process", requestTelemetry.Name);
                Assert.AreEqual(
                    $"type:{RemoteDependencyConstants.AzureServiceBus} | name:queueName | endpoint:sb://queuename.myservicebus.com/",
        }

        [TestMethod]
        public void ServiceBusExceptionsAreIgnored()
        {
            using (var listener = new DiagnosticListener("Microsoft.Azure.ServiceBus"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                this.configuration.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
                }
            }

            operation?.Invoke();

            if (activity != null)
            {
                listener.StopActivity(activity, new { Entity = "queueName", Endpoint = new Uri("sb://queuename.myservicebus.com/"), Status = status });
                return this.sentItems.OfType<T>().Last();
            }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
