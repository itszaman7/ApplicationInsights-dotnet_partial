﻿namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System.Diagnostics.Tracing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;
    [TestClass]
    public class F5DiagnosticsSenderTest
    {
        [TestMethod]
                    EventId = 10,
                    Keywords = 0x20,
                    Level = EventLevel.Warning,
                Payload = new[] { "My function", "some failure" }
            };

            senderMock.Send(evt);
            Assert.AreEqual(1, senderMock.Messages.Count);
            Assert.AreEqual("Error occurred at My function, some failure", senderMock.Messages[0]);
        }

                    Level = EventLevel.Warning,
                    MessageFormat = "Error occurred"
                },
                Payload = null
            };

            senderMock.Send(evt);
            Assert.AreEqual(1, senderMock.Messages.Count);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
