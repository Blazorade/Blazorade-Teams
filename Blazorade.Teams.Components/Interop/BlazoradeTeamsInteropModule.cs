using Blazorade.Teams.Components.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Components.Interop
{
    public class BlazoradeTeamsInteropModule : InteropModuleBase
    {
        public BlazoradeTeamsInteropModule(AzureAdApplicationOptions appOptions, IJSRuntime jsRuntime, ApplicationInitializationModule appInitModule) : base(appOptions, jsRuntime)
        {
            this.AppInitialization = appInitModule ?? throw new ArgumentNullException(nameof(appInitModule));
        }

        public ApplicationInitializationModule AppInitialization { get; protected set; }

        public async Task<Context> GetContextAsync()
        {
            return await new CallbackProxy<Context>(await this.GetBlazoradeTeamsJSModuleAsync())
                .GetResultAsync("getContext");
        }

        public async Task InitializeAsync(Func<Task> callback)
        {
            this.ValidateCallbackMethod(callback.Method);

            var btm = await this.GetBlazoradeTeamsJSModuleAsync();
            await btm.InvokeVoidAsync("initialize", CallbackDefinition.Create(callback));
        }

        public async Task InitializeAsync()
        {
            await new CallbackProxy(await this.GetBlazoradeTeamsJSModuleAsync())
                .GetResultAsync("initialize");
        }

        public async ValueTask<bool> IsTeamsHostAvailableAsync()
        {
            var module = await this.GetBlazoradeTeamsJSModuleAsync();
            return await module.InvokeAsync<bool>("isTeamsHostAvailable");
        }


        internal async Task<AuthenticationResult> GetTokenAsync(Context context)
        {
            var module = await this.GetBlazoradeMsalModuleAsync();
            return await new CallbackProxy<AuthenticationResult>(await this.GetBlazoradeMsalModuleAsync())
                .GetResultAsync(
                    "getTokenSilent",
                    args: new Dictionary<string, object>
                    {
                        { "context", context },
                        { "config", new MsalConfig(this.ApplicationSettings) }
                    }
                );
        }
    }
}
