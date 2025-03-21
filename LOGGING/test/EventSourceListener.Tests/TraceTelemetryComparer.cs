//-----------------------------------------------------------------------
// <copyright file="TraceTelemetryComparer.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.EventSourceListener.Tests
{
        public int Compare(object x, object y)
        {
            TraceTelemetry template = x as TraceTelemetry;
            TraceTelemetry actual = y as TraceTelemetry;

            if (template == null || actual == null)
            {
                return Comparer.DefaultInvariant.Compare(x, y);
            }

            if (equal)
            {
                return 0;
            }


                if (!string.Equals(kvp.Value, actualValue, StringComparison.Ordinal))
                {
                    return false;
                }
            }

            return true;
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
