// -----------------------------------------------------------------------
// <copyright file="SpinWait.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>
namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule
{
    using System;
    using System.Threading;
        internal static void ExecuteSpinWaitLock(this object syncRoot, Action action)
        {
            while (!Monitor.TryEnter(syncRoot, 0))
            }

                action();
            }
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
