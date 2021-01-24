using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorade.Core.Components;
using Blazorade.Teams.Configuration;
using Blazorade.Teams.Interop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace TeamsTabAppServer.Pages
{
    partial class AuthStart : BlazoradeComponentBase
    {
        [Inject]
        private BlazoradeTeamsInteropModule teams { get; set; }

        [Inject]
        private IOptions<AzureAdApplicationOptions> options { get; set; }

        [Inject]
        private NavigationManager navigation { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            var uri = navigation.ToAbsoluteUri(navigation.Uri);

            var queryParameters = QueryHelpers.ParseQuery(uri.Query);

            if (!queryParameters.TryGetValue("scopes", out var scopes))
            {
                navigation.NavigateTo("/auth-end?error_description=No+scopes+requested");
            }

            if (!queryParameters.TryGetValue("api", out var api))
            {
                api = "https://graph.microsoft.com";
            }

            var queryParams = new Dictionary<string, string>
                                  {
                                      {"tenant", options.Value.TenantId},
                                      {"client_id", options.Value.ClientId},
                                      {"response_type", "token"},
                                      {"scope", $"{api}/{scopes}"},
                                      {"redirect_uri", navigation.BaseUri + "auth-end"},
                                      {"nonce", Guid.NewGuid().ToString()},
                                  };

            var authoriseUrl = QueryHelpers.AddQueryString($"https://login.microsoftonline.com/{options.Value.TenantId}/oauth2/v2.0/authorize", queryParams);

            navigation.NavigateTo(authoriseUrl);
        }
    }
}