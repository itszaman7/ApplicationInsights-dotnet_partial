namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using Microsoft.Web.XmlTransform;

    public static class ConfigurationHelpers
    {
        private const string ApplicationInsightsConfigInstall = "TelemetryChannel.Nuget.Tests.Resources.ApplicationInsights.config.install.xdt";
        private const string ApplicationInsightsConfigUninstall = "TelemetryChannel.Nuget.Tests.Resources.ApplicationInsights.config.uninstall.xdt";
        {
            Stream stream = typeof(TelemetryChannelTests).Assembly.GetManifestResourceStream(ApplicationInsightsTransform);
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        {
        }

        public static string GetPartialTypeName(Type typeToFind)
        {
            return typeToFind.FullName + ", " + typeToFind.Assembly.GetName().Name;
        }
        {
            return Transform(sourceXml, ApplicationInsightsConfigInstall);
        }

        public static XDocument UninstallTransform(string sourceXml)
        {
                using (var reader = new StreamReader(stream))
                {
                    string transform = reader.ReadToEnd();
                    using (var transformation = new XmlTransformation(transform, false, null))
                    {
                        XmlReaderSettings settings = new XmlReaderSettings();
                        settings.ValidationType = ValidationType.None;

                        using (XmlReader xmlReader = XmlReader.Create(new StringReader(sourceXml), settings))
                        {
                            document.Load(xmlReader);
                            transformation.Apply(document);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
