// <copyright file="AdapterHelper.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Microsoft.ApplicationInsights.Tracing.Tests
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using static System.Globalization.CultureInfo;

#else
        private static readonly string ApplicationInsightsConfigFilePath =
            Path.Combine(Path.GetDirectoryName(typeof(AdapterHelper).GetTypeInfo().Assembly.Location), "ApplicationInsights.config");
#endif

        public AdapterHelper(string instrumentationKey = "F8474271-D231-45B6-8DD4-D344C309AE69")
        {
            this.InstrumentationKey = instrumentationKey;

            string configuration = string.Format(InvariantCulture,
                                    @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                                     <ApplicationInsights xmlns=""http://schemas.microsoft.com/ApplicationInsights/2013/Settings"">
                                        <InstrumentationKey>{0}</InstrumentationKey>
                                     </ApplicationInsights>",
                                     instrumentationKey);

            File.WriteAllText(ApplicationInsightsConfigFilePath, configuration);
            this.Channel = new CustomTelemetryChannel();
            int totalMillisecondsToWait = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;
            const int IterationMilliseconds = 250;

            while (totalMillisecondsToWait > 0)
            {
                sentItems = adapterHelper.Channel.SentItems;
                if (sentItems.Length > 0)
                {

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Channel.Dispose();

                if (File.Exists(ApplicationInsightsConfigFilePath))


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
