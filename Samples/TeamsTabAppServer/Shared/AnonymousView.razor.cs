using Blazorade.Msal.Security;
using Blazorade.Msal.Services;
using Blazorade.Teams.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamsTabAppServer.Shared
{
    partial class AnonymousView
    {
        [Parameter]
        public ApplicationContext Context { get; set; }

        private AuthenticationResult Token { get; set; }


        private async Task AuthenticateAsync(MouseEventArgs e)
        {
            this.Token = await this.Context.TeamsInterop.Authentication.AcquireTokenAsync(new TokenAcquisitionRequest
            {
                LoginHint = this.Context.Context.LoginHint,
                Prompt = LoginPrompt.Consent,
                Timeout = 60000
            });
            this.StateHasChanged();
        }

    }
}
