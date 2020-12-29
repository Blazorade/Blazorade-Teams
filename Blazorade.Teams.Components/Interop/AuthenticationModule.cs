using Blazorade.Teams.Components.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Components.Interop
{
    public class AuthenticationModule : InteropModuleBase
    {
        public AuthenticationModule(AzureAdApplicationOptions appOptions, IJSRuntime jsRuntime) : base(appOptions, jsRuntime) { }


        internal async Task GetTokenAsync(Context context, Func<AuthenticationResult, Task> successCallback, Func<string, Task> failureCallback)
        {
            var module = await this.GetBlazoradeMsalProxyModuleAsync();
            await module.InvokeVoidAsync("getTokenSilent", new MsalConfig(this.ApplicationSettings), context, CallbackDefinition.Create(successCallback), CallbackDefinition.Create(failureCallback));
        }

    }
}
