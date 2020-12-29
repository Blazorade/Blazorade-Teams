using Blazorade.Teams.Components.Interop;
using Blazorade.Teams.Components.Model;
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
        public TeamsApplication()
        {
            this.ApplicationContext = new ApplicationContext { };
        }


        [Parameter]
        public bool RequireAuthentication { get; set; }

        [Parameter]
        public RenderFragment<ApplicationContext> ApplicationTemplate { get; set; }

        private ApplicationContext _ApplicationContext;
        public ApplicationContext ApplicationContext
        {
            get => _ApplicationContext;
            set
            {
                if (null == value) throw new ArgumentNullException();
                _ApplicationContext = value;
            }
        }


        protected bool ShowApplicationTemplate { get; set; }

        [JSInvokable]
        public async Task OnAppInitializedAsync()
        {
            await this.TeamsInterop.AppInitialization.NotifyAppLoadedAsync();
            await this.TeamsInterop.GetContextAsync(this.OnGotContextAsync);
        }

        [JSInvokable]
        public async Task OnAuthResultSuccessAsync(AuthenticationResult authResult)
        {
            this.ApplicationContext.AuthResult = authResult;
            await this.TeamsInterop.AppInitialization.NotifySuccessAsync();

            this.ShowApplicationTemplate = true;
            this.StateHasChanged();
        }

        [JSInvokable]
        public async Task OnAuthResultFailureAsync(string reason)
        {
            this.ApplicationContext.AuthResult = new AuthenticationResult
            {
                IsFailed = true,
                FailureReason = reason
            };

            await this.TeamsInterop.AppInitialization.NotifyFailureAsync(reason, FailedReason.AuthFailed);
        }

        [JSInvokable]
        public async Task OnGotContextAsync(Context context)
        {
            this.ApplicationContext.Context = context;
            this.StateHasChanged();

            if(this.RequireAuthentication)
            {
                await this.TeamsInterop.Authentication.GetTokenAsync(context, this.OnAuthResultSuccessAsync, this.OnAuthResultFailureAsync);
            }
            else
            {
                await this.TeamsInterop.AppInitialization.NotifySuccessAsync();
                this.ShowApplicationTemplate = true;
                this.StateHasChanged();
            }
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
