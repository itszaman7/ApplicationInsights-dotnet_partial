#if NET452
namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Linq;

    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Filtering;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.ServiceContract;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
    using Microsoft.ApplicationInsights.Web.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CollectionConfigurationTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CollectionConfigurationThrowsOnNullInput()
        {
            // ARRANGE

            // ACT
            CollectionConfigurationError[] errors;
            new CollectionConfiguration(null, out errors, new ClockMock());

            // ASSERT
        }

        [TestMethod]
        public void CollectionConfigurationCreatesMetrics()
        {
            // ARRANGE
            CollectionConfigurationError[] errors;
            var filters = new[]
            {
                new FilterConjunctionGroupInfo()
                {
                    Filters = new[] { new FilterInfo() { FieldName = "Name", Predicate = Predicate.Equal, Comparand = "Request1" } }
                }
            };

            var metrics = new[]
            {
                new CalculatedMetricInfo()
                {
                    Id = "Metric0",
                    TelemetryType = TelemetryType.Request,
                    Projection = "Name",
                    Aggregation = AggregationType.Avg,
                    FilterGroups = filters
                },
                new CalculatedMetricInfo()
                {
                    Id = "Metric1",
                    TelemetryType = TelemetryType.Request,
                    Projection = "Id",
                    Aggregation = AggregationType.Sum,
                    FilterGroups = filters
                },
                new CalculatedMetricInfo()
                {
                    Id = "Metric2",
                    TelemetryType = TelemetryType.Dependency,
                    Projection = "Name",
                    Aggregation = AggregationType.Avg,
                    FilterGroups = filters
                },
                new CalculatedMetricInfo()
                {
                    Id = "Metric3",
                    TelemetryType = TelemetryType.Exception,
                    Projection = "Message",
                    Aggregation = AggregationType.Avg,
                    FilterGroups = filters
                },
                new CalculatedMetricInfo()
                {
                    Id = "Metric4",
                    TelemetryType = TelemetryType.Event,
                    Projection = "Name",
                    Aggregation = AggregationType.Avg,
                    FilterGroups = filters
                },
                new CalculatedMetricInfo()
                {
                    Id = "Metric5",
                    TelemetryType = TelemetryType.Trace,
                    Projection = "Message",
                    Aggregation = AggregationType.Avg,
                    FilterGroups = filters
                }
            };

            // ACT
            var collectionConfiguration = new CollectionConfiguration(
                new CollectionConfigurationInfo() { Metrics = metrics },
                out errors,
                new ClockMock());

            // ASSERT
            Assert.AreEqual(6, collectionConfiguration.TelemetryMetadata.Count());

            Assert.AreEqual("Metric0", collectionConfiguration.RequestMetrics.First().Id);
            Assert.AreEqual("Metric1", collectionConfiguration.RequestMetrics.Last().Id);
            Assert.AreEqual("Metric2", collectionConfiguration.DependencyMetrics.Single().Id);
            Assert.AreEqual("Metric3", collectionConfiguration.ExceptionMetrics.Single().Id);
            Assert.AreEqual("Metric4", collectionConfiguration.EventMetrics.Single().Id);
            Assert.AreEqual("Metric5", collectionConfiguration.TraceMetrics.Single().Id);
        }

        [TestMethod]
        public void CollectionConfigurationReportsPerformanceCountersWithDuplicateIds()
        {
            // ARRANGE
            CollectionConfigurationError[] errors;
            var filter1 = new FilterInfo() { FieldName = "Name", Predicate = Predicate.Equal, Comparand = "Request1" };
            var filter2 = new FilterInfo() { FieldName = "Name", Predicate = Predicate.Equal, Comparand = "Request1" };
            var metrics = new[]
            {
                new CalculatedMetricInfo()
                {
                    Id = "PerformanceCounter1",
                    TelemetryType = TelemetryType.PerformanceCounter,
                    Projection = @"\SomeObject\SomeCounter",
                    Aggregation = AggregationType.Avg,
                    FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filter1 } } }
                },
                new CalculatedMetricInfo()
                {
                    Id = "PerformanceCounter1",
                    Projection = @"\SomeObject(SomeInstance)\SomeCounter",
                    Aggregation = AggregationType.Avg,
                    FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filter2 } } }
                }
            };

            // ACT
            var collectionConfiguration = new CollectionConfiguration(
                new CollectionConfigurationInfo() { ETag = "ETag1", Metrics = metrics },
                out errors,
        }

        [TestMethod]
        public void CollectionConfigurationReportsMetricsWithDuplicateIds()
        {
            // ARRANGE
            CollectionConfigurationError[] errors;
            var filter1 = new FilterInfo() { FieldName = "Name", Predicate = Predicate.Equal, Comparand = "Request1" };
            var filter2 = new FilterInfo() { FieldName = "Name", Predicate = Predicate.Equal, Comparand = "Request1" };
            var metrics = new[]
            {
                new CalculatedMetricInfo()
                {
                    Id = "Metric1",
                    TelemetryType = TelemetryType.Request,
                    Projection = "Name",
                    Aggregation = AggregationType.Avg,
                    FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filter1 } } }
                },
                new CalculatedMetricInfo()
                    Id = "Metric2",
                    TelemetryType = TelemetryType.Request,
                    Projection = "Name",
                    Aggregation = AggregationType.Avg,
                    FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filter2 } } }
                }
            };

            // ACT
            var collectionConfiguration = new CollectionConfiguration(
                new CollectionConfigurationInfo() { ETag = "ETag1", Metrics = metrics },
                out errors,
                new ClockMock());

            // ASSERT
            Assert.AreEqual(2, collectionConfiguration.RequestMetrics.Count());
            Assert.AreEqual("Metric1", collectionConfiguration.TelemetryMetadata.First().Item1);
            Assert.AreEqual("Metric2", collectionConfiguration.TelemetryMetadata.Last().Item1);

            Assert.AreEqual(CollectionConfigurationErrorType.MetricDuplicateIds, errors.Single().ErrorType);
            Assert.AreEqual("Metric with a duplicate id ignored: Metric1", errors.Single().Message);
            Assert.AreEqual(string.Empty, errors.Single().FullException);
            Assert.AreEqual(2, errors.Single().Data.Count);
            Assert.AreEqual("Metric1", errors.Single().Data["MetricId"]);
            Assert.AreEqual("ETag1", errors.Single().Data["ETag"]);
        }

        [TestMethod]
        public void CollectionConfigurationReportsInvalidFilterForMetric()
        {
                    Projection = "Name",
                    Aggregation = AggregationType.Avg,
                    FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfo } } }
                }
            };

            // ACT
            var collectionConfiguration = new CollectionConfiguration(
                new CollectionConfigurationInfo() { Metrics = metrics },
                out errors,
                errors.Single()
                    .FullException.Contains(
                        "Error finding property NonExistentFieldName in the type Microsoft.ApplicationInsights.DataContracts.RequestTelemetry"));
            Assert.AreEqual(5, errors.Single().Data.Count);
            Assert.AreEqual("Metric1", errors.Single().Data["MetricId"]);
            Assert.AreEqual(null, errors.Single().Data["ETag"]);
            Assert.AreEqual("NonExistentFieldName", errors.Single().Data["FilterFieldName"]);
            Assert.AreEqual(Predicate.Equal.ToString(), errors.Single().Data["FilterPredicate"]);
            Assert.AreEqual("Request", errors.Single().Data["FilterComparand"]);
        }

        [TestMethod]
        public void CollectionConfigurationReportsInvalidMetric()
        {
            // ARRANGE
            CollectionConfigurationError[] errors;
            var filterInfo = new FilterInfo() { FieldName = "Name", Predicate = Predicate.Equal, Comparand = "Request" };
            var metrics = new[]
            {
                new CalculatedMetricInfo()
            Assert.AreEqual(0, collectionConfiguration.TelemetryMetadata.Count());

            Assert.AreEqual(CollectionConfigurationErrorType.MetricFailureToCreate, errors.Single().ErrorType);
            Assert.AreEqual(
                "Failed to create metric Id: 'Metric1', TelemetryType: 'Request', Projection: 'NonExistentFieldName', Aggregation: 'Avg', FilterGroups: [Name Equal Request].",
                errors.Single().Message);
            Assert.IsTrue(errors.Single().FullException.Contains("Could not construct the projection"));
            Assert.AreEqual(2, errors.Single().Data.Count);
            Assert.AreEqual("Metric1", errors.Single().Data["MetricId"]);
            Assert.AreEqual("ETag1", errors.Single().Data["ETag"]);
        }

        [TestMethod]
        public void CollectionConfigurationReportsMultipleInvalidFiltersAndMetrics()
        {
            // ARRANGE
            CollectionConfigurationError[] errors;
            var filterInfo1 = new FilterInfo() { FieldName = "NonExistentFilterFieldName1", Predicate = Predicate.Equal, Comparand = "Request" };
            var filterInfo2 = new FilterInfo() { FieldName = "NonExistentFilterFieldName2", Predicate = Predicate.Equal, Comparand = "Request" };
            var metrics = new[]
                    Projection = "NonExistentProjectionName2",
                    Aggregation = AggregationType.Avg,
                    FilterGroups = new[] { new FilterConjunctionGroupInfo() { Filters = new[] { filterInfo1, filterInfo2 } } }
                }
            };

            // ACT
            var collectionConfiguration = new CollectionConfiguration(
                new CollectionConfigurationInfo() { ETag = "ETag1", Metrics = metrics },
                out errors,
            Assert.AreEqual(0, collectionConfiguration.TelemetryMetadata.Count());

            Assert.AreEqual(6, errors.Length);

            Assert.AreEqual(CollectionConfigurationErrorType.FilterFailureToCreateUnexpected, errors[0].ErrorType);
            Assert.AreEqual("Failed to create a filter NonExistentFilterFieldName1 Equal Request.", errors[0].Message);
            Assert.IsTrue(
                errors[0].FullException.Contains(
                    "Error finding property NonExistentFilterFieldName1 in the type Microsoft.ApplicationInsights.DataContracts.RequestTelemetry"));
            Assert.AreEqual(5, errors[0].Data.Count);
            Assert.AreEqual("Metric1", errors[1].Data["MetricId"]);
            Assert.AreEqual("ETag1", errors[1].Data["ETag"]);
            Assert.AreEqual("NonExistentFilterFieldName2", errors[1].Data["FilterFieldName"]);
            Assert.AreEqual(Predicate.Equal.ToString(), errors[1].Data["FilterPredicate"]);
            Assert.AreEqual("Request", errors[1].Data["FilterComparand"]);

            Assert.AreEqual(CollectionConfigurationErrorType.MetricFailureToCreate, errors[2].ErrorType);
            Assert.AreEqual(
                "Failed to create metric Id: 'Metric1', TelemetryType: 'Request', Projection: 'NonExistentProjectionName1', Aggregation: 'Avg', FilterGroups: [NonExistentFilterFieldName1 Equal Request, NonExistentFilterFieldName2 Equal Request].",
                errors[2].Message);
            Assert.AreEqual("Metric1", errors[2].Data["MetricId"]);
            Assert.AreEqual("ETag1", errors[2].Data["ETag"]);

            Assert.AreEqual(CollectionConfigurationErrorType.FilterFailureToCreateUnexpected, errors[3].ErrorType);
            Assert.AreEqual("Failed to create a filter NonExistentFilterFieldName1 Equal Request.", errors[3].Message);
            Assert.IsTrue(
                errors[3].FullException.Contains(
                    "Error finding property NonExistentFilterFieldName1 in the type Microsoft.ApplicationInsights.DataContracts.RequestTelemetry"));
            Assert.AreEqual(5, errors[3].Data.Count);
            Assert.AreEqual("Metric2", errors[3].Data["MetricId"]);
                    "Error finding property NonExistentFilterFieldName2 in the type Microsoft.ApplicationInsights.DataContracts.RequestTelemetry"));
            Assert.AreEqual(5, errors[4].Data.Count);
            Assert.AreEqual("Metric2", errors[4].Data["MetricId"]);
            Assert.AreEqual("ETag1", errors[4].Data["ETag"]);
            Assert.AreEqual("NonExistentFilterFieldName2", errors[4].Data["FilterFieldName"]);
            Assert.AreEqual(Predicate.Equal.ToString(), errors[4].Data["FilterPredicate"]);
            Assert.AreEqual("Request", errors[4].Data["FilterComparand"]);

            Assert.AreEqual(CollectionConfigurationErrorType.MetricFailureToCreate, errors[5].ErrorType);
            Assert.AreEqual(
                "Failed to create metric Id: 'Metric2', TelemetryType: 'Request', Projection: 'NonExistentProjectionName2', Aggregation: 'Avg', FilterGroups: [NonExistentFilterFieldName1 Equal Request, NonExistentFilterFieldName2 Equal Request].",
                errors[5].Message);
            Assert.IsTrue(errors[5].FullException.Contains("Could not construct the projection"));
            Assert.AreEqual(2, errors[5].Data.Count);
            Assert.AreEqual("Metric2", errors[5].Data["MetricId"]);
            Assert.AreEqual("ETag1", errors[5].Data["ETag"]);
        }

        [TestMethod]
        public void CollectionConfigurationCreatesDocumentStreams()
        {
            // ARRANGE
            CollectionConfigurationError[] errors;
            var documentStreamInfos = new[]
            {
                new DocumentStreamInfo()
                {
                    Id = "Stream1",
                    DocumentFilterGroups =
                        new[]
                        }
                },
                new DocumentStreamInfo()
                {
                    Id = "Stream4",
                    DocumentFilterGroups =
                        new[]
                        {
                            new DocumentFilterConjunctionGroupInfo()
                            {
                                TelemetryType = TelemetryType.Event,
                                Filters = new FilterConjunctionGroupInfo() { Filters = new FilterInfo[0] }
                            }
                        }
                }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo() { DocumentStreams = documentStreamInfos, ETag = "ETag1" };

            // ACT
            var collectionConfiguration = new CollectionConfiguration(collectionConfigurationInfo, out errors, new ClockMock());

            // ASSERT
            DocumentStream[] documentStreams = collectionConfiguration.DocumentStreams.ToArray();
            Assert.AreEqual(4, documentStreams.Length);
            Assert.AreEqual("Stream1", documentStreams[0].Id);
            Assert.AreEqual("Stream2", documentStreams[1].Id);
            Assert.AreEqual("Stream3", documentStreams[2].Id);
            Assert.AreEqual("Stream4", documentStreams[3].Id);
        }

        [TestMethod]
        public void CollectionConfigurationCarriesOverQuotaWhenCreatingDocumentStreams()
        {
            // ARRANGE
            var timeProvider = new ClockMock();

            CollectionConfigurationError[] errors;
            var oldDocumentStreamInfos = new[]
            {
                            {
                                TelemetryType = TelemetryType.Request,
                                Filters = new FilterConjunctionGroupInfo() { Filters = new FilterInfo[0] }
                            }
                        }
                },
                new DocumentStreamInfo()
                {
                    Id = "Stream2",
                    DocumentFilterGroups =
                        new[]
                        {
                            new DocumentFilterConjunctionGroupInfo()
                            {
                                TelemetryType = TelemetryType.Dependency,
                                Filters = new FilterConjunctionGroupInfo() { Filters = new FilterInfo[0] }
                            }
                        }
                },
                new DocumentStreamInfo()
                            {
                                TelemetryType = TelemetryType.Request,
                                Filters = new FilterConjunctionGroupInfo() { Filters = new FilterInfo[0] }
                            }
                        }
                },
                new DocumentStreamInfo()
                {
                    Id = "Stream2",
                    DocumentFilterGroups =
                            new DocumentFilterConjunctionGroupInfo()
                            {
                                TelemetryType = TelemetryType.Dependency,
                                Filters = new FilterConjunctionGroupInfo() { Filters = new FilterInfo[0] }
                            }
                        }
                },
                new DocumentStreamInfo()
                {
                            new DocumentFilterConjunctionGroupInfo()
                            {
                                TelemetryType = TelemetryType.Event,
                                Filters = new FilterConjunctionGroupInfo() { Filters = new FilterInfo[0] }
                            }
                        }
                },
                new DocumentStreamInfo()
                {
                    Id = "Stream5",
            var oldDocumentStreamInfos = new[]
            {
                new DocumentStreamInfo()
                {
                    Id = "Stream1",
                    DocumentFilterGroups =
                        new[]
                        {
                            new DocumentFilterConjunctionGroupInfo()
                            {
                },
                new DocumentStreamInfo()
                {
                    Id = "Stream2",
                    DocumentFilterGroups =
                        new[]
                        {
                            new DocumentFilterConjunctionGroupInfo()
                            {
                                TelemetryType = TelemetryType.Dependency,
                                Filters = new FilterConjunctionGroupInfo() { Filters = new FilterInfo[0] }
                            }
                        }
                }
            };

            var newDocumentStreamInfos = new[]
            {
                new DocumentStreamInfo()
                {
                    Id = "Stream1",
                    DocumentFilterGroups =
                        new[]
                        {
                            new DocumentFilterConjunctionGroupInfo()
                            {
                                TelemetryType = TelemetryType.Request,
                                Filters = new FilterConjunctionGroupInfo() { Filters = new FilterInfo[0] }
                            }
                        }
                },
                new DocumentStreamInfo()
                {
                    Id = "Stream2",
                    DocumentFilterGroups =
                        new[]
                        {
                            new DocumentFilterConjunctionGroupInfo()
                            {
                                TelemetryType = TelemetryType.Dependency,
                            }
                        }
                },
                new DocumentStreamInfo()
                {
                    Id = "Stream3",
                    DocumentFilterGroups =
                        new[]
                        {
                            new DocumentFilterConjunctionGroupInfo()
                    Id = "Stream4",
                    DocumentFilterGroups =
                        new[]
                        {
                            new DocumentFilterConjunctionGroupInfo()
                            {
                                TelemetryType = TelemetryType.Event,
                                Filters = new FilterConjunctionGroupInfo() { Filters = new FilterInfo[0] }
                            }
                        }
            // ACT
            // the initial quota is 3
            telemetryProcessor.Process(new RequestTelemetry() { Context = { InstrumentationKey = "some ikey" } });

            telemetryProcessor.Process(new DependencyTelemetry() { Context = { InstrumentationKey = "some ikey" } });
            telemetryProcessor.Process(new DependencyTelemetry() { Context = { InstrumentationKey = "some ikey" } });

            telemetryProcessor.Process(new ExceptionTelemetry() { Context = { InstrumentationKey = "some ikey" } });
            telemetryProcessor.Process(new ExceptionTelemetry() { Context = { InstrumentationKey = "some ikey" } });
            telemetryProcessor.Process(new ExceptionTelemetry() { Context = { InstrumentationKey = "some ikey" } });
            // ACT
            // the new configuration must carry the quotas over from the old one (only for those document streams that already existed)
            var newCollectionConfigurationInfo = new CollectionConfigurationInfo() { DocumentStreams = newDocumentStreamInfos, ETag = "ETag1" };
            var newCollectionConfiguration = new CollectionConfiguration(
                newCollectionConfigurationInfo,
                out errors,
                timeProvider,
                oldCollectionConfiguration.DocumentStreams);

            // ASSERT
            Assert.AreEqual("Stream1", oldDocumentStreams[0].Id);
            Assert.AreEqual(9, oldDocumentStreams[0].RequestQuotaTracker.CurrentQuota);
            Assert.AreEqual(10, oldDocumentStreams[0].DependencyQuotaTracker.CurrentQuota);
            Assert.AreEqual(10, oldDocumentStreams[0].ExceptionQuotaTracker.CurrentQuota);
            Assert.AreEqual(10, oldDocumentStreams[0].EventQuotaTracker.CurrentQuota);
            Assert.AreEqual(10, oldDocumentStreams[0].EventQuotaTracker.CurrentQuota);

            Assert.AreEqual("Stream2", oldDocumentStreams[1].Id);
            Assert.AreEqual(10, oldDocumentStreams[1].RequestQuotaTracker.CurrentQuota);
            Assert.AreEqual(8, oldDocumentStreams[1].DependencyQuotaTracker.CurrentQuota);
            Assert.AreEqual(10, newDocumentStreams[0].ExceptionQuotaTracker.CurrentQuota);
            Assert.AreEqual(10, newDocumentStreams[0].EventQuotaTracker.CurrentQuota);
            Assert.AreEqual(10, newDocumentStreams[0].EventQuotaTracker.CurrentQuota);

            Assert.AreEqual("Stream2", newDocumentStreams[1].Id);
            Assert.AreEqual(10, newDocumentStreams[1].RequestQuotaTracker.CurrentQuota);
            Assert.AreEqual(8, newDocumentStreams[1].DependencyQuotaTracker.CurrentQuota);
            Assert.AreEqual(10, newDocumentStreams[1].ExceptionQuotaTracker.CurrentQuota);
            Assert.AreEqual(10, newDocumentStreams[1].EventQuotaTracker.CurrentQuota);
            Assert.AreEqual(10, newDocumentStreams[1].EventQuotaTracker.CurrentQuota);
        {
            // ARRANGE
            CollectionConfigurationError[] errors;
            var documentStreamInfos = new[]
            {
                new DocumentStreamInfo()
                {
                    Id = "Stream1",
                    DocumentFilterGroups =
                        new[]
                        {
                            new DocumentFilterConjunctionGroupInfo()
                            {
                                TelemetryType = TelemetryType.Request,
                                Filters = new FilterConjunctionGroupInfo() { Filters = new FilterInfo[0] }
                            }
                        }
                },
                new DocumentStreamInfo()
                {
                            {
                                TelemetryType = TelemetryType.Dependency,
                                Filters = new FilterConjunctionGroupInfo() { Filters = new FilterInfo[0] }
                            }
                        }
                },
                new DocumentStreamInfo()
                {
                    Id = "Stream2",
                    DocumentFilterGroups =
            Assert.AreEqual("Stream1", errors.Single().Data["DocumentStreamId"]);
            Assert.AreEqual("ETag1", errors.Single().Data["ETag"]);
        }

        [TestMethod]
        public void CollectionConfigurationReportsInvalidFilterForDocumentStreams()
        {
            // ARRANGE
            CollectionConfigurationError[] errors;
            var documentStreamInfos = new[]
            {
                new DocumentStreamInfo()
                {
                    Id = "Stream1",
                    DocumentFilterGroups =
                        new[]
                        {
                            new DocumentFilterConjunctionGroupInfo()
                            {
                                TelemetryType = TelemetryType.Request,
                    Id = "Stream1",
                    DocumentFilterGroups =
                        new[]
                        {
                            new DocumentFilterConjunctionGroupInfo()
                            {
                                TelemetryType = (TelemetryType)505,
                                Filters = new FilterConjunctionGroupInfo() { Filters = new FilterInfo[0] }
                            }
                        }
                                Filters =
                                    new FilterConjunctionGroupInfo()
                                    {
                                        Filters =
                                            new[]
                                            {
                                                new FilterInfo() { FieldName = "NonExistentFilterFieldName1" },
                                                new FilterInfo() { FieldName = "NonExistentFilterFieldName2" }
                                            }
                                    }
                                                new FilterInfo() { FieldName = "NonExistentFilterFieldName4" }
                                            }
                                    }
                            }
                        }
                }
            };

            var collectionConfigurationInfo = new CollectionConfigurationInfo() { DocumentStreams = documentStreamInfos, ETag = "ETag1" };

            // ASSERT
            Assert.AreEqual(2, collectionConfiguration.DocumentStreams.Count());
            Assert.AreEqual("Stream1", collectionConfiguration.DocumentStreams.First().Id);
            Assert.AreEqual("Stream2", collectionConfiguration.DocumentStreams.Last().Id);

            Assert.AreEqual(3, errors.Length);

            Assert.AreEqual(CollectionConfigurationErrorType.DocumentStreamFailureToCreateFilterUnexpected, errors[0].ErrorType);
            Assert.AreEqual(
                "Failed to create a document stream filter TelemetryType: '505', filters: 'NonExistentFilterFieldName1 Equal , NonExistentFilterFieldName2 Equal '.",
            Assert.AreEqual(2, errors[0].Data.Count);
            Assert.AreEqual("Stream1", errors[0].Data["DocumentStreamId"]);
            Assert.AreEqual("ETag1", errors[0].Data["ETag"]);

            Assert.AreEqual(CollectionConfigurationErrorType.FilterFailureToCreateUnexpected, errors[1].ErrorType);
            Assert.AreEqual("Failed to create a filter NonExistentFilterFieldName3 Equal .", errors[1].Message);
            Assert.IsTrue(errors[1].FullException.Contains("Comparand"));
            Assert.AreEqual(5, errors[1].Data.Count);
            Assert.AreEqual("Stream2", errors[1].Data["DocumentStreamId"]);
            Assert.AreEqual("ETag1", errors[1].Data["ETag"]);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
