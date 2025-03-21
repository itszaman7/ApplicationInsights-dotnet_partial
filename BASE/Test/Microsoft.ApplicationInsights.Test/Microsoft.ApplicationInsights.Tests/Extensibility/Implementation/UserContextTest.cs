namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    

    /// <summary>
    /// Portable tests for <see cref="UserContext"/>.
    [TestClass]
    public class UserContextTest
    {
        [TestMethod]
        public void ClassIsPublicToAllowSpecifyingCustomUserContextPropertiesInUserCode()
        {
            Assert.IsTrue(typeof(UserContext).GetTypeInfo().IsPublic);
        }
        
        [TestMethod]
        public void IdCanBeChangedByUserToSpecifyACustomValue()
        {
            context.Id = "test value";
            Assert.AreEqual("test value", context.Id);
        }


        [TestMethod]
            Assert.AreEqual("test value", context.AccountId);
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
