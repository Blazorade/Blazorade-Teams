using Blazorade.Teams.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Interop
{
    public class AuthenticationModule : InteropModuleBase
    {
        public AuthenticationModule(AzureAdApplicationOptions appOptions, IJSRuntime jsRuntime) : base(appOptions, jsRuntime) { }


        /// <summary>
        /// Uses MSAL.js to authenticate the user specified in <paramref name="context"/> and returns the result
        /// of that authentication.
        /// </summary>
        /// <param name="context">The Teams context to use when resolving the token.</param>
        /// <remarks>
        /// The method attempts to perform the authentication silently, but will fall back to using a popup dialog
        /// if the authentication did not succeed.
        /// </remarks>
        public async Task<AuthenticationResult> GetAuthenticationResultAsync(Context context)
        {
            var module = await this.GetBlazoradeMsalModuleAsync();
            return await new CallbackProxy<AuthenticationResult>(await this.GetBlazoradeMsalModuleAsync())
                .GetResultAsync(
                    "getTokenSilent",
                    args: new Dictionary<string, object>
                    {
                        { "context", context },
                        { "config", new MsalConfig(this.ApplicationSettings) }
                    }
                );
        }

    }
}
