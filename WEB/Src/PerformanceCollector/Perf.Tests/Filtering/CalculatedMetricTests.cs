namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Linq;

    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Filtering;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CalculatedMetricTests
    {
        [TestMethod]
        public void CalculatedMetricFiltersCorrectly()
        {
            // ARRANGE
            var filterInfo1 = new FilterInfo() { FieldName = "Name", Predicate = Predicate.Contains, Comparand = "dog" };
            var filterInfo2 = new FilterInfo() { FieldName = "Name", Predicate = Predicate.Contains, Comparand = "cat" };
            var metricInfo = new CalculatedMetricInfo()
            {
                Id = "Metric1",
                TelemetryType = TelemetryType.Request,
                Projection = "Name",
                Aggregation = AggregationType.Sum,
                FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfo1, filterInfo2 } } }
            };

            var telemetryThatMustPass = new RequestTelemetry() { Name = "Both the words 'dog' and 'CAT' are here, which satisfies both filters" };
            var telemetryThatMustFail1 = new RequestTelemetry() { Name = "This value only contains the word 'dog', but not the other one" };
            var telemetryThatMustFail2 = new RequestTelemetry() { Name = "This value only contains the word 'cat', but not the other one" };

            // ACT
            CollectionConfigurationError[] errors;
            var metric = new CalculatedMetric<RequestTelemetry>(metricInfo, out errors);

            // ASSERT
            Assert.AreEqual(0, errors.Length);

            Assert.IsTrue(metric.CheckFilters(telemetryThatMustPass, out errors));
            Assert.AreEqual(0, errors.Length);

            Assert.IsFalse(metric.CheckFilters(telemetryThatMustFail1, out errors));
            Assert.AreEqual(0, errors.Length);

            Assert.IsFalse(metric.CheckFilters(telemetryThatMustFail2, out errors));
            Assert.AreEqual(0, errors.Length);
        }

        [TestMethod]
        public void CalculatedMetricHandlesNoFiltersCorrectly()
        {
            // ARRANGE
            var metricInfo = new CalculatedMetricInfo()
            {
                Id = "Metric1",
            CollectionConfigurationError[] errors;
            var metric = new CalculatedMetric<RequestTelemetry>(metricInfo, out errors);

            // ASSERT
            Assert.AreEqual(0, errors.Length);

            Assert.IsTrue(metric.CheckFilters(telemetryThatMustPass, out errors));
            Assert.AreEqual(0, errors.Length);
        }

        [TestMethod]
        public void CalculatedMetricHandlesNullFiltersCorrectly()
        {
            // ARRANGE
            var metricInfo = new CalculatedMetricInfo()
            {
                Id = "Metric1",
                TelemetryType = TelemetryType.Request,
                Projection = "Name",
                Aggregation = AggregationType.Sum,

            // ACT
            CollectionConfigurationError[] errors;
            var metric = new CalculatedMetric<RequestTelemetry>(metricInfo, out errors);

            // ASSERT
            Assert.AreEqual(0, errors.Length);

            Assert.IsTrue(metric.CheckFilters(telemetryThatMustPass, out errors));
            Assert.AreEqual(0, errors.Length);

        [TestMethod]
        public void CalculatedMetricPerformsLogicalConnectionsBetweenFiltersCorrectly()
        {
            // ARRANGE
            var filterInfoDog = new FilterInfo() { FieldName = "Name", Predicate = Predicate.Contains, Comparand = "dog" };
            var filterInfoCat = new FilterInfo() { FieldName = "Name", Predicate = Predicate.Contains, Comparand = "cat" };
            var filterInfoApple = new FilterInfo() { FieldName = "Name", Predicate = Predicate.Contains, Comparand = "apple" };
            var filterInfoOrange = new FilterInfo() { FieldName = "Name", Predicate = Predicate.Contains, Comparand = "orange" };
            var metricInfo = new CalculatedMetricInfo()
                    }
            };

            var telemetryThatMustPass1 = new RequestTelemetry() { Name = "Both the words 'dog' and 'CAT' are here, which satisfies the first OR." };
            var telemetryThatMustPass2 = new RequestTelemetry() { Name = "Both the words 'apple' and 'ORANGE' are here, which satisfies the second OR." };
            var telemetryThatMustPass3 = new RequestTelemetry() { Name = "All four words are here: 'dog', 'cat', 'apple', and 'orange'!" };
            var telemetryThatMustFail1 = new RequestTelemetry() { Name = "This value only contains the words 'dog' and 'apple', which is not enough to satisfy any of the OR conditions." };
            var telemetryThatMustFail2 = new RequestTelemetry() { Name = "This value only contains the word 'cat' and 'orange', which is not enough to satisfy any of the OR conditions." };
            var telemetryThatMustFail3 = new RequestTelemetry() { Name = "None of the words are here!" };

            Assert.IsFalse(metric.CheckFilters(telemetryThatMustFail1, out errors));
            Assert.AreEqual(0, errors.Length);

            Assert.IsFalse(metric.CheckFilters(telemetryThatMustFail2, out errors));
            Assert.AreEqual(0, errors.Length);

            Assert.IsFalse(metric.CheckFilters(telemetryThatMustFail3, out errors));
            Assert.AreEqual(0, errors.Length);
        }

        [TestMethod]
        public void CalculatedMetricProjectsCorrectly()
        {
            // ARRANGE
            var metricInfo = new CalculatedMetricInfo()
            {
                Id = "Metric1",
                TelemetryType = TelemetryType.Request,
                Projection = "Id",
                Aggregation = AggregationType.Sum,
            var metricInfo = new CalculatedMetricInfo()
            {
                Id = "Metric1",
                TelemetryType = TelemetryType.Request,
                Projection = "CustomDimensions.Dimension1",
                Aggregation = AggregationType.Sum,
                FilterGroups = new FilterConjunctionGroupInfo[0]
            };

        public void CalculatedMetricProjectsCorrectlyWhenCount()
        {
            // ARRANGE
            var metricInfo = new CalculatedMetricInfo()
            {
                Id = "Metric1",
                TelemetryType = TelemetryType.Request,
                Projection = "COUNT()",
                Aggregation = AggregationType.Sum,
                FilterGroups = new FilterConjunctionGroupInfo[0]

            // ASSERT
            Assert.AreEqual(AggregationType.Sum, metric.AggregationType);
            Assert.AreEqual(0, errors.Length);
            Assert.AreEqual(123.56, projection);
        }

        [TestMethod]
        public void CalculatedMetricProjectsCorrectlyWhenTimeSpan()
        {
            // ARRANGE
            var metricInfo = new CalculatedMetricInfo()
            {
                Id = "Metric1",
                TelemetryType = TelemetryType.Request,
                Projection = "Duration",
                Aggregation = AggregationType.Avg,
                FilterGroups = new FilterConjunctionGroupInfo[0]
            };

        }
        
        [TestMethod]
        public void CalculatedMetricReportsErrorsForInvalidFilters()
        {
            // ARRANGE
            var filterInfo1 = new FilterInfo() { FieldName = "Name", Predicate = Predicate.Equal, Comparand = "Sky" };
            var filterInfo2 = new FilterInfo() { FieldName = "NonExistentField", Predicate = Predicate.Equal, Comparand = "Comparand" };
            var metricInfo = new CalculatedMetricInfo()
            {
                Id = "Metric1",
                TelemetryType = TelemetryType.Request,
                Projection = "Name",
                Aggregation = AggregationType.Avg,
                FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfo1, filterInfo2 } } }
            };

            // ACT
            CollectionConfigurationError[] errors;
            var metric = new CalculatedMetric<RequestTelemetry>(metricInfo, out errors);

            // ASSERT
            Assert.AreEqual(CollectionConfigurationErrorType.FilterFailureToCreateUnexpected, errors.Single().ErrorType);
            Assert.AreEqual(
                "Failed to create a filter NonExistentField Equal Comparand.",
                errors.Single().Message);
            Assert.IsTrue(errors.Single().FullException.Contains("Error finding property NonExistentField in the type Microsoft.ApplicationInsights.DataContracts.RequestTelemetry"));
            Assert.AreEqual(4, errors[0].Data.Count);
            Assert.AreEqual("Metric1", errors[0].Data["MetricId"]);
            Assert.AreEqual("NonExistentField", errors[0].Data["FilterFieldName"]);
            Assert.AreEqual(Predicate.Equal.ToString(), errors[0].Data["FilterPredicate"]);
            Assert.AreEqual("Comparand", errors[0].Data["FilterComparand"]);

            // we must be left with the one valid filter only
            Assert.IsTrue(metric.CheckFilters(new RequestTelemetry() { Name = "sky" }, out errors));
            Assert.AreEqual(0, errors.Length);

            Assert.IsFalse(metric.CheckFilters(new RequestTelemetry() { Name = "sky1" }, out errors));
            Assert.AreEqual(0, errors.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CalculatedMetricThrowsWhenInvalidProjection()
        {
            // ARRANGE
            var metricInfo = new CalculatedMetricInfo()
            {
                Id = "Metric1",
                TelemetryType = TelemetryType.Request,
                // ASSERT
                Assert.IsTrue(e.ToString().Contains("Id"));
                return;
            }

            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
            {
                Id = "Metric1",
                TelemetryType = TelemetryType.Request,
                Projection = "*",
                Aggregation = AggregationType.Sum,
                FilterGroups = new FilterConjunctionGroupInfo[0]
            };

            // ACT
            CollectionConfigurationError[] errors;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
