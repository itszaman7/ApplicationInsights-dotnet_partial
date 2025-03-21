//-----------------------------------------------------------------------
// <copyright file="EventSourceNamingMatchRuleBase.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.EventSourceListener
{
        /// <param name="obj">Object to compare with.</param>
        /// <returns>True if the supplied object is equal to "this", otherwise false.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as EventSourceListeningRequestBase;
            if (other == null)
            {
                return false;
            }

            return string.Equals(this.Name, other.Name, System.StringComparison.Ordinal) && this.PrefixMatch == other.PrefixMatch;
        }
        /// <summary>
        /// Gets the hash code for the event source name matching rule.
        /// </summary>
            return this.Name.GetHashCode() ^ this.PrefixMatch.GetHashCode();
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
