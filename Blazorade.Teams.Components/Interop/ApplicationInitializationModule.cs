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
        public ApplicationInitializationModule(IJSRuntime jsRuntime) : base(jsRuntime) { }





        public async Task NotifyAppLoadedAsync()
        {
            await this.JSRuntime.InvokeVoidAsync("microsoftTeams.appInitialization.notifyAppLoaded");
        }

        public async Task NotifyFailureAsync(FailedRequest failedRequest = null)
        {
            await this.JSRuntime.InvokeVoidAsync("microsoftTeams.appInitialization.notifyFailure", failedRequest ?? new FailedRequest());
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
            await this.JSRuntime.InvokeVoidAsync("microsoftTeams.appInitialization.notifySuccess");
        }
    }
}
