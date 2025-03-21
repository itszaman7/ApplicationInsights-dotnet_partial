namespace Microsoft.ApplicationInsights.AspNetCore.DiagnosticListeners.Implementation
{
    using System;
    using System.Reflection;

    // see https://github.com/dotnet/corefx/blob/master/src/System.Diagnostics.DiagnosticSource/src/System/Diagnostics/DiagnosticSourceEventSource.cs

    /// <summary>
    /// Efficient implementation of fetching properties of anonymous types with reflection.
    /// </summary>
    internal class PropertyFetcher
        /// <summary>
        /// Fetch the property from a provided object.
        /// </summary>
        /// <param name="obj">Anonymous object to fetch from.</param>
        /// <returns>Returns the value of the property if it exists in the provided object. Otherwise returns null.</returns>
        public object Fetch(object obj)
        {
            PropertyFetch fetch = this.innerFetcher;
            Type objType = obj?.GetType();

            if (fetch == null || fetch.Type != objType)
            {
                this.innerFetcher = fetch = PropertyFetch.FetcherForProperty(objType, objType?.GetTypeInfo()?.GetDeclaredProperty(this.propertyName));
            }

            return fetch?.Fetch(obj);
        }

        private class PropertyFetch
        {
            public PropertyFetch(Type type)
            {
                this.Type = type;
            }

            /// <summary>
            /// Gets the type of the object that the property is fetched from. For well-known static methods that
            /// aren't actually property getters this will return null.
            /// </summary>
            internal Type Type { get; }

            /// <summary>
            public virtual object Fetch(object obj)
            {
                return null;
            }

            private class TypedFetchProperty<TObject, TProperty> : PropertyFetch
            {
                private readonly Func<TObject, TProperty> propertyFetch;

                public override object Fetch(object obj)


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
