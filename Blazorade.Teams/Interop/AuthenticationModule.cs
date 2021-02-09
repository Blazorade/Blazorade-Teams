using Blazorade.Core.Interop;
using Blazorade.Msal.Security;
using Blazorade.Msal.Services;
using Blazorade.Teams.Configuration;
using Blazorade.Teams.Interop.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blazorade.Teams.Interop
{
    /// <summary>
    /// Represents the authentication module in the Teams SDK.
    /// </summary>
    public class AuthenticationModule : InteropModuleBase
    {
        /// <inheritdoc/>
        public AuthenticationModule(BlazoradeTeamsOptions appOptions, IJSRuntime jsRuntime, NavigationManager navMan, BlazoradeMsalService msalService, LocalStorageService localStorage) : base(appOptions, jsRuntime)
        {
            this.NavMan = navMan ?? throw new ArgumentNullException(nameof(navMan));
            this.MsalService = msalService ?? throw new ArgumentNullException(nameof(msalService));
            this.LocalStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
        }


        private readonly BlazoradeMsalService MsalService;
        private readonly NavigationManager NavMan;
        private readonly LocalStorageService LocalStorage;

        public async Task<AuthenticationResult> AcquireTokenAsync(string loginHint = null, IEnumerable<string> scopes = null)
        {
            AuthenticationResult token = null;

            if(null == scopes)
            {
                scopes = this.GetScopes();
            }
            try
            {
                token = await this.MsalService.AcquireTokenSilentAsync(loginHint: loginHint, scopes: scopes);
            }
            catch { }

            if(null == token)
            {
                token = await this.AuthenticateAsync(loginHint: loginHint, scopes: scopes);
            }

            return token;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(string loginHint = null, IEnumerable<string> scopes = null)
        {
            scopes = scopes ?? this.ApplicationSettings.DefaultScopes;
            var module = await this.GetBlazoradeTeamsJSModuleAsync();

            var requestInfo = new TokenRequestInfo
            {
                LoginHint = loginHint,
                Scopes = new List<string>(scopes)
            };
            await this.LocalStorage.SetItemAsync(TokenRequestInfo.CreateKey(this.ApplicationSettings.ClientId), requestInfo);

            var data = new Dictionary<string, object>
            {
                { "url", this.NavMan.ToAbsoluteUri(this.ApplicationSettings.LoginUrl) }
            };

            string result = null;
            AuthenticationResult token = null;
            using (var handler = new DotNetInstanceCallbackHandler<string>(module, "authentication_authenticate", data))
            {
                result = await handler.GetResultAsync();
            }

            if(result?.Length > 0)
            {
                try
                {
                    token = JsonSerializer.Deserialize<AuthenticationResult>(result);
                }
                catch { }
            }

            if(null == token)
            {
                try
                {
                    token = await this.MsalService.AcquireTokenSilentAsync(fallbackToDefaultLoginHint: true);
                }
                catch { }
            }

            return token;
        }

        public IEnumerable<string> GetScopes(IEnumerable<string> additionalScopes)
        {
            var scopes = new List<string>(additionalScopes ?? new string[0]);
            foreach(var scope in this.ApplicationSettings.DefaultScopes ?? new string[0])
            {
                if (null == scopes.FirstOrDefault(x => x.ToLower() == scope?.ToLower()))
                {
                    scopes.Add(scope);
                }
            }

            return scopes;
        }

        public IEnumerable<string> GetScopes(string additionalScopes = null)
        {
            var arr = (additionalScopes ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries);
            return this.GetScopes(from x in arr select x.Trim());
        }

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
