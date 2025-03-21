namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class RequestDurationBucketExtractorTest
    {
        [TestMethod]
        public void RequestDurationBucket()
        {
            var item = new RequestTelemetry();            
            var extractor = new DurationBucketExtractor();

            durationAndBucket.Add(0, "<250ms");
            durationAndBucket.Add(249, "<250ms");
            durationAndBucket.Add(250, "250ms-500ms");
            durationAndBucket.Add(499, "250ms-500ms");
            durationAndBucket.Add(999, "500ms-1sec");
            durationAndBucket.Add(1000, "1sec-3sec");
            durationAndBucket.Add(2999, "1sec-3sec");
            durationAndBucket.Add(3000, "3sec-7sec");
            durationAndBucket.Add(29999, "15sec-30sec");
            durationAndBucket.Add(30000, "30sec-1min");
            durationAndBucket.Add(59000, "30sec-1min");
            durationAndBucket.Add(59999, "30sec-1min");            
            durationAndBucket.Add(60000, "1min-2min");
            durationAndBucket.Add(119999, "1min-2min");
                Assert.AreEqual(entry.Value, extractedDimension, "duration:" + entry.Key);
            }
        }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
