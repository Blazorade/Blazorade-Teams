using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Components.Interop
{
    public class BlazoradeTeamsInteropModule : InteropModuleBase
    {
        public BlazoradeTeamsInteropModule(IJSRuntime jsRuntime, ApplicationInitializationModule appInitModule) : base(jsRuntime)
        {
            this.AppInitialization = appInitModule ?? throw new ArgumentNullException(nameof(appInitModule));
        }

        public ApplicationInitializationModule AppInitialization { get; protected set; }

        public async Task GetContextAsync(Func<Context, Task> callback)
        {
            var btm = await this.GetBlazoradeTeamsJSModuleAsync();
            await btm.InvokeVoidAsync("getContext", CallbackDefinition.Create(callback.Target, callback.Method.Name));
        }

        public async Task InitializeAsync(Func<Task> callback)
        {
            var btm = await this.GetBlazoradeTeamsJSModuleAsync();
            await btm.InvokeVoidAsync("initialize", CallbackDefinition.Create(callback.Target, callback.Method.Name));
        }


    }
}
