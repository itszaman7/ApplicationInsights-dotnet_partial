namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;

        {
            this.Name = GetApplicationName();
        }
            //// It is prohibited by EventSource rules to have more parameters in WriteEvent that in event source method
            //// Parameter will be available in payload but in the next versions EventSource may 
            //// start validating that number of parameters match
            //// It is not allowed to call additional methods, only WriteEvent

            string name;
            }
            catch (Exception exp)
            {
                name = "Undefined " + exp.Message ?? exp.ToString();
            }



# This file contains partial code from the original project
# Some functionality may be missing or incomplete
