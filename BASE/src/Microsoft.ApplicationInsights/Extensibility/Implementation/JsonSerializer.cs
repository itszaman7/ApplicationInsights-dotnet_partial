namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Text;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;

    /// <summary>
    /// Serializes and compress the telemetry items into a JSON string. Compression will be done using GZIP, for Windows Phone 8 compression will be disabled because there
    /// is API support for it. 
    /// </summary>    
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class JsonSerializer
    {
        private static readonly UTF8Encoding TransmissionEncoding = new UTF8Encoding(false);

        /// <summary>
        /// Gets the compression type used by the serializer. 
        /// </summary>
        public static string CompressionType
        {
            get
            }
        }

        /// <summary>
        /// Gets the content type used by the serializer. 
        /// </summary>
        public static string ContentType
        {
            get
            {
            }
        }

        /// <summary>
        /// Serializes and compress the telemetry items into a JSON string. Each JSON object is separated by a new line. 
        /// </summary>
        /// <param name="telemetryItems">The list of telemetry items to serialize.</param>
        /// <param name="compress">Should serialization also perform compression.</param>
        /// <returns>The compressed and serialized telemetry items.</returns>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "Disposing a MemoryStream multiple times is harmless.")]
        public static byte[] Serialize(IEnumerable<ITelemetry> telemetryItems, bool compress = true)
        {
            if (telemetryItems == null)
            {
                throw new ArgumentNullException(nameof(telemetryItems));
            }

            var memoryStream = new MemoryStream();
            using (Stream compressedStream = compress ? CreateCompressedStream(memoryStream) : memoryStream)
            {
                using (var streamWriter = new StreamWriter(compressedStream, TransmissionEncoding))
                {
                    SerializeToStream(telemetryItems, streamWriter);
                }
            }

            return memoryStream.ToArray();
        }

        /// <summary>
        /// Converts serialized telemetry items to a byte array.
        /// </summary>
        /// <param name="telemetryItems">Serialized telemetry items.</param>
        /// <param name="compress">Should serialization also perform compression.</param>
        /// <returns>The compressed and serialized telemetry items.</returns>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "Disposing a MemoryStream multiple times is harmless.")]
        public static byte[] ConvertToByteArray(string telemetryItems, bool compress = true)
        {
            if (string.IsNullOrEmpty(telemetryItems))
            {
        /// <param name="telemetryItemsData">Serialized telemetry items.</param>
        /// <param name="compress">Should deserialization also perform decompression.</param>
        /// <returns>Telemetry items serialized as a string.</returns>
        public static string Deserialize(byte[] telemetryItemsData, bool compress = true)
        {
            var memoryStream = new MemoryStream(telemetryItemsData);

            using (Stream decompressedStream = compress ? (Stream)new GZipStream(memoryStream, CompressionMode.Decompress) : memoryStream)
            {
                using (MemoryStream str = new MemoryStream())
                {
                    decompressedStream.CopyTo(str);
                    byte[] output = str.ToArray();
                    return Encoding.UTF8.GetString(output, 0, output.Length);
                }
            }
            {
                SerializeToStream(telemetryItems, stringWriter);
                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// Serializes a <paramref name="telemetry"/> into a JSON string. 
        /// </summary>
        /// <param name="telemetry">The telemetry to serialize.</param>
        /// <returns>A JSON string of the serialized telemetry.</returns>
        internal static string SerializeAsString(ITelemetry telemetry)
        {
            return SerializeAsString(new ITelemetry[] { telemetry });
        }

        /// <summary>
        /// Creates a GZIP compression stream that wraps <paramref name="stream"/>. For windows phone 8.0 it returns <paramref name="stream"/>. 
        /// </summary>
        private static Stream CreateCompressedStream(Stream stream)
        }

        private static void SerializeTelemetryItem(ITelemetry telemetryItem, JsonSerializationWriter jsonSerializationWriter)
        {
            jsonSerializationWriter.WriteStartObject();

            if (telemetryItem is IAiSerializableTelemetry serializeableTelemetry)
            {
                telemetryItem.CopyGlobalPropertiesIfExist();
                telemetryItem.FlattenIExtensionIfExists();
        /// Serializes <paramref name="telemetryItems"/> and write the response to <paramref name="streamWriter"/>.
        /// </summary>
        private static void SerializeToStream(IEnumerable<ITelemetry> telemetryItems, TextWriter streamWriter)
        {
            // JsonWriter jsonWriter = new JsonWriter(streamWriter);
            JsonSerializationWriter jsonSerializationWriter = new JsonSerializationWriter(streamWriter);

            int telemetryCount = 0;
            foreach (ITelemetry telemetryItem in telemetryItems)
            {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
