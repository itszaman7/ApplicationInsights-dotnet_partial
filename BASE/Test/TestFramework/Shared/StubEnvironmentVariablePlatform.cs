namespace Microsoft.ApplicationInsights.TestFramework
    using System.Collections.Generic;

    {
        private readonly Dictionary<string, string> environmentVariables = new Dictionary<string, string>();

        public void SetEnvironmentVariable(string name, string value) => this.environmentVariables.Add(name, value);
    }
}

# This file contains partial code from the original project
# Some functionality may be missing or incomplete
