using Blazorade.Core.Interop;
using Blazorade.Teams.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Interop
{
    /// <summary>
    /// Represents the authentication module in the Teams SDK.
    /// </summary>
    public class AuthenticationModule : InteropModuleBase
    {
        /// <inheritdoc/>
        public AuthenticationModule(AzureAdApplicationOptions appOptions, IJSRuntime jsRuntime) : base(appOptions, jsRuntime) { }



        /// <summary>
        /// Notifies the frame that initiated this authentication request that the request failed. This function is
        /// usable only on the authentication window. This call causes the authentication window to be closed.
        /// </summary>
        /// <param name="reason">Failure reason.</param>
        /// <param name="callbackUrl">Specifies the url to redirect back to if the client is Win32 Outlook.</param>
        /// <returns></returns>
        public async Task NotifyFailureAsync(string reason = null, string callbackUrl = null)
        {
            var module = await this.GetBlazoradeTeamsJSModuleAsync();
            await module.InvokeVoidAsync("authentication_notifyFailure", reason, callbackUrl);
        }

        /// <summary>
        /// Notifies the frame that initiated this authentication request that the request was successful. This function 
        /// is usable only on the authentication window. This call causes the authentication window to be closed.
        /// </summary>
        /// <param name="result">
        /// Specifies a result for the authentication. If specified, the frame that initiated the authentication
        /// pop-up receives this value in its callback.
        /// </param>
        /// <param name="callbackUrl">Specifies the url to redirect back to if the client is Win32 Outlook.</param>
        /// <returns></returns>
        public async Task NotifySuccessAsync(string result = null, string callbackUrl = null)
        {
            var module = await this.GetBlazoradeTeamsJSModuleAsync();
            await module.InvokeVoidAsync("authentication_notifySuccess", result, callbackUrl);
        }
    }
}
