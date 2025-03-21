namespace Microsoft.ApplicationInsights.Web.TestFramework
{
    using System;

    internal class StubStream : MemoryStream
        public Action<bool> OnDispose;

        {
            this.OnDispose = disposing => { };
        {
            this.OnDispose(disposing);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
