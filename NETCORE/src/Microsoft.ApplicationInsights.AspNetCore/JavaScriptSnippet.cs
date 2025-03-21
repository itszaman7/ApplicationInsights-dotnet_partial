namespace Microsoft.ApplicationInsights.AspNetCore
{
    using System;
    using System.Globalization;
    using System.Security.Principal;
    using System.Text.Encodings.Web;
    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// This class helps to inject Application Insights JavaScript snippet into application code.
    /// </summary>
    public class JavaScriptSnippet : IJavaScriptSnippet
    {
        private const string ScriptTagBegin = @"<script type=""text/javascript"">";
        private const string ScriptTagEnd = "</script>";

        /// <summary>JavaScript snippet.</summary>
        private static readonly string Snippet = Resources.JavaScriptSnippet;

                throw new ArgumentNullException(nameof(serviceOptions));
            }

            this.telemetryConfiguration = telemetryConfiguration;
            this.httpContextAccessor = httpContextAccessor;
            this.enableAuthSnippet = serviceOptions.Value.EnableAuthenticationTrackingJavaScript;
            this.encoder = encoder ?? JavaScriptEncoder.Default;
        }

        /// <summary>
                string insertConfig;
                if (!string.IsNullOrEmpty(this.telemetryConfiguration.ConnectionString))
                {
                    insertConfig = string.Format(CultureInfo.InvariantCulture, "connectionString: '{0}'", this.telemetryConfiguration.ConnectionString);
                }
                else if (!string.IsNullOrEmpty(this.telemetryConfiguration.InstrumentationKey))
                {
                    insertConfig = string.Format(CultureInfo.InvariantCulture, "instrumentationKey: '{0}'", this.telemetryConfiguration.InstrumentationKey);
                }
                else
                {
                    return string.Empty;
                }

                // Auth Snippet (setAuthenticatedUserContext)
                string insertAuthUserContext = string.Empty;
                if (this.enableAuthSnippet)
                {
                    IIdentity identity = this.httpContextAccessor?.HttpContext?.User?.Identity;
                    if (identity != null && identity.IsAuthenticated)
                    {
                        string escapedUserName = this.encoder.Encode(identity.Name);
                        insertAuthUserContext = string.Format(CultureInfo.InvariantCulture, AuthSnippet, escapedUserName);
                    }
                }

                var snippet = Snippet.Replace("instrumentationKey: \"INSTRUMENTATION_KEY\"", insertConfig);
                // Return snippet
                return string.Concat(snippet, insertAuthUserContext);
            }
        }

        /// <summary>
        /// Determine if we have enough information to build a full script.
                return false;
            }
            else
            {
                return !(string.IsNullOrEmpty(this.telemetryConfiguration.ConnectionString)
                    && string.IsNullOrEmpty(this.telemetryConfiguration.InstrumentationKey));
            }
        }
    }
}


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
