using Microsoft.ApplicationInsights.WorkerService.TelemetryInitializers;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.EventCounterCollector;
using Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.ApplicationInsights.WindowsServer;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using System.Reflection;
using Microsoft.Extensions.Options;
using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

namespace Microsoft.ApplicationInsights.WorkerService.Tests
{
    public class ExtensionsTests
    {
        private readonly ITestOutputHelper output;
        private const string TestConnectionString = "InstrumentationKey=11111111-2222-3333-4444-555555555555;IngestionEndpoint=http://127.0.0.1";
        public const string TestInstrumentationKey = "11111111-2222-3333-4444-555555555555";
        public const string TestInstrumentationKeyEnv = "AAAAAAAA-BBBB-CCCC-DDDD-DDDDDDDDDD";
        public const string TestEndPoint = "http://testendpoint/v2/track";
        public const string TestEndPointEnv = "http://testendpointend/v2/track";
        public ExtensionsTests(ITestOutputHelper output)
        {
            this.output = output;
            this.output.WriteLine("Initialized");
        }

        private static IServiceProvider TestShim(string configType, bool isEnabled, Action<ApplicationInsightsServiceOptions, bool> testConfig)
        {
            // ARRANGE
            Action<ApplicationInsightsServiceOptions> serviceOptions = null;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "content", "config-all-settings-" + isEnabled.ToString().ToLower() + ".json");

            if (configType == "Code")
            {
                filePath = null;

                // This will set the property defined in the test.
                serviceOptions = o => { testConfig(o, isEnabled); };
            }

            // ACT
            var services = CreateServicesAndAddApplicationinsightsWorker(
                jsonPath: filePath,
                serviceOptions: serviceOptions,
                useDefaultConfig: configType == "DefaultConfiguration" ? true : false);

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Get telemetry client to trigger TelemetryConfig setup.
            var tc = serviceProvider.GetService<TelemetryClient>();

            // Verify that Modules were added to DI.
            var modules = serviceProvider.GetServices<ITelemetryModule>();
            Assert.NotNull(modules);

            return serviceProvider;
        }

        public static ServiceCollection CreateServicesAndAddApplicationinsightsWorker(string jsonPath, Action<ApplicationInsightsServiceOptions> serviceOptions = null, bool useDefaultConfig = true)
        {
            IConfigurationRoot config;
            var services = new ServiceCollection();

            if (jsonPath != null)
            {
                var jsonFullPath = Path.Combine(Directory.GetCurrentDirectory(), jsonPath);
                Console.WriteLine("json:" + jsonFullPath);
                try
                {
                    config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(jsonFullPath).Build();
                }
                catch (Exception)
                {
                    throw new Exception("Unable to build with json:" + jsonFullPath);
                }
            }
            else
            {
                var configBuilder = new ConfigurationBuilder()
                    .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), true)
                    .AddEnvironmentVariables();
                config = configBuilder.Build();
            }

            if (useDefaultConfig)
            {
                services.AddSingleton<IConfiguration>(config);
                services.AddApplicationInsightsTelemetryWorkerService();
            }
            else
            {
                services.AddApplicationInsightsTelemetryWorkerService(config);
            }

            if (serviceOptions != null)
            {
                services.Configure(serviceOptions);
            }

            return services;
        }

            services.AddSingleton<ITelemetryModule, TestTelemetryModule>();
            if (manuallyRegisterDiagnosticsTelemetryModule)
            {
                services.AddSingleton<ITelemetryModule, DiagnosticsTelemetryModule>();
            }

            services.AddKeyedSingleton(typeof(ITestService), serviceKey: new(), implementationType: typeof(TestService));
            services.AddKeyedSingleton(typeof(ITestService), serviceKey: new(), implementationInstance: new TestService());
            services.AddKeyedSingleton(typeof(ITestService), serviceKey: new(), implementationFactory: (sp, key) => new TestService());


            var telemetryModules = sp.GetServices<ITelemetryModule>();

            Assert.Equal(8, telemetryModules.Count());

            Assert.Single(telemetryModules.Where(m => m.GetType() == typeof(DiagnosticsTelemetryModule)));
        }

        [Theory]
        [InlineData(typeof(ITelemetryInitializer), typeof(ApplicationInsights.WorkerService.TelemetryInitializers.DomainNameRoleInstanceTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(HttpDependenciesParsingTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(TelemetryConfiguration), null, ServiceLifetime.Singleton)]
        [InlineData(typeof(TelemetryClient), typeof(TelemetryClient), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryChannel), typeof(ServerTelemetryChannel), ServiceLifetime.Singleton)]
        public void RegistersExpectedServicesOnlyOnce(Type serviceType, Type implementationType, ServiceLifetime lifecycle)
        {
            var services = CreateServicesAndAddApplicationinsightsWorker(null);
            services.AddApplicationInsightsTelemetryWorkerService();
            ServiceDescriptor service = services.Single(s => s.ServiceType == serviceType && s.ImplementationType == implementationType);
            Assert.Equal(lifecycle, service.Lifetime);
        }

        [Fact]
        public void DoesNotThrowWithoutInstrumentationKey()
        {
            var services = CreateServicesAndAddApplicationinsightsWorker(null);
        }

        [Fact]
        public void ReadsSettingsFromSuppliedConfiguration()
            var services = new ServiceCollection();

            services.AddApplicationInsightsTelemetryWorkerService(config);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetRequiredService<TelemetryConfiguration>();
            Assert.Equal(TestInstrumentationKey, telemetryConfiguration.InstrumentationKey);
            Assert.Equal(TestEndPoint, telemetryConfiguration.TelemetryChannel.EndpointAddress);
            Assert.Equal(true, telemetryConfiguration.TelemetryChannel.DeveloperMode);
        }

            var services = new ServiceCollection();
            // This line mimics the default behavior by CreateDefaultBuilder
            services.AddSingleton<IConfiguration>(config);

            // ACT
            services.AddApplicationInsightsTelemetryWorkerService();

            // VALIDATE
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetRequiredService<TelemetryConfiguration>();
            // this test validates that SDK reads settings from this configuration by default
            // and gives priority to the ENV variables than the one from config.

            // ARRANGE
            Environment.SetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", TestInstrumentationKeyEnv);
            Environment.SetEnvironmentVariable("APPINSIGHTS_ENDPOINTADDRESS", TestEndPointEnv);
            try
            {
                var jsonFullPath = Path.Combine(Directory.GetCurrentDirectory(), "content", "sample-appsettings.json");
                this.output.WriteLine("json:" + jsonFullPath);

                // VALIDATE
                IServiceProvider serviceProvider = services.BuildServiceProvider();
                var telemetryConfiguration = serviceProvider.GetRequiredService<TelemetryConfiguration>();
                Assert.Equal(TestInstrumentationKeyEnv, telemetryConfiguration.InstrumentationKey);
                Assert.Equal(TestEndPointEnv, telemetryConfiguration.TelemetryChannel.EndpointAddress);
            }
            finally
            {
                Environment.SetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", null);
            Environment.SetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", TestInstrumentationKeyEnv);
            try
            {
                var jsonFullPath = Path.Combine(Directory.GetCurrentDirectory(), "content", "sample-appsettings.json");
                this.output.WriteLine("json:" + jsonFullPath);

                // This config will have ikey,endpoint from json and env. But the one
                // user explicitly provider is expected to win.
                var config = new ConfigurationBuilder().AddJsonFile(jsonFullPath).AddEnvironmentVariables().Build();
                var services = new ServiceCollection();

                // This line mimics the default behavior by CreateDefaultBuilder
                services.AddSingleton<IConfiguration>(config);

                // ACT
                services.AddApplicationInsightsTelemetryWorkerService("userkey");

                // VALIDATE
                IServiceProvider serviceProvider = services.BuildServiceProvider();
                var telemetryConfiguration = serviceProvider.GetRequiredService<TelemetryConfiguration>();
        public void DoesNoThrowIfNoSettingsFound()
        {
            // Host.CreateDefaultBuilder() in .NET Core 3.0  adds appsetting.json and env variable
            // to configuration and is made available for constructor injection.
            // This test validates that SDK does not throw any error if it cannot find
            // application insights configuration in default IConfiguration.
            // ARRANGE
            var jsonFullPath = Path.Combine(Directory.GetCurrentDirectory(), "content", "sample-appsettings_dontexist.json");
            this.output.WriteLine("json:" + jsonFullPath);
            var config = new ConfigurationBuilder().AddJsonFile(jsonFullPath, true).Build();
            var appIdProvider = serviceProvider.GetRequiredService<IApplicationIdProvider>();
            Assert.NotNull(appIdProvider);
            Assert.True(appIdProvider is ApplicationInsightsApplicationIdProvider);

            // AppID
            var channel = serviceProvider.GetRequiredService<ITelemetryChannel>();
            Assert.NotNull(channel);
            Assert.True(channel is ServerTelemetryChannel);

            // TelemetryModules
            Assert.Equal(7, modules.Count());

            var perfCounterModule = modules.FirstOrDefault<ITelemetryModule>(t => t.GetType() == typeof(PerformanceCollectorModule));
            Assert.NotNull(perfCounterModule);

            var qpModule = modules.FirstOrDefault<ITelemetryModule>(t => t.GetType() == typeof(QuickPulseTelemetryModule));
            Assert.NotNull(qpModule);

            var evtCounterModule = modules.FirstOrDefault<ITelemetryModule>(t => t.GetType() == typeof(EventCounterCollectionModule));
            Assert.NotNull(evtCounterModule);
            Assert.Contains(telemetryConfiguration.DefaultTelemetrySink.TelemetryProcessors, proc => proc.GetType().Name.Contains("AutocollectedMetricsExtractor"));
            Assert.Contains(telemetryConfiguration.DefaultTelemetrySink.TelemetryProcessors, proc => proc.GetType().Name.Contains("AdaptiveSamplingTelemetryProcessor"));
            Assert.Contains(telemetryConfiguration.DefaultTelemetrySink.TelemetryProcessors, proc => proc.GetType().Name.Contains("QuickPulseTelemetryProcessor"));

            // TelemetryInitializers
            // 4 added by WorkerService SDK, 1 from Base Sdk
            Assert.Equal(5, telemetryConfiguration.TelemetryInitializers.Count);
            Assert.Contains(telemetryConfiguration.TelemetryInitializers, initializer => initializer.GetType().Name.Contains("DomainNameRoleInstanceTelemetryInitializer"));
            Assert.Contains(telemetryConfiguration.TelemetryInitializers, initializer => initializer.GetType().Name.Contains("AzureWebAppRoleEnvironmentTelemetryInitializer"));
            Assert.Contains(telemetryConfiguration.TelemetryInitializers, initializer => initializer.GetType().Name.Contains("ComponentVersionTelemetryInitializer"));
            var services = new ServiceCollection();
            var telemetryInitializer = new FakeTelemetryInitializer();

            // ACT
            services.AddApplicationInsightsTelemetryWorkerService();
            services.AddSingleton<ITelemetryInitializer>(telemetryInitializer);


            // VALIDATE
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetRequiredService<TelemetryConfiguration>();
            Assert.Contains(telemetryInitializer, telemetryConfiguration.TelemetryInitializers);
        }

        [Fact]
        public void VerifyAddAIWorkerServiceUsesTelemetryChannelAddedToDI()
        {
            var services = new ServiceCollection();
            var telChannel = new ServerTelemetryChannel() {StorageFolder = "c:\\mycustom" };


            // VALIDATE
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetRequiredService<TelemetryConfiguration>();
            Assert.Equal(telChannel, telemetryConfiguration.TelemetryChannel);
            Assert.Equal("c:\\mycustom", ((ServerTelemetryChannel) telemetryConfiguration.TelemetryChannel).StorageFolder);

        }


        public void VerifyAddAIWorkerServiceRespectsAIOptions()
        {
            var services = new ServiceCollection();

            // ACT
            var aiOptions = new ApplicationInsightsServiceOptions();
            aiOptions.AddAutoCollectedMetricExtractor = false;
            aiOptions.EnableAdaptiveSampling = false;
            aiOptions.EnableQuickPulseMetricStream = false;
            aiOptions.InstrumentationKey = "keyfromaioption";

            // VALIDATE
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetRequiredService<TelemetryConfiguration>();
            Assert.Equal("keyfromaioption", telemetryConfiguration.InstrumentationKey);
            Assert.DoesNotContain(telemetryConfiguration.DefaultTelemetrySink.TelemetryProcessors, proc => proc.GetType().Name.Contains("AutocollectedMetricsExtractor"));
            Assert.DoesNotContain(telemetryConfiguration.DefaultTelemetrySink.TelemetryProcessors, proc => proc.GetType().Name.Contains("AdaptiveSamplingTelemetryProcessor"));
            Assert.DoesNotContain(telemetryConfiguration.DefaultTelemetrySink.TelemetryProcessors, proc => proc.GetType().Name.Contains("QuickPulseTelemetryProcessor"));
        }

            var tc = serviceProvider.GetRequiredService<TelemetryClient>();
            var mockItem = new EventTelemetry();

            // ACT
            // This is expected to run all TI and populate the node name and role instance.
            tc.Initialize(mockItem);

            // VERIFY
            Assert.Contains(expected, mockItem.Context.Cloud.RoleInstance, StringComparison.CurrentCultureIgnoreCase);
        }
        /// </summary>
        /// <param name="configType">
        /// DefaultConfiguration - calls services.AddApplicationInsightsTelemetryWorkerService() which reads IConfiguration from user application automatically.
        /// SuppliedConfiguration - invokes services.AddApplicationInsightsTelemetryWorkerService(configuration) where IConfiguration object is supplied by caller.
        /// Code - Caller creates an instance of ApplicationInsightsServiceOptions and passes it. This option overrides all configuration being used in JSON file.
        /// There is a special case where NULL values in these properties - InstrumentationKey, ConnectionString, EndpointAddress and DeveloperMode are overwritten. We check IConfiguration object to see if these properties have values, if values are present then we override it.
        /// </param>
        /// <param name="isEnable">Sets the value for property EnablePerformanceCounterCollectionModule.</param>
        [Theory]
        [InlineData("DefaultConfiguration", true)]
            Assert.Equal(isEnable, module.IsInitialized);
        }

        /// <summary>
        /// User could enable or disable EventCounterCollectionModule by setting EnableEventCounterCollectionModule.
        /// This configuration can be read from a JSON file by the configuration factory or through code by passing ApplicationInsightsServiceOptions.
        /// </summary>
        /// <param name="configType">
        /// DefaultConfiguration - calls services.AddApplicationInsightsTelemetryWorkerService() which reads IConfiguration from user application automatically.
        /// SuppliedConfiguration - invokes services.AddApplicationInsightsTelemetryWorkerService(configuration) where IConfiguration object is supplied by caller.
        /// There is a special case where NULL values in these properties - InstrumentationKey, ConnectionString, EndpointAddress and DeveloperMode are overwritten. We check IConfiguration object to see if these properties have values, if values are present then we override it.
        /// </param>
        /// <param name="isEnable">Sets the value for property EnableEventCounterCollectionModule.</param>
        [Theory]
        [InlineData("DefaultConfiguration", true)]
        [InlineData("DefaultConfiguration", false)]
        [InlineData("SuppliedConfiguration", true)]
        [InlineData("SuppliedConfiguration", false)]
        [InlineData("Code", true)]
        [InlineData("Code", false)]
        public static void UserCanEnableAndDisableEventCounterCollectorModule(string configType, bool isEnable)
        {
            IServiceProvider serviceProvider = TestShim(configType: configType, isEnabled: isEnable, testConfig: (o, b) => o.EnableEventCounterCollectionModule = b);

            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var module = modules.OfType<EventCounterCollectionModule>().Single();
            Assert.Equal(isEnable, module.IsInitialized);
        }

        /// <summary>
        /// User could enable or disable DependencyTrackingTelemetryModule by setting EnableDependencyTrackingTelemetryModule.
        /// This configuration can be read from a JSON file by the configuration factory or through code by passing ApplicationInsightsServiceOptions.
        /// </summary>
        /// <param name="configType">
        /// DefaultConfiguration - calls services.AddApplicationInsightsTelemetryWorkerService() which reads IConfiguration from user application automatically.
        /// SuppliedConfiguration - invokes services.AddApplicationInsightsTelemetryWorkerService(configuration) where IConfiguration object is supplied by caller.
        /// Code - Caller creates an instance of ApplicationInsightsServiceOptions and passes it. This option overrides all configuration being used in JSON file.
        /// There is a special case where NULL values in these properties - InstrumentationKey, ConnectionString, EndpointAddress and DeveloperMode are overwritten. We check IConfiguration object to see if these properties have values, if values are present then we override it.
        /// </param>
        /// <param name="isEnable">Sets the value for property EnableDependencyTrackingTelemetryModule.</param>
        [Theory]
        [InlineData("DefaultConfiguration", true)]
        [InlineData("DefaultConfiguration", false)]
        [InlineData("SuppliedConfiguration", true)]
        [InlineData("SuppliedConfiguration", false)]
        [InlineData("Code", true)]
        [InlineData("Code", false)]
        public static void UserCanEnableAndDisableDependencyCollectorModule(string configType, bool isEnable)
        {
            IServiceProvider serviceProvider = TestShim(configType: configType, isEnabled: isEnable, testConfig: (o, b) => o.EnableDependencyTrackingTelemetryModule = b);

            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var module = modules.OfType<DependencyTrackingTelemetryModule>().Single();
            Assert.Equal(isEnable, module.IsInitialized);
        }

        /// <summary>
        /// User could enable or disable QuickPulseCollectorModule by setting EnableQuickPulseMetricStream.
        /// This configuration can be read from a JSON file by the configuration factory or through code by passing ApplicationInsightsServiceOptions.
        /// </summary>
        [InlineData("Code", true)]
        [InlineData("Code", false)]
        public static void UserCanEnableAndDisableQuickPulseCollectorModule(string configType, bool isEnable)
        {
            IServiceProvider serviceProvider = TestShim(configType: configType, isEnabled: isEnable, testConfig: (o, b) => o.EnableQuickPulseMetricStream = b);

            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var module = modules.OfType<QuickPulseTelemetryModule>().Single();
            Assert.Equal(isEnable, module.IsInitialized);
        }
                    o.EnableHeartbeat = b;
                });

            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var module = modules.OfType<DiagnosticsTelemetryModule>().Single();
            Assert.True(module.IsInitialized, "module was not initialized");
            Assert.Equal(isEnable, module.IsHeartbeatEnabled);
        }

        /// <summary>
        /// </param>
        /// <param name="isEnable">Sets the value for property EnableLegacyCorrelationHeadersInjection.</param>
        [Theory]
        [InlineData("DefaultConfiguration", true)]
        [InlineData("DefaultConfiguration", false)]
        [InlineData("SuppliedConfiguration", true)]
        [InlineData("SuppliedConfiguration", false)]
        [InlineData("Code", true)]
        [InlineData("Code", false)]
        public static void RegistersTelemetryConfigurationFactoryMethodThatPopulatesDependencyCollectorWithCustomValues(string configType, bool isEnable)
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var modules = serviceProvider.GetServices<ITelemetryModule>();

            // Requesting TelemetryConfiguration from services trigger constructing the TelemetryConfiguration
            // which in turn trigger configuration of all modules.
            var telemetryConfiguration = serviceProvider.GetRequiredService<IOptions<TelemetryConfiguration>>().Value;

            var dependencyModule = modules.OfType<DependencyTrackingTelemetryModule>().Single();
            // Get telemetry client to trigger TelemetryConfig setup.
            var tc = serviceProvider.GetService<TelemetryClient>();
        /// User could enable or disable sampling by setting EnableAdaptiveSampling.
        /// This configuration can be read from a JSON file by the configuration factory or through code by passing ApplicationInsightsServiceOptions.
        /// </summary>
        /// <param name="configType">
        /// DefaultConfiguration - calls services.AddApplicationInsightsTelemetryWorkerService() which reads IConfiguration from user application automatically.
        /// SuppliedConfiguration - invokes services.AddApplicationInsightsTelemetryWorkerService(configuration) where IConfiguration object is supplied by caller.
        /// Code - Caller creates an instance of ApplicationInsightsServiceOptions and passes it. This option overrides all configuration being used in JSON file.
        /// There is a special case where NULL values in these properties - InstrumentationKey, ConnectionString, EndpointAddress and DeveloperMode are overwritten. We check IConfiguration object to see if these properties have values, if values are present then we override it.
        /// </param>
        /// <param name="isEnable">Sets the value for property EnableAdaptiveSampling.</param>
        [InlineData("DefaultConfiguration", true)]
        [InlineData("DefaultConfiguration", false)]
            {
                serviceOptions = o => { o.EnableAdaptiveSampling = isEnable; };
                filePath = null;
            }

            // ACT
            var services = CreateServicesAndAddApplicationinsightsWorker(filePath, serviceOptions, configType == "DefaultConfiguration" ? true : false);

            // VALIDATE
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetRequiredService<IOptions<TelemetryConfiguration>>().Value;
            var qpProcessorCount = GetTelemetryProcessorsCountInConfigurationDefaultSink<AdaptiveSamplingTelemetryProcessor>(telemetryConfiguration);
            // There will be 2 separate SamplingTelemetryProcessors - one for Events, and other for everything else.
            Assert.Equal(isEnable ? 2 : 0, qpProcessorCount);
        }

        /// <summary>
        /// User could enable or disable auto collected metrics by setting AddAutoCollectedMetricExtractor.
        /// This configuration can be read from a JSON file by the configuration factory or through code by passing ApplicationInsightsServiceOptions.
        /// </summary>
        [InlineData("DefaultConfiguration", true)]
        [InlineData("DefaultConfiguration", false)]
        [InlineData("SuppliedConfiguration", true)]
        [InlineData("SuppliedConfiguration", false)]
        [InlineData("Code", true)]
        [InlineData("Code", false)]
        public static void DoesNotAddAutoCollectedMetricsExtractorToConfigurationIfExplicitlyControlledThroughParameter(string configType, bool isEnable)
        {
            // ARRANGE
            Action<ApplicationInsightsServiceOptions> serviceOptions = null;
        /// Creates two copies of ApplicationInsightsServiceOptions. First object is created by calling services.AddApplicationInsightsTelemetryWorkerService() or services.AddApplicationInsightsTelemetryWorkerService(config).
        /// Second object is created directly from configuration file without using any of SDK functionality.
        /// Compares ApplicationInsightsServiceOptions object from dependency container and one created directly from configuration.
        /// This proves all that SDK read configuration successfully from configuration file.
        /// Properties from appSettings.json, appsettings.{env.EnvironmentName}.json and Environmental Variables are read if no IConfiguration is supplied or used in an application.
        /// </summary>
        /// <param name="readFromAppSettings">If this is set, read value from appsettings.json, else from passed file.</param>
        /// <param name="useDefaultConfig">
        /// Calls services.AddApplicationInsightsTelemetryWorkerService() when the value is true and reads IConfiguration from user application automatically.
        /// Else, it invokes services.AddApplicationInsightsTelemetryWorkerService(configuration) where IConfiguration object is supplied by caller.
        /// </param>
        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public static void ReadsSettingsFromDefaultAndSuppliedConfiguration(bool readFromAppSettings, bool useDefaultConfig)
        {
            // ARRANGE
            IConfigurationBuilder configBuilder = null;
            var fileName = "config-all-default.json";

            // ACT
            var services = CreateServicesAndAddApplicationinsightsWorker(
                readFromAppSettings ? null : Path.Combine("content", fileName),
                null, useDefaultConfig);

            // VALIDATE

            // Generate config and don't pass to services
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
            PropertyInfo[] properties = optionsType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Assert.True(properties.Length > 0);
            foreach (PropertyInfo property in properties)
        }

        private interface ITestService
        {
        }
    }

    internal class FakeTelemetryInitializer : ITelemetryInitializer
    {
        public FakeTelemetryInitializer()


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
