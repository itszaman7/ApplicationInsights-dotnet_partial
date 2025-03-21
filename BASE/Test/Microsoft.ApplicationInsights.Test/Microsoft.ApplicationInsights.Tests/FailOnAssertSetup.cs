// <copyright file="FailOnAssertSetup.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Microsoft.ApplicationInsights
{
#if NETFRAMEWORK
    using System;
    using System.Diagnostics;
    using System.Linq;
            // Fail on Debug.Assert rather than popping up a window
            var defaultListener = Debug.Listeners
                .OfType<DefaultTraceListener>()
                .FirstOrDefault();
            if (defaultListener != null)
            {
                Debug.Listeners.Remove(defaultListener);
                Debug.Listeners.Add(new FailOnDebugAssertTraceListener());

        /// <summary>
        /// Converts Debug.Assert into a test failure.
        /// </summary>
            public override void Write(string message)
            {
                Console.Write(message);

            public override void WriteLine(string message)
            {
                Console.WriteLine(message);
        }
    }
#endif
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
