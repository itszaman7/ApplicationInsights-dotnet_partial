namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Defines extension methods that allow coding against <see cref="Type"/> without conditional compilation on versions of .NET framework.
    /// </summary>
    internal static class TypeExtensions
        /// Returns all the public properties of the specified type.
        /// </summary>
        /// <remarks>
        /// This method emulates the built-in method of the <see cref="Type"/> class which is not available on Windows Runtime.
            var properties = new List<PropertyInfo>();
            properties.AddRange(type.GetTypeInfo().DeclaredProperties);
            var baseType = type.GetTypeInfo().BaseType;
            if (baseType != null)

        public static Type[] GetGenericArguments(this Type type)
        {
            return type.GetTypeInfo().GenericTypeArguments;
        }

        public static Type[] GetInterfaces(this Type type)
        {
            return type.GetTypeInfo().ImplementedInterfaces.ToArray();
        }

        public static bool IsAbstract(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
