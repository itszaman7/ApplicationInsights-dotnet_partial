namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TransmissionPolicy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

        public void VerifyCalcuateMinimums()
        {
            // Setup
            var policies = new TransmissionPolicyCollection(policies: new List<TransmissionPolicy>
                new MockValueTransmissionPolicy(maxSenderCapacity: 1, maxBufferCapacity: 2, maxStorageCapacity: 3),
                new MockValueTransmissionPolicy(maxSenderCapacity: 101, maxBufferCapacity: 102, maxStorageCapacity: 103),
            });


        [TestMethod]
        public void VerifyCalcuateMinimums_CanHandleNulls()
        {
            // Setup
            var policies = new TransmissionPolicyCollection(policies: new List<TransmissionPolicy>
            {
                new MockValueTransmissionPolicy(maxSenderCapacity: null, maxBufferCapacity: null, maxStorageCapacity: null),
        private class MockValueTransmissionPolicy : TransmissionPolicy
                this.MaxStorageCapacity = maxStorageCapacity;
            }
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
