using Blazorade.Core.Interop;
using Blazorade.Teams.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        public async Task<JwtSecurityToken> GetAuthenticationResultAsync(Context context)
        {
            var module = await GetBlazoradeTeamsJSModuleAsync().ConfigureAwait(true);
            //var module = await this.GetBlazoradeMsalModuleAsync().ConfigureAwait(true);
            //var data = new Dictionary<string, object>
            //{
            //    { "context", context },
            //    { "config", new MsalConfig(this.ApplicationSettings) }
            //};

            var token = await new DotNetInstanceCallbackHandler<string>(module, "getAuthToken").GetResultAsync();
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(token);

            //return null;

            //return await new DotNetInstanceCallbackHandler<AuthenticationResult>(module, "getTokenSilent", data: data)
            //    .GetResultAsync();
        }

    }
}
