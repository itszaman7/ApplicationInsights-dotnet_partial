#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer.Implementation
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Represents an instance of a role. 
    /// </summary>
    internal class RoleInstance :
        RuntimeBindingObject
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleInstance"/> class.
        /// </summary>
        /// <param name="targetObject">The target object.</param>
        /// <param name="loadedAssembly">The loaded assembly.</param>
        {
            get
            {
                if (this.TargetObject == null)
                {
        /// </summary>
        public Role Role
        {
            get
            {

                object role = this.GetProperty("Role");
        /// Gets the target object instance.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="activationArgs">The activation arguments.</param>
        /// <returns>
        /// The activated instance is one is required.
        /// </returns>
        protected override object GetTargetObjectInstance(Type targetType, object[] activationArgs)
        {
            return activationArgs[0];


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
