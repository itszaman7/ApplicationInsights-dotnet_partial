namespace Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    [TestClass]
    public class ApplicationInsightsApplicationIdProviderTests : ApplicationIdTestBase
    {
        /// <summary>
        /// Lookup is expected to fail on first call, this is how it invokes the Http request.
        /// </summary>
        [TestMethod, Timeout(testTimeoutMilliseconds)]
        public void VerifyFailsOnFirstRequest()
        {
            var mockProfileServiceWrapper = GenerateMockServiceWrapper(HttpStatusCode.OK, testApplicationId);
            var aiApplicationIdProvider = new ApplicationInsightsApplicationIdProvider(mockProfileServiceWrapper);

            Assert.IsFalse(aiApplicationIdProvider.TryGetApplicationId(testInstrumentationKey, out string ignore));
        }

        /// <summary>
        /// Lookup is expected to succeed on the second call, after the Http request has completed.
        /// </summary>
        [TestMethod, Timeout(testTimeoutMilliseconds)]
        public void VerifySucceedsOnSecondRequest()
        {
            var mockProfileServiceWrapper = GenerateMockServiceWrapper(HttpStatusCode.OK, testApplicationId);
            var aiApplicationIdProvider = new ApplicationInsightsApplicationIdProvider(mockProfileServiceWrapper);

                Thread.Sleep(taskWaitMilliseconds);
            }

            Assert.IsTrue(aiApplicationIdProvider.TryGetApplicationId(testInstrumentationKey, out string actual));
            Assert.AreEqual(testFormattedApplicationId, actual);
        }

        /// <summary>
        /// Protect against injection attacks. Test that if an malicious value is returned, that value will be truncated.
        /// </summary>
        [TestMethod, Timeout(testTimeoutMilliseconds)]
        public void VerifyMaliciousApplicationIdIsTruncated() 
        {
            // 50 character string.
            var testApplicationId = "a123456789b123546789c123456789d123456798e123456789";

            // An arbitrary string that is expected to be truncated.
            var malicious = "00000000000000000000000000000000000000000000000000000000000";

            var testFormattedApplicationId = ApplicationIdHelper.ApplyFormatting(testApplicationId);
                {
                    Content = new StringContent(testApplicationId)
                });
            });
            var aiApplicationIdProvider = new ApplicationInsightsApplicationIdProvider(mockProfileServiceWrapper);

            Console.WriteLine($"first request: {DateTime.UtcNow}");
            Assert.IsFalse(aiApplicationIdProvider.TryGetApplicationId(testInstrumentationKey, out string ignore1));
            Console.WriteLine($"second request: {DateTime.UtcNow}");
            Assert.IsFalse(aiApplicationIdProvider.TryGetApplicationId(testInstrumentationKey, out string ignore2));
            Assert.AreEqual(1, aiApplicationIdProvider.FetchTasks.Count);
        }

        [TestMethod, Timeout(testTimeoutMilliseconds)]
        public void VerifyWhenRequestFailsWillWaitBeforeRetry() 
        {
            mockMethodFailOnceStateBool = false;
            var mockProfileServiceWrapper = GenerateMockServiceWrapper(this.MockMethodFailOnce);
            var aiApplicationIdProvider = new ApplicationInsightsApplicationIdProvider(mockProfileServiceWrapper);
            var stopWatch = new Stopwatch();
            }
                Console.WriteLine("wait for retry");
                Thread.Sleep(taskWaitMilliseconds);
            }
            stopWatch.Stop();
            Assert.IsTrue(stopWatch.Elapsed >= failedRequestRetryWaitTime, "too fast, did not wait timeout");

            Console.WriteLine($"\nthird request: {DateTime.UtcNow.ToString("HH:mm:ss:fffff")}");
            Assert.IsFalse(aiApplicationIdProvider.TryGetApplicationId(testInstrumentationKey, out string ignore3)); // second retry should fail (because no matching value), but will create new request task

            // wait for async tasks to complete
            }

            Console.WriteLine("second request");
            Assert.IsFalse(aiApplicationIdProvider.TryGetApplicationId(testInstrumentationKey, out string ignore2)); // retry should fail, fatal error
            Thread.Sleep(failedRequestRetryWaitTime + failedRequestRetryWaitTime); // wait for timeout to expire (2x timeout).

            Console.WriteLine("third request");
            Assert.IsFalse(aiApplicationIdProvider.TryGetApplicationId(testInstrumentationKey, out string ignore3)); // retry should still fail, fatal error
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
