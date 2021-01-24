using Blazorade.Core.Interop;
using Blazorade.Teams.Components;
using Blazorade.Teams.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Blazorade.Teams.Interop
{
    /// <summary>
    /// The root module for facilitating communications with the Teams JavaScript SDK.
    /// </summary>
    public class BlazoradeTeamsInteropModule : InteropModuleBase
    {
        /// <summary>
        /// Creates a new instance of the module class.
        /// </summary>
        /// <remarks>
        /// This class should not be initialized in code. It will be created automatically through Dependency Injection
        /// and provided through the <see cref="TeamsApplication.TeamsInterop"/> property.
        /// </remarks>
        public BlazoradeTeamsInteropModule(AzureAdApplicationOptions appOptions, IJSRuntime jsRuntime, ApplicationInitializationModule appInitModule, SettingsModule settingsModule, AuthenticationModule authModule) : base(appOptions, jsRuntime)
        {
            this.Authentication = authModule ?? throw new ArgumentNullException(nameof(authModule));
            this.AppInitialization = appInitModule ?? throw new ArgumentNullException(nameof(appInitModule));
            this.Settings = settingsModule ?? throw new ArgumentNullException(nameof(settingsModule));
        }

        /// <summary>
        /// Returns a module that is used for authentication.
        /// </summary>
        public AuthenticationModule Authentication { get; protected set; }

        /// <summary>
        /// A module that represents the <c>appInitialization</c> module in the Teams SDK.
        /// </summary>
        public ApplicationInitializationModule AppInitialization { get; protected set; }

        /// <summary>
        /// A module that represents the <c>settings</c> module in the Teams SDK.
        /// </summary>
        public SettingsModule Settings { get; protected set; }



        /// <summary>
        /// Acquires an access token for the currently logged on user using the application configuration.
        /// </summary>
        /// <remarks>
        /// This method first tries to acquire the token silently. If that fails, then the Teams authentication
        /// dialog is used.
        /// </remarks>
        public async Task<AuthenticationResult> AcquireTokenAsync()
        {
            //var data = await this.GetMsalDataAsync();

            //var module = await this.GetBlazoradeTeamsJSModuleAsync();
            //var handler = new DotNetInstanceCallbackHandler<AuthenticationResult>(module, "acquireToken", data);
            //return await handler.GetResultAsync();
            return null;
        }

        /// <summary>
        /// Returns the access token for the application silently, meaning no user interaction is required.
        /// </summary>
        /// <remarks>
        /// This requires that the token is cached by MSAL from a previous token request.
        /// </remarks>
        /// <returns>Returns the access token or <c>null</c> if it was not available.</returns>
        public async Task<AuthenticationResult> AcquireTokenSilentAsync()
        {
            var data = await this.GetMsalDataAsync();
            var module = await this.GetBlazoradeTeamsJSModuleAsync();
            var handler = new DotNetInstanceCallbackHandler<AuthenticationResult>(module, "msal_acquireTokenSilent", data);
            return await handler.GetResultAsync();
        }

        public async Task<AuthenticationResult> AcquireTokenPopupAsync()
        {
            var data = await this.GetMsalDataAsync();
            var module = await this.GetBlazoradeTeamsJSModuleAsync();
            var handler = new DotNetInstanceCallbackHandler<AuthenticationResult>(module, "acquireTokenPopup", data);
            return await handler.GetResultAsync();
        }


        /// <summary>
        /// Returns the context information loaded from Teams.
        /// </summary>
        /// <remarks>
        /// This method is called for you when you use the <see cref="TeamsApplication"/> component.
        /// </remarks>
        public async Task<Context> GetContextAsync()
        {
            return await new DotNetInstanceCallbackHandler<Context>(await this.GetBlazoradeTeamsJSModuleAsync(), "getContext")
                .GetResultAsync();
        }

        /// <summary>
        /// Initializes your application with Teams.
        /// </summary>
        /// <remarks>
        /// This method is invoked for you when you use the <see cref="TeamsApplication"/> component.
        /// </remarks>
        public async Task InitializeAsync()
        {
            await new DotNetInstanceCallbackHandler(await this.GetBlazoradeTeamsJSModuleAsync(), "initialize")
                .GetResultAsync();
        }

        /// <summary>
        /// Determines whether the application has been loaded inside of Teams.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If this method returns <c>false</c>, all the other methods on this module an its child modules
        /// will also very likely fail.
        /// </para>
        /// <para>
        /// You don't have to worry about this when using the <see cref="TeamsApplication"/> component as the
        /// root component on your pages, and putting your application content in the
        /// <see cref="TeamsApplication.ApplicationTemplate"/> template.
        /// </para>
        /// </remarks>
        public async ValueTask<bool> IsTeamsHostAvailableAsync()
        {
            var module = await this.GetBlazoradeTeamsJSModuleAsync();
            return await module.InvokeAsync<bool>("isTeamsHostAvailable");
        }


        internal async Task MsalLoginRedirectAsync(string loginHint, string state)
        {
            var module = await this.GetBlazoradeTeamsJSModuleAsync();
            var data = new Dictionary<string, object>();
            data["msalConfig"] = new MsalConfig(this.ApplicationSettings);
            data["loginHint"] = loginHint;
            data["state"] = state;

            var result = await module.InvokeAsync<bool>("msal_loginRedirect", data);
        }

        private async Task<Dictionary<string, object>> GetMsalDataAsync()
        {
            return new Dictionary<string, object>()
            {
                { "context", await this.GetContextAsync() },
                { "msalConfig", new MsalConfig(this.ApplicationSettings) }
            };
        }
    }
}
