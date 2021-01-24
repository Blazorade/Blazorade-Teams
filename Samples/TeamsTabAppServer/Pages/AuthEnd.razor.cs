using System.Threading.Tasks;
using Blazorade.Teams.Interop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace TeamsTabAppServer.Pages
{
    partial class AuthEnd
    {
        [Inject]
        private BlazoradeTeamsInteropModule teams { get; set; }

        [Inject]
        private NavigationManager navigation { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await teams.InitializeAsync();

            var uri = navigation.ToAbsoluteUri(navigation.Uri);

            var parameters = QueryHelpers.ParseQuery(uri.Fragment.Substring(1));

            if (parameters.TryGetValue("access_token", out var accessToken))
            {
                await teams.Authentication.NotifyConsentSuccess(accessToken);
            }
            else
            {
                await teams.Authentication.NotifyConsentFailure(parameters["error_description"]);
            }
        }
    }
}