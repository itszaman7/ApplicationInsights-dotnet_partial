namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Encapsulates logic for serializing objects to JSON. 
    /// </summary>
    public interface IJsonWriter
    {
        /// <summary>
        /// Writes opening/left square bracket.
        void WriteEndArray();

        /// <summary>
        /// Writes closing/right curly brace.
        /// </summary>
        void WriteEndObject();

        void WriteComma();

        /// <summary>
        /// Writes a <see cref="String"/> property.
        /// </summary>
        void WriteProperty(string name, string value);

        /// </summary>
        void WriteProperty(string name, double? value);

        /// <summary>
        /// Writes a <see cref="TimeSpan"/> property.
        /// </summary>
        void WriteProperty(string name, TimeSpan? value);
        /// Writes a <see cref="IDictionary{String, Double}"/> property.
        /// </summary>
        void WriteProperty(string name, IDictionary<string, double> values);

        /// <summary>
        /// Writes a <see cref="IDictionary{String, String}"/> property.
        /// </summary>
        /// </summary>
        void WritePropertyName(string name);

        /// <summary>
        /// Writes <see cref="Object"/> as raw value directly.
        /// </summary>
        void WriteRawValue(object value);


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
