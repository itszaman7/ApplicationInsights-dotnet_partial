using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.ApplicationInsights.Metrics.ConcurrentDatastructures
{
#pragma warning disable CA1822  // Method should be static
    using Microsoft.ApplicationInsights.Metrics.TestUtility;

    /// <summary />
    [TestClass]
    public class MultidimensionalCube2Tests
    {
        /// <summary />
        [TestMethod]
        public void Ctors()
        {
            int[] dimensionValuesCountLimits = new int[] { 10, 11, 12, 13 };
            int factoryCallsCount = 0;
            object[] lastFactoryCall = null;

            {
                var cube = new MultidimensionalCube2<int>(
                                        (vector) => { Interlocked.Increment(ref factoryCallsCount); lastFactoryCall = vector; return 18; },
                                        (int []) dimensionValuesCountLimits);

                CtorTestImplementation(cube, ref dimensionValuesCountLimits, ref factoryCallsCount, ref lastFactoryCall, "2", 18, Int32.MaxValue);
            }
            {
                var cube = new MultidimensionalCube2<int>(
                                        3,
                                        (vector) => { Interlocked.Increment(ref factoryCallsCount); lastFactoryCall = vector; return -6; },
                                        (int []) dimensionValuesCountLimits);

                CtorTestImplementation(cube, ref dimensionValuesCountLimits, ref factoryCallsCount, ref lastFactoryCall, "4", -6, 3);
            }
        }

        /// <summary />
        [TestMethod]
        public void TryGetOrCreatePoint_PropagesUserException()
        {
            var cube = new MultidimensionalCube2<int>(
                                        (vector) => { if (vector[0].Equals("magic")) return 42; else throw new InvalidOperationException("User Exception"); },
                                        10, 11, 12);

            Assert.AreEqual(0, cube.TotalPointsCount);

            Assert.ThrowsException<InvalidOperationException>( () => cube.TryGetOrCreatePoint("A", "B", "C") );
            Assert.AreEqual(0, cube.TotalPointsCount);

            Assert.ThrowsException<ArgumentNullException>( () => cube.TryGetOrCreatePoint("A", null, "C") );
            Assert.AreEqual(0, cube.TotalPointsCount);

            Assert.AreEqual(false, cube.TryGetPoint("magic", "B", "C").IsSuccess);
            Assert.AreEqual(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, cube.TryGetPoint("magic", "B", "C").ResultCode);
            Assert.AreEqual(0, cube.TotalPointsCount);

            Assert.AreEqual(42, cube.TryGetOrCreatePoint("magic", "B", "C").Point);
            Assert.AreEqual(42, cube.TryGetPoint("magic", "B", "C").Point);

            Assert.AreEqual(1, cube.TotalPointsCount);
        }

        /// <summary />
        [TestMethod]
        public void TryGetOrCreatePoint_IncorrectDimsDetected()
        {
            var cube = new MultidimensionalCube2<int>(
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

            Assert.AreEqual(1, cube.TotalPointsCount);
        }

        /// <summary />
        [TestMethod]
        public void TryGetOrCreatePoint_NullParameters()
        {
            var cube = new MultidimensionalCube2<int>(
                                        (vector) => 0,
                                        10, 11, 12);

            Assert.AreEqual(0, cube.TotalPointsCount);

            Assert.ThrowsException<ArgumentNullException>(() => cube.TryGetOrCreatePoint("A", null, "B"));
            Assert.ThrowsException<ArgumentNullException>(() => cube.TryGetPoint(null, "B", "B"));
            Assert.ThrowsException<ArgumentNullException>(() => cube.TryGetPoint("A", null, "B"));

            Assert.ThrowsException<ArgumentNullException>(() => cube.TryGetOrCreatePoint(null));
            Assert.ThrowsException<ArgumentNullException>(() => cube.TryGetPoint(null));

            Assert.AreEqual(0, cube.TotalPointsCount);
        }

        /// <summary />
        [TestMethod]
        public void TryGetOrCreatePoint_RespectsDimensionValuesCountLimits()
        {
            int[] dimensionValuesCountLimits = new int[] { 3, 5, 4 };


            var cube = new MultidimensionalCube2<int>(
                                        (vector) => { return Int32.Parse(vector[0]); },
                                        dimensionValuesCountLimits);

            MultidimensionalPointResult<int> result;
            for (int d1 = 0; d1 < dimensionValuesCountLimits[0]; d1++)
            {
                for (int d2 = 0; d2 < dimensionValuesCountLimits[1]; d2++)
                {
                    for (int d3 = 0; d3 < dimensionValuesCountLimits[2]; d3++)
                        Assert.AreEqual(
                                    1 + d3 + d2 * dimensionValuesCountLimits[2] + d1 * dimensionValuesCountLimits[1] * dimensionValuesCountLimits[2],
                                    cube.TotalPointsCount,
                                    $"d3 = {d3}; d2 = {d2}; d1 = {d1}");

                        result = cube.TryGetPoint($"{d1 * 10}", $"{d2}", $"{d3}");
                        Assert.IsTrue(result.IsSuccess);
                        Assert.AreEqual(d1 * 10, result.Point);
                        Assert.AreEqual(
                                    1 + d3 + d2 * dimensionValuesCountLimits[2] + d1 * dimensionValuesCountLimits[1] * dimensionValuesCountLimits[2],
                Assert.AreEqual(0, result.Point);
                Assert.AreEqual(result.ResultCode, MultidimensionalPointResultCodes.Failure_SubdimensionsCountLimitReached);
                Assert.AreEqual(1, result.FailureCoordinateIndex);

                result = cube.TryGetPoint($"{d1 * 10}", $"{dimensionValuesCountLimits[1]}", $"{dimensionValuesCountLimits[2]}");
                Assert.IsFalse(result.IsSuccess);
                Assert.AreEqual(0, result.Point);
                Assert.AreEqual(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, result.ResultCode);
                Assert.AreEqual(-1, result.FailureCoordinateIndex);
            }

            result = cube.TryGetOrCreatePoint($"{dimensionValuesCountLimits[0] * 10}", $"{dimensionValuesCountLimits[1]}", $"{dimensionValuesCountLimits[2]}");
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(0, result.Point);
            Assert.AreEqual(result.ResultCode, MultidimensionalPointResultCodes.Failure_SubdimensionsCountLimitReached);
            Assert.AreEqual(0, result.FailureCoordinateIndex);

            result = cube.TryGetPoint($"{dimensionValuesCountLimits[0] * 10}", $"{dimensionValuesCountLimits[1]}", $"{dimensionValuesCountLimits[2]}");
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(0, result.Point);
            string[] lastFactoryCall = null;

            var cube = new MultidimensionalCube2<int>(
                                        1000,
                                        (vector) => { lastFactoryCall = vector;  return Int32.Parse(vector[0]); },
                                        10000, 10000, 10000);

            Assert.AreEqual(1000, cube.TotalPointsCountLimit);

            MultidimensionalPointResult<int> result;
            for (int p = 0; p < 1000; p++)
            {
                result = cube.TryGetPoint($"{p}", "foo", "bar");
                Assert.IsFalse(result.IsSuccess);
                Assert.AreEqual(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, result.ResultCode);
                Assert.AreEqual(0, result.Point);
                Assert.AreEqual(p, cube.TotalPointsCount);

                TestUtil.AssertAreEqual(p == 0 ? null : new string[] { $"{p - 1}", "foo", "bar" }, lastFactoryCall);

                result = cube.TryGetOrCreatePoint($"{p}", "foo", "bar");
                Assert.IsTrue(result.IsSuccess);
                Assert.IsTrue(result.IsPointCreatedNew);
                Assert.AreEqual(p, result.Point);
                Assert.AreEqual(p + 1, cube.TotalPointsCount);

                TestUtil.AssertAreEqual(new string[] { $"{p}", "foo", "bar" }, lastFactoryCall);

                result = cube.TryGetPoint($"{p}", "foo", "bar");
                Assert.IsTrue(result.IsSuccess);
                Assert.IsFalse(result.IsPointCreatedNew);
                Assert.AreEqual(p, result.Point);
                Assert.AreEqual(p + 1, cube.TotalPointsCount);

                TestUtil.AssertAreEqual(new string[] { $"{p}", "foo", "bar" }, lastFactoryCall);
            }

            result = cube.TryGetOrCreatePoint($"{1000}", "foo", "bar"); 
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(0, result.Point);
            Assert.AreEqual(result.ResultCode, MultidimensionalPointResultCodes.Failure_TotalPointsCountLimitReached);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(1000, cube.TotalPointsCount);
            TestUtil.AssertAreEqual(new string[] { $"999", "foo", "bar" }, lastFactoryCall);

            result = cube.TryGetPoint($"{1000}", "foo", "bar");
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(0, result.Point);
            Assert.AreEqual(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, result.ResultCode);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.IsFalse(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, result.ResultCode);
            Assert.AreEqual(0, result.Point);
            Assert.AreEqual(0, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("1", "1", "1");

            Assert.IsTrue(result.IsSuccess);

            result = cube.TryGetOrCreatePoint("1", "1", "1");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(1, cube.TotalPointsCount);

            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, result.ResultCode);
            Assert.AreEqual(0, result.Point);
            Assert.AreEqual(1, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("1", "1", "7");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_NewPointCreated, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(2, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("1", "1", "7");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(2, cube.TotalPointsCount);

            result = cube.TryGetPoint("1", "1", "7");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, result.ResultCode);

            result = cube.TryGetPoint("2", "1", "7");

            Assert.IsFalse(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, result.ResultCode);
            Assert.AreEqual(0, result.Point);
            Assert.AreEqual(2, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("2", "1", "7");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_NewPointCreated, result.ResultCode);
            Assert.AreEqual(2, result.Point);
            Assert.AreEqual(3, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("2", "1", "7");
            result = cube.TryGetPoint("1", "1", "1");

            Assert.IsFalse(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, result.ResultCode);
            Assert.AreEqual(0, result.Point);
            Assert.AreEqual(0, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("1", "1", "1");
            Assert.IsTrue(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_NewPointCreated, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(1, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("1", "1", "1");
            result = cube.TryGetOrCreatePoint("1", "1", "2");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(2, cube.TotalPointsCount);

            result = cube.TryGetPoint("1", "1", "3");

            Assert.IsFalse(result.IsSuccess);
            Assert.IsFalse(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, result.ResultCode);
            Assert.AreEqual(0, result.Point);
            Assert.AreEqual(2, cube.TotalPointsCount);

            // Triggers creation of fallback dimension value.
            // i.e 3 will be replaced with "dimCapValue"
            result = cube.TryGetOrCreatePoint("1", "1", "3");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_NewPointCreatedAboveDimCapLimit, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(3, cube.TotalPointsCount);

            result = cube.TryGetPoint("1", "1", "3");
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_NewPointCreated, result.ResultCode);
            Assert.AreEqual(1, result.Point);
            Assert.AreEqual(4, cube.TotalPointsCount);


            // Triggers creation of fallback dimension value.
            // i.e both 3 will be replaced with "dimCapValue"
            result = cube.TryGetOrCreatePoint("1", "3", "3");

            Assert.AreEqual(5, cube.TotalPointsCount);

            // Force 1st dimension to reach cap. (preparing for the next case below)
            result = cube.TryGetOrCreatePoint("2", "2", "1");

            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.IsPointCreatedNew);
            Assert.AreEqual(-1, result.FailureCoordinateIndex);
            Assert.AreEqual(MultidimensionalPointResultCodes.Success_NewPointCreated, result.ResultCode);
            Assert.AreEqual(2, result.Point);
                                        (vector) => Int32.Parse(vector[0]), true,
                                        dimCapValue,
                                        2, 2, 2);

            MultidimensionalPointResult<int> result;

            Assert.AreEqual(0, cube.TotalPointsCount);

            result = cube.TryGetOrCreatePoint("1", "1", "1");
            Assert.IsTrue(result.IsSuccess);
            cube.TryGetOrCreatePoint("45", "12", "-8");
            cube.TryGetOrCreatePoint("10", "20", "30");
            cube.TryGetOrCreatePoint("1", "5", "3");
            cube.TryGetOrCreatePoint("1", "2", "4");
            cube.TryGetOrCreatePoint("-6", "14", "132");

            Assert.AreEqual(6, cube.TotalPointsCount);

            IReadOnlyCollection<KeyValuePair<string[], int>> allPoints1 = cube.GetAllPoints();

                            ref int factoryCallsCount,
                            ref object[] lastFactoryCall,
                            string suffix,
                            int point,
                            int totalPointsCountLimit)
        {
            Assert.IsNotNull(cube);

            Assert.AreEqual(dimensionValuesCountLimits.Length, cube.DimensionsCount);



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
