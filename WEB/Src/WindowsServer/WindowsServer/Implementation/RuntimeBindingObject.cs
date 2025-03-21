#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer.Implementation
{
    using System;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// A runtime bound object for a given .NET type.
    /// </summary>
    internal abstract class RuntimeBindingObject :
        MarshalByRefObject
    {
        /// <summary>
        /// The target type for our object.
        /// </summary>
        private Type targetType;

        /// <summary>
        /// The target object.
        /// </summary>
        private object targetObject;

        /// <summary>
        /// The assembly which is loaded reflectively.
        /// </summary>
        private Assembly loadedAssembly;

        /// <summary>
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="activationArgs">The activation arguments.</param>
        /// <returns>The activated instance is one is required.</returns>
        protected abstract object GetTargetObjectInstance(Type targetType, object[] activationArgs);

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="args">The arguments.</param>
        {
            if (string.IsNullOrEmpty(name) == true)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (parameterTypes == null)
            {
                return this.InvokeHelper(name, bindingFlags | BindingFlags.GetProperty, args, null);
            }

            PropertyInfo info = this.targetType.GetProperty(name, bindingFlags, null, null, parameterTypes, null);
            if (info == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Could not get property info for '{0}' with the specified parameters.", name));
            }

            return info.GetValue(this.targetObject, args);
        }
        
        /// <param name="bindingFlags">The binding flags.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The return value for our invocation.</returns>
        private object InvokeHelper(string name, BindingFlags bindingFlags, object[] args, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(name) == true)
            {
                throw new ArgumentNullException(nameof(name));
            }

            object output;
            try
            {
                output = this.targetType.InvokeMember(name, bindingFlags, null, this.targetObject, args, culture);
            }
            catch (TargetInvocationException exception)
            {
                if (exception.InnerException == null)
                {
                    throw;
                }

                throw exception.InnerException;
            }

            return output;
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
