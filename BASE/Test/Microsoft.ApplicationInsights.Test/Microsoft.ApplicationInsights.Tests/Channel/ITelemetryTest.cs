namespace Microsoft.ApplicationInsights.Channel
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Microsoft.ApplicationInsights.DataContracts;
    using AI;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    internal class ITelemetryTest<TTelemetry, TEndpointData> 
        where TTelemetry : ITelemetry, new()
        where TEndpointData : Domain
    {
        public void Run()
        {
            this.ClassShouldBePublic();
            this.ClassShouldHaveDefaultConstructorToSupportTelemetryContext();
            this.ClassShouldHaveParameterizedConstructorToSimplifyCreationOfValidTelemetryInstancesInUserCode();
            this.ClassShouldImplementISupportCustomPropertiesIfItDefinesPropertiesProperty();
            this.TestProperties();
            this.TestExtension();
            this.SerializeWritesTimestampAsExpectedByEndpoint();
            this.SerializeWritesSequenceAsExpectedByEndpoint();
            this.SerializeWritesInstrumentationKeyAsExpectedByEndpoint();
            this.SerializeWritesTelemetryNameAsExpectedByEndpoint();
            this.SerializeWritesDataBaseTypeAsExpectedByEndpoint();
        }

        private void TestExtension()
        {
            // Extention field exists
            var extensionField = typeof(TTelemetry).GetRuntimeProperties().Any(p => p.Name == "Extension");
            Assert.IsNotNull(extensionField);
            
            TTelemetry tel = new TTelemetry();
            Assert.IsNull(tel.Extension, "Extension should be null by default");

            {
                this.TestStringProperty(property);
            }
            else if (property.PropertyType == typeof(int))
            {
                this.TestIntProperty(property);
            }
            else if (property.PropertyType == typeof(double))
            {
                this.TestDoubleProperty(property);
            {
                this.TestDateTimeOffsetProperty(property);
            }
            else if (property.PropertyType == typeof(TelemetryContext))
            {
                this.TestTelemetryContextProperty(property);
            }
        }
        
        private void PropertyShouldNotBeNullByDefaultToPreventNullReferenceExceptions(PropertyInfo property)
        {
            try
            {
                var instance = new TTelemetry();
                var actualValue = property.GetValue(instance, null);
                Assert.IsNotNull(actualValue, nameof(TTelemetry) + "." + property.Name + " should not be null by default to prevent NullReferenceException.");
            }
            catch (TargetInvocationException e)
            {
                Assert.Fail(nameof(TTelemetry) + "." + property.Name + " should not be null by default to prevent NullReferenceException." + e.InnerException.Message);
            }
        }

        private void PropertySetterShouldThrowException(PropertyInfo property, object invalidValue, Type expectedException)
        {
            if (property.CanWrite)
            {
                var instance = new TTelemetry();
                try
                {
                var instance = new TTelemetry();
                property.SetValue(instance, value, null);
                Assert.AreEqual(value, property.GetValue(instance, null), nameof(TTelemetry) + "." + property.Name + " setter should change property value.");
            }
        }

        private void TestTelemetryContextProperty(PropertyInfo property)
        {
            this.PropertyShouldNotBeNullByDefaultToPreventNullReferenceExceptions(property);
        }
        }

        private void TestDoubleProperty(PropertyInfo property)
        {
            this.PropertySetterShouldChangePropertyValue(property, 4.2);
        }

        private void TestIntProperty(PropertyInfo property)
        {
            this.PropertySetterShouldChangePropertyValue(property, "TestValue");
        }

        private void ClassShouldBePublic()
        {
            Assert.IsTrue(typeof(TTelemetry).GetTypeInfo().IsPublic, nameof(TTelemetry) + " should be public to allow instantiation in user code.");
        }

        private void ClassShouldHaveDefaultConstructorToSupportTelemetryContext()
        {
                nameof(TTelemetry) + " should have default constructor to support TelemetryContext.");
        }

        private void ClassShouldHaveParameterizedConstructorToSimplifyCreationOfValidTelemetryInstancesInUserCode()
        {
            Assert.IsTrue(
                typeof(TTelemetry).GetTypeInfo().DeclaredConstructors.Any(c => c.GetParameters().Length > 0),
                nameof(TTelemetry) + " should have a parameterized constructor to simplify creation of valid telemetry in user code.");
        }

        private void ClassShouldImplementISupportCustomPropertiesIfItDefinesPropertiesProperty()
        {            
            if (typeof(TTelemetry).GetRuntimeProperties().Any(p => p.Name == "Properties"))
            {
                Assert.IsTrue(typeof(ISupportProperties).GetTypeInfo().IsAssignableFrom(typeof(TTelemetry).GetTypeInfo()));
            }
        }

        private void SerializeWritesTimestampAsExpectedByEndpoint()
        {
            var expected = new TTelemetry { Timestamp = DateTimeOffset.UtcNow };
            expected.Sanitize();

            TelemetryItem<TEndpointData> actual = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<TEndpointData>(expected);


            Assert.AreEqual<DateTimeOffset>(expected.Timestamp, DateTimeOffset.Parse(actual.time, null, System.Globalization.DateTimeStyles.AssumeUniversal));
        }

        private void SerializeWritesSequenceAsExpectedByEndpoint()
        {
            var expected = new TTelemetry();
            expected.Context.InstrumentationKey = "312CBD79-9DBB-4C48-A7DA-3CC2A931CB71";
            expected.Sanitize();

            TelemetryItem<TEndpointData> actual = TelemetryItemTestHelper
                .SerializeDeserializeTelemetryItem<TEndpointData>(expected);

            Assert.AreEqual(
                this.ExtractEnvelopeNameFromType(typeof(TTelemetry)),
            }
#pragma warning disable 618
            else if (telemetryType == typeof(SessionStateTelemetry))
            {
                // handle TraceTelemetry separately
                result = "Event";
            }
            else if (telemetryType == typeof(PerformanceCounterTelemetry))
            {
                // handle TraceTelemetry separately

        private string ExtractEnvelopeNameFromType(Type telemetryType)
        {
            string result;

            if (telemetryType == typeof(MetricTelemetry))
            {
                result = ItemType.Metric;
            }
            else if (telemetryType == typeof(RequestTelemetry))
            }
            else if (telemetryType == typeof(TraceTelemetry))
            {
                result = ItemType.Message;
            }
            else if (telemetryType == typeof(EventTelemetry))
            {
                result = ItemType.Event;
            }
            else if (telemetryType == typeof(PageViewTelemetry))


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
