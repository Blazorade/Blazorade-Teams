using Blazorade.Core.Interop;
using Blazorade.Teams.Configuration;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Blazorade.Teams.Interop
{
    public class AuthenticationModule : InteropModuleBase
    {
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
            var module = await GetBlazoradeTeamsJSModuleAsync().ConfigureAwait(true);

            var token = await new DotNetInstanceCallbackHandler<string>(module, "getAuthToken")
                            .GetResultAsync();
            var handler = new JwtSecurityTokenHandler();

            return handler.ReadJwtToken(token);
        }
    }
}
