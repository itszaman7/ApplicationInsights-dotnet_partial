namespace FunctionalTests.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
        public bool FailureDetected { get; set; }

        public TelemetryHttpListenerObservable(string url, ITestOutputHelper output) : base(url, output)
        {
        }        
        protected override IEnumerable<Envelope> CreateNewItemsFromContext(HttpListenerContext context)
        {
            try
            {
                var request = context.Request;
                string content = string.Empty;

                if (!string.IsNullOrWhiteSpace(request.Headers["Content-Encoding"]) &&
                    string.Equals("gzip", request.Headers["Content-Encoding"],
                        StringComparison.InvariantCultureIgnoreCase))
                }

                Trace.WriteLine("=>");
            {
                context.Response.Close();
            }
        }

        {
            var gzipStream = new GZipStream(request.InputStream, CompressionMode.Decompress);
            using (var streamReader = new StreamReader(gzipStream, request.ContentEncoding))
            {
                return streamReader.ReadToEnd();


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
