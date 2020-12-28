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
        public AuthenticationModule(IJSRuntime jsRuntime) : base(jsRuntime) { }


        public async Task GetAuthTokenAsync(AuthTokenRequest tokenRequest, Func<string, Task> successCallback, Func<string, Task> failureCallback)
        {
            this.ValidateCallbackMethod(successCallback.Method);
            this.ValidateCallbackMethod(failureCallback.Method);

            var module = await this.GetBlazoradeTeamsJSModuleAsync();
            await module.InvokeVoidAsync(
                "authentication_getAuthToken",
                tokenRequest,
                CallbackDefinition.Create(successCallback),
                CallbackDefinition.Create(failureCallback)
            );
        }
    }
}
