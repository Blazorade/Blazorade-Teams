using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Components.Interop
{
    public class ApplicationInitializationModule : InteropModuleBase
    {
        public ApplicationInitializationModule(IJSRuntime jsRuntime) : base(jsRuntime)
        {
            this.JSRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        }





        public async Task NotifyAppLoadedAsync()
        {
            var m = await this.GetBlazoradeTeamsJSModuleAsync();
            await m.InvokeVoidAsync("appInitialization_notifyAppLoaded");
        }

        public async Task NotifyFailureAsync(FailedRequest failedRequest = null)
        {
            var m = await this.GetBlazoradeTeamsJSModuleAsync();
            await m.InvokeVoidAsync("appInitialization_notifyFailure", failedRequest ?? new FailedRequest());
        }

        public async Task NotifyFailureAsync(string message, FailedReason? reason = null)
        {
            var failedRequest = new FailedRequest
            {
                Message = message, 
                Reason = reason ?? new FailedRequest().Reason
            };

            await this.NotifyFailureAsync(failedRequest);
        }

        public async Task NotifySuccessAsync()
        {
            var m = await this.GetBlazoradeTeamsJSModuleAsync();
            await m.InvokeVoidAsync("appInitialization_notifySuccess");
        }
    }
}
