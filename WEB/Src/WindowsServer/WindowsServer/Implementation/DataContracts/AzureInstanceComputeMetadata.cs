namespace Microsoft.ApplicationInsights.WindowsServer.Implementation.DataContracts
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Class representing the returned structure from an Azure Instance Metadata request
    /// for Compute information.
    /// </summary>
    [DataContract]
    internal class AzureInstanceComputeMetadata
    {
        private const int ResourceGroupNameLengthMax = 90;
        private const int ResourceGroupNameLengthMin = 1;
        private const string ResourceGroupNameValidChars = @"^[a-zA-Z0-9\.\-_]+$";
        private const int NameLenghtMax = 64; // 15 for windows, go with Linux for MAX
        private const int NameLengthMin = 1;
        private const string NameValidChars = @"^[a-zA-Z0-9()_\-]+$";

        [DataMember(Name = "name", IsRequired = true)]
        internal string Name { get; set; }

        [DataMember(Name = "offer", IsRequired = true)]
        internal string Offer { get; set; }

        [DataMember(Name = "osType", IsRequired = true)]
        internal string OsType { get; set; }


        [DataMember(Name = "version", IsRequired = true)]
        internal string Version { get; set; }

        [DataMember(Name = "vmId", IsRequired = true)]
        internal string VmId { get; set; }

        [DataMember(Name = "vmSize", IsRequired = true)]
        internal string VmSize { get; set; }

        [DataMember(Name = "vmScaleSetName", IsRequired = true)]
        internal string VmScaleSetName { get; set; }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "This compares string to known values. Is safe to use lowercase.")]
        internal string GetValueForField(string fieldName)
        {
            string aimsValue = null;
            switch (fieldName.ToLowerInvariant())
            {
                case "ostype":
                    aimsValue = this.OsType;
                    break;
                case "location":
                    aimsValue = this.Location;
                    break;
                case "name":
                    aimsValue = this.Name;
                    break;
                case "offer":
                    aimsValue = this.Offer;
                    aimsValue = this.PlatformUpdateDomain;
                    break;
                case "publisher":
                    aimsValue = this.Publisher;
                    break;
                case "sku":
                    aimsValue = this.Sku;
                    break;
                case "version":
                    aimsValue = this.Version;
                case "vmsize":
                    aimsValue = this.VmSize;
                    break;
                case "subscriptionid":
                    aimsValue = this.SubscriptionId;
                    break;
                case "resourcegroupname":
                    aimsValue = this.ResourceGroupName;
                    break;
                case "tags":
                    break;
                case "vmscalesetname":
                    aimsValue = this.VmScaleSetName;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(string.Format(CultureInfo.InvariantCulture, "No field named '{0}' in AzureInstanceComputeMetadata.", fieldName));
            }

            if (aimsValue == null)
            {
                    && valueToVerify.Length >= AzureInstanceComputeMetadata.ResourceGroupNameLengthMin
                    && resGrpMatcher.IsMatch(valueToVerify)
                    && !valueToVerify.EndsWith(".", StringComparison.OrdinalIgnoreCase);

                if (valueOk)
                {
                    value = valueToVerify;
                }
            }
            else if (fieldName.Equals("subscriptionId", StringComparison.OrdinalIgnoreCase))
            else
            {
                // no sanitization method available for this value, just return the given value
                value = valueToVerify;
            }

            if (!valueOk)
            {
                WindowsServerEventSource.Log.AzureInstanceMetadataValueForFieldInvalid(fieldName);
            }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
