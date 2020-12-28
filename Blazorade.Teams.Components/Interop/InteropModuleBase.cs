using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Components.Interop
{
    public abstract class InteropModuleBase
    {

        protected InteropModuleBase(IJSRuntime jsRuntime)
        {
            this.JSRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        }

        protected IJSRuntime JSRuntime { get; set; }



        private Task<IJSObjectReference> _BlazoradeTeamsJSModule;
        protected async Task<IJSObjectReference> GetBlazoradeTeamsJSModuleAsync()
        {
            var teamsModule = await this.GetTeamsSdkJSModuleAsync();
            return await (_BlazoradeTeamsJSModule ??= this.JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Blazorade.Teams.Components/js/blazoradeTeams.js").AsTask());
        }

        private Task<IJSObjectReference> _TeamsSdkJSModule;
        private protected Task<IJSObjectReference> GetTeamsSdkJSModuleAsync()
        {
            return _TeamsSdkJSModule ??= this.JSRuntime.InvokeAsync<IJSObjectReference>("import", "https://statics.teams.cdn.office.net/sdk/v1.7.0/js/MicrosoftTeams.min.js").AsTask();
        }

    }




}
