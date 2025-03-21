// -----------------------------------------------------------------------
// <copyright file="XdtTests.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. 
// All rights reserved.  2014
// </copyright>
// -----------------------------------------------------------------------

namespace Xdt.Tests
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Web.XmlTransform;

    [TestClass]
    public class XdtTests
    {
        [TestMethod]
        [TestCategory("XdtTests")]
        public void XdtTraceListenerAppTest()
        [TestCategory("XdtTests")]
        public void XdtTraceListenerWebTest()
        {
            this.ValidateTransform(
                ".TraceListener.web.config.install.xdt",
                ".TraceListener.web.config.uninstall.xdt",
                ".TraceListener.TestDataSet.xml");
        }

        [TestMethod]

        [TestMethod]
        [TestCategory("XdtTests")]
        public void XdtLog4NetWebTest()
        {
            this.ValidateTransform(
               ".Log4Net.web.config.install.xdt",
               ".Log4Net.web.config.uninstall.xdt",
               ".Log4Net.TestDataSet.xml");
        }

        [TestMethod]
        [TestCategory("XdtTests")]
        public void XdtLog4NetAppTest()
        {
            this.ValidateTransform(
               ".Log4Net.app.config.install.xdt",
               ".Log4Net.app.config.uninstall.xdt",
               ".Log4Net.TestDataSet.xml");
        }

        [TestMethod]
        [TestCategory("XdtTests")]
        public void DiagnosticSourceListenerTest()
        {
            this.ValidateTransform(
               ".DiagnosticSourceListener.ApplicationInsights.config.install.xdt",
               ".DiagnosticSourceListener.ApplicationInsights.config.uninstall.xdt",
               ".DiagnosticSourceListener.TestDataSet.xml");
        }

            var dataSetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(dataSetName);
            var dataSetXml = XElement.Load(dataSetStream);

            using (var installXdtStreamReader = new StreamReader(installXdtStream))
            {
                using (var uninstallXdtStreamReader = new StreamReader(uninstallXdtStream))
                {
                    var installTransformation = new XmlTransformation(installXdtStreamReader.ReadToEnd(), false, null);
                    var uninstallTransformation = new XmlTransformation(uninstallXdtStreamReader.ReadToEnd(), false, null);
                        var expectedPostTransform = GetInnerXml(item.XPathSelectElement("./expectedPostTransform"));

                        var targetDocument = new XmlDocument();
                        targetDocument.LoadXml(original);

                        bool success = installTransformation.Apply(targetDocument);
                        Assert.IsTrue(success, "Transformation (install) has failed. XDT: {0}, XML: {1}", installXdtName, item);

                        // validate the transformation result
                        Assert.IsTrue(
                                expectedPostTransform,
                                targetDocument.OuterXml,
                            Environment.NewLine,
                            expectedPostTransform,
                            targetDocument.OuterXml);

                        var transformedDocument = targetDocument.OuterXml;

                        // apply uninstall transformation
                        success = uninstallTransformation.Apply(targetDocument);
                        Assert.IsTrue(success, "Transformation (uninstall) has failed. XDT: {0}, XML: {1}", uninstallXdtName, transformedDocument);

                                targetDocument.OuterXml,
                                StringComparison.InvariantCulture),
                            "Unexpected transform (uninstall) result. Expected:{0}{1}{0}{0} Actual:{0}{2}",
                            Environment.NewLine,
                            original,
                            targetDocument.OuterXml);
                    }
                }
            }
        }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
