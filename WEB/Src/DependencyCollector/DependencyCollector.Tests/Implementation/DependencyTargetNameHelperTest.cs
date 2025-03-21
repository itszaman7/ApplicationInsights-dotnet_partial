namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    [TestClass]
    public class DependencyTargetNameHelperTest
        {
            Uri httpPortDefaultUri = new Uri("http://www.microsoft.com");
            Uri httpsPortDefaultUri = new Uri("https://www.microsoft.com");
            Uri httpPortExplicitUri = new Uri("http://www.microsoft.com:80");
            Uri randomPortExplicitUri = new Uri("http://www.microsoft.com:1010");
            Assert.AreEqual(httpPortDefaultUri.Host, DependencyTargetNameHelper.GetDependencyTargetName(httpPortDefaultUri));
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
