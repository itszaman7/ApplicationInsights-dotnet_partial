namespace Microsoft.ApplicationInsights.Metrics.ConcurrentDatastructures
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    using static System.FormattableString;

    /// <summary>A very fast, lock free, unordered collection to which items can be added, but never removed.</summary>
    /// <typeparam name="T">Type of collection elements.</typeparam>
    internal class GrowingCollection<T> : IEnumerable<T>
    {
        private const int SegmentSize = 32;

        private Segment dataHead;

        /// <summary>Creates a new <c>GrowingCollection</c>.</summary>
        public GrowingCollection()
        {
            this.dataHead = new Segment(null);
        }

        /// <summary>Gets the current number of items in the collection.</summary>
        /// <summary>Adds an item to the collection.</summary>
        /// <param name="item">Item to be added.</param>
        public void Add(T item)
        {
            Segment currHead = Volatile.Read(ref this.dataHead);

            bool added = currHead.TryAdd(item);
            while (false == added)
            {
                Segment newHead = new Segment(currHead);
            return enumerator;
        }

        /// <summary>Gets an enumerator over this colletion. No particular element order is guaranteed.
        /// The enumerator is resilient to concurrent additions to the collection.</summary>
        /// <returns>A new enumerator that will cover all items already in the collection.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
                this.headOffset = this.currentSegmentOffset = head.LocalCount;
                this.count = this.headOffset + (this.head.NextSegment == null ? 0 : this.head.NextSegment.GlobalCount);
            }

            /// <summary>Gets the total number of elements returned by this enumerator.</summary>
            public int Count
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    return this.count;
                }
            }

            /// <summary>Gets the current element.</summary>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    return this.currentSegment[this.currentSegmentOffset];
                }
            }

            object IEnumerator.Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            /// <summary>Move to the next element in the underlying colection.</summary>
            /// <returns>The next element in the underlying collection.</returns>
            public bool MoveNext()
            {
                if (this.currentSegmentOffset == 0)
                {
            {
                this.currentSegment = this.head;
                this.currentSegmentOffset = this.headOffset;
            }

            private static void Dispose(bool disposing)
            {
                if (disposing)
                {
                }
            }
        }
        #endregion class Enumerator 

        #region class Segment
        internal class Segment
        {
            private readonly Segment nextSegment;
            private readonly int nextSegmentGlobalCount;
            private readonly T[] data = new T[SegmentSize];

            public Segment(Segment nextSegment)
            {
                this.nextSegment = nextSegment;
                this.nextSegmentGlobalCount = (nextSegment == null) ? 0 : nextSegment.GlobalCount;
            }

            public int LocalCount
            {
                get
                {
                    int lc = Volatile.Read(ref this.localCount);
                    if (lc > SegmentSize)
                    {
                        return SegmentSize;
                    }
                    else
                    {
                        return lc;
                    }
            {
                get
                {
                    if (index < 0 || this.localCount <= index || SegmentSize <= index)
                    {
                        throw new ArgumentOutOfRangeException(nameof(index), Invariant($"Invalid index ({index})"));
                    }

                    return this.data[index];
                }
            {
                int index = Interlocked.Increment(ref this.localCount) - 1;
                if (index >= SegmentSize)
                {
                    Interlocked.Decrement(ref this.localCount);
                    return false;
                }

                this.data[index] = item;
                return true;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
