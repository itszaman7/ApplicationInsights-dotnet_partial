using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Microsoft.ApplicationInsights.Metrics.ConcurrentDatastructures
{
    using Microsoft.ApplicationInsights.Metrics.TestUtility;

    /// <summary />
    [TestClass]
    public class MultidimensionalCubeTests
    {
        /// <summary />
        [TestMethod]
        public void Ctors()
        {
            int[] subdimensionsCountLimits = new int[] { 10, 11, 12, 13 };
            int factoryCallsCount = 0;
            object[] lastFactoryCall = null;

            {
                var cube = new MultidimensionalCube<string, int>(
                                        (vector) => { Interlocked.Increment(ref factoryCallsCount); lastFactoryCall = vector; return 42; },
                                        (IEnumerable<int>) subdimensionsCountLimits);

                CtorTestImplementation(cube, ref subdimensionsCountLimits, ref factoryCallsCount, ref lastFactoryCall, "1", 42, Int32.MaxValue);
            }
            {
                var cube = new MultidimensionalCube<string, int>(
                                        (vector) => { Interlocked.Increment(ref factoryCallsCount); lastFactoryCall = vector; return 18; },
                                        (int []) subdimensionsCountLimits);

                CtorTestImplementation(cube, ref subdimensionsCountLimits, ref factoryCallsCount, ref lastFactoryCall, "2", 18, Int32.MaxValue);
            }
            {
                var cube = new MultidimensionalCube<string, int>(
                                        5,
                                        (vector) => { Interlocked.Increment(ref factoryCallsCount); lastFactoryCall = vector; return 100; },
                                        (IEnumerable<int>) subdimensionsCountLimits);

                CtorTestImplementation(cube, ref subdimensionsCountLimits, ref factoryCallsCount, ref lastFactoryCall, "3", 100, 5);
            }
            {
                var cube = new MultidimensionalCube<string, int>(
                                        3,
                                        (vector) => { Interlocked.Increment(ref factoryCallsCount); lastFactoryCall = vector; return -6; },
                                        (int[]) subdimensionsCountLimits);

                CtorTestImplementation(cube, ref subdimensionsCountLimits, ref factoryCallsCount, ref lastFactoryCall, "4", -6, 3);
            }
        }

        /// <summary />
        [TestMethod]
        public void TryGetOrCreatePoint_PropagesUserException()
        {
            var cube = new MultidimensionalCube<string, int>(
                                        (vector) => { if (vector[0].Equals("magic")) return 42; else throw new InvalidOperationException("User Exception"); },
                                        10, 11, 12);

            Assert.AreEqual(0, cube.TotalPointsCount);

            Assert.ThrowsException<InvalidOperationException>(() => cube.TryGetOrCreatePoint("A", "B", "C"));

            Assert.AreEqual(0, cube.TotalPointsCount);

            Assert.IsFalse(cube.TryGetPoint("A", null, "B").IsSuccess);
            Assert.IsTrue(0 != (MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested & cube.TryGetPoint("A", null, "B").ResultCode));

            Assert.AreEqual(0, cube.TotalPointsCount);

            Assert.AreEqual(42, cube.TryGetOrCreatePoint("magic", "B", "C").Point);
        [TestMethod]
        public void TryGetOrCreatePoint_IncorrectDimsDetected()
        {
            var cube = new MultidimensionalCube<string, int>(
                                        (vector) => 0,
                                        10, 11, 12);

            Assert.AreEqual(0, cube.TotalPointsCount);

            Assert.ThrowsException<ArgumentException>(() => cube.TryGetOrCreatePoint("A", "B"));
            Assert.ThrowsException<ArgumentException>(() => cube.TryGetPoint("A", "B"));

            Assert.ThrowsException<ArgumentException>(() => cube.TryGetOrCreatePoint("A", "B", "C", "D"));
            Assert.ThrowsException<ArgumentException>(() => cube.TryGetPoint("A", "B", "C", "D"));

            Assert.AreEqual(0, cube.TotalPointsCount);

            Assert.IsTrue(cube.TryGetOrCreatePoint("A", "B", "C1").IsSuccess);
            Assert.IsTrue(cube.TryGetPoint("A", "B", "C1").IsSuccess);
            Assert.IsFalse(cube.TryGetPoint("A", "B", "C2").IsSuccess);

            Assert.ThrowsException<ArgumentNullException>(() => cube.TryGetOrCreatePoint("A", null, "B"));
            Assert.ThrowsException<ArgumentNullException>(() => cube.TryGetPoint(null, "B", "B"));
            Assert.IsFalse(cube.TryGetPoint("A", null, "B").IsSuccess);
            Assert.IsTrue(0 != (MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested & cube.TryGetPoint("A", null, "B").ResultCode));

            Assert.ThrowsException<ArgumentNullException>(() => cube.TryGetOrCreatePoint(null));
            Assert.ThrowsException<ArgumentNullException>(() => cube.TryGetPoint(null));

            Assert.AreEqual(0, cube.TotalPointsCount);
                        Assert.IsFalse(result.IsSuccess);
                        Assert.AreEqual(0, result.Point);
                        Assert.IsTrue(0 != (MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested & result.ResultCode));
                        Assert.AreEqual(
                                    d3 + d2 * subdimensionsCountLimits[2] + d1 * subdimensionsCountLimits[1] * subdimensionsCountLimits[2],
                                    cube.TotalPointsCount,
                                    $"d3 = {d3}; d2 = {d2}; d1 = {d1}");

                        result = cube.TryGetOrCreatePoint($"{d1 * 10}", $"{d2}", $"{d3}");
                        Assert.IsTrue(result.IsSuccess);

                    result = cube.TryGetPoint($"{d1 * 10}", $"{d2}", $"{subdimensionsCountLimits[2]}");
                    Assert.IsFalse(result.IsSuccess);
                    Assert.AreEqual(0, result.Point);
                    Assert.IsTrue(0 != (MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested & result.ResultCode));
                    Assert.AreEqual(2, result.FailureCoordinateIndex);

                }

                result = cube.TryGetOrCreatePoint($"{d1 * 10}", $"{subdimensionsCountLimits[1]}", $"{subdimensionsCountLimits[2]}");

            var cube = new MultidimensionalCube<string, int>(
                                        1000,
                                        (vector) => { lastFactoryCall = vector;  return Int32.Parse(vector[0]); },
                                        10000, 10000, 10000);

            Assert.AreEqual(1000, cube.TotalPointsCountLimit);

            MultidimensionalPointResult<int> result;
            for (int p = 0; p < 1000; p++)

                TestUtil.AssertAreEqual(new string[] { $"{p}", "foo", "bar" }, lastFactoryCall);

                result = cube.TryGetPoint($"{p}", "foo", "bar");
                Assert.IsTrue(result.IsSuccess);
                Assert.IsFalse(result.IsPointCreatedNew);
                Assert.AreEqual(p, result.Point);
                Assert.AreEqual(p + 1, cube.TotalPointsCount);

                TestUtil.AssertAreEqual(new string[] { $"{p}", "foo", "bar" }, lastFactoryCall);
            result = cube.TryGetOrCreatePoint($"{1000}", "foo", "bar"); 
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(0, result.Point);
            Assert.IsTrue(0 != (result.ResultCode & MultidimensionalPointResultCodes.Failure_TotalPointsCountLimitReached));
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(1000, cube.TotalPointsCount);
            TestUtil.AssertAreEqual(new string[] { $"999", "foo", "bar" }, lastFactoryCall);

            result = cube.TryGetPoint($"{1000}", "foo", "bar");
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(0, result.Point);
            Assert.IsTrue(0 != (MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested & result.ResultCode));
            Assert.AreEqual(0, result.FailureCoordinateIndex);
            Assert.AreEqual(1000, cube.TotalPointsCount);
            TestUtil.AssertAreEqual(new string[] { $"999", "foo", "bar" }, lastFactoryCall);
        }

        /// <summary />
        [TestMethod]
        public void TryGetOrCreatePoint_GetsAndCreates()

            Assert.IsFalse(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(0, result.FailureCoordinateIndex);
            Assert.IsTrue(0 != (MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested & result.ResultCode));
            Assert.AreEqual(0, result.Point);
            Assert.AreEqual(0, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("1", "1", "1");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_NewPointCreated, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(1, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("1", "1", "1");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(1, cube.TotalPointsCount);


            result = cube.TryGetPoint("1", "1", "7");

            Assert.IsFalse(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(2, result.FailureCoordinateIndex);
            Assert.IsTrue(0 != (MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested & result.ResultCode));
            Assert.AreEqual(0, result.Point);
            Assert.AreEqual(1, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("1", "1", "7");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(2, cube.TotalPointsCount);


            result = cube.TryGetPoint("2", "1", "7");

            Assert.IsFalse(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(0, result.FailureCoordinateIndex);
            Assert.IsTrue(0 != (MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested & result.ResultCode));
            Assert.AreEqual(0, result.Point);
            Assert.AreEqual(2, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("2", "1", "7");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_NewPointCreated, result.ResultCode);
            Assert.AreEqual(2, result.Point);
            Assert.AreEqual(3, cube.TotalPointsCount);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, result.ResultCode);
            Assert.AreEqual(2, result.Point);
            Assert.AreEqual(3, cube.TotalPointsCount);

            result = cube.TryGetPoint("2", "1", "7");

            Assert.IsTrue(result.IsSuccess);
                    (points1, points2, expectedVector, expectedPoint) =>
                    {
                        Assert.AreEqual(1, points1.Where((p) => TestUtil.AreEqual(expectedVector, p.Key)).Count());
                        Assert.AreEqual(expectedPoint, points1.First( (p) => TestUtil.AreEqual(expectedVector, p.Key) ).Value);

                        Assert.IsTrue(points2.ContainsKey(expectedVector));
                        Assert.AreEqual(expectedPoint, points2[expectedVector]);
                    };

            AssertContains(allPoints1, allPoints2, new string[] { "1", "2", "3" }, 1);
                                            //Trace.WriteLine(passed);
                                            if (passed < delayMillis)
                                            {
                                                throw new InvalidOperationException($"delayMillis was {delayMillis}, but only {passed} millis passed.");
                                            }
                                            //else
                                            //{
                                            //    throw new InvalidOperationException($"Test issue: Bad timing. delayMillis was {delayMillis}, but {passed} millis passed.");
                                            //}
                                            return Int32.Parse(vector[0]);
                                        },
                                        10000, 10000, 10000);

            MultidimensionalPointResult<int> result;

            // Warmup:
            try
            {
                baseTS = DateTimeOffset.Now;
                result = cube.TryGetOrCreatePointAsync(
                factoryCallsCount = 0;
                baseTS = DateTimeOffset.Now;
                result = cube.TryGetOrCreatePointAsync(
                                        TimeSpan.FromMilliseconds(2),
                                        TimeSpan.FromMilliseconds(7),
                                        CancellationToken.None,
                                        "7", "500", "foo")
                             .GetAwaiter().GetResult();

                Assert.Fail("An ApplicationException was expected to escape.");
            }
            catch (InvalidOperationException)
            {
            }

            Assert.AreEqual(3, factoryCallsCount, "Timing dependent. Might sometimes fail.");

            //Trace.WriteLine("--------");

            factoryCallsCount = 0;
            baseTS = DateTimeOffset.Now;

            result = cube.TryGetOrCreatePointAsync(
                                    TimeSpan.FromMilliseconds(100),
                                    TimeSpan.FromMilliseconds(1000),
                                    CancellationToken.None,
                                    "7", "500", "foo")
                            .GetAwaiter().GetResult();

            Assert.AreEqual(7, factoryCallsCount, "Timing dependent. Might sometimes fail.");


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
