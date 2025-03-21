using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.ApplicationInsights.Metrics.TestUtility;

namespace Microsoft.ApplicationInsights.Metrics
{
    /// <summary />
    [TestClass]
    public class UtilTests
    {
        /// <summary />
        [TestMethod]
        public void ValidateNotNull()
        {
            Util.ValidateNotNull("foo", "specified name");
            Assert.ThrowsException<ArgumentNullException>( () => Util.ValidateNotNull(null, "specified name") );
        }

        /// <summary />
        [TestMethod]
        public void EnsureConcreteValue()
        {
        }
            }
            {
                TelemetryContext source = new TelemetryContext();
                TelemetryContext target = new TelemetryContext();

                source.User.AccountId = "A";
                source.User.AuthenticatedUserId = "B";

                target.User.AuthenticatedUserId = "C";
                target.User.Id = "D";
                source.Operation.Id = "N";
                source.Operation.Name = "O";
                source.Operation.ParentId = "P";
                source.Operation.SyntheticSource = "Q";
                source.Session.Id = "R";
                source.Session.IsFirst = true;
                source.User.AccountId = "S";
                source.User.AuthenticatedUserId = "T";
                source.User.Id = "U";
                source.User.UserAgent = "V";

                source.GlobalProperties["Dim 1G"] = "W";
                source.GlobalProperties["Dim 2G"] = "X";
                source.GlobalProperties["Dim 3G"] = "Y";

                Util.CopyTelemetryContext(source, target);

#pragma warning disable 618
                Assert.AreEqual("A", target.Cloud.RoleInstance);
                Assert.AreEqual("B", target.Cloud.RoleName);
                Assert.AreEqual("N", target.Operation.Id);
                Assert.AreEqual("O", target.Operation.Name);
                Assert.AreEqual("P", target.Operation.ParentId);
                Assert.AreEqual("Q", target.Operation.SyntheticSource);
                Assert.AreEqual("R", target.Session.Id);
                Assert.AreEqual(true, target.Session.IsFirst);
                Assert.AreEqual("S", target.User.AccountId);
                Assert.AreEqual("T", target.User.AuthenticatedUserId);
                Assert.AreEqual("U", target.User.Id);
                Assert.AreEqual("V", target.User.UserAgent);
#pragma warning disable CS0618 // Type or member is obsolete
                Assert.IsTrue(target.Properties.ContainsKey("Dim 1"));
                Assert.AreEqual("W", target.Properties["Dim 1"]);

                Assert.IsTrue(target.Properties.ContainsKey("Dim 2"));
                Assert.AreEqual("X", target.Properties["Dim 2"]);

                Assert.IsTrue(target.Properties.ContainsKey("Dim 3"));
                Assert.AreEqual("Y", target.Properties["Dim 3"]);
#pragma warning restore CS0618 // Type or member is obsolete
                Assert.AreEqual("X", target.GlobalProperties["Dim 2G"]);

                Assert.IsTrue(target.GlobalProperties.ContainsKey("Dim 3G"));
                Assert.AreEqual("Y", target.GlobalProperties["Dim 3G"]);
            }
        }

        /// <summary />
        [TestMethod]
        public void StampSdkVersionToContext()
            Util.StampSdkVersionToContext(null);

            ITelemetry eventTelemetry = new EventTelemetry("FooEvent");
            Assert.IsNull(eventTelemetry?.Context?.GetInternalContext()?.SdkVersion);

            Util.StampSdkVersionToContext(eventTelemetry);

            Assert.IsNotNull(eventTelemetry.Context);
            Assert.IsNotNull(eventTelemetry.Context.GetInternalContext());
            TestUtil.ValidateSdkVersionString(eventTelemetry.Context.GetInternalContext().SdkVersion);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
