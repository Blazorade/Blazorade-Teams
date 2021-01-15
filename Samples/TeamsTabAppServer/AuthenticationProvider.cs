using Blazorade.Teams.Model;
using Microsoft.Graph;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TeamsTabAppServer
{
internal class AuthenticationProvider : IAuthenticationProvider
{
    public AuthenticationProvider(ApplicationContext context)
    {
        this.Context = context
                ?? throw new ArgumentNullException(nameof(context));
    }

    private ApplicationContext Context;

    public async Task AuthenticateRequestAsync(HttpRequestMessage request)
    {
        var authResult = await this.Context.TeamsInterop.Authentication.GetAuthenticationResultAsync(this.Context.Context);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
    }
}
}
