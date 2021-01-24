using System.Collections.Generic;
using Blazorade.Core.Interop;
using Blazorade.Teams.Configuration;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Blazorade.Teams.Model;
using Microsoft.Extensions.Options;

namespace Blazorade.Teams.Interop
{
    public class AuthenticationModule : InteropModuleBase
    {
        private IJSObjectReference module;

        public AuthenticationModule(IOptions<AzureAdApplicationOptions> appOptions, IJSRuntime jsRuntime)
            : base(appOptions, jsRuntime)
        {
        }

        /// <summary>
        /// Use the TeamsSDK to retrieve the authentication token, triggering a consent dialog if required for the
        /// following (currently) supported scopes - email, profile, offline_access, OpenId
        /// </summary>
        /// <remarks>
        /// The method attempts to perform the authentication silently, but will fall back to using a popup dialog
        /// if the authentication did not succeed.
        /// Not all scopes assigned to an Application will trigger the consent popup. The current work around
        /// is to have the Admin grant consent for the Tenant
        /// </remarks>
        public async Task<JwtSecurityToken> GetAuthenticationResultAsync()
        {
            await LoadBlazorTeamsJavascript();
            var token = await new DotNetInstanceCallbackHandler<string>(module, "getAuthToken")
                            .GetResultAsync();
            var handler = new JwtSecurityTokenHandler();

            return handler.ReadJwtToken(token);
        }

        internal async Task<ConsentResult> PopupConsent(string api, IEnumerable<string> scopes)
        {
            await LoadBlazorTeamsJavascript();
            var data = new Dictionary<string, object>
                       {
                           {"scopes", string.Join("+", scopes)}
                       };

            if (!string.IsNullOrEmpty(api))
            {
                data.Add("api", api);
            }

            var result = await new DotNetInstanceCallbackHandler<ConsentResult>(module, "showConsentDialog", data)
                               .GetResultAsync()
                               .ConfigureAwait(true);

            return result;
        }

        /// <summary>
        /// Call microsoftTeams.authentication.notifySuccess with the new token retrieved
        /// from the consenting process
        /// </summary>
        /// <param name="accessToken"></param>
        public async Task NotifyConsentSuccess(string accessToken)
        {
            await LoadBlazorTeamsJavascript();

            var args = new DotNetInstanceCallbackArgs
            {
                Data = new Dictionary<string, object>
                                  {
                                      {"token", accessToken}
                                  },
            };

            await module.InvokeVoidAsync("notifyConsentSuccess", args);
        }

        /// <summary>
        /// Call  microsoftTeams.authentication.notifyFailure when the user has not consented to
        /// the requested permission/s
        /// </summary>
        /// <param name="error"></param>
        public async Task NotifyConsentFailure(string error)
        {
            await LoadBlazorTeamsJavascript();

            var args = new DotNetInstanceCallbackArgs
                       {
                           Data = new Dictionary<string, object>
                                  {
                                      {"error", error}
                                  },
                       };

            await module.InvokeVoidAsync("notifyConsentFailure", args);
        }

        private async Task LoadBlazorTeamsJavascript()
        {
            module ??= await GetBlazoradeTeamsJSModuleAsync().ConfigureAwait(true);
        }
    }
}
