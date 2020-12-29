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
        public BlazoradeTeamsInteropModule(AzureAdApplicationOptions appOptions, IJSRuntime jsRuntime, ApplicationInitializationModule appInitModule, AuthenticationModule authModule) : base(appOptions, jsRuntime)
        {
            this.AppInitialization = appInitModule ?? throw new ArgumentNullException(nameof(appInitModule));
            this.Authentication = authModule ?? throw new ArgumentNullException(nameof(authModule));
        }

        public ApplicationInitializationModule AppInitialization { get; protected set; }

        public AuthenticationModule Authentication { get; protected set; }

        public async Task GetContextAsync(Func<Context, Task> callback)
        {
            this.ValidateCallbackMethod(callback.Method);

            var btm = await this.GetBlazoradeTeamsJSModuleAsync();
            await btm.InvokeVoidAsync("getContext", CallbackDefinition.Create(callback));
        }

        public async Task InitializeAsync(Func<Task> callback)
        {
            this.ValidateCallbackMethod(callback.Method);

            var btm = await this.GetBlazoradeTeamsJSModuleAsync();
            await btm.InvokeVoidAsync("initialize", CallbackDefinition.Create(callback));
        }

    }
}
