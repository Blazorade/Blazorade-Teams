using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Blazorade.Teams.Configuration;
using Blazorade.Teams.Interop;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using UserAssertion = Microsoft.Identity.Client.UserAssertion;

namespace Blazorade.Teams.Authentication
{
    /// <summary>
    /// Exchange an authenticated user token for an on-behalf-of token for
    /// calling downstream API's
    /// </summary>
    public class OnBehalfOfFlow
    {
        private readonly BlazoradeTeamsInteropModule module;
        private const string AssertionType = "urn:ietf:params:oauth:grant-type:jwt-bearer";
        private readonly AzureAdApplicationOptions options;
        private IEnumerable<string> scopes = new List<string>
                                             {
                                                 "openid",
                                                 "offline_access",
                                                 "email",
                                                 "profile"
                                             };
        private string api;

        public OnBehalfOfFlow(IOptions<AzureAdApplicationOptions> options, BlazoradeTeamsInteropModule module)
        {
            this.module = module;
            this.options = options.Value;
        }

        /// <summary>
        /// Perform an On-Behalf-Of request with the following scopes for passing to the down stream api
        /// openid, offline_access, email, profile
        /// </summary>
        /// <param name="userToken"></param>
        /// <param name="api">api the requested scopes are valid for i.e. https://graph.mircosoft.com</param>
        /// <returns>Configured IAuthenticationProvider, or null when consent is required</returns>
        public async Task<IAuthenticationProvider> ExchangeToken(JwtSecurityToken userToken, string api = null)
        {
            this.api = api;

            // We are passing an *assertion* to Azure AD about the current user
            // Here we specify that assertion's type, that is a JWT Bearer token
            var userAssertion = new UserAssertion(userToken.RawData, AssertionType);

            var application = ConfidentialClientApplicationBuilder.Create(options.ClientId)
                                                                  .WithAuthority(options.Authority)
                                                                  .WithClientSecret(options.ClientSecret)
                                                                  .Build();

            try
            {
                //Acquire access token
                var result = await application.AcquireTokenOnBehalfOf(scopes, userAssertion)
                                              .ExecuteAsync();

                //Set the authentication header
                return  new AuthenticationProvider(new AuthenticationHeaderValue("Bearer", result.AccessToken));
            }
            catch (MsalUiRequiredException)
            {
                return await ObtainConsent();
            }
        }

        /// <summary>
        /// Perform an On-Behalf-Of request with specific scopes needed for the downstream api call.
        /// This allows for incremental consenting - a user will ultimately only consent to the permissions needed to get
        /// their job done
        /// </summary>
        /// <param name="userToken"></param>
        /// <param name="api">api the requested scopes are valid for i.e. https://graph.mircosoft.com</param>
        /// <param name="scopes"></param>
        /// <returns>Authentication header value with the bearer token set, or null when consent is required</returns>
        public async Task<IAuthenticationProvider> ExchangeToken(JwtSecurityToken userToken, IEnumerable<string> scopes, string api = null)
        {
            this.scopes = scopes;
            return await ExchangeToken(userToken, api);
        }

        private async Task<IAuthenticationProvider> ObtainConsent()
        {
            var result = await module.Authentication.PopupConsent(api, scopes);

            if (!result.Consented)
            {
                return null;
            }

            return new AuthenticationProvider(new AuthenticationHeaderValue("Bearer", result.Token));
        }
    }
}
