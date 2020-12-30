using Blazorade.Teams.Components.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Components.Interop
{
    public abstract class InteropModuleBase
    {

        protected InteropModuleBase(AzureAdApplicationOptions appOptions, IJSRuntime jsRuntime)
        {
            this.ApplicationSettings = appOptions ?? throw new ArgumentNullException(nameof(appOptions));
            this.JSRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        }

        protected IJSRuntime JSRuntime { get; private set; }

        protected AzureAdApplicationOptions ApplicationSettings { get; private set; }


        private Task<IJSObjectReference> _BlazoradeTeamsJSModule;
        protected Task<IJSObjectReference> GetBlazoradeTeamsJSModuleAsync()
        {
            return this.GetTeamsSdkModuleAsync()
                .ContinueWith(state =>
                {
                    return (_BlazoradeTeamsJSModule ??= this.JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Blazorade.Teams.Components/js/blazoradeTeams.js").AsTask()).Result;
                });
        }

        private Task<IJSObjectReference> _BlazoradeMsalModule;
        internal Task<IJSObjectReference> GetBlazoradeMsalModuleAsync()
        {
            return this.GetMsalModuleAsync()
                .ContinueWith(state =>
                {
                    return (_BlazoradeMsalModule ??= this.JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Blazorade.Teams.Components/js/blazoradeMsal.js").AsTask()).Result;
                });
        }

        protected void ValidateCallbackMethod(MethodInfo method)
        {
            var attribute = method.GetCustomAttribute<JSInvokableAttribute>();
            if (null == attribute)
            {
                throw new ArgumentException($"The given callback must be a defined method decorate with the '{typeof(JSInvokableAttribute).FullName}' attribute.", nameof(method));
            }
        }


        private Task<IJSObjectReference> _MsalModule;
        private Task<IJSObjectReference> GetMsalModuleAsync()
        {
            return _MsalModule ??= this.JSRuntime.InvokeAsync<IJSObjectReference>("import", "https://alcdn.msftauth.net/browser/2.8.0/js/msal-browser.min.js").AsTask();
        }

        private Task<IJSObjectReference> _TeamsSdkModule;
        private Task<IJSObjectReference> GetTeamsSdkModuleAsync()
        {
            return _TeamsSdkModule ??= this.JSRuntime.InvokeAsync<IJSObjectReference>("import", "https://statics.teams.cdn.office.net/sdk/v1.7.0/js/MicrosoftTeams.min.js").AsTask();
        }
    }




}
