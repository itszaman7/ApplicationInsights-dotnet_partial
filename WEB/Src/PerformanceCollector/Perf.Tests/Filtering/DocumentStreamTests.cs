namespace Microsoft.ApplicationInsights.Tests
{
    using System.Linq;

    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Filtering;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DocumentStreamTests
    {
        [TestMethod]
        public void DocumentStreamHandlesNoFiltersCorrectly()
        {
            // ARRANGE
            CollectionConfigurationError[] errors;
            var documentStreamInfo = new DocumentStreamInfo() { DocumentFilterGroups = new DocumentFilterConjunctionGroupInfo[0] };
            var documentStream = new DocumentStream(documentStreamInfo, out errors, new ClockMock());
            var request = new RequestTelemetry() { Id = "apple" };

            // ACT
            CollectionConfigurationError[] runtimeErrors;
            bool result = documentStream.CheckFilters(request, out runtimeErrors);
            
            // ASSERT
            Assert.AreEqual(0, errors.Length);
            Assert.AreEqual(0, runtimeErrors.Length);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DocumentStreamHandlesNullFiltersCorrectly()
        {
            // ARRANGE
            CollectionConfigurationError[] errors;
            var documentStreamInfo = new DocumentStreamInfo() { DocumentFilterGroups = null };
            var documentStream = new DocumentStream(documentStreamInfo, out errors, new ClockMock());
            var request = new RequestTelemetry() { Id = "apple" };

                            TelemetryType = TelemetryType.Request,
                            Filters = new FilterConjunctionGroupInfo { Filters = new[] { filterApple, filterOrange } }
                        },
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Request,
                            Filters = new FilterConjunctionGroupInfo { Filters = new[] { filterMango } }
                        }
                    }
            };
            var documentStream = new DocumentStream(documentStreamInfo, out errors, new ClockMock());
            var requests = new[]
            {
                new RequestTelemetry() { Id = "apple" }, new RequestTelemetry() { Id = "orange" }, new RequestTelemetry() { Id = "mango" },
                new RequestTelemetry() { Id = "apple orange" }, new RequestTelemetry() { Id = "apple mango" },
                new RequestTelemetry() { Id = "orange mango" }, new RequestTelemetry() { Id = "apple orange mango" },
                new RequestTelemetry() { Id = "none of the above" }
            };

            // ACT

            Assert.IsFalse(errorsEncountered);

            Assert.IsFalse(results[0]);
            Assert.IsFalse(results[1]);
            Assert.IsTrue(results[2]);
            Assert.IsTrue(results[3]);
            Assert.IsTrue(results[4]);
            Assert.IsTrue(results[5]);
            Assert.IsTrue(results[6]);
            Assert.IsFalse(results[7]);
        }

        [TestMethod]
        public void DocumentStreamFiltersDependenciesCorrectly()
        {
            // ARRANGE
            CollectionConfigurationError[] errors;
            FilterInfo filterApple = new FilterInfo { FieldName = "Id", Predicate = Predicate.Contains, Comparand = "apple" };
            FilterInfo filterOrange = new FilterInfo { FieldName = "Id", Predicate = Predicate.Contains, Comparand = "orange" };
            var documentStreamInfo = new DocumentStreamInfo()
            {
                DocumentFilterGroups =
                    new[]
                    {
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Dependency,
                            Filters = new FilterConjunctionGroupInfo { Filters = new[] { filterApple, filterOrange } }
                        },
            };
            var documentStream = new DocumentStream(documentStreamInfo, out errors, new ClockMock());
            var dependencies = new[]
            {
                new DependencyTelemetry() { Id = "apple" }, new DependencyTelemetry() { Id = "orange" }, new DependencyTelemetry() { Id = "mango" },
                new DependencyTelemetry() { Id = "apple orange" }, new DependencyTelemetry() { Id = "apple mango" },
                new DependencyTelemetry() { Id = "orange mango" }, new DependencyTelemetry() { Id = "apple orange mango" },
                new DependencyTelemetry() { Id = "none of the above" }
            };


            Assert.IsFalse(errorsEncountered);

            Assert.IsFalse(results[0]);
            Assert.IsFalse(results[1]);
            Assert.IsTrue(results[2]);
            Assert.IsTrue(results[3]);
            Assert.IsTrue(results[4]);
            Assert.IsTrue(results[5]);
            Assert.IsTrue(results[6]);
            FilterInfo filterOrange = new FilterInfo { FieldName = "Message", Predicate = Predicate.Contains, Comparand = "orange" };
            FilterInfo filterMango = new FilterInfo { FieldName = "Message", Predicate = Predicate.Contains, Comparand = "mango" };

            // (apple AND orange) OR mango
            var documentStreamInfo = new DocumentStreamInfo()
            {
                DocumentFilterGroups =
                    new[]
                    {
                        new DocumentFilterConjunctionGroupInfo()
                            TelemetryType = TelemetryType.Exception,
                            Filters = new FilterConjunctionGroupInfo { Filters = new[] { filterApple, filterOrange } }
                        },
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Exception,
                            Filters = new FilterConjunctionGroupInfo { Filters = new[] { filterMango } }
                        }
                    }
            };
            var documentStream = new DocumentStream(documentStreamInfo, out errors, new ClockMock());
            var exceptions = new[]
            {
                new ExceptionTelemetry() { Message = "apple" }, new ExceptionTelemetry() { Message = "orange" },
                new ExceptionTelemetry() { Message = "mango" }, new ExceptionTelemetry() { Message = "apple orange" },
                new ExceptionTelemetry() { Message = "apple mango" }, new ExceptionTelemetry() { Message = "orange mango" },
                new ExceptionTelemetry() { Message = "apple orange mango" }, new ExceptionTelemetry() { Message = "none of the above" }
            };

            // ACT
                if (runtimeErrors.Any())
                {
                    errorsEncountered = true;
                }
            }

            // ASSERT
            Assert.AreEqual(0, errors.Length);

            Assert.IsFalse(errorsEncountered);

            Assert.IsFalse(results[0]);
            Assert.IsFalse(results[1]);
            Assert.IsTrue(results[2]);
            Assert.IsTrue(results[3]);
            Assert.IsTrue(results[4]);
            Assert.IsTrue(results[5]);
            Assert.IsTrue(results[6]);
            Assert.IsFalse(results[7]);
        }
                            Filters = new FilterConjunctionGroupInfo { Filters = new[] { filterApple, filterOrange } }
                        },
                        new DocumentFilterConjunctionGroupInfo()
                        {
                            TelemetryType = TelemetryType.Event,
                            Filters = new FilterConjunctionGroupInfo { Filters = new[] { filterMango } }
                        }
                    }
            };
            var documentStream = new DocumentStream(documentStreamInfo, out errors, new ClockMock());
                new EventTelemetry() { Name = "orange mango" }, new EventTelemetry() { Name = "apple orange mango" },
                new EventTelemetry() { Name = "none of the above" }
            };

            // ACT
            var results = new bool[events.Length];
            bool errorsEncountered = false;
            for (int i = 0; i < events.Length; i++)
            {
                CollectionConfigurationError[] runtimeErrors;

            Assert.IsFalse(errorsEncountered);

            Assert.IsFalse(results[0]);
            Assert.IsFalse(results[1]);
            Assert.IsTrue(results[2]);
            Assert.IsTrue(results[3]);
            Assert.IsTrue(results[4]);
            Assert.IsTrue(results[5]);
            Assert.IsTrue(results[6]);
                        {
                            TelemetryType = TelemetryType.Request,
                            Filters = new FilterConjunctionGroupInfo { Filters = new[] { filterApple, filterOrange } }
                        },
            Assert.IsTrue(errors[1].FullException.Contains("Error finding property NonExistentField2 in the type Microsoft.ApplicationInsights.DataContracts.RequestTelemetry"));
            Assert.AreEqual(3, errors[1].Data.Count);
            Assert.AreEqual("NonExistentField2", errors[1].Data["FilterFieldName"]);
            Assert.AreEqual(Predicate.Contains.ToString(), errors[1].Data["FilterPredicate"]);
            Assert.AreEqual("orange", errors[1].Data["FilterComparand"]);

            Assert.AreEqual(CollectionConfigurationErrorType.FilterFailureToCreateUnexpected, errors[2].ErrorType);
            Assert.AreEqual(
                "Failed to create a filter NonExistentField3 Contains mango.",
                errors[2].Message);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
