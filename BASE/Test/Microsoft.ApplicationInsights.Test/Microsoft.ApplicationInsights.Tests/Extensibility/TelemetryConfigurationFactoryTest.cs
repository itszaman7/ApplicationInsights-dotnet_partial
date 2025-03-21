namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Platform;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Shared.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    

    using EventLevel = System.Diagnostics.Tracing.EventLevel;

    [TestClass]
    public class TelemetryConfigurationFactoryTest
    {
        private const string EnvironmentVariableName = "APPINSIGHTS_INSTRUMENTATIONKEY";
        private const string EnvironmentVariableConnectionString = "APPLICATIONINSIGHTS_CONNECTION_STRING";

        [TestCleanup]
        public void TestCleanup()
        {
            Environment.SetEnvironmentVariable(EnvironmentVariableName, null);
            Environment.SetEnvironmentVariable(EnvironmentVariableConnectionString, null);
            PlatformSingleton.Current = null; // Force reinitialization in future tests so that new environment variables will be loaded.
        }

        #region Instance

        [TestMethod]
        public void ClassIsInternalAndNotMeantForPublicConsumption()
        {
            Assert.IsFalse(typeof(TelemetryConfigurationFactory).GetTypeInfo().IsPublic);
        }

        [TestMethod]
        public void InstanceReturnsDefaultTelemetryConfigurationFactoryInstanceUsedByTelemetryConfiguration()
        {
            Assert.IsNotNull(TelemetryConfigurationFactory.Instance);
        }

        [TestMethod]
        public void InstanceCanGeSetByTestsToIsolateTestingOfTelemetryConfigurationFromRealFactoryLogic()
        {
            var replacement = new TestableTelemetryConfigurationFactory();
            TelemetryConfigurationFactory.Instance = replacement;
            Assert.AreSame(replacement, TelemetryConfigurationFactory.Instance);
        }

        [TestMethod]
        public void InstanceIsLazilyInitializedToSimplifyResettingOfGlobalStateInTests()
        {
            TelemetryConfigurationFactory.Instance = null;
            Assert.IsNotNull(TelemetryConfigurationFactory.Instance);
        }

        #endregion

        #region Initialize

        [TestMethod]
        public void InitializeCreatesInMemoryChannel()
        {
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null);

            AssertEx.IsType<InMemoryChannel>(configuration.TelemetryChannel);
        }
        
        [TestMethod]
        public void InitializesInstanceWithEmptyInstrumentationKey()
        {
            string configFileContents = Configuration("<InstrumentationKey></InstrumentationKey>");

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            using (var testableTelemetryModules = new TestableTelemetryModules())
            {
                new TestableTelemetryConfigurationFactory().Initialize(configuration, testableTelemetryModules, configFileContents);

                // Assume that LoadFromXml method is called, tested separately
                Assert.AreEqual(string.Empty, configuration.InstrumentationKey);
            }
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void InitializesInstanceWithEmptyConnectionString()
        {
            string configFileContents = Configuration($"<ConnectionString></ConnectionString>");

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            using (var testableTelemetryModules = new TestableTelemetryModules())
            {
                new TestableTelemetryConfigurationFactory().Initialize(configuration, testableTelemetryModules, configFileContents);

                // Assume that LoadFromXml method is called, tested separately
                Assert.AreEqual(null, configuration.ConnectionString);
                Assert.AreEqual(string.Empty, configuration.InstrumentationKey);
            }
        }

        [TestMethod]
        public void InitializesInstanceWithInformationFromConfigurationFileWhenItExists()
        {
            string configFileContents = Configuration("<InstrumentationKey>F8474271-D231-45B6-8DD4-D344C309AE69</InstrumentationKey>");

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            using (var testableTelemetryModules = new TestableTelemetryModules())
            {
                new TestableTelemetryConfigurationFactory().Initialize(configuration, testableTelemetryModules, configFileContents);

                // Assume that LoadFromXml method is called, tested separately
                Assert.IsFalse(string.IsNullOrEmpty(configuration.InstrumentationKey));
            }
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifyChannelEndpointsAreSetWhenParsingFromConfigFile_InMemoryChannel()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            // PART 1 - CONFIGURATION FACTORY IS EXPECTED TO CREATE A CONFIG THAT MATCHES THE XML
            string ikeyConfig = "00000000-0000-0000-1111-000000000000";
            string ikeyConfigConnectionString = "00000000-0000-0000-2222-000000000000";

            string configString = @"<InstrumentationKey>00000000-0000-0000-1111-000000000000</InstrumentationKey>
  <TelemetryChannel Type=""Microsoft.ApplicationInsights.Channel.InMemoryChannel, Microsoft.ApplicationInsights"">
    <EndpointAddress>http://10.0.0.0/v2/track</EndpointAddress>
    <DeveloperMode>true</DeveloperMode>
  </TelemetryChannel>";

            string configFileContents = Configuration(configString);
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(ikeyConfig, configuration.InstrumentationKey);
            Assert.AreEqual(true, configuration.TelemetryChannel.DeveloperMode);
            Assert.AreEqual("http://10.0.0.0/v2/track", configuration.TelemetryChannel.EndpointAddress, "failed to set Channel Endpoint to config value");

            // PART 2 - VERIFY SETTING THE CONNECTION STRING WILL OVERWRITE CHANNEL ENDPOINT.
            TelemetryConfiguration.Active = configuration;

            TelemetryConfiguration.Active.ConnectionString = $"InstrumentationKey={ikeyConfigConnectionString};IngestionEndpoint=https://localhost:63029/";

            var client = new TelemetryClient();

            Assert.AreEqual(string.Empty, client.InstrumentationKey);
            Assert.AreEqual(ikeyConfigConnectionString, client.TelemetryConfiguration.InstrumentationKey);
            Assert.AreEqual("https://localhost:63029/v2/track", client.TelemetryConfiguration.TelemetryChannel.EndpointAddress);
#pragma warning restore CS0618 // Type or member is obsolete
        }


        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifyChannelEndpointsAreSetWhenParsingFromConfigFile_ServerTelemetryChannel()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            // PART 1 - CONFIGURATION FACTORY IS EXPECTED TO CREATE A CONFIG THAT MATCHES THE XML
            string ikeyConfig = "00000000-0000-0000-1111-000000000000";
            string ikeyConfigConnectionString = "00000000-0000-0000-2222-000000000000";

            string configString = @"<InstrumentationKey>00000000-0000-0000-1111-000000000000</InstrumentationKey>
  <TelemetryChannel Type=""Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.ServerTelemetryChannel, Microsoft.AI.ServerTelemetryChannel"">
    <EndpointAddress>http://10.0.0.0/v2/track</EndpointAddress>
  </TelemetryChannel>";

            string configFileContents = Configuration(configString);
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(ikeyConfig, configuration.InstrumentationKey);
            Assert.AreEqual("http://10.0.0.0/v2/track", configuration.TelemetryChannel.EndpointAddress, "failed to set Channel Endpoint to config value");

            // PART 2 - VERIFY SETTING THE CONNECTION STRING WILL OVERWRITE CHANNEL ENDPOINT.
            TelemetryConfiguration.Active = configuration;

            TelemetryConfiguration.Active.ConnectionString = $"InstrumentationKey={ikeyConfigConnectionString};IngestionEndpoint=https://localhost:63029/";

            var client = new TelemetryClient();

            Assert.AreEqual(string.Empty, client.InstrumentationKey);
            Assert.AreEqual(ikeyConfigConnectionString, client.TelemetryConfiguration.InstrumentationKey);
            Assert.AreEqual("https://localhost:63029/v2/track", client.TelemetryConfiguration.TelemetryChannel.EndpointAddress);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifyThatChannelEndpointIsNotOverwrittenIfManuallyConfigured()
        {
            var configuration = new TelemetryConfiguration();
            Assert.AreEqual("https://dc.services.visualstudio.com/", configuration.EndpointContainer.Ingestion.AbsoluteUri);

            var customEndpoint = "http://10.0.0.0/v2/track";
            var customChannel = new InMemoryChannel
            {
                EndpointAddress = customEndpoint
            };

            Assert.AreEqual(customEndpoint, customChannel.EndpointAddress);

            configuration.TelemetryChannel = customChannel;

            Assert.AreEqual(customEndpoint, customChannel.EndpointAddress, "channel endpoint was overwritten by config");
        }


        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifySelectInstrumentationKeyChooses_EnVarConnectionString()
        {
            // SETUP
            string ikeyEnVarConnectionString = Guid.NewGuid().ToString();
            Environment.SetEnvironmentVariable(EnvironmentVariableConnectionString, $"InstrumentationKey={ikeyEnVarConnectionString}");

            string ikeyEnVar = Guid.NewGuid().ToString();
            Environment.SetEnvironmentVariable(EnvironmentVariableName, ikeyEnVar);

            string ikeyConfig = "e6f55001-f7d1-4242-b9f4-83660d0487f9";
            string ikeyConfigConnectionString = "F8474271-D231-45B6-8DD4-D344C309AE69";

            string configFileContents = Configuration($"<InstrumentationKey>{ikeyConfig}</InstrumentationKey><ConnectionString>InstrumentationKey={ikeyConfigConnectionString}</ConnectionString>");

            TelemetryConfiguration configuration = new TelemetryConfiguration();

            // ACT
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            // ASSERT
            Assert.AreEqual(ikeyEnVarConnectionString, configuration.InstrumentationKey);
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifySelectInstrumentationKeyChooses_EnVar()
        {
            // SETUP
            string ikeyEnVar = Guid.NewGuid().ToString();
            Environment.SetEnvironmentVariable(EnvironmentVariableName, ikeyEnVar);

            string ikeyConfig = "e6f55001-f7d1-4242-b9f4-83660d0487f9";
            string ikeyConfigConnectionString = "F8474271-D231-45B6-8DD4-D344C309AE69";

            string configFileContents = Configuration($"<InstrumentationKey>{ikeyConfig}</InstrumentationKey><ConnectionString>InstrumentationKey={ikeyConfigConnectionString}</ConnectionString>");

            TelemetryConfiguration configuration = new TelemetryConfiguration();

            // ACT
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            // ASSERT
            Assert.AreEqual(ikeyEnVar, configuration.InstrumentationKey);
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifySelectInstrumentationKeyChooses_ConfigConnectionString()
        {
            // SETUP
            string ikeyConfig = "e6f55001-f7d1-4242-b9f4-83660d0487f9";
            string ikeyConfigConnectionString = "F8474271-D231-45B6-8DD4-D344C309AE69";

            string configFileContents = Configuration($"<InstrumentationKey>{ikeyConfig}</InstrumentationKey><ConnectionString>InstrumentationKey={ikeyConfigConnectionString}</ConnectionString>");

            TelemetryConfiguration configuration = new TelemetryConfiguration();

            // ACT
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            // ASSERT
            Assert.AreEqual(ikeyConfigConnectionString, configuration.InstrumentationKey);
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifySelectInstrumentationKeyChooses_ConfigConnectionString_Reverse()
        {
            // SETUP
            string ikeyConfig = "e6f55001-f7d1-4242-b9f4-83660d0487f9";
            string ikeyConfigConnectionString = "F8474271-D231-45B6-8DD4-D344C309AE69";

            string configFileContents = Configuration($"<ConnectionString>InstrumentationKey={ikeyConfigConnectionString}</ConnectionString><InstrumentationKey>{ikeyConfig}</InstrumentationKey>");

            TelemetryConfiguration configuration = new TelemetryConfiguration();

            // ACT
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            // ASSERT
            Assert.AreEqual(ikeyConfig, configuration.InstrumentationKey);
        }
            // ACT
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            // ASSERT
            Assert.AreEqual(ikeyConfig, configuration.InstrumentationKey);
        }

        [TestMethod]
        public void InitializeAddsOperationContextTelemetryInitializerByDefault()
        {
        {
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, Configuration("</blah>"));

            AssertEx.IsType<InMemoryChannel>(configuration.TelemetryChannel);
        }

        [TestMethod]
        public void InitializeCreatesInMemoryChannelEvenWhenConfigIsInvalid()
        {

            // ACT
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null);

            // ASSERT
            Assert.AreEqual(string.Empty, configuration.InstrumentationKey);
        }

        #endregion

        public void CreateInstanceReturnsInstanceOfTypeSpecifiedByTypeName()
        {
            Type type = typeof(StubTelemetryInitializer);
            object instance = TestableTelemetryConfigurationFactory.CreateInstance(typeof(ITelemetryInitializer), type.AssemblyQualifiedName);
            Assert.AreEqual(type, instance.GetType());
        }

        [TestMethod]
        public void CreateInstanceReturnsNullWhenTypeCannotBeFound()
        {
        [TestMethod]
        public void CreateInstanceReturnsNullWhenInstanceDoesNotImplementExpectedInterface()
        {
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            Type invalidType = typeof(object);
            Assert.IsNull(TestableTelemetryConfigurationFactory.CreateInstance(typeof(ITelemetryInitializer), invalidType.AssemblyQualifiedName));
        }

        #endregion

        #region LoadFromXml

        [TestMethod]
        public void LoadFromXmlInitializesGivenTelemetryConfigurationInstanceFromXml()
        {
            string expected = Guid.NewGuid().ToString();
            string profile = Configuration("<InstrumentationKey>" + expected + "</InstrumentationKey>");

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            TestableTelemetryConfigurationFactory.LoadFromXml(configuration, null, XDocument.Parse(profile));

        [TestMethod]
        public void LoadInstanceSetsInstancePropertiesFromChildElementValuesOfDefinition()
        {
            var definition = new XElement(
                "Definition",
                new XAttribute("Type", typeof(StubClassWithProperties).AssemblyQualifiedName),
                new XElement("StringProperty", "TestValue"));

            object instance = TestableTelemetryConfigurationFactory.LoadInstance(definition, typeof(StubClassWithProperties), null, null);

            Assert.AreEqual("TestValue", ((StubClassWithProperties)instance).StringProperty);
        }

        [TestMethod]
        public void LoadInstanceSetsInstancePropertiesOfTimeSpanTypeFromChildElementValuesOfDefinitionWithTimeSpanFormat()
        {
            var definition = new XElement(
                "Definition",
                new XAttribute("Type", typeof(StubClassWithProperties).AssemblyQualifiedName),
        [TestMethod]
        public void LoadInstanceSetsInstancePropertiesOfTimeSpanTypeFromChildElementValuesOfDefinitionWithOneInteger()
        {
            var definition = new XElement(
                "Definition",
                new XAttribute("Type", typeof(StubClassWithProperties).AssemblyQualifiedName),
                new XElement("TimeSpanProperty", "7"));

            object instance = TestableTelemetryConfigurationFactory.LoadInstance(definition, typeof(StubClassWithProperties), null, null);

            Assert.AreEqual("TestValue", original.StringProperty);
        }

        [TestMethod]
        public void LoadInstanceHandlesEnumPropertiesWithNumericValue()
        {
            var definition = new XElement(
                "Definition",
                new XElement("EnumProperty", "3"));
        {
            var definition = new XElement(
                "Definition",
                new XElement("EnumProperty", "Informational"));

            var original = new StubClassWithProperties();
            object instance = TestableTelemetryConfigurationFactory.LoadInstance(definition, typeof(StubClassWithProperties), original, null);

            Assert.AreEqual(EventLevel.Informational, original.EnumProperty);
        }

            object actual = TestableTelemetryConfigurationFactory.LoadInstance(definition, typeof(string), null, null);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LoadInstanceReturnsNullGivenEmptyXmlElementForReferenceType()
        {
            var definition = new XElement("Definition");
            Assert.IsNull(instance);
        }

        [TestMethod]
        public void LoadInstanceReturnsOriginalValueGivenNullXmlElement()
        {
            var original = "Test Value";
            object loaded = TestableTelemetryConfigurationFactory.LoadInstance(null, original.GetType(), original, null);
            Assert.AreSame(original, loaded);
        }
        public void LoadInstanceCreatesNewInstanceOfExpectedTypeWhenTypeAttributeIsNotSpecified()
        {
            var definition = new XElement("Definition", new XElement("Int32Property", 42));

            object instance = TestableTelemetryConfigurationFactory.LoadInstance(definition, typeof(StubClassWithProperties), null, null);

            var loaded = AssertEx.IsType<StubClassWithProperties>(instance);
            Assert.AreEqual(42, loaded.Int32Property);
        }

            object instance = TestableTelemetryConfigurationFactory.LoadInstance(definition, typeof(StubClassWithProperties), null, null);

            var loaded = AssertEx.IsType<StubClassWithProperties>(instance);
            Assert.AreEqual(42, loaded.Int32Property);
        }

#endregion

#region TelemetryProcesors

                  "<Add Type=\"" + typeof(StubTelemetryProcessor2).AssemblyQualifiedName + "\" />" +
                  "</TelemetryProcessors>");

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            using (var testableTelemetryModules = new TestableTelemetryModules())
            {
                new TestableTelemetryConfigurationFactory().Initialize(configuration, testableTelemetryModules, configFileContents);

                // Assume that LoadFromXml method is called, tested separately
                Assert.IsTrue(configuration.TelemetryProcessors != null);

                //validate the chain linking stub1->stub2->pass through->sink
                var tp1 = (StubTelemetryProcessor)configuration.TelemetryProcessorChain.FirstTelemetryProcessor;
                var tp2 = (StubTelemetryProcessor2)tp1.next;
                var passThroughProcessor = tp2.next as PassThroughProcessor;
                Assert.IsNotNull(passThroughProcessor);

                // The sink has only a transmission processor and a default channel.
                var sink = passThroughProcessor.Sink;
                Assert.IsNotNull(sink);
                Assert.AreEqual(1, sink.TelemetryProcessorChain.TelemetryProcessors.Count);
                AssertEx.IsType<TransmissionProcessor>(sink.TelemetryProcessorChain.FirstTelemetryProcessor);
                AssertEx.IsType<InMemoryChannel>(sink.TelemetryChannel);
            }
        }

        [TestMethod]
        public void InitializeTelemetryProcessorsWithWrongProcessorInTheMiddle()
        {
            string configFileContents = Configuration(
                "<TelemetryProcessors>" +
                  "<Add Type=\"" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + "\" />" +
                  "<Add Type = \"Invalid, Invalid\" />" +
                  "<Add Type=\"" + typeof(StubTelemetryProcessor2).AssemblyQualifiedName + "\" />" +
                  "</TelemetryProcessors>");

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            using (var testableTelemetryModules = new TestableTelemetryModules())
            {
                new TestableTelemetryConfigurationFactory().Initialize(configuration, testableTelemetryModules, configFileContents);

                Assert.IsTrue(configuration.TelemetryProcessors != null);
                AssertEx.IsType<StubTelemetryProcessor>(configuration.TelemetryProcessorChain.FirstTelemetryProcessor);

                //validate the chain linking stub1->stub2->pass through->sink
                var tp1 = (StubTelemetryProcessor)configuration.TelemetryProcessorChain.FirstTelemetryProcessor;
                var tp2 = (StubTelemetryProcessor2)tp1.next;
                var passThroughProcessor = tp2.next as PassThroughProcessor;
                Assert.IsNotNull(passThroughProcessor);


            // The sink has only a transmission processor and a default channel.
            var sink = passThroughProcessor.Sink;
            Assert.IsNotNull(sink);
            Assert.AreEqual(1, sink.TelemetryProcessorChain.TelemetryProcessors.Count);
            AssertEx.IsType<TransmissionProcessor>(sink.TelemetryProcessorChain.FirstTelemetryProcessor);
            AssertEx.IsType<InMemoryChannel>(sink.TelemetryChannel);
        }

        [TestMethod]
        public void RebuildDoesNotRemoveTelemetryProcessorsLoadedFromConfiguration()
        {
            string configFileContents = Configuration(
                @"                  
                  <TelemetryProcessors>
                    <Add Type=""" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />                  
                  </TelemetryProcessors>"
                );

            TelemetryConfiguration configuration = new TelemetryConfiguration();

            Assert.AreEqual(3, configuration.TelemetryProcessors.Count);
            AssertEx.IsType<StubTelemetryProcessor>(configuration.TelemetryProcessors[0]);
        }

        [TestMethod]
        public void UseAddsProcessorAfterProcessorsDefinedInConfiguration()
        {
            string configFileContents = Configuration(
                @"                  
                    <Add Type=""" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />                  
                  </TelemetryProcessors>"
                );

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            var builder = configuration.TelemetryProcessorChainBuilder;
            builder.Use(_ => new StubTelemetryProcessor2(_));
            builder.Build();
            string configFileContents = Configuration(
                @"<TelemetryModules>
                    <Add Type = """ + typeof(StubConfigurable).AssemblyQualifiedName + @"""  />
                  </TelemetryModules>"
                );

            using (var modules = new TestableTelemetryModules())
            {
                new TestableTelemetryConfigurationFactory().Initialize(new TelemetryConfiguration(), modules, configFileContents);

                AssertEx.IsType<DiagnosticsTelemetryModule>(modules.Modules[0]);
            }
        }


        [TestMethod]
        public void InitializeTelemetryModulesFromConfigurationFileWhenOneModuleCannotBeLoaded()
        {
            string configFileContents = Configuration(
                @"<TelemetryModules>
                //Assert.DoesNotThrow
                new TestableTelemetryConfigurationFactory().Initialize(
                    new TelemetryConfiguration(),
                    modules,
                    configFileContents);
            }
        }

#endregion

#region TelemetryInitializers
        [TestMethod]
        public void InitializeAddTelemetryInitializersWithOneInvalid()
        {
            string configFileContents = Configuration(
                @"<TelemetryInitializers>
                    <Add Type=""Invalid, Invalid"" />
                    <Add Type=""" + typeof(StubTelemetryInitializer).AssemblyQualifiedName + @""" />
                  </TelemetryInitializers>"
                );

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(2, configuration.TelemetryInitializers.Count); // Time and operation initializers are added by default
            Assert.IsNotNull(configuration.TelemetryInitializers.First(item => item.GetType().Name == "StubTelemetryInitializer"));
        }


#endregion

#region LoadInstances<T>

        [TestMethod]
        public void LoadInstancesPopulatesListWithInstancesOfSpecifiedType()
        {
            var element = XElement.Parse(@"
                <List xmlns=""http://schemas.microsoft.com/ApplicationInsights/2013/Settings"">
                    <Add Type=""" + typeof(StubTelemetryInitializer).AssemblyQualifiedName + @""" />           
                </List>");
        public void LoadInstancesUpdatesInstanceWithMatchingType()
        {
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            var element = XElement.Parse(@"
                <List xmlns=""http://schemas.microsoft.com/ApplicationInsights/2013/Settings"">
                    <Add Type=""" + typeof(StubConfigurableWithProperties).AssemblyQualifiedName + @""" > 
                        <Int32Property>77</Int32Property>
                    </Add>
                </List>");

            Assert.AreEqual(configurableElement, telemetryModules[0]);
            Assert.AreEqual(77, configurableElement.Int32Property);
        }

        [TestMethod]
        public void LoadInstancesPopulatesListWithPrimitiveValues()
        {
            var definition = XElement.Parse(@"
                <List xmlns=""http://schemas.microsoft.com/ApplicationInsights/2013/Settings"">
                    <Add>41</Add>
        {
            var definition = XElement.Parse(@"
                <List xmlns=""http://schemas.microsoft.com/ApplicationInsights/2013/Settings"">
                    <Unknown/>
                    <Add>42</Add>
                </List>");

            var instances = new List<int>();
            //Assert.
            TestableTelemetryConfigurationFactory.LoadInstances(definition, instances, null);
        }

#endregion

#region LoadProperties

        [TestMethod]
        public void LoadPropertiesConvertsPropertyValuesFromStringToPropertyType()
        {
            var definition = new XElement("Definition", new XElement("Int32Property", "42"));
        public void LoadPropertiesReturnsNullWhenInstanceDoesNotHavePropertyWithSpecifiedName()
        {
            var definition = new XElement("Definition", new XElement("InvalidProperty", "AnyValue"));
            //Assert.DoesNotThrow
            TestableTelemetryConfigurationFactory.LoadProperties(definition, new StubClassWithProperties(), null);
        }

        [TestMethod]
        public void LoadPropertiesIgnoresUnknownTelemetryConfigurationPropertiesToAllowStatusMonitorDefineItsOwnSections()
        {
        {
            var definition = new XElement("Definition", new XElement("ChildProperty", new XAttribute("Type", typeof(StubClassWithProperties).AssemblyQualifiedName)));
            var instance = new StubClassWithProperties();

            TestableTelemetryConfigurationFactory.LoadProperties(definition, instance, null);

            Assert.AreEqual(typeof(StubClassWithProperties), instance.ChildProperty.GetType());
        }

        [TestMethod]
        public void LoadPropertiesRecursivelyLoadsInstanceSpecifiedByTypeAttribute()
        {
            var definition = new XElement(
                "Definition",
                new XElement(
                    "ChildProperty",
                    new XAttribute("Type", typeof(StubClassWithProperties).AssemblyQualifiedName),
                    new XElement("StringProperty", "TestValue")));
            var instance = new StubClassWithProperties();

            XElement definition = XDocument.Parse(Configuration(@"<TelemetryModules/>")).Root;
            var instance = new TelemetryConfiguration();
            //Assert.DoesNotThrow
            TestableTelemetryConfigurationFactory.LoadProperties(definition, instance, null);
        }

        [TestMethod]
        public void LoadPropertiesLoadsPropertiesFromAttributes()
        {
            var definition = new XElement("Definition", new XAttribute("Int32Property", "42"));

            var instance = new StubClassWithProperties();
            TestableTelemetryConfigurationFactory.LoadProperties(definition, instance, null);

            Assert.AreEqual(42, instance.Int32Property);
        }

        [TestMethod]
        public void LoadPropertiesGivesPrecedenceToValuesFromElementsBecauseTheyAppearBelowAttributes()
        {
                @"<TelemetryChannel Type=""" + typeof(StubTelemetryChannel).AssemblyQualifiedName + @""">
                    <IntegerProperty>123a</IntegerProperty>
                 </TelemetryChannel>")).Root;

            var instance = new TelemetryConfiguration();

            TestableTelemetryConfigurationFactory.LoadProperties(definition, instance, null);
        }

        [TestMethod]
#if NET7_0_OR_GREATER
        [ExpectedExceptionWithMessage(typeof(ArgumentException), "Failed to parse configuration value. Property: 'IntegerProperty' Reason: The input string '123a' was not in a correct format.")]
#else
        [ExpectedExceptionWithMessage(typeof(ArgumentException), "Failed to parse configuration value. Property: 'IntegerProperty' Reason: Input string was not in a correct format.")]
#endif
        public void LoadProperties_TelemetryClientThrowsException()
        {
            string testConfig = Configuration(
                @"<TelemetryChannel Type=""" + typeof(StubTelemetryChannel).AssemblyQualifiedName + @""">
                    <IntegerProperty>123a</IntegerProperty>
                 </TelemetryChannel>");

            new TelemetryClient(TelemetryConfiguration.CreateFromConfiguration(testConfig));
        }

        [TestMethod]
        public void LoadPropertiesIgnoresNamespaceDeclarationWhenLoadingFromAttributes()
        {
            var definition = new XElement("Definition", new XAttribute("xmlns", "http://somenamespace"));

        }

#endregion

#region TelemetrySinks

        [TestMethod]
        public void EmptyConfigurationCreatesDefaultSink()
        {
            string configFileContents = Configuration(string.Empty);
            Assert.IsTrue(configuration.TelemetryProcessors[0] is PassThroughProcessor);

            // The sink has just a transmission processor feeding into InMemoryChannel.
            var defaultSink = configuration.DefaultTelemetrySink;
            var sinkTelemetryProcessors = defaultSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(1, sinkTelemetryProcessors.Count);
            Assert.IsTrue(sinkTelemetryProcessors[0] is TransmissionProcessor);

            Assert.IsTrue(defaultSink.TelemetryChannel is InMemoryChannel);
        }

        [TestMethod]
        public void NoSinkConfigurationWithCustomChannel()
        {
            string configFileContents = Configuration(@"
                <TelemetryChannel Type=""" + typeof(StubTelemetryChannel).AssemblyQualifiedName + @""" />
            ");

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);
            // Common telemetry processor chain has just one PassThroughProcessor.
            Assert.AreEqual(1, configuration.TelemetryProcessors.Count);
            Assert.IsTrue(configuration.TelemetryProcessors[0] is PassThroughProcessor);

            // The sink has just a transmission processor feeding into the custom channel.
            var defaultSink = configuration.DefaultTelemetrySink;
            var sinkTelemetryProcessors = defaultSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(1, sinkTelemetryProcessors.Count);
            Assert.IsTrue(sinkTelemetryProcessors[0] is TransmissionProcessor);

                </TelemetrySinks>
            ");
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(1, configuration.TelemetrySinks.Count);

            // Common telemetry processor chain has just one PassThroughProcessor.
            Assert.AreEqual(1, configuration.TelemetryProcessors.Count);
            Assert.IsTrue(configuration.TelemetryProcessors[0] is PassThroughProcessor);

            Assert.IsTrue(defaultSink.TelemetryChannel is InMemoryChannel);
        }

        [TestMethod]
        public void DefaultSinkWithCustomProcessors()
        {
            string configFileContents = Configuration(@"
                <TelemetrySinks>
                    <Add Name=""default"">
                    </Add>
                </TelemetrySinks>
            ");
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(1, configuration.TelemetrySinks.Count);

            // Common telemetry processor chain has just one PassThroughProcessor.
            Assert.AreEqual(1, configuration.TelemetryProcessors.Count);
                    </Add>
                </TelemetrySinks>
            ");
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(1, configuration.TelemetrySinks.Count);

            // Common telemetry processor chain has just one PassThroughProcessor.
            Assert.AreEqual(1, configuration.TelemetryProcessors.Count);
                <TelemetryProcessors>
                    <Add Type=""" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />
                </TelemetryProcessors>

                <TelemetrySinks>
                    <Add Name=""default"">
                        <TelemetryProcessors>
                            <Add Type=""" + typeof(StubTelemetryProcessor2).AssemblyQualifiedName + @""" />
                        </TelemetryProcessors>
                    </Add>
        }

        [TestMethod]
        public void NonDefaultEmptySink()
        {
            string configFileContents = Configuration(@"
                <TelemetrySinks>
                    <Add Name=""custom"" />
                </TelemetrySinks>
            ");
            var sinkTelemetryProcessors = defaultSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(1, sinkTelemetryProcessors.Count);
            Assert.IsTrue(sinkTelemetryProcessors[0] is TransmissionProcessor);
            Assert.IsTrue(defaultSink.TelemetryChannel is InMemoryChannel);

            var customSink = configuration.TelemetrySinks[1];
            var customSinkTelemetryProcessors = customSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(1, customSinkTelemetryProcessors.Count);
            Assert.IsTrue(customSinkTelemetryProcessors[0] is TransmissionProcessor);
            Assert.IsTrue(customSink.TelemetryChannel is InMemoryChannel);
        [TestMethod]
        public void NonDefaultSinkWithCustomChannel()
        {
            string configFileContents = Configuration(@"
                <TelemetrySinks>
                    <Add Name=""custom"">
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel).AssemblyQualifiedName + @""" />
                    </Add>
                </TelemetrySinks>
            ");
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(2, configuration.TelemetrySinks.Count);

            Assert.AreEqual(1, configuration.TelemetryProcessors.Count);
            Assert.IsTrue(configuration.TelemetryProcessors[0] is BroadcastProcessor);

            var defaultSink = configuration.DefaultTelemetrySink;
            var sinkTelemetryProcessors = defaultSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(1, sinkTelemetryProcessors.Count);
            Assert.IsTrue(sinkTelemetryProcessors[0] is TransmissionProcessor);
            Assert.IsTrue(defaultSink.TelemetryChannel is InMemoryChannel);

            var customSink = configuration.TelemetrySinks[1];
            var customSinkTelemetryProcessors = customSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(1, customSinkTelemetryProcessors.Count);
            Assert.IsTrue(customSinkTelemetryProcessors[0] is TransmissionProcessor);
            Assert.IsTrue(customSink.TelemetryChannel is StubTelemetryChannel);
        }
                            <Add Type = """ + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />
                       </TelemetryProcessors>
                    </Add>
                </TelemetrySinks>
            ");
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(2, configuration.TelemetrySinks.Count);

            Assert.AreEqual(1, configuration.TelemetryProcessors.Count);
            Assert.IsTrue(configuration.TelemetryProcessors[0] is BroadcastProcessor);

            var defaultSink = configuration.DefaultTelemetrySink;
            var sinkTelemetryProcessors = defaultSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(1, sinkTelemetryProcessors.Count);
            Assert.IsTrue(sinkTelemetryProcessors[0] is TransmissionProcessor);
            Assert.IsTrue(defaultSink.TelemetryChannel is InMemoryChannel);

            var customSink = configuration.TelemetrySinks[1];
            var customSinkTelemetryProcessors = customSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(2, customSinkTelemetryProcessors.Count);
            Assert.IsTrue(customSinkTelemetryProcessors[0] is StubTelemetryProcessor);
            Assert.IsTrue(customSinkTelemetryProcessors[1] is TransmissionProcessor);
            Assert.IsTrue(customSink.TelemetryChannel is InMemoryChannel);
        }


        [TestMethod]
        public void NonDefaultSinkWithCustomChannelAndProcessors()
                    <Add Name=""custom"">
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel).AssemblyQualifiedName + @""" />
                        <TelemetryProcessors>
                            <Add Type = """ + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />
                       </TelemetryProcessors>
                    </Add>
                </TelemetrySinks>
            ");
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);
        }

        [TestMethod]
        public void DefaultAndNonDefaultSink()
        {
            string configFileContents = Configuration(@"
                <TelemetrySinks>
                    <Add Name=""default"">
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel).AssemblyQualifiedName + @""" />
                        <TelemetryProcessors>
                            <Add Type=""" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />
                        </TelemetryProcessors>
                    </Add>
                    <Add Name=""custom"">
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel2).AssemblyQualifiedName + @""" />
                        <TelemetryProcessors>
                            <Add Type = """ + typeof(StubTelemetryProcessor2).AssemblyQualifiedName + @""" />
                       </TelemetryProcessors>
                    </Add>
                </TelemetrySinks>
            Assert.AreEqual(2, configuration.TelemetrySinks.Count);

            Assert.AreEqual(1, configuration.TelemetryProcessors.Count);
            Assert.IsTrue(configuration.TelemetryProcessors[0] is BroadcastProcessor);

            var defaultSink = configuration.DefaultTelemetrySink;
            var sinkTelemetryProcessors = defaultSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(2, sinkTelemetryProcessors.Count);
            Assert.IsTrue(sinkTelemetryProcessors[0] is StubTelemetryProcessor);
            Assert.IsTrue(sinkTelemetryProcessors[1] is TransmissionProcessor);
        public void MultipleCustomSinks()
        {
            string configFileContents = Configuration(@"
                <TelemetrySinks>
                    <Add Name=""alpha"">
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel).AssemblyQualifiedName + @""" />
                        <TelemetryProcessors>
                            <Add Type=""" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />
                        </TelemetryProcessors>
                    </Add>
                    <Add Name=""bravo"">
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel2).AssemblyQualifiedName + @""" />
                        <TelemetryProcessors>
                            <Add Type = """ + typeof(StubTelemetryProcessor2).AssemblyQualifiedName + @""" />
                       </TelemetryProcessors>
                    </Add>
                </TelemetrySinks>
            ");
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);
                    <Add Name=""default"">
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel).AssemblyQualifiedName + @""" />
                        <TelemetryProcessors>
                            <Add Type=""" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />
                        </TelemetryProcessors>
                    </Add>
                    <Add Name=""alpha"">
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel).AssemblyQualifiedName + @""" />
                        <TelemetryProcessors>
                            <Add Type=""" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />
                        </TelemetryProcessors>
                    </Add>
                    <Add>
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel).AssemblyQualifiedName + @""" />
                        <TelemetryProcessors>
                            <Add Type=""" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />
                        </TelemetryProcessors>
                    </Add>
                    <Add Name=""default"">
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel2).AssemblyQualifiedName + @""" />
                        <TelemetryProcessors>
                            <Add Type = """ + typeof(StubTelemetryProcessor2).AssemblyQualifiedName + @""" />
                       </TelemetryProcessors>
                    </Add>
                    <Add Name=""alpha"">
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel2).AssemblyQualifiedName + @""" />
                        <TelemetryProcessors>
                            <Add Type=""" + typeof(StubTelemetryProcessor2).AssemblyQualifiedName + @""" />
                        </TelemetryProcessors>
                    </Add>
            Assert.IsTrue(defaultSink.TelemetryChannel is StubTelemetryChannel2);

            var alphaSink = configuration.TelemetrySinks[1];
            Assert.AreEqual("alpha", alphaSink.Name);
            var alphaSinkTelemetryProcessors = alphaSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(3, alphaSinkTelemetryProcessors.Count);
            Assert.IsTrue(alphaSinkTelemetryProcessors[0] is StubTelemetryProcessor);
            Assert.IsTrue(alphaSinkTelemetryProcessors[1] is StubTelemetryProcessor2);
            Assert.IsTrue(alphaSinkTelemetryProcessors[2] is TransmissionProcessor);
            Assert.IsTrue(alphaSink.TelemetryChannel is StubTelemetryChannel2);

            var firstUnnamedSink = configuration.TelemetrySinks[2];
            Assert.IsTrue(string.IsNullOrEmpty(firstUnnamedSink.Name));
            var firstUnnamedSinkTelemetryProcessors = firstUnnamedSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(2, firstUnnamedSinkTelemetryProcessors.Count);
            Assert.IsTrue(firstUnnamedSinkTelemetryProcessors[0] is StubTelemetryProcessor);
            Assert.IsTrue(firstUnnamedSinkTelemetryProcessors[1] is TransmissionProcessor);
            Assert.IsTrue(firstUnnamedSink.TelemetryChannel is StubTelemetryChannel);

            var secondUnnamedSink = configuration.TelemetrySinks[3];
            Assert.IsTrue(string.IsNullOrEmpty(secondUnnamedSink.Name));
            var secondUnnamedSinkTelemetryProcessors = secondUnnamedSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(1, secondUnnamedSinkTelemetryProcessors.Count);
            Assert.IsTrue(secondUnnamedSinkTelemetryProcessors[0] is TransmissionProcessor);
            Assert.IsTrue(secondUnnamedSink.TelemetryChannel is InMemoryChannel);
        }

        [TestMethod]
        public void TelemetrySinkInitializesChannelAndAllProcessors()
        {
            TelemetrySink sink = new TelemetrySink(configuration);
            var channel = new StubTelemetryChannel2();
            sink.TelemetryChannel = channel;
            StubTelemetryProcessor2 processor = null;
            sink.TelemetryProcessorChainBuilder.Use(next =>
            {
                processor = new StubTelemetryProcessor2(next);
                return processor;
            });
            sink.Initialize(configuration);
#endregion

        [TestMethod]
        public void InitializeIsMarkesAsInternalSdkOperation()
        {
            bool isInternalOperation = false;

            StubConfigurableWithStaticCallback.OnInitialize = (item) => { isInternalOperation = SdkInternalOperationsMonitor.IsEntered(); };

            Assert.AreEqual(false, SdkInternalOperationsMonitor.IsEntered());
                );

            using (var modules = new TestableTelemetryModules())
            {
                new TestableTelemetryConfigurationFactory().Initialize(new TelemetryConfiguration(), modules, configFileContents);

                Assert.AreEqual(true, isInternalOperation);
                Assert.AreEqual(false, SdkInternalOperationsMonitor.IsEntered());
            }
        }
            {
                TelemetryConfigurationFactory.LoadInstances(definition, instances, modules);
            }

            public static new void LoadProperties(XElement definition, object instance, TelemetryModules modules)
            {
                TelemetryConfigurationFactory.LoadProperties(definition, instance, modules);
            }
        }


            public EventLevel EnumProperty { get; set; }
        }

        private class StubConfigurable : ITelemetryModule
        {
            public TelemetryConfiguration Configuration { get; set; }

            public bool Initialized { get; set; }

                this.Initialized = true;
            }
        }

        private class StubConfigurableWithStaticCallback : ITelemetryModule
        {
            /// <summary>
            /// Gets or sets the callback invoked by the <see cref="Initialize"/> method.
            /// </summary>
            public static Action<TelemetryConfiguration> OnInitialize = item => { };

            public void Initialize(TelemetryConfiguration configuration)
            {
                OnInitialize(configuration);
            }
        }

        private class StubConfigurableTelemetryInitializer : StubConfigurable, ITelemetryInitializer
        {
            public void Initialize(ITelemetry telemetry)

            public string StringProperty { get; set; }

            public TelemetryConfiguration Configuration { get; set; }

            public Action<TelemetryConfiguration> OnInitialize { get; set; }

            public void Initialize(TelemetryConfiguration configuration)
            {
                if (this.OnInitialize != null)
                this.next = next;
            }

            public void Process(ITelemetry telemetry) {  }

            public void Initialize(TelemetryConfiguration config)
            {
                this.Initialized = true;
            }
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
