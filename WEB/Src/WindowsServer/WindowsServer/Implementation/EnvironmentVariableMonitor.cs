namespace Microsoft.ApplicationInsights.WindowsServer.Implementation
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Security;
    using System.Threading;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// Utility to monitor the value of environment variables which may change 
    /// during the run of an application. Checks the environment variables 
    /// intermittently.
    /// </summary>
    internal abstract class EnvironmentVariableMonitor : IDisposable
    {
        // Environment variables tracked by this monitor.
        protected readonly ConcurrentDictionary<string, string> CheckedValues;

        // help ensure no Timer problems by enforcing a minimum check interval
        protected readonly TimeSpan MinimumCheckInterval = TimeSpan.FromSeconds(5);

        // enabled flag primarily used during dispose
        protected volatile bool isEnabled = true;

        // how often we allow the code to re-check the environment
        protected TimeSpan checkInterval;
            this.CheckedValues = new ConcurrentDictionary<string, string>();
            this.checkInterval = checkInterval > this.MinimumCheckInterval ? checkInterval : this.MinimumCheckInterval;

            foreach (string varName in envVars)
            {
                this.CheckedValues.TryAdd(varName, Environment.GetEnvironmentVariable(varName));
            }

            this.environmentCheckTimer = new Timer(this.CheckVariablesIntermittent, null, Timeout.Infinite, Timeout.Infinite);
            this.environmentCheckTimer.Change(checkInterval, TimeSpan.FromMilliseconds(-1));
        }

        /// <summary>
        /// Get the latest value assigned to an environment variable.
        /// </summary>
        /// <param name="envVarName">Name of the environment variable to acquire.</param>
        /// <param name="value">Current cached value of the environment variable.</param>
        public void GetCurrentEnvironmentVariableValue(string envVarName, ref string value)
        {
            value = this.CheckedValues.GetOrAdd(envVarName, (key) => { return Environment.GetEnvironmentVariable(key); });
                        envValue = Environment.GetEnvironmentVariable(kvp.Key);
                    }
                    catch (SecurityException e)
                    {
                        WindowsServerEventSource.Log.SecurityExceptionThrownAccessingEnvironmentVariable(kvp.Key, e.ToInvariantString());
                        this.isEnabled = false;
                        break;
                    }

                    if (envValue != null
                        && !envValue.Equals(kvp.Value, StringComparison.Ordinal)
                        && this.CheckedValues.TryUpdate(kvp.Key, envValue, kvp.Value))
                    {
                        shouldTriggerOnUpdate = true;
                    }
                }

                if (shouldTriggerOnUpdate)
                {
                    this.OnEnvironmentVariableUpdated();
                }
                this.isEnabled = false;

                if (this.environmentCheckTimer != null)
                {
                    try
                    {
                        this.environmentCheckTimer.Dispose();
                    }
                    catch (Exception e)
                    {


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
