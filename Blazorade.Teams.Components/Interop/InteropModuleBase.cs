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
            return _BlazoradeTeamsJSModule ??= this.JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Blazorade.Teams.Components/js/blazoradeTeams.js").AsTask();
        }

        private Task<IJSObjectReference> _BlazoradeMsalProxyModule;
        internal Task<IJSObjectReference> GetBlazoradeMsalProxyModuleAsync()
        {
            return _BlazoradeMsalProxyModule ??= this.JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Blazorade.Teams.Components/js/blazoradeMsalProxy.js").AsTask();
        }

        protected void ValidateCallbackMethod(MethodInfo method)
        {
            var attribute = method.GetCustomAttribute<JSInvokableAttribute>();
            if (null == attribute)
            {
                throw new ArgumentException($"The given callback must be a defined method decorate with the '{typeof(JSInvokableAttribute).FullName}' attribute.", nameof(method));
            }
        }

    }




}
