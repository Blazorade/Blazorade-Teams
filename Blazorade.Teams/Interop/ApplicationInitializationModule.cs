namespace Blazorade.Teams.Interop;

using Blazorade.Teams.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ApplicationInitializationModule : InteropModuleBase
{
    public ApplicationInitializationModule(BlazoradeTeamsOptions appOptions, IJSRuntime jsRuntime) : base(appOptions, jsRuntime) { }





    public Task NotifyAppLoadedAsync()
    {
        return this.GetBlazoradeTeamsJSModuleAsync().ContinueWith(module =>
            {
                module.Result.InvokeVoidAsync("appInitialization_notifyAppLoaded");
            });
    }

    public Task NotifyFailureAsync(FailedRequest failedRequest = null)
    {
        return this.GetBlazoradeTeamsJSModuleAsync().ContinueWith(module =>
            {
                module.Result.InvokeVoidAsync("appInitialization_notifyFailure", failedRequest ?? new FailedRequest());
            });
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

    public Task NotifySuccessAsync()
    {
        return this.GetBlazoradeTeamsJSModuleAsync().ContinueWith(module =>
            {
                module.Result.InvokeVoidAsync("appInitialization_notifySuccess");
            });
    }
}
