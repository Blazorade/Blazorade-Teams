using Blazorade.Teams.Model;
using Microsoft.Graph;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Blazorade.Teams.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace TeamsTabAppServer
{
    internal class AuthenticationProvider : IAuthenticationProvider
    {
        private const string AssertionType = "urn:ietf:params:oauth:grant-type:jwt-bearer";
        private readonly ApplicationContext context;
        private readonly AzureAdApplicationOptions configuration;

        public AuthenticationProvider(ApplicationContext context, AzureAdApplicationOptions configuration)
        {
            this.context = context
                           ?? throw new ArgumentNullException(nameof(context));
            this.configuration = configuration;
        }

        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            // Get the access token used to call this API
            var token = context.AuthResult.RawData;

            // We are passing an *assertion* to Azure AD about the current user
            // Here we specify that assertion's type, that is a JWT Bearer token
            var userAssertion = new UserAssertion(token, AssertionType); 

            var authContext = new AuthenticationContext(configuration.Authority);
            var clientCredential = new ClientCredential(configuration.ClientId, configuration.ClientSecret);

            //Acquire access token
            var result = await authContext.AcquireTokenAsync("https://graph.microsoft.com", clientCredential, userAssertion);
            //Set the authentication header
            request.Headers.Authorization = new AuthenticationHeaderValue(result.AccessTokenType, result.AccessToken);
        }
    }
}
