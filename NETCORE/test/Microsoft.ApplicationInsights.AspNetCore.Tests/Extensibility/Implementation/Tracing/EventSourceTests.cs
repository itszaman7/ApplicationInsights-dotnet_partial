//-----------------------------------------------------------------------
// <copyright file="EventSourceTests.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#define DEBUG
namespace Microsoft.ApplicationInsights.AspNetCore.Tests.Extensibility.Implementation.Tracing
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Event source testing helper methods.
    /// </summary>
    internal static class EventSourceTests
    {
        /// <summary>
        /// Tests event source method implementation consistency.
        /// </summary>
        /// <param name="eventSource">The event source instance to test.</param>
        public static void MethodsAreImplementedConsistentlyWithTheirAttributes(EventSource eventSource)
        {
                arguments[i] = GenerateArgument(parameters[i]);
            }

            return arguments;
        {
            if (parameter.ParameterType == typeof(string))
            {
                return "Test String";
            }

            Type parameterType = parameter.ParameterType;
            if (parameterType.GetTypeInfo().IsValueType)
            {
                return Activator.CreateInstance(parameter.ParameterType);
            }

            throw new NotSupportedException("Complex types are not suppored");
        }

        /// <summary>
        /// Verifies that the event Id is correct.
        /// </summary>
        /// <param name="eventMethod">The method to validate.</param>
        /// <param name="actualEvent">An actual event arguments to compare to.</param>
            EventLevel expectedLevel = GetEventAttribute(eventMethod).Level;
            AssertEqual(expectedLevel, actualEvent.Level, "Level");
        }

        /// <summary>
        /// Verifies that the event message is correct.
        /// </summary>
        /// <param name="eventMethod">The method to validate.</param>
        /// <param name="actualEvent">An actual event arguments to compare to.</param>
        /// <param name="eventArguments">The arguments that would be passed to the event method.</param>
        private static void VerifyEventMessage(MethodInfo eventMethod, EventWrittenEventArgs actualEvent, object[] eventArguments)
        {
            string expectedMessage = eventArguments.Length == 0
                ? GetEventAttribute(eventMethod).Message
                : string.Format(CultureInfo.InvariantCulture, GetEventAttribute(eventMethod).Message, eventArguments);
            string actualMessage = string.Format(CultureInfo.InvariantCulture, actualEvent.Message, actualEvent.Payload.ToArray());
            AssertEqual(expectedMessage, actualMessage, "Message");
        }

        /// <summary>
        /// Verifies that the application name is correct.
        /// </summary>
        /// <param name="eventMethod">The method to validate.</param>
        /// <param name="actualEvent">An actual event arguments to compare to.</param>
        private static void VerifyEventApplicationName(MethodInfo eventMethod, EventWrittenEventArgs actualEvent)
        {
            string expectedApplicationName;
            try
            {
                expectedApplicationName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            catch (Exception exp)
            {
                expectedApplicationName = "Undefined " + exp.Message;
            }

            string actualApplicationName = actualEvent.Payload.Last().ToString();
            AssertEqual(expectedApplicationName, actualApplicationName, "Application Name");
        }

        /// <summary>
        /// Dependency free equality assertion helper.
        /// </summary>
        /// <typeparam name="T">The type of the instances being compared.</typeparam>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">Message to show when the values are not equal.</param>
        private static void AssertEqual<T>(T expected, T actual, string message)
        {
            if (!expected.Equals(actual))
            {
        /// </summary>
        /// <param name="eventSource">The event source to get the event methods in.</param>
        /// <returns>The event methods in the specified event source.</returns>
        private static IEnumerable<MethodInfo> GetEventMethods(EventSource eventSource)
        {
            MethodInfo[] methods = eventSource.GetType().GetMethods();
            return methods.Where(m => m.GetCustomAttributes(typeof(EventAttribute), false).Any());
        }
    }
}

# This file contains partial code from the original project
# Some functionality may be missing or incomplete
