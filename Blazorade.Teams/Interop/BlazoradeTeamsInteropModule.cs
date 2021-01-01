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
        public BlazoradeTeamsInteropModule(AzureAdApplicationOptions appOptions, IJSRuntime jsRuntime, ApplicationInitializationModule appInitModule, SettingsModule settingsModule) : base(appOptions, jsRuntime)
        {
            this.AppInitialization = appInitModule ?? throw new ArgumentNullException(nameof(appInitModule));
            this.Settings = settingsModule ?? throw new ArgumentNullException(nameof(settingsModule));
        }

        /// <summary>
        /// A module that represents the <c>appInitialization</c> module in the Teams SDK.
        /// </summary>
        public ApplicationInitializationModule AppInitialization { get; protected set; }

        /// <summary>
        /// A module that represents the <c>settings</c> module in the Teams SDK.
        /// </summary>
        public SettingsModule Settings { get; protected set; }


        /// <summary>
        /// Returns the context information loaded from Teams.
        /// </summary>
        /// <remarks>
        /// This method is called for you when you use the <see cref="TeamsApplication"/> component.
        /// </remarks>
        public async Task<Context> GetContextAsync()
        {
            return await new CallbackProxy<Context>(await this.GetBlazoradeTeamsJSModuleAsync())
                .GetResultAsync("getContext");
        }

        /// <summary>
        /// Initializes your application with Teams.
        /// </summary>
        /// <remarks>
        /// This method is invoked for you when you use the <see cref="TeamsApplication"/> component.
        /// </remarks>
        public async Task InitializeAsync()
        {
            await new CallbackProxy(await this.GetBlazoradeTeamsJSModuleAsync())
                .GetResultAsync("initialize");
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

        /// <summary>
        /// Uses MSAL.js to get an access token for the logged in user.
        /// </summary>
        /// <param name="context">The Teams context to use when resolving the token.</param>
        /// <remarks>
        /// The access token returned by this method defines the scopes that have been defined on your Azure AD
        /// application, which you configure in your application's startup class by using the
        /// <see cref="BlazoradeTeamsServiceCollectionExtensionMethods.AddBlazoradeTeams"/> methods.
        /// </remarks>
        public async Task<AuthenticationResult> GetTokenAsync(Context context)
        {
            var module = await this.GetBlazoradeMsalModuleAsync();
            return await new CallbackProxy<AuthenticationResult>(await this.GetBlazoradeMsalModuleAsync())
                .GetResultAsync(
                    "getTokenSilent",
                    args: new Dictionary<string, object>
                    {
                        { "context", context },
                        { "config", new MsalConfig(this.ApplicationSettings) }
                    }
                );
        }
    }
}
