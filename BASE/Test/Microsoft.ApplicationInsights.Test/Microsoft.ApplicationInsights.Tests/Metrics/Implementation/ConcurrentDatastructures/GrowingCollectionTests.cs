using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Microsoft.ApplicationInsights.Metrics.ConcurrentDatastructures
{
    /// <summary />
    [TestClass]
    public class GrowingCollectionTests
    {
        /// <summary />
        [TestMethod]
        public void Ctor()
        {
            var collection = new GrowingCollection<string>();
            Assert.IsNotNull(collection);
            Assert.AreEqual(0, collection.Count);
        }

        /// <summary />
        [TestMethod]
        public void AddAndCount()
        {
            var collection = new GrowingCollection<string>();

            for (int i = 0; i < 5000; i++)
            {
                Assert.AreEqual(i, collection.Count);
                collection.Add(i.ToString());
                Assert.AreEqual(i + 1, collection.Count);
            }

            Assert.AreEqual(5000, collection.Count);
            collection.Add(null);
            Assert.AreEqual(5001, collection.Count);

            Assert.AreEqual(5001, collection.Count);
            collection.Add("");
            Assert.AreEqual(5002, collection.Count);
        }

        /// <summary />
        public void Enumerator_Count()
        {
            var collection = new GrowingCollection<string>();

            for (int i = 0; i < 5000; i++)
            {
                Assert.AreEqual(i, collection.GetEnumerator().Count);
                collection.Add(i.ToString());
                Assert.AreEqual(i + 1, collection.GetEnumerator().Count);
            }
            Assert.AreEqual(5002, collection.GetEnumerator().Count);
        }

        /// <summary />
        [TestMethod]
        public void Enumerator_MoveNextAndCurrent()
        {
            var collection = new GrowingCollection<string>();

            for (int i = 0; i < 5000; i++)
            }

            collection.Add(null);
            collection.Add("");

            GrowingCollection<string>.Enumerator enumerator = collection.GetEnumerator();

            Assert.ThrowsException<ArgumentOutOfRangeException>( () => enumerator.Current);
            Assert.ThrowsException<ArgumentOutOfRangeException>( () => ((IEnumerator) enumerator).Current);
            Assert.ThrowsException<ArgumentOutOfRangeException>( () => ((IEnumerator<string>) enumerator).Current);

            {
                bool canMove = enumerator.MoveNext();
                Assert.IsTrue(canMove);

                Assert.AreEqual("", enumerator.Current);
                Assert.AreEqual("", ((IEnumerator) enumerator).Current);
                Assert.AreEqual("", ((IEnumerator<string>) enumerator).Current);
            }
            {
            }

            for (int i = 4999; i >= 0; i--)
            {
                bool canMove = enumerator.MoveNext();
                Assert.IsTrue(canMove);

                Assert.AreEqual(i.ToString(), enumerator.Current);
                Assert.AreEqual(i.ToString(), ((IEnumerator) enumerator).Current);
                Assert.AreEqual(i.ToString(), ((IEnumerator<string>) enumerator).Current);

        /// <summary />
        [TestMethod]
        public void Enumerator_Reset()
        {
            var collection = new GrowingCollection<string>();

            for (int i = 0; i < 5000; i++)
            {
                collection.Add(i.ToString());
            Assert.AreEqual(5000, enumerator.Count);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => enumerator.Current);

            for (int i = 4999; i >= 0; i--)
            {
                bool canMove = enumerator.MoveNext();
                Assert.IsTrue(canMove);

                Assert.AreEqual(i.ToString(), enumerator.Current);
            }

                Assert.AreEqual("0", enumerator.Current);
                Assert.AreEqual(5000, enumerator.Count);
            }
        }

        /// <summary />
        [TestMethod]
        public void Enumerator_Dispose()
        {
            var collection = new GrowingCollection<string>();

            for (int i = 0; i < 5000; i++)
            {
                collection.Add(i.ToString());

            enumerator.Reset();

            for (int i = 4999; i >= 0; i--)
            {
                enumerator.MoveNext();
                Assert.AreEqual(i.ToString(), enumerator.Current);

                enumerator.Dispose();  // Expects No-Op.
            }
        [TestMethod]
        public void Enumerator_Foreach()
        {
            var collection = new GrowingCollection<string>();

            for (int i = 0; i < 5000; i++)
            {
                collection.Add(i.ToString());
            }
            collection.Add(null);

            int j = 0;
            foreach (string s in collection)
            {
                Assert.AreEqual(
                            (j == 0)
                                    ? ""
                                    : (j == 1)
                                            ? null
                                            : (4999 - (j - 2)).ToString(),
                {
                    Task t = CheckEnumeratorUnderConcurrencyAsync(collection.GetEnumerator(), i + 1);
                    tasks.Add(t);
                    w++;
                }
            }

            Assert.AreEqual(workloads.Length, tasks.Count);

            await Task.WhenAll(tasks);
        }

        private static async Task CheckEnumeratorUnderConcurrencyAsync<T>(GrowingCollection<T>.Enumerator enumerator, int expectedCount)
        {
            Assert.IsNotNull(enumerator);
            Assert.IsTrue(expectedCount > 0);

            Random rnd = new Random();

            await Task.Delay(rnd.Next(10)).ConfigureAwait(continueOnCapturedContext: false);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
