using Blazorade.Teams.Components.Interop;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Components
{
    partial class TeamsApplication
    {

        [JSInvokable]
        public async Task OnAppInitializedAsync()
        {
            await this.TeamsInterop.GetContextAsync(this.OnGotContextAsync);
        }

        [JSInvokable]
        public async Task OnGotContextAsync(Context context)
        {
            await this.TeamsInterop.AppInitialization.NotifyAppLoadedAsync();
            //await this.TeamsInterop.Authentication.GetAuthTokenAsync(new AuthTokenRequest(), this.OnGotAuthTokenSuccessAsync, this.OnGotAuthTokenFailureAsync);
            await this.TeamsInterop.AppInitialization.NotifySuccessAsync();
        }

        [JSInvokable]
        public async Task OnGotAuthTokenSuccessAsync(string token)
        {

        }

        [JSInvokable]
        public async Task OnGotAuthTokenFailureAsync(string reason)
        {
            await this.TeamsInterop.AppInitialization.NotifyFailureAsync(reason, FailedReason.AuthFailed);
        }



        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if(firstRender)
            {
                try
                {
                    await this.TeamsInterop.InitializeAsync(this.OnAppInitializedAsync);
                }
                catch (Exception ex)
                {
                    await this.TeamsInterop.AppInitialization.NotifyFailureAsync(ex.Message, FailedReason.Other);
                }
            }
        }

    }
}
