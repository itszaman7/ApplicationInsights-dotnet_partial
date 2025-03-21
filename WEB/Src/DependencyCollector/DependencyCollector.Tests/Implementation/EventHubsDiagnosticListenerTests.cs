namespace Microsoft.ApplicationInsights.DependencyCollector
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EventHubsDiagnosticListenerTests
    {
        private TelemetryConfiguration configuration;
        private List<ITelemetry> sentItems;

        [TestInitialize]
        public void TestInitialize()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;
            this.configuration = new TelemetryConfiguration();
            this.configuration.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
            this.sentItems = new List<ITelemetry>();
            this.configuration.TelemetryChannel = new StubTelemetryChannel { OnSend = item => this.sentItems.Add(item), EndpointAddress = "https://dc.services.visualstudio.com/v2/track" };
            this.configuration.InstrumentationKey = Guid.NewGuid().ToString();
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
            using (var listener = new DiagnosticListener("Microsoft.Azure.EventHubs"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.EventHubs");
                module.Initialize(this.configuration);

                listener.Write(
                    "Microsoft.Azure.EventHubs.Send.Stop", 
                    new
                    {
                        Entity = "ehname",
                        Endpoint = new Uri("sb://eventhubname.servicebus.windows.net/"),
                        PartitionKey = "SomePartitionKeyHere",
                        Status = TaskStatus.RanToCompletion
                    });

                Assert.IsFalse(this.sentItems.Any());
            }
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.EventHubs");
                module.Initialize(this.configuration);

                Activity sendActivity = null;
                Activity parentActivity = new Activity("parent").AddBaggage("k1", "v1").Start();
                parentActivity.TraceStateString = "state=some";
                var telemetry = this.TrackOperation<DependencyTelemetry>(
                    listener, 
                    "Microsoft.Azure.EventHubs.Send", 

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("Send", telemetry.Name);
                Assert.AreEqual(RemoteDependencyConstants.AzureEventHubs, telemetry.Type);
                Assert.AreEqual("sb://eventhubname.servicebus.windows.net/ehname", telemetry.Target);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.AreEqual(sendActivity.ParentSpanId.ToHexString(), telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);

                Assert.AreEqual("v1", telemetry.Properties["k1"]);
                Assert.AreEqual("eventhubname.servicebus.windows.net", telemetry.Properties["peer.hostname"]);
                Assert.AreEqual("ehname", telemetry.Properties["eh.event_hub_name"]);
                Assert.AreEqual("SomePartitionKeyHere", telemetry.Properties["eh.partition_key"]);
                    TaskStatus.RanToCompletion,
                    null,
                    () => sendActivity = Activity.Current);

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("Receive", telemetry.Name);
                Assert.AreEqual(RemoteDependencyConstants.AzureEventHubs, telemetry.Type);
                Assert.AreEqual("sb://eventhubname.servicebus.windows.net/ehname", telemetry.Target);
                Assert.IsTrue(telemetry.Success.Value);


                Assert.IsTrue(telemetry.Properties.TryGetValue("tracestate", out var tracestate));
                Assert.AreEqual("state=some", tracestate);
            }
        }

        [TestMethod]
        public void EventHubsSuccessfulReceiveShortNameIsHandled()
        {
            using (var listener = new DiagnosticListener("Microsoft.Azure.EventHubs"))
                Assert.AreEqual(RemoteDependencyConstants.AzureEventHubs, telemetry.Type);
                Assert.AreEqual("sb://eventhubname.servicebus.windows.net/ehname", telemetry.Target);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.AreEqual(sendActivity.ParentSpanId.ToHexString(), telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);

                Assert.AreEqual("v1", telemetry.Properties["k1"]);
                Assert.AreEqual("eventhubname.servicebus.windows.net", telemetry.Properties["peer.hostname"]);
                Assert.AreEqual("ehname", telemetry.Properties["eh.event_hub_name"]);
                Assert.AreEqual("SomePartitionKeyHere", telemetry.Properties["eh.partition_key"]);
                Assert.AreEqual("EventHubClient1(ehname)", telemetry.Properties["eh.client_id"]);

                Assert.IsTrue(telemetry.Properties.TryGetValue("tracestate", out var tracestate));
                Assert.AreEqual("state=some", tracestate);
            }
        }

        [TestMethod]
        public void EventHubsSuccessfulSendShortNameIsHandled()
        {
            using (var listener = new DiagnosticListener("Microsoft.Azure.EventHubs"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.EventHubs");
                module.Initialize(this.configuration);

                Activity sendActivity = null;
                Activity parentActivity = new Activity("parent").AddBaggage("k1", "v1").Start();

        [TestMethod]
        public void EventHubsSuccessfulSendIsHandledW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;
            using (var listener = new DiagnosticListener("Microsoft.Azure.EventHubs"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.EventHubs");

                Assert.AreEqual("eventhubname.servicebus.windows.net", telemetry.Properties["peer.hostname"]);
                Assert.AreEqual("ehname", telemetry.Properties["eh.event_hub_name"]);
                Assert.AreEqual("SomePartitionKeyHere", telemetry.Properties["eh.partition_key"]);
                Assert.AreEqual("EventHubClient1(ehname)", telemetry.Properties["eh.client_id"]);
            }
        }

        [TestMethod]
        public void EventHubsSuccessfulSendIsHandledWithExternalParent()
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.EventHubs");
                module.Initialize(this.configuration);

                Activity sendActivity = null;
                var telemetry = this.TrackOperation<DependencyTelemetry>(
                    listener,
                    "Microsoft.Azure.EventHubs.Send",
                    TaskStatus.RanToCompletion,
                    "parent",
                    () => sendActivity = Activity.Current);

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("Send", telemetry.Name);
                Assert.AreEqual(RemoteDependencyConstants.AzureEventHubs, telemetry.Type);
                Assert.AreEqual("sb://eventhubname.servicebus.windows.net/ehname", telemetry.Target);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.AreEqual("parent", telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
        public void EventHubsFailedSendIsHandled()
        {
            using (var module = new DependencyTrackingTelemetryModule())
            using (var listener = new DiagnosticListener("Microsoft.Azure.EventHubs"))
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.EventHubs");
                module.Initialize(this.configuration);

                Activity sendActivity = null;
                Activity parentActivity = new Activity("parent").AddBaggage("k1", "v1").Start();
                Assert.IsNotNull(telemetry);
                Assert.AreEqual("Send", telemetry.Name);
                Assert.AreEqual(RemoteDependencyConstants.AzureEventHubs, telemetry.Type);
                Assert.AreEqual("sb://eventhubname.servicebus.windows.net/ehname", telemetry.Target);
                Assert.IsFalse(telemetry.Success.Value);

                Assert.AreEqual(sendActivity.ParentSpanId.ToHexString(), telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);

            using (var listener = new DiagnosticListener("Microsoft.Azure.EventHubs"))
            {
                this.configuration.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.EventHubs");
                module.Initialize(this.configuration);

                Activity parentActivity = new Activity("parent").AddBaggage("k1", "v1").Start();
                if (listener.IsEnabled("Microsoft.Azure.EventHubs.Send.Exception"))
                {
                    listener.Write("Microsoft.Azure.EventHubs.Send.Exception", new { Exception = new Exception("123") });

        private T TrackOperation<T>(
            DiagnosticListener listener, 
            string activityName,
            TaskStatus status,
            string parentId = null,
            Action operation = null) where T : OperationTelemetry
        {
            Activity activity = null;
            int itemCountBefore = this.sentItems.Count;
                activity.AddTag("peer.hostname", "eventhubname.servicebus.windows.net");
                activity.AddTag("eh.event_hub_name", "ehname");
                activity.AddTag("eh.partition_key", "SomePartitionKeyHere");
                activity.AddTag("eh.client_id", "EventHubClient1(ehname)");

                if (Activity.Current == null && parentId != null)
                {
                    activity.SetParentId(parentId);
                }

                listener.StopActivity(
                    activity,
                    new
                    {
                        Entity = "ehname",
                        Endpoint = new Uri("sb://eventhubname.servicebus.windows.net/"),
                        PartitionKey = "SomePartitionKeyHere",
                        Status = status
                    });

                // a single new telemetry item was added
                Assert.AreEqual(itemCountBefore + 1, this.sentItems.Count);
                return this.sentItems.Last() as T;
            }

            // no new telemetry items were added
            Assert.AreEqual(itemCountBefore, this.sentItems.Count);
            return null;
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
