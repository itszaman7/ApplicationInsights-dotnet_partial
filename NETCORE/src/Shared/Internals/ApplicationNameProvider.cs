namespace Microsoft.ApplicationInsights.Shared.Internals
{
    using System;
    using System.Reflection;

    /// This class provides the assembly name for the EventSource implementations.
    /// </summary>
    internal sealed class ApplicationNameProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationNameProvider"/> class.
        /// </summary>
        public ApplicationNameProvider()
        {
            this.Name = GetApplicationName();
        /// </summary>
        public string Name { get; private set; }
            }
            catch (Exception exp)


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
