using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace Blazorade.Teams.Authentication
{
    internal class AuthenticationProvider : IAuthenticationProvider
    {
        private readonly AuthenticationHeaderValue header;

        public AuthenticationProvider(AuthenticationHeaderValue header)
        {
            this.header = header;
        }

        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            request.Headers.Authorization = header;
        }
    }
}