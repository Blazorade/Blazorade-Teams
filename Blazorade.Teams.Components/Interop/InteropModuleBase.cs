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

        protected InteropModuleBase(IJSRuntime jsRuntime)
        {
            this.JSRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        }

        protected IJSRuntime JSRuntime { get; set; }



        private Task<IJSObjectReference> _BlazoradeTeamsJSModule;
        protected async Task<IJSObjectReference> GetBlazoradeTeamsJSModuleAsync()
        {
            return await (_BlazoradeTeamsJSModule ??= this.JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Blazorade.Teams.Components/js/blazoradeTeams.js").AsTask());
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
