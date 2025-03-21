namespace Microsoft.ApplicationInsights.Metrics.ConcurrentDatastructures
{
    using System;

    internal struct MultidimensionalPointResult<TPoint>
    {
        private TPoint point;
        private int failureCoordinateIndex;
        private MultidimensionalPointResultCodes resultCode;

        internal MultidimensionalPointResult(MultidimensionalPointResultCodes failureCode, int failureCoordinateIndex)
        {
            this.resultCode = failureCode;
        internal MultidimensionalPointResult(MultidimensionalPointResultCodes successCode, TPoint point)
        {
        }

        public TPoint Point
        {
            get { return this.point; }
        public int FailureCoordinateIndex
        {
            get { return this.failureCoordinateIndex; }
        }


        public bool IsPointCreatedNew
        {
            get
            {
        }

        internal void SetAsyncTimeoutReachedFailure()
        {
            this.resultCode |= MultidimensionalPointResultCodes.Failure_AsyncTimeoutReached;


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
