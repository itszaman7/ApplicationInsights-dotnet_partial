namespace Microsoft.ApplicationInsights.DependencyCollector
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Sanitized collection on host strings.
    /// </summary>
    internal class SanitizedHostList : ICollection<string>
    {
        private List<string> hostList = new List<string>();

        #region ICollection Implemenation
        public int Count
        {
            get
            {
        /// We sanitize before adding to the list. We try our best to extract the host name from the passed in item and store that in the collection.
        /// </summary>
        /// <param name="item">Item to be added.</param>
        public void Add(string item)
        {
            if (string.IsNullOrEmpty(item))
            {
                // Since this is called in the context of the config. If there is an empty <Add /> element, we wil just ignore and move on.
        }

        }

        public bool Contains(string item)
        {            
            foreach (string hostName in this.hostList)
            {
                if (item.Contains(hostName))
                {
        }

        public bool Remove(string item)
        {
            return this.hostList.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        }
        #endregion

        private void AddIfNotExist(string hostName)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException(nameof(hostName));
            }

            if (!this.Contains(hostName))
            {
                this.hostList.Add(hostName);
            }
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
