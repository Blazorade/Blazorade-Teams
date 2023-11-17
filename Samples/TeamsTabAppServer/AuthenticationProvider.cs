namespace TeamsTabAppServer;

using Blazorade.Msal.Services;
using Blazorade.Teams.Model;
using Microsoft.Graph;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

internal class AuthenticationProvider : IAuthenticationProvider
{
    public AuthenticationProvider(ApplicationContext context, BlazoradeMsalService msalService)
    {
        this.Context = context ?? throw new ArgumentNullException(nameof(context));
        this.MsalService = msalService ?? throw new ArgumentNullException(nameof(msalService));
    }

    private readonly ApplicationContext Context;
    private readonly BlazoradeMsalService MsalService;

    public async Task AuthenticateRequestAsync(HttpRequestMessage request)
    {
        var authResult = await this.MsalService.AcquireTokenSilentAsync(loginHint: this.Context?.Context?.LoginHint, fallbackToDefaultLoginHint: true);
        if (null != authResult)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
        }
    }
}