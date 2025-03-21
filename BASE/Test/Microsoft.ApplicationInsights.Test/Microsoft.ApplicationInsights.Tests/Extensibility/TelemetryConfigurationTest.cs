namespace Microsoft.ApplicationInsights.Extensibility
{
    using System;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.ObjectModel;

    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.ApplicationInsights.Channel;

    using System.Diagnostics;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId;

    [TestClass]
    public class TelemetryConfigurationTest
    {
        #region W3C
        [TestMethod]
        public void TelemetryConfigurationStaticConstructorSetsW3CToTrueIfNotEnforced()
        {
            try
            {
                // Accessing TelemetryConfiguration trigger static constructor
                var tc = new TelemetryConfiguration();

                Assert.IsTrue(Activity.ForceDefaultIdFormat);
                Assert.AreEqual(ActivityIdFormat.W3C, Activity.DefaultIdFormat);
            }
            finally
            {
                Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
                Activity.ForceDefaultIdFormat = false;
            }
        }

        #endregion

        [TestMethod]
        public void TelemetryConfigurationIsPublicToAllowUsersManipulateConfigurationProgrammatically()
        {
            Assert.IsTrue(typeof(TelemetryConfiguration).GetTypeInfo().IsPublic);
        }

        [TestMethod]
        public void NewTelemetryConfigurationWithChannelUsesSpecifiedChannel()
        {
            StubTelemetryChannel stubChannel = new StubTelemetryChannel();
            bool channelDisposed = false;
            stubChannel.OnDispose += () => { channelDisposed = true; };
            TelemetryConfiguration config = new TelemetryConfiguration(string.Empty, stubChannel);
            Assert.AreSame(stubChannel, config.TelemetryChannel);
            config.Dispose();
            Assert.IsFalse(channelDisposed);
        }

        [TestMethod]
        public void NewTelemetryConfigurationWithoutChannelCreatesDefaultInMemoryChannel()
        {
            TelemetryConfiguration config = new TelemetryConfiguration();
            var channel = config.TelemetryChannel as Channel.InMemoryChannel;
            Assert.IsNotNull(channel);
            config.Dispose();
            Assert.IsTrue(channel.IsDisposed);
        }

        [TestMethod]
        public void NewTelemetryConfigurationWithInstrumentationKeyAndChannelUsesSpecifiedKeyAndChannel()
        {
            string expectedKey = "expected";
            StubTelemetryChannel stubChannel = new StubTelemetryChannel();
            bool channelDisposed = false;
            stubChannel.OnDispose += () => { channelDisposed = true; };
            TelemetryConfiguration config = new TelemetryConfiguration(expectedKey, stubChannel);
            Assert.AreEqual(expectedKey, config.InstrumentationKey);
            Assert.AreSame(stubChannel, config.TelemetryChannel);
            config.Dispose();
            Assert.IsFalse(channelDisposed);
        }

        [TestMethod]
        public void NewTelemetryConfigurationWithInstrumentationKeyButNoChannelCreatesDefaultInMemoryChannel()
        {
            string expectedKey = "expected";
            TelemetryConfiguration config = new TelemetryConfiguration(expectedKey);
            Assert.AreEqual(expectedKey, config.InstrumentationKey);
            var channel = config.TelemetryChannel as Channel.InMemoryChannel;
            Assert.IsNotNull(channel);
            config.Dispose();
            Assert.IsTrue(channel.IsDisposed);
        }

        #region Active

        [TestMethod]
        public void ActiveIsPublicToAllowUsersToAccessActiveTelemetryConfigurationInAdvancedScenarios()
        {
            Assert.IsTrue(typeof(TelemetryConfiguration).GetTypeInfo().GetDeclaredProperty("Active").GetGetMethod(true).IsPublic);
        }

        [TestMethod]
        public void ActiveSetterIsInternalAndNotMeantToBeUsedByOurCustomers()
        {
            Assert.IsFalse(typeof(TelemetryConfiguration).GetTypeInfo().GetDeclaredProperty("Active").GetSetMethod(true).IsPublic);
        }
#pragma warning disable 612, 618
        [TestMethod]
        public void ActiveIsLazilyInitializedToDelayCostOfLoadingConfigurationFromFile()
        {
            try
            {
                TelemetryConfiguration.Active = null;
                Assert.IsNotNull(TelemetryConfiguration.Active);
            }
        {
            TelemetryModules modules = new TestableTelemetryModules();
            TelemetryConfigurationFactory.Instance = new StubTelemetryConfigurationFactory
            {
                OnInitialize = (c, m) =>
                {
                    modules = m;
                },
            };

            TelemetryConfiguration.Active = null;
            Assert.IsNotNull(TelemetryConfiguration.Active);

            Assert.AreSame(modules, TelemetryModules.Instance);
        }

        [TestMethod]
        public void ActiveUsesTelemetryConfigurationFactoryToInitializeTheInstance()
        {
            bool factoryInvoked = false;
            {
                var dummy = TelemetryConfiguration.Active;
                Assert.IsTrue(factoryInvoked);
            }
            finally
            {
                TelemetryConfigurationFactory.Instance = null;
                TelemetryConfiguration.Active = null;
            }
        }

        [TestMethod]
        public void ActiveInitializesSingleInstanceRegardlessOfNumberOfThreadsTryingToAccessIt()
        {
            int numberOfInstancesInitialized = 0;
            TelemetryConfiguration.Active = null;
            TelemetryConfigurationFactory.Instance = new StubTelemetryConfigurationFactory
            {
                OnInitialize = (configuration, _) => { Interlocked.Increment(ref numberOfInstancesInitialized); },
            };
            {
                var tasks = new Task[8];
                for (int i = 0; i < tasks.Length; i++)
                {
                    tasks[i] = Task.Run(() => TelemetryConfiguration.Active);
                }

                Task.WaitAll(tasks);
                Assert.AreEqual(1, numberOfInstancesInitialized);
            }
            finally
            {
                TelemetryConfiguration.Active = null;
                TelemetryConfigurationFactory.Instance = null;
            }
        }

        [TestMethod]
        [Timeout(1000)]
        public void ActiveInitializesSingleInstanceWhenConfigurationComponentsAccessActiveRecursively()
        {
            int numberOfInstancesInitialized = 0;
            TelemetryConfiguration.Active = null;
            TelemetryConfigurationFactory.Instance = new StubTelemetryConfigurationFactory
            {
                OnInitialize = (configuration, _) =>
                {
                    Interlocked.Increment(ref numberOfInstancesInitialized);
                    var dummy = TelemetryConfiguration.Active;
                },
            };
            try
            {
                var dummy = TelemetryConfiguration.Active;
                Assert.AreEqual(1, numberOfInstancesInitialized);
            }
            finally
            {
                TelemetryConfiguration.Active = null;
                TelemetryConfigurationFactory.Instance = null;
            }
        }
#pragma warning restore 612, 618
        #endregion

        #region CreateDefault

        [TestMethod]
        public void DefaultDoesNotInitializeTelemetryModuleCollection()
        {
            TelemetryModules modules = new TestableTelemetryModules();
            TelemetryConfigurationFactory.Instance = new StubTelemetryConfigurationFactory
            {
                OnInitialize = (c, m) =>
                {
                    modules = m;
                },
            };

            Assert.IsNotNull(TelemetryConfiguration.CreateDefault());
        [TestMethod]
        public void InstrumentationKeyCanBeSetToProgrammaticallyDefineInstrumentationKeyForAllContextsInApplication()
        {
            var configuration = new TelemetryConfiguration();
            configuration.InstrumentationKey = "99C6A712-B2B5-46E3-97F4-F83F69999324";
            Assert.AreEqual("99C6A712-B2B5-46E3-97F4-F83F69999324", configuration.InstrumentationKey);
        }

        #endregion

        #region Connection String
        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifySetConnectionString_ShouldSetConnectionString()
        {
            var ikey = Guid.NewGuid().ToString();
            var connectionString = $"InstrumentationKey={ikey}";

            var configuration = new TelemetryConfiguration
            {
                ConnectionString = connectionString
            };

            Assert.AreEqual(connectionString, configuration.ConnectionString, "connection string was not set.");
            Assert.AreEqual(ikey, configuration.InstrumentationKey, "instrumentation key was not set.");
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        [ExpectedException(typeof(ArgumentNullException))]
            var configuration = new TelemetryConfiguration
            {
                ConnectionString = null
            };
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifySetConnectionString_SetsEndpoint()
        {
        public void VerifySetConnectionString_SetsChannelDefaultEndpoint()
        {
            var connectionString = $"InstrumentationKey=00000000-0000-0000-0000-000000000000";

            var channel = new InMemoryChannel();

            var configuration = new TelemetryConfiguration
            {
                TelemetryChannel = channel,
                ConnectionString = connectionString,
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifySetConnectionString_SetsChannelExpliticEndpoint()
        {
            var explicitEndpoint = "https://127.0.0.1/";
            var connectionString = $"InstrumentationKey=00000000-0000-0000-0000-000000000000;IngestionEndpoint={explicitEndpoint}";

            var channel = new InMemoryChannel();
        [TestCategory("ConnectionString")]
        public void Configuration_DefaultScenario()
        {
            var configuration = new TelemetryConfiguration();

            Assert.AreEqual(string.Empty, configuration.InstrumentationKey);
            Assert.AreEqual("https://dc.services.visualstudio.com/v2/track", configuration.DefaultTelemetrySink.TelemetryChannel.EndpointAddress);
        }

        [TestMethod]

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void Configuration_CreateDefaultScenario_WithConnectionString()
        {
            var configuration = TelemetryConfiguration.CreateDefault();
            configuration.ConnectionString = "InstrumentationKey=00000000-0000-0000-0000-000000000000;IngestionEndpoint=https://127.0.0.1/";

            Assert.AreEqual("00000000-0000-0000-0000-000000000000", configuration.InstrumentationKey);
            Assert.AreEqual("https://127.0.0.1/v2/track", configuration.DefaultTelemetrySink.TelemetryChannel.EndpointAddress);
        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifySetConnectionString_SetsApplicationIdProvider_FromConnectionString_Reverse()
        {
            var connectionString = $"InstrumentationKey=00000000-0000-0000-0000-000000000000";

            var applicationIdProvider = new ApplicationInsightsApplicationIdProvider();

            var configuration = new TelemetryConfiguration
            {
                Next = applicationIdProvider
            };

            var configuration = new TelemetryConfiguration
            {
                ConnectionString = connectionString,
            };

            configuration.ApplicationIdProvider = applicationIdProvider;

            Assert.AreEqual("00000000-0000-0000-0000-000000000000", configuration.InstrumentationKey);
            Assert.AreEqual("https://dc.services.visualstudio.com/", configuration.EndpointContainer.Ingestion.AbsoluteUri);
            Assert.AreEqual("https://dc.services.visualstudio.com/api/profiles/{0}/appId", applicationIdProvider.ProfileQueryEndpoint);
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifySetConnectionString_IgnoresDictionaryApplicationIdProvider()
        {
            var connectionString = $"InstrumentationKey=00000000-0000-0000-0000-000000000000";

            var applicationIdProvider = new DictionaryApplicationIdProvider();

            var configuration = new TelemetryConfiguration
            {
                ApplicationIdProvider = applicationIdProvider,
                ConnectionString = connectionString,
            };

            Assert.AreEqual("00000000-0000-0000-0000-000000000000", configuration.InstrumentationKey);

        #region TelemetryChannel

        [TestMethod]
        public void TelemetryChannelCanBeSetByUserToReplaceDefaultChannelForTesting()
        {
            var configuration = new TelemetryConfiguration();

            var customChannel = new StubTelemetryChannel();
            configuration.TelemetryChannel = customChannel;
            
            AssertEx.IsType<ReadOnlyCollection<ITelemetryProcessor>>(configuration.TelemetryProcessors);
        }

        #endregion

        #region Serialized Configuration
        [TestMethod]
        public void TelemetryConfigThrowsIfSerializedConfigIsNull()
        {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
