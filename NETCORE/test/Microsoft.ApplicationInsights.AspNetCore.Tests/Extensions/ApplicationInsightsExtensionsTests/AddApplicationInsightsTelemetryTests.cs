using Xunit;

namespace Microsoft.Extensions.DependencyInjection.Test
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Logging;

    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.AspNetCore;
    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
    using Microsoft.ApplicationInsights.AspNetCore.Logging;
    using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
    using Microsoft.ApplicationInsights.AspNetCore.Tests;
    using Microsoft.ApplicationInsights.AspNetCore.Tests.Helpers;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.Extensibility;
#if NETCOREAPP
    using Microsoft.ApplicationInsights.Extensibility.EventCounterCollector;
#endif
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
    using Microsoft.ApplicationInsights.Extensibility.W3C;
    using Microsoft.ApplicationInsights.WindowsServer;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Options;

    public class AddApplicationInsightsTelemetryTests : BaseTestClass
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public static void TelemetryModulesResolvableWhenKeyedServiceRegistered(bool manuallyRegisterDiagnosticsTelemetryModule)
        {
            // Note: This test verifies a regression doesn't get introduced for:
            // https://github.com/microsoft/ApplicationInsights-dotnet/issues/2879

            var services = new ServiceCollection();

            services.AddSingleton<ITelemetryModule, TestTelemetryModule>();
            if (manuallyRegisterDiagnosticsTelemetryModule)
            {
                services.AddSingleton<ITelemetryModule, DiagnosticsTelemetryModule>();
            }

            services.AddKeyedSingleton(typeof(ITestService), serviceKey: new(), implementationType: typeof(TestService));
            services.AddKeyedSingleton(typeof(ITestService), serviceKey: new(), implementationInstance: new TestService());
            services.AddKeyedSingleton(typeof(ITestService), serviceKey: new(), implementationFactory: (sp, key) => new TestService());

            services.AddApplicationInsightsTelemetry();

            using var sp = services.BuildServiceProvider();

            var telemetryModules = sp.GetServices<ITelemetryModule>();

            Assert.Equal(8, telemetryModules.Count());

            Assert.Single(telemetryModules.Where(m => m.GetType() == typeof(DiagnosticsTelemetryModule)));
        }

        [Theory]
        [InlineData(typeof(ITelemetryInitializer), typeof(ApplicationInsights.AspNetCore.TelemetryInitializers.DomainNameRoleInstanceTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(AzureAppServiceRoleNameFromHostNameHeaderInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(ComponentVersionTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(ClientIpHeaderTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(OperationNameTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(SyntheticTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(WebSessionTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(WebUserTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(HttpDependenciesParsingTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(TelemetryConfiguration), null, ServiceLifetime.Singleton)]
        [InlineData(typeof(TelemetryClient), typeof(TelemetryClient), ServiceLifetime.Singleton)]
        public static void RegistersExpectedServices(Type serviceType, Type implementationType, ServiceLifetime lifecycle)
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, null);
            ServiceDescriptor service = services.Single(s => s.ServiceType == serviceType && s.ImplementationType == implementationType);
            Assert.Equal(lifecycle, service.Lifetime);
        }

        [Theory]
        [InlineData(typeof(ITelemetryInitializer), typeof(ApplicationInsights.AspNetCore.TelemetryInitializers.DomainNameRoleInstanceTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(AzureAppServiceRoleNameFromHostNameHeaderInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(ComponentVersionTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(ClientIpHeaderTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(OperationNameTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(SyntheticTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(WebSessionTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(WebUserTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(HttpDependenciesParsingTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(TelemetryConfiguration), null, ServiceLifetime.Singleton)]
        [InlineData(typeof(TelemetryClient), typeof(TelemetryClient), ServiceLifetime.Singleton)]
        public static void RegistersExpectedServicesOnlyOnce(Type serviceType, Type implementationType, ServiceLifetime lifecycle)
        {
            var services = GetServiceCollectionWithContextAccessor();
            services.AddApplicationInsightsTelemetry();
            services.AddApplicationInsightsTelemetry();
            ServiceDescriptor service = services.Single(s => s.ServiceType == serviceType && s.ImplementationType == implementationType);
            Assert.Equal(lifecycle, service.Lifetime);
        }

        [Fact]
        public static void DoesNotThrowWithoutInstrumentationKey()
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, null);
        }

        [Fact]
        public static void RegistersTelemetryConfigurationFactoryMethodThatCreatesDefaultInstance()
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, null);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            Assert.Contains(telemetryConfiguration.TelemetryInitializers, t => t is OperationNameTelemetryInitializer);
        }

        /// <summary>
        /// Tests that the instrumentation key configuration can be read from a JSON file by the configuration factory.
        /// </summary>
        /// <param name="useDefaultConfig">
        /// Calls services.AddApplicationInsightsTelemetry() when the value is true and reads IConfiguration from user application automatically.
        /// Else, it invokes services.AddApplicationInsightsTelemetry(configuration) where IConfiguration object is supplied by caller.
        /// </param>
        [Theory]
        [InlineData(false)]
        public static void RegistersTelemetryConfigurationFactoryMethodThatReadsInstrumentationKeyFromConfiguration(bool useDefaultConfig)
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(Path.Combine("content", "config-instrumentation-key.json"), null, null, true, useDefaultConfig);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            if (useDefaultConfig)
            {
                Assert.Equal(InstrumentationKeyInAppSettings, telemetryConfiguration.InstrumentationKey);
            }
            else
            {
                Assert.Equal(TestInstrumentationKey, telemetryConfiguration.InstrumentationKey);
            }
        }

        /// <summary>
        /// Tests that the connection string can be read from a JSON file by the configuration factory.
        /// </summary>
        /// <param name="useDefaultConfig">
        /// Calls services.AddApplicationInsightsTelemetry() when the value is true and reads IConfiguration from user application automatically.
        /// Else, it invokes services.AddApplicationInsightsTelemetry(configuration) where IConfiguration object is supplied by caller.
        /// </param>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Trait", "ConnectionString")]
        public static void RegistersTelemetryConfigurationFactoryMethodThatReadsConnectionStringFromConfiguration(bool useDefaultConfig)
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(Path.Combine("content", "config-connection-string.json"), null, null, true, useDefaultConfig);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            Assert.Equal(TestConnectionString, telemetryConfiguration.ConnectionString);
            Assert.Equal(TestInstrumentationKey, telemetryConfiguration.InstrumentationKey);
            Assert.Equal("http://127.0.0.1/", telemetryConfiguration.EndpointContainer.Ingestion.AbsoluteUri);
        }

        /// <summary>
        /// Tests that the connection string can be read from a JSON file by the configuration factory.
        /// This config has both a connection string and an instrumentation key. It is expected to use the ikey from the connection string.
        /// </summary>
        [Fact]
        [Trait("Trait", "ConnectionString")]
        public static void RegistersTelemetryConfigurationFactoryMethodThatReadsConnectionStringAndInstrumentationKeyFromConfiguration()
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(Path.Combine("content", "config-connection-string-and-instrumentation-key.json"), null);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            Assert.Equal(TestConnectionString, telemetryConfiguration.ConnectionString);
            Assert.Equal(TestInstrumentationKey, telemetryConfiguration.InstrumentationKey);
            Assert.Equal("http://127.0.0.1/", telemetryConfiguration.EndpointContainer.Ingestion.AbsoluteUri);
        }

        /// <summary>
        /// Tests that the Active configuration singleton is updated, but another instance of telemetry configuration is created for dependency injection.
        /// ASP.NET Core developers should always use Dependency Injection instead of static singleton approach.
        /// See Microsoft/ApplicationInsights-dotnet#613
        /// </summary>
        [Fact]
        public static void ConfigurationFactoryMethodUpdatesTheActiveConfigurationSingletonByDefault()
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(Path.Combine("content", "config-instrumentation-key.json"), null, null, true, false);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            TelemetryConfiguration telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            Assert.Equal(TestInstrumentationKey, telemetryConfiguration.InstrumentationKey);
        }

        /// <summary>
        /// We determine if Active telemetry needs to be configured based on the assumptions that 'default' configuration
        // created by base SDK has single preset ITelemetryInitializer. If it ever changes, change TelemetryConfigurationOptions.IsActiveConfigured method as well.
        /// </summary>
        [Fact]
        /// <summary>
        /// Tests that the developer mode can be read from a JSON file by the configuration factory.
        /// </summary>
        /// <param name="useDefaultConfig">
        /// Calls services.AddApplicationInsightsTelemetry() when the value is true and reads IConfiguration from user application automatically.
        /// Else, it invokes services.AddApplicationInsightsTelemetry(configuration) where IConfiguration object is supplied by caller.
        /// </param>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]

        /// <summary>
        /// Validates that while using services.AddApplicationInsightsTelemetry(); ikey is read from
        /// Environment
        /// </summary>
        [Fact]
        [Trait("Trait", "ConnectionString")]
        public static void AddApplicationInsightsTelemetry_ReadsConnectionString_FromEnvironment()
        {
            var services = GetServiceCollectionWithContextAccessor();
                Assert.Equal(TestInstrumentationKey, telemetryConfiguration.InstrumentationKey);
            }
            finally
            {
                Environment.SetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", null);
            }
        }

        /// <summary>
        /// Validates that while using services.AddApplicationInsightsTelemetry(ikey), supplied ikey is
                Assert.Equal(ikeyExpected, telemetryConfiguration.InstrumentationKey);
                Assert.Equal(hostExpected, telemetryConfiguration.TelemetryChannel.EndpointAddress);
            }
            finally
            {
                File.WriteAllText("appsettings.json", originalText);
            }
        }

        /// <summary>
                IServiceProvider serviceProvider = services.BuildServiceProvider();
                var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
                Assert.Equal(suppliedIKey, telemetryConfiguration.InstrumentationKey);
            }
            finally
            {
                text = text.Replace(ikey, InstrumentationKeyInAppSettings);
                File.WriteAllText("appsettings.json", text);
            }
        }
        /// </summary>
        [Fact]
        public static void AddApplicationInsightsTelemetryDoesNotReadInstrumentationKeyFromDefaultAppsettingsIfSuppliedViaOptions()
        {
            string suppliedIKey = "suppliedikey";
            var options = new ApplicationInsightsServiceOptions() { InstrumentationKey = suppliedIKey };
            string ikey = Guid.NewGuid().ToString();
            string text = File.ReadAllText("appsettings.json");
            try
            {
        /// </summary>
        [Fact]
        public static void AddApplicationInsightsTelemetryDoesNotOverrideEmptyInstrumentationKeyFromAiOptions()
        {
            // Create new options, which will be default have null ikey and endpoint.
            var options = new ApplicationInsightsServiceOptions();
            string ikey = Guid.NewGuid().ToString();
            string text = File.ReadAllText("appsettings.json");
            try
            {
                text = text.Replace(InstrumentationKeyInAppSettings, ikey);
                text = text.Replace("hosthere", "newhost");
                File.WriteAllText("appsettings.json", text);

                var services = GetServiceCollectionWithContextAccessor();
                services.AddApplicationInsightsTelemetry(options);
                IServiceProvider serviceProvider = services.BuildServiceProvider();
                var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
                Assert.Equal(ikey, telemetryConfiguration.InstrumentationKey);
                Assert.Equal("http://newhost/v2/track/", telemetryConfiguration.DefaultTelemetrySink.TelemetryChannel.EndpointAddress);
            }
            finally
            {
                text = text.Replace(ikey, InstrumentationKeyInAppSettings);
                text = text.Replace("newhost", "hosthere");
                File.WriteAllText("appsettings.json", text);
            }
        }

        [Fact]
                EnableDebugLogger = true,
                EnableQuickPulseMetricStream = false,
                EndpointAddress = "http://test",
                EnableHeartbeat = false,
            services.AddApplicationInsightsTelemetry(options);
            ApplicationInsightsServiceOptions servicesOptions = null;
            services.Configure((ApplicationInsightsServiceOptions o) =>
            {
                servicesOptions = o;
            });

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            TelemetryConfiguration telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

            PropertyInfo[] properties = optionsType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Assert.True(properties.Length > 0);
            foreach (PropertyInfo property in properties)
            {
                Assert.Equal(property.GetValue(options).ToString(), property.GetValue(servicesOptions).ToString());
            }
        }


        [Fact]
            //      QuickPulseTelemetryModule, DiagnosticsTelemetryModule, DependencyTrackingTelemetryModule, EventCollectorCollectionModule
            Assert.Equal(8, modules.Count());
#else
            Assert.Equal(7, modules.Count());
#endif

            var perfCounterModule = modules.OfType<PerformanceCollectorModule>().Single();
            Assert.NotNull(perfCounterModule);

#if NETCOREAPP
            Assert.NotNull(azureMetadataHeartBeatModuleDescriptor);

            var quickPulseModuleDescriptor = modules.OfType<QuickPulseTelemetryModule>().Single();
            Assert.NotNull(quickPulseModuleDescriptor);
        }

#if NETCOREAPP
        [Fact]
        public static void RegistersTelemetryConfigurationFactoryMethodThatPopulatesEventCounterCollectorWithDefaultListOfCounters()
        {
            //ARRANGE
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, null, null, false);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var modules = serviceProvider.GetServices<ITelemetryModule>();

            //ACT

            // Requesting TelemetryConfiguration from services trigger constructing the TelemetryConfiguration
            // which in turn trigger configuration of all modules.
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
#endif


        [Fact]
        public static void RegistersTelemetryConfigurationFactoryMethodThatPopulatesDependencyCollectorWithDefaultValues()
        {
            //ARRANGE
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, null, null, false);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var modules = serviceProvider.GetServices<ITelemetryModule>();
            //ACT

            // Requesting TelemetryConfiguration from services trigger constructing the TelemetryConfiguration
            // which in turn trigger configuration of all modules.
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            var dependencyModule = modules.OfType<DependencyTrackingTelemetryModule>().Single();

            //VALIDATE
            Assert.Equal(4, dependencyModule.ExcludeComponentCorrelationHttpHeadersOnDomains.Count);
            Assert.False(dependencyModule.ExcludeComponentCorrelationHttpHeadersOnDomains.Contains("localhost"));
            Assert.False(dependencyModule.ExcludeComponentCorrelationHttpHeadersOnDomains.Contains("127.0.0.1"));
        }

        /// <summary>
        /// User could enable or disable LegacyCorrelationHeadersInjection of DependencyCollectorOptions.
        /// This configuration can be read from a JSON file by the configuration factory or through code by passing ApplicationInsightsServiceOptions.
        /// </summary>
        /// <param name="configType">
        /// DefaultConfiguration - calls services.AddApplicationInsightsTelemetry() which reads IConfiguration from user application automatically.
        /// SuppliedConfiguration - invokes services.AddApplicationInsightsTelemetry(configuration) where IConfiguration object is supplied by caller.
        /// Code - Caller creates an instance of ApplicationInsightsServiceOptions and passes it. This option overrides all configuration being used in JSON file.
        /// There is a special case where NULL values in these properties - InstrumentationKey, ConnectionString, EndpointAddress and DeveloperMode are overwritten. We check IConfiguration object to see if these properties have values, if values are present then we override it.
        /// </param>
        /// <param name="isEnable">Sets the value for property EnableLegacyCorrelationHeadersInjection.</param>
        [Theory]
        [InlineData("DefaultConfiguration", true)]
        [InlineData("DefaultConfiguration", false)]
        [InlineData("SuppliedConfiguration", true)]
        [InlineData("SuppliedConfiguration", false)]
        [InlineData("Code", true)]
            // Get telemetry client to trigger TelemetryConfig setup.
            var tc = serviceProvider.GetService<TelemetryClient>();

            // VALIDATE
            Assert.Equal(isEnable ? 6 : 4, dependencyModule.ExcludeComponentCorrelationHttpHeadersOnDomains.Count);
            Assert.Equal(isEnable, dependencyModule.ExcludeComponentCorrelationHttpHeadersOnDomains.Contains("localhost") ? true : false);
            Assert.Equal(isEnable, dependencyModule.ExcludeComponentCorrelationHttpHeadersOnDomains.Contains("127.0.0.1") ? true : false);
        }

        [Fact]
        public static void RegistersTelemetryConfigurationFactoryMethodThatPopulatesItWithTelemetryProcessorFactoriesFromContainer()
        {
            var services = GetServiceCollectionWithContextAccessor();
            services.AddApplicationInsightsTelemetryProcessor<FakeTelemetryProcessor>();

            services.AddApplicationInsightsTelemetry(new ConfigurationBuilder().Build());

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            // TP added via AddApplicationInsightsTelemetryProcessor is added to the default sink.
            var services = GetServiceCollectionWithContextAccessor();
            Assert.Throws<ArgumentException>(() => services.AddApplicationInsightsTelemetryProcessor(typeof(string)));
            Assert.Throws<ArgumentException>(() => services.AddApplicationInsightsTelemetryProcessor(typeof(ITelemetryProcessor)));
        }

        [Fact]
        public static void AddApplicationInsightsTelemetryProcessorWithImportingConstructor()
        {
            var services = GetServiceCollectionWithContextAccessor();
            services.AddApplicationInsightsTelemetryProcessor<FakeTelemetryProcessorWithImportingConstructor>();
            services.AddApplicationInsightsTelemetry(new ConfigurationBuilder().Build());
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

            // TP added via AddApplicationInsightsTelemetryProcessor is added to the default sink.
            FakeTelemetryProcessorWithImportingConstructor telemetryProcessor = telemetryConfiguration.DefaultTelemetrySink.TelemetryProcessors.OfType<FakeTelemetryProcessorWithImportingConstructor>().FirstOrDefault();
            Assert.NotNull(telemetryProcessor);
            Assert.Same(serviceProvider.GetService<IHostingEnvironment>(), telemetryProcessor.HostingEnvironment);
        }

        {
            //ARRANGE
            var services = GetServiceCollectionWithContextAccessor();
            services.AddSingleton<ITelemetryModule, TestTelemetryModule>();

            //ACT
            services.ConfigureTelemetryModule<TestTelemetryModule>
            ((module, o) => module.CustomProperty = "mycustomvalue");
            services.AddApplicationInsightsTelemetry(new ConfigurationBuilder().Build());
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            Assert.NotNull(testTelemetryModule);
            Assert.Equal("mycustomvalue", testTelemetryModule.CustomProperty);
            Assert.True(testTelemetryModule.IsInitialized);
        }

        [Fact]
        /// <summary>
        /// We've added the DiagnosticsTelemetryModule to the default TelemetryModules in AspNetCore DI.
        /// During setup, we expect this module to be discovered and set on the other Heartbeat TelemetryModules.
        /// </summary>
        public static void VerifyIfHeartbeatPropertyManagerSetOnOtherModules_Default()
        {
            //ARRANGE
            var services = GetServiceCollectionWithContextAccessor();

            //ACT
            services.AddApplicationInsightsTelemetry(new ConfigurationBuilder().Build());
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();


            //VALIDATE
            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var count = modules.OfType<DiagnosticsTelemetryModule>().Count();
            Assert.Equal(1, count);

            var appServicesHeartbeatTelemetryModule = modules.OfType<AppServicesHeartbeatTelemetryModule>().Single();
            var hpm1 = appServicesHeartbeatTelemetryModule.HeartbeatPropertyManager;
            Assert.NotNull(hpm1);
            Assert.Same(diagnosticsTelemetryModule, hpm1);
        /// During setup, we expect this module to be discovered and set on the other Heartbeat TelemetryModules.
        /// </summary>
        public static void VerifyIfHeartbeatPropertyManagerSetOnOtherModules_UserDefinedType()
        {
            //ARRANGE
            var services = GetServiceCollectionWithContextAccessor();

            // VERIFY THAT A USER CAN SPECIFY THEIR OWN TYPE
            services.AddSingleton<ITelemetryModule, DiagnosticsTelemetryModule>();

            //act
            services.AddApplicationInsightsTelemetry(new ConfigurationBuilder().Build());
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

            //VALIDATE
            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var count = modules.OfType<DiagnosticsTelemetryModule>().Count();
            Assert.Equal(1, count);

            Assert.NotNull(hpm1);

            var azureInstanceMetadataTelemetryModule = modules.OfType<AzureInstanceMetadataTelemetryModule>().Single();
            var hpm2 = azureInstanceMetadataTelemetryModule.HeartbeatPropertyManager;
            Assert.NotNull(hpm2);

            Assert.Same(hpm1, hpm2);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        /// <summary>
        /// Previously we encouraged users to add the DiagnosticsTelemetryModule manually.
        /// Users could have added this either as an INSTANCE or as a TYPE.
        /// We don't want to add it a second time so need to confirm that we catch both cases.
        /// </summary>
        public static void TestingAddDiagnosticsTelemetryModule(bool manualAddInstance, bool manualAddType)
        {
                services.AddSingleton<ITelemetryModule>(new DiagnosticsTelemetryModule());
            }
            else if (manualAddType)
            {
                services.AddSingleton<ITelemetryModule, DiagnosticsTelemetryModule>();
            }

            //ACT
            services.AddApplicationInsightsTelemetry(new ConfigurationBuilder().Build());
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            //VALIDATE
            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var count = modules.OfType<DiagnosticsTelemetryModule>().Count();

            Assert.Equal(1, count);
        }


        [Fact]

            //ACT
            services.ConfigureTelemetryModule<TestTelemetryModule>
                ((module, o) => module.CustomProperty = o.ApplicationVersion);
            services.AddApplicationInsightsTelemetry(serviceOptions);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Requesting TelemetryConfiguration from services trigger constructing the TelemetryConfiguration
            // which in turn trigger configuration of all modules.
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

            //VALIDATE
            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var testTelemetryModule = modules.OfType<TestTelemetryModule>().Single();

            //The module should be initialized and configured as instructed.
            Assert.NotNull(testTelemetryModule);
            Assert.Equal("123", testTelemetryModule.CustomProperty);
            Assert.True(testTelemetryModule.IsInitialized);
        }

        [Fact]
        public static void ConfigureApplicationInsightsTelemetryModuleWorksWithoutOptions()
        {
            //ARRANGE
            var services = GetServiceCollectionWithContextAccessor();
            services.AddSingleton<ITelemetryModule, TestTelemetryModule>();

            //ACT
            services.ConfigureTelemetryModule<TestTelemetryModule>
                ((module, o) => module.CustomProperty = "mycustomproperty");

            services.AddApplicationInsightsTelemetry();
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Requesting TelemetryConfiguration from services trigger constructing the TelemetryConfiguration
            // which in turn trigger configuration of all modules.
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

            //VALIDATE
            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var testTelemetryModule = modules.OfType<TestTelemetryModule>().Single();

            //The module should be initialized and configured as instructed.
            Assert.NotNull(testTelemetryModule);
            Assert.Equal("mycustomproperty", testTelemetryModule.CustomProperty);
            Assert.True(testTelemetryModule.IsInitialized);
        }

        [Fact]
        {
            //ARRANGE
            var services = GetServiceCollectionWithContextAccessor();

            //ACT
            services.AddApplicationInsightsTelemetry();
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

            //VALIDATE
        /// DefaultConfiguration - calls services.AddApplicationInsightsTelemetry() which reads IConfiguration from user application automatically.
        /// SuppliedConfiguration - invokes services.AddApplicationInsightsTelemetry(configuration) where IConfiguration object is supplied by caller.
        /// Code - Caller creates an instance of ApplicationInsightsServiceOptions and passes it. This option overrides all configuration being used in JSON file.
        /// There is a special case where NULL values in these properties - InstrumentationKey, ConnectionString, EndpointAddress and DeveloperMode are overwritten. We check IConfiguration object to see if these properties have values, if values are present then we override it.
        /// </param>
        /// <param name="isEnable">Sets the value for property InjectResponseHeaders, TrackExceptions and EnableW3CDistributedTracing.</param>
        [Theory]
        [InlineData("DefaultConfiguration", true)]
        [InlineData("DefaultConfiguration", false)]
        [InlineData("SuppliedConfiguration", true)]

            if (configType == "Code")
            {
                serviceOptions = o =>
                {
                    o.RequestCollectionOptions.InjectResponseHeaders = isEnable;
                    o.RequestCollectionOptions.TrackExceptions = isEnable;
                    // o.RequestCollectionOptions.EnableW3CDistributedTracing = isEnable; // Obsolete
                };
                filePath = null;

            // ACT
            var services = CreateServicesAndAddApplicationinsightsTelemetry(filePath, null, serviceOptions, true, configType == "DefaultConfiguration" ? true : false);

            // VALIDATE
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

            var requestTrackingModule = (RequestTrackingTelemetryModule)serviceProvider
                .GetServices<ITelemetryModule>().FirstOrDefault(x => x.GetType() == typeof(RequestTrackingTelemetryModule));
            Assert.Throws<ArgumentNullException>(() => services.ConfigureTelemetryModule<TestTelemetryModule>((Action<TestTelemetryModule>)null));
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [Fact]
        public static void ConfigureApplicationInsightsTelemetryModuleDoesNotThrowIfModuleNotFound()
        {
            //ARRANGE
            var services = GetServiceCollectionWithContextAccessor();
            // Intentionally NOT adding the module
            // No exceptions thrown here.
            Assert.Null(testTelemetryModule);
        }

        [Fact]
        public static void AddsAddaptiveSamplingServiceToTheConfigurationByDefault()
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, "http://localhost:1234/v2/track/", null, false);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
        /// <summary>
        /// User could enable or disable sampling by setting EnableAdaptiveSampling.
        /// This configuration can be read from a JSON file by the configuration factory or through code by passing ApplicationInsightsServiceOptions.
        /// </summary>
        /// <param name="configType">
        /// DefaultConfiguration - calls services.AddApplicationInsightsTelemetry() which reads IConfiguration from user application automatically.
        /// SuppliedConfiguration - invokes services.AddApplicationInsightsTelemetry(configuration) where IConfiguration object is supplied by caller.
        /// Code - Caller creates an instance of ApplicationInsightsServiceOptions and passes it. This option overrides all configuration being used in JSON file.
        /// There is a special case where NULL values in these properties - InstrumentationKey, ConnectionString, EndpointAddress and DeveloperMode are overwritten. We check IConfiguration object to see if these properties have values, if values are present then we override it.
        /// </param>
        /// <param name="isEnable">Sets the value for property EnableAdaptiveSampling.</param>
        [Theory]
        [InlineData("DefaultConfiguration", true)]
        [InlineData("DefaultConfiguration", false)]
        [InlineData("SuppliedConfiguration", true)]
        [InlineData("SuppliedConfiguration", false)]
        [InlineData("Code", true)]
        [InlineData("Code", false)]
        public static void DoesNotAddSamplingToConfigurationIfExplicitlyControlledThroughParameter(string configType, bool isEnable)
        {
                serviceOptions = o => { o.EnableAdaptiveSampling = isEnable; };
                filePath = null;
            }

            // ACT
            var services = CreateServicesAndAddApplicationinsightsTelemetry(filePath, null, serviceOptions, true, configType == "DefaultConfiguration" ? true : false);

            // VALIDATE
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

        [Fact]
        public static void AddsAddaptiveSamplingServiceToTheConfigurationWithServiceOptions()
        {
            Action<ApplicationInsightsServiceOptions> serviceOptions = options => options.EnableAdaptiveSampling = true;
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, "http://localhost:1234/v2/track/", serviceOptions, false);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            var adaptiveSamplingProcessorCount = GetTelemetryProcessorsCountInConfigurationDefaultSink<AdaptiveSamplingTelemetryProcessor>(telemetryConfiguration);
            // There will be 2 separate SamplingTelemetryProcessors - one for Events, and other for everything else.
        }

        [Fact]
        public static void AddsServerTelemetryChannelByDefault()
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, "http://localhost:1234/v2/track/", null, false);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            Assert.Equal(typeof(ServerTelemetryChannel), telemetryConfiguration.TelemetryChannel.GetType());
        }

        [Fact]
        [Trait("Trait", "ConnectionString")]
        public static void AddApplicationInsightsSettings_SetsConnectionString()
        {
            var services = GetServiceCollectionWithContextAccessor();
            services.AddSingleton<ITelemetryChannel>(new InMemoryChannel());
            var config = new ConfigurationBuilder().AddApplicationInsightsSettings(connectionString: TestConnectionString).Build();
            services.AddApplicationInsightsTelemetry(config);

            Assert.Equal(TestConnectionString, telemetryConfiguration.ConnectionString);
            Assert.Equal(TestInstrumentationKey, telemetryConfiguration.InstrumentationKey);
            Assert.Equal("http://127.0.0.1/", telemetryConfiguration.EndpointContainer.Ingestion.AbsoluteUri);
        }

        [Fact]
        [Trait("Trait", "Endpoints")]
        public static void DoesNotOverWriteExistingChannel()
        {
            var testEndpoint = "http://localhost:1234/v2/track/";
            Assert.Equal(testEndpoint, telemetryConfiguration.TelemetryChannel.EndpointAddress);
        }

        [Fact]
        public static void FallbacktoDefaultChannelWhenNoChannelFoundInDI()
        {
            var testEndpoint = "http://localhost:1234/v2/track/";

            // ARRANGE
            var services = GetServiceCollectionWithContextAccessor();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            Assert.Equal(typeof(InMemoryChannel), telemetryConfiguration.TelemetryChannel.GetType());
            Assert.Equal(testEndpoint, telemetryConfiguration.TelemetryChannel.EndpointAddress);
        }

        [Fact]
        public static void VerifyNoExceptionWhenAppIdProviderNotFoundInDI()
        {
            // ARRANGE
            var services = GetServiceCollectionWithContextAccessor();
                if (descriptor.ServiceType == typeof(IApplicationIdProvider))
                {
                    services.RemoveAt(i);
                }
            }

            // ACT
            IServiceProvider serviceProvider = services.BuildServiceProvider();


            // VERIFY
            var requestTrackingModule = serviceProvider.GetServices<ITelemetryModule>().FirstOrDefault(x => x.GetType()
                == typeof(RequestTrackingTelemetryModule));

            Assert.NotNull(requestTrackingModule); // this verifies the instance was created without exception
        }

        [Fact]
        public static void VerifyUserCanOverrideAppIdProvider()
        {
            // none on the main pipeline except the PassThrough.

            var qpProcessorCount = GetTelemetryProcessorsCountInConfiguration<QuickPulseTelemetryProcessor>(telemetryConfiguration);
            Assert.Equal(0, qpProcessorCount);

            var metricExtractorProcessorCount = GetTelemetryProcessorsCountInConfiguration<AutocollectedMetricsExtractor>(telemetryConfiguration);
            Assert.Equal(0, metricExtractorProcessorCount);

            var samplingProcessorCount = GetTelemetryProcessorsCountInConfiguration<AdaptiveSamplingTelemetryProcessor>(telemetryConfiguration);
            Assert.Equal(0, samplingProcessorCount);
            Assert.Equal("PassThroughProcessor", telemetryConfiguration.TelemetryProcessors[0].GetType().Name);
        }

        [Fact]
        public static void AddsQuickPulseProcessorToTheConfigurationByDefault()
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, "http://localhost:1234/v2/track/", null, false);
            services.AddSingleton<ITelemetryChannel, InMemoryChannel>();
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            services.AddSingleton<ITelemetryChannel, InMemoryChannel>();
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            var metricExtractorProcessorCount = GetTelemetryProcessorsCountInConfigurationDefaultSink<AutocollectedMetricsExtractor>(telemetryConfiguration);
            Assert.Equal(1, metricExtractorProcessorCount);
        }

        /// <summary>
        /// User could enable or disable auto collected metrics by setting AddAutoCollectedMetricExtractor.
        /// This configuration can be read from a JSON file by the configuration factory or through code by passing ApplicationInsightsServiceOptions.
        public static void DoesNotAddAutoCollectedMetricsExtractorToConfigurationIfExplicitlyControlledThroughParameter(string configType, bool isEnable)
        {
            // ARRANGE
            Action<ApplicationInsightsServiceOptions> serviceOptions = null;
            var filePath = Path.Combine("content", "config-all-settings-" + isEnable.ToString().ToLower() + ".json");

            if (configType == "Code")
            {
                serviceOptions = o => { o.AddAutoCollectedMetricExtractor = isEnable; };
                filePath = null;
            }

            // ACT
            var services = CreateServicesAndAddApplicationinsightsTelemetry(filePath, null, serviceOptions, true, configType == "DefaultConfiguration" ? true : false);

            // VALIDATE
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            var metricExtractorProcessorCount = GetTelemetryProcessorsCountInConfigurationDefaultSink<AutocollectedMetricsExtractor>(telemetryConfiguration);
            Assert.Equal(isEnable ? 1 : 0, metricExtractorProcessorCount);
        [Theory]
        [InlineData("DefaultConfiguration", true)]
        [InlineData("DefaultConfiguration", false)]
        [InlineData("SuppliedConfiguration", true)]
        [InlineData("SuppliedConfiguration", false)]
        [InlineData("Code", true)]
        [InlineData("Code", false)]
        public static void UserCanEnableAndDisableAuthenticationTrackingJavaScript(string configType, bool isEnable)
        {
            // ARRANGE

            // VALIDATE
            // Get telemetry client to trigger TelemetryConfig setup.
            var tc = serviceProvider.GetService<TelemetryClient>();

            Type javaScriptSnippetType = typeof(JavaScriptSnippet);
            var javaScriptSnippet = serviceProvider.GetService<JavaScriptSnippet>();
            // Get the JavaScriptSnippet private field value for enableAuthSnippet.
            FieldInfo enableAuthSnippetField = javaScriptSnippetType.GetField("enableAuthSnippet", BindingFlags.NonPublic | BindingFlags.Instance);
            // JavaScriptSnippet.enableAuthSnippet is set to true when EnableAuthenticationTrackingJavaScript is enabled, else it is set to false.
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, "http://localhost:1234/v2/track/", null, false);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            var modules = serviceProvider.GetServices<ITelemetryModule>();
            Assert.NotNull(modules.OfType<AppServicesHeartbeatTelemetryModule>().Single());
            Assert.NotNull(modules.OfType<AzureInstanceMetadataTelemetryModule>().Single());
        }

        [Fact]
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, "http://localhost:1234/v2/track/");
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

#pragma warning disable CS0618 // Type or member is obsolete
            Assert.DoesNotContain(telemetryConfiguration.TelemetryInitializers, t => t is W3COperationCorrelationTelemetryInitializer);
#pragma warning restore CS0618 // Type or member is obsolete

            var modules = serviceProvider.GetServices<ITelemetryModule>().ToList();

        private static int GetTelemetryProcessorsCountInConfiguration<T>(TelemetryConfiguration telemetryConfiguration)
        {
            return telemetryConfiguration.TelemetryProcessors.Where(processor => processor.GetType() == typeof(T)).Count();
        }

        private static int GetTelemetryProcessorsCountInConfigurationDefaultSink<T>(TelemetryConfiguration telemetryConfiguration)
        {
            return telemetryConfiguration.DefaultTelemetrySink.TelemetryProcessors.Where(processor => processor.GetType() == typeof(T)).Count();
        }

#pragma warning disable CS0618 // Type or member is obsolete
            loggerProvider.AddApplicationInsights(serviceProvider, (s, level) => true, () => firstLoggerCallback = true);
            loggerProvider.AddApplicationInsights(serviceProvider, (s, level) => true, () => secondLoggerCallback = true);
#pragma warning restore CS0618 // Type or member is obsolete

            Assert.True(firstLoggerCallback);
            Assert.False(secondLoggerCallback);
        }

        /// Compares ApplicationInsightsServiceOptions object from dependency container and one created directly from configuration.
        /// This proves all that SDK read configuration successfully from configuration file.
        /// Properties from appSettings.json, appsettings.{env.EnvironmentName}.json and Environmental Variables are read if no IConfiguration is supplied or used in an application.
        /// </summary>
        /// <param name="readFromAppSettings">If this is set, read value from appsettings.json, else from passed file.</param>
        /// <param name="useDefaultConfig">
        /// Calls services.AddApplicationInsightsTelemetry() when the value is true and reads IConfiguration from user application automatically.
        /// Else, it invokes services.AddApplicationInsightsTelemetry(configuration) where IConfiguration object is supplied by caller.
        /// </param>
        [Theory]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public static void ReadsSettingsFromDefaultAndSuppliedConfiguration(bool readFromAppSettings, bool useDefaultConfig)
        {
            // ARRANGE
            IConfigurationBuilder configBuilder = null;
            var fileName = "config-all-default.json";

            // ACT
            var services = CreateServicesAndAddApplicationinsightsTelemetry(
            // VALIDATE

            // Generate config and don't pass to services
            // this is directly generated from config file
            // which could be used to validate the data from dependency container

            if (!readFromAppSettings)
            {
                configBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
            else
            {
                configBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false);
            }

            var config = configBuilder.Build();

            // Compare ApplicationInsightsServiceOptions from dependency container and configuration
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            // ApplicationInsightsServiceOptions from dependency container
            var servicesOptions = serviceProvider.GetRequiredService<IOptions<ApplicationInsightsServiceOptions>>().Value;

            // Create ApplicationInsightsServiceOptions from configuration for validation.
            var aiOptions = new ApplicationInsightsServiceOptions();
            config.GetSection("ApplicationInsights").Bind(aiOptions);
            config.GetSection("ApplicationInsights:TelemetryChannel").Bind(aiOptions);

            Type optionsType = typeof(ApplicationInsightsServiceOptions);
                var telemetryConfiguration = serviceProvider.GetRequiredService<TelemetryConfiguration>();
                Assert.Equal(TestInstrumentationKey, telemetryConfiguration.InstrumentationKey);
                Assert.Equal(TestConnectionString, telemetryConfiguration.ConnectionString);
                Assert.Equal(TestEndPoint, telemetryConfiguration.TelemetryChannel.EndpointAddress);
                Assert.True(telemetryConfiguration.TelemetryChannel.DeveloperMode);
            }
            finally
            {
                Environment.SetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", null);
                Environment.SetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING", null);
                Environment.SetEnvironmentVariable("APPINSIGHTS_ENDPOINTADDRESS", null);
                Environment.SetEnvironmentVariable("APPINSIGHTS_DEVELOPER_MODE", null);
            }
        }

        [Fact]
        public static void VerifiesIkeyProvidedInAddApplicationInsightsAlwaysWinsOverOtherOptions()
        {
            // ARRANGE
            Environment.SetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", TestInstrumentationKey);
            {
                var jsonFullPath = Path.Combine(Directory.GetCurrentDirectory(), "content", "config-instrumentation-key.json");

                // This config will have ikey,endpoint from json and env. But the one
                // user explicitly provider is expected to win.
                var config = new ConfigurationBuilder().AddJsonFile(jsonFullPath).AddEnvironmentVariables().Build();
                var services = GetServiceCollectionWithContextAccessor();

                // This line mimics the default behavior by CreateDefaultBuilder
                services.AddSingleton<IConfiguration>(config);
                var telemetryConfiguration = serviceProvider.GetRequiredService<TelemetryConfiguration>();
                Assert.Equal("userkey", telemetryConfiguration.InstrumentationKey);
            }
            finally
            {
                Environment.SetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", null);
            }
        }

        [Fact]
            // ACT
            // Calls services.AddApplicationInsightsTelemetry(), which by default reads from appSettings.json
            var services = CreateServicesAndAddApplicationinsightsTelemetry(filePath, null, null, true, true);

            // VALIDATE
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetRequiredService<TelemetryConfiguration>();
            Assert.Equal(InstrumentationKeyInAppSettings, telemetryConfiguration.InstrumentationKey);
        }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
