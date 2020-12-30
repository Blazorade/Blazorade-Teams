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

        [Parameter]
        public RenderFragment HostNotAvailableTemplate { get; set; }

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

        protected bool ShowHostNotAvailableTemplate { get; set; }



        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            
            if(firstRender)
            {
                var isHostAvailable = await this.TeamsInterop.IsTeamsHostAvailableAsync();
                if(isHostAvailable)
                {
                    try
                    {
                        await this.InitializeAsync();
                    }
                    catch (Exception ex)
                    {
                        await this.TeamsInterop.AppInitialization.NotifyFailureAsync(ex.Message, FailedReason.Other);
                    }
                }
                else
                {
                    this.ShowHostNotAvailableTemplate = true;
                    this.StateHasChanged();
                }
            }
        }

        private async Task InitializeAsync()
        {
            await this.TeamsInterop.InitializeAsync();

            await this.TeamsInterop.AppInitialization.NotifyAppLoadedAsync();
            var context = await this.TeamsInterop.GetContextAsync();

            this.ApplicationContext.Context = context;
            this.StateHasChanged();

            if (this.RequireAuthentication)
            {
                var authResult = await this.TeamsInterop.GetTokenAsync(context);
                this.ApplicationContext.AuthResult = authResult;
                await this.TeamsInterop.AppInitialization.NotifySuccessAsync();

                this.ShowApplicationTemplate = true;
                this.StateHasChanged();
            }
            else
            {
                await this.TeamsInterop.AppInitialization.NotifySuccessAsync();
                this.ShowApplicationTemplate = true;
                this.StateHasChanged();
            }
        }
    }
}
